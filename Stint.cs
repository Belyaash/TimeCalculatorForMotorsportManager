using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
}