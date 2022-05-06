/**
 * Collection of data structures used throughout the application to interface
 * with the database and make requests.
 * @author Zach Goethel
 * @author Matthew Krings
 * @author Cristoph Meyer
 * @author Tyler Vernezze
 */

namespace CS341_YMCA.Helpers;

// Models bound to the fields required by user SQL procedures
#region SiteUserActionModels

/// <summary>
/// Schema for attempting to check credentials' validity.
/// </summary>
public class SiteUserAuthenticateRequest
{
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
}

/// <summary>
/// Schema for registering a new account.
/// </summary>
public class SiteUserRegisterRequest
{
    public string FirstName { get; set; } = "";
    public string? LastName { get; set; }
    public string Email { get; set; } = "";
    public bool IsAdmin { get; set; } = false;
    public bool Enabled { get; set; } = true;
}

/// <summary>
/// Schema for results after regering an account.
/// </summary>
public class SiteUserRegisterResult
{
    public int Id { get; set; }
    public Guid ResetToken { get; set; }
}

/// <summary>
/// Schema for requesting a password reset by email.
/// </summary>
public class UserRequestResetRequest
{
    public string Email { get; set; } = "";
}

/// <summary>
/// Schema for result after requesting a password reset.
/// </summary>
public class UserRequestResetResult
{
    public Guid ResetToken { get; set; }
}

/// <summary>
/// Schema for completing the password reset with a new password.
/// </summary>
public class SiteUserResetPasswordRequest
{
    public Guid ResetToken { get; set; }
    public string PasswordHash { get; set; } = "";
}

/// <summary>
/// Schema of a single user retrieved from the database.
/// </summary>
public class SiteUserDBO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "Guest";
    public string? LastName { get; set; }
    public string Email { get; set; } = "";
    public bool HasPassword { get; set; }
    public bool HasPendingReset { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public DateTime? MemberThru { get; set; }
    public bool IsMember { get; set; }
    public string FulfilledCsv { get; set; } = "";
    public bool Enabled { get; set; }
}

/// <summary>
/// Schema of input parameters for site user listing.
/// </summary>
public class SiteUserListRequest
{
    public string? NameFilter { get; set; } = null;
    public string? EmailFilter { get; set; } = null;
}

/// <summary>
/// Schema for a request to modify a site user record.
/// </summary>
public class SiteUserSetRequest
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public bool? IsAdmin { get; set; }
    public DateTime? MemberThru { get; set; }
    public string? FulfilledCsv { get; set; }
    public bool? Enabled { get; set; }
}

/// <summary>
/// Schema for a request to delete a site user and related records.
/// </summary>
public class SiteUserDeleteRequest
{
    public int Id { get; set; }
}

/// <summary>
/// Schema for a request to enter payments into the database.
/// </summary>
public class EnterUserPaymentRequest
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string CardNumber { get; set; } = "";
    public int SecurityCode { get; set; }
    public int PostalCode { get; set; }
    public string HolderName { get; set; } = "";
    public DateTime CardExpiry { get; set; }
}

/// <summary>
/// Schema for a single payment retrieved from the database.
/// </summary>
public class UserPaymentDBO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string CardNumber { get; set; } = "";
    public int SecurityCode { get; set; }
    public DateTime CardExpiry { get; set; }
    public int PostalCode { get; set; }
    public string HolderName { get; set; } = "";
    public DateTime Paid { get; set; }
    public string? Item { get; set; }
}

/// <summary>
/// Schema of the result of entering a new payment record.
/// </summary>
public class UserPaymentEnterResult
{
    public int Id { get; set; }
}

#endregion

// Region bound to request and result parameters for classes
#region ClassActionModels

/// <summary>
/// Schema of a request to modify a class detail record.
/// </summary>
public class ClassSetRequest
{
    public int? Id { get; set; }
    public string? ClassName { get; set; }
    public bool? AllowEnrollment { get; set; }
    public bool? Enabled { get; set; }
    public string? ShortDescription { get; set; }
    public string? LongDescription { get; set; }
    public DateTime? MemberEnrollmentStart { get; set; }
    public int? MemberEnrollmentDays { get; set; }
    public bool? AllowNonMembers { get; set; } = false;
    public DateTime? NonMemberEnrollmentStart { get; set; }
    public int? NonMemberEnrollmentDays { get; set; }
    public decimal? MemberPrice { get; set; }
    public decimal? NonMemberPrice { get; set; }
    public string? Location { get; set; }
    public int? MaxSeats { get; set; }
    public string? FulfillCsv { get; set; }
    public string? RequireCsv { get; set; }
    public int? ClassThumbId { get; set; }
    public int? ClassPhotoId { get; set; }
}

/// <summary>
/// Schema of the result of creating or modifying a class.
/// </summary>
public class ClassSetResult
{
    public int Id { get; set; }
}

/// <summary>
/// Schema of possible filters for a class listing query.
/// </summary>
public class ClassListRequest
{
    public string? NameFilter { get; set; } = null;
    public bool? IncludeDisabled { get; set; } = null;
}

