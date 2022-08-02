using InfectionSimulator.Enums;
using Simulator.Classes;
using Simulator.Interfaces;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace InfectionSimulator.ViewModels
{
    internal class BoardViewModel : BaseViewModel
    {
        private Simulator.Simulator _simulator { get; set; }
        private PersonViewModel[,] _persons { get; set; }
        private int SimulationDays { get; set; }

        public PersonViewModel SelectedPerson { get; set; }
        public ICommand PlayPauseCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand SettingsSaveCommand { get; set; }
        public ICommand GridTapCommand { get; set; }

        public int RecoveryDays { get; set; }

        public bool IsSettingsOpen { get; set; }
        public double Infectiousness { get; set; }
        public int SimulationDaysProp { get; set; }
        public double Immunity { get; set; }
        public double ImmunityIncrease { get; set; }

        private int SimulationDay { get; set; } = 0;
        public string SimulationDayLabel => "Day: " + SimulationDay;

        private SimulationStatusEnum SimulationStatusEnum { get; set; }
        public string SimulationStatusEnumLabel => "Status: " + SimulationStatusEnum.ToString();

        private void GridTap()
        {
            IsSettingsOpen = false;

            if (SelectedPerson != null)
                SelectedPerson.IsSelected = false;

            SelectedPerson = null;
            MessagingCenter.Send(this, Const.BOARD_RESET);
        }

        private bool CanExecuteCommand()
        {
            return !IsSettingsOpen;
        }

        private Action CommandToExecute(Action action)
        {
            return new Action(() =>
            {
                action();
                RefreshCanExecute();
            });
        }

        private void RefreshCanExecute()
        {
            (PlayPauseCommand as Command).RaiseCanExecuteChanged();
            (ResetCommand as Command).RaiseCanExecuteChanged();
        }

        private void OpenSettings()
        {
            if (App.AppTimer.Enabled)
                StopTimer();

            IsSettingsOpen = !IsSettingsOpen;
        }

        private void ResetBoard(int size)
        {
            App.AppTimer.Stop();
            Immunity = 0.6;
            ImmunityIncrease = 0.1;
            SimulationDaysProp = 20;
            Infectiousness = 0.5;
            RecoveryDays = 5;

            SetSmulationDays(SimulationDaysProp);

            _persons = new PersonViewModel[size, size];

            for (int i = 0; i < _persons.GetLength(0); i++)
                for (int j = 0; j < _persons.GetLength(1); j++)
                    _persons[i, j] = new PersonViewModel();

            SetRandomInfectedPersons(_persons);

            _simulator = new Simulator.Simulator(_persons, new DefaultRandom(), Immunity, ImmunityIncrease, Infectiousness, RecoveryDays);

            SimulationDay = 0;
            SimulationStatusEnum = SimulationStatusEnum.Stopped;
            SelectedPerson = null;
            MessagingCenter.Send(this, Const.BOARD_RESET);
        }

        private void SetRandomInfectedPersons(IPerson[,] personList)
        {
            var randomGenerator = new Random();
            var size = personList.GetLength(0);
            for (int i = 0; i < 10; i++)
            {
                var x = randomGenerator.Next(0, size);
                var y = randomGenerator.Next(0, size);
                var person = personList[x, y];
                if (person != null)
                    person.SetPersonInfected();
            }
        }

        private void SetSmulationDays(int simulationDays)
        {
            if (simulationDays > SimulationDay)
                SimulationDays = simulationDays;
        }

        private void StartStopTimer()
        {
            if (App.AppTimer.Enabled)
            {
                StopTimer();
            }
            else
            {
                App.AppTimer.Start();
                SimulationStatusEnum = SimulationStatusEnum.Running;
            }
        }

        private void SaveSettings()
        {
            _simulator.SaveSettings(Immunity, ImmunityIncrease, Infectiousness, RecoveryDays);
            SetSmulationDays(SimulationDaysProp);
            IsSettingsOpen = false;
        }

        internal void SelectPerson(int x, int y)
        {
            if (SelectedPerson != null)
                SelectedPerson.IsSelected = false;

            SelectedPerson = _persons[x, y] ?? new PersonViewModel();
            SelectedPerson.IsSelected = true;
        }

        internal void Tick()
        {
            if (SimulationDay >= SimulationDays)
            {
                App.AppTimer.Stop();
                SimulationStatusEnum = SimulationStatusEnum.Finished;
                RefreshCanExecute();
                return;
            }
            _simulator.UpdatePersonArray();
            SimulationDay++;
        }

        internal PersonViewModel GetPerson(int x, int y)
        {
            return _persons[x, y];
        }

        internal void SetPersonCoordinates(int x, int y, int xCoordinate, int yCoordinate)
        {
            var person = _persons[x, y];
            if (person != null)
            {
                person.X = xCoordinate;
                person.Y = yCoordinate;
            }
        }

        internal void StopTimer()
        {
            App.AppTimer.Stop();
            SimulationStatusEnum = SimulationStatusEnum.Paused;
        }

        public BoardViewModel(int size)
        {
            IsSettingsOpen = true;
            SettingsCommand = new Command(CommandToExecute(() => OpenSettings()));
            SettingsSaveCommand = new Command(CommandToExecute(() => SaveSettings()));
            PlayPauseCommand = new Command(CommandToExecute(() => StartStopTimer()), CanExecuteCommand);
            ResetCommand = new Command(CommandToExecute(() => ResetBoard(size)), CanExecuteCommand);
            GridTapCommand = new Command(CommandToExecute(() => GridTap()));

            ResetBoard(size);
        }
    }
}