using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Queue
{
    public abstract class QueueBase<T>
    {
        private static ConcurrentQueue<T> _queue;

        static QueueBase()
        {
            _queue = new ConcurrentQueue<T>();
        }

        private Action<T> _listener;

        public QueueBase(Action<T> listener)
            : this()
        {
            _listener = listener;
            Start();
        }

        public QueueBase()
        {
            // send-only
        }

        public int Count => _queue.Count;

        public void Clear() => _queue.Clear();

        public void Send(T message)
        {
            _queue.Enqueue(message);
        }

        private async void Start()
        {
            while (_listener != null)
            {
                await Task.Delay(100);
                if (_queue.TryDequeue(out var value))
                {
                    _listener(value);
                }
            }
        }
    }
}
