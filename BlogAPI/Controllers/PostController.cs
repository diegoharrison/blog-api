using BlogAPI.Models;
using BlogAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Post), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(int id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound("Postagem não encontrada");
        }
        return Ok(post);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Post>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAll()
    {
        var posts = await _postService.GetAllPostsAsync();
        if (posts.Count == 0)
        {
            return NotFound("Nenhuma postagem encontrada");
        }
        return Ok(posts);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Post), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(Post post)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _postService.CreatePostAsync(post);
        return CreatedAtAction(nameof(Get), new { id = post.Id }, post);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, Post post)
    {
        var existingPost = await _postService.GetPostByIdAsync(id);
        if (existingPost == null)
        {
            return NotFound("Postagem não encontrada");
        }

        post.Id = id;
        await _postService.UpdatePostAsync(post);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var existingPost = await _postService.GetPostByIdAsync(id);
        if (existingPost == null)
        {
            return NotFound("Postagem não encontrada");
        }

        await _postService.DeletePostAsync(existingPost);

        return NoContent();
    }
}