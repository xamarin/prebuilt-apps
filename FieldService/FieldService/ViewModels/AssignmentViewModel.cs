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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;
using FieldService.Utilities;
using CancellationToken = System.Threading.CancellationToken;

#if NETFX_CORE
using Timer = FieldService.WinRT.Utilities.Timer;
#else
using System.Timers;
#endif

namespace FieldService.ViewModels {
    /// <summary>
    /// View model for assignments list
    /// </summary>
    public class AssignmentViewModel : ViewModelBase {
        readonly IAssignmentService service;
        readonly Timer timer;
        TimeSpan hours = TimeSpan.Zero;
        TimeSpan currentHours = TimeSpan.Zero;
        List<Assignment> assignments;
	Assignment activeAssignment, selectedAssignment, lastAssignment;
        Data.Signature signature;
        TimerEntry timerEntry;

        /// <summary>
        /// Event when Hours is updated
        /// </summary>
        public event EventHandler HoursChanged;

        /// <summary>
        /// Event when Recording is changed
        /// </summary>
        public event EventHandler RecordingChanged;

        public AssignmentViewModel ()
        {
            AvailableStatuses = new AssignmentStatus []
            {
                AssignmentStatus.Hold,
                AssignmentStatus.Active,
                AssignmentStatus.Complete,
            };

            service = ServiceContainer.Resolve<IAssignmentService> ();

            timer = new Timer (1000);

#if !NETFX_CORE
            //This causes our timer to fire it's events on the UI thread
            timer.SynchronizingObject = ServiceContainer.Resolve<ISynchronizeInvoke> ();
#endif
            timer.Elapsed += (sender, e) => {
                CurrentHours = currentHours.Add (TimeSpan.FromSeconds (1));
                Hours = hours.Add (TimeSpan.FromSeconds (1));
            };
        }

        /// <summary>
        /// Selected assignment
        /// </summary>
        public virtual Assignment SelectedAssignment
        {
            get { return selectedAssignment; }
            set { selectedAssignment = value; OnPropertyChanged ("SelectedAssignment"); }
        }

	/// <summary>
	/// The "last" assignment, used for when switching between history
	/// </summary>
	public Assignment LastAssignment
	{
		get { return lastAssignment; }
		set { lastAssignment = value; OnPropertyChanged ("LastAssignment"); }
	}

        /// <summary>
        /// List of available statuses an assignment can be set to
        /// </summary>
        public AssignmentStatus [] AvailableStatuses
        {
            get;
            private set;
        }

        /// <summary>
        /// True if the active assignment is recording hours
        /// </summary>
        public bool Recording
        {
            get { return timer.Enabled; }
            set { timer.Enabled = value; OnRecordingChanged (); }
        }

        /// <summary>
        /// Called when Recording changes
        /// </summary>
        protected virtual void OnRecordingChanged ()
        {
            OnPropertyChanged ("Recording");
            var method = RecordingChanged;
            if (method != null)
                RecordingChanged (this, EventArgs.Empty);
        }

        /// <summary>
        /// Number of hours for the current record session on the active assignment
        /// </summary>
        public TimeSpan CurrentHours
        {
            get { return currentHours; }
            set { currentHours = value; OnPropertyChanged ("CurrentHours"); }
        }

        /// <summary>
        /// Number of accumulated hours on the active assignment
        /// </summary>
        public TimeSpan Hours
        {
            get { return hours; }
            set { hours = value; OnHoursChanged (); }
        }

