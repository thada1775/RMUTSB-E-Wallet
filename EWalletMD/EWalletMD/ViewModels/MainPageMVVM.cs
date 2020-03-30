using EWalletMD.Models;
using EWalletMD.Views;
using GalaSoft.MvvmLight.Command;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Thaismartcontract.API;
using Thaismartcontract.WalletService;
using Thaismartcontract.WalletService.Extension;
using Thaismartcontract.WalletService.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EWalletMD.ViewModels
{
    public class MainPageMVVM : INotifyPropertyChanged
    {


        //private KeyService keyService;
        //private byte[] PubkeyHash;
        //private List<Account> myAccounts = new List<Account>();
        //private List<AccountService> myaccountServices = new List<AccountService>();
        //private bool resetBit = false;
        //private List<WalletContract> mycontracts;
        //private List<WalletService> mywalletServices = new List<WalletService>();
        private ContractService contractService;
        //private BitcoinSecret privateKey;
        private ContactService contactService;
        private List<string> alltransactions = new List<string>();
        private bool IsDone = false;
        public event PropertyChangedEventHandler PropertyChanged;
        BlockExplorerService apiBlockExplorer = new BlockExplorerService();
        private Account _account;

        public MainPageMVVM(Account account)
        {
            RefreshCommand = new RelayCommand(RefreshAction);
            _account = account;
            Transactions = new ObservableCollection<LedgerTran>();

            InitializeEMoney();
        }
        public async void InitializeEMoney()
        {
            //keyService = new KeyService("1234");
            //var savedKey = keyService.GetKey();
            //privateKey = savedKey.SecretKey;
            //PubkeyHash = savedKey.PublicKey.Hash.ToBytes();
            contactService = new ContactService();
            
            string connectionString = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "tsc-wallet.db");
            //contractService = new ContractService(connectionString, new DigibyteAPI(new APIOptions() { BaseURL = "https://digibyteblockexplorer.com" }));   //รุบุที่อยู่รหัสสัญญา
            //contractService = new ContractService(connectionString, new DigibyteAPI(new APIOptions() { BaseURL = await apiBlockExplorer.GetBlockExplorer() }));   //รุบุที่อยู่รหัสสัญญา0
            await GenerateEMoney();
        }


        public ICommand RefreshCommand { get; set; }

        private string _balance;
        public string balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                RaisePropertyChanged();
            }
        }
        private string _Currency;
        public string Currency
        {
            get { return _Currency; }
            set
            {
                _Currency = value;
                RaisePropertyChanged();
            }
        }
        private bool _visibleReference;
        public bool VisibleReference
        {
            get { return _visibleReference; }
            set
            {
                _visibleReference = value;
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

        private async void RefreshAction()
        {
            if (IsDone)
            {
                await GenerateEMoney();
            }
        }
        private ObservableCollection<LedgerTran> _transactions { get; set; }
        public ObservableCollection<LedgerTran> Transactions
        {
            get
            {
                return this._transactions;
            }
            set
            {
                this._transactions = value;
                RaisePropertyChanged();
            }
        }


        private bool isUpdated = false;
        private Ledger shownLedger;

        private LedgerTran _selectingTransaction { get; set; }
        public LedgerTran SelectingTransaction
        {
            get
            {
                return _selectingTransaction;
            }
            set
            {
                if(value != null)
                {
                    this._selectingTransaction = value;
                    GotoBlockexplorer();
                }
                
            }
        }
        private async void GotoBlockexplorer()
        { 
            String uri = $"{await apiBlockExplorer.GetBlockExplorer()}/tx/{SelectingTransaction.TxId}";
            await apiBlockExplorer.GetBlockExplorer();
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }

        public async Task GenerateEMoney()
        {
            ProgressBar = true;
            IsDone = false;
            try
            {
                var isAvailable = await _account.AccountService.GetWalletService().IsNewTransactionAvailable();
                if (isAvailable)
                {
                    await _account.AccountService.GetWalletService().Rescan(false);
                }
            }
            catch(Exception e)
            {
                ProgressBar = false;
                IsDone = true;
                return;
            }
            IsDone = true;
            await ShowEMoney();
            
        }
        public async Task ShowEMoney()
        {
            try
            {
                
                var lastAccount = _account.AccountService.GetAccount(_account.WitnessProgram);
                
                //var allmycontracts = contractService.FindLocalContract();
                //var currentContract = allmycontracts.First(c => c.NameString == _account.TokenName);
                //var mycontracts = _account.AccountService.GetContractService();
                balance = lastAccount.Balance.ToString("N" + _account.AccountService.GetWalletContract().NoOfDecimal.ToString());
                //balance = _account.Balance.ToString("N" + _account.AccountService.GetWalletContract().NoOfDecimal.ToString());
                Currency = _account.TokenUnit;     //show Currency

                var walletService = _account.AccountService.GetWalletService();

                var allLedgers = walletService.GetMyLedgers(limit: 5);
                alltransactions = new List<string>(); //set null
                Transactions = new ObservableCollection<LedgerTran>();
                foreach (var ledger in allLedgers)
                {
                    string transaction = null;
                    if (ledger.TokenString == _account.TokenUnit)
                    {
                        var displayTime = ledger.Time.AddHours(7);
                        string ReferenceCode = "";

                        if (ledger.ReferenceCode != null)
                        {
                            ReferenceCode = $"หมายเหตุ : {ledger.ReferenceCode}";
                            VisibleReference = true;
                        }
                        else
                        {
                            VisibleReference = false;
                        }
                        if (ledger.TokenReceiverHash.SequenceEqual(_account.WitnessProgram))
                        {
                            Transactions.Add(new LedgerTran
                            {
                                Operation = "รับจาก",
                                Address = contactService.GetContact(ledger.TokenSenderHashHex),
                                Amount = $"+{ledger.Amount.ToString("N" + _account.AccountService.GetWalletContract().NoOfDecimal.ToString())}",
                                Time = $"เวลา: {displayTime.ToString("d/MM/yyyy HH:mm", new CultureInfo("th-TH"))}น.",
                                VisibleReference = VisibleReference,
                                ReferenceCode = ReferenceCode,
                                MyTextColor = Color.Green,
                                TxId = ledger.TxId
                            });
                        }
                        else
                        {
                            Transactions.Add(new LedgerTran
                            {
                                Operation = "ส่งไป",                                
                                Address = contactService.GetContact(ledger.TokenReceiverHashHex),
                                Amount = $"-{ledger.Amount.ToString("N" + _account.AccountService.GetWalletContract().NoOfDecimal.ToString())}",
                                Time = $"เวลา: {displayTime.ToString("d/MM/yyyy HH:mm", new CultureInfo("th-TH"))}น.",
                                //Time = $"เวลา: {displayTime.Day}/{displayTime.Month}/{displayTime.Year} {displayTime.ToShortTimeString()}น.",
                                VisibleReference = this.VisibleReference,
                                ReferenceCode = ReferenceCode,
                                MyTextColor = Color.Red,
                                TxId = ledger.TxId
                            });
                        }
                    }
                    alltransactions.Add(transaction);
                }
            }
            catch (Exception)
            {
                ProgressBar = false;
            }
            ProgressBar = false;

            //if (mywalletServices != null && mywalletServices.Count() > 0)
            //{
            //    foreach (var service in mywalletServices)
            //    {
            //        try
            //        {
            //            if (await service.IsNewTransactionAvailable())  //test try
            //                isUpdated = true;
            //        }
            //        catch (Exception e)
            //        {
            //            ProgressBar = false;
            //            Page page = new Page();
            //            await page.DisplayAlert("แจ้งเตือน", "ไม่สามารถเรียกดูรายการธุรรมได้ โปรดตรวจสอบการเชื่อมต่ออินเทอร์เน็ต", "OK");
            //        }
            //    }
            //}
            //else
            //{
            //    isUpdated = true;
            //}

            //if (isUpdated)
            //{
            //    try
            //    {
            //        //mywalletServices.Clear();
            //        //myaccountServices.Clear();
            //        //myAccounts.Clear();
            //        //mycontracts = contractService.FindLocalContract();

            //        //foreach (var con in mycontracts)
            //        //{
            //        //    mywalletServices.Add(contractService.CreateWalletService(con, privateKey));
            //        //}
            //        //foreach (var service in mywalletServices)
            //        //{
            //        //    await service.Rescan(resetBit);
            //        //    myaccountServices.Add(service.GetAccountService());
            //        //}

            //        //foreach (var service in myaccountServices)
            //        //{
            //        //    myAccounts.Add(service.GetAccount(privateKey.GetAddress()));
            //        //}

            //        //if (_account != null)
            //        //{
            //            //var currentContract1 = mycontracts.First(c => c.NameString == _account.TokenName);      //test one account
            //            //var currenaccount = myAccounts.First(c => c.TokenName == _account.TokenName);
            //            //balance = currenaccount.Balance.ToString("N" + currentContract1.NoOfDecimal.ToString());     //test one account
            //            //Currency = currenaccount.TokenUnit;     //show Currency
            //        //}

            //        var allLedgers = _account.AccountService.GetWalletService().GetMyLedgers(limit: 5);
            //        //mywalletServices
            //        //    .SelectMany(ws => ws.GetMyLedgers(limit: 5))
            //        //    .ToList();

            //        var allLedgers = _account.AccountService.GetWalletService().GetMyLedgers(limit: 5);
            //        shownLedger = allLedgers[0];
            //        alltransactions = new List<string>(); //set null
            //        Transactions = new ObservableCollection<LedgerTran>();
            //        foreach (var ledger in allLedgers)
            //        {
            //            if (!ledger.TokenSenderHash.QuickCompare(PubkeyHash) && !ledger.TokenReceiverHash.QuickCompare(PubkeyHash))
            //            {
            //                continue;
            //            }

            //            if (ledger.Status != ProcessStatus.Processed)
            //            {
            //                continue;
            //            }

            //            var currentContract = mycontracts.First(c => c.TokenName.QuickCompare(ledger.TokenName));

            //            string transaction = null;
            //            if (ledger.TokenString == _account.TokenUnit)
            //            {
            //                var displayTime = ledger.Time.AddHours(7);
            //                string ReferenceCode = "" ;

            //                if (ledger.ReferenceCode != null)
            //                {
            //                    ReferenceCode = $"หมายเหตุ : {ledger.ReferenceCode}";
            //                    VisibleReference = true;
            //                }
            //                else
            //                {
            //                    VisibleReference = false;
            //                }
            //                if (ledger.TokenReceiverHash.SequenceEqual(PubkeyHash))
            //                {
            //                    Transactions.Add(new LedgerTran
            //                    {
            //                        Operation = "รับจาก"
            //                        ,

            //                        Address = contactService.GetContact(ledger.TokenSenderHashHex)
            //                        ,
            //                        Amount = $"+{ledger.Amount.ToString("N" + currentContract.NoOfDecimal.ToString())}"
            //                        ,
            //                        Time = $"เวลา: {displayTime.ToString("d/MM/yyyy HH:mm", new CultureInfo("th-TH"))}น."
            //                        ,
            //                        VisibleReference = this.VisibleReference
            //                        ,
            //                        ReferenceCode = ReferenceCode
            //                        ,
            //                        MyTextColor = Color.Green
            //                    });
            //                }
            //                else
            //                {
            //                    Transactions.Add(new LedgerTran
            //                    {
            //                        Operation = "ส่งไป"
            //                        ,
            //                        //Address = ledger.TokenReceiverHashHex
            //                        Address = contactService.GetContact(ledger.TokenReceiverHashHex)
            //                        ,
            //                        Amount = $"-{ledger.Amount.ToString("N" + currentContract.NoOfDecimal.ToString())}"
            //                        ,
            //                        Time = $"เวลา: {displayTime.Day}/{displayTime.Month}/{displayTime.Year} {displayTime.ToShortTimeString()}น."
            //                        ,
            //                        VisibleReference = this.VisibleReference
            //                        ,
            //                        ReferenceCode = ReferenceCode
            //                        ,
            //                        MyTextColor = Color.Red
            //                    });
            //                }
            //            }
            //            else
            //            {

            //            }
            //            alltransactions.Add(transaction);

            //        }

            //        resetBit = false;
            //        IsDone = true;
            //        isUpdated = false;
            //    }
            //    catch (Exception e)
            //    {

            //        if (false)
            //        {
            //            resetBit = true;
            //        }
            //        else
            //        {
            //            Thread.Sleep(3000);
            //        }
            //        //InitializeEMoney();
            //        //Application.Current.MainPage = new WaringEntryPage();
            //        ProgressBar = false;
            //    }

            //}
            //ProgressBar = false;
        }


        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
