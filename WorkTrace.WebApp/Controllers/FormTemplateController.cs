using Microsoft.AspNetCore.Mvc;
using WorkTrace.WebApp.Filters;
using WorkTrace.WebApp.Helpers;
using WorkTrace.WebApp.Models.Dtos.FormTemplate;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp.Controllers
{
    [AuthorizeSession]
    public class FormTemplateController : Controller
    {
        private readonly IFormTemplateApiService _formTemplateApiService;

        public FormTemplateController(IFormTemplateApiService formTemplateApiService)
        {
            _formTemplateApiService = formTemplateApiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var templates = await _formTemplateApiService.GetAllAsync();
                return Json(new { success = true, data = templates });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al obtener plantillas: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            try
            {
                var template = await _formTemplateApiService.GetByIdAsync(id);
                if (template == null) return NotFound();
                return Json(new { success = true, data = template });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al obtener plantilla: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFormTemplateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
            }

            try
            {
                var result = await _formTemplateApiService.CreateAsync(request);
                if (result == null)
                    return Json(new { success = false, message = "La API no devolvió una plantilla creada." });

                return Json(new { success = true, message = "Plantilla creada exitosamente.", data = result });
            }
            catch (Exception ex)
            {
                var errorMessage = ErrorParser.Parse(ex.Message);
                return Json(new { success = false, message = "No se pudo crear la plantilla: " + errorMessage });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] string id, [FromBody] UpdateFormTemplateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
            }

            try
            {
                var result = await _formTemplateApiService.UpdateAsync(id, request);
                if (result == null)
                    return Json(new { success = false, message = "La API no devolvió un resultado exitoso." });

                return Json(new { success = true, message = "Plantilla actualizada exitosamente.", data = result });
            }
            catch (Exception ex)
            {
                var errorMessage = ErrorParser.Parse(ex.Message);
                return Json(new { success = false, message = "No se pudo actualizar la plantilla: " + errorMessage });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuestions([FromQuery] string id, [FromBody] List<UpdateFormQuestionsRequest> questions)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
            }

            try
            {
                var result = await _formTemplateApiService.UpdateQuestionsAsync(id, questions);
                if (result == null)
                    return Json(new { success = false, message = "La API no devolvió un resultado exitoso." });

                return Json(new { success = true, message = "Preguntas actualizadas exitosamente.", data = result });
            }
            catch (Exception ex)
            {
                var errorMessage = ErrorParser.Parse(ex.Message);
                return Json(new { success = false, message = "No se pudo actualizar las preguntas: " + errorMessage });
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Activate([FromQuery] string id)
        {
            try
            {
                var (success, error) = await _formTemplateApiService.ActivateAsync(id);
                if (success)
                    return Json(new { success = true, message = "Plantilla activada exitosamente." });
                return Json(new { success = false, message = "Error al activar: " + error });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error interno: " + ex.Message });
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Deactivate([FromQuery] string id)
        {
            try
            {
                var (success, error) = await _formTemplateApiService.DeactivateAsync(id);
                if (success)
                    return Json(new { success = true, message = "Plantilla desactivada exitosamente." });
                return Json(new { success = false, message = "Error al desactivar: " + error });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error interno: " + ex.Message });
            }
        }
    }
}
