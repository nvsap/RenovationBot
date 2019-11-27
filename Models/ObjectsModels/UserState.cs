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
    public class UserState
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        public int State { get; set; }
        public int UserId { get; set; }
        public int BuildingId { get; set; }
    }
    public enum UserStatesEnum
    {
        Empty,
        AddBuildingName,
        AddBuildingAddress,
        AddBuildingComment,
        AddBuildingPhoto
    }
}
