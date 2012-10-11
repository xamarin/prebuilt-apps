using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FieldService.Data {

    /// <summary>
    /// Default IService implementation
    /// - uses a fake sleep, to emulate network interaction
    /// </summary>
    public class SampleLoginService : ILoginService {

        //We are using NCrunch during development, we can remove this later to prevent confusion
#if NCRUNCH
        private const int Sleep = 1;
#else
        private const int Sleep = 1000;
#endif

        public Task<bool> LoginAsync (string username, string password)
        {
            return Task.Factory.StartNew (() => {

#if NETFX_CORE
                new System.Threading.ManualResetEvent(false).WaitOne(Sleep);
#else
                Thread.Sleep (Sleep);
#endif

                return true;
            });
        }
    }
}