using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ToDoList.API.Models;
using ToDoList.API.Models.Configuration;
using ToDoList.API.Services;

namespace ToDoList.API.Controllers
{
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        private readonly IOptionsMonitor<GetOptions> _getOptions;
        private IRepository _repository;

        public ToDoListController(IRepository repository, IOptionsMonitor<GetOptions> getOptions)
        {
            _repository = repository;
            _getOptions = getOptions;
        }

        /// <summary>
        /// Query ToDoItem by specific description and done
        /// </summary>
        /// <param name="description">todoItem descripion</param>
        /// <param name="done">ToDoItem status</param>
        /// <returns></returns>
        /// <response code="200">Returned all todoitems by specific description and done</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ToDoItem>>> QueryAsync(
            string description, bool? done)
        {
            var list = await _repository.QueryAsync(description, done);
            return Ok(list
                .Where(item => item.Description.IndexOf(_getOptions.CurrentValue.FilterCriteria,
                _getOptions.CurrentValue.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) >= 0));
        }

        /// <summary>
        /// Update ToDoItem with partial model
        /// </summary>
        /// <param name="id">Id of ToDoItem to update</param>
        /// <param name="updateModel">Patial update model</param>
        /// <returns></returns>
        /// <response code="200">Updated ToDoItem</response>
        /// <response code="400">If id is empty</response>
        /// <response code="404">If id is not existed in database</response>   
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToDoItem>> UpdateAsync(
            [Required] string id, [Required] ToDoItemUpdate updateModel)
        {
            //check id
            if (string.IsNullOrEmpty(id))
                return BadRequest(new Dictionary<string, string>() { { "message", "Id is required" } });

            var modelInDb = await _repository.GetAsync(id);
            if (modelInDb == null)
                return NotFound(new Dictionary<string, string>() { { "message", $"Can't find {id}" } });

            //update
            var updated = await _repository.UpdateAsync(id, updateModel);
            return Ok(updated);
        }

        /// <summary>
        /// Delete ToDoItem by id
        /// </summary>
        /// <param name="id">Id of todoItem to delete</param>
        /// <returns></returns>
        /// <response code="204">ToDoItem deleted</response>
        /// <response code="400">If id is empty</response>
        /// <response code="404">If id is not existed in database</response>   
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            //check id
            if (string.IsNullOrEmpty(id))
                return BadRequest(new Dictionary<string, string>() { { "message", "Id is required" } });

            var modelInDb = await _repository.GetAsync(id);
            if (modelInDb == null)
                return NotFound(new Dictionary<string, string>() { { "message", $"Can't find {id}" } });

            //delete
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Upsert ToDoItem
        /// </summary>
        /// <param name="todoItem"></param>
        /// <returns></returns>
        /// <response code="200">Upserted ToDoItem</response>
        /// <response code="400">If id is empty</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ToDoItem>> UpsertAsync(ToDoItem todoItem)
        {
            if (string.IsNullOrEmpty(todoItem.Id))
                return BadRequest(new Dictionary<string, string>() { { "message", "Id is required" } });

            await _repository.UpsertAsync(todoItem);

            var model = _repository.GetAsync(todoItem.Id).Result;
            //if (model == null)
            //    return new StatusCodeResult(500);
            return Ok(model);
        }


    }
}
