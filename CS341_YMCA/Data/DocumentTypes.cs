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
        public DateTime? MemberThru { get; set; }
        public bool IsMember { get; set; }
    }

    public class SiteUserListRequest
    {
        public string? EmailFilter { get; set; } = null;
    }

    public class SiteUserSetRequest
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool? IsAdmin { get; set; }
        public DateTime? MemberThru { get; set; }
    }

    #endregion

    // Region bound to request and result parameters for classes
    #region ClassActionModels

    public class ClassSetRequest
    {
        public int? Id { get; set; }
        public string? ClassName { get; set; }
        public bool? AllowEnrollment { get; set; }
        public bool? Enabled { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string? PrereqIds { get; set; }
        public DateTime? MemberEnrollmentStart { get; set; }
        public int? MemberEnrollmentDays { get; set; }
        public bool? AllowNonMembers { get; set; } = false;
        public DateTime? NonMemberEnrollmentStart { get; set; }
        public int? NonMemberEnrollmentDays { get; set; }
        public float? MemberPrice { get; set; }
        public float? NonMemberPrice { get; set; }
    }

    public class ClassSetResult
    {
        public int Id { get; set; }
    }

    public class ClassListRequest
    {
        public string? NameFilter { get; set; } = null;
        public bool? IncludeDisabled { get; set; } = null;
    }

    public class ClassDBO
    {
        public int? Id { get; set; }
        public string ClassName { get; set; } = "New Class";
        public bool AllowEnrollment { get; set; } = false;
        public bool Enabled { get; set; } = true;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string ShortDescription { get; set; } = "";
        public string LongDescription { get; set; } = "";
        public string PrereqIds { get; set; } = "";
        public DateTime? MemberEnrollmentStart { get; set; }
        public int? MemberEnrollmentDays { get; set; }
        public bool AllowNonMembers { get; set; } = false;
        public DateTime? NonMemberEnrollmentStart { get; set; }
        public int? NonMemberEnrollmentDays { get; set; }
        public float MemberPrice { get; set; } = 0.0f;
        public float NonMemberPrice { get; set; } = 0.0f;
    }

    #endregion
}
