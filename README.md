# Filter-Object to SQL Where-Clause

Have a look at the full example code [here](./dbfilter/).

The most important parts explained quickly:

We create a filter object that can turn itself into an expression:

```C#
public class ProductFilter
{
	public string? Location { get; init; } = null;
	public Guid? Warehouse { get; init; } = null;
	public string? ProductName { get; init; } = null;
	
	// one could probably use reflection to automate this
	public Expression<Func<ProductFilterData, bool>> ToExpression()
	{
		var parameter = Expression.Parameter(typeof(ProductFilterData), "product");

		// we need some default expression to start out with: `true == true`
		var expression = Expression.Equal(Expression.Constant(true), Expression.Constant(true));
		
		if (ProductName != null)
		{
			var propertyName = Expression.Property(parameter, nameof(ProductFilterData.ProductName));
			var constantName = Expression.Constant(ProductName);
			var nameExpr = Expression.Equal(propertyName, constantName);
			expression = Expression.AndAlso(expression, nameExpr);
		}

		// repeat logic for other properties...
		
		return Expression.Lambda<Func<ProductFilterData, bool>>(expression, parameter);
	}
}
```

The filter object expects data to be of a specific form:

```C#
public class ProductFilterData
{
	public string Location { get; init; }
	public Guid Warehouse { get; init; }
	public string ProductName { get; init; }
}
```

Finally, we can create SQL queries like:

```C#
using var db = new WarehouseContext();

// let's query our database:
var result = db.ProductsAtStorageBins
	.Include(ps => ps.Product)
	.Include(ps => ps.StorageBin)
	// first we need to put data into correct form...
    // (Use property initializer instead of constructors; constructors cannot
    // be translated into SQL!)
	.Select(ps => new ProductFilterData
	{
		Location = ps.StorageBin.Id,
		Warehouse = ps.StorageBin.Warehouse.Id,
		ProductName = ps.Product.Name,
	})
	// ...and then we apply our filter
	.Where(new ProductFilter { ProductName = "Dingsbums", Location = "00-01-00-05-03" }.ToExpression());
```

Daraus wird folgendes SQL (MariaDB) generiert:

```sql
SELECT `s`.`Id` AS `Location`, `w`.`Id` AS `Warehouse`, `p0`.`Name` AS `ProductName`
FROM `ProductsAtStorageBins` AS `p`
INNER JOIN `Products` AS `p0` ON `p`.`ProductId` = `p0`.`Id`
INNER JOIN `StorageBins` AS `s` ON `p`.`StorageBinId` = `s`.`Id`
INNER JOIN `Warehouses` AS `w` ON `s`.`WarehouseId` = `w`.`Id`
WHERE (`p0`.`Name` = 'Dingsbums') AND (`s`.`Id` = '00-01-00-05-03')
```
