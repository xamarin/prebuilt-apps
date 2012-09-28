using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Data {
    public interface IService {
        Task<bool> LoginAsync (string username, string password);
    }
}
