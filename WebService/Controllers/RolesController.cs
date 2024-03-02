using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Helpers.Authorization;
using Common.Messages.DTOS;
using Common.Messages.Requests;
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
public class RolesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepositoryService<AspNetRole> _repositoryService;

    public RolesController(IRepositoryService<AspNetRole> repositoryService, IMapper mapper)
    {
        _repositoryService = repositoryService;
        _mapper = mapper;
    }

    // GET: api/<RolesController>
    [HttpGet]
    public async Task<IEnumerable<RoleDTO>> Get()
    {
        var roles = await _repositoryService.GetAllAsync();
        var rolesDTO = roles.AsQueryable().ProjectTo<RoleDTO>(_mapper.ConfigurationProvider).ToList();
        return rolesDTO;
    }

    // POST api/<RolesController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BaseEntityRequest request)
    {
        var role = new AspNetRole(request.Name);
        var result = await _repositoryService.CreateEntityAsync(role);
        return Ok(result);
    }

    // PUT api/<RolesController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] BaseEntityRequest request)
    {
        var current = await _repositoryService.GetByIdAsync(id);
        if (current != null)
        {
            current.SetNames(request.Name);
            var results = await _repositoryService.UpdateEntityAsync(current);
            return Ok(results);
        }

        return NotFound();
    }

    // DELETE api/<RolesController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var results = await _repositoryService.DeleteEntityAsync(id);
        return Ok(results);
    }
}