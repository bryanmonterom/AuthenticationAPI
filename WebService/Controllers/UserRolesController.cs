using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Helpers.Authorization;
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
[ApplicationAuthorize(ClaimTypes.System, Constants.AppNames.Authentication)]
public class UserRolesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepositoryService<AspNetUserRoles> _repositoryService;

    public UserRolesController(IRepositoryService<AspNetUserRoles> repositoryService, IMapper mapper)
    {
        _repositoryService = repositoryService;
        _mapper = mapper;
    }

    // GET: api/<UserRolesController>
    [HttpGet]
    public async Task<IEnumerable<UserRoleDTO>> Get()
    {
        try
        {
            var roles = await _repositoryService.GetAllAsync();
            var rolesDTO = roles.AsQueryable().ProjectTo<UserRoleDTO>(_mapper.ConfigurationProvider).ToList();
            return rolesDTO;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // POST api/<UserRolesController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserRoleDTO value)
    {
        var role = new AspNetUserRoles
        {
            RoleId = value.RoleId,
            UserId = value.UserId
        };

        var result = await _repositoryService.CreateEntityAsync(role);
        return Ok(result);
    }


    // DELETE api/<UserRolesController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(UserRoleDTO userRoleDto)
    {
        var results = await _repositoryService.DeleteEntityAsync(new { userRoleDto.UserId, userRoleDto.RoleId });
        return Ok(results);
    }
}