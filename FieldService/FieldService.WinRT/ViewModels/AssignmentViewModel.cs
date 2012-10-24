using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.WinRT.ViewModels {
    public class AssignmentViewModel : FieldService.ViewModels.AssignmentViewModel {
        readonly DelegateCommand recordCommand;

        public AssignmentViewModel ()
        {
            recordCommand = new DelegateCommand (async _ => {
                if (Recording)
                    await Pause ();
                else
                    await Record ();
            }, _ => !IsBusy);
        }

        /// <summary>
        /// Hours formatted for WinRT
        /// </summary>
        public string HoursFormatted
        {
            get { return string.Format ("{0}h {1}m {2}s", Hours.Hours, Hours.Minutes, Hours.Seconds); }
        }

        /// <summary>
        /// Command for the record button
        /// </summary>
        public DelegateCommand RecordCommand
        {
            get { return recordCommand; }
        }

        protected override void OnIsBusyChanged ()
        {
            base.OnIsBusyChanged ();

            if (recordCommand != null)
                recordCommand.RaiseCanExecuteChanged ();
        }

        protected override void OnHoursChanged ()
        {
            base.OnHoursChanged ();

            OnPropertyChanged ("HoursFormatted");
        }
    }
}
