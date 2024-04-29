using BlogAPI.Models;

namespace BlogAPI.Repositories.Interfaces;

public interface IPostRepository
{
    Task<Post> GetPostByIdAsync(int id);
    Task<List<Post>> GetAllPostsAsync();
    Task CreatePostAsync(Post post);
    Task UpdatePostAsync(Post post);
    Task DeletePostAsync(Post post);
}
