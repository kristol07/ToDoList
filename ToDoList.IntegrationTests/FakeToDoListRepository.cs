using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoList.API.Models;
using ToDoList.API.Services;

namespace ToDoList.IntegrationTests
{
    public class FakeToDoListRepository : IRepository
    {
        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ToDoItem> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ToDoItem>> QueryAsync(string description, bool? done)
        {
            throw new NotImplementedException();
        }

        public Task<ToDoItem> UpdateAsync(string id, ToDoItemUpdate updateModel)
        {
            throw new NotImplementedException();
        }

        public Task UpsertAsync(ToDoItem model)
        {
            throw new NotImplementedException();
        }
    }
}
