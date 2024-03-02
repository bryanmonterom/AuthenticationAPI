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
public class UserApplicationsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepositoryService<UserApplication> _repositoryService;

    public UserApplicationsController(IRepositoryService<UserApplication> repositoryService, IMapper mapper)
    {
        _repositoryService = repositoryService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<UserApplicationDTO>> Get()
    {
        var roles = await _repositoryService.GetAllAsync();
        var appsDTO = roles.AsQueryable().ProjectTo<UserApplicationDTO>(_mapper.ConfigurationProvider).ToList();
        return appsDTO;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserApplicationDTO request)
    {
        var role = new UserApplication(request.IdUser, request.IdApplication);
        var result = await _repositoryService.CreateEntityAsync(role);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(UserApplicationDTO request)
    {
        var results = await _repositoryService.DeleteEntityAsync(request.IdUser, request.IdApplication);

        return Ok(results);
    }
}