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

            // Applying Markdown to content
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            PostDetailView postDetailView = new PostDetailView
            {
                Id = id,
                Title = post.Title,
                Author = post.Author,
                PublishedDate = post.PublishedDate,
                Summary = post.Summary,
                Content = Markdown.ToHtml(post.Content, pipeline),
                Tags = post.Tags
            };
            return postDetailView;   
        }

        public async Task<Post> GetPostServAsync(int id)
        {
            // no need for try catch because post will only get result or null from DataAccess
            // Exception is being handling within DataAccess.
            Post post = await _unityOfWork.PostRepoUOW.GetAsync(u => u.Id == id);
            return post;
        }
        public async Task UpdatePostServAsync(Post obj)
        {
            // Get the post from the database and not memory
            Post post = await _unityOfWork.PostRepoUOW.GetAsync(u => u.Id == obj.Id);
            if(post == null)
            {
                throw new NotFoundException($"Post with ID: {obj.Id} was not found.");
            }

            // Update the post
            post.Title = obj.Title;
            post.Author = obj.Author;
            post.PublishedDate = DateTime.SpecifyKind(obj.PublishedDate, DateTimeKind.Utc);
            post.Summary = obj.Summary;
            post.Content = obj.Content;
            post.Tags = obj.Tags;
            post.UpdatedDate = DateTime.UtcNow;

            // Update the post in the database
            await _unityOfWork.PostRepoUOW.UpdateAsync(post);
            await _unityOfWork.SaveAsync();

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


            // before running the postMetaView, I want to make sure the About The Posts
            // is on top of the list.
            postMetaView.EnsureAboutThePostsOnTop();
            
            return postMetaView;
        }
    }
}