using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;

namespace Gmphan.DataAccessLib.Repository
{
    public class PostRepo : Repository<Post>, IPostRepo
    {
        private readonly AppDbContext _db;
        public PostRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<PostMetaDTO>> GetMetaDTORepoAsync()
        {
            try
            {
                List<PostMetaDTO> postMetaDTOs = _db.Posts
                    .Select(p => new PostMetaDTO
                    {
                        Id = p.Id,
                        Title = p.Title,
                    })
                    .ToList();
                if(postMetaDTOs.Count >= 0 )
                {
                    return postMetaDTOs;
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }

        public async Task UpdateAsync(Post obj)
        {
            _db.Posts.Update(obj);
        }
    }
}