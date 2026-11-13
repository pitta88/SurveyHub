namespace SurveyHub.Domain.Institutions;

public sealed class Institution
{
    public const int NameMaxLength = 500;
    public const int UnitIdMin = 100000;
    public const int UnitIdMax = 999999;

    public int UnitId { get; private set; }  // Key
    public string Name { get; private set; }
    public string State { get; private set; }

    public Institution(int unitId, string name, string state)
    {
        UnitId = ValidateUnitId(unitId);
        Name = ValidateName(name, nameof(name));
        State = ValidateState(state);
    }

    public void Rename(string newName)
    {
        Name = ValidateName(newName, nameof(newName));
    }

    private static int ValidateUnitId(int unitId)
    {
        if (unitId < UnitIdMin || unitId > UnitIdMax)
        {
            throw new ArgumentOutOfRangeException(nameof(unitId), unitId, "UnitId must be exactly 6-digit number.");
        }
        return unitId;
    }

    private static string ValidateName(string name, string paramName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, paramName);

        if (name.Length > NameMaxLength)
        {
            throw new ArgumentException($"Name must be {NameMaxLength} characters or fewer.", paramName);
        }

        return name;
    }

    private static string ValidateState(string state)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(state, nameof(state));

        if (state.Length != 2 || !state.All(char.IsAsciiLetterUpper))
        {
            throw new ArgumentException("State must be a 2-letter uppercase code.", nameof(state));
        }

        return state;
    }

}