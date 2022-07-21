using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleInput;
using TimeCalculatorForMotorsportManager.Interfaces;

namespace TimeCalculatorForMotorsportManager;

internal class Stint : IStint
{
    public TimeSpan LapTime { get; }

    public int Laps { get; }


    private double _fuel;
    public double Fuel
    {
        get => _fuel;
        private set => _fuel = Laps > value ? Laps : value;
    }

    internal TimeSpan FuelPenaltyPerUnit { get; set; }

    TimeSpan IStint.FuelPenaltyPerUnit
    {
        get => this.FuelPenaltyPerUnit;
        set => this.FuelPenaltyPerUnit = value;
    }

    public Stint(TimeSpan lapTime, int laps, double fuel)
    {
        LapTime = lapTime;
        Laps = laps;
        _fuel = fuel;
        FuelPenaltyPerUnit = TimeSpan.Zero;
    }
    public Stint(TimeSpan lapTime, int laps, double fuel, TimeSpan fuelPenaltyPerUnit)
    {
        LapTime = lapTime;
        Laps = laps;
        _fuel = fuel;
        FuelPenaltyPerUnit = fuelPenaltyPerUnit;
    }

    public TimeSpan StintTime()
    {
        TimeSpan stintTime = LapTime.Multiply(Laps);
        stintTime += GetAllPenaltyTime();
        return stintTime;
    }

    private TimeSpan GetAllPenaltyTime()
    {
        return FuelPenaltyPerUnit.Multiply((2 * _fuel - (Laps-1)) * Laps / 2);
    }

    internal static IStint CreateStint(bool isFuelPenaltySet, TimeSpan fuelPenaltyPerUnit)
    {
        return isFuelPenaltySet
            ? Stint.CreateStintWithoutInputFuelPenalty(fuelPenaltyPerUnit)
            : Stint.CreateFullStintInConsole();
    }

    private static Stint CreateFullStintInConsole()
    {
        var stint = CreateStintWithoutInputFuelPenalty(TimeSpan.Zero);
        var fuelPenaltyPerUnit = Input.CreateMinutesSecondsMillisecondsTimeSpan("Write a penalty for fuel per lap");
        stint.FuelPenaltyPerUnit = fuelPenaltyPerUnit;
        return stint;
    }

    private static Stint CreateStintWithoutInputFuelPenalty(TimeSpan fuelPenaltyPerUnit)
    {
        TimeSpan lapTime = Input.CreateMinutesSecondsMillisecondsTimeSpan("Write a medium lap time");
        int laps = Input.CreateNumber<int>("Write a count of laps", MinMax<int>.HigherThan(1));
        double fuel = Input.CreateNumber<double>("Write a fuel count", MinMax<double>.HigherThan(laps));
        return new Stint(lapTime, laps, fuel, fuelPenaltyPerUnit);
    }
}