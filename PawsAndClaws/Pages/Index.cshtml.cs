using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace PawsAndClaws.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public List<Model.Appointment> _appointmentList { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

        }

        public void OnGet()
        {
            _appointmentList = new List<Model.Appointment>();
            DbAccess db = new DbAccess();
            _appointmentList = db.GetAllAppointments();
        }

        public IActionResult OnPostDelete()
        {
            int test = 5;
            return Page();
        }
    }
}
