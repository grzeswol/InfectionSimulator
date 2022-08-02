using InfectionSimulator.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InfectionSimulator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BoardPage : ContentPage
    {
        private const int CELL_SPACING = 2;
        private const int SIZE = 100;
        private readonly BoardViewModel _viewModel;
        private SKRect[,] _rectList;
        private int _cellSize = 1;
        private bool IsCanvasInitialized = false;

        public BoardPage()
        {
            InitializeComponent();
            _viewModel = new BoardViewModel(SIZE);
            BindingContext = _viewModel;
            SetTimer();
            MessagingCenter.Subscribe<BoardViewModel>(this, Const.BOARD_RESET, (model) =>
            {
                SetCoordinates();
                Canvas.InvalidateSurface();
            });
            MessagingCenter.Subscribe<BoardViewModel>(this, Const.GRID_TAPPED, (model) =>
            {
                Canvas.InvalidateSurface();
            });
            SetRectList();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<BoardViewModel>(this, Const.BOARD_RESET);
            MessagingCenter.Unsubscribe<BoardViewModel>(this, Const.GRID_TAPPED);
        }

        private void SetTimer()
        {
            App.AppTimer.Elapsed += AppTimer_Elapsed;
        }

        private void AppTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _viewModel.Tick();
            Canvas.InvalidateSurface();
        }

        private void SetCoordinates()
        {
            var info = Canvas.CanvasSize;
            _cellSize = (int)Math.Min(info.Width / SIZE, info.Height / SIZE);

            var xMargin = (int)((info.Width - SIZE * _cellSize) / 2);
            var yMargin = (int)((info.Height - SIZE * _cellSize) / 2);

            for (int x = 0; x < SIZE; x++)
                for (int y = 0; y < SIZE; y++)
                {
                    var xCoordinate = x * _cellSize + xMargin + CELL_SPACING / 2;
                    var yCoordinate = y * _cellSize + yMargin + CELL_SPACING / 2;

                    _viewModel.SetPersonCoordinates(x, y, xCoordinate, yCoordinate);
                }
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            if (!IsCanvasInitialized)
            {
                SetCoordinates();
                IsCanvasInitialized = true;
            }

            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            for (int x = 0; x < SIZE; x++)
                for (int y = 0; y < SIZE; y++)
                {
                    var person = _viewModel.GetPerson(x, y);
                    var rect = new SKRect(person.X, person.Y, person.X + _cellSize - CELL_SPACING, person.Y + _cellSize - CELL_SPACING);
                    _rectList[x, y] = rect;

                    if (person.IsSelected)
                        canvas.DrawRect(rect, GetPaint(Color.Yellow));
                    else if (person.IsHealthy)
                        canvas.DrawRect(rect, GetPaint(Color.Green));
                    else
                        canvas.DrawRect(rect, GetPaint(Color.Red));
                }
        }

        private SKPaint GetPaint(Color color)
        {
            return new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = color.ToSKColor(),
            };
        }

        private void SetRectList()
        {
            _rectList = new SKRect[SIZE, SIZE];
        }

        private void Canvas_Touch(object sender, SKTouchEventArgs e)
        {
            e.Handled = true;
            SelectPerson(e.Location);
        }

        private void SelectPerson(SKPoint location)
        {
            _viewModel.StopTimer();
            for (int x = 0; x < SIZE; x++)
                for (int y = 0; y < SIZE; y++)
                {
                    if (_rectList[x, y].Contains(location))
                    {
                        _viewModel.SelectPerson(x, y);
                        Canvas.InvalidateSurface();
                    }
                }
        }
    }
}