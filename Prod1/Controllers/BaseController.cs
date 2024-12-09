using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace btlz.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : Controller
{
    protected IMapper Mapper;

    protected BaseController(IMapper mapper)
        => Mapper = mapper;
}