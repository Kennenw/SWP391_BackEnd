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

    public interface ICommentServices
    {
        public List<CommentDTO> GetComment();
        public CommentDTO GetCommentById(int id);
        public void CreateComment(CommentDTO commentDTO);
        public void UpdateComment(int id, CommentDTO commentDTO);
        public void DeleteComment(int id);
    }
    public class CommentServices : ICommentServices
    {
        private readonly UnitOfWork _unitOfWork;

        public CommentServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public void CreateComment(CommentDTO commentDTO)
        {
            var user = _unitOfWork.AccountRepo.GetById((int)commentDTO.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            Comment comment = new Comment
            {
                UserId = user.AccountId,
                Title = commentDTO.Title,
                Image = commentDTO.Image,
                Context = commentDTO.Context,
                Status = true,
            };

            if (commentDTO.PostId.HasValue && commentDTO.PostId.Value != 0)
            {
                var post = _unitOfWork.PostRepo.GetById((int)commentDTO.PostId);
                if (post == null)
                {
                    throw new Exception("Post not found");
                }
                comment.PostId = post.PostId;
            }
            else if (commentDTO.CourtId.HasValue && commentDTO.CourtId.Value != 0)
            {
                var court = _unitOfWork.CourtRepo.GetById((int)commentDTO.CourtId);
                if (court == null)
                {
                    throw new Exception("Court not found");
                }
                comment.CourtId = court.CourtId;
            }
            else
            {
                throw new Exception("Either PostId or CourtId must be provided");
            }

            _unitOfWork.CommentRepo.Create(comment); 
            _unitOfWork.SaveChanges();
        }





        public void DeleteComment(int id)
        {
            var comment = _unitOfWork.CommentRepo.GetById(id);
            if (comment != null)
            {
                comment.Status = false;
                _unitOfWork.CommentRepo.Update(comment);
                _unitOfWork.SaveChanges();
            }
        }

        public List<CommentDTO> GetComment()
        {
            return _unitOfWork.CommentRepo.GetAll().Where(c => c.Status == true).Select(comment => new CommentDTO
            {
                UserId = comment.UserId,
                CommentId = comment.CommentId,
                Title = comment.Title,
                Image = comment.Image,
                Context = comment.Context,
                PostId = comment.PostId,
                CourtId = comment.CourtId,
                Status = comment.Status,
            }).ToList();
        }

        public CommentDTO GetCommentById(int id)
        {
            var comment = _unitOfWork.CommentRepo.GetById(id);
            if (comment == null || comment.Status == false)
            {
                return null;
            }
            return new CommentDTO
            {
                UserId = comment.UserId,
                CommentId = comment.CommentId,
                Title = comment.Title,
                Image = comment.Image,
                Context = comment.Context,
                PostId = comment.PostId,    
                CourtId = comment.CourtId,
            };
        }

        public void UpdateComment(int id, CommentDTO commentDTO)
        {
            var comment = _unitOfWork.CommentRepo.GetById(id);
            if (comment != null)
            {
                comment.Title = commentDTO.Title;
                comment.Image = commentDTO.Image;
                comment.Context = commentDTO.Context;
                _unitOfWork.CommentRepo.Update(comment);
                _unitOfWork.SaveChanges();
            }
        }
    }
}
