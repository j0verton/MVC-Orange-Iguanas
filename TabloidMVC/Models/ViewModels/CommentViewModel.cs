using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentViewModel
    {
        public UserProfile User { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
