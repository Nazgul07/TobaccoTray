using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TobaccoTray.Contracts;

namespace ExamplePlugin
{
    public class Example : IFlameable
    {
		public void Ingnite(NotifyIcon notifyIcon)
		{
			notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Example Plugin", null, Popup, "Example Plugin"));
		}

		private void Popup(object sender, EventArgs e)
		{
			MessageBox.Show("Hello World!");
		}


		public void Extinguish()
		{
			MessageBox.Show("Goodbye World!");
		}
	}
}
