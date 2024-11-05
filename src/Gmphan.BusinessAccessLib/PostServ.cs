using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.BusinessAccessLib.CustomExceptions;
using Gmphan.DataAccessLib.Repository;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;
using Markdig;
using Microsoft.Extensions.Logging;

namespace Gmphan.BusinessAccessLib
{
    public class PostServ : IPostServ
    {
        private readonly ILogger<PostServ> _logger;
        private readonly IUnityOfWork _unityOfWork;
        public PostServ(ILogger<PostServ> logger
                        , IUnityOfWork unityOfWork)
        {
            _logger = logger;
            _unityOfWork = unityOfWork;
        }

        public async Task AddNewPostServAsync(Post obj)
        {
            try
            {
                obj.PublishedDate = DateTime.SpecifyKind(obj.PublishedDate, DateTimeKind.Utc);
                obj.CreatedDate = DateTime.UtcNow;
                obj.UpdatedDate = obj.CreatedDate;
                await _unityOfWork.PostRepoUOW.AddAsync(obj);
                await _unityOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("AddNewPostServAsync", ex);
            }
            
        }

        public async Task<PostDetailView> GetPostDetailViewServAsync(int id)
        {
            // no need for try catch because post will only get result or null from DataAccess
            // Exception is being handling within DataAccess.
            Post post = await _unityOfWork.PostRepoUOW.GetAsync(u => u.Id == id);

            // Applying Markdown to conent
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            PostDetailView postDetailView = new PostDetailView
            {
                Title = post.Title,
                Author = post.Author,
                PublishedDate = post.PublishedDate,
                Summary = post.Summary,
                Content = Markdown.ToHtml(post.Content, pipeline),
                Tags = post.Tags
            };
            return postDetailView;   
        }

        public async Task<PostMetaView> GetPostMetaViewServAsync()
        {
            var postMetaDTOs = await _unityOfWork.PostRepoUOW.GetMetaDTORepoAsync();
            if(postMetaDTOs == null)
            {
                throw new NotFoundException($"None Post's Title and ID was found.");
            }
            
            PostMetaView postMetaView = new PostMetaView
            {
                PostMetaDTOs = postMetaDTOs
            };

            return postMetaView;
        }
    }
}