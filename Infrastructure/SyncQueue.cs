using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ASP.Infrastructure
{
    internal class SyncQueue<T>
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
                        while (_queue.TryDequeue(out var qItem))
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

