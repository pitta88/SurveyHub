namespace SurveyHub.Domain.Institutions;

public sealed class Institution
{
    public const int NameMaxLength = 200;

    public int Id { get; private set; } // filled by DB - Identity
    public string UnitId { get; private set; }
    public string Name { get; private set; }
    public string State { get; private set; }

    public Institution(string unitId, string name, string state)
    {
        UnitId = ValidateUnitId(unitId);
        Name = ValidateName(name, nameof(name));
        State = ValidateState(state);
    }

    public void Rename(string newName)
    {
        Name = ValidateName(newName, nameof(newName));
    }

    private static string ValidateUnitId(string unitId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(unitId);

        if (unitId.Length != 6 || !unitId.All(char.IsAsciiDigit))
        {
            throw new ArgumentException("UnitId must be exactly 6 digits.", nameof(unitId));
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