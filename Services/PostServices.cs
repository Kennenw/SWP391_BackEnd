using Microsoft.Identity.Client;
using Repositories;
using Repositories.DTO;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public interface IPostServices
    {
        public List<PostDTO> PostSearch(string query);
        public PostDTO GetPostById(int id);
        public void CreatePost(PostDTO postDTO);
        public void UpdatePost(int id, PostDTO postDTO);
        public void DeletePost(int id);
        Task UploadPostImageAsync(int postId, byte[] imageBytes);
        public List<PostDTO> GetPost();
        void RatePost(int userId,int postId, double rating);
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
                Context = postDTO.Context,
                Status = true,  
                Title = postDTO.Title,
                Image = postDTO.Image,
            };
            _unitOfWork.PostRepo.Create(post);
            _unitOfWork.SaveChanges();
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
        public List<PostDTO> PostSearch(string query)
        {
            var post = _unitOfWork.PostRepo.GetAll();
            if (!string.IsNullOrEmpty(query))
            {
                post = post.Where(a => a.Title.Contains(query)).ToList();
            }       
            var postDTOs = post.Where(a => a.Status == true).Select(a => new PostDTO
            {
                PostId = a.PostId,
                AccountId = a.AccountId,
                Image = GetPostImagePath(a.PostId),
                Context = a.Context,
                TotalRate = a.TotalRate,
                Title = a.Title,
            }).ToList();
            return postDTOs;
        }

        public PostDTO GetPostById(int id)
        {
            Post post = _unitOfWork.PostRepo.GetById(id);
            var image = GetPostImagePath(id);
            if (post == null || post.Status == false)
            {
                return null;
            }

            return new PostDTO
            {
                PostId = post.PostId,
                AccountId = post.AccountId,
                Image = image,
                Context = post.Context,
                TotalRate = post.TotalRate,
                Title = post.Title,
            };
        }

        public void UpdatePost(int id, PostDTO postDTO)
        {
            var post = _unitOfWork.PostRepo.GetById(id);
            if(post != null)
            {
                post.Title = postDTO.Title;
                post.Context = postDTO.Context;
                _unitOfWork.PostRepo.Update(post);
                _unitOfWork.SaveChanges();
            }
        }
        public async Task UploadPostImageAsync(int postId, byte[] imageBytes)
        {
            var post = _unitOfWork.PostRepo.GetById(postId);
            if (post == null)
            {
                Console.WriteLine("Post not found.");
                return;
            }

            var fileName = Guid.NewGuid().ToString() + ".png";
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(uploadPath))
            {
                Console.WriteLine("Creating uploads directory.");
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, fileName);

            await File.WriteAllBytesAsync(filePath, imageBytes);

            post.Image = fileName;

            _unitOfWork.PostRepo.Update(post);
            _unitOfWork.SaveChanges();

            Console.WriteLine("Image saved successfully.");
        }

        public List<PostDTO> GetPost()
        {
            var post = _unitOfWork.PostRepo.GetAll();          
            var postDTOs = post.Where(a => a.Status == true).Select(a => new PostDTO
            {
                PostId = a.PostId,
                AccountId = a.AccountId,
                Image = GetPostImagePath(a.PostId),
                Context = a.Context,
                TotalRate = a.TotalRate,
                Title = a.Title,
            }).ToList();
            return postDTOs;
        }

        public void RatePost(int postId, int userId, double rating)
        {
            var post = _unitOfWork.PostRepo.GetById(postId);
            if (post == null || post.Status == false) return;

            var existingRating = _unitOfWork.RatingPostRepo.GetAll()
                .FirstOrDefault(rp => rp.PostId == postId && rp.UserId == userId);

            if (existingRating != null)
            {
                existingRating.RatingValue = rating;
                _unitOfWork.RatingPostRepo.Update(existingRating);
            }
            else
            {
                var ratingPost = new RatingPost
                {
                    PostId = postId,
                    UserId = userId,
                    RatingValue = rating
                };
                _unitOfWork.RatingPostRepo.Create(ratingPost);
            }

            var countRatings = _unitOfWork.RatingPostRepo.GetAll().Count(rp => rp.PostId == postId);
            var totalRatings = _unitOfWork.RatingPostRepo.GetAll().Where(rp => rp.PostId == postId).Sum(rp => rp.RatingValue);

            post.TotalRate = totalRatings > 0 ? (double)totalRatings / countRatings : 0;
            _unitOfWork.PostRepo.Update(post);
            _unitOfWork.SaveChanges();
        }

        public string GetPostImagePath(int postId)
        {
            var post = _unitOfWork.PostRepo.GetById(postId);
            if (post == null)
            {
                throw new Exception("Post not found.");
            }

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            var imagePath = Path.Combine(uploadPath, post.Image);

            if (!System.IO.File.Exists(imagePath))
            {
                throw new Exception("Image file not found.");
            }

            return imagePath;
        }
    }
}
