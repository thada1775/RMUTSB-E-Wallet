using NBitcoin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thaismartcontract.API;
using Thaismartcontract.WalletService;
using Thaismartcontract.WalletService.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace EWalletMD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceiveMoneyPage : ContentPage
    {
        ZXingBarcodeImageView barcode;

        private KeyService keyService;
        private CryptoKeyPair currentKeyPair;

        private WalletContract currentContract;
        private List<WalletContract> mycontracts;
        private List<WalletService> mywalletServices = new List<WalletService>();
        private ContractService contractService;
        private BitcoinSecret privateKey;
        private BitcoinPubKeyAddress publicKey;
        private WalletService walletService;
        private ContactService contactService;

        private string publicKeyWif;
        private string amount = null;
        //private string contract = null;
        private Account _account;
        public ReceiveMoneyPage(Account account)
        {
            _account = account;
            InitializeComponent();
            InitializeEMoneyAsync();
            InitializeQRCode();

        }
        public void InitializeEMoneyAsync()
        {
            keyService = new KeyService("1234");

            var savedKey = keyService.GetKey();
            privateKey = savedKey.SecretKey;
            publicKeyWif = savedKey.PublicKeyWif;
            //string connectionString = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "tsc-wallet.db");
            //contractService = new ContractService(connectionString, new DigibyteAPI(new APIOptions() { BaseURL = "https://digibyteblockexplorer.com" }));   //รุบุที่อยู่รหัสสัญญา
            //mycontracts = contractService.FindLocalContract();
            //foreach (var contract in mycontracts)
            //{
            //    if (contract.NameString == _account.TokenName)
            //    {
            //        this.contract = contract.ID;

            //    }
            //}
            
            CurrencyLabelTop.Text = _account.TokenUnit;
            CurrencyLabel.Text = _account.TokenUnit; 
        }
        public void InitializeQRCode()
        {
            barcode = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "zxingBarcodeImageView",
            };
            barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
            barcode.BarcodeOptions.Width = 300;
            barcode.BarcodeOptions.Height = 300;
            barcode.BarcodeOptions.Margin = 10;
            
            barcode.BarcodeValue = publicKeyWif;


            //this.Content = barcode;
            stackPrinc.Children.Add(barcode);
            pbkeyEntry.Text = publicKeyWif;
        }

        private async void CoppyButton_Clicked(object sender, EventArgs e)
        {
            
                await Clipboard.SetTextAsync(pbkeyEntry.Text);
                if (Clipboard.HasText)
                {
                    var text = Clipboard.GetTextAsync();
                    string message = "คัดลอกกุญแจสาธารณะสำเร็จ";
                    DependencyService.Get<IMESSAGE>().Shorttime(message);
            }
        }

        

        private void AmountEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.amount = amountEntry.Text;
            GenerateQRCode();
        }
        private void GenerateQRCode()
        {
            string resultQR_code = publicKeyWif;
            if (amount != null)
            {
                resultQR_code += "?" + amount;  
            }
            barcode.BarcodeValue = resultQR_code;
            stackPrinc.Children.Add(barcode);
        }
        //private void GenerateQRCode()
        //{
        //    string resultQR_code = publicKeyWif;
        //    if (amount != null && contract != null)
        //    {
        //        resultQR_code += "?" + amount + ";" + contract;

        //    }
        //    else if (amount != null && contract == null)
        //    {
        //        resultQR_code += "?" + amount;

        //    }
        //    else if (amount == null && contract != null)
        //    {
        //        resultQR_code += ";" + contract;

        //    }
        //    else
        //    {

        //    }

        //    barcode.BarcodeValue = resultQR_code;
        //    stackPrinc.Children.Add(barcode);
        //}
    }
}