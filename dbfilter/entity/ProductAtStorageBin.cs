using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace dbfilter.entity;

[PrimaryKey(nameof(StorageBinId), nameof(ProductId))]
public class ProductAtStorageBin
{
	public string StorageBinId { get; init; }
	public StorageBin StorageBin { get; init; }
	public string ProductId { get; init; }
	public Product Product { get; init; }
	public uint Quantity { get; init; }
}