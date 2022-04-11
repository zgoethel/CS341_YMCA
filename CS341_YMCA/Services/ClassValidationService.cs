namespace CS341_YMCA.Services;

using CS341_YMCA.Helpers;

/// <summary>
/// Set of actions to check class schedule sessions against sets of other class
/// schedule sessions. Ensures no two sessions overlap in a specific class'
/// schedule, nor any user's enrollment set.
/// </summary>
public class ClassValidationService
{
    /// <summary>
    /// Checks whether a range of times overlaps.
    /// </summary>
    /// <param name="StartA">Start of the "first" period in question.</param>
    /// <param name="EndA">End of the "first" period in question.</param>
    /// <param name="StartB">Start of the "second" period in question.</param>
    /// <param name="EndB">End of the "second" period in question.</param>
    /// <returns>Whether the periods overlap for any amount of time.</returns>
    private static bool PeriodsOverlap(DateTime StartA, DateTime EndA, DateTime StartB, DateTime EndB)
    {
        // Periods overlap if both sessions mutually begin before the other ends
        return StartA < EndB && StartB < EndA;
    }

    /// <summary>
    /// Checks whether a specific schedule session conflicts with a set of other
    /// schedule entries on a specific day.
    /// </summary>
    /// <param name="InQuestion">A single schedule entry to check.</param>
    /// <param name="Others">The set of other sessions to check against.</param>
    /// <param name="Day">Day to check on this particular call.</param>
    /// <exception cref="Exception">If a schedule conflict is detected.</exception>
    private void ValidateDay(ClassScheduleDBO InQuestion, List<ClassScheduleDBO> Others, DateTime Day)
    {
        // Calculation ASSUMES relevant schedule happens on `Day`;
        // find all classes which occur on that day
        var OnDay = GetClassesOnDay(Others, Day.Date)
            // Filter out the record being edited
            .FindAll((It) => It.Id != InQuestion.Id);
        foreach (var Other in OnDay)
        {
            // Calculate the actual date/time on that specific day
            var RelStart = Day.Date
                .AddHours(InQuestion.FirstDate.Hour)
                .AddMinutes(InQuestion.FirstDate.Minute);
            var OtherStart = Day.Date
                .AddHours(Other.FirstDate.Hour)
                .AddMinutes(Other.FirstDate.Minute);
            if (PeriodsOverlap(
                RelStart, RelStart.AddMinutes(InQuestion.Duration),
                OtherStart, OtherStart.AddMinutes(Other.Duration)
            ))
                throw new Exception($"This schedule would conflict with a session of '{Other.ClassName}' at {Other.FirstDate.ToShortTimeString()} on {Day.ToShortDateString()}.");
        }
    }

    /// <summary>
    /// Checks whether a specific schedule session conflicts with a set of other
    /// schedule entries.
    /// </summary>
    /// <param name="InQuestion">A single schedule entry to check.</param>
    /// <param name="Others">The set of other sessions to check against.</param>
    /// <exception cref="Exception">If a schedule conflict is detected.</exception>
    public void Validate(ClassScheduleDBO InQuestion, List<ClassScheduleDBO> Others)
    {
        // Non-repeating classes only need to check one day
        if (InQuestion.Recurrence == 0)
        {
            ValidateDay(InQuestion, Others, InQuestion.FirstDate.Date);
            return;
        }

        // Check the entire range of the class, or up to 1024 if the class
        // repeats indefinitely
        for (int i = 0;
            i <= ((InQuestion.Occurrences <= 0) ? 1024 : InQuestion.Occurrences);
            i++)
        {
            // Calculate date of current iteration
            var Date = InQuestion.FirstDate
                .Date
                .AddDays(InQuestion.Recurrence * i);
            // Check that day for conflicts
            ValidateDay(InQuestion, Others, Date);
        }
    }

    /// <summary>
    /// Filters a set of schedule sessions down to those which occur on a
    /// specific date, taking into account recurrence interval and length.
    /// </summary>
    /// <param name="Schedule">Full schedule of sessions to pull from.</param>
    /// <param name="Day">This is the day to filter down to.</param>
    /// <returns>Subset of the schedule occurring on the specified day.</returns>
    public List<ClassScheduleDBO> GetClassesOnDay(List<ClassScheduleDBO> Schedule, DateTime Day)
    {
        return Schedule.FindAll((It) =>
        {
            // Select if this is the first day
            var JustDay = It.FirstDate.Date;
            if (JustDay == Day.Date)
                return true;
            // Don't select if class is over (or hasn't started yet)
            if (It.Recurrence <= 0 || Day.Date < JustDay)
                return false;
            // Select if today falls on an even recurrence
            var EndDate = JustDay.AddDays(It.Recurrence * It.Occurrences);
            var DiffDays = Day.Date.Subtract(JustDay).Days;
            if ((Day.Date <= EndDate || It.Occurrences <= 0)
                && DiffDays % It.Recurrence == 0)
                return true;

            return false;
        });
    }
}