        /// <summary>
        /// Called when Hours changes
        /// </summary>
        protected virtual void OnHoursChanged ()
        {
            OnPropertyChanged ("Hours");

            var method = HoursChanged;
            if (method != null)
                HoursChanged (this, EventArgs.Empty);
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
        /// The current signature
        /// </summary>
        public Data.Signature Signature
        {
            get { return signature; }
            set { signature = value; OnPropertyChanged ("Signature"); }
        }

        /// <summary>
        /// Loads the assignments asynchronously
        /// </summary>
        public Task LoadAssignmentsAsync ()
        {
            return service
                .GetAssignmentsAsync ()
                .ContinueOnCurrentThread (t => {
                    //Grab the active assignment
                    ActiveAssignment = t.Result.FirstOrDefault (a => a.Status == AssignmentStatus.Active);
                    //Grab everything besides the active assignment
                    Assignments = t.Result.Where (a => a.Status != AssignmentStatus.Active).ToList ();
                    return t.Result;
                });
        }

        /// <summary>
        /// Loads the timer entry
        /// </summary>
        public Task LoadTimerEntryAsync ()
        {
            return service
                .GetTimerEntryAsync ()
                .ContinueOnCurrentThread (t => {
                    timerEntry = t.Result;
                    if (timerEntry != null) {
                        Recording = true;
                        CurrentHours =
                            Hours = (DateTime.Now - timerEntry.Date);
                        timer.Enabled = true;
                    }
                    return timerEntry;
                });
        }

        /// <summary>
        /// Saves an assignment, and applies any needed changes to the active one
        /// </summary>
        public Task SaveAssignmentAsync (Assignment assignment)
        {
            //Save the assignment
            Task task = service.SaveAssignmentAsync (assignment);

            //If the active assignment should be put on hold
            if (activeAssignment != null &&
                assignment != activeAssignment &&
                assignment.Status == AssignmentStatus.Active) {

                //Set the active assignment to hold and save it
                activeAssignment.Status = AssignmentStatus.Hold;
                if (Recording) {
                    task = task.ContinueWith (PauseAsync ());
                }
                task = task.ContinueWith (service.SaveAssignmentAsync (activeAssignment));
            }

            //If we are saving the active assignment, we need to pause it
            if (assignment == activeAssignment &&
                assignment.Status != AssignmentStatus.Active &&
                Recording) {
                task = task.ContinueWith (PauseAsync ());
                Hours = TimeSpan.Zero;
            }

            //Set the active assignment
            if (assignment.Status == AssignmentStatus.Active) {
                ActiveAssignment = assignment;
            }

            return task;
        }

        /// <summary>
        /// Saves the current signature
        /// </summary>
        public Task SaveSignatureAsync ()
        {
            return service.SaveSignatureAsync (Signature);
        }

        /// <summary>
        /// Loads the signature for an assignment
        /// </summary>
        public Task LoadSignatureAsync (Assignment assignment)
        {
            return service.GetSignatureAsync (assignment).ContinueOnCurrentThread (t => Signature = t.Result);
        }

        /// <summary>
        /// Starts timer
        /// </summary>
        public Task RecordAsync ()
        {
            if (activeAssignment == null)
                return Task.Factory.StartNew (delegate { });

            IsBusy =
                Recording = true;

            if (timerEntry == null)
                timerEntry = new TimerEntry ();
            timerEntry.Date = DateTime.Now;

            return service
                .SaveTimerEntryAsync (timerEntry)
                .ContinueOnCurrentThread (_ => IsBusy = false);
        }

        /// <summary>
        /// Pauses timer
        /// </summary>
        public Task PauseAsync ()
        {
            if (activeAssignment == null || timerEntry == null)
                return Task.Factory.StartNew (delegate { });

            IsBusy = true;
            Recording = false;

            var labor = new Labor {
                Type = LaborType.Hourly,
                AssignmentId = activeAssignment.Id,
                Description = "Time entered automatically at: " + DateTime.Now.ToShortTimeString (),
                Hours = (DateTime.Now - timerEntry.Date),
            };

            return service
                .SaveLaborAsync (labor)
                .ContinueWith (service.DeleteTimerEntryAsync (timerEntry))
                .ContinueOnCurrentThread (_ => {
                    CurrentHours = TimeSpan.Zero;
                    IsBusy = false;
                });
        }
    }
}