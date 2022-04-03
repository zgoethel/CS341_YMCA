namespace CS341_YMCA.Services;

using CS341_YMCA.Helpers;

public class ClassValidationService
{
    private ClassRepository Classes { get; set; }
    
    public ClassValidationService(ClassRepository Classes)
    {
        this.Classes = Classes;
    }

    private static bool PeriodsOverlap(DateTime StartA, DateTime EndA, DateTime StartB, DateTime EndB)
    {
        return StartA < EndB && StartB < EndA;
    }

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
            {
                var _Class = Classes.Class_GetById(Other.ClassId).Get()!;
                throw new Exception($"This schedule would conflict with a session of '{_Class.ClassName}' at {Other.FirstDate.ToShortTimeString()} on {Day.ToShortDateString()}.");
            }
        }
    }

    public void Validate(ClassScheduleDBO InQuestion, List<ClassScheduleDBO> Others)
    {
        if (InQuestion.Recurrence == 0)
        {
            ValidateDay(InQuestion, Others, InQuestion.FirstDate.Date);
            return;
        }

        for (int i = 0;
            i <= ((InQuestion.Occurrences <= 0) ? 1024 : InQuestion.Occurrences);
            i++)
        {
            var Date = InQuestion.FirstDate
                .Date
                .AddDays(InQuestion.Recurrence * i);
            ValidateDay(InQuestion, Others, Date);
        }
    }

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
