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
    public partial class RecoverPage : ContentPage
    {
        private KeyService keyService;
        private CryptoKeyPair currentKeyPair;
        public RecoverPage()
        {
            InitializeComponent();
            InitializeKeyManager();
        }
        public void InitializeKeyManager()
        {
            keyService = new KeyService("1234");
        }

        private void Savekey_Clicked(object sender, EventArgs e)
        {
            string keySeed = key1.Text + " " + key2.Text + " " + key3.Text + " " + key4.Text + " " + key5.Text + " " +
                key6.Text + " " + key7.Text + " " + key8.Text + " " + key9.Text + " " + key10.Text + " " + key11.Text + " " +
                key12.Text + " " + key13.Text + " " + key14.Text + " " + key15.Text;

            var result = keyService.ParseSeed(keySeed);
            if (result != null)
            {
                currentKeyPair = result;
                keyService.SaveKey(currentKeyPair);
                DisplayAlert("แจ้งเตือน", "บันทึกคีย์ข้อความเรียบร้อย", "OK");
                Navigation.PushModalAsync(new AddContractPage());
            }
            else
            {
                DisplayAlert("แจ้งเตือน","คีย์ข้อความไม่ถูกต้อง", "OK");
            }
        }
    }
}