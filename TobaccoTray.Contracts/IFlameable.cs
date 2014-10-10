using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TobaccoTray.Contracts
{
	public interface IFlameable
	{
		void Ingnite(NotifyIcon notifyIcon);
		void Extinguish();
	}
}
