using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.API.Models.Configuration
{
    public sealed class PagingOptions
    {
        public bool Enable { get; set; }

        public int PageCount { get; set; }
    }
}
