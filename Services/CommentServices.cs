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
            var comment = new Comment
            {
                Title = commentDTO.Title,
                Image = commentDTO.Image,
                Context = commentDTO.Context,
                PostId = commentDTO.PostId,
                Status = true
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
            return _unitOfWork.CommentRepo.GetAll().Select(comment => new CommentDTO
            {
                CommentId = comment.CommentId,
                Title = comment.Title,
                Image = comment.Image,
                Context = comment.Context,
                PostId = comment.PostId
            }).ToList();
        }

        public CommentDTO GetCommentById(int id)
        {
            var comment = _unitOfWork.CommentRepo.GetById(id);
            return new CommentDTO
            {
                CommentId = comment.CommentId,
                Title = comment.Title,
                Image = comment.Image,
                Context = comment.Context,
                PostId = comment.PostId
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
                comment.PostId = commentDTO.PostId;
                _unitOfWork.CommentRepo.Update(comment);
                _unitOfWork.SaveChanges();
            }
        }
    }
}
