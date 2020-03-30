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

    
    public partial class GenKeyPage : ContentPage
    {
        private  KeyService keyService;
        private  CryptoKeyPair currentKeyPair;
        //private readonly string currentPassword = "0825576211";

        public GenKeyPage()
        {
            InitializeComponent();
            InitializeKeyManager();
        }
        public void InitializeKeyManager()
        {
            keyService = new KeyService("1234");

            //currentKeyPair = keyService.GetKey();

            currentKeyPair = keyService.GenerateKey();
            
            string[] keyBySprit = currentKeyPair.Seed.Split(" ".ToCharArray());

            key1.Text = keyBySprit[0];
            key2.Text = keyBySprit[1];
            key3.Text = keyBySprit[2];
            key4.Text = keyBySprit[3];
            key5.Text = keyBySprit[4];
            key6.Text = keyBySprit[5];
            key7.Text = keyBySprit[6];
            key8.Text = keyBySprit[7];
            key9.Text = keyBySprit[8];
            key10.Text = keyBySprit[9];
            key11.Text = keyBySprit[10];
            key12.Text = keyBySprit[11];
            key13.Text = keyBySprit[12];
            key14.Text = keyBySprit[13];
            key15.Text = keyBySprit[14];
            
        }

        private void Savekey_Clicked(object sender, EventArgs e)
        {
            keyService.SaveKey(currentKeyPair);

            if (!keyService.HasSetupKey())
            {
                Navigation.PushModalAsync(new EntryPage());
            }
            else
            {
                Navigation.PushModalAsync(new AddContractPage());
            }
        }
    }
}