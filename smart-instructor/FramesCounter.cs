using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

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
            RemoveOldFrames();

            Unlock();
        }
        public int GetFps()
        {
            if (!TryLock())
            {
                return 0;
            }

            RemoveOldFrames();
            int fps = _frames.Count * 1000 / (int)Math.Min(_watch.ElapsedMilliseconds + 1, 1000);
            Unlock();

            return fps;
        }
        public void Reset()
        {
            _frames = new List<long>();
            _watch.Restart();
        }

        private void RemoveOldFrames() => _frames.RemoveAll(e => _watch.ElapsedMilliseconds - e >= 1000);
    }
}
