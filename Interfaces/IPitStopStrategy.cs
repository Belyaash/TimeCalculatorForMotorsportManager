namespace TimeCalculatorForMotorsportManager.Interfaces;

public interface IPitStopStrategy
{
    TimeSpan TotalTime { get; }

    static IPitStopStrategy CreateStrategiesAndReturnFastest()
    {
        return PitStopStrategy.CreateStrategiesAndReturnFastest();
    }
}