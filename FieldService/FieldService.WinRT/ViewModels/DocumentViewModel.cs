using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;

namespace FieldService.WinRT.ViewModels {
    public class DocumentViewModel : FieldService.ViewModels.DocumentViewModel {

        public IEnumerable<Document> TopDocuments
        {
            get
            {
                if (Documents == null)
                    return null;

                return Documents.Take (5);
            }
        }
    }
}
