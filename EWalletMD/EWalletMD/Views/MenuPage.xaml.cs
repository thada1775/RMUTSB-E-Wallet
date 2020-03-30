using EWalletMD.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EWalletMD.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();
            
            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Browse, Title="หน้าหลัก",Image = "townMenu.png"},
                new HomeMenuItem {Id = MenuItemType.Recover, Title="กู้คืนกระเป๋า" ,Image = "restoreMenu.png"},
                new HomeMenuItem {Id = MenuItemType.Addcontract, Title="เพิ่มรหัสสัญญา" ,Image = "addMenu.png"},
                new HomeMenuItem {Id = MenuItemType.MyAddress, Title="กระเป๋าเงินของฉัน" ,Image = "userMenu.png"},
                new HomeMenuItem {Id = MenuItemType.ChangeAddress, Title="ระบุ Block Explorer" ,Image = "pinMenu.png"},
                new HomeMenuItem {Id = MenuItemType.Contact, Title="บัญชีผู้ใช้" ,Image = "profileMenu.png"},
            };

            ListViewMenu.ItemsSource = menuItems;
            //ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.SelectedItem = null;
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };

            ListViewMenu.ItemTapped += (object sender, ItemTappedEventArgs e) =>
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
    }
}