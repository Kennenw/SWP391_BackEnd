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
                RoleName = roleDTO.RoleName,
                Status = true
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
            if (check == null || check.Status == false)
            {
                return null;
            }
            return new RoleDTO
                {
                    RoleId = check.RoleId,
                    RoleName = check.RoleName,
                    Status = check.Status
                };
        }

        public List<RoleDTO> GetRole()
        {
            return _unitOfWork.RoleRepo.GetAll().Where(ar => ar.Status == true)
                    .Select(role => new RoleDTO
                    {
                        RoleId = role.RoleId,
                        RoleName = role.RoleName,
                        Status = role.Status
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

    }
}
