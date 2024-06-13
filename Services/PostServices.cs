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

namespace Services
{
    public interface IPostServices
    {
        public PagedResult<PostDTO> PostPagedResult(string query, int pageNumber, int pageSize);
        public PostDTO GetPostById(int id);
        public void CreatePost(PostDTO postDTO);
        public void UpdatePost(int id, PostDTO postDTO);
        public void DeletePost(int id);
        Task UploadPostImage(int postId, string base64Image);
        public PagedResult<PostDTO> GetPost(int pageNumber, int pageSize);
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
                Content = postDTO.Content,
                Status = true,  
                Title = postDTO.Title,
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
        public PagedResult<PostDTO> PostPagedResult(string query, int pageNumber, int pageSize)
        {
            var post = _unitOfWork.PostRepo.GetAll();
            if (!string.IsNullOrEmpty(query))
            {
                post = post.Where(a => a.Title.Contains(query)).ToList();
            }       
            var totalItemAccount = post.Count;
            var pagedAccount = post.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var postDTOs = pagedAccount.Where(a => a.Status == true).Select(a => new PostDTO
            {
                PostId = a.PostId,
                AccountId = a.AccountId,
                Image = a.Image,
                Content = a.Content,
                TotalRate = a.TotalRate,
                Title = a.Title,
            }).ToList();
            return new PagedResult<PostDTO>
            {
                Items = postDTOs,
                TotalItem = totalItemAccount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }

        public PostDTO GetPostById(int id)
        {
            Post post = _unitOfWork.PostRepo.GetById(id);
            if (post == null || post.Status == false)
            {
                return null;
            }

            return new PostDTO
            {
                PostId = post.PostId,
                AccountId = post.AccountId,
                Image = post.Image,
                Content = post.Content,
                TotalRate = post.TotalRate,
                Title = post.Title,
                Rate = post.Rate,
            };
        }

        public void UpdatePost(int id, PostDTO postDTO)
        {
            var post = _unitOfWork.PostRepo.GetById(id);
            if(post != null)
            {
                post.Title = postDTO.Title;
                post.Content = postDTO.Content;

                _unitOfWork.PostRepo.Update(post);
                _unitOfWork.SaveChanges();
            }
        }
        public async Task UploadPostImage(int postId, string base64Image)
        {
            var post = _unitOfWork.PostRepo.GetById(postId);
            if (post == null || post.Status == false) return;

            // Giải mã chuỗi base64 về mảng byte
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            var fileName = Guid.NewGuid().ToString() + ".jpg";
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);

            await File.WriteAllBytesAsync(filePath, imageBytes);

            post.Image = fileName; // Lưu tên file vào cột Image

            _unitOfWork.PostRepo.Update(post);
            _unitOfWork.SaveChanges();
        }

        public PagedResult<PostDTO> GetPost(int pageNumber, int pageSize)
        {
            var post = _unitOfWork.PostRepo.GetAll();          
            var totalItemAccount = post.Count;
            var pagedAccount = post.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var postDTOs = pagedAccount.Where(a => a.Status == true).Select(a => new PostDTO
            {
                PostId = a.PostId,
                AccountId = a.AccountId,
                Image = a.Image,
                Content = a.Content,
                TotalRate = a.TotalRate,
                Title = a.Title,
            }).ToList();
            return new PagedResult<PostDTO>
            {
                Items = postDTOs,
                TotalItem = totalItemAccount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }
    }
}
