using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper.ViewModels
{
    public class ControlsViewModel : ViewModelBase
    {
        public List<ControlsItem> Items => new() {
            new()
            {
                Description = "Pan",
                Control = "Left click and drag",
            },
            new()
            {
                Description = "Zoom",
                Control = "Mouse wheel",
            },
            new()
            {
                Description = "Start/stop measurement",
                Control = "Ctrl + Left click",
            }
        };

        public string Title => "Controls";
        public string RightClickDescription => "To place new markers, move a marker or start a measurement, use the right click menu.";

        public struct ControlsItem
        {
            public string Description { get; set; }
            public string Control { get; set; }
        }
    }
}
