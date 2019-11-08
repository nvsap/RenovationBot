using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RenovationBot.Models.ObjectsModels
{
    public class Building
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Address { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ParrentUserId { get; set; }
        public string Link { get; set; }
    }
}
