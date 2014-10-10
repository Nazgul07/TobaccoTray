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
		public List<IFlameable> Plugins { get; set; }

		public System.ComponentModel.Container Components { get; set; }

		public NotifyIcon NotifyIcon { get; set; }

		public TobaccoTray()
		{
			InitializeContext();
		}

		private void InitializeContext()
		{
			Plugins = new List<IFlameable>();
			Components = new System.ComponentModel.Container();
			NotifyIcon = new NotifyIcon(Components)
			{
				ContextMenuStrip = new ContextMenuStrip(),
				Icon = Resources.Resources.Icon,
				Text = "Tobacco Tray",
				Visible = true
			};
			Ignite();
			NotifyIcon.ContextMenuStrip.Opening += ReIgnite;
		}

		private void Extinguish()
		{
			Plugins.ForEach(x => x.Extinguish());
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
					Plugins.Add(flamable);
					flamable.Ingnite(NotifyIcon);
				}
			}
			NotifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Quit", null, Exit, "Quit"));
		}
		private void ReIgnite(object sender, System.ComponentModel.CancelEventArgs e)
		{
			bool needsToExtinguish = false;
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

					if (!Plugins.Contains(flamable, new FlamableComparer()))
					{
						needsToExtinguish = true;
					}
				}
			}
			if (needsToExtinguish)
			{
				Extinguish();
				Ignite();
			}
		}
		void Exit(object sender, EventArgs e)
		{
			Extinguish();
			ExitThread();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && Components != null) { Components.Dispose(); }
		}

		protected override void ExitThreadCore()
		{
			NotifyIcon.Visible = false; 
			base.ExitThreadCore();
		}
	}
	
}
