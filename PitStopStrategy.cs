using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleInput;
using TimeCalculatorForMotorsportManager.Interfaces;

namespace TimeCalculatorForMotorsportManager;
internal class PitStopStrategy : IPitStopStrategy
{
    public TimeSpan TotalTime { get; private set; }

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


    private PitStopStrategy()
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
        strategy.CreateAllStintsInStrategy();
        Console.WriteLine($"Current strategy time is {strategy.TotalTime}");
        Console.WriteLine();
        return strategy;
    }

    public static PitStopStrategy CreateStrategy(TimeSpan fuelPenaltyPerUnit)
    {
        var strategy = new PitStopStrategy(fuelPenaltyPerUnit);
        strategy.CreateAllStintsInStrategy();
        Console.WriteLine($"Current strategy time is {strategy.TotalTime}");
        Console.WriteLine();
        return strategy;
    }

    private void CreateAllStintsInStrategy()
    {
        var stint = Stint.CreateStint(_isFuelPenaltySet, FuelPenaltyPerUnit);
        FuelPenaltyPerUnit = stint.FuelPenaltyPerUnit;
        TotalTime += stint.StintTime();
        AddNewStintsIfUserWant(FuelPenaltyPerUnit);
    }

    private void AddNewStintsIfUserWant(TimeSpan fuelPenaltyPerUnit)
    {
        bool isAdd;
        do
        {
            isAdd = Input.CreateBoolean("Do you want to add stint? (y/n)");
            if (isAdd)
            {
                TotalTime += Input.CreateMinutesSecondsMillisecondsTimeSpan("Enter Pit-Stop time");
                TotalTime += Stint.CreateStint(true, fuelPenaltyPerUnit).StintTime();
            }
        } while (isAdd);
    }



    public static PitStopStrategy CreateStrategiesAndReturnFastest()
    {
        var strategiesList = CreatePitStopStrategiesList();
        PitStopStrategy fastestStrategy = FindFastestStrategy(strategiesList);
        return fastestStrategy;
    }

    private static List<PitStopStrategy> CreatePitStopStrategiesList()
    {
        var strategiesList = new List<PitStopStrategy>();
        bool isStrategiesCreationNotEnd;
        do
        {
            strategiesList.Add(strategiesList.Count == 0
                ? CreateStrategy()
                : CreateStrategy(strategiesList[0].FuelPenaltyPerUnit));
            isStrategiesCreationNotEnd = Input.CreateBoolean("Do you want to add strategy? (y/n)");
        } while (isStrategiesCreationNotEnd);

        return strategiesList;
    }

    private static PitStopStrategy FindFastestStrategy(IReadOnlyList<PitStopStrategy> strategiesList)
    {
        var fastestStrategy = strategiesList[0];
        for (int i = 1; i < strategiesList.Count; i++)
        {
            fastestStrategy = ComparePitStrategies(fastestStrategy, strategiesList[i]);
        }

        return fastestStrategy;
    }

    private static PitStopStrategy ComparePitStrategies(PitStopStrategy fastestStrategy, PitStopStrategy newStrategy)
    {
        int compareTo = fastestStrategy.TotalTime.CompareTo(newStrategy.TotalTime);
        return compareTo == 1 ? newStrategy : fastestStrategy;
    }
}