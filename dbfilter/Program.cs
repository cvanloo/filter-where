using dbfilter;
using Microsoft.EntityFrameworkCore;

using var db = new WarehouseContext();
if (!db.Database.CanConnect())
{
	throw new Exception("help!");
}

// re-create database for testing purposes
db.Database.EnsureDeleted();
db.Database.EnsureCreated();

// let's query our database:
var result = db.ProductsAtStorageBins
	.Include(ps => ps.Product)
	.Include(ps => ps.StorageBin)
	// first we need to put data into correct form...
	.Select(ps => new ProductFilterData
	{
		Location = ps.StorageBin.Id,
		Warehouse = ps.StorageBin.Warehouse.Id,
		ProductName = ps.Product.Name,
	})
	// ...and then we apply our filter
	.Where(new ProductFilter { ProductName = "Dingsbums", Location = "00-01-00-05-03" }.ToExpression());

// the above generates the following SQL:
// SELECT `s`.`Id` AS `Location`, `w`.`Id` AS `Warehouse`, `p0`.`Name` AS `ProductName`
// FROM `ProductsAtStorageBins` AS `p`
// INNER JOIN `Products` AS `p0` ON `p`.`ProductId` = `p0`.`Id`
// INNER JOIN `StorageBins` AS `s` ON `p`.`StorageBinId` = `s`.`Id`
// INNER JOIN `Warehouses` AS `w` ON `s`.`WarehouseId` = `w`.`Id`
// WHERE (`p0`.`Name` = 'Dingsbums') AND (`s`.`Id` = '00-01-00-05-03')

foreach (var product in result)
{
	Console.WriteLine($"Name: {product.ProductName}, Location: {product.Location}");
}
