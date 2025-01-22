using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib.ViewModels
{
    public class PostMetaView
    {
        public List<PostMetaDTO> PostMetaDTOs { get; set; }
        public PostMetaView()
        {
            PostMetaDTOs = new List<PostMetaDTO>();
        }
        public void EnsureAboutThePostsOnTop()
        {
            var aboutThePosts = PostMetaDTOs.FirstOrDefault(p => p.Title == "About The Posts");
            if (aboutThePosts != null)
            {
                PostMetaDTOs.Remove(aboutThePosts);
                PostMetaDTOs.Insert(0, aboutThePosts);
            }
        }
    }
}