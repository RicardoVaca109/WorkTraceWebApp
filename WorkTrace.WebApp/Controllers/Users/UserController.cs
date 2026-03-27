using Microsoft.AspNetCore.Mvc;
using WorkTrace.WebApp.Filters;
using WorkTrace.WebApp.Models.Dtos.Users;
using WorkTrace.WebApp.Services.Interfaces;
using WorkTrace.WebApp.Helpers;

namespace WorkTrace.WebApp.Controllers.Users;

[AuthorizeSession]
public class UserController : Controller
{
    private readonly IUserApiService _userApiService;

    public UserController(IUserApiService userApiService)
    {
        _userApiService = userApiService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var users = await _userApiService.GetAllAsync() ?? new List<UserInformationResponse>();
            return View(users);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error al obtener la lista de usuarios: {ex.Message}";
            return View(new List<UserInformationResponse>());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
        }

        try
        {
            var createdUser = await _userApiService.CreateAsync(model);
            if (createdUser == null)
            {
                return Json(new { success = false, message = "La API no devolvió un usuario creado." });
            }

            return Json(new { success = true, message = "Usuario creado exitosamente." });
        }
        catch (Exception ex)
        {
            var errorMessage = ErrorParser.Parse(ex.Message);
            return Json(new { success = false, message = "No se pudo crear el usuario: \n" + errorMessage });
        }
    }

    public IActionResult GetEditUserForm([FromQuery] UpdateUserRequest model)
    {
        return PartialView("_EditUserForm", model);
    }

    [HttpGet]
    public async Task<IActionResult> GetUser(string id)
    {
        try
        {
            var user = await _userApiService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Json(user);
        }
        catch
        {
            return StatusCode(500, "Error al obtener los datos del usuario.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Update(string id, UpdateUserRequest model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
        }

        try
        {
            var result = await _userApiService.UpdateAsync(id, model);
            if (result == null)
            {
                return Json(new { success = false, message = "La API no devolvió un resultado exitoso." });
            }
            return Json(new { success = true, message = "Usuario actualizado exitosamente." });
        }
        catch (Exception ex)
        {
            var errorMessage = ErrorParser.Parse(ex.Message);
            return Json(new { success = false, message = "No se pudo actualizar el usuario: \n" + errorMessage });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Deactivate(string id)
    {
        try
        {
            var (success, errorMessage) = await _userApiService.DeactivateAsync(id);
            if (success)
            {
                TempData["Success"] = "Usuario desactivado exitosamente.";
            }
            else
            {
                TempData["Error"] = $"No se pudo desactivar el usuario: {errorMessage}";
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Ocurrió un error al desactivar el usuario: {ex.Message}";
        }
        return RedirectToAction("Index");
    }
}