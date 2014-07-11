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
