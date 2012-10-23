using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FieldService.Android.Utilities {
    public class EventArgs<T> : EventArgs{

        public T Value
        {
            get;
            set;
        }
    }
}