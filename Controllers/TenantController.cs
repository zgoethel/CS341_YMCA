using DomainIdentity.Data;
using Microsoft.AspNetCore.Mvc;

namespace CS341_YMCA.Controllers
{
    public class UserController : Controller
    {
        private readonly Database Sql;

        public UserController(Database Sql)
        {
            this.Sql = Sql;
        }

        [Route("/SiteUser/Authenticate")]
        public IActionResult SiteUser_Authenticate(string Email, string PasswordHash)
        {
            Sql.ExecuteProcedure<object>("SiteUser", new
            {
                Email,
                PasswordHash
            }, (_) => {  });

            return Json(new
            {
                Success = true
            });
        }
    }
}
