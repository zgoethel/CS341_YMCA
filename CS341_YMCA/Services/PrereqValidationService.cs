namespace CS341_YMCA.Services;

using System.Collections.Generic;

/// <summary>
/// Set of actions to check and modify a user's fulfilled prereq codes.
/// </summary>
public class PrereqValidationService
{
    private readonly DatabaseService sql;
    private readonly SiteUserRepository siteUsers;
    private readonly ClassRepository classes;

    public PrereqValidationService(DatabaseService sql,
        SiteUserRepository siteUsers,
        ClassRepository classes)
    {
        this.sql = sql;
        this.siteUsers = siteUsers;
        this.classes = classes;
    }

    /// <summary>
    /// Enters a grade into the database on the enrollment record for the
    /// correct user and class.
    /// </summary>
    /// <param name="userId">User which will receive the grade.</param>
    /// <param name="classId">Class which the grade is for.</param>
    /// <param name="result">Class grade, true for pass and false for fail.</param>
    private void MarkResult(int userId, int classId, bool result)
    {
        sql.ExecuteProcedure<object>(
            "ClassEnrollment_MarkResult",
            new { userId, classId, result },
            (_) => {  });
    }

    /// <summary>
    /// Marks the user's enrollment in a class as grade "failed."
    /// </summary>
    /// <param name="userId">User which will receive the grade.</param>
    /// <param name="classId">Class which the grade is for.</param>
    public void MarkFailed(int userId, int classId)
    {
        MarkResult(userId, classId, false);
    }

    /// <summary>
    /// Marks the user's enrollment in a class as grade "passed." Grants the
    /// user all prereq codes which the class "fulfills."
    /// </summary>
    /// <param name="userId">User which will receive the grade.</param>
    /// <param name="classId">Class which the grade is for.</param>
    public void MarkPassed(int userId, int classId)
    {
        var course = classes.Class_GetById(classId).Get()!;
        if (!string.IsNullOrEmpty(course.FulfillCsv))
            GrantPrereqs(userId, course.FulfillCsv.Split(",").ToList());

        MarkResult(userId, classId, true);
    }

    /// <summary>
    /// Appends the fulfilled prereqs to the existing fulfilled prereqs of the
    /// specified user.
    /// </summary>
    /// <param name="userId">Account receiving the fulfilled codes.</param>
    /// <param name="prereqs">Collection of prereq codes to grant.</param>
    public void GrantPrereqs(int userId, List<string> prereqs)
    {
        var user = siteUsers.SiteUser_GetById(userId).Get()!;
        var list = Array.FindAll(user.FulfilledCsv.Split(","), (it) => !string.IsNullOrEmpty(it))
            .ToHashSet()
            .Union(prereqs)
            .ToList();
        user.FulfilledCsv = string.Join(",", list);

        siteUsers.SiteUser_Set(Id: userId, FulfilledCsv: user.FulfilledCsv).Get();
    }

    /// <param name="userId">Account to check for fulfilled codes.</param>
    /// <param name="prereqs">Collection of prereq codes to check.</param>
    /// <returns>Fulfillment status of the user, true if met and false if not.</returns>
    public bool HasPrereqs(int userId, List<string> prereqs)
    {
        var user = siteUsers.SiteUser_GetById(userId).Get()!;
        var fulfilled = Array.FindAll(user.FulfilledCsv.Split(","),
            (it) => !string.IsNullOrEmpty(it));
        foreach (var prereq in prereqs)
            if (!fulfilled.Contains(prereq)) return false;

        return true;
    }
}
