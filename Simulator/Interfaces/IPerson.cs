namespace Simulator.Interfaces
{
    public interface IPerson
    {
        bool IsHealthy { get; }
        double Immunity { get; set; }

        void SetPersonInfected();

        void SetPersonHealthy();

        void IncreaseImmunity(double immunityIncrease);

        int DaysInfected { get; set; }
        int TimesInfected { get; set; }
    }
}