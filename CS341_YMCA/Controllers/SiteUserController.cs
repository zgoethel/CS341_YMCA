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
        private readonly EmailSender Smtp;

        public SiteUserController(Database Sql, EmailSender Smtp)
        {
            this.Sql = Sql;
            this.Smtp = Smtp;
        }

        private void SendResetEmail(string Email, Guid Token)
        {
            var ResetLink = Request.Scheme.ToString()
                + "://"
                + Request.Host.ToString()
                + "/SiteUser/ResetPasswordFlow?ResetToken="
                + Token.ToString();

            Smtp.SendEmail(null, Email, "Your account's password reset link",
                @"
                    <p>Thank you for showing interest in our service.</p>
                    <p>Please click on <a href='" + ResetLink + @"'>this link</a> to set a new password. If
                    the link does not work, try copying and pasting the whole URL below:</p>
                    <br/><br/>

                    <p><a href='" + ResetLink + @"'>" + ResetLink + @"</a></p>
                ");
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
                Sql.ExecuteProcedure<object>(
                    "SiteUser_Authenticate",
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
                Sql.ExecuteProcedure<SiteUserRegisterResult>(
                    "SiteUser_Register",
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

                this.SendResetEmail(Email, _Result?.ResetToken ?? throw new Exception("Generated token not found for request"));

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
                Sql.ExecuteProcedure<UserRequestResetResult>(
                    "SiteUser_RequestReset",
                    new UserRequestResetRequest()
                    {
                        Email = Email
                    }, (Result) =>
                    {
                        _Result = Result;
                    });

                this.SendResetEmail(Email, _Result?.ResetToken ?? throw new Exception("Generated token not found for request"));

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
                Sql.ExecuteProcedure<object>(
                    "SiteUser_ResetPassword",
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
