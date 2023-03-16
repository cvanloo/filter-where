namespace dbfilter.entity;

public class Warehouse
{
	public Guid Id { get; init; }
	public string Name { get; init; }
	public IEnumerable<StorageBin> Bins { get; init; }
}