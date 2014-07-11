//
//  Copyright 2014  Xamarin Inc.
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
using SQLite;
using FieldService.Data;

namespace FieldService.Data
{
	public class Worker
	{
//		public Worker ()
//		{
//		}

		/// <summary>
		/// Assignment Id
		/// </summary>
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		/// <summary>
		/// The password
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// The password
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The user name
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// The password
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// The password
		/// </summary>
		public string Contact { get; set; }

		/// <summary>
		/// The password
		/// </summary>
		public string Cost { get; set; }

		/// <summary>
		/// The assignment's status
		/// </summary>
		public WorkerType type { get; set; }


	}
}

