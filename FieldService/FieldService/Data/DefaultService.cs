using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FieldService.Data {
    public class DefaultService : IService {
        //We are using NCrunch during development, we can remove this later to prevent confusion
#if NCRUNCH
        public const int Sleep = 1;
#else
        public const int Sleep = 1000;
#endif

        public Task<bool> LoginAsync (string username, string password)
        {
            return Task.Factory.StartNew (() => {
                Thread.Sleep (Sleep);

                return true;
            });
        }
    }
}
