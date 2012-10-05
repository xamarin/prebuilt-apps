using System;
using System.Collections.Generic;

namespace EmployeeDirectory.Data
{
	public interface IFavoritesRepository
	{
		IEnumerable<Person> GetAll ();
		Person FindById (string id);
		bool IsFavorite (Person person);
		void InsertOrUpdate (Person person);
		void Delete (Person person);
	}
}

