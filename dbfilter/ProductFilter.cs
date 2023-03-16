using System.Linq.Expressions;

namespace dbfilter;

public class ProductFilterData
{
	public string Location { get; init; }
	public Guid Warehouse { get; init; }
	public string ProductName { get; init; }
}

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

		if (Warehouse != null)
		{
			var propertyWarehouse = Expression.Property(parameter, nameof(ProductFilterData.Warehouse));
			var constantWarehouse = Expression.Constant(Warehouse);
			var warehouseExpr = Expression.Equal(propertyWarehouse, constantWarehouse);
			expression = Expression.AndAlso(expression, warehouseExpr);
		}

		if (Location != null)
		{
			var propertyLocation = Expression.Property(parameter, nameof(ProductFilterData.Location));
			var constantLocation = Expression.Constant(Location);
			var locationExpr = Expression.Equal(propertyLocation, constantLocation);
			expression = Expression.AndAlso(expression, locationExpr);
		}
		
		return Expression.Lambda<Func<ProductFilterData, bool>>(expression, parameter);
	}
}