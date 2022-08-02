using Simulator.Interfaces;

namespace InfectionSimulator.ViewModels
{
    internal class PersonViewModel : BaseViewModel, IPerson
    {
        #region Properties

        public int DaysInfected { get; set; }
        public double Immunity { get; set; }
        public bool IsHealthy { get; private set; }
        public bool IsSelected { get; set; }
        public int TimesInfected { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        #endregion Properties

        #region Constructors

        public PersonViewModel()
        {
            IsHealthy = true;
            IsSelected = false;
        }

        #endregion Constructors

        #region Methods

        public void IncreaseImmunity(double immunityIncrease)
        {
            Immunity += immunityIncrease;
        }

        public void SetPersonHealthy()
        {
            IsHealthy = true;
            DaysInfected = 0;
        }

        public void SetPersonInfected()
        {
            IsHealthy = false;
            TimesInfected++;
        }

        #endregion Methods
    }
}