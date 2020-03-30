using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thaismartcontract.API;
using Thaismartcontract.WalletService;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EWalletMD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestViewData : ContentPage
    {
        private KeyService keyService;
        private  CryptoKeyPair currentKeyPair;
        public TestViewData()
        {
            InitializeComponent();
            Viewdata();
        }
        void Viewdata()
        {

            keyService = new KeyService("1234");
            currentKeyPair = keyService.GetKey();

            data1.Text = currentKeyPair.PublicKeyWif;
        }
    }
}