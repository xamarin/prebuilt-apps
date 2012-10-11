using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Data {

    /// <summary>
    /// Interface providing backend data to ViewModels
    /// </summary>
    public interface ILoginService {

        /// <summary>
        /// Asynchronous login
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>A task indicating if successful</returns>
        Task<bool> LoginAsync (string username, string password);
    }
}