/// <summary>
/// Schema of a single class retrieved from the database.
/// </summary>
public class ClassDBO
{
    public int Id { get; set; }
    public string ClassName { get; set; } = "New Class";
    public bool AllowEnrollment { get; set; } = false;
    public bool Enabled { get; set; } = true;
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public string ShortDescription { get; set; } = "";
    public string LongDescription { get; set; } = "";
    public DateTime? MemberEnrollmentStart { get; set; }
    public int? MemberEnrollmentDays { get; set; }
    public bool AllowNonMembers { get; set; } = false;
    public DateTime? NonMemberEnrollmentStart { get; set; }
    public int? NonMemberEnrollmentDays { get; set; }
    public decimal MemberPrice { get; set; } = 0.0m;
    public decimal NonMemberPrice { get; set; } = 0.0m;
    public string? Location { get; set; }
    public int? MaxSeats { get; set; }
    public bool MemberEnrollmentOpen { get; set; }
    public bool NonMemberEnrollmentOpen { get; set; }
    public string FulfillCsv { get; set; } = "";
    public string RequireCsv { get; set; } = "";
    public int SeatsTaken { get; set; }
    public int? ClassThumbId { get; set; }
    public int? ClassPhotoId { get; set; }
    public DateTime? CanceledDate { get; set; }
}

/// <summary>
/// Schema of a single class schedule session record.
/// </summary>
public class ClassScheduleDBO
{
    public int? Id { get; set; }
    public int ClassId { get; set; }
    public DateTime FirstDate { get; set; } = DateTime.Now;
    public int Recurrence { get; set; }
    public int Duration { get; set; }
    public int Occurrences { get; set; }
    public string ClassName { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public DateTime? CanceledDate { get; set; }
}

/// <summary>
/// Schema of a request to modify a class schedule session.
/// </summary>
public class ClassScheduleSetRequest
{
    public int? Id { get; set; }
    public int? ClassId { get; set; }
    public DateTime? FirstDate { get; set; }
    public int? Recurrence { get; set; }
    public int? Occurrences { get; set; }
    public int? Duration { get; set; }
}

/// <summary>
/// Schema of the reuslt of modifying a class schedule session.
/// </summary>
public class ClassScheduleSetResult
{
    public int Id { get; set; }
}

/// <summary>
/// Schema of a request to delete a class and related details.
/// </summary>
public class ClassDeleteRequest
{
    public int Id { get; set; }
}

/// <summary>
/// Schema of the result of retrieving all class requirement codes.
/// </summary>
public class ClassListReqResult
{
    public string Value { get; set; } = "";
}

/// <summary>
/// Schema of a request to drop a user from a class.
/// </summary>
public class ClassDropUserRequest
{
    public int UserId { get; set; }
    public int ClassId { get; set; }
}

/// <summary>
/// Schema of a request to enroll a user in a class.
/// </summary>
public class ClassEnrollUserRequest
{
    public int UserId { get; set; }
    public int ClassId { get; set; }
    public int? PaymentId { get; set; }
}

/// <summary>
/// Schema of a single class enrollment record in the database.
/// </summary>
public class ClassEnrollmentDBO
{
    public int Id { get; set; }
    public int ClassId { get; set; }
    public int UserId { get; set; }
    public int? PaymentId { get; set; }
    public DateTime EnrolledDate { get; set; }
    public bool? PassedYN { get; set; }
    public string ClassName { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string? LastName { get; set; }
    public string Email { get; set; } = "";
    public bool IsMember { get; set; }
    public DateTime? CanceledDate { get; set; }
    public bool UserEnabled { get; set; }
}

/// <summary>
/// Schema for a request to calculate user-specific class values.
/// </summary>
public class ClassCalculateDetailsRequest
{
    public int ClassId { get; set; }
    public int UserId { get; set; }
}

/// <summary>
/// Schema of user-specific calculation results for class.
/// </summary>
public class ClassCalculateDetailsResult
{
    public bool IsEnrolled { get; set; }
    public decimal ThisUserCost { get; set; }
    public bool CanEnroll { get; set; }
    public bool OpenForUser { get; set; }
    public bool UnlimitedSeats { get; set; }
    public bool ClosedForUser { get; set; }
    public DateTime? EnrollmentOpen { get; set; }
    public DateTime? EnrollmentClose { get; set; }
}

#endregion

// Region bound to request and result parameters for file storage
#region FileStorageActionModels

/// <summary>
/// Schema for a request to record an uploaded file.
/// </summary>
/// 
public class FileStorageEnterRequest
{
    public string StoredName { get; set; } = "";
    public string OriginalName { get; set; } = "";
    public int SizeBytes { get; set; }
    public string MimeType { get; set; } = "";
    public int? UploadedBy { get; set; }
}

/// <summary>
/// Schema for a result after storing an uploaded file.
/// </summary>
public class FileStorageEnterResult
{
    public int Id { get; set; }
}

/// <summary>
/// Schema of a file storage record in the database.
/// </summary>
public class FileStorageDBO
{
    public int Id { get; set; }
    public string StoredName { get; set; } = "";
    public string OriginalName { get; set; } = "";
    public int SizeBytes { get; set; }
    public string MimeType { get; set; } = "";
    public DateTime Uploaded { get; set; }
    public int? UploadedBy { get; set; }
}

#endregion