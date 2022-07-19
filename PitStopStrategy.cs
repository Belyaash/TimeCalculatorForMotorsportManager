using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleInput;
using TimeCalculatorForMotorsportManager.Interfaces;

namespace TimeCalculatorForMotorsportManager;
internal class PitStopStrategy
{
    private TimeSpan TotalTime { get; set; }
    private TimeSpan _fuelPenaltyPerUnit = TimeSpan.Zero;

    private TimeSpan FuelPenaltyPerUnit
    {
        get => _fuelPenaltyPerUnit;
        set
        {
            _fuelPenaltyPerUnit = value;
            _isFuelPenaltySet = true;
        }
    }

    private bool _isFuelPenaltySet = false;


    public PitStopStrategy()
    {
        TotalTime = TimeSpan.Zero;
    }

    private PitStopStrategy(TimeSpan fuelPenaltyPerUnit)
    {
        TotalTime = TimeSpan.Zero;
        FuelPenaltyPerUnit = fuelPenaltyPerUnit;
        _isFuelPenaltySet = true;
    }

    public static PitStopStrategy CreateStrategy()
    {
        var strategy = new PitStopStrategy();
        strategy.CreateAllStintsInStrategy(strategy.FuelPenaltyPerUnit);
        return strategy;
    }

    public static PitStopStrategy CreateStrategy(TimeSpan fuelPenaltyPerUnit)
    {
        var strategy = new PitStopStrategy(fuelPenaltyPerUnit);
        strategy.CreateAllStintsInStrategy(strategy.FuelPenaltyPerUnit);
        return strategy;
    }

    public static PitStopStrategy CreateStrategiesAndFindFastest()
    {
        var strategy = PitStopStrategy.CreateStrategy();
        Console.WriteLine($"Current strategy time is {strategy.TotalTime}");
        PitStopStrategy fastestStrategy = AddNewStrategiesAndFindFastestIfUserWant(strategy);
        return fastestStrategy;
    }

    private void CreateAllStintsInStrategy(TimeSpan fuelPenaltyPerUnit)
    {
        var a = CreateStint(_isFuelPenaltySet).StintTime();
        TotalTime += a;
        Console.WriteLine(a);
        Console.WriteLine(fuelPenaltyPerUnit);
        AddNewStintsIfUserWant(fuelPenaltyPerUnit);
    }

    private IStint CreateStint(bool isFuelPenaltySetted)
    {
        return isFuelPenaltySetted 
            ? CreateStintWithoutInputFuelPenalty() 
            : CreateFullStintInConsole();
    }

    private IStint CreateFullStintInConsole()
    {
        var stint = CreateStintWithoutInputFuelPenalty();
        FuelPenaltyPerUnit = Input.CreateMinutesSecondsMillisecondsTimeSpan("Write a penalty for fuel per lap");
        stint.FuelPenaltyPerUnit = FuelPenaltyPerUnit;
        return stint;
    }

    private IStint CreateStintWithoutInputFuelPenalty()
    {
        TimeSpan lapTime = Input.CreateMinutesSecondsMillisecondsTimeSpan("Write a medium lap time");
        int laps = Input.CreateNumber<int>("Write a count of laps", MinMax<int>.HigherThan(1));
        double fuel = Input.CreateNumber<double>("Write a fuel count", MinMax<double>.HigherThan(laps));
        return new Stint(lapTime, laps, fuel, FuelPenaltyPerUnit);
    }


    private void AddNewStintsIfUserWant(TimeSpan fuelPenaltyPerUnit)
    {
        bool isAdd = true;
        do
        {
            isAdd = AskToAdd("Do you want to add stint? (y/n)");
            if (isAdd)
            {
                TotalTime += Input.CreateMinutesSecondsMillisecondsTimeSpan("Enter Pit-Stop time");
                TotalTime += CreateStint(true).StintTime();
            }
        } while (isAdd);
    }

    private static bool AskToAdd(string message)
    {
        Console.WriteLine(message);
        bool isAddCommand = GetYesNoByConsoleInput();
        return isAddCommand;
    }

    private static bool GetYesNoByConsoleInput()
    {
        ConsoleKeyInfo key = Console.ReadKey(true);
        switch (key.KeyChar)
        {
            case 'y' or 'Y':
                return true;
            case 'n' or 'N':
                return false;
            default:
                Console.Beep();
                return GetYesNoByConsoleInput();
        }
    }
    private static PitStopStrategy AddNewStrategiesAndFindFastestIfUserWant(PitStopStrategy strategy)
    {
        var fastestStrategy = strategy;
        bool isStrategiesCreationEnd = true;
        do
        {
            isStrategiesCreationEnd = AskToAdd("Do you want to add strategy? (y/n)");
            if (isStrategiesCreationEnd)
            {
                var newStrategy = PitStopStrategy.CreateStrategy(strategy.FuelPenaltyPerUnit);
                fastestStrategy = ComparePitStrategy(fastestStrategy, newStrategy);
            }
        } while (isStrategiesCreationEnd);

        return fastestStrategy;
    }

    private static PitStopStrategy ComparePitStrategy(PitStopStrategy fastestStrategy, PitStopStrategy newStrategy)
    {
        int compareTo = fastestStrategy.TotalTime.CompareTo(newStrategy.TotalTime);
        switch (compareTo)
        {
            case -1:
                Console.WriteLine($"Last strategy is faster on {newStrategy.TotalTime - fastestStrategy.TotalTime}");
                return newStrategy;
            default:
                Console.WriteLine($"Last strategy is slower on {fastestStrategy.TotalTime - newStrategy.TotalTime}");
                return fastestStrategy;
        }
    }
}