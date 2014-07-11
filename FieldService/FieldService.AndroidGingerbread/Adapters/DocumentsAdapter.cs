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
//    limitations under the License.using System;

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

namespace FieldService.Android.Adapters {
    public class DocumentsAdapter : ArrayAdapter<Document> {
        List<Document> documents;
        int resourceId;

        public DocumentsAdapter (Context context, int resourceId, List<Document> documents)
            : base (context, resourceId, documents)
        {
            this.documents = documents;
            this.resourceId = resourceId;
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            Document document = null;
            var view = convertView;
            if (view == null) {
                LayoutInflater inflator = (LayoutInflater)Context.GetSystemService (Context.LayoutInflaterService);
                view = inflator.Inflate (resourceId, null);
            }

            if (documents != null && documents.Count > position) {
                document = documents [position];
            }

            if (document == null) {
                return view;
            }
            var title = view.FindViewById<TextView> (Resource.Id.documentListItemDocTitle);
            var docType = view.FindViewById<TextView> (Resource.Id.documentListItemDocType);

            title.Text = document.Title;
            docType.Text = "PDF";

            title.Tag = position;

            return view;
        }
    }
}