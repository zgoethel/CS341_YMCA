using CS341_YMCA.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CS341_YMCA.Controllers
{
    /**
     * Provides API access to the internal class management, scheduling, and
     * browsing subsystem.
     */
    public class ClassController : Controller
    {
        private readonly Database Sql;

        private readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
        private bool IsDevelopment => Env.Equals("Development");

        public ClassController(Database Sql)
        {
            this.Sql = Sql;
        }

        /**
         * Allows creation and udpating of basic class data.
         */
        public EndpointResultToken<int> Class_Set(
            int? Id = null,
            string? ClassName = null,
            bool? AllowEnrollment = null,
            bool? Enabled = null,
            string? ShortDescription = null,
            string? LongDescription = null,
            string? PrereqIds = null
        )
        {
            EndpointResultToken<int> Result = new();

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
                        PrereqIds = PrereqIds
                    }, (_Result) =>
                    {
                        Result.Value = _Result.Id;
                    });
            }
            catch (SqlException Ex)
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
         * Lists classes according to provided filter parameters.
         */
        public EndpointResultToken<List<ClassDBO>> Class_List(
            string? NameFilter = null,
            bool? IncludeDisabled = null
        )
        {
            var Result = new EndpointResultToken<List<ClassDBO>>
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

        /**
         * Gets class data associated with an ID.
         */
        public EndpointResultToken<ClassDBO> Class_GetById(
            int Id
        )
        {
            EndpointResultToken<ClassDBO> Result = new();

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
    }
}
