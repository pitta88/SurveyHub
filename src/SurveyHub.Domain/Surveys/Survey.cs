namespace SurveyHub.Domain.Surveys;

public sealed class Survey
{
    public const int TitleMaxLength = 200;
    public const int CodeMaxLength = 20;
    public int Id { get; private set; }
    public string Code { get; private set; }
    public string Title { get; private set; }
    public DateOnly OpensOn { get; private set; }
    public DateOnly Deadline { get; private set; }

    public Survey(string code, string title, DateOnly opensOn, DateOnly deadline)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        if (deadline < opensOn)
        {
            throw new ArgumentException("Deadline must be on or after the opening date.", nameof(deadline));
        }

        Code = code;
        Title = title;
        OpensOn = opensOn;
        Deadline = deadline;
    }

    public bool IsOpen(DateOnly today) => OpensOn <= today && today <= Deadline;
}
