using BlogAPI.Models;

namespace BlogAPI.Services.Interfaces;

public interface IPostService
{
    Task<Post> GetPostByIdAsync(int id);
    Task<List<Post>> GetAllPostsAsync();
    Task CreatePostAsync(Post post);
    Task UpdatePostAsync(Post post);
    Task DeletePostAsync(Post post);
}
