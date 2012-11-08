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
        DocumentViewModel documentViewModel;

        public DocumentsAdapter (Context context, int resourceId, List<Document> documents)
            : base (context, resourceId, documents)
        {
            this.documents = documents;
            this.resourceId = resourceId;
            documentViewModel = ServiceContainer.Resolve<DocumentViewModel> ();
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
            var image = view.FindViewById<ImageView> (Resource.Id.documentListItemIcon);

            title.Text = document.Title;
            docType.Text = "PDF";

            title.Tag = position;

            return view;
        }
    }
}