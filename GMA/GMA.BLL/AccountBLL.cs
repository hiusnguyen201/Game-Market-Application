using GMA.DAL;
using GMA.Models;

namespace GMA.BLL;

public class AccountBLL
{
    private AccountDAL accountDAL = new AccountDAL();
    public Account SearchByUsername(string username) => accountDAL.GetByUsername(username);

    public Account SearchByEmail(string email) => accountDAL.GetByEmail(email);

    public Account SearchAccountLogin(string username, string password) => accountDAL.GetAccountLogin(username, password);

    public int Save(Account account)
    {
        int result = 0;
        if (account != null)
        {
            result = accountDAL.SaveAccount(account);
        }
        return result;
    }

    public void UpdateMoney(int id, double deposit) => accountDAL.UpdateAccountMoney(id, deposit);
}