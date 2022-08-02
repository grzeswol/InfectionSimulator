using Simulator.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Simulator
{
    public class Simulator
    {
        private readonly IPerson[,] _personArray = new IPerson[0, 0];
        private readonly IRandom _randomGenerator;
        private int RecoveryDays;
        private double ImmunityIncrease;
        public double Infectiousness;

        public Simulator(IPerson[,] personArray, IRandom randomGenerator, double immunity, double immunityIncrease, double infectiousness, int recoveryDays)
        {
            _personArray = personArray;
            _randomGenerator = randomGenerator;
            RecoveryDays = recoveryDays;
            ImmunityIncrease = immunityIncrease;
            Infectiousness = infectiousness;

            SetPeopleImmunity(immunity);
        }

        private void SetPeopleImmunity(double immunity)
        {
            for (int i = 0; i < _personArray.GetLength(0); i++)
                for (int j = 0; j < _personArray.GetLength(1); j++)
                    _personArray[i, j].Immunity = immunity;
        }

        public void UpdatePersonArray()
        {
            for (int i = 0; i < _personArray.GetLength(0); i++)
            {
                for (int j = 0; j < _personArray.GetLength(1); j++)
                {
                    var person = _personArray[i, j];
                    if (person.IsHealthy)
                    {
                        if (_randomGenerator.NextDouble() <= Infectiousness)
                        {
                            var neighbours = GetPersonNeighbours(i, j);
                            var infectedNeighboursCount = neighbours.Count(n => !n.IsHealthy);

                            var personImmunity = person.Immunity;
                            for (int k = 0; k < infectedNeighboursCount; k++)
                            {
                                if (_randomGenerator.NextDouble() > personImmunity)
                                {
                                    person.SetPersonInfected();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (person.DaysInfected >= RecoveryDays)
                        {
                            person.SetPersonHealthy();
                            person.IncreaseImmunity(ImmunityIncrease);
                        }
                        else
                            person.DaysInfected++;
                    }
                }
            }
        }

        public List<IPerson> GetPersonNeighbours(int x, int y)
        {
            var result = new List<IPerson>();
            var xvals = new int[3] { -1, 0, 1 };
            var yvals = new int[3] { -1, 0, 1 };
            var arrayLength = _personArray.GetLength(0) - 1;
            foreach (var xOffset in xvals)
            {
                foreach (var yOffset in yvals)
                {
                    if ((x + xOffset < 0 || y + yOffset < 0)
                        || (xOffset == 0 && yOffset == 0)
                        || (x + xOffset > arrayLength || y + yOffset > arrayLength))
                        continue;

                    result.Add(_personArray[x + xOffset, y + yOffset]);
                }
            }
            return result;
        }

        public void SaveSettings(double immunity, double immunityIncrease, double infectiousness, int recoveryDays)
        {
            ImmunityIncrease = immunityIncrease;
            Infectiousness = infectiousness;
            RecoveryDays = recoveryDays;
            SetPeopleImmunity(immunity);
        }
    }
}