using Xamarin.Forms;

namespace InfectionSimulator.Controls
{
    public partial class NumericalEntry : Entry
    {
        public static readonly BindableProperty AllowDecimalsProperty = BindableProperty.Create(nameof(AllowDecimalsProperty), typeof(bool), typeof(NumericalEntry), defaultValue: true);

        public bool AllowDecimals
        {
            get { return (bool)GetValue(AllowDecimalsProperty); }
            set { SetValue(AllowDecimalsProperty, value); }
        }

        public NumericalEntry()
        {
            InitializeComponent();
        }
    }
}