using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.API.Models
{
    public class ToDoItem
    {
        [Required]
        [BsonId]
        public string Id { get; set; }
        [StringLength(50)]
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("createdTime")]
        public DateTime CreatedTime { get; set; }
        [BsonElement("done")]
        public bool Done { get; set; }
        [BsonElement("favorite")]
        public bool Favorite { get; set; }
        [BsonElement("children")]
        public ToDoItem[] Children { get; set; }
    }
}
