namespace CS341_YMCA.Data
{
    // Models bound to the fields required by user SQL procedures
    #region SiteUserActionModels

    public class SiteUserAuthenticateRequest
    {
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
    }

    public class SiteUserRegisterRequest
    {
        public string FirstName { get; set; } = "";
        public string? LastName { get; set; }
        public string Email { get; set; } = "";
        public bool IsAdmin { get; set; } = false;
    }

    public class SiteUserRegisterResult
    {
        public int Id { get; set; }
        public Guid ResetToken { get; set; }
    }

    public class UserRequestResetRequest
    {
        public string Email { get; set; } = "";
    }

    public class UserRequestResetResult
    {
        public Guid ResetToken { get; set; }
    }

    public class SiteUserResetPasswordRequest
    {
        public Guid ResetToken { get; set; }
        public string PasswordHash { get; set; } = "";
    }

    public class SiteUserDBO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string? LastName { get; set; }
        public string Email { get; set; } = "";
        public bool HasPassword { get; set; }
        public bool HasPendingReset { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    #endregion
}
