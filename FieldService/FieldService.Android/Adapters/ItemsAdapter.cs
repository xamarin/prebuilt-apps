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
using System.Collections.Generic;
using Android.Content;
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