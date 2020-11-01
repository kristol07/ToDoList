using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.API.Models;
using ToDoList.API.Models.Configuration;

namespace ToDoList.API.Services
{
    public class MongoDbRepository : IRepository
    {
        private readonly MongoDbOptions _mongoDbOptions;
        private IMongoCollection<ToDoItem> _items;
        public MongoDbRepository(IOptionsMonitor<MongoDbOptions> mongoDbOptions)
        {
            _mongoDbOptions = mongoDbOptions.CurrentValue;

            var client = new MongoClient(_mongoDbOptions.ConnectionString);
            var db = client.GetDatabase(_mongoDbOptions.DataBaseName);
            _items = db.GetCollection<ToDoItem>(_mongoDbOptions.CollectionName);
        }

        public async Task DeleteAsync(string id)
        {
            await _items.DeleteOneAsync(i => i.Id == id);
        }

        public async Task<ToDoItem> GetAsync(string id)
        {
            var result = await _items.Find(i => i.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<ToDoItem>> QueryAsync(string description, bool? done)
        {
            IEnumerable<ToDoItem> models = _items.FindSync(i => true).ToEnumerable();

            if (!string.IsNullOrEmpty(description))
                models = models.Where(v => v.Description?.IndexOf(description, StringComparison.OrdinalIgnoreCase) >= 0);

            if (done != null)
                models = models.Where(v => v.Done == done.Value);

            return models.ToList();
        }

        public async Task<ToDoItem> UpdateAsync(string id, ToDoItemUpdate updateModel)
        {
            var item = _items.FindAsync(i => i.Id == id).Result.FirstOrDefault();
            if (item != null)
            {
                if (!string.IsNullOrEmpty(updateModel.Description))
                    item.Description = updateModel.Description;

                if (updateModel.Favorite != null)
                    item.Favorite = updateModel.Favorite.Value;

                if (updateModel.Done != null)
                    item.Done = updateModel.Done.Value;

                _items.FindOneAndReplace(i => i.Id == item.Id, item);

                return item;
            }
            return null;
        }

        public async Task UpsertAsync(ToDoItem model)
        {
            var item = _items.FindAsync(i => i.Id == model.Id);
            if (item.Result.Any())
            {
                await _items.FindOneAndReplaceAsync(i => i.Id == model.Id, model);
            }
            else
            {
                await _items.InsertOneAsync(model);
            }
        }
    }
}
