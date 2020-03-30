using Plugin.Clipboard;
using Plugin.LocalNotifications;
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
    public partial class MyAddress : ContentPage
    {
        
        private string PublicKey;
        public MyAddress()
        {
            InitializeComponent();
            InitializeEMoney();
            UpdateUI();
        }
        public  void InitializeEMoney()
        {
            KeyService keyService = new KeyService("1234");
            var saveKey = keyService.GetKey();
            
            PublicKey = saveKey.PublicKeyWif;
        }
        public void UpdateUI()
        {
           
            PubKeyLabel.Text = PublicKey;
            QrcodeGenerate.BarcodeValue = PublicKey;
        }

        private void coppyButton_Clicked(object sender, EventArgs e)
        {
            CrossClipboard.Current.SetText(PublicKey);
            string message = "คัดลอกกุญแจสาธารณะสำเร็จ";
            DependencyService.Get<IMESSAGE>().Shorttime(message);
            
        }
    }
}