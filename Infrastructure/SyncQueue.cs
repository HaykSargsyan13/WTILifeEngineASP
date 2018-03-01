using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ASP.Infrastructure
{
    public class SyncQueue<T>
    {
        private readonly ConcurrentQueue<T> _queue;
        private readonly SemaphoreSlim _semaphore;
        private readonly object _sync = new object();

        public event EventHandler<T> NewItem;

        protected virtual void OnNewItem(T e)
        {
            NewItem?.Invoke(this, e);
        }

        public SyncQueue()
        {
            _queue = new ConcurrentQueue<T>();
            _semaphore = new SemaphoreSlim(2);
        }

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);

            if (_semaphore.CurrentCount < 1 || !_semaphore.Wait(0))
                return;

            Task.Run(() =>
            {
                lock (_sync)
                {
                    try
                    {
                        T qItem;
                        while (_queue.TryDequeue(out qItem))
                        {
                            OnNewItem(qItem);
                        }
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
            });
        }


    }
}

