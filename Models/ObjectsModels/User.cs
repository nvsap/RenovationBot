using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RenovationBot.Models.ObjectsModels
{
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Name { get; set; }
    }
}
