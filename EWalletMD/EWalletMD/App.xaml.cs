using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EWalletMD.Services;
using EWalletMD.Views;
using Thaismartcontract.WalletService;
using Thaismartcontract.API;
using System.IO;
using Xamarin.Essentials;

namespace EWalletMD
{
    public partial class App : Application
    {
        public static string InsightAPI = "https://insight.thaismartcontract.com/";
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            //keyService = new KeyService(Password);
            //var currentKey =  keyService.GetKey();


            //string pathKeydb = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "key.db");
            
            //string pathTscWalletdb = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "tsc-wallet.db");
            
            
            //if (!File.Exists(pathKeydb))
            //{
            //    MainPage = new EntryPage();
            //}
            //else if (!File.Exists(pathTscWalletdb))
            //{
            //    MainPage = new AddContractPage();           
            //}
            //else
            //{
            //    //MainPage = new AllCurrencyPage();
            //    MainPage = new MainPage();
            //    //MainPage = new AuthenPinView();
            //}


        }
        //static LiteDBHelper db;
        //public static LiteDBHelper LiteDB
        //{
        //    get
        //    {
        //        if (db == null)
        //        {
        //            db = new LiteDBHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XamarinLiteDB.db"));
        //       }
        //       return db;
        //  }
        //}

        protected override void OnStart()
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp.txt");
            // Handle when your app starts
            if (!File.Exists(fileName))
            {
                MainPage = new AuthenPinView();
            }
            else
            {
                //MainPage = new InputPassPage();
                //MainPage = new NavigationPage(new InputPassPage());
                //Page page = new EntryPage();
                //page.Navigation.PushModalAsync(new InputPassPage());
                MainPage = new InputPassPage();
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            NavigationPage PinPass = new NavigationPage(new InputPassPage());

            
        }

    }
}
