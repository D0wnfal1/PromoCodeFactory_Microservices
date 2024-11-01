using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Pcf.ReceivingFromPartner.Core.Abstractions.Repositories;
using Pcf.ReceivingFromPartner.Core.Domain;
using Pcf.ReceivingFromPartner.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class MongoRepository<T> : IRepository<T> where T : BaseEntity
{
	private readonly IMongoCollection<T> _collection;

	public MongoRepository(IMongoClient mongoClient, IOptions<MongoSettings> mongoSettings)
	{
		var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
		_collection = database.GetCollection<T>(typeof(T).Name);
	}

	public async Task<IEnumerable<T>> GetAllAsync()
	{
		return await _collection.Find(_ => true).ToListAsync();
	}

	public async Task<T> GetByIdAsync(ObjectId id)
	{
		return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
	}

	public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<ObjectId> ids)
	{
		return await _collection.Find(x => ids.Contains(x.Id)).ToListAsync();
	}

	public async Task<T> GetFirstWhere(Expression<Func<T, bool>> predicate)
	{
		return await _collection.Find(predicate).FirstOrDefaultAsync();
	}

	public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
	{
		return await _collection.Find(predicate).ToListAsync();
	}

	public async Task AddAsync(T entity)
	{
		await _collection.InsertOneAsync(entity);
	}

	public async Task UpdateAsync(T entity)
	{
		await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
	}

	public async Task DeleteAsync(T entity)
	{
		await _collection.DeleteOneAsync(x => x.Id == entity.Id);
	}
	public void DeleteAll()
	{
		_collection.DeleteMany(Builders<T>.Filter.Empty);
	}

	public void AddMany(IEnumerable<T> entities)
	{
		_collection.InsertMany(entities);
	}

}
