using Repositories;
using Repositories.DTO;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Services
{
    public interface IAccountServices
    {
        public PagedResult<AccountDTO> GetAccount(SortContent sortContent, int pageNumber, int pageSize);
        public AccountDTO GetAccountById(int id);
        public AccountDTO GetAccountByName(string name);
        public void CreateAccount(AccountDTO account);
        public void UpdateAccount(int id, AccountDTO account);
        public void DeleteAccount(int id);
        public AccountDTO Login(string username, string password);
        public PagedResult<AccountDTO> PagedResult(string query,SortContent sortContent,int pageNumber, int pageSize);
    }
    public  class AccountServices : IAccountServices
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
            return new AccountDTO{
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

        public AccountDTO GetAccountByName(string name)
        {
            var account = _unitOfWork.AccountRepo.GetByName(name);
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

        public void UpdateAccount(int id, AccountDTO accountDTO)
        {
            var account = _unitOfWork.AccountRepo.GetById(id);
            if (account != null)
            {
                account.AccountName = accountDTO.AccountName;
                account.Password = accountDTO.Password;
                account.FullName = accountDTO.FullName;
                account.Phone = accountDTO.Phone;
                account.Email = accountDTO.Email;
                account.RoleId = accountDTO.RoleId;
                account.Status = accountDTO.Status;
                _unitOfWork.AccountRepo.Update(account);
                _unitOfWork.SaveChanges();
            }
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

        public void CreateAccount(AccountDTO accountDTO)
        {
            var account = new Account
            {
                AccountName = accountDTO.AccountName,
                Password = accountDTO.Password,
                FullName = accountDTO.FullName,
                Phone = accountDTO.Phone,
                Email = accountDTO.Email,
                RoleId = accountDTO.RoleId,
                Status = true,
            };
            _unitOfWork.AccountRepo.Create(account);
            _unitOfWork.SaveChanges();
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
            if(!string.IsNullOrEmpty(query))
            {
                account = account.Where(a => a.AccountName.Contains(query) || a.FullName.Contains(query)).ToList();
            }
            switch(sortContent.sortAccountBy)
            {
                case SortAccountByEnum.AccountId:
                    account = sortContent.sortType == SortTypeEnum.Ascending
                    ?account.OrderBy(a => a.AccountId).ToList()
                    :account.OrderByDescending(a => a.AccountId).ToList();
                    break;

                case SortAccountByEnum.AccountName:
                    account = sortContent.sortType == SortTypeEnum.Ascending
                    ?account.OrderBy(a => a.AccountName).ToList()
                    :account.OrderByDescending(a => a.AccountName).ToList();
                    break;
                case SortAccountByEnum.FullName:
                    account = sortContent.sortType == SortTypeEnum.Ascending
                    ?account.OrderBy(a => a.FullName).ToList()
                    :account.OrderByDescending (a => a.FullName).ToList();
                    break;
                case SortAccountByEnum.RoleId:
                    account = sortContent.sortType == SortTypeEnum.Ascending
                    ?account.OrderBy(a => a.RoleId).ToList()
                    :account.OrderByDescending(a => a.Role).ToList();
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
    }
}
