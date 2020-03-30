using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using EWalletMD.Models;
using EWalletMD.Views;
using EWalletMD.ViewModels;
using Thaismartcontract.WalletService;
using NBitcoin;
using Thaismartcontract.WalletService.Model;
using Thaismartcontract.API;
using System.IO;
using Thaismartcontract.WalletService.Extension;
using System.Diagnostics;

namespace EWalletMD.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        private KeyService keyService;
        private CryptoKeyPair currentKeyPair;

        private List<Account> myAccounts = new List<Account>();
        private List<AccountService> myaccountServices = new List<AccountService>();
        private bool resetBit = false;
        private List<WalletContract> mycontracts;
        private List<WalletService> mywalletServices = new List<WalletService>();
        private ContractService contractService;
        private BitcoinSecret privateKey;
        private BitcoinPubKeyAddress publicKey;
        private WalletService walletService;
        private ContactService contactService;
        private List<string> transactions = new List<string>();
        private Account _account { get; set; }

        
        public ItemsPage(Account account)
        {
            _account = account;
            BindingContext = new MainPageMVVM(_account);
            InitializeComponent();
            Title = "ประวัติธุรกรรม";
            lsttran.HasUnevenRows = true;

            lsttran.ItemTapped += (object sender, ItemTappedEventArgs e) =>
            {
                // don't do anything if we just de-selected the row.
                if (e.Item == null) return;

                // Optionally pause a bit to allow the preselect hint.
                Task.Delay(500);

                // Deselect the item.
                if (sender is ListView lv) lv.SelectedItem = null;

                // Do something with the selection.

            };
        }
        public void AddItem_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void Sendmoney_Clicked(object sender, EventArgs e)
        {
            
            //await Navigation.PushModalAsync(new SendMoneyPage());
            //await Navigation.PushModalAsync(new NavigationPage(new SendMoneyPage())); 
            //Application.Current.MainPage = new NavigationPage(new SendMoneyPage());
            await Navigation.PushAsync(new SendMoneyPage(_account));
            
        }

        private async void Receipmoney_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushModalAsync(new ReceiveMoneyPage());
            await Navigation.PushAsync(new ReceiveMoneyPage(_account));
        }
    }
}