using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thaismartcontract.WalletService;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EWalletMD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SecretKeyDataPage : ContentPage
    { 
        public SecretKeyDataPage()
        {
            InitializeComponent();

            

            InitializeKey();
        }
        private void InitializeKey()
        {
            KeyService keyService = new KeyService("1234");
            var saveKey = keyService.GetKey();
            seedLabel.Text = saveKey.Seed;
            privateKeyLabel.Text = saveKey.SecretKeyWif;

            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            int BackCount = 2;
            for (var counter = 1; counter < BackCount; counter++)
            {
                if (Navigation.NavigationStack.Count != 0)      //for delete page from stack
                {
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                }
            }

        }
    }
}