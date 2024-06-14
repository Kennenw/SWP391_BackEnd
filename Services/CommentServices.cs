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
        public void CreateComment(int idUser,CommentDTO commentDTO);
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

        public void CreateComment(int idUser,CommentDTO commentDTO)
        {
            var user = _unitOfWork.AccountRepo.GetById(idUser);
            var comment = new Comment
            {
                UserId = user.AccountId,
                Title = commentDTO.Title,
                Image = commentDTO.Image,
                Context = commentDTO.Context,
                Status = true,
            };
            _unitOfWork.CommentRepo.Create(comment);
            _unitOfWork.SaveChanges();
        }

        public void DeleteComment(int id)
        {
            var comment = _unitOfWork.CommentRepo.GetById(id);
            if(comment != null)
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
            };
        }

        public void UpdateComment(int id, CommentDTO commentDTO)
        {
            var comment = _unitOfWork.CommentRepo.GetById(id);
            if(comment != null)
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
