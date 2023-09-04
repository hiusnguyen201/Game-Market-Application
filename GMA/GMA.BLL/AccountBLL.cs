using GMA.DAL;
using GMA.Models;

namespace GMA.BLL;

public class AccountBLL
{
    private AccountDAL accountDAL = new AccountDAL();
    private OrderBLL orderBLL = new OrderBLL();
    private static Account accountLoggedIn = null;

    public Account GetAccountLoggedIn()
    {
        return accountLoggedIn;
    }

    public void SetAccountLoggedIn(Account account)
    {
        account.AccountOrders = orderBLL.GetAll(account.AccountId);
        accountLoggedIn = account;
    }

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

    public int UpdateMoney(int choice)
    {
        int result = 0;
        
        switch (choice)
        {
            case 1:
                GetAccountLoggedIn().Money += 75000;
                break;

            case 2:
                GetAccountLoggedIn().Money += 150000;
                break;

            case 3:
                GetAccountLoggedIn().Money += 375000;
                break;

            case 4:
                GetAccountLoggedIn().Money += 750000;
                break;

            case 5:
                GetAccountLoggedIn().Money += 1500000;
                break;
        }

        result = accountDAL.UpdateAccountMoney(GetAccountLoggedIn().AccountId, GetAccountLoggedIn().Money);
        return result;
    }

    public int UpdateMoney(int aid, double mn)
    {
        int result = 0;
        result = accountDAL.UpdateAccountMoney(GetAccountLoggedIn().AccountId, GetAccountLoggedIn().Money);
        return result;
    }
}