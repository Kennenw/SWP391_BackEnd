using Repositories;
using Repositories.DTO;
using Repositories.Entities;
using Repositories.Repositories;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Services
{
    public interface IAccountServices
    {
        PagedResult<AccountDTO> GetAccount( int pageNumber, int pageSize);
        AccountDTO GetAccountById(int id);
        AccountDTO GetAccountByName(string name);
        void DeleteAccount(int id);
        AccountDTO Login(string username, string password);
        PagedResult<AccountDTO> PagedResult(string query, int pageNumber, int pageSize);
        SelfProfile GetSelfProfile(int id);
        bool RegisterUser(RegisterInformation info);
        bool RegisterStaff(AccountDTO info);
        bool UpdatePassword(string email, UpdatePassword info);
        bool UpdateProfile(int user_id, UpdateProfileUser param);
        Task<int> SettingPassword(int user_id, SettingPasswordRequest info);
        bool IsUserExist(string? email);
        bool IsAdminAndStaff(int user_id);
        bool IsAdmin(int user_id);
        bool UpdateRoleUser(int user_id, Role role_id);
        Task UploadCourtImageAsync(int accountId, byte[] imageBytes);

    }
    public class AccountServices : IAccountServices
    {
        private readonly UnitOfWork _unitOfWork;
        private string image = "clone-account.png";
        public AccountServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public PagedResult<AccountDTO> GetAccount( int pageNumber, int pageSize)
        {
            var account = _unitOfWork.AccountRepo.GetAll();          
            var totalItemAccount = account.Count;
            var pagedAccount = account.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var accountDTOs = pagedAccount.Select(a => new AccountDTO
            {
                AccountId = a.AccountId,
                AccountName = a.AccountName,
                Password = a.Password,
                FullName = a.FullName,
                Phone = a.Phone,
                Email = a.Email,
                RoleId = a.RoleId,
                Image = a.Image,
                Status = a.Status,
            }).ToList();
            return new PagedResult<AccountDTO>
            {
                Items = accountDTOs,
                TotalItem = totalItemAccount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

        }
        public AccountDTO GetAccountById(int id)
        {
            var account = _unitOfWork.AccountRepo.GetById(id);
            if(account == null)
            {
                return null;
            }
            return new AccountDTO
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                Password = account.Password,
                FullName = account.FullName,
                Phone = account.Phone,
                Email = account.Email,
                RoleId = account.RoleId,
                Status = account.Status,
                Image = account.Image,
            };
        }

        public AccountDTO GetAccountByName(string name)
        {
            var account = _unitOfWork.AccountRepo.GetByName(name);
            if (account == null)
            {
                return null;
            }
            return new AccountDTO
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                Password = account.Password,
                FullName = account.FullName,
                Phone = account.Phone,
                Email = account.Email,
                RoleId = account.RoleId,
                Status = account.Status,
                Image = account.Image,
            };
        }


        public void DeleteAccount(int id)
        {

            var account = _unitOfWork.AccountRepo.GetById(id);
            if (account != null)
            {
                account.Status = false;
                _unitOfWork.AccountRepo.Update(account);
                _unitOfWork.SaveChanges();
            }
        }


        public AccountDTO Login(string username, string password)
        {
            var account = _unitOfWork.AccountRepo.GetAccountByEmailAndPassword(username, password);
            if (account == null)
            {
                return null;
            }
            return new AccountDTO
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                Password = account.Password,
                FullName = account.FullName,
                Phone = account.Phone,
                Email = account.Email,
                RoleId = account.RoleId,
                Status = account.Status,
            };
        }

        public PagedResult<AccountDTO> PagedResult(string query,  int pageNumber, int pageSize)
        {
            var account = _unitOfWork.AccountRepo.GetAll();
            if (!string.IsNullOrEmpty(query))
            {
                account = account.Where(a => a.AccountName.Contains(query) || a.FullName.Contains(query)).ToList();
            }
            var totalItemAccount = account.Count;
            var pagedAccount = account.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var accountDTOs = pagedAccount.Select(a => new AccountDTO
            {
                AccountId = a.AccountId,
                AccountName = a.AccountName,
                Password = a.Password,
                FullName = a.FullName,
                Phone = a.Phone,
                Email = a.Email,
                Image = a.Image,
                RoleId = a.RoleId,
                Status = a.Status
            }).ToList();
            return new PagedResult<AccountDTO>
            {
                Items = accountDTOs,
                TotalItem = totalItemAccount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }

        public SelfProfile GetSelfProfile(int id)
        {
            var user = _unitOfWork.AccountRepo.GetById(id);
            if (user == null || user.Status == false)
            {
                return null;
            }
            return new SelfProfile
            {
                UserName = user.AccountName,
                FullName = user.FullName,
                PhoneNumber = user.Phone,
                ImgUrl = user.Image,
            };
        }

        public bool IsUserExist(string? email)
        {
            return _unitOfWork.AccountRepo.IsUserExist(email);
        }

        public bool RegisterUser(RegisterInformation info)
        {
            var check = _unitOfWork.AccountRepo.GetAccountByEmail(info.Email);
            if(check == null)
            {
                var user = new Account
                {
                    AccountName = info.UserName,
                    Phone = info.PhoneNum,
                    Email = info.Email,
                    FullName = info.FullName,
                    Password = info.Password,
                    Image = image,
                    RoleId = 2,
                    Status = true
                };
                _unitOfWork.AccountRepo.Create(user);
                _unitOfWork.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RegisterStaff(AccountDTO info)
        {
            var check = _unitOfWork.AccountRepo.GetAccountByEmail(info.Email);
            if (check == null)
            {
                var user = new Account
                {
                    AccountName = info.AccountName,
                    Phone = info.Phone,
                    Email = info.Email,
                    FullName = info.FullName,
                    Password = info.Password,
                    RoleId = info.RoleId,
                    Image = image,
                    Status = true
                };
                _unitOfWork.AccountRepo.Create(user);
                _unitOfWork.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdatePassword(string email, UpdatePassword info)
        {
            var user = _unitOfWork.AccountRepo.GetAccountByEmail(email);
            if (user.Password != info.NewPassword)
            {
                user.Password = info.NewPassword;
                _unitOfWork.AccountRepo.Update(user);
                _unitOfWork.SaveChanges();
                Console.WriteLine("Update sussecfully.");
                return true;
            }
            else
            {
                Console.WriteLine("Error: New password must be different from the current password.");
                return false;
            }
        }

        public bool UpdateProfile(int user_id, UpdateProfileUser param)
        {
            var user = _unitOfWork.AccountRepo.GetById(user_id);
            if (user != null)
            {
                user.AccountName = param.UserName;
                user.FullName = param.FullName;
                user.Phone = param.PhoneNumber;
                _unitOfWork.AccountRepo.Update(user);
                _unitOfWork.SaveChanges();
                return true;
            }return false;
        }

        public async Task<int> SettingPassword(int user_id, SettingPasswordRequest info)
        {
            var user =  _unitOfWork.AccountRepo.GetById(user_id);
            if (user == null || user.Status == false)
            {
                return -2;
            }

            if (user.Password != info.OldPass)
            {
                return 0;
            }

            if (info.NewPass != info.ReEnterPass)
            {
                return -1;
            }

            user.Password = info.NewPass;
            _unitOfWork.AccountRepo.Update(user);
            _unitOfWork.SaveChanges();
            return 1;
        }
        public bool IsAdmin(int user_id)
        {
            return _unitOfWork.AccountRepo.IsAdmin(user_id);
        }

        public bool IsAdminAndStaff(int user_id)
        {
            return _unitOfWork.AccountRepo.IsAdminAndStaff(user_id);
        }

        public bool UpdateRoleUser(int user_id, Role role_id)
        {
            var user = _unitOfWork.AccountRepo.GetById(user_id);

            if (user == null || user.Status == false)
                throw new Exception("Invalid user id");

            if (user.RoleId != role_id.RoleId)
            {
                user.RoleId = role_id.RoleId;
                _unitOfWork.AccountRepo.Update(user);
                _unitOfWork.SaveChanges();
            }

            return true;
        }

        public async Task UploadCourtImageAsync(int accountId, byte[] imageBytes)
        {
            var account = _unitOfWork.AccountRepo.GetById(accountId);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
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

            account.Image = fileName;

            _unitOfWork.AccountRepo.Update(account);
            _unitOfWork.SaveChanges();

            Console.WriteLine("Image saved successfully.");
        }

    }
}
