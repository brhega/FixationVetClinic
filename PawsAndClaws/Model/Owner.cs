using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PawsAndClaws.Model
{
    public class Owner
    {
        public int ID { get; set; }

        [Required]
        public string name { get; set; }

        public string phoneNumber { get; set; }

    }
}
