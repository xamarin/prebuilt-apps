using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;

namespace FieldService.WinRT.ViewModels {
    public class DocumentViewModel : FieldService.ViewModels.DocumentViewModel {

        /// <summary>
        /// List of the top documents
        /// </summary>
        public IEnumerable<Document> TopDocuments
        {
            get
            {
                if (Documents == null)
                    return null;

                return Documents.Take (9);
            }
        }

        protected override void OnPropertyChanged (string propertyName)
        {
            base.OnPropertyChanged (propertyName);

            //Make sure property changed is raised for new properties
            if (propertyName == "Documents") {
                OnPropertyChanged ("TopDocuments");
            }
        }
    }
}
