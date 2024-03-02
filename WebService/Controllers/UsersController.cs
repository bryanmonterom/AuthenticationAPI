using AuthenticationService.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Messages.DTOS;
using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenticationAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepositoryService<User> _repositoryService;

    public UsersController(IRepositoryService<User> repositoryService, IUserService userService, IMapper mapper)
    {
        _repositoryService = repositoryService;
        _mapper = mapper;
    }

    // GET: api/<UserRolesController>
    [HttpGet]
    public async Task<IEnumerable<UserDTO>> Get()
    {
        var roles = await _repositoryService.GetAllAsync();
        var rolesDTO = roles.AsQueryable().ProjectTo<UserDTO>(_mapper.ConfigurationProvider).ToList();
        return rolesDTO;
    }


    // DELETE api/<UserRolesController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(UserDTO userRoleDto)
    {
        var results = await _repositoryService.DeleteEntityAsync(null);
        return Ok(results);
    }
}