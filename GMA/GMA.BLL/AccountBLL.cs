using GMA.DAL;
using GMA.Models;

namespace GMA.BLL;

public class AccountBLL
{
    private AccountDAL accountDAL = new AccountDAL();

    public int Save(Account account)
    {
        int result = 0;
        if (account != null)
        {
            result = accountDAL.SaveAccount(account);
        }
        return result;
    }

    public Account SearchById(int id) => accountDAL.GetById(id);

    public Account SearchByUsername(string username) => accountDAL.GetByUsername(username);

    public Account SearchByEmail(string email) => accountDAL.GetByEmail(email);

    public Account SearchAccountLogin(string username, string password) => accountDAL.GetAccountLogin(username, password);

    public int UpdateMoney(int accId, double mn) => accountDAL.UpdateAccountMoney(accId, mn);
}