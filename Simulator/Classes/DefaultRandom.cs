using Simulator.Interfaces;
using System;

namespace Simulator.Classes
{
    public class DefaultRandom : IRandom
    {
        public double NextDouble()
        {
            return new Random().NextDouble();
        }

        public int NextInt(int minValue, int maxValue)
        {
            return new Random().Next(minValue, maxValue);
        }
    }
}