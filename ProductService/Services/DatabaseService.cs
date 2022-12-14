using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductService;
using ProductService.Entities;

namespace BookStoreApi.Services;

public class DatabaseService
{
    private readonly IMongoCollection<Product> productCollection;

    public DatabaseService(
        IOptions<DatabaseSettings> productDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            productDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            productDatabaseSettings.Value.DatabaseName);

        productCollection = mongoDatabase.GetCollection<Product>(
            productDatabaseSettings.Value.ProductCollectionName);

        

    }

    public async Task<List<Product>> GetAsync() =>
        await productCollection.Find(_ => true).ToListAsync();

    public async Task<Product?> GetAsync(string id) =>
        await productCollection.Find(x => x.id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Product product) =>
        await productCollection.InsertOneAsync(product);

    public async Task UpdateAsync(string id, Product updatedProduct) =>
        await productCollection.ReplaceOneAsync(x => x.id == id, updatedProduct);

    public async Task RemoveAsync(string id) =>
        await productCollection.DeleteOneAsync(x => x.id == id);

    public async Task<List<Product>> GetByNameAsync(string name)
    {
        var filter = new BsonDocument { { "name", new BsonDocument { { "$regex", name } } } };
        var filterLower = new BsonDocument { { "name", new BsonDocument { { "$regex", name.ToLower() } } } };
        var product = await productCollection.Find(filter).ToListAsync();
        var productLower = await productCollection.Find(filterLower).ToListAsync(); 
        var result=product.Concat(productLower);
        return result.ToList();
    }

    public async Task DropAll()=>
        await productCollection.DeleteManyAsync(x => x.name!=" ");
}