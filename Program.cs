internal class Program
{
    private static int _num = 0;
    private static Lock _lock = new Lock();

    private static void Thread1()
    {
        for (int i = 0; i < 1000; i++)
        {
            _lock.Acquire();
            _num++;
            _lock.Release();
        }
    }

    private static void Thread2()
    {
        for (int i = 0; i < 1000; i++)
        {
            _lock.Acquire();
            _num--;
            _lock.Release();
        }
    }

    private static void Main(string[] args)
    {
        Task thread1 = new Task(Thread1);
        Task thread2 = new Task(Thread2);
        thread1.Start();
        thread2.Start();

        Task.WaitAll(thread1, thread2);

        Console.WriteLine(_num);
    }
}

class Lock
{
    // bool => Kernel
    private AutoResetEvent _available = new AutoResetEvent(true);

    public void Acquire()
    {
        _available.WaitOne(); // Try entrance
    }

    public void Release()
    {
        _available.Set();
    }
}