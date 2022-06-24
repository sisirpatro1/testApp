using System;
using System.Threading;
using System.Diagnostics;
using System.Management;
using System.Runtime;
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
        static List<Thread> threads = new List<Thread>();
        static List<Worker> workers = new List<Worker>();
        static Worker? worker;
        static Thread? thread;
        static readonly int n = 1500;
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the Console application.");
            ThreadTest();
        }
        //static int FACTORIAL_OF = 100;

        static void ThreadTest()
        {
            
            // Environment.ProcessorCount;

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("Entered into {0} loop out of {2} loops and the memory allocated as {1}", i, ALLOCATION_SIZE, n);
                worker = new Worker(AllocationTest);
                thread = new Thread(worker.DoWork);
                workers.Add(worker);
                threads.Add(thread);                
                //thread.Start();
                //Thread.Sleep(100);
            }

            threads.ForEach(t => t.Start());

            Console.WriteLine("Press ENTER key to stop...");

            Thread.Sleep(900);
            Console.WriteLine("Thread sleep completed");
            Deallocate();
            Console.WriteLine("All threads are aborted");
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

        static void Deallocate()
        {
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("Workers removed {0}.", i);
                workers.Remove(worker);
                threads.Remove(thread);
                GC.Collect();
            }
            Console.WriteLine("Deallocating the memory here.");
            for (int i = 0; i < ALLOCATIONS; i++)
            {
                GC.Collect();
            }
        }
    }
}