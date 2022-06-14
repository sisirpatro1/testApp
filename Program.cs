using System;
using System.Threading;
using System.Diagnostics;
using System.Management;
namespace HighCPUUsage
{
    public class Worker
    {
        protected volatile bool shouldStop;
        protected Action action;
        public Worker(Action doSomething)
        {
            action = doSomething;
        }

        public void DoWork()
        {
            while (!shouldStop)
            {
                action();
            }
        }

        public void RequestStop()
        {
            shouldStop = true;
        }
    }
    class Program
    {
        static readonly int ALLOCATIONS = 1000;
        static readonly int ALLOCATION_SIZE = 1638;

        static void Main(string[] args)
        {
            ThreadTest();
        }
        //static int FACTORIAL_OF = 100;

        static void ThreadTest()
        {
            List<Thread> threads = new List<Thread>();
            List<Worker> workers = new List<Worker>();
            int n = 1500;// Environment.ProcessorCount;

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("Entered into {0} loop out of {2} loops and the memory allocated as {1}", i, ALLOCATION_SIZE, n);
                Worker worker = new Worker(AllocationTest);
                Thread thread = new Thread(worker.DoWork);
                workers.Add(worker);
                threads.Add(thread);
            }

            threads.ForEach(t => t.Start());

            Console.WriteLine("Press ENTER key to stop...");
            Console.ReadLine();

            workers.ForEach(w => w.RequestStop());
            threads.ForEach(t => t.Join());

            Console.WriteLine("Done");
        }

        static void AllocationTest()
        {
            // Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);
            object[] objects = new object[ALLOCATIONS];
            //Console.WriteLine("Entered into the allocationTest module.");
            for (int i = 0; i < ALLOCATIONS; i++)
            {
                objects[i] = new byte[ALLOCATION_SIZE];
            }
        }
    }
}