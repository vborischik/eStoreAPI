using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace eStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/api/menu")]
    [ApiController]
    [Authorize]
    public class MenuController : ControllerBase
    {
        [HttpGet]
public ActionResult<IEnumerable<string>> GetMenu()
{
    // Извлекаем все claims типа "permissions"
    var permissions = User.Claims
        .Where(c => c.Type == "permissions")
        .Select(c => c.Value)
        .ToList();

    // Определяем, имеет ли пользователь статус менеджера
    // (предполагается, что если в claims присутствует "Manager", то это менеджер)
    bool isManager = permissions.Contains("Manager");

    // Полный список меню для всех ролей
    var menu = new List<string>
    {
        "Products",
        "Categories",
        "Customers",
        "Orders"
    };

    // Если пользователь — менеджер, убираем пункт "Orders"
    if (isManager)
    {
        menu.Remove("Orders");
    }

    return Ok(menu);
}

    }
}
