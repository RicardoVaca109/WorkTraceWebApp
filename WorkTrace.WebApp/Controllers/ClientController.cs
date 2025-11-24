using Microsoft.AspNetCore.Mvc;
using WorkTrace.WebApp.Filters;
using WorkTrace.WebApp.Models.Dtos;
using WorkTrace.WebApp.Models.Dtos.Clients;
using WorkTrace.WebApp.Services.Interfaces;
using WorkTrace.WebApp.Helpers;

namespace WorkTrace.WebApp.Controllers
{
    [AuthorizeSession]
    public class ClientController : Controller
    {
        private readonly IClientApiService _clientApiService;
        private readonly IAssignmentApiService _assignmentApiService;

        public ClientController(IClientApiService clientApiService, IAssignmentApiService assignmentApiService)
        {
            _clientApiService = clientApiService;
            _assignmentApiService = assignmentApiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var clients = await _clientApiService.GetAllAsync() ?? new List<ClientInformationResponse>();
                return View(clients);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener la lista de clientes: {ex.Message}";
                return View(new List<ClientInformationResponse>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClientRequest model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
            }

            try
            {
                var createdClient = await _clientApiService.CreateAsync(model);
                if (createdClient == null)
                {
                    return Json(new { success = false, message = "La API no devolvió un cliente creado." });
                }

                return Json(new { success = true, message = "Cliente creado exitosamente." });
            }
            catch (Exception ex)
            {
                var errorMessage = ErrorParser.Parse(ex.Message);
                return Json(new { success = false, message = "No se pudo crear el cliente: \n" + errorMessage });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, UpdateClientRequest model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
            }

            try
            {
                var result = await _clientApiService.UpdateAsync(id, model);
                if (result == null)
                {
                    return Json(new { success = false, message = "La API no devolvió un resultado exitoso." });
                }
                return Json(new { success = true, message = "Cliente actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                var errorMessage = ErrorParser.Parse(ex.Message);
                return Json(new { success = false, message = "No se pudo actualizar el cliente: \n" + errorMessage });
            }
        }

        public IActionResult GetEditClientForm([FromQuery] UpdateClientRequest model)
        {
            return PartialView("_EditClientForm", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var client = await _clientApiService.GetByIdAsync(id);
                if (client == null)
                {
                    return NotFound();
                }
                return Json(client);
            }
            catch
            {
                return StatusCode(500, "Error al obtener los datos del cliente.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetClientHistory(string id)
        {
            try
            {
                var history = await _assignmentApiService.GetClientHistoryAsync(id) ?? new List<ClientHistoryResponse>();
                return PartialView("_ClientHistory", history);
            }
            catch (Exception)
            {
                // TODO: Log the exception
                return StatusCode(500, "Error al obtener el historial del cliente.");
            }
        }
    }
}
