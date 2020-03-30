using NBitcoin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thaismartcontract.API;
using Thaismartcontract.WalletService;
using Thaismartcontract.WalletService.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace EWalletMD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendMoneyPage : ContentPage
    {
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
        private AccountService accountService;
        private Account account;
        private Account _account;

        public SendMoneyPage(Account account)
        {
            _account = account;
            InitializeComponent();
            InitializeEMoneyAsync();
        }
        public async  void InitializeEMoneyAsync()
        {
            keyService = new KeyService("1234");

            var savedKey = keyService.GetKey();
            privateKey = savedKey.SecretKey;
            string connectionString = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "tsc-wallet.db");
            BlockExplorerService apiBlockExplorer = new BlockExplorerService();
            contractService = new ContractService(connectionString, new DigibyteAPI(new APIOptions() { BaseURL = await apiBlockExplorer.GetBlockExplorer() }));   //รุบุที่อยู่รหัสสัญญา
            UpdateUI();
        }
        private void UpdateUI()
        {
            mycontracts = contractService.FindLocalContract();
            foreach (var contract in mycontracts)
            {
                if(contract.NameString == _account.TokenName)
                {
                    currentContract = contract;
                }
            }
            FeeLabel.Text = Math.Pow(10, -currentContract.NoOfDecimal).ToString();
            CurrencyLabelTop.Text = _account.TokenUnit;
            //walletService = contractService.CreateWalletService(currentContract, privateKey);
            //accountService = walletService.GetAccountService();
            //account = accountService.GetAccount(privateKey);
            CurrencyLable.Text = _account.TokenUnit;
            LeftBalanceLabel.Text = _account.Balance.ToString("N" + currentContract.NoOfDecimal);
            ValidateInput();
        }

        private async void Sendbutton_Clicked(object sender, EventArgs e)
        {
            string ErrorAdress = "";
            string ErrorReference = "";
            walletService = contractService.CreateWalletService(currentContract, privateKey);    //การเตรียมการใช้งานกระเป๋า
            ValidateInput();
            try
            {
                new BitcoinPubKeyAddress(receiver.Text, privateKey.Network);
            }
            catch(Exception c)
            {
                ErrorAdress = "ที่อยู่ผู้รับไม่ถูกต้อง หรือ ไม่มีการเชื่อมต่ออินเทอร์เน็ต";
            }
            if (reference.Text != null)
            {
                if (reference.Text.Length >= 20)
                {
                    ErrorReference = "หมายเหตุไม่ควรใส่เกิน 20 ตัวอักษร";
                }
            }
            if (!string.IsNullOrEmpty(ErrorAdress) && !string.IsNullOrEmpty(ErrorReference))
            {
                await DisplayAlert("แจ้งเตือน", "1."+ErrorAdress+"\n2."+ErrorReference, "OK");
            }
            else if(!string.IsNullOrEmpty(ErrorAdress) && string.IsNullOrEmpty(ErrorReference))
            {
                //string converted = currentContract.NoOfDecimal.ToString();
                
                await DisplayAlert("แจ้งเตือน", "1." + ErrorAdress, "OK");
            }
            else if (string.IsNullOrEmpty(ErrorAdress) && !string.IsNullOrEmpty(ErrorReference))
            {
                await DisplayAlert("แจ้งเตือน", "1." + ErrorReference, "OK");
            }
            else
            {
                decimal amountresult = Convert.ToDecimal(amountEntry.Text);
                var leftAmount = _account.Balance - decimal.Parse(amountEntry.Text) - decimal.Parse(FeeLabel.Text);
                string currency = _account.TokenUnit;
                string display = "จำนวน\t" + amountresult + "\t" + currency + "\nค่าธรรมเนียม\t" + FeeLabel.Text + "\t" + currency + "\nคงเหลือ\t" + leftAmount + "\t" + currency;
                bool answer = await DisplayAlert("ต้องการส่งเงินใช่หรือไม่", display, "Yes", "No");
                //Debug.WriteLine("Answer: " + answer);
                if (answer == true)
                {
                    try
                    {
                        var ledger = walletService.CreateLedger(OperationCode.Transfer, receiver.Text, amountresult, reference.Text);
                        await walletService.BroadcastLedger(ledger, false);
                    }
                    catch(Exception ex)
                    {
                        string a = ex.ToString();
                    }
                    

                    await Navigation.PopAsync();
                }
                else
                {

                }
            }
            
        }
        private void ValidateInput()
        {
            if (!string.IsNullOrEmpty(amountEntry.Text))
            {
                amountEntry.Text = Math.Round(decimal.Parse(amountEntry.Text), currentContract.NoOfDecimal).ToString();
                var leftAmount = _account.Balance - decimal.Parse(amountEntry.Text) - decimal.Parse(FeeLabel.Text);
                LeftBalanceLabel.Text = leftAmount.ToString("N" + _account.AccountService.GetWalletContract().NoOfDecimal.ToString());
            }
            else
            {
                LeftBalanceLabel.Text = _account.Balance.ToString("N" + _account.AccountService.GetWalletContract().NoOfDecimal.ToString());
            }
            if (currentContract == null)
            {
                sendbutton.BackgroundColor = Color.Gray;
                sendbutton.IsEnabled = false;
                return;
            }
                    
                    
                    
                    if (decimal.Parse(LeftBalanceLabel.Text) < 0)
                    {
                        LeftBalanceLabel.TextColor = Color.Red;
                        sendbutton.BackgroundColor = Color.Gray;
                        sendbutton.IsEnabled = false;
                    }
                    else if (string.IsNullOrEmpty(amountEntry.Text))
                    {
                        LeftBalanceLabel.TextColor = Color.Gray;
                        sendbutton.BackgroundColor = Color.Gray;
                        sendbutton.IsEnabled = false;
                    }
                    else if (string.IsNullOrEmpty(receiver.Text))
                    {
                        LeftBalanceLabel.TextColor = Color.Gray;
                        sendbutton.BackgroundColor = Color.Gray;
                        sendbutton.IsEnabled = false;
                    }
                    else
                    {
                        LeftBalanceLabel.TextColor = Color.Gray;
                        sendbutton.BackgroundColor = Color.CornflowerBlue;
                        sendbutton.IsEnabled = true;
                        if (decimal.Parse(amountEntry.Text).Equals(0m))
                        {
                            LeftBalanceLabel.TextColor = Color.Gray;
                            sendbutton.BackgroundColor = Color.Gray;
                            sendbutton.IsEnabled = false;
                        }
                    }
        }

        private async void QrcodeButton_Clicked(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            await Navigation.PushModalAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopModalAsync();

                    ///ตัดคำจากqrcode///
                    string combinedText = result.Text;
                    int point1 = combinedText.IndexOf('?'); //ค้นหาเครื่องหมาย
                    

                    string publickey = "";
                    string amount = "";
                   
                    if (point1 != -1)
                    {
                        publickey = combinedText.Substring(0, point1);
                        amount = combinedText.Substring(point1 + 1);
                    }
                    else
                    {
                        publickey = combinedText.Substring(0);
                    }
                    ///ตัดคำจากqrcode///

                    receiver.Text = publickey;
                    amountEntry.Text = amount;
                });
            };
        }

        //private async void QrcodeButton_Clicked(object sender, EventArgs e)
        //{
        //    var scan = new ZXingScannerPage();
        //    await Navigation.PushModalAsync(scan);
        //    scan.OnScanResult += (result) =>
        //    {
        //        Device.BeginInvokeOnMainThread(async () =>
        //        {
        //            await Navigation.PopModalAsync();

        //            ///ตัดคำจากqrcode///
        //            string combinedText = result.Text;
        //            int point1 = combinedText.IndexOf('?'); //ค้นหาเครื่องหมาย
        //            int point2 = combinedText.IndexOf(';');

        //            string publickey = "";
        //            string amount = "";
        //            string contract = "";

        //            if (point1 != -1 && point2 != -1)
        //            {
        //                publickey = combinedText.Substring(0, point1);
        //                amount = combinedText.Substring(point1 + 1, point2 - (point1 + 1));
        //                contract = combinedText.Substring(point2 + 1);
        //            }
        //            else if (point1 != -1 && point2 == -1)
        //            {
        //                publickey = combinedText.Substring(0, point1);
        //                amount = combinedText.Substring(point1 + 1);
        //            }
        //            else if (point1 == -1 && point2 != -1)
        //            {
        //                publickey = combinedText.Substring(0, point2);
        //                contract = combinedText.Substring(point2 + 1);
        //            }
        //            else
        //            {
        //                publickey = combinedText.Substring(0);
        //            }
        //            ///ตัดคำจากqrcode///

        //            receiver.Text = publickey;
        //            amountEntry.Text = amount;


        //            //mycontracts = contractService.FindLocalContract();
        //            foreach (var _contract in mycontracts)
        //            {
        //                if (_contract.ID == contract)
        //                {
        //                    CurrencyLabelTop.Text = _contract.TokenString;
        //                    continue;
        //                }
        //            }

        //        });
        //    };
        //    //var scan = new ZXingScannerPage();
        //    //await Navigation.PushAsync(scan);
        //    //scan.OnScanResult += (result) =>
        //    //{
        //    //    Device.BeginInvokeOnMainThread(async () =>
        //    //    {
        //    //        await Navigation.PopAsync();
        //    //        receiver.Text = result.Text;

        //    //    });
        //    //};
        //}

        private void amountEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateInput(); 
        }

        private void receiver_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateInput();
        }
    }
}