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
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android {
    public class ItemsAdapter : ArrayAdapter<AssignmentItem> {

        AssignmentViewModel assignmentViewModel;
        IList<AssignmentItem> assignmentItems;
        int resourceId;

        public ItemsAdapter (Context context, int resourceId, IList<AssignmentItem> assignmentItems)
            : base (context, resourceId, assignmentItems)
        {
            this.assignmentItems = assignmentItems;
            this.resourceId = resourceId;
            this.assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            AssignmentItem item = null;
            var view = convertView;
            if (view == null) {
                LayoutInflater inflator = (LayoutInflater)Context.GetSystemService (Context.LayoutInflaterService);
                view = inflator.Inflate (resourceId, null);
            }

            if (assignmentItems.Count > position) {
                item = assignmentItems [position];
            }

            if (item == null) {
                return view;
            }

            var name = view.FindViewById<TextView> (Resource.Id.itemName);
            var checkBox = view.FindViewById<CheckBox> (Resource.Id.itemCheckBox);
            var trashButton = view.FindViewById<CheckBox> (Resource.Id.itemTrashButton);

            name.Text = string.Format ("#{0} {1}", item.Number, item.Name);

            return view;
        }
    }
}