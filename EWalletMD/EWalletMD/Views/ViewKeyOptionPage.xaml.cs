using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EWalletMD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewKeyOptionPage : ContentPage
    {
        public ViewKeyOptionPage()
        {
            InitializeComponent();
            Title = "เรียกดูบัญชีของฉัน";

            //MainListView.ItemsSource = new List<string>
            //{
            //     "แนะนำบัญชีของฉัน", "ข้อมูลบัญชีของฉัน"
            //};
            MainListView.ItemsSource = new List<AddressIconView>
            {
                 new AddressIconView{Name = "แนะนำบัญชีของฉัน",Image = "gps.png" },
                 new AddressIconView{Name = "ข้อมูลบัญชีของฉัน",Image = "notebook.png" }
            };

            MainListView.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                // don't do anything if we just de-selected the row.
                if (e.Item == null) return;

                // Optionally pause a bit to allow the preselect hint.
                Task.Delay(500);

                // Deselect the item.
                if (sender is ListView lv) lv.SelectedItem = null;

                // Do something with the selection.
               
            };
        }

        private async void MainListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var IndexMenu = e.SelectedItemIndex;

            switch (IndexMenu)
            {
                case 0:
                    await Navigation.PushAsync(new MyAddress());
                    break;
                case 1:
                    await DisplayAlert("คำเตือน", "คีย์ข้อความและกุญแจสาธารณะไม่ควรเปิดเผยสู่สาธารณะ โปรดตรวจสอบข้อมูลอย่างระมัดระวัง","รับทราบ" );
                    
                    await Navigation.PushAsync(new InputPassPage(new SecretKeyDataPage()));
                    break;
                default:
                    break;
            }

        }
    }
}
public class AddressIconView
{
    public string Name { get; set; }
    public string Image { get; set; }
}