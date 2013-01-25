using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        Assignment assignment;
        AssignmentHistory selectedAssignmentHistory;

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

        public Assignment PastAssignment
        {
            get { return assignment; }
            set { assignment = value; OnPropertyChanged ("PastAssignment"); }
        }

        public AssignmentHistory SelectedAssignmentHistory
        {
            get { return selectedAssignmentHistory; }
            set { selectedAssignmentHistory = value; OnPropertyChanged ("SelectedAssignmentHistory"); }
        }

        /// <summary>
        /// Loads the assignment history
        /// </summary>
        public Task LoadHistoryAsync (Assignment assignment)
        {
            return service
                .GetAssignmentHistoryAsync (assignment)
                .ContinueOnCurrentThread (t => History = t.Result);
        }

        public Task LoadAssignmentFromHistory (AssignmentHistory assignmentHistory)
        {
            return service
                .GetAssignmentFromHistory (assignmentHistory)
                .ContinueOnCurrentThread (t => {
                    PastAssignment = t.Result.FirstOrDefault ();
                    PastAssignment.IsHistory = true;
                    return t.Result;
                });
        }
    }
}
