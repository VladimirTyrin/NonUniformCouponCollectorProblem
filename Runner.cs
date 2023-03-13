using System.Buffers;

public static class Runner
{
    public static double GetExpectation(int n, int runCount, int threadCount, double[] weights)
    {
        var runCountPerThread = (int)Math.Ceiling(1.0 * runCount / threadCount);

        var threadContexts = new List<ThreadContext>(threadCount);
        var threads = new List<Thread>(threadCount);

        for (var i = 0; i < threadCount; ++i)
        {
            threadContexts.Add(new ThreadContext(n, runCountPerThread, weights));
            threads.Add(new Thread(ThreadProc));
        }
        
        for (var i = 0; i < threadCount; ++i)
        {
            threads[i].Start(threadContexts[i]);
        }
        
        threads.ForEach(t => t.Join());
        var sum = threadContexts.Sum(c => c.GetSum());

        return sum * 1.0 / threadCount / runCountPerThread;
    }

    static void ThreadProc(object? o)
    {
        var context = (ThreadContext)o!;

        for (var i = 0; i < context.RunCount; ++i)
        {
            context.Add(RunToCompletion(context.N, context.Picker));
        }
    }
    
    private static int RunToCompletion(int n, IndexPicker picker)
    {
        var pooledArray = ArrayPool<int>.Shared.Rent(n);
        var values = pooledArray.AsSpan().Slice(0, n);
        values.Clear();
        var count = 0;
        while (! values.AreAllPositive())
        {
            count++;
            var index = picker.Pick();
            values[index] += 1;
        }

        ArrayPool<int>.Shared.Return(pooledArray);
        return count;
    }

    private class ThreadContext
    {
        public readonly int N;
        public readonly int RunCount;
        public readonly IndexPicker Picker;
        private long _sum;

        public ThreadContext(int n, int runCount, double[] weights)
        {
            N = n;
            RunCount = runCount;
            Picker = new IndexPicker(weights, new Random());
        }

        public long GetSum() => _sum;

        public void Add(int count) => _sum += count;
    }
}