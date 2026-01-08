using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using WorkTrace.WebApp.Filters;
using WorkTrace.WebApp.Models;
using WorkTrace.WebApp.Models.Dtos;
using WorkTrace.WebApp.Models.Dtos.Clients;
using WorkTrace.WebApp.Models.Dtos.Service;
using WorkTrace.WebApp.Models.Dtos.Status;
using WorkTrace.WebApp.Models.Dtos.Users;
using WorkTrace.WebApp.Services.Interfaces;
using WorkTrace.WebApp.Helpers;

namespace WorkTrace.WebApp.Controllers;

[AuthorizeSession]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAssignmentApiService _assignmentApiService;
    private readonly IUserApiService _userApiService;
    private readonly IClientApiService _clientApiService;
    private readonly IServiceApiService _serviceApiService;
    private readonly IStatusApiService _statusApiService;
    private readonly IFormTemplateApiService _formTemplateApiService;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public HomeController(
        ILogger<HomeController> logger,
        IAssignmentApiService assignmentApiService,
        IUserApiService userApiService,
        IClientApiService clientApiService,
        IServiceApiService serviceApiService,
        IStatusApiService statusApiService,
        IFormTemplateApiService formTemplateApiService,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _assignmentApiService = assignmentApiService;
        _userApiService = userApiService;
        _clientApiService = clientApiService;
        _serviceApiService = serviceApiService;
        _statusApiService = statusApiService;
        _formTemplateApiService = formTemplateApiService;
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Clientes()
    {
        return View();
    }

    public async Task<IActionResult> Estadistica()
    {
        try 
        {
            var templates = await _formTemplateApiService.GetAllAsync() ?? new List<WorkTrace.WebApp.Models.Dtos.FormTemplate.FormTemplateResponse>();
            return View(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching form templates");
            TempData["Error"] = "Error al cargar las plantillas de formulario.";
            return View(new List<WorkTrace.WebApp.Models.Dtos.FormTemplate.FormTemplateResponse>());
        }
    }

    private string? GetUserIdFromSession()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");
        if (string.IsNullOrEmpty(token)) return null;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        return jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }

    public async Task<IActionResult> CalendarioActividades()
    {
        try
        {
            var assignmentsTask = _assignmentApiService.GetAllAsync();
            var usersTask = _userApiService.GetAllAsync();
            var clientsTask = _clientApiService.GetAllAsync();
            var servicesTask = _serviceApiService.GetAllAsync();
            var statusesTask = _statusApiService.GetAllAsync();

            await Task.WhenAll(assignmentsTask, usersTask, clientsTask, servicesTask, statusesTask);

            var assignments = assignmentsTask.Result ?? new List<AssignmentResponse>();
            var allUsers = usersTask.Result ?? new List<UserInformationResponse>();
            var clients = clientsTask.Result ?? new List<ClientInformationResponse>();
            var services = servicesTask.Result ?? new List<ServiceInformationResponse>();
            var statuses = statusesTask.Result ?? new List<StatusInformationResponse>();

            var userDict = allUsers.ToDictionary(u => u.Id, u => u.FullName);
            var clientDict = clients.ToDictionary(c => c.Id, c => c.FullName);
            var serviceDict = services.ToDictionary(s => s.Id, s => s.Name);
            var statusDict = statuses.ToDictionary(s => s.Id, s => s.Name);

            var calendarEvents = assignments.Select(a => new
            {
                id = a.Id,
                title = statusDict.GetValueOrDefault(a.Status, "Estado Desconocido"),
                start = a.AssignedDate,
                extendedProps = new
                {
                    client = clientDict.GetValueOrDefault(a.Client, "Cliente Desconocido"),
                    service = serviceDict.GetValueOrDefault(a.Service, "Servicio Desconocido"),
                    users = a.Users.Select(userId => userDict.GetValueOrDefault(userId, "Usuario Desconocido")).ToList(),
                    address = a.Address,
                    status = statusDict.GetValueOrDefault(a.Status, "Estado Desconocido"),
                    checkIn = a.CheckIn,
                    checkOut = a.CheckOut,
                    createdByUser = a.CreatedByUser
                }
            }).ToList();

            var activeUsers = allUsers.Where(u => u.IsActive).ToList();

            var viewModel = new CalendarioActividadesViewModel
            {
                EventsJson = JsonSerializer.Serialize(calendarEvents, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                Users = activeUsers,
                Clients = clients,
                Services = services,
                Statuses = statuses,
                LoggedInUserId = GetUserIdFromSession(),
                UsersJson = JsonSerializer.Serialize(userDict, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                ClientsJson = JsonSerializer.Serialize(clientDict, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                ServicesJson = JsonSerializer.Serialize(serviceDict, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                StatusesJson = JsonSerializer.Serialize(statusDict, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading data for CalendarioActividades");
            TempData["Error"] = "Error al cargar los datos del calendario.";
            return View(new CalendarioActividadesViewModel { EventsJson = "[]" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAssignment([FromBody] CreateAssignmentRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
        }

        try
        {
            var loggedInUserId = GetUserIdFromSession();
            if (string.IsNullOrEmpty(loggedInUserId))
            {
                return Json(new { success = false, message = "Usuario no autenticado." });
            }

            request.CreatedByUser = loggedInUserId;

            var result = await _assignmentApiService.CreateAsync(request);
            if (result == null)
            {
                return Json(new { success = false, message = "La API no devolvió una asignación creada." });
            }
            return Json(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating assignment");
            var errorMessage = ErrorParser.Parse(ex.Message);
            return Json(new { success = false, message = "No se pudo crear la asignación: \n" + errorMessage });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAssignment(string id, [FromBody] UpdateAssignmentRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
        }

        try
        {
            var result = await _assignmentApiService.UpdateAsync(id, request);
            if (result == null)
            {
                return Json(new { success = false, message = "La API no devolvió un resultado exitoso." });
            }
            return Json(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating assignment");
            var errorMessage = ErrorParser.Parse(ex.Message);
            return Json(new { success = false, message = "No se pudo actualizar la asignación: \n" + errorMessage });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAssignment(string id)
    {
        try
        {
            var assignment = await _assignmentApiService.GetByIdAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }
            return Json(assignment);
        }
        catch
        {
            return StatusCode(500, "Error al obtener los datos de la asignación.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Seguimiento(double? latitude, double? longitude)
    {
        var users = await _userApiService.GetAllAsync();
        var filteredUsers = users?.Where(u => u.Role == Shared.UserRoles.Técnico || u.Role == Shared.UserRoles.Vendedor).ToList() ?? new List<UserInformationResponse>();

        ViewBag.Users = filteredUsers;
        ViewBag.Latitude = latitude;
        ViewBag.Longitude = longitude;
        return View();
    }



    public async Task<IActionResult> Servicios()
    {
        var services = await _serviceApiService.GetAllAsync() ?? new List<ServiceInformationResponse>();
        var model = new ServiceViewModel
        {
            Services = services,
            NewService = new CreateServiceRequest()
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateService([Bind(Prefix = "NewService")] CreateServiceRequest request)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid data submitted.";
            return RedirectToAction("Servicios");
        }

        try
        {
            var result = await _serviceApiService.CreateAsync(request);
            if (result != null)
            {
                TempData["Success"] = "Service created successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to create the service.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating service");
            var errorMessage = ErrorParser.Parse(ex.Message);
            TempData["Error"] = $"Ocurrió un error: {errorMessage}";
        }

        return RedirectToAction("Servicios");
    }

    [HttpGet]
    public async Task<IActionResult> GetService(string id)
    {
        try
        {
            var service = await _serviceApiService.GetByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return Json(service);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting service by id {Id}", id);
            return StatusCode(500, "Error interno del servidor al obtener el servicio.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateService(string id, [FromBody] UpdateServiceRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
        }

        try
        {
            var result = await _serviceApiService.UpdateAsync(id, request);
            if (result != null)
            {
                return Json(new { success = true, data = result });
            }
            else
            {
                return Json(new { success = false, message = "La API no devolvió un resultado exitoso." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating service");
            var errorMessage = ErrorParser.Parse(ex.Message);
            return Json(new { success = false, message = "No se pudo actualizar el servicio: \n" + errorMessage });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateStatus([FromBody] CreateStatusRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
        }

        try
        {
            var result = await _statusApiService.CreateAsync(request);
            if (result == null)
            {
                return Json(new { success = false, message = "La API no devolvió un estado creado." });
            }
            return Json(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating status");
            var errorMessage = ErrorParser.Parse(ex.Message);
            return Json(new { success = false, message = "No se pudo crear el estado: \n" + errorMessage });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
