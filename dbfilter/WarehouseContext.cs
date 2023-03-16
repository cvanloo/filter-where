using dbfilter.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace dbfilter;

public class WarehouseContext : DbContext
{
	public DbSet<Warehouse> Warehouses { get; init; }
	public DbSet<StorageBin> StorageBins { get; init; }
	public DbSet<Product> Products { get; init; }
	public DbSet<ProductAtStorageBin> ProductsAtStorageBins { get; init; }
	
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		// test only, who cares?
		const string connectionString = "server=localhost;user=dbfilteruser;password=123456;database=dbfiltertest";
		var serverVersion = new MySqlServerVersion(new Version());
		optionsBuilder
			.UseMySql(connectionString, serverVersion)
			.LogTo(Console.WriteLine, LogLevel.Information)
			.EnableSensitiveDataLogging()
			.EnableDetailedErrors();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		var warehouseId = Guid.NewGuid();
		const string productId = "10002014";
		const string storageBinId = "00-01-00-05-03";
		
		modelBuilder.Entity<Warehouse>()
			.HasData(new Warehouse { Id = warehouseId, Name = "Someplace" });

		modelBuilder.Entity<StorageBin>()
			.HasData(new StorageBin { WarehouseId = warehouseId, Id = storageBinId });

		modelBuilder.Entity<Product>()
			.HasData(new Product { Id = productId, Name = "Dingsbums", Description = "Irgend so ein Ding." });

		modelBuilder.Entity<ProductAtStorageBin>()
			.HasData(new ProductAtStorageBin { ProductId = productId, StorageBinId = storageBinId });
	}
}