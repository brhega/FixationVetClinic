using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PawsAndClaws.Model
{
    public class Appointment
    {
        public int appointmentID { get; set; }

        [Required]
        public Pet scheduledPet { get; set; }

        [Required]
        public Owner scheduledOwner { get; set; }

        public DateTime scheduledTime { get; set; }

        public string reason { get; set; }
    }
}
