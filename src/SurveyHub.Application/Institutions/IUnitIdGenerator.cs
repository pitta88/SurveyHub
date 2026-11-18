public interface IUnitIdGenerator
{
    Task<int> NextAsync(CancellationToken ct);
}
