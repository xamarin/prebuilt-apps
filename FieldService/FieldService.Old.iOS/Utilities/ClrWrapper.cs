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
using MonoTouch.Foundation;

namespace FieldService.iOS
{
	/// <summary>
	/// An NSObject for holding CLR objects, for passing C# objects through NSDictionary, etc.
	/// </summary>
	public class ClrWrapper : NSObject
	{
		public ClrWrapper (object value)
		{
			Value = value;
		}

		public object Value { get; private set; }
	}

	/// <summary>
	/// Extension methods for use in conjunction with ClrWrapper
	/// </summary>
	public static class ClrWrapperExtensions
	{
		/// <summary>
		/// Wraps the CLR object for use as an NSObject
		/// </summary>
		public static ClrWrapper WrapObject(this object value)
		{
			return new ClrWrapper(value);
		}

		/// <summary>
		/// Unwraps a ClrWrapper to return the underlying CLR object
		/// </summary>
		public static T UnwrapObject<T>(this NSObject obj)
		{
			return (T)((ClrWrapper)obj).Value;
		}
	}
}

