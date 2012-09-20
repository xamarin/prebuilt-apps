using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmployeeDirectory.Data;

namespace EmployeeDirectory
{
	public interface IDirectoryService
	{
		Task<IList<Person>> SearchAsync (Filter filter, int sizeLimit);
	}


}

