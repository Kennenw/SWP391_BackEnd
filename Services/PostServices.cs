using Microsoft.Identity.Client;
using Repositories;
using Repositories.DTO;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IPostServices
    {
        public List<PostDTO> GetPost();
        public PostDTO GetPostById(int id);
        public void CreatePost(PostDTO postDTO);
        public void UpdatePost(int id, PostDTO postDTO);
        public void DeletePost(int id);
    }
    public class PostServices : IPostServices
    {
        private readonly UnitOfWork _unitOfWork;

        public PostServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public void CreatePost(PostDTO postDTO)
        {
            Post post = new Post()
            {
                AccountId = postDTO.AccountId,
                Image = postDTO.Image,
                Context = postDTO.Context,
                Vote = postDTO.Vote,
            };
        }

        public void DeletePost(int id)
        {
            Post post = _unitOfWork.PostRepo.GetById(id);
            if(post != null)
            {
                post.Status = false;
                _unitOfWork.PostRepo.Update(post);
                _unitOfWork.SaveChanges();
            }
        }

        public List<PostDTO> GetPost()
        {
            List<Post> posts = _unitOfWork.PostRepo.GetAll();
            List<PostDTO> postDTOs = new List<PostDTO>();

            foreach (var post in posts)
            {
                postDTOs.Add(new PostDTO
                {
                    PostId = post.PostId,
                    AccountId = post.AccountId,
                    Image = post.Image,
                    Context = post.Context,
                    Vote = post.Vote,
                    Comments = post.Comments.Select(c => new CommentDTO
                    {
                        CommentId = c.CommentId,
                        Title = c.Title,
                        Image = c.Image,
                        Context = c.Context,
                        PostId = c.PostId
                    }).ToList()
                });
            }

            return postDTOs;
        }

        public PostDTO GetPostById(int id)
        {
            Post post = _unitOfWork.PostRepo.GetById(id);
            if (post == null)
            {
                return null;
            }

            return new PostDTO
            {
                PostId = post.PostId,
                AccountId = post.AccountId,
                Image = post.Image,
                Context = post.Context,
                Vote = post.Vote,
                Comments = post.Comments.Select(c => new CommentDTO
                {
                    CommentId = c.CommentId,
                    Title = c.Title,
                    Image = c.Image,
                    Context = c.Context,
                    PostId = c.PostId
                }).ToList()
            };
        }

        public void UpdatePost(int id, PostDTO postDTO)
        {
            var post = _unitOfWork.PostRepo.GetById(id);
            if(post != null)
            {
                post.AccountId = postDTO.AccountId;
                post.Image = postDTO.Image;
                post.Context = postDTO.Context;
                post.Vote = postDTO.Vote;
                post.Status = false;
                _unitOfWork.PostRepo.Update(post);
                _unitOfWork.SaveChanges();
            }
        }
    }
}
