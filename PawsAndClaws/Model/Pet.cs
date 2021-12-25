using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PawsAndClaws.Model
{
    public class Pet
    {
        public int ID { get; set; }

        public Owner petsOwner { get; set; }

        [Required]
        public string name { get; set; }
    }
}
