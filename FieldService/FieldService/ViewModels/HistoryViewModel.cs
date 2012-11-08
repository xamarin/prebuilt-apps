using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.ViewModels {
    /// <summary>
    /// ViewModel for assignment history
    /// </summary>
    public class HistoryViewModel : ViewModelBase {
        readonly IAssignmentService service;
        List<AssignmentHistory> history;

        public HistoryViewModel ()
        {
            service = ServiceContainer.Resolve<IAssignmentService> ();
        }

        /// <summary>
        /// List of assignment history
        /// </summary>
        public List<AssignmentHistory> History
        {
            get { return history; }
            set { history = value; OnPropertyChanged ("History"); }
        }

        /// <summary>
        /// Loads the assignment history
        /// </summary>
        public Task LoadHistory (Assignment assignment)
        {
            return service
                .GetAssignmentHistoryAsync (assignment)
                .ContinueOnUIThread (t => History = t.Result);
        }
    }
}
