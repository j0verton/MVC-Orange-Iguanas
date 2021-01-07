using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        List<Post> GetAllPublishedPosts();
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
        List<Post> GetPostByUserId(int UserId);
        void Delete(int id);
        void UpdatePost(Post post);
        Post GetPostById(int id);
        List<Post> GetAllPosts();
    }
}