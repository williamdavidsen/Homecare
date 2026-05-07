using Homecare.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Homecare.ViewModels
{
    public class AppointmentCreateViewModel
    {
        // Appointment fields from the form.
        public Appointment Appointment { get; set; } = new Appointment();

        // Single-select Requested Task.
        public int? SelectedTaskId { get; set; }

        // Dropdown contents.
        public IEnumerable<SelectListItem> TaskSelectList { get; set; } = new List<SelectListItem>();
    }
}
