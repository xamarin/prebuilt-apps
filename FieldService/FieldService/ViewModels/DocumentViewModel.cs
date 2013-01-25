using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.ViewModels {
    /// <summary>
    /// ViewModel for managing documents
    /// </summary>
    public class DocumentViewModel : ViewModelBase {
        readonly IAssignmentService service;
        List<Document> documents;

        public DocumentViewModel ()
        {
            service = ServiceContainer.Resolve<IAssignmentService> ();
        }

        /// <summary>
        /// List of documents to display
        /// </summary>
        public List<Document> Documents
        {
            get { return documents; }
            set { documents = value; OnPropertyChanged ("Documents"); }
        }

        /// <summary>
        /// Gets the list of documents
        /// </summary>
        public Task LoadDocumentsAsync ()
        {
            return service.GetDocumentsAsync ()
                .ContinueOnCurrentThread (t => Documents = t.Result);
        }
    }
}
