using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobaccoTray.Contracts;

namespace TobaccoTray
{
	class FlamableComparer : IEqualityComparer<IFlameable>
	{
		public bool Equals(IFlameable x, IFlameable y)
		{
			return x.GetType() == y.GetType();
		}
		public int GetHashCode(IFlameable x)
		{
			return x.GetHashCode();
		}
	}
}
