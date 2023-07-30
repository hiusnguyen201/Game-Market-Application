using GMA.DAL;
using GMA.Models;

namespace GMA.BLL;

public class AccountBLL
{
    public Account SearchByUsername(string username)
    {
        AccountDAL accountDAL = new AccountDAL();
        Account account = accountDAL.GetByUsername(username);
        return account;
    }

    public Account SearchByEmail(string email)
    {
        AccountDAL accountDAL = new AccountDAL();
        Account account = accountDAL.GetByEmail(email);
        return account;
    }

    public Account SearchAccountLogin(string username, string password)
    {
        AccountDAL accountDAL = new AccountDAL();
        Account account = accountDAL.GetAccountLogin(username, password);
        return account;
    }

    public int Save(Account account)
    {
        int result = 0;
        if(account != null)
        {
            AccountDAL accountDAL = new AccountDAL();
            result =  accountDAL.SaveAccount(account);
        }
        return result;
    }

    public void UpdateMoney(int id, double deposit)
    {
        AccountDAL accountDAL = new AccountDAL();
        accountDAL.UpdateAccountMoney(id, deposit);
    }
}
