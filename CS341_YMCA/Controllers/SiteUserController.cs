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
        private readonly IHttpContextAccessor Con;
        private readonly LinkGenerator Links;

        private string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
        private bool IsDevelopment => Env.Equals("Development");

        public SiteUserController(Database Sql, EmailSender Smtp, IHttpContextAccessor Con, LinkGenerator Links)
        {
            this.Sql = Sql;
            this.Smtp = Smtp;
            this.Con = Con;
            this.Links = Links;
        }

        private void SendResetEmail(string Email, Guid ResetToken)
        {
            var Context = Con.HttpContext ?? throw new Exception("There is no active HTTP context.");
            var ResetLink = Links.GetUriByAction(
                Context, "ResetPasswordFlow", "SiteUser",
                new
                {
                    ResetToken
                });

            Smtp.SendEmail(null, Email, "Your account's password reset link",
                @"
                    <p>Thank you for showing interest in our service.</p>
                    <p>Please click on <a href='" + ResetLink + @"'>this link</a> to set a new password. If
                    the link does not work, try copying and pasting the whole URL below:</p>
                    <br/>

                    <p><a href='" + ResetLink + @"'>" + ResetLink + @"</a></p>
                ");
        }

        /**
         * User authentication endpoint to check credentials.
         */
        //[Route("/SiteUser/Authenticate")]
        public EndpointResultToken SiteUser_Authenticate(
            string Email,
            string PasswordHash
        )
        {
            EndpointResultToken Result = new();

            try
            {
                Sql.ExecuteProcedure<object>(
                    "SiteUser_Authenticate",
                    new SiteUserAuthenticateRequest()
                    {
                        Email = Email,
                        PasswordHash = PasswordHash
                    }, (_) => { });
            } catch (SqlException Ex)
            {
                Result.Success = false;
                Result.Error = Ex.Message;
            } catch (Exception Ex)
            {
                Result.Success = false;
                Result.Error = IsDevelopment ? Ex.Message : "An unexpected error has occurred.";
            }

            return Result;
        }

        /**
         * User registration endpoint to make member accounts. Does not expose
         * ability to create admin users.
         */
        //[Route("/SiteUser/Register")]
        public EndpointResultToken SiteUser_Register(
            string FirstName,
            string? LastName,
            string Email
        )
        {
            EndpointResultToken Result = new();

            try
            {
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
                        this.SendResetEmail(Email, Result.ResetToken);
                    });
            } catch (SqlException Ex)
            {
                Result.Success = false;
                Result.Error = Ex.Message;
            } catch (Exception Ex)
            {
                Result.Success = false;
                Result.Error = IsDevelopment ? Ex.Message : "An unexpected error has occurred.";
            }

            return Result;
        }

        /**
         * Endpoint which allows the user to request a password reset.
         */
        //[Route("/SiteUser/RequestReset")]
        public EndpointResultToken SiteUser_RequestReset(
            string Email
        )
        {
            EndpointResultToken Result = new();

            try
            {
                Sql.ExecuteProcedure<UserRequestResetResult>(
                    "SiteUser_RequestReset",
                    new UserRequestResetRequest()
                    {
                        Email = Email
                    }, (Result) =>
                    {
                        this.SendResetEmail(Email, Result.ResetToken);
                    });
            } catch (SqlException Ex)
            {
                Result.Success = false;
                Result.Error = Ex.Message;
            } catch (Exception Ex)
            {
                Result.Success = false;
                Result.Error = IsDevelopment ? Ex.Message : "An unexpected error has occurred.";
            }

            return Result;
        }

        /**
         * Endpoint which allows users to complete their password resets (by
         * clicking on a link sent via email).
         */
        //[Route("/SiteUser/ResetPassword")]
        public EndpointResultToken SiteUser_ResetPassword(
            Guid ResetToken,
            string PasswordHash
        )
        {
            EndpointResultToken Result = new();

            try
            {
                Sql.ExecuteProcedure<object>(
                    "SiteUser_ResetPassword",
                    new SiteUserResetPasswordRequest()
                    {
                        ResetToken = ResetToken,
                        PasswordHash = PasswordHash
                    }, (_) => { });
            } catch (SqlException Ex)
            {
                Result.Success = false;
                Result.Error = Ex.Message;
            } catch (Exception Ex)
            {
                Result.Success = false;
                Result.Error = IsDevelopment ? Ex.Message : "An unexpected error has occurred.";
            }

            return Result;
        }
    }
}
