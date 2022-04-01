using CS341_YMCA.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CS341_YMCA.Services;

/**
 * Provides access to the internal account and authentication
 * subsystems within the database.
 */
public class SiteUserRepository : Controller
{
    private readonly LinkGenerator Links;
    private readonly IHttpContextAccessor Con;
    private readonly Database Sql;
    private readonly EmailSender Smtp;

    private readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
    private bool IsDevelopment => Env.Equals("Development");

    public SiteUserRepository(Database Sql, EmailSender Smtp, IHttpContextAccessor Con, LinkGenerator Links)
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
    public ResultToken<object> SiteUser_Authenticate(
        string Email,
        string PasswordHash
    )
    {
        ResultToken<object> Result = new();

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
    public ResultToken<object> SiteUser_Register(
        string FirstName,
        string? LastName,
        string Email
    )
    {
        ResultToken<object> Result = new();

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
    public ResultToken<object> SiteUser_RequestReset(
        string Email
    )
    {
        ResultToken<object> Result = new();

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
    public ResultToken<object> SiteUser_ResetPassword(
        Guid ResetToken,
        string PasswordHash
    )
    {
        ResultToken<object> Result = new();

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
     * Lists users according to provided filters.
     */
    public ResultToken<List<SiteUserDBO>> SiteUser_List(
        string? EmailFilter = null
    )
    {
        var Result = new ResultToken<List<SiteUserDBO>>
        {
            Value = new()
        };

        try
        {
            Sql.ExecuteProcedure<SiteUserDBO>(
                "SiteUser_List",
                new SiteUserListRequest()
                {
                    EmailFilter = EmailFilter,
                }, (_Result) =>
                {
                    Result.Value.Add(_Result);
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
     * Gets user data associated with an email.
     */
    public ResultToken<SiteUserDBO> SiteUser_GetByEmail(
        string Email
    )
    {
        ResultToken<SiteUserDBO> Result = new();

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
    public ResultToken<SiteUserDBO> SiteUser_GetById(
        int Id
    )
    {
        ResultToken<SiteUserDBO> Result = new();

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

    public ResultToken<object> SiteUser_Set(
        int Id,
        string? FirstName = null,
        string? LastName = null,
        string? Email = null,
        bool? IsAdmin = null,
        DateTime? MemberThru = null
    )
    {
        ResultToken<object> Result = new();

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
                }, (_) => {  });

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

    /**
     * Deletes the user with specified ID from the SiteUsers table.
     */
    public ResultToken<object> SiteUser_DeleteById(
        int Id
    )
    {
        ResultToken<object> Result = new();
        Result.Value = new();

        try
        {
            Sql.ExecuteProcedure<SiteUserDeleteRequest>(
                "SiteUser_DeleteById",
                new SiteUserDeleteRequest()
                {
                    Id = Id
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
     * Enters a payment into the database, returning the ID with which it was stored.
     */
    public ResultToken<int> SiteUserPayments_Enter(
        int UserId,
        decimal Amount,
        string CardNumber,
        int SecurityCode,
        int PostalCode,
        string HolderName
    )
    {
        ResultToken<int> Result = new();

        try
        {
            Sql.ExecuteProcedure<UserPaymentEnterResult>(
                "SiteUserPayments_Enter",
                new EnterUserPaymentRequest()
                {
                    UserId = UserId,
                    Amount = Amount,
                    CardNumber = CardNumber,
                    SecurityCode = SecurityCode,
                    PostalCode = PostalCode,
                    HolderName = HolderName
                }, (_Result) =>
                {
                    Result.Value = _Result.Id;
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

    public ResultToken<UserPaymentDBO> SiteUserPayments_GetById(
        int Id
    )
    {
        ResultToken<UserPaymentDBO> Result = new();

        try
        {
            Sql.ExecuteProcedure<UserPaymentDBO>(
                "SiteUserPayments_GetById",
                new { Id },
                (_Result) =>
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

    public ResultToken<List<UserPaymentDBO>> SiteUserPayments_GetByUserId(
        int UserId
    )
    {
        ResultToken<List<UserPaymentDBO>> Result = new();
        Result.Value = new();

        try
        {
            Sql.ExecuteProcedure<UserPaymentDBO>(
                "SiteUserPayments_GetByUserId",
                new { UserId },
                (_Result) =>
                {
                    Result.Value.Add(_Result);
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

    public ResultToken<List<UserPaymentDBO>> SiteUserPayments_List()
    {
        ResultToken<List<UserPaymentDBO>> Result = new();
        Result.Value = new();

        try
        {
            Sql.ExecuteProcedure<UserPaymentDBO>(
                "SiteUserPayments_List",
                new {  },
                (_Result) =>
                {
                    Result.Value.Add(_Result);
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
}
