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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EWalletMD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class ModelBase : INotifyPropertyChanged
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

    public class ViewModelBase : ModelBase { }

    public class PinAuthViewModel : ViewModelBase
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

        public static IList<char> PinPass;
        public PinAuthViewModel()
        {
            if (PinPass == null)
            {
                PinPass = new List<char>();
                ValidatorFunc = (arg) =>
                {
                    for(int i =0; i< arg.Count; i++)
                    {
                        PinPass.Add(arg[i]);
                    }
                    return true;
                };
            }
            else
            {
                ValidatorFunc = (arg) =>
                {
                    var a = PinPass.SequenceEqual(arg);
                    if(a) 
                    {
                        
                        AuthenPinView.PinPass = PinPass;
                        return true;  
                    }
                    return false;
                };
            }
               
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

    public partial class AuthenPinView : ContentPage
    {
        public static IList<char> PinPass { get; set; }
        public AuthenPinView()
        {
            InitializeComponent();
            var viewModel = new PinAuthViewModel();
            BindingContext = viewModel;
        }
        public void AuthenPinViewAgian()
        {
            var viewModel = new PinAuthViewModel();
            BindingContext = viewModel;
        }
        private void PinView_Success(object sender, EventArgs e)
        {
            if (PinPass != null)        //if First password inputting is true
            {
                StringBuilder builder = new StringBuilder();
                foreach (char value in PinPass)
                {
                    builder.Append(value);
                }
                string result = builder.ToString();
                //DisplayAlert("", result, "OK");
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp.txt");
                File.WriteAllText(fileName, result);
                
                Application.Current.MainPage = new EntryPage();
            }
            else
            {
                //this.DisplayAlert("", "success", "OK");
                AuthLabel.Text = "ยืนยันรหัสผ่านอีกครั้ง";
                AuthenPinViewAgian();
            }   
        }

        private void PinView_Error(object sender, EventArgs e)
        {
            AuthLabel.Text = "ยืนยันรหัสผ่านไม่ถูกต้อง โปรดระบุอีกครั้ง";
            
        }
    }
}