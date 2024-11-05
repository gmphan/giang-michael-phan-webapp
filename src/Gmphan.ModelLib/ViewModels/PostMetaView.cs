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
    }
}