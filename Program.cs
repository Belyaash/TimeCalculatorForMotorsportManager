namespace TimeCalculatorForMotorsportManager;
using ConsoleInput;
using TimeCalculatorForMotorsportManager.Interfaces;

public static class Program
{
    public static void Main()
    {
        var a = new Stint(new TimeSpan(0, 0, 1, 0), 20, 1800, new TimeSpan(0, 0, 0, 0, 1));
        Console.WriteLine(a.StintTime());
        IPitStopStrategy b = PitStopStrategy.CreateStrategiesAndReturnFastest();
        Console.WriteLine($"Fastest strategy time is {b.TotalTime}");
    }
}