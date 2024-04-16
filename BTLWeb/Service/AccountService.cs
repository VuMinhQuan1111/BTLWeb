using BTLWeb.Models;

namespace BTLWeb.Service
{
    public class AccountService
    {
        private static AccountService? _instance = null;

        private AccountService()
        {

        }

        public static AccountService Instance()
        {
            return _instance ??= new AccountService();
        }

        
    }
}
