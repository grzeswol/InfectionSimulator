namespace Simulator.Interfaces
{
    public interface IRandom
    {
        double NextDouble();

        int NextInt(int minValue, int maxValue);
    }
}