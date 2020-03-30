using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thaismartcontract.WalletService;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EWalletMD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class ModelBase2 : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string key = null)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(key));
            }
        }

        protected void RaisePropertiesChanged(params string[] keys)
        {
            if (keys != null)
            {
                foreach (string key in keys)
                {
                    var propertyChanged = PropertyChanged;
                    if (propertyChanged != null)
                    {
                        propertyChanged(this, new PropertyChangedEventArgs(key));
                    }
                }
            }
        }
    }

    public class ViewModelBase2 : ModelBase { }

    public class PinAuthViewModel2 : ViewModelBase
    {
        public Func<IList<char>, bool> ValidatorFunc { get; }

        private int _pinLength;
        public int PinLength
        {
            get => _pinLength;
            private set
            {
                _pinLength = value;
                RaisePropertyChanged(nameof(PinLength));
            }
        }

        public ICommand SwitchPinLengthCommand { get; }

        public ICommand ErrorCommand { get; }

        public ICommand SuccessCommand { get; }

        public IList<char> PinPass;
        private KeyService keyService;
        public String Password { get; set; }
        public PinAuthViewModel2()
        {
                ValidatorFunc = (arg) =>
                {
                   
                        StringBuilder builder = new StringBuilder();
                        foreach (char value in arg)
                        {
                            builder.Append(value);
                        }
                        Password = builder.ToString();
                        string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp.txt");
                        string passWordFromFile = File.ReadAllText(fileName);
                        if(string.Equals(Password, passWordFromFile))
                        {
                            
                            InputPassPage.Password = Password;
                            return true;
                        }
                        
                        return false;
                    
                };
            

            PinLength = 6;

            SwitchPinLengthCommand = new Command(() =>
            {
                PinLength = PinLength == 4 ? 6 : 4;
            });

            ErrorCommand = new Command(() =>
            {
                Debug.WriteLine("Entered PIN is wrong");
            });

            SuccessCommand = new Command(() =>
            {
                //Debug.WriteLine("Entered PIN is correct");

            });
        }

    }
    public partial class InputPassPage : ContentPage
    {
        private KeyService keyService;
        private object gotoPage;
        public static string Password { get; set; }
        
        public InputPassPage(object gotoPage = null)
        {
            
            InitializeComponent();
            
            if(gotoPage != null)
            {
                this.gotoPage = gotoPage;
            }
            var viewModel = new PinAuthViewModel2();
            BindingContext = viewModel;
        }
       
        private async void PinView_Success(object sender, EventArgs e)
        {
            string pathKeydb = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "key.db");

            string pathTscWalletdb = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "tsc-wallet.db");

            if(this.gotoPage != null)       //if use Authen from ViewAddress
            {
                await Navigation.PushAsync(gotoPage as Page);              

            }
            else
            {
                if (!File.Exists(pathKeydb))
                {
                    Application.Current.MainPage = new EntryPage();
                }
                else if (!File.Exists(pathTscWalletdb))
                {
                    Application.Current.MainPage = new AddContractPage();
                }
                else
                {
                    //MainPage = new AllCurrencyPage();
                    Application.Current.MainPage = new MainPage();
                }
            }

            
            
        }

        private void PinView_Error(object sender, EventArgs e)
        {
            AuthLabel.Text = "รหัสผ่านไม่ถูกต้อง";

        }
    }
}