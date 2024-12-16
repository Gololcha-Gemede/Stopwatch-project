using System;
using System.Threading;

namespace my_csharp
{
    public class Stopwatch
    {
        
        private TimeSpan _timeElapsed;
        private bool _isRunning;
        private Timer _timer;

       
        public delegate void StopwatchEventHandler(string message);
        public event StopwatchEventHandler OnStarted;
        public event StopwatchEventHandler OnStopped;
        public event StopwatchEventHandler OnReset;

       
        public Stopwatch()
        {
            _timeElapsed = TimeSpan.Zero;
            _isRunning = false;
        }

        // Methods
        public void Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _timer = new Timer(Tick, null, 0, 1000); 
                OnStarted?.Invoke("Stopwatch Started!");
            }
            else
            {
                Console.WriteLine("Stopwatch is already running.");
            }
        }

        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _timer.Dispose();
                OnStopped?.Invoke("Stopwatch Stopped!");
            }
            else
            {
                Console.WriteLine("Stopwatch is not running.");
            }
        }

        public void Reset()
        {
            _timeElapsed = TimeSpan.Zero;
            if (_isRunning)
            {
                _timer.Dispose();
                _isRunning = false;
            }
            OnReset?.Invoke("Stopwatch Reset!");
        }

        private void Tick(object state)
        {
            _timeElapsed = _timeElapsed.Add(TimeSpan.FromSeconds(1));
            Console.Clear();
            Console.WriteLine($"Time Elapsed: {_timeElapsed}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            // Subscribe to events
            stopwatch.OnStarted += message => Console.WriteLine(message);
            stopwatch.OnStopped += message => Console.WriteLine(message);
            stopwatch.OnReset += message => Console.WriteLine(message);

            bool exit = false;
            Console.WriteLine("Stopwatch Console Application");
            Console.WriteLine("Press S to Start, T to Stop, R to Reset, Q to Quit.");

            while (!exit)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        case ConsoleKey.S:
                            stopwatch.Start();
                            break;
                        case ConsoleKey.T:
                            stopwatch.Stop();
                            break;
                        case ConsoleKey.R:
                            stopwatch.Reset();
                            break;
                        case ConsoleKey.Q:
                            stopwatch.Stop();
                            exit = true;
                            Console.WriteLine("Exiting application...");
                            break;
                        default:
                            Console.WriteLine("Invalid input. Please press S, T, R, or Q.");
                            break;
                    }
                }
                Thread.Sleep(100); // Prevent CPU overuse
            }
        }
    }
}
