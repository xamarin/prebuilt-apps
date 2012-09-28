using System;
using System.ComponentModel;

namespace EmployeeDirectory
{
	public class ViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public ViewModel ()
		{
		}

		protected virtual void OnPropertyChanged (string name)
		{
			var ev = PropertyChanged;
			if (ev != null) {
				ev (this, new PropertyChangedEventArgs (name));
			}
		}
	}
}

