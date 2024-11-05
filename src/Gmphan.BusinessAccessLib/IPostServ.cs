using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;

namespace Gmphan.BusinessAccessLib
{
    public interface IPostServ
    {
        public Task<PostMetaView> GetPostMetaViewServAsync();
        public Task AddNewPostServAsync(Post obj);
        public Task<PostDetailView> GetPostDetailViewServAsync(int id);
    }
}