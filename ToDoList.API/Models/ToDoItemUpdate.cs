using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.API.Models
{
    public class ToDoItemUpdate
    {
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("done")]
        public bool? Done { get; set; }
        [BsonElement("favorite")]
        public bool? Favorite { get; set; }
    }
}
