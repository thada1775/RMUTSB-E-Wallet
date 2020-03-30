using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thaismartcontract.API;
using Thaismartcontract.WalletService;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace EWalletMD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class AddContractPage : ContentPage
    {

        private char[] ValidChars = "abcdef0123456789".ToCharArray();
        private ContractService contractService;
        public AddContractPage()
        {
            InitializeComponent();
            
        }
        private void Addcontract_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Check contract
            if (addcontract.Text.Length != 64 || addcontract.Text.Any(c => !ValidChars.Contains(c)))
            {
                buttoncontract.IsEnabled = false;
            }
            else
            {
                buttoncontract.IsEnabled = true;
            }
        }

        private async void AddContract_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(addcontract.Text))
            {
                await DisplayAlert("แจ้งเตือน", "กรุณาใส่รหัสสัญญา", "OK");
            }
            else
            {
                BlockExplorerService apiBlockExplorer = new BlockExplorerService();
                string connectionString = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "tsc-wallet.db");
                contractService = new ContractService(connectionString, new DigibyteAPI(new APIOptions() { BaseURL = await apiBlockExplorer.GetBlockExplorer() }));
                //contractService = new ContractService(@"tsc-wallet.db", new DigibyteAPI(new APIOptions() { BaseURL = "https://digibyteblockexplorer.com" }));
                var result = await contractService.FindContract(addcontract.Text);
                if (result == null)
                {
                    await Navigation.PushModalAsync(new AddContractPage());
                }
                else
                {
                    //await Navigation.PushModalAsync(new MainPage());
                    Application.Current.MainPage = new MainPage();
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
                    addcontract.Text = result.Text;

                });
            };
            //var scan = new ZXingScannerPage();
            //await Navigation.PushAsync(scan);
            //scan.OnScanResult += (result) =>
            //{
            //    Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        await Navigation.PopAsync();
            //        receiver.Text = result.Text;

            //    });
            //};
        }
    }
}