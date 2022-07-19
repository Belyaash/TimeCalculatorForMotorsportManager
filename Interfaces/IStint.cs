namespace TimeCalculatorForMotorsportManager.Interfaces;

public interface IStint
{
    public TimeSpan LapTime { get; }
    public int Laps { get; }
    public double Fuel { get; }
    public TimeSpan FuelPenaltyPerUnit { get; internal set; }

    public TimeSpan StintTime();
}