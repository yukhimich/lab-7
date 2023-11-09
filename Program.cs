using System;
using System.Collections.Generic;
using System.Linq;

public class Calculator<T>
{
    public delegate T Operation(T a, T b);

    public Operation Add { get; set; }
    public Operation Subtract { get; set; }
    public Operation Multiply { get; set; }
    public Operation Divide { get; set; }

    public Calculator()
    {
        Add = (a, b) => (dynamic)a + (dynamic)b;
        Subtract = (a, b) => (dynamic)a - (dynamic)b;
        Multiply = (a, b) => (dynamic)a * (dynamic)b;
        Divide = (a, b) => (dynamic)a / (dynamic)b;
    }

    public T PerformOperation(Operation operation, T a, T b)
    {
        return operation(a, b);
    }
}

class Program
{
    static void Main()
    {
        Calculator<int> intCalculator = new Calculator<int>();
        Console.WriteLine(intCalculator.PerformOperation(intCalculator.Add, 5, 3));        // 8
        Console.WriteLine(intCalculator.PerformOperation(intCalculator.Subtract, 10, 4));  // 6
        Console.WriteLine(intCalculator.PerformOperation(intCalculator.Multiply, 7, 2));    // 14
        Console.WriteLine(intCalculator.PerformOperation(intCalculator.Divide, 8, 2));      // 4

        Calculator<double> doubleCalculator = new Calculator<double>();
        Console.WriteLine(doubleCalculator.PerformOperation(doubleCalculator.Add, 3.5, 1.5));         // 5.0
        Console.WriteLine(doubleCalculator.PerformOperation(doubleCalculator.Subtract, 10.5, 4.2));  // 6.3
        Console.WriteLine(doubleCalculator.PerformOperation(doubleCalculator.Multiply, 2.5, 3.0));   // 7.5
        Console.WriteLine(doubleCalculator.PerformOperation(doubleCalculator.Divide, 8.0, 2.0));     // 4.0
    }
}
using System;
using System.Collections.Generic;

public class Repository<T>
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        items.Add(item);
    }

    public List<T> Find(Predicate<T> criteria)
    {
        return items.FindAll(criteria);
    }
}

class Program
{
    static void Main()
    {
        Repository<int> intRepository = new Repository<int>();
        intRepository.Add(5);
        intRepository.Add(10);
        intRepository.Add(15);
        intRepository.Add(20);

        List<int> result = intRepository.Find(item => item > 10);
        foreach (int item in result)
        {
            Console.WriteLine(item);
        }

        Repository<string> stringRepository = new Repository<string>();
        stringRepository.Add("apple");
        stringRepository.Add("banana");
        stringRepository.Add("cherry");
        stringRepository.Add("date");

        List<string> result2 = stringRepository.Find(item => item.StartsWith("b"));
        foreach (string item in result2)
        {
            Console.WriteLine(item);
        }
    }
}
using System;
using System.Collections.Generic;

public class FunctionCache<TKey, TResult>
{
    private Dictionary<TKey, TResult> cache = new Dictionary<TKey, TResult>();
    private Func<TKey, TResult> function;
    private TimeSpan cacheDuration;

    public FunctionCache(Func<TKey, TResult> func, TimeSpan duration)
    {
        function = func;
        cacheDuration = duration;
    }

    public TResult GetResult(TKey key)
    {
        if (cache.ContainsKey(key) && DateTime.Now - cache[key] < cacheDuration)
        {
            return cache[key];
        }
        else
        {
            TResult result = function(key);
            cache[key] = result;
            return result;
        }
    }
}

class Program
{
    static void Main()
    {
        Func<int, double> squareRootFunction = x => Math.Sqrt(x);
        FunctionCache<int, double> cache = new FunctionCache<int, double>(squareRootFunction, TimeSpan.FromMinutes(1));

        double result = cache.GetResult(25);
        Console.WriteLine(result);  // Результат обчислення кореня квадратного з 25

        // Повторне викликання через короткий час поверне кешований результат
        result = cache.GetResult(25);
        Console.WriteLine(result);  // Значення повернуте з кешу

        // Зачекайте більше, ніж годину, щоб дані у кеші протухли
        System.Threading.Thread.Sleep(TimeSpan.FromMinutes(61));

        // Тепер результат повинен бути переспособлений
        result = cache.GetResult(25);
        Console.WriteLine(result);  // Результат обчислення кореня квадратного з 25
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

public class TaskScheduler<TTask, TPriority> where TPriority : IComparable<TPriority>
{
    private SortedDictionary<TPriority, Queue<TTask>> tasksByPriority = new SortedDictionary<TPriority, Queue<TTask>>();

    public void AddTask(TTask task, TPriority priority)
    {
        if (!tasksByPriority.ContainsKey(priority))
        {
            tasksByPriority[priority] = new Queue<TTask>();
        }
        tasksByPriority[priority].Enqueue(task);
    }

    public void ExecuteNext(TaskExecution<TTask> execution)
    {
        if (tasksByPriority.Count > 0)
        {
            var highestPriority = tasksByPriority.Keys.Last();
            var taskQueue = tasksByPriority[highestPriority];
            var nextTask = taskQueue.Dequeue();
            if (taskQueue.Count == 0)
            {
                tasksByPriority.Remove(highestPriority);
            }
            execution(nextTask);
        }
    }

    public void ReturnToPool(TaskExecution<TTask> poolFunction, TTask task, TPriority priority)
    {
        AddTask(task, priority);
        poolFunction(task);
    }
}

public delegate void TaskExecution<TTask>(TTask task);

class Program
{
    static void Main()
    {
        TaskScheduler<string, int> taskScheduler = new TaskScheduler<string, int>();

        taskScheduler.AddTask("Task A", 3);
        taskScheduler.AddTask("Task B", 2);
        taskScheduler.AddTask("Task C", 1);

        Console.WriteLine("Executing tasks:");
        taskScheduler.ExecuteNext(task => Console.WriteLine($"Executing {task}"));

        taskScheduler.ReturnToPool(task => Console.WriteLine($"Returning {task} to the pool"), "Task B", 2);

        Console.WriteLine("Executing tasks after returning a task to the pool:");
        taskScheduler.ExecuteNext(task => Console.WriteLine($"Executing {task}"));
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
