using DepthSenseWrapper;
using Forms.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartInstructor
{
	class DepthSensorThread : ThreadService
	{
		public DepthSensorThread() : base("depth-sensor-thread")
		{
			DisableThreadSleep = true;

			Update += new EventHandler<ThreadService>((obj, service) =>
			{
				_depthSensor = new DepthSensor();
				_depthSensor.Run();
				Thread.Sleep(100);
			});
		}

		public short[,] GetDepthMap()
		{
			if (IsRunning)
			{
				short[,] map = _depthSensor?.GetDepthMap();
				if (map?.Length == 0)
				{
					return null;
				}
				return map;
			}
			return null;
		}

		private DepthSensor _depthSensor;
	}
}
