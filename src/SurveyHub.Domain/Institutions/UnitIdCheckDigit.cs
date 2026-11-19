namespace SurveyHub.Domain.Institutions;

public static class UnitIdCheckDigit
{
    public const int BaseMin = 10000;
    public const int BaseMax = 99999;

    private static readonly int[] Weights = [3, 1, 7, 3, 1];
    public static int Append(int baseNumber) => baseNumber * 10 + Compute(baseNumber);
    public static bool IsValid(int unitId) => Compute(unitId / 10) == unitId % 10;

    private static int Compute(int baseNumber)
    {
        if (baseNumber < BaseMin || baseNumber > BaseMax)
        {
            throw new ArgumentOutOfRangeException(nameof(baseNumber), baseNumber, "Base number must be 5 digits.");
        }

        int digitSum = 0;
        int n = baseNumber;

        for (int i = Weights.Length - 1; i >= 0; i--)
        {
            digitSum += Weights[i] * (n % 10);
            n /= 10;
        }

        return (10 - digitSum % 10) % 10;
    }
}