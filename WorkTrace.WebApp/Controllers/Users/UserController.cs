using Microsoft.AspNetCore.Mvc;
using WorkTrace.WebApp.Filters;
using WorkTrace.WebApp.Models.Dtos.Users;
using WorkTrace.WebApp.Services.Interfaces;

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
            var users = await _userApiService.GetAllAsync();
            return View(users);
        }
        catch
        {
            TempData["Error"] = "Error al obtener la lista de usuarios.";
            return View(new List<UserInformationResponse>());
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateUserRequest());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var createdUser = await _userApiService.CreateAsync(model);

            TempData["Success"] = "Usuario creado exitosamente.";
            return RedirectToAction("Index");
        }
        catch
        {
            TempData["Error"] = "No se pudo crear el usuario.";
            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string id)
    {
        try
        {
            var user = await _userApiService.GetByIdAsync(id);

            var updateModel = new UpdateUserRequest
            {
                FullName = user.FullName,
                DocumentNumber = user.DocumentNumber,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive
            };

            ViewBag.UserId = id;
            return View(updateModel);
        }
        catch
        {
            TempData["Error"] = "Usuario no encontrado.";
            return RedirectToAction("Index");
        }
    }
}