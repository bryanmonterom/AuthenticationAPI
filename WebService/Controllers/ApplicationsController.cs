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
public class ApplicationsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepositoryService<Application> _repositoryService;

    public ApplicationsController(IRepositoryService<Application> repositoryService, IMapper mapper)
    {
        _repositoryService = repositoryService;
        _mapper = mapper;
    }

    // GET: api/<ApplicationsController>
    [HttpGet]
    public async Task<IEnumerable<ApplicationDTO>> Get()
    {
        var roles = await _repositoryService.GetAllAsync();
        var appsDTO = roles.AsQueryable().ProjectTo<ApplicationDTO>(_mapper.ConfigurationProvider).ToList();
        return appsDTO;
    }

    // POST api/<ApplicationsController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BaseEntityRequest request)
    {
        var role = new Application(request.Name);
        var result = await _repositoryService.CreateEntityAsync(role);
        return Ok(result);
    }

    // PUT api/<ApplicationsController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] BaseEntityRequest request)
    {
        var current = await _repositoryService.GetByIdAsync(id);
        if (current != null)
        {
            current.Name = request.Name;
            var results = await _repositoryService.UpdateEntityAsync(current);
            return Ok(results);
        }

        return NotFound();
    }

    // DELETE api/<ApplicationsController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var results = await _repositoryService.DeleteEntityAsync(id);
        return Ok(results);
    }
}