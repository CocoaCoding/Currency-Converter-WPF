using Commands;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Currency_Converter
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Alle currency rates, bound to the ComboBox
        /// </summary>
        public List<string> CurrencyRateKeys
        {
            get => currencyRateKeys;
            set
            {
                currencyRateKeys = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Selected currency rate, bound to the ComboBox
        /// </summary>
        public string SelectedCurrencyRate
        {
            get => selectedCurrencyRate;
            set
            {
                selectedCurrencyRate = value;
                this.CurrencyOutput = string.Empty;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Currency Input Value, bound to TextBox
        /// </summary>
        public string CurrencyInput
        {
            get => currencyInput;
            set
            {
                currencyInput = value;
                RaisePropertyChanged();
            }
        }
        
        /// <summary>
        /// Converted Currency Value, bound to Label
        /// </summary>
        public string CurrencyOutput
        {
            get => currencyOutput;
            set
            {
                currencyOutput = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommandSimple ConvertButtonCommand { get; set; }

        private CurrencyConverter currencyConverter;
        private List<string> currencyRateKeys;
        private string selectedCurrencyRate;
        private string currencyInput;
        private string currencyOutput;

        public MainWindowViewModel()
        {
            this.CurrencyInput = "";
            this.CurrencyOutput = "";

            currencyConverter = new CurrencyConverter();
            this.CurrencyRateKeys = currencyConverter.CurrencyRateKeys;
            this.SelectedCurrencyRate = this.CurrencyRateKeys.First();

            this.ConvertButtonCommand = new DelegateCommandSimple(() => this.ConvertCurrency());
        }

        private void ConvertCurrency()
        {
            string rateId = this.SelectedCurrencyRate;
            double currencyValueDouble;

            if (double.TryParse(this.CurrencyInput, NumberStyles.Any, CultureInfo.CurrentCulture, out currencyValueDouble))
            {
                double convertedValue = this.currencyConverter.Convert(rateId, currencyValueDouble);
                this.CurrencyOutput = string.Format(System.Globalization.CultureInfo.CurrentCulture, "{0} {1:N2}", rateId, convertedValue);
            }
            else
            {
                this.CurrencyOutput = "Error";
            }
        }
    }
}
