//
//  Copyright 2012  Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NETFX_CORE
using Timer = FieldService.WinRT.Utilities.Timer;
#else
using System.Timers;
#endif
using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.ViewModels {
    /// <summary>
    /// View model for assignments list
    /// </summary>
    public class AssignmentViewModel : ViewModelBase {
        readonly IAssignmentService service;
        readonly Timer timer;
        TimeSpan hours = TimeSpan.Zero;
        List<Assignment> assignments;
        Assignment activeAssignment;
        TimerEntry timerEntry;

        public AssignmentViewModel ()
        {
            service = ServiceContainer.Resolve<IAssignmentService> ();

            var taskSource = new TaskCompletionSource<TimeSpan> ();

            timer = new Timer (1000);
            timer.Elapsed += (sender, e) => {
                taskSource.SetResult (hours.Add (TimeSpan.FromSeconds (1)));
                taskSource.Task.ContinueOnUIThread (t => Hours = t.Result);
            };
        }

        /// <summary>
        /// True if the active assignment is recording hours
        /// </summary>
        public bool Recording
        {
            get { return timer.Enabled; }
            set { timer.Enabled = value; OnPropertyChanged ("Recording"); }
        }

        /// <summary>
        /// Number of accumulated hours on the active assignment
        /// </summary>
        public TimeSpan Hours
        {
            get { return hours; }
            set { hours = value; OnPropertyChanged("Hours"); }
        }

        /// <summary>
        /// The current active assignment, this can be null
        /// </summary>
        public Assignment ActiveAssignment
        {
            get { return activeAssignment; }
            set { activeAssignment = value; OnPropertyChanged ("ActiveAssignment"); }
        }

        /// <summary>
        /// List of assignments
        /// </summary>
        public List<Assignment> Assignments
        {
            get { return assignments; }
            set { assignments = value; OnPropertyChanged ("Assignments"); }
        }

        /// <summary>
        /// Loads the assignments asynchronously
        /// </summary>
        public Task LoadAssignmentsAsync ()
        {
            IsBusy = true;
            return service
                .GetAssignmentsAsync ()
                .ContinueOnUIThread (t => {
                    //Grab the active assignment
                    ActiveAssignment = t.Result.FirstOrDefault (a => a.Status == AssignmentStatus.Active);
                    //Grab everything besides the active assignment
                    Assignments = t.Result.Where (a => a.Status != AssignmentStatus.Active).ToList ();
                    IsBusy = false;
                    return t.Result;
                });
        }

        /// <summary>
        /// Loads the timer entry
        /// </summary>
        public Task LoadTimerEntry ()
        {
            IsBusy = true;
            return service
                .GetTimerEntryAsync ()
                .ContinueOnUIThread (t => {
                    IsBusy = false;
                    timerEntry = t.Result;
                    return t.Result;
                });
        }

        public Task SaveAssignment (Assignment assignment)
        {
            IsBusy = true;

            //Save the assignment
            var task = service.SaveAssignment (assignment);

            //If the active assignment
            if (activeAssignment != null &&
                assignment != activeAssignment &&
                assignment.Status == AssignmentStatus.Active) {

                //Set the active assignment to hold and save it
                activeAssignment.Status = AssignmentStatus.Hold;
                task = task.ContinueWith (service.SaveAssignment (activeAssignment));
            }

            //Attach a task to set IsBusy
            return task.ContinueOnUIThread (t => {
                IsBusy = false;
                return t.Result;
            });
        }

        public Task Record ()
        {
            IsBusy = true;
            Recording = true;

            if (timerEntry == null)
                timerEntry = new TimerEntry ();
            timerEntry.Date = DateTime.Now;

            return service
                .SaveTimerEntry(timerEntry)
                .ContinueOnUIThread(t => IsBusy = false);
        }

        public Task Pause ()
        {
            IsBusy = true;
            Recording = false;

            //TODO: save a labor entry

            return service
                .DeleteTimerEntry (timerEntry)
                .ContinueOnUIThread (t => IsBusy = false);
        }
    }
}