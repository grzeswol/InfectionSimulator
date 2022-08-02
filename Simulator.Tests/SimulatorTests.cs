using Simulator.Classes;
using Simulator.Tests.Classes;

namespace Simulator.Tests
{
    internal class SimulatorTests
    {
        private TestPerson[,] persons;
        private Simulator simulator;

        [SetUp]
        public void Setup()
        {
            persons = new TestPerson[10, 10];

            for (int i = 0; i < persons.GetLength(0); i++)
                for (int j = 0; j < persons.GetLength(1); j++)
                    persons[i, j] = new TestPerson();

            simulator = new Simulator(persons, new DefaultRandom(), 2, 0.2, 0.5, 5);
        }

        [TestCase(0, 0)]
        [TestCase(0, 9)]
        [TestCase(9, 0)]
        [TestCase(9, 9)]
        public void CornerCasesShouldReturn3Neighbours(int x, int y)
        {
            var neighbours = simulator.GetPersonNeighbours(x, y);
            Assert.IsTrue(neighbours.Count == 3);
        }

        [TestCase(0, 4)]
        [TestCase(4, 0)]
        [TestCase(9, 4)]
        [TestCase(4, 9)]
        public void EdgeCasesShouldReturn5Neighbours(int x, int y)
        {
            var neighbours = simulator.GetPersonNeighbours(x, y);
            Assert.IsTrue(neighbours.Count == 5);
        }

        [TestCase(5, 5)]
        public void MiddleCasesShouldReturn8Neighbours(int x, int y)
        {
            var neighbours = simulator.GetPersonNeighbours(x, y);
            Assert.IsTrue(neighbours.Count == 8);
        }
    }
}