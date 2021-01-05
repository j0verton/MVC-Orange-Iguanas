using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        void Add(Tag tag);
        void AddTagToPost(int tagId, int postId);
        void DeleteTag(int id);
        void EditTag(Tag tag);
        List<Tag> GetAllTags();
        Tag GetTagById(int id);
        List<Tag> GetTagsByPost(int id);
        void RemoveTagFromPost(int postTagId);
    }
}