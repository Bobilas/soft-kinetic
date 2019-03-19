using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartInstructor
{
	class FramesCounter
	{
		public FramesCounter()
		{
			_watch.Start();
		}

		private List<long> _frames = new List<long>(100);
		private readonly Stopwatch _watch = new Stopwatch();
		private object _lock = new object();

		private bool TryLock()
		{
			return Monitor.TryEnter(_lock, 10);
		}
		private void Unlock()
		{
			Monitor.Exit(_lock);
		}

		public void Update()
		{
			if (!TryLock())
			{
				return;
			}

			if (_frames.Count == 0)
			{
				_watch.Restart();
			}
			_frames.Add(_watch.ElapsedMilliseconds);
			_frames = _frames.Where(e => _watch.ElapsedMilliseconds - e < 1000).ToList();

			Unlock();
		}
		public int GetFps()
		{
			if (!TryLock())
			{
				return -1;
			}

			_frames = _frames.Where(e => _watch.ElapsedMilliseconds - e < 1000).ToList();
			int fps = _frames.Count * 1000 / (int)Math.Min(_watch.ElapsedMilliseconds, 1000);
			Unlock();

			return fps > 0 ? fps : -1;
		}
		public void Reset()
		{
			_frames = new List<long>();
			_watch.Restart();
		}
	}
}
