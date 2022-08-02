using InfectionSimulator.Controls;
using System.Linq;
using Xamarin.Forms;

namespace InfectionSimulator.Behaviors
{
    public class EntryNumericalValidationBehavior : Behavior<NumericalEntry>
    {
        #region Properties

        public static readonly BindableProperty AllowDecimalsProperty = BindableProperty.Create(nameof(AllowDecimalsProperty), typeof(bool), typeof(EntryNumericalValidationBehavior), defaultValue: true);

        public bool AllowDecimals
        {
            get { return (bool)GetValue(AllowDecimalsProperty); }
            set { SetValue(AllowDecimalsProperty, value); }
        }

        #endregion Properties

        #region Methods

        private static bool IsTextValid(char x, bool allowDecimals)
        {
            bool isValid = char.IsDigit(x);
            if (allowDecimals)
                isValid = isValid || x == '.';

            return isValid;
        }

        private static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(args.NewTextValue))
            {
                var entry = ((NumericalEntry)sender);
                bool isValid = args.NewTextValue.ToCharArray().All(x => IsTextValid(x, entry.AllowDecimals));
                entry.Text = isValid ? args.NewTextValue : args.OldTextValue;
            }
        }

        protected override void OnAttachedTo(NumericalEntry entry)
        {
            base.OnAttachedTo(entry);
            entry.TextChanged += OnEntryTextChanged;
            entry.BindingContextChanged += (sender, _) => BindingContext = ((BindableObject)sender).BindingContext;
        }

        protected override void OnDetachingFrom(NumericalEntry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        #endregion Methods
    }
}