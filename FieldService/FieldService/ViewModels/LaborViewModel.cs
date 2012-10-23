using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.ViewModels {
    /// <summary>
    /// View model for labor hours
    /// </summary>
    public class LaborViewModel : ViewModelBase {
        readonly IAssignmentService service;
        List<Labor> laborHours;

        public LaborViewModel ()
        {
            service = ServiceContainer.Resolve<IAssignmentService> ();
        }

        /// <summary>
        /// List of labor hours
        /// </summary>
        public List<Labor> LaborHours
        {
            get { return laborHours; }
            set { laborHours = value; OnPropertyChanged ("LaborHours"); }
        }

        /// <summary>
        /// Loads the list of labor hours
        /// </summary>
        public Task LoadLaborHours (Assignment assignment)
        {
            return service
                .GetLaborForAssignmentAsync (assignment)
                .ContinueOnUIThread (t => LaborHours = t.Result);
        }

        /// <summary>
        /// Saves a labor entry
        /// </summary>
        public Task SaveLabor (Assignment assignment, Labor labor)
        {
            bool newItem = labor.ID == 0;

            return service.SaveLabor (labor)
                .ContinueWith (t => {
                    if (newItem)
                        laborHours.Add (labor);
                    CalculateHours (assignment);
                });
        }

        /// <summary>
        /// Deletes a labor entry
        /// </summary>
        public Task DeleteLabor (Assignment assignment, Labor labor)
        {
            return service.DeleteLabor (labor)
                .ContinueWith (t => {
                    laborHours.Remove (labor);
                    CalculateHours (assignment);
                });
        }

        /// <summary>
        /// Just a quick method to re-sum the hours
        /// </summary>
        private void CalculateHours (Assignment assignment)
        {
            assignment.TotalHours = TimeSpan.FromMilliseconds (laborHours.Sum (l => l.Hours.TotalMilliseconds));
        }
    }
}
