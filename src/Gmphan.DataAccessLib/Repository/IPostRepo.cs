using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;

namespace Gmphan.DataAccessLib.Repository
{
    public interface IPostRepo : IRepository<Post>
    {
        public Task<List<PostMetaDTO>> GetMetaDTORepoAsync();
        public Task UpdateAsync(Post obj);
    }
}