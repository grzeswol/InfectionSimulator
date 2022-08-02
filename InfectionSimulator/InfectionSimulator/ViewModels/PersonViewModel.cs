using Simulator.Interfaces;

namespace InfectionSimulator.ViewModels
{
    internal class PersonViewModel : BaseViewModel, IPerson
    {
        public bool IsHealthy { get; private set; }
        public double Immunity { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsSelected { get; set; }
        public int DaysInfected { get; set; }
        public int TimesInfected { get; set; }

        public PersonViewModel()
        {
            IsHealthy = true;
            IsSelected = false;
        }

        public void SetPersonInfected()
        {
            IsHealthy = false;
            TimesInfected++;
        }

        public void SetPersonHealthy()
        {
            IsHealthy = true;
            DaysInfected = 0;
        }

        public void IncreaseImmunity(double immunityIncrease)
        {
            Immunity += immunityIncrease;
        }
    }
}