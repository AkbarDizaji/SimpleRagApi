using Microsoft.AspNetCore.Mvc;
using SimpleRagApi.Models;
using SimpleRagApi.Services;

namespace SimpleRagApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RagController : ControllerBase
{
    private readonly RagService _ragService;

    public RagController(RagService ragService)
    {
        _ragService = ragService;
    }

    [HttpPost("index")]
    public async Task<IActionResult> Index([FromBody] IndexRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest("Text is required.");
        }

        try
        {
            var count = await _ragService.IndexDocumentAsync(request.Text);
            return Ok(new { message = "Indexed successfully", chunkCount = count });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] AskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return BadRequest("Question is required.");
        }

        try
        {
            var result = await _ragService.AskAsync(request.Question);
            return Ok(new 
            { 
                answer = result.Answer, 
                contextChunks = result.ContextChunks 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
