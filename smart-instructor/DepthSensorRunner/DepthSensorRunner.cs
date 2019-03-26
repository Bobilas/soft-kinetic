using Po.Forms.Threading;
using System;
using System.Threading;
using DepthSenseWrapper;
using System.Threading.Tasks;

namespace SmartInstructor.Depth
{
    class DepthSensorRunner
    {
        public DepthSensorRunner()
        {
            Run();
        }

        public void Run()
        {
            Task.Run(() =>
            {
                try
                {
                    _depthSensor = new DepthSensor();
                    _depthSensor.Run();
                }
                catch { }
                Thread.Sleep(50);
            });
        }

        public short[,] GetDepthMap()
        {
            short[,] map = _depthSensor?.GetDepthMap();
            if (map?.Length == 0)
            {
                return null;
            }
            return map;
        }

        private DepthSensor _depthSensor;
    }
}
