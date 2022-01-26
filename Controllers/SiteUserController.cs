using CS341_YMCA.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CS341_YMCA.Controllers
{
    /**
     * Provides API access to the internal account and authentication
     * subsystems within the database.
     */
    public class SiteUserController : Controller
    {
        private readonly Database Sql;

        public SiteUserController(Database Sql)
        {
            this.Sql = Sql;
        }

        /**
         * User authentication endpoint to check credentials.
         */
        [Route("/SiteUser/Authenticate")]
        public IActionResult SiteUser_Authenticate(
            string Email,
            string PasswordHash
        )
        {
            try
            {
                Sql.ExecuteProcedure<object>("SiteUser_Authenticate",
                    new SiteUserAuthenticateRequest()
                    {
                        Email = Email,
                        PasswordHash = PasswordHash
                    }, (_) => { });

                return Json(new
                {
                    Success = true
                });
            } catch (SqlException Ex)
            {
                return Json(new
                {
                    Success = false,
                    Error = Ex.Message
                });
            }
        }

        /**
         * User registration endpoint to make member accounts. Does not expose
         * ability to create admin users.
         */
        [Route("/SiteUser/Register")]
        public IActionResult SiteUser_Register(
            string FirstName,
            string? LastName,
            string Email
        )
        {
            try
            {
                SiteUserRegisterResult? _Result = null;
                Sql.ExecuteProcedure<SiteUserRegisterResult>("SiteUser_Register",
                    new SiteUserRegisterRequest()
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        Email = Email,
                        IsAdmin = false
                    }, (Result) =>
                    {
                        _Result = Result;
                    });

                //TODO Send reset email to user

                return Json(new
                {
                    Success = true
                });
            } catch (SqlException Ex)
            {
                return Json(new
                {
                    Success = false,
                    Error = Ex.Message
                });
            }
        }

        /**
         * Endpoint which allows the user to request a password reset.
         */
        [Route("/SiteUser/RequestReset")]
        public IActionResult SiteUser_RequestReset(
            string Email
        )
        {
            try
            {
                UserRequestResetResult? _Result = null;
                Sql.ExecuteProcedure<UserRequestResetResult>("SiteUser_RequestReset",
                    new UserRequestResetRequest()
                    {
                        Email = Email
                    }, (Result) =>
                    {
                        _Result = Result;
                    });

                //TODO Send reset email to user

                return Json(new
                {
                    Success = true
                });
            } catch (SqlException Ex)
            {
                return Json(new
                {
                    Success = false,
                    Error = Ex.Message
                });
            }
        }

        /**
         * Endpoint which allows users to complete their password resets (by
         * clicking on a link sent via email).
         */
        [Route("/SiteUser/ResetPassword")]
        public IActionResult SiteUser_ResetPassword(
            Guid ResetToken,
            string PasswordHash
        )
        {
            try
            {
                Sql.ExecuteProcedure<object>("SiteUser_ResetPassword",
                    new SiteUserResetPasswordRequest()
                    {
                        ResetToken = ResetToken,
                        PasswordHash = PasswordHash
                    }, (_) => { });

                return Json(new
                {
                    Success = true
                });
            } catch (SqlException Ex)
            {
                return Json(new
                {
                    Success = false,
                    Error = Ex.Message
                });
            }
        }
    }
}
