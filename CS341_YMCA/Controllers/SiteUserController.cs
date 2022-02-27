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
        private readonly LinkGenerator Links;
        private readonly IHttpContextAccessor Con;
        private readonly Database Sql;
        private readonly EmailSender Smtp;

        private readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
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
        public EndpointResultToken<object> SiteUser_Authenticate(
            string Email,
            string PasswordHash
        )
        {
            EndpointResultToken<object> Result = new();

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
        public EndpointResultToken<object> SiteUser_Register(
            string FirstName,
            string? LastName,
            string Email
        )
        {
            EndpointResultToken<object> Result = new();

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
        public EndpointResultToken<object> SiteUser_RequestReset(
            string Email
        )
        {
            EndpointResultToken<object> Result = new();

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
        public EndpointResultToken<object> SiteUser_ResetPassword(
            Guid ResetToken,
            string PasswordHash
        )
        {
            EndpointResultToken<object> Result = new();

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

        /**
         * Gets user data associated with an email.
         */
        public EndpointResultToken<SiteUserDBO> SiteUser_GetByEmail(
            string Email
        )
        {
            EndpointResultToken<SiteUserDBO> Result = new();

            try
            {
                Sql.ExecuteProcedure<SiteUserDBO>(
                    "SiteUser_GetByEmail",
                    new
                    {
                        Email
                    }, (_Result) =>
                    {
                        Result.Value = _Result;
                    });

                if (Result.Value == null)
                {
                    Result.Success = false;
                    Result.Error = "Record with given ID not found.";
                }
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
         * Gets user data associated with an ID.
         */
        public EndpointResultToken<SiteUserDBO> SiteUser_GetById(
            int Id
        )
        {
            EndpointResultToken<SiteUserDBO> Result = new();

            try
            {
                Sql.ExecuteProcedure<SiteUserDBO>(
                    "SiteUser_GetById",
                    new
                    {
                        Id
                    }, (_Result) =>
                    {
                        Result.Value = _Result;
                    });

                if (Result.Value == null)
                {
                    Result.Success = false;
                    Result.Error = "Record with given ID not found.";
                }
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

        public EndpointResultToken<object> SiteUser_Set(
            int Id,
            string? FirstName = null,
            string? LastName = null,
            string? Email = null,
            bool? IsAdmin = null,
            DateTime? MemberThru = null
        )
        {
            EndpointResultToken<object> Result = new();

            try
            {
                Sql.ExecuteProcedure<object>(
                    "SiteUser_Set",
                    new SiteUserSetRequest()
                    {
                        Id = Id,
                        FirstName = FirstName,
                        LastName = LastName,
                        Email = Email,
                        IsAdmin = IsAdmin,
                        MemberThru = MemberThru
                    }, (_) => { });

                Result.Success = true;
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
