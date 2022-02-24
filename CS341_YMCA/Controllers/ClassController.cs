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
        private readonly EmailSender Smtp;
        private readonly IHttpContextAccessor Con;
        private readonly LinkGenerator Links;

        private string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
        private bool IsDevelopment => Env.Equals("Development");

        public ClassController(Database Sql, EmailSender Smtp, IHttpContextAccessor Con, LinkGenerator Links)
        {
            this.Sql = Sql;
            this.Smtp = Smtp;
            this.Con = Con;
            this.Links = Links;
        }

        /**
         * Allows creation and udpating of basic class data.
         */
        public int Class_Set(
            int? Id = null,
            string? ClassName = null,
            bool? AllowEnrollment = null,
            bool? Enabled = null
        )
        {
            int Result = new();

            Sql.ExecuteProcedure<ClassSetResult>(
                "Class_Set",
                new ClassSetRequest()
                {
                    Id = Id,
                    ClassName = ClassName,
                    AllowEnrollment = AllowEnrollment,
                    Enabled = Enabled
                }, (_Result) =>
                {
                    Result = _Result.Id;
                });

            return Result;
        }

        /**
         * Lists classes according to provided filter parameters.
         */
        public List<ClassDBO> Class_List(
            string? NameFilter = null,
            bool? IncludeDisabled = null,
            int? Top = null,
            int? Skip = null
        )
        {
            var Result = new List<ClassDBO>();

            Sql.ExecuteProcedure<ClassDBO>(
                "Class_List",
                new ClassListRequest()
                {
                    NameFilter = NameFilter,
                    IncludeDisabled = IncludeDisabled,
                    Top = Top,
                    Skip = Skip
                }, (_Result) =>
                {
                    Result.Add(_Result);
                });

            return Result;
        }
    }
}
