using ArgusReduxCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgusGodotVisualizer
{
	public static class ArgusCoreWrapper
	{
		public static readonly IServiceProvider ServiceProvider;
		public static readonly TrackerManager TrackerManager;
		public static readonly IUDPNetworkService UDPNetwork;

		static ArgusCoreWrapper()
		{
			ServiceProvider = ArgusCoreService.CreateDefaultSetup();
			TrackerManager = ServiceProvider.GetRequiredService<TrackerManager>();
			UDPNetwork = ServiceProvider.GetRequiredService<IUDPNetworkService>();

			UDPNetwork.StartListening();
		}
	}
}
