using EWalletMD.Views;
using GalaSoft.MvvmLight.Command;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Thaismartcontract.API;
using Thaismartcontract.WalletService;
using Thaismartcontract.WalletService.Extension;
using Thaismartcontract.WalletService.Model;
using Xamarin.Forms;

namespace EWalletMD.ViewModels
{
    public class AllCurrencyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private KeyService keyService;
        //private CryptoKeyPair currentKeyPair;
        private byte[] PubkeyHash;
        private List<Account> myAccounts = new List<Account>();
        private List<AccountService> myaccountServices = new List<AccountService>();
        private bool resetBit = false;
        private List<WalletContract> mycontracts;
        private List<WalletService> mywalletServices = new List<WalletService>();
        private ContractService contractService;
        private BitcoinSecret privateKey;
        //private BitcoinPubKeyAddress publicKey;
        //private WalletService walletService;
        private ContactService contactService;
        private List<string> Currency = new List<string>();
        private bool IsDone = false;
        public bool IsTimerRun = false;
        public INavigation Navigation { get; set; }
        public bool IsInitialized { get; private set; } = false;
        public AllCurrencyViewModel(INavigation navigation)
        {
            //SelectCurrency = new Account(); //test
            Currency_List = new ObservableCollection<Account>();
            RefreshCommand = new RelayCommand(RefreshAction);
            this.Navigation = navigation;
            //Task.Run(() => InitializeEMoney().Wait());
            IsEnabledRefreshButton = true;
        }

        //public void StopTimer()
        //{
        //    allowTimer = false;
        //}

        public bool triggerStartTimer;
        public void StartTimmer()
        {
            IsTimerRun = true;
            Device.StartTimer(TimeSpan.FromSeconds(20), () =>
            {
                // If you want to update UI, make sure its on the on the main thread.
                // Otherwise, you can remove the BeginInvokeOnMainThread
                if (triggerStartTimer)
                {
                    var task = Task.Factory.StartNew(async () => await GenerateEMoney());
                    task.Wait();
                }

                // do something with time...
                //return allowTimer;
                IsTimerRun = false;
                return true;
            });
        }

        public async Task InitializeEMoney()
        {
            IsInitialized = true;
            ProgressBar = true;
            keyService = new KeyService("1234");
            var savedKey = keyService.GetKey();
            privateKey = savedKey.SecretKey;
            PubkeyHash = savedKey.PublicKey.Hash.ToBytes();
            contactService = new ContactService();
            BlockExplorerService apiBlockExplorer = new BlockExplorerService();
            string connectionString = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "tsc-wallet.db");
            //contractService = new ContractService(connectionString, new DigibyteAPI(new APIOptions() { BaseURL = "https://digibyteblockexplorer.com" }));   //รุบุที่อยู่รหัสสัญญา
            contractService = new ContractService(connectionString, new DigibyteAPI(new APIOptions() { BaseURL = await apiBlockExplorer.GetBlockExplorer() }));   //รุบุที่อยู่รหัสสัญญา

            //await GenerateEMoney();
            var task = Task.Factory.StartNew(async () => await GenerateEMoney());
            task.Wait();
            StartTimmer();
        }

        public ICommand RefreshCommand { get; set; }


        private ObservableCollection<Account> _currency_List;
        public ObservableCollection<Account> Currency_List
        {
            get
            {
                return this._currency_List;
            }
            set
            {
                this._currency_List = value;
                RaisePropertyChanged();
            }
        }
        private bool _progressBar { get; set; }
        public bool ProgressBar
        {
            get { return _progressBar; }
            set
            {
                _progressBar = value;
                RaisePropertyChanged();
            }
        }

        private bool _isEnabledRefreshButton { get; set; }
        public bool IsEnabledRefreshButton
        {
            get { return _isEnabledRefreshButton;  }
            set
            {
                _isEnabledRefreshButton = value;
                RaisePropertyChanged();
            }
        }

        private Account _selectCurrency { get; set; }
        public Account SelectCurrency
        {
            get { return _selectCurrency; }
            set
            {
                if (value != null)
                {
                    _selectCurrency = value;

                    HandleSelectedItem();
                }

            }
        }
        private async void HandleSelectedItem()     //after select listview
        {
            await Navigation.PushAsync(new ItemsPage(SelectCurrency));
        }

        private void RefreshAction()
        {
            if (IsDone)
            {
                //await GenerateEMoney();
                var task = Task.Factory.StartNew(async () => await GenerateEMoney());
                task.Wait();
            }
        }

        private bool isUpdated = false;


        public async Task GenerateEMoney()
        {
            IsEnabledRefreshButton = false;
            IsDone = false;
            triggerStartTimer = false;      //test stoptimer
            ProgressBar = true;
            if (mywalletServices != null && mywalletServices.Count() > 0)
            {
                foreach (var service in mywalletServices)
                {
                    try
                    {
                        if (await service.IsNewTransactionAvailable()) isUpdated = true;
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }
            }
            else
            {
                isUpdated = true;
            }


            mywalletServices.Clear();
            myaccountServices.Clear();
            myAccounts.Clear();
            mycontracts = contractService.FindLocalContract();

            foreach (var con in mycontracts)
            {
                mywalletServices.Add(contractService.CreateWalletService(con, privateKey));
            }
            foreach (var service in mywalletServices)
            {
                if (isUpdated)       //Have new transaction --> Rescan
                {
                    await service.Rescan(resetBit);
                }
                myaccountServices.Add(service.GetAccountService());
            }

            foreach (var service in myaccountServices)
            {
                myAccounts.Add(service.GetAccount(privateKey.GetAddress()));
            }
            try
            {
                Currency_List.Clear();
                foreach (var account in myAccounts)
                {
                    //var currentContract = mycontracts.First(c => c.NameString == account.TokenName);
                    //string currency = account.TokenUnit + "\t\t\t\t" + account.Balance.ToString("N" + currentContract.NoOfDecimal.ToString());
                    //Currency.Add(currency);
                    Currency_List.Add(account);
                }
            }
            catch (Exception e)
            {

                Thread.Sleep(3000);
                ProgressBar = false;
                IsDone = true;
                IsEnabledRefreshButton = false;
            }

            resetBit = false;
            isUpdated = false;
            ProgressBar = false;
            triggerStartTimer = true;
            IsDone = true;
            IsEnabledRefreshButton = true;
        }


        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
