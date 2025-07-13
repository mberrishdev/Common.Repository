# Common.Repository

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![NuGet](https://img.shields.io/badge/NuGet-Mberish.Common.Repository-green.svg)](https://www.nuget.org/packages/Mberish.Common.Repository)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A powerful and flexible repository pattern implementation for Entity Framework Core that simplifies data access and provides a clean abstraction layer for your .NET applications.

## 🚀 Features

- **Generic Repository Pattern**: Type-safe repository implementations for any entity
- **Query Repository**: Optimized read-only operations with advanced querying capabilities
- **Unit of Work Pattern**: Transaction management with scoped operations
- **Pagination Support**: Built-in pagination with `PagedList<T>` and `PagingDetails`
- **Sorting Support**: Flexible sorting with `SortingDetails<T>` and `SortItem`
- **Dependency Injection**: Seamless integration with .NET DI container
- **Save Change Strategies**: Configurable save strategies (PerOperation/PerUnitOfWork)
- **Async/Await Support**: Full async support throughout the library
- **Entity Framework Core Integration**: Built specifically for EF Core 6.0+

## 📦 Installation

```bash
dotnet add package BerrishDev.Common.Repository
```

## 🏗️ Architecture

The library follows clean architecture principles and provides:

- **Repository Interfaces**: `IRepository<T>` and `IQueryRepository<T>`
- **EF Core Implementation**: `EFCoreRepository<TDbContext, TEntity>` and `EfCoreQueryRepository<TDbContext, TEntity>`
- **Unit of Work**: `IUnitOfWork` and `IUnitOfWorkScope` for transaction management
- **Pagination**: `PagedList<T>` and `PagingDetails` for efficient data paging
- **Sorting**: `SortingDetails<T>`, `SortItem`, and `SortDirection` for flexible data ordering

## 🚀 Quick Start

### 1. Configure Services

```csharp
using Common.Repository.EfCore.Extensions;
using Common.Repository.EfCore.Options;

// In your Program.cs or Startup.cs
services.AddEfCoreDbContext<YourDbContext>(options =>
{
    options.UseSqlServer(connectionString);
}, repositoryOptions: options =>
{
    options.SaveChangeStrategy = SaveChangeStrategy.PerUnitOfWork;
});

services.AddUnitOfWork();
```

### 2. Create Your Entity

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### 3. Use the Repository

```csharp
public class ProductService
{
    private readonly IRepository<Product> _repository;
    private readonly IQueryRepository<Product> _queryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(
        IRepository<Product> repository,
        IQueryRepository<Product> queryRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _queryRepository = queryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        using var scope = await _unitOfWork.CreateScopeAsync(cancellationToken);
        
        var createdProduct = await _repository.InsertAsync(product, cancellationToken);
        await scope.CompletAsync(cancellationToken);
        
        return createdProduct;
    }

    public async Task<PagedList<Product>> GetProductsAsync(
        int pageIndex, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        return await _queryRepository.GetListByPageAsync(
            pageIndex, 
            pageSize, 
            cancellationToken: cancellationToken);
    }
}
```

## 📚 API Reference

### Repository Interfaces

#### IRepository<TEntity>

Provides full CRUD operations:

```csharp
public interface IRepository<TEntity> : IQueryRepository<TEntity>
{
    // Insert operations
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    // Update operations
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    // Delete operations
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    // Query operations with update tracking
    Task<List<TEntity>> GetListForUpdateAsync(
        List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? relatedProperties = null,
        Expression<Func<TEntity, bool>>? predicate = null,
        SortingDetails<TEntity>? sortingDetails = null,
        int? skip = null, 
        int? take = null,
        CancellationToken cancellationToken = default);
        
    Task<TEntity> GetForUpdateAsync(
        Expression<Func<TEntity, bool>> predicate,
        List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? relatedProperties = null,
        CancellationToken cancellationToken = default);
}
```

#### IQueryRepository<TEntity>

Provides read-only operations:

```csharp
public interface IQueryRepository<TEntity>
{
    // Basic query operations
    Task<List<TEntity>> GetListAsync(
        List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? relatedProperties = null,
        Expression<Func<TEntity, bool>>? predicate = null,
        SortingDetails<TEntity>? sortingDetails = null,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default);
        
    // Pagination
    Task<PagedList<TEntity>> GetListByPageAsync(
        int pageIndex, 
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? relatedProperties = null,
        SortingDetails<TEntity>? sortingDetails = null,
        CancellationToken cancellationToken = default);
        
    // Single entity operations
    Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? relatedProperties = null,
        CancellationToken cancellationToken = default);
        
    // Aggregation operations
    Task<long> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
        
    Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
}
```

### Unit of Work

#### IUnitOfWork

```csharp
public interface IUnitOfWork
{
    Task<IUnitOfWorkScope> CreateScopeAsync(CancellationToken cancellationToken = default);
}
```

#### IUnitOfWorkScope

```csharp
public interface IUnitOfWorkScope : IDisposable
{
    Task CompletAsync(CancellationToken cancellationToken = default);
}
```

### Pagination

#### PagedList<T>

```csharp
public class PagedList<TItem>
{
    public PagingDetails PagingDetails { get; }
    public List<TItem> List { get; }
}
```

#### PagingDetails

```csharp
public class PagingDetails
{
    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }
    public bool HasPreviousPage { get; }
    public bool HasNextPage { get; }
}
```

### Sorting

#### SortingDetails<T>

```csharp
public class SortingDetails<TEntity>
{
    public List<SortItem> SortItems { get; set; } = new();
}
```

#### SortItem

```csharp
public class SortItem
{
    public string PropertyName { get; set; }
    public SortDirection Direction { get; set; }
}
```

#### SortDirection

```csharp
public enum SortDirection
{
    ASC,
    DESC
}
```

## 🔧 Configuration Options

### SaveChangeStrategy

Configure when changes are saved to the database:

```csharp
public enum SaveChangeStrategy
{
    PerOperation,    // Save changes after each operation
    PerUnitOfWork    // Save changes only when unit of work completes
}
```

### RepositoryAttribute

Control repository generation for specific DbSet properties:

```csharp
public class YourDbContext : DbContext
{
    [Repository(CreateGenericRepository = false, CreateQueryRepository = true)]
    public DbSet<ReadOnlyEntity> ReadOnlyEntities { get; set; }
    
    [Repository(CreateGenericRepository = true, CreateQueryRepository = false)]
    public DbSet<WriteOnlyEntity> WriteOnlyEntities { get; set; }
}
```

## 📖 Examples

### Advanced Querying with Includes

```csharp
// Get products with categories and suppliers
var products = await _queryRepository.GetListAsync(
    relatedProperties: new List<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>
    {
        q => q.Include(p => p.Category),
        q => q.Include(p => p.Supplier)
    },
    predicate: p => p.Price > 100,
    sortingDetails: new SortingDetails<Product>
    {
        SortItems = new List<SortItem>
        {
            new() { PropertyName = "Name", Direction = SortDirection.ASC },
            new() { PropertyName = "Price", Direction = SortDirection.DESC }
        }
    },
    skip: 10,
    take: 20
);
```

### Pagination with Filtering

```csharp
// Get paginated products by category
var pagedProducts = await _queryRepository.GetListByPageAsync(
    pageIndex: 0,
    pageSize: 10,
    predicate: p => p.CategoryId == categoryId,
    sortingDetails: new SortingDetails<Product>
    {
        SortItems = new List<SortItem>
        {
            new() { PropertyName = "CreatedAt", Direction = SortDirection.DESC }
        }
    }
);

// Access pagination details
Console.WriteLine($"Total items: {pagedProducts.PagingDetails.TotalCount}");
Console.WriteLine($"Total pages: {pagedProducts.PagingDetails.TotalPages}");
Console.WriteLine($"Has next page: {pagedProducts.PagingDetails.HasNextPage}");
```

### Transaction Management

```csharp
public async Task TransferMoneyAsync(int fromAccountId, int toAccountId, decimal amount)
{
    using var scope = await _unitOfWork.CreateScopeAsync();
    
    try
    {
        var fromAccount = await _repository.GetForUpdateAsync(a => a.Id == fromAccountId);
        var toAccount = await _repository.GetForUpdateAsync(a => a.Id == toAccountId);
        
        fromAccount.Balance -= amount;
        toAccount.Balance += amount;
        
        await _repository.UpdateAsync(fromAccount);
        await _repository.UpdateAsync(toAccount);
        
        await scope.CompletAsync();
    }
    catch
    {
        // Transaction will be rolled back automatically
        throw;
    }
}
```

## 🏗️ Project Structure

```
Common.Repository/
├── Common.Repository/           # Main library
│   ├── EfCore/                 # EF Core specific implementations
│   │   ├── Extensions/         # Service collection extensions
│   │   └── Options/           # Configuration options
│   ├── Repository/            # Repository interfaces and implementations
│   │   └── EfCore/           # EF Core repository implementations
│   ├── UnitOfWork/           # Unit of work interfaces and implementations
│   │   └── EfCore/          # EF Core unit of work implementations
│   ├── Lists/               # Pagination and sorting utilities
│   │   ├── Pagination/     # Paging classes
│   │   └── Sorting/        # Sorting classes
│   └── Exceptions/         # Custom exceptions
├── Sample/                 # Example implementation
│   ├── Sample.API/        # Web API example
│   ├── Sample.Application/ # Application layer example
│   ├── Sample.Domain/     # Domain entities example
│   └── Sample.Persistence/ # Data access layer example
└── Common.Repository.sln  # Solution file
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Mikheil berishvili** - [GitHub](https://github.com/mberrishdev)

## 🙏 Acknowledgments

- Entity Framework Core team for the excellent ORM
- .NET community for inspiration and best practices
- All contributors who help improve this library

---

**Note**: This library is designed for .NET 6.0+ and Entity Framework Core 6.0+. Make sure your project targets the appropriate framework version. 