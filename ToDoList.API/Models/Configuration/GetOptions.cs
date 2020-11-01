using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.API.Models.Configuration
{
    public sealed class GetOptions
    {
        public bool CaseSensitive { get; set; }

        [Required]
        public string FilterCriteria { get; set; }

        public PagingOptions PagingOptions { get; set; }
    }
}
