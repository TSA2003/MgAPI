using MgAPI.Business.JSONModels;
using MgAPI.Business.Services.Interfaces;
using MgAPI.Data;
using MgAPI.Data.Entities;
using MgAPI.Data.Repositories;
using MgAPI.Services.Authorization;
using MgAPI.Services.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MgAPI.Business.Services
{
    public class PostService : IPostService
    {
        private PostRepository _postRepository;
        private UserRepository _userRepository;

        public PostService(PostRepository postRepository, UserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<Post> GetAll()
        {
            return _postRepository.ReadAll();
        }

        public Post GetById(string id)
        {
            var post = _postRepository.Read(id);
            if (post == null) throw new KeyNotFoundException("Post not found");
            return post;
        }

        public Post Create(CreatePostRequest model)
        {
            User author = _userRepository.Read(x => x.ID == model.AuthorID);
            Post post = new Post
            {
                ID = Guid.NewGuid().ToString(),
                Author = author,
                Title = model.Title,
                Description = model.Description,
                PostDate = DateTime.Now
            };


            _postRepository.Create(post);
            author.Posts.Add(post);

            return post;
        }

        public Post Edit(EditPostRequest model)
        {
            Post post = _postRepository.Read(x => x.ID == model.ID);
            post.Title = model.Title;
            post.Description = model.Description;

            return post;
        }

        public void Delete(string id)
        {
            Post post = _postRepository.Read(x => x.ID == id);
            if (post == null) throw new KeyNotFoundException("Post not found");
            _postRepository.Delete(post.ID);
        }
    }
}
