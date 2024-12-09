using Microsoft.AspNetCore.Mvc;
using MyMusicTaste.Database;

namespace MyMusicTaste.Controllers;

public class LogoutController : Controller {
    private readonly IIdentityProvider _identity;

    public LogoutController(IIdentityProvider identity) {
        _identity = identity;
    }
    
    [HttpPost]
    public async Task<IActionResult> LogoutAsync() {
        await _identity.LogOutUserAsync();
        return Redirect("/");
    }
}