using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PawsAndClaws.Pages
{
    /*For complexity/time reasons, each Add Appointment form submission will create a new appointment, pet and owner
    Even though the database is setup to remove redundant pets and owners through normalization*/
    public class AddAppointmentModel : PageModel
    {
        [BindProperty]
        public Model.Appointment _appointment { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPostSave()
        {
            if (ModelState.IsValid)
            {
                DbAccess db = new DbAccess();
                db.AddAppointment(_appointment);
                return RedirectToPage("/Index");
            }
            else
            {
                return Page();
            }
        }

        public IActionResult OnPostCancel()
        {
            return RedirectToPage("/Index");
        }
    }
}