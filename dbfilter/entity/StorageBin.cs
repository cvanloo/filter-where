namespace dbfilter.entity;

public class StorageBin
{
	public string Id { get; init; }
	public Guid WarehouseId { get; init; }
	public Warehouse Warehouse { get; init; }
}