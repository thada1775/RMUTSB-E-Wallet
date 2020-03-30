using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using EWalletMD.Models;
using Thaismartcontract.WalletService;
using Thaismartcontract.API;
using Thaismartcontract.WalletService.Model;

namespace EWalletMD.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();


        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Browse, (NavigationPage)Detail);

            //test


        }


        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Browse:
                        MenuPages.Add(id, new NavigationPage(new AllCurrencyPage()));
                        break; 
                    case (int)MenuItemType.Recover:
                        MenuPages.Add(id, new NavigationPage(new WarningRecoverPage()));
                        break;
                    case (int)MenuItemType.Addcontract:
                        MenuPages.Add(id, new NavigationPage(new AddContractPage()));
                        break;
                    case (int)MenuItemType.MyAddress:
                        MenuPages.Add(id, new NavigationPage(new ViewKeyOptionPage()));
                        break;
                    case (int)MenuItemType.ChangeAddress:
                        MenuPages.Add(id, new NavigationPage(new ChangeAPIPage()));
                        break;
                    case (int)MenuItemType.Contact:
                        MenuPages.Add(id, new NavigationPage(new ContactPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}