using Repositories.Models;
using System.Linq;

namespace Repositories.DAOs
{
    public class SystemAccountDAO
    {
        private static SystemAccountDAO? instance;
        private static readonly object instanceLock = new object();

        // Private constructor for blocking using new to create new object
        private SystemAccountDAO() { }

        // Singleton Pattern
        public static SystemAccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SystemAccountDAO();
                    }
                    return instance;
                }
            }
        }

        // Function to get account by Email for Login
        public SystemAccount? GetAccountByEmail(FunewsManagementContext context, string email)
        {
            return context.SystemAccounts?.SingleOrDefault(a => a.AccountEmail == email);
        }
        // Các hàm bổ sung cho chức năng Quản lý của Admin
        public List<SystemAccount> GetAccounts(FunewsManagementContext context)
        {
            return context.SystemAccounts.ToList();
        }

        public List<SystemAccount> SearchAccounts(FunewsManagementContext context, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return GetAccounts(context);
            }

            keyword = keyword.Trim().ToLower();

            return context.SystemAccounts
                .Where(a => a.AccountId.ToString().Contains(keyword)
                         || (a.AccountName != null && a.AccountName.ToLower().Contains(keyword))
                         || (a.AccountEmail != null && a.AccountEmail.ToLower().Contains(keyword))
                         || (a.AccountRole.HasValue && a.AccountRole.Value.ToString().Contains(keyword)))
                .ToList();
        }

        public SystemAccount? GetAccountById(FunewsManagementContext context, short id)
        {
            return context.SystemAccounts.SingleOrDefault(a => a.AccountId == id);
        }

        public void AddAccount(FunewsManagementContext context, SystemAccount account)
        {
            context.SystemAccounts.Add(account);
            context.SaveChanges();
        }

        public void UpdateAccount(FunewsManagementContext context, SystemAccount account)
        {
            var tracked = context.SystemAccounts.Local.FirstOrDefault(entry => entry.AccountId == account.AccountId);
            if (tracked != null && tracked != account)
            {
                context.Entry(tracked).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            context.SystemAccounts.Update(account);
            context.SaveChanges();
        }

        public void DeleteAccount(FunewsManagementContext context, SystemAccount account)
        {
            context.SystemAccounts.Remove(account);
            context.SaveChanges();
        }
    }
}
