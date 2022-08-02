using Simulator.Interfaces;

namespace Simulator.Tests.Classes
{
    internal class TestPerson : IPerson
    {
        public bool IsHealthy { get; private set; }
        public double Immunity { get; set; }
        int IPerson.DaysInfected { get; set; }
        int IPerson.TimesInfected { get; set; }

        public void SetPersonInfected()
        {
            IsHealthy = false;
        }

        void IPerson.IncreaseImmunity(double immunityIncrease)
        {
            throw new NotImplementedException();
        }

        void IPerson.SetPersonHealthy()
        {
            throw new NotImplementedException();
        }
    }
}