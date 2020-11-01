using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.API.Models;

namespace ToDoList.API.Services
{
    public interface IRepository
    {
        Task DeleteAsync(string id);
        Task<ToDoItem> GetAsync(string id);
        Task<List<ToDoItem>> QueryAsync(string description, bool? done);
        Task<ToDoItem> UpdateAsync(string id, ToDoItemUpdate updateModel);
        Task UpsertAsync(ToDoItem model);
    }
}
