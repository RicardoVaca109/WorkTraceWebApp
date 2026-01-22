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
using Newtonsoft.Json.Linq;

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
    private readonly ITakenRequirementApiService _takenRequirementApiService;
    private readonly IEvaluationDashboardApiService _dashboardApiService;
    private readonly IAssignmentEvaluationApiService _assignmentEvaluationApiService;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public HomeController(
        ILogger<HomeController> logger,
        IAssignmentApiService assignmentApiService,
        IUserApiService userApiService,
        IClientApiService clientApiService,
        IServiceApiService serviceApiService,
        IStatusApiService statusApiService,
        IFormTemplateApiService formTemplateApiService,
        ITakenRequirementApiService takenRequirementApiService,
        IEvaluationDashboardApiService dashboardApiService,
        IAssignmentEvaluationApiService assignmentEvaluationApiService,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _assignmentApiService = assignmentApiService;
        _userApiService = userApiService;
        _clientApiService = clientApiService;
        _serviceApiService = serviceApiService;
        _statusApiService = statusApiService;
        _formTemplateApiService = formTemplateApiService;
        _takenRequirementApiService = takenRequirementApiService;
        _dashboardApiService = dashboardApiService;
        _assignmentEvaluationApiService = assignmentEvaluationApiService;
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
            var usersTask = _userApiService.GetAllAsync();
            var templatesTask = _formTemplateApiService.GetAllAsync();

            await Task.WhenAll(usersTask, templatesTask);

            var users = usersTask.Result;
            var templates = templatesTask.Result;

            var targetUsers = users?
                .Where(u => u.Role == Shared.UserRoles.Técnico || u.Role == Shared.UserRoles.Vendedor)
                .OrderBy(u => u.FullName)
                .ToList() ?? new List<UserInformationResponse>();
            
            var viewModel = new WorkTrace.WebApp.Models.ViewModels.DashboardViewModel
            {
                Users = targetUsers,
                Templates = templates ?? new List<WorkTrace.WebApp.Models.Dtos.FormTemplate.FormTemplateResponse>()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data for dashboard view");
            TempData["Error"] = "Error al cargar los datos de la vista.";
            return View(new WorkTrace.WebApp.Models.ViewModels.DashboardViewModel());
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboardData(string userId, DateTime start, DateTime end)
    {
        try
        {
            var data = await _dashboardApiService.GetDashboardAsync(userId, start, end);
            return Json(new { success = true, data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching dashboard data");
            return Json(new { success = false, message = "Error al obtener datos del tablero." });
        }
    }

    public IActionResult Inventario()
    {
        ViewBag.Token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");
        return View();
    }

    public IActionResult ImportarSheyla()
    {
        ViewBag.Token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");
        return View();
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
            var templatesTask = _formTemplateApiService.GetAllAsync();

            await Task.WhenAll(assignmentsTask, usersTask, clientsTask, servicesTask, statusesTask, templatesTask);

            var assignments = assignmentsTask.Result ?? new List<AssignmentResponse>();
            var allUsers = usersTask.Result ?? new List<UserInformationResponse>();
            var clients = clientsTask.Result ?? new List<ClientInformationResponse>();
            var services = servicesTask.Result ?? new List<ServiceInformationResponse>();
            var statuses = statusesTask.Result ?? new List<StatusInformationResponse>();
            var templates = templatesTask.Result ?? new List<WorkTrace.WebApp.Models.Dtos.FormTemplate.FormTemplateResponse>();

            var userDict = allUsers.ToDictionary(u => u.Id, u => u.FullName);
            var clientDict = clients.ToDictionary(c => c.Id, c => c.FullName);
            var serviceDict = services.ToDictionary(s => s.Id, s => s.Name);
            var statusDict = statuses.ToDictionary(s => s.Id, s => s.Name);
            var activeTemplates = templates.Where(t => t.IsActive).ToList();
            var templateDict = activeTemplates.ToDictionary(t => t.Id, t => t.Name);

            var calendarEvents = assignments.Select(a => {
                var formIds = ParseAssignedForms(a.AssignedForms);
                return new
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
                        createdByUser = a.CreatedByUser,
                        assignedTemplates = formIds.Select(tid => templateDict.GetValueOrDefault(tid, "Plantilla Desconocida")).ToList(),
                        assignedTemplateIds = formIds
                    }
                };
            }).ToList();

            var activeUsers = allUsers.Where(u => u.IsActive).ToList();

            var viewModel = new CalendarioActividadesViewModel
            {
                EventsJson = JsonSerializer.Serialize(calendarEvents, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                Users = activeUsers,
                Clients = clients,
                Services = services,
                Statuses = statuses,
                FormTemplates = activeTemplates,
                LoggedInUserId = GetUserIdFromSession(),
                UsersJson = JsonSerializer.Serialize(userDict, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                ClientsJson = JsonSerializer.Serialize(clientDict, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                ServicesJson = JsonSerializer.Serialize(serviceDict, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                StatusesJson = JsonSerializer.Serialize(statusDict, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                FormTemplatesJson = JsonSerializer.Serialize(templateDict, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
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

    public async Task<IActionResult> Requerimientos()
    {
        var requirementsTask = _takenRequirementApiService.GetAllAsync();
        var clientsTask = _clientApiService.GetAllAsync();

        await Task.WhenAll(requirementsTask, clientsTask);

        var requirements = requirementsTask.Result ?? new List<WorkTrace.WebApp.Models.Dtos.TakenRequirement.TakenRequirementResponse>();
        var clients = clientsTask.Result ?? new List<ClientInformationResponse>();
        
        ViewBag.Clients = clients;
        ViewBag.CurrentUserId = GetUserIdFromSession();

        return View(requirements);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTakenRequirement([FromBody] WorkTrace.WebApp.Models.Dtos.TakenRequirement.CreateTakenRequirementRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
        }

        try
        {
            var result = await _takenRequirementApiService.CreateAsync(request);
            if (result == null)
            {
                return Json(new { success = false, message = "La API no devolvió un requerimiento creado." });
            }
            return Json(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating requirement");
            return Json(new { success = false, message = "No se pudo crear el requerimiento." });
        }
    }

    // Update Action Injected
    [HttpPut]
    [Route("TakenRequirements/Update")]
    public async Task<IActionResult> UpdateTakenRequirement([FromBody] WorkTrace.WebApp.Models.Dtos.TakenRequirement.UpdateTakenRequirementRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
        }

        try
        {
            var result = await _takenRequirementApiService.UpdateAsync(request.Id, request);
            if (result == null)
            {
                return Json(new { success = false, message = "La API no devolvió un resultado exitoso." });
            }
            return Json(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating requirement");
            return Json(new { success = false, message = "No se pudo actualizar el requerimiento." });
        }
    }

    [HttpGet("TakenRequirements/GetByDateAll")]
    public async Task<IActionResult> GetTakenRequirementsByDate(DateTime start, DateTime end)
    {
        try
        {
            var result = await _takenRequirementApiService.GetByDateAllAsync(start, end);
            return Json(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting requirements by date");
            return StatusCode(500, "Error interno del servidor.");
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
            return Json(new { 
                success = true, 
                data = new {
                    id = result.Id,
                    users = result.Users,
                    service = result.Service,
                    client = result.Client,
                    status = result.Status,
                    assignedDate = result.AssignedDate,
                    address = result.Address,
                    destinationLocation = result.DestinationLocation,
                    createdByUser = result.CreatedByUser,
                    checkIn = result.CheckIn,
                    checkOut = result.CheckOut,
                    assignedTemplateIds = ParseAssignedForms(result.AssignedForms)
                }
            });
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
            // Fetch current state to calculate delta
            var currentAssignment = await _assignmentApiService.GetByIdAsync(id);
            if (currentAssignment != null && request.AssignedForms != null)
            {
                var currentFormIds = currentAssignment.AssignedForms?.Select(x => x.Id).ToList() ?? new List<string>();
                var newFormIds = request.AssignedForms;

                request.AddForms = newFormIds.Except(currentFormIds).ToList();
                request.RemoveForms = currentFormIds.Except(newFormIds).ToList();
            }

            var updateResult = await _assignmentApiService.UpdateAsync(id, request);
            if (updateResult == null)
            {
                return Json(new { success = false, message = "La API no devolvió un resultado exitoso." });
            }

            // Re-fetch to ensure all relationships/IDs are fresh
            var result = await _assignmentApiService.GetByIdAsync(id);
            if (result == null) result = updateResult;

            return Json(new { 
                success = true, 
                data = new {
                    id = result.Id,
                    users = result.Users,
                    service = result.Service,
                    client = result.Client,
                    status = result.Status,
                    assignedDate = result.AssignedDate,
                    address = result.Address,
                    destinationLocation = result.DestinationLocation,
                    createdByUser = result.CreatedByUser,
                    checkIn = result.CheckIn,
                    checkOut = result.CheckOut,
                    assignedTemplateIds = ParseAssignedForms(result.AssignedForms)
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating assignment");
            var errorMessage = ErrorParser.Parse(ex.Message);
            return Json(new { success = false, message = "No se pudo actualizar la asignación: \n" + errorMessage });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAssignmentEvaluationDetail(string assignmentId)
    {
        try
        {
            var detail = await _assignmentEvaluationApiService.GetEvaluationDetailByAssignmentAsync(assignmentId);
            if (detail == null)
            {
                // It's possible there is no evaluation yet. Return null or specific status.
                // Returning null here so the frontend can handle "no evaluation found" gracefully.
                return Json(null);
            }
            return Json(new { success = true, data = detail });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting assignment evaluation detail for {AssignmentId}", assignmentId);
            return StatusCode(500, "Error al obtener los detalles de la evaluación.");
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
            return Json(new {
                id = assignment.Id,
                users = assignment.Users,
                service = assignment.Service,
                client = assignment.Client,
                status = assignment.Status,
                assignedDate = assignment.AssignedDate,
                address = assignment.Address,
                destinationLocation = assignment.DestinationLocation,
                createdByUser = assignment.CreatedByUser,
                checkIn = assignment.CheckIn,
                checkOut = assignment.CheckOut,
                assignedTemplateIds = ParseAssignedForms(assignment.AssignedForms)
            });
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

    private List<string> ParseAssignedForms(object? assignedFormsData)
    {
        if (assignedFormsData == null) return new List<string>();

        var result = new List<string>();

        try
        {
            if (assignedFormsData is JsonElement element)
            {
                if (element.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in element.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.String)
                        {
                            result.Add(item.GetString() ?? "");
                        }
                        else if (item.ValueKind == JsonValueKind.Object)
                        {
                            // Try "Id", "id", "ID"
                            if (item.TryGetProperty("Id", out var idProp) || item.TryGetProperty("id", out idProp) || item.TryGetProperty("ID", out idProp))
                            {
                                result.Add(idProp.GetString() ?? "");
                            }
                        }
                    }
                }
            }
            else if (assignedFormsData is JArray jArray)
            {
                foreach (var item in jArray)
                {
                    if (item.Type == JTokenType.String)
                    {
                        result.Add(item.ToString());
                    }
                    else if (item.Type == JTokenType.Object)
                    {
                        var id = item["Id"]?.ToString() ?? item["id"]?.ToString() ?? item["ID"]?.ToString();
                        if (id != null) result.Add(id);
                    }
                }
            }
            else if (assignedFormsData is IEnumerable<string> strList)
            {
                result.AddRange(strList);
            }
            else if (assignedFormsData is IEnumerable<AssignedFormResponse> objList)
            {
                result.AddRange(objList.Select(x => x.Id));
            }
            // Fallback for when the object is deserialized but structure is unknown (e.g. List<object>)
            else 
            {
                 // Attempt serialization/deserialization as a safe fallback
                 try 
                 {
                     var json = JsonSerializer.Serialize(assignedFormsData);
                     using (var doc = JsonDocument.Parse(json))
                     {
                         if(doc.RootElement.ValueKind == JsonValueKind.Array)
                         {
                             foreach(var item in doc.RootElement.EnumerateArray())
                             {
                                if (item.ValueKind == JsonValueKind.String)
                                    result.Add(item.GetString() ?? "");
                                else if (item.ValueKind == JsonValueKind.Object)
                                {
                                     if (item.TryGetProperty("Id", out var idProp) || item.TryGetProperty("id", out idProp))
                                        result.Add(idProp.GetString() ?? "");
                                }
                             }
                         }
                     }
                 } 
                 catch {}
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing assigned forms");
        }

        return result;
    }
}
