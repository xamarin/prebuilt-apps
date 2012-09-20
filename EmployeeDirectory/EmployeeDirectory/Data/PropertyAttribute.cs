using System;

namespace EmployeeDirectory
{
	public class PropertyAttribute : Attribute
	{
		public string Group { get; set; }
		public string Ldap { get; set; }

		public PropertyAttribute ()
		{
			Ldap = "";
		}
	}
}

