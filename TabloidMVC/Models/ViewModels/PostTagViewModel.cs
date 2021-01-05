using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostTagViewModel
    {
        public Post Post { get; set; }
        public List<Tag> Tags { get; set; }

        public List<Tag> AppliedTags { get; set; }

        public Tag CurrentTag { get; set; }

    }
}
