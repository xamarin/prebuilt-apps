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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService {
    /// <summary>
    /// Class for registering services for the app
    /// </summary>
    public static class ServiceRegistrar {

        /// <summary>
        /// Call on startup of the app, it configures ServiceContainer
        /// </summary>
        public static void Startup ()
        {
            ServiceContainer.Register<ILoginService> (() => new SampleLoginService ());
            ServiceContainer.Register<IAssignmentService> (() => new SampleAssignmentService ());

#if !NETFX_CORE
            //Only do these on iOS or Android
            ServiceContainer.Register<MenuViewModel> ();
            ServiceContainer.Register<AssignmentViewModel>();
            ServiceContainer.Register<DocumentViewModel>();
            ServiceContainer.Register<ExpenseViewModel>();
            ServiceContainer.Register<HistoryViewModel>();
            ServiceContainer.Register<ItemViewModel>();
            ServiceContainer.Register<LaborViewModel>();
            ServiceContainer.Register<LoginViewModel>();
            ServiceContainer.Register<PhotoViewModel>();
#endif
        }
    }
}
