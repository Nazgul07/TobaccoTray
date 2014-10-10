using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using TobaccoTray.Contracts;

namespace TobaccoTray
{
	public class TobaccoTray : ApplicationContext
	{
		public System.ComponentModel.Container components { get; set; }

		public NotifyIcon NotifyIcon { get; set; }

		public TobaccoTray()
		{
			InitializeContext();
		}
		private void InitializeContext()
		{
			components = new System.ComponentModel.Container();
			NotifyIcon = new NotifyIcon(components)
			{
				ContextMenuStrip = new ContextMenuStrip(),
				Icon = Resources.Resources.Icon,
				Text = "Tobacco Tray",
				Visible = true
			};
			NotifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
		}

		private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			NotifyIcon.ContextMenuStrip.Items.Clear();
			e.Cancel = false;
			NotifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Quit", null, Exit, "Quit"));
			Ignite();
		}

		private void Ignite()
		{
			foreach (string assembly in Directory.GetFiles("Plugins", "*.dll", SearchOption.TopDirectoryOnly))
			{
				Assembly plugin = Assembly.LoadFrom(assembly);
				// Get the type to use.
				var results = from type in plugin.GetTypes()
							  where typeof(IFlameable).IsAssignableFrom(type)
							  select type;
				foreach (Type type in results)
				{
					IFlameable flamable = Activator.CreateInstance(type) as IFlameable;
					flamable.Ingnite(NotifyIcon);
				}
			}
		}

		void Exit(object sender, EventArgs e)
		{
			ExitThread();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null) { components.Dispose(); }
		}

		protected override void ExitThreadCore()
		{
			//if (mainForm != null) { mainForm.Close(); }
			NotifyIcon.Visible = false; 
			base.ExitThreadCore();
		}
	}

}
