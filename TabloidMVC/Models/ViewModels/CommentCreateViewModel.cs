using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentCreateViewModel
    {
        public int UserId { get; set; }
        public Post ParentPost { get; set; }
        public List<Comment> Comments {get; set; }
        public Comment NewComment { get; set; }
       public bool ActiveUser { get; set; }
        
    }
}
