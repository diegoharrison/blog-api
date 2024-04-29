using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Repositories.Interfaces;
using BlogAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace BlogAPI.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly WebSocketManager _webSocketManager;

    public PostService(IPostRepository postRepository, WebSocketManager webSocketManager)
    {
        _postRepository = postRepository;
        _webSocketManager = webSocketManager; 
    }

    public async Task<Post> GetPostByIdAsync(int id)
    {
        return await _postRepository.GetPostByIdAsync(id);
    }

    public async Task<List<Post>> GetAllPostsAsync()
    {
        return await _postRepository.GetAllPostsAsync();
    }

    public async Task CreatePostAsync(Post post)
    {
        await _postRepository.CreatePostAsync(post);
        var json = JsonSerializer.Serialize(post);
        await _webSocketManager.SendToAllAsync(json);
    }

    public async Task UpdatePostAsync(Post post)
    {
        await _postRepository.UpdatePostAsync(post);
    }

    public async Task DeletePostAsync(Post post)
    {
        await _postRepository.DeletePostAsync(post);
    }
}
