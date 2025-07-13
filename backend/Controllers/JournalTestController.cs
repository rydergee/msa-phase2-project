using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MockMate.Api.Models;
using MockMate.Api.Repositories.Interfaces;
using System.Security.Claims;

namespace MockMate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JournalTestController : ControllerBase
{
    private readonly IJournalEntryRepository _journalRepository;

    public JournalTestController(IJournalEntryRepository journalRepository)
    {
        _journalRepository = journalRepository;
    }

    [HttpGet]
    public async Task<ActionResult<string>> GetTest()
    {
        return Ok("Journal API is working");
    }
}
