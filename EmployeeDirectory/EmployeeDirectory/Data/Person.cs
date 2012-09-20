using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EmployeeDirectory.Data
{
	/// <summary>
	/// Person.
	/// </summary>
	/// <remarks>
	/// Derived from <a href="http://fsuid.fsu.edu/admin/lib/WinADLDAPAttributes.html#RANGE!B19">Windows AD LDAP Schema</a>
	/// and <a href="http://www.zytrax.com/books/ldap/ape/core-schema.html">core.schema</a> from OpenLDAP.
	/// </remarks>
	[Serializable]
	public class Person
	{
		string id;
		public string Id {
			get {
				if (!string.IsNullOrEmpty (id)) {
					return id;
				}
				else if (!string.IsNullOrEmpty (Email)) {
					return Email;
				}
				else {
					return Name;
				}
			}
			set {
				id = value;
			}
		}

		[Property (Group = "General", Ldap = "cn")]
		public string Name { get; set; }

		[Property (Group = "General", Ldap = "givenName")]
		public string FirstName { get; set; }

		[Property (Group = "General", Ldap = "initials")]
		public string Initials { get; set; }

		[Property (Group = "General", Ldap = "sn")]
		public string LastName { get; set; }

		[Property (Group = "General", Ldap = "displayName")]
		public string DisplayName { get; set; }

		[Property (Group = "General", Ldap = "description")]
		public string Description { get; set; }



		[Property (Group = "Contact", Ldap = "physicalDeliveryOfficeName,roomNumber")]
		public string Office { get; set; }

		[Property (Group = "Contact", Ldap = "telephoneNumber,otherTelephone")]
		public List<string> TelephoneNumbers { get; set; }

		[Property (Group = "Contact", Ldap = "homePhone,otherHomePhone")]
		public List<string> HomeNumbers { get; set; }

		[Property (Group = "Contact", Ldap = "mobile,otherMobile")]
		public List<string> MobileNumbers { get; set; }

		[Property (Group = "Contact", Ldap = "mail")]
		public string Email { get; set; }

		[Property (Group = "Contact", Ldap = "wWWHomePage")]
		public string WebPage { get; set; }

		[Property (Group = "Contact", Ldap = "notes")]
		public string Info { get; set; }

		[Property (Group = "Contact", Ldap = "twitter")]
		public string Twitter { get; set; }


		[Property (Group = "Address", Ldap = "street,streetAddress")]
		public string Street { get; set; }

		[Property (Group = "Address", Ldap = "postOfficeBox")]
		public string POBox { get; set; }

		[Property (Group = "Address", Ldap = "l,localityName")]
		public string City { get; set; }

		[Property (Group = "Address", Ldap = "st,stateOrProvinceName")]
		public string State { get; set; }

		[Property (Group = "Address", Ldap = "postalCode")]
		public string PostalCode { get; set; }

		[Property (Group = "Address", Ldap = "c,co,countryCode")]
		public string Country { get; set; }



		[Property (Group = "Organization", Ldap = "title")]
		public string Title { get; set; }

		[Property (Group = "Organization", Ldap = "department,ou,organizationalUnitName")]
		public string Department { get; set; }

		[Property (Group = "Organization", Ldap = "company,o,organizationName")]
		public string Company { get; set; }

		[Property (Group = "Organization", Ldap = "manager")]
		public string Manager { get; set; }


		public bool HasEmail {
			get { return !string.IsNullOrWhiteSpace (Email); }
		}

		public string TitleAndDepartment {
			get {
				return (Title + " " + Department).Trim ();
			}
		}

		public string FirstNameAndInitials {
			get {
				if (!string.IsNullOrWhiteSpace (FirstName)) {
					if (!string.IsNullOrWhiteSpace (Initials)) {
						return FirstName + " " + Initials;
					}
					else {
						return FirstName;
					}
				}
				else {
					return SplitFirstAndLastName ()[0];
				}
			}
		}

		public string SafeLastName {
			get {
				if (!string.IsNullOrWhiteSpace (LastName)) {
					return LastName;
				}
				else {
					return SplitFirstAndLastName ()[1];
				}
			}
		}

		public string SafeDisplayName {
			get {
				if (!string.IsNullOrWhiteSpace (DisplayName)) {
					return DisplayName;
				}
				else if (!string.IsNullOrWhiteSpace (Name)) {
					return Name;
				}
				else if (!string.IsNullOrEmpty (Initials)) {
					return FirstName + " " + Initials + " " + LastName;
				}
				else {
					return FirstName + " " + LastName;
				}
			}
		}

		public Person ()
		{
			TelephoneNumbers = new List<string> ();
			HomeNumbers = new List<string> ();
			MobileNumbers = new List<string> ();
		}

		public override string ToString ()
		{
			return SafeDisplayName;
		}

		static readonly char[] WS = new [] { ' ', '\t', '\r', '\n' };

		string[] SplitFirstAndLastName ()
		{
			var r = new [] { "", "" };
			var parts = SafeDisplayName.Split (WS, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 1) {
				r[0] = parts[0];
			}
			else if (parts.Length == 2) {
				r[0] = parts[0];
				r[1] = parts[1];
			}
			else {
				var li = parts.Length - 1;
				while (li - 1 > 0 && char.IsLower (parts[li-1][0])) {
					li--;
				}
				r[0] = string.Join (" ", parts.Take (li));
				r[1] = string.Join (" ", parts.Skip (li));
			}
			return r;
		}
	}
}

