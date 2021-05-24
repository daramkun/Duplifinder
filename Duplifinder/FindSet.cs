using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Duplifinder
{
	public enum FindingMethod
	{
		FilenameSimiliarity,
		HashValueSameness,
	}

	class FindSet
	{
		public FindingMethod FindingMethod { get; }
		public IList<string> Filenames { get; } = new ObservableCollection<string>();

		public FindSet(FindingMethod findingMethod)
		{
			FindingMethod = findingMethod;
		}
	}
}
