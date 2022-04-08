using CS341_YMCA.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CS341_YMCA.Services;

/// <summary>
/// Provides access to the internal class management, scheduling, and browsing
/// subsystems within the database.
/// </summary>
public class ClassRepository : Controller
{
    private readonly Database Sql;

    private readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
    private bool IsDevelopment => Env.Equals("Development");

    public ClassRepository(Database Sql)
    {
        this.Sql = Sql;
    }

    /// <summary>
    /// Allows creation and udpating of basic class data.
    /// </summary>
    public ResultToken<int> Class_Set(
        int? Id = null,
        string? ClassName = null,
        bool? AllowEnrollment = null,
        bool? Enabled = null,
        string? ShortDescription = null,
        string? LongDescription = null,
        DateTime? MemberEnrollmentStart = null,
        int? MemberEnrollmentDays = null,
        DateTime? NonMemberEnrollmentStart = null,
        int? NonMemberEnrollmentDays = null,
        bool? AllowNonMembers = null,
        decimal? MemberPrice = null,
        decimal? NonMemberPrice = null,
        string? Location = null,
        int? MaxSeats = null,
        string? FulfillCsv = null,
        string? RequireCsv = null)
    {
        ResultToken<int> Result = new();

        try
        {
            Sql.ExecuteProcedure<ClassSetResult>(
                "Class_Set",
                new ClassSetRequest()
                {
                    Id = Id,
                    ClassName = ClassName,
                    AllowEnrollment = AllowEnrollment,
                    Enabled = Enabled,
                    ShortDescription = ShortDescription,
                    LongDescription = LongDescription,
                    MemberEnrollmentStart = MemberEnrollmentStart,
                    MemberEnrollmentDays = MemberEnrollmentDays,
                    NonMemberEnrollmentStart = NonMemberEnrollmentStart,
                    NonMemberEnrollmentDays = NonMemberEnrollmentDays,
                    AllowNonMembers = AllowNonMembers,
                    MemberPrice = MemberPrice,
                    NonMemberPrice = NonMemberPrice,
                    Location = Location,
                    MaxSeats = MaxSeats,
                    FulfillCsv = FulfillCsv,
                    RequireCsv = RequireCsv
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

    /// <summary>
    /// Lists classes according to provided filter parameters.
    /// </summary>
    public ResultToken<List<ClassDBO>> Class_List(
        string? NameFilter = null,
        bool? IncludeDisabled = null)
    {
        var Result = new ResultToken<List<ClassDBO>>
        {
            Value = new()
        };

        try
        {
            Sql.ExecuteProcedure<ClassDBO>(
                "Class_List",
                new ClassListRequest()
                {
                    NameFilter = NameFilter,
                    IncludeDisabled = IncludeDisabled
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

    /// <summary>
    /// Gets class data associated with an ID.
    /// </summary>
    public ResultToken<ClassDBO> Class_GetById(
        int Id)
    {
        ResultToken<ClassDBO> Result = new();

        try
        {
            Sql.ExecuteProcedure<ClassDBO>(
                "Class_GetById",
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
                Result.Error = "Record with given ID not found";
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

    /// <summary>
    /// Gets class data associated with IDs as CSV.
    /// </summary>
    public ResultToken<List<ClassDBO>> Class_GetByIds(
        string Csv)
    {
        ResultToken<List<ClassDBO>> Result = new();
        Result.Value = new();

        try
        {
            Sql.ExecuteProcedure<ClassDBO>(
                "Class_GetByIds",
                new
                {
                    Csv = Csv ?? ""
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

    /// <summary>
    /// Lists a class' schedule sessions according to class ID.
    /// </summary>
    public ResultToken<List<ClassScheduleDBO>> ClassSchedule_List(
        int ClassId)
    {
        var Result = new ResultToken<List<ClassScheduleDBO>>
        {
            Value = new()
        };

        try
        {
            Sql.ExecuteProcedure<ClassScheduleDBO>(
                "ClassSchedule_List",
                new
                {
                    ClassId
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

    /// <summary>
    /// Allows creation and udpating of class schedule data.
    /// </summary>
    public ResultToken<int> ClassSchedule_Set(
        int? Id = null,
        int? ClassId = null,
        DateTime? FirstDate = null,
        int? Recurrence = null,
        int? Occurrences = null,
        int? Duration = null)
    {
        ResultToken<int> Result = new();

        try
        {
            Sql.ExecuteProcedure<ClassScheduleSetResult>(
                "ClassSchedule_Set",
                new ClassScheduleSetRequest()
                {
                    Id = Id,
                    ClassId = ClassId,
                    FirstDate = FirstDate,
                    Recurrence = Recurrence,
                    Occurrences = Occurrences,
                    Duration = Duration
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

    /// <summary>
    /// Deletes the class with specified ID from the ClassMain table.
    /// </summary>
    public ResultToken<object> Class_DeleteById(
        int Id)
    {
        ResultToken<object> Result = new();

        try
        {
            Sql.ExecuteProcedure<object>(
                "Class_DeleteById",
                new ClassDeleteRequest()
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

    /// <summary>
    /// Gets list of all distinct existing prereq codes on classes.
    /// </summary>
    public ResultToken<List<string>> Class_ListReqs()
    {
        ResultToken<List<string>> Result = new();
        Result.Value = new();

        try
        {
            Sql.ExecuteProcedure<ClassListReqResult>(
                "Class_ListReqs",
                new {  },
                (_Result) =>
                {
                    Result.Value.Add(_Result.Value);
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

    /// <summary>
    /// Drops the specified user from the specified class.
    /// </summary>
    public ResultToken<object> Class_DropUser(
        int ClassId,
        int UserId)
    {
        ResultToken<object> Result = new();

        try
        {
            Sql.ExecuteProcedure<object>(
                "Class_DropUser",
                new ClassDropUserRequest()
                {
                    ClassId = ClassId,
                    UserId = UserId
                },
                (_Result) => {  });
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

    /// <summary>
    /// Enrolls the specified user in the specified class, possibly with a payment.
    /// </summary>
    public ResultToken<object> Class_EnrollUser(
        int ClassId,
        int UserId,
        int? PaymentId = null)
    {
        ResultToken<object> Result = new();

        try
        {
            Sql.ExecuteProcedure<object>(
                "Class_EnrollUser",
                new ClassEnrollUserRequest()
                {
                    ClassId = ClassId,
                    UserId = UserId,
                    PaymentId = PaymentId
                },
                (_Result) => {  });
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

    /// <summary>
    /// Retrieve all active enrollment records for a single class.
    /// </summary>
    public ResultToken<List<ClassEnrollmentDBO>> ClassEnrollment_GetByClassId(
        int ClassId)
    {
        ResultToken<List<ClassEnrollmentDBO>> Result = new();
        Result.Value = new();

        try
        {
            Sql.ExecuteProcedure<ClassEnrollmentDBO>(
                "ClassEnrollment_GetByClassId",
                new { ClassId },
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

    /// <summary>
    /// Retrieves the set of enrollment history associated to a specific user.
    /// </summary>
    public ResultToken<List<ClassEnrollmentDBO>> ClassEnrollment_GetByUserId(
        int UserId)
    {
        ResultToken<List<ClassEnrollmentDBO>> Result = new();
        Result.Value = new();

        try
        {
            Sql.ExecuteProcedure<ClassEnrollmentDBO>(
                "ClassEnrollment_GetByUserId",
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

    /// <summary>
    /// Gets all schedule sessions for all classes in which the user is enrolled.
    /// </summary>
    public ResultToken<List<ClassScheduleDBO>> ClassSchedule_GetByUserId(
        int UserId)
    {
        ResultToken<List<ClassScheduleDBO>> Result = new();
        Result.Value = new();

        try
        {
            Sql.ExecuteProcedure<ClassScheduleDBO>(
                "ClassSchedule_GetByUserId",
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

    /// <summary>
    /// Deletes all class sessions with IDs in the CSV.
    /// </summary>
    public ResultToken<object> ClassSchedule_DeleteByIds(
        string IdCsv)
    {
        ResultToken<object> Result = new();

        try
        {
            Sql.ExecuteProcedure<object>(
                "ClassSchedule_DeleteByIds",
                new { IdCsv },
                (_Result) => {  });
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

    /// <summary>
    /// Calculates user-specific class details based on enrollment and class
    /// schedule data.
    /// </summary>
    public ResultToken<ClassCalculateDetailsResult> Class_CalculateDetails(
        int ClassId,
        int UserId)
    {
        ResultToken<ClassCalculateDetailsResult> Result = new();

        try
        {
            Sql.ExecuteProcedure<ClassCalculateDetailsResult>(
                "Class_CalculateDetails",
                new ClassCalculateDetailsRequest()
                {
                    ClassId = ClassId,
                    UserId = UserId
                }, (_Result) => {
                    Result.Value = _Result;
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
