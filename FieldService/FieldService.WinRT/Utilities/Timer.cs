using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace FieldService.WinRT.Utilities {
    /// <summary>
    /// This is a wrapper around DispatcherTimer for WinRT
    /// It is for exposing methods identical to System.Timers.Timer, to make code simpler in view models using it
    /// </summary>
    public class Timer {
        readonly DispatcherTimer timer = new DispatcherTimer ();

        /// <summary>
        /// Default constructor
        /// </summary>
        public Timer ()
        {
            timer.Tick += (sender, e) => {
                var method = Elapsed;
                if (method != null) {
                    method (this, EventArgs.Empty);
                }
            };
        }

        /// <summary>
        /// Constructor providing an interval of milliseconds
        /// </summary>
        public Timer (int interval)
            : this()
        {
            timer.Interval = TimeSpan.FromMilliseconds (interval);
        }

        /// <summary>
        /// Event fired when Tick is fired
        /// </summary>
        public event EventHandler Elapsed;

        /// <summary>
        /// Calls Start() and Stop() appropriately
        /// </summary>
        public bool Enabled
        {
            get { return timer.IsEnabled; }
            set
            {
                if (timer.IsEnabled != value) {
                    if (value)
                        timer.Start ();
                    else
                        timer.Stop ();
                }
            }
        }
    }
}
