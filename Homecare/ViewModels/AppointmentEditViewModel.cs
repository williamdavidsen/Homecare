using Homecare.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Homecare.ViewModels
{
    public class AppointmentEditViewModel
    {
        public Appointment Appointment { get; set; } = new Appointment();

        // Single-select task on edit.
        public int? SelectedTaskId { get; set; }

        // Dropdown contents.
        public IEnumerable<SelectListItem> TaskSelectList { get; set; } = new List<SelectListItem>();
    }
}
