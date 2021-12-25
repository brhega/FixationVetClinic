using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PawsAndClaws.Pages
{
    public class EditAppointmentModel : PageModel
    {

        [BindProperty]
        public Model.Appointment _appointment { get; set; }

        public void OnGet(int appointmentID)
        {
            DbAccess db = new DbAccess();
            _appointment = db.GetAppointment(appointmentID);
        }

        public IActionResult OnPostSave()
        {
            if (ModelState.IsValid)
            {
                DbAccess db = new DbAccess();
                db.EditAppointment(_appointment);
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

        public IActionResult OnPostDelete()
        {
            DbAccess db = new DbAccess();
            db.DeleteAppointment(_appointment.appointmentID);

            return RedirectToPage("/Index");
        }
    }
}