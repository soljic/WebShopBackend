using API.DTOs;
using API.SignalR;
using Application.Core;
using Application.Notifications;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    private readonly IHubContext<NotificatonHub> _hubContext;

    public UsersController(UserManager<AppUser> userManager, IConfiguration config, HttpClient httpClient, IMapper mapper, DataContext context,  IHubContext<NotificatonHub> hubContext)
    {
        _userManager = userManager;
        _config = config;
        _httpClient = httpClient;
        _mapper = mapper;
        _context = context;
        _hubContext = hubContext;
    }

    // GET
    [HttpGet]
    [Authorize]
    public async  Task<ActionResult<UserDto>> GetUser()
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        if (currentUser == null) return  NotFound();

        var user =  _mapper.Map<UserDto>(currentUser);
        return HandleResult(Result<UserDto>.Success(user));
    }

    
    [HttpPost("partner/{id}")]
    [Authorize]
  public async Task<ActionResult<UserDto>> CreatePartner(string id)
{
    var currentUser = await _userManager.GetUserAsync(HttpContext.User);
    var partner = await _userManager.FindByIdAsync(id);
    
    ArgumentNullException.ThrowIfNull(currentUser);
    ArgumentNullException.ThrowIfNull(partner);

    if (currentUser?.PartnerId != null || partner?.PartnerId != null)
        return NotFound("Person already has a partner");

    currentUser.PartnerId = id;
    partner.PartnerId = currentUser.Id;

    _context.Entry(currentUser).State = EntityState.Modified;
    _context.Entry(partner).State = EntityState.Modified;

    if (!(await _context.SaveChangesAsync() > 0))
        return BadRequest("Partners weren't created");
    
    var command = new Create.Command
    {
        Body = "A new partner has been created.",
        UserName = currentUser.UserName
    };

    await _hubContext.Clients.User(currentUser.Id).SendAsync("SendNotification", command);

    var user = _mapper.Map<UserDto>(currentUser);
    return HandleResult(Result<UserDto>.Success(user));
}
    
    
  [HttpPost("remove-partner/{id}")]
  [Authorize]
  public async Task<ActionResult<UserDto>> RemovePartner(string id)
  {
      var currentUser = await _userManager.GetUserAsync(HttpContext.User);
      var partner = await _userManager.FindByIdAsync(id);
    
      ArgumentNullException.ThrowIfNull(currentUser);
      ArgumentNullException.ThrowIfNull(partner);

      if (currentUser?.PartnerId != partner?.Id)
          return NotFound("These two are not partners");

      currentUser.PartnerId = null;
      partner.PartnerId = null;

      _context.Entry(currentUser).State = EntityState.Modified;
      _context.Entry(partner).State = EntityState.Modified;

      if (!(await _context.SaveChangesAsync() > 0))
          return BadRequest("Partners weren't removed, problem saving in the database");

      var user = _mapper.Map<UserDto>(currentUser);
      return HandleResult(Result<UserDto>.Success(user));
  }


}