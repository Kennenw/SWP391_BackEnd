using Repositories;
using Repositories.DTO;
using Repositories.Entities;
using Repositories.Repositories;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace Services
{
    public interface IAccountServices
    {
        public PagedResult<AccountDTO> GetAccount(SortContent sortContent, int pageNumber, int pageSize);
        public AccountDTO GetAccountById(int id);
        public AccountDTO GetAccountByName(string name);
        public void DeleteAccount(int id);
        public AccountDTO Login(string username, string password);
        public PagedResult<AccountDTO> PagedResult(string query, SortContent sortContent, int pageNumber, int pageSize);
        SelfProfile GetSelfProfile(int id);
        bool RegisterUser(RegisterInformation info);

        bool RegisterStaffManager(StaffManagerDTO info);

        public bool UpdatePassword(string email, UpdatePassword info);
        bool UpdateProfile(int user_id, UpdateProfileUser param);
        Task<int> SettingPassword(int user_id, SettingPasswordRequest info);
        public bool IsUserExist(string? email);
        public bool IsAdminAndStaff(int user_id);
        public bool IsAdmin(int user_id);
        bool UpdateRoleUser(int user_id, Role role_id);
        Task UploadAccountImage(int accountId, string base64Image);

    }
    public class AccountServices : IAccountServices
    {
        private readonly UnitOfWork _unitOfWork;
         
        public AccountServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public PagedResult<AccountDTO> GetAccount(SortContent sortContent, int pageNumber, int pageSize)
        {
            var account = _unitOfWork.AccountRepo.GetAll();
            switch (sortContent.sortAccountBy)
            {
                case SortAccountByEnum.AccountId:
                    account = sortContent.sortType == SortTypeEnum.Ascending
                    ? account.OrderBy(a => a.AccountId).ToList()
                    : account.OrderByDescending(a => a.AccountId).ToList();
                    break;
                case SortAccountByEnum.AccountName:
                    account = sortContent.sortType == SortTypeEnum.Ascending
                    ? account.OrderBy(a => a.AccountName).ToList()
                    : account.OrderByDescending(a => a.AccountName).ToList();
                    break;
                case SortAccountByEnum.FullName:
                    account = sortContent.sortType == SortTypeEnum.Ascending
                    ? account.OrderBy(a => a.FullName).ToList()
                    : account.OrderByDescending(a => a.FullName).ToList();
                    break;
                case SortAccountByEnum.RoleId:
                    account = sortContent.sortType == SortTypeEnum.Ascending
                    ? account.OrderBy(a => a.RoleId).ToList()
                    : account.OrderByDescending(a => a.RoleId).ToList();
                    break;
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
                ManagedCourts = account.Courts.Select(c => new CourtDTO
                {
                    CourtId = c.CourtId,
                    AreaId = c.AreaId,
                    CourtName = c.CourtName,
                    OpenTime = c.OpenTime,
                    CloseTime = c.CloseTime,
                    Rule = c.Rules,
                    Status = c.Status
                }).ToList()
            };
        }

        public AccountDTO GetAccountByName(string name)
        {
            var account = _unitOfWork.AccountRepo.GetByName(name);
            if (account == null || account.Status == false)
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

        public PagedResult<AccountDTO> PagedResult(string query, SortContent sortContent, int pageNumber, int pageSize)
        {
            var account = _unitOfWork.AccountRepo.GetAll();
            if (!string.IsNullOrEmpty(query))
            {
                account = account.Where(a => a.AccountName.Contains(query) || a.FullName.Contains(query)).ToList();
            }
            switch (sortContent.sortAccountBy)
            {
                case SortAccountByEnum.AccountId:
                    account = sortContent.sortType == SortTypeEnum.Ascending
                    ? account.OrderBy(a => a.AccountId).ToList()
                    : account.OrderByDescending(a => a.AccountId).ToList();
                    break;

                case SortAccountByEnum.AccountName:
                    account = sortContent.sortType == SortTypeEnum.Ascending
                    ? account.OrderBy(a => a.AccountName).ToList()
                    : account.OrderByDescending(a => a.AccountName).ToList();
                    break;
                case SortAccountByEnum.FullName:
                    account = sortContent.sortType == SortTypeEnum.Ascending
                    ? account.OrderBy(a => a.FullName).ToList()
                    : account.OrderByDescending(a => a.FullName).ToList();
                    break;
                case SortAccountByEnum.RoleId:
                    account = sortContent.sortType == SortTypeEnum.Ascending
                    ? account.OrderBy(a => a.RoleId).ToList()
                    : account.OrderByDescending(a => a.Role).ToList();
                    break;
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
                    RoleId = 2,
                    Status = true
                };
                _unitOfWork.AccountRepo.Create(user);
                _unitOfWork.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RegisterStaffManager(StaffManagerDTO info)
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
                //if (param.ImgUrl != user.Image)
                //{

                //}
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

        public async Task UploadAccountImage(int accountId, string base64Image)
        {
            var user = _unitOfWork.AccountRepo.GetById(accountId);
            if (user == null) return;

            // Giải mã chuỗi base64 về mảng byte
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            var fileName = Guid.NewGuid().ToString() + ".jpg";
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);

            await File.WriteAllBytesAsync(filePath, imageBytes);

            user.Image = fileName; // Lưu tên file vào cột Image

            _unitOfWork.AccountRepo.Update(user);
            _unitOfWork.SaveChanges();
        }
    }
}
