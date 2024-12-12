using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace btlz.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public abstract class BaseController : Controller;

    
