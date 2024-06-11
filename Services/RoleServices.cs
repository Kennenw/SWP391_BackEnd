using Repositories;
using Repositories.Entities;
using Repositories.DTO;
using Repositories.Repositories;

namespace Services
{
    public interface IRoleServices
    {
        public List<RoleDTO> GetRole();
        public RoleDTO GetRoleById(int id);
        public void CreateRole(RoleDTO role);
        public void UpdateRole(int id, RoleDTO role);
        public void DeleteRole(int id);
        Task<bool> UpdateRole(int user_id, UserRole role_id);
    }
    public class RoleServices : IRoleServices
    {
        private readonly UnitOfWork _unitOfWork;
        public RoleServices() 
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public void CreateRole(RoleDTO roleDTO)
        {
            var role = new Role
            {
                RoleName = roleDTO.RoleName
            };
            _unitOfWork.RoleRepo.Create(role);
            _unitOfWork.SaveChanges();
        }

        public void DeleteRole(int id)
        {
            var role = _unitOfWork.RoleRepo.GetById(id);
            if(role != null)
            {
                role.Status = false;
                _unitOfWork.RoleRepo.Update(role);
                _unitOfWork.SaveChanges();
            }
        }

        public RoleDTO GetRoleById(int id)
        {
            var check =  _unitOfWork.RoleRepo.GetById(id);
                return new RoleDTO
                {
                    RoleId = check.RoleId,
                    RoleName = check.RoleName
                };
        }

        public List<RoleDTO> GetRole()
        {
            return _unitOfWork.RoleRepo.GetAll()
                    .Select(role => new RoleDTO
                    {
                        RoleId = role.RoleId,
                        RoleName = role.RoleName
                    }).ToList();
        }

        public void UpdateRole(int id, RoleDTO roleDTO)
        {
            var role = _unitOfWork.RoleRepo.GetById(id);
            if (role != null)
            {
                role.RoleName = roleDTO.RoleName;
                role.Status = true;
                _unitOfWork.RoleRepo.Update(role);
                _unitOfWork.SaveChanges();
            }
        }

        public async Task<bool> UpdateRole(int user_id, UserRole role_id)
        {
            var user = _unitOfWork.AccountRepo.GetById(user_id);

            if (user == null)
                throw new Exception("Invalid user id");

            if (user.RoleId != (int)role_id)
            {
                user.RoleId = (int)role_id;
                _unitOfWork.SaveChanges();
            }

            return true;
        }
    }
}
