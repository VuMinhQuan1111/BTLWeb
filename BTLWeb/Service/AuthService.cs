using BTLWeb.Models;
using BTLWeb.Models.ModelsView;
using Microsoft.EntityFrameworkCore;
namespace BTLWeb.Service
{
    public class AuthService
    {
        
        public static bool CreateSession(HttpContext httpContext, MV_SignIn vm)
        {
            BtlwebContext db = new BtlwebContext();
            TblUser? tbluser = db.TblUsers.Where(u => u.UsersName == vm.UsersName && u.UsersPass == vm.UsersPass).FirstOrDefault();
            if(tbluser != null)
            {
                httpContext.Session.SetInt32("UsersId", tbluser.UsersId);
                return true;
            }
            return false;
        }

        public static bool CreateUser(MV_Register vm)
        {
            BtlwebContext db = new BtlwebContext();
            if (db.TblUsers.Where(u => u.UsersEmail == vm.UsersEmail).FirstOrDefault() == null)
            {
                TblUser tblUser = new TblUser
                {
                    UsersName = vm.UsersName,
                    UsersEmail = vm.UsersEmail,
                    UsersPass = vm.UsersPass,
                    UsersRole = "Client",
                };
                db.TblUsers.Add(tblUser);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static void Logout(HttpContext httpContext)
        {
            httpContext.Session.Clear();
            httpContext.Session.Remove("UsersId");
        }

        public static bool IsAuthenticated(HttpContext httpContext)
        {
            if(httpContext.Session.GetInt32("UsersId") != null)
            {
                return true;
            }
            return false;
        }
        

        public static TblUser? GetAuthenticatedUser(HttpContext httpContext)
        {
            BtlwebContext db = new BtlwebContext();
            int usersId = httpContext.Session.GetInt32("UsersId") ?? -1;
            if(usersId > 0)
            {
                return db.TblUsers.Where(u => u.UsersId == usersId).FirstOrDefault();
            }
            return null;
        }
    }
}
