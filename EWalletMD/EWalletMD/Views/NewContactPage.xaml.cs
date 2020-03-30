using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thaismartcontract.WalletService;
using Thaismartcontract.WalletService.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace EWalletMD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewContactPage : ContentPage
    {
        private ContactService contactService;
        private ObservableCollection<Contact> itemStates;
        private Contact Contact;
        public NewContactPage(Contact contact = null)
        {

            InitializeComponent();
            if (contact != null)
            {
                this.Contact = contact;
                deleteButton.IsVisible = true;
                Title = "แก้ไขบัญชี";
            }
            
            
            InitializeContact();
        }
        private void InitializeContact()
        {
            
            itemStates = new ObservableCollection<Contact>();
            contactService = new ContactService();
            if(this.Contact != null)
            {
                nameEntry.Text = this.Contact.Name;
                pubkeyEntry.Text = this.Contact.Address;
            }
            
        }
        async void Save_Clicked(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(nameEntry.Text) && !string.IsNullOrEmpty(pubkeyEntry.Text))
            {
                var newContact = contactService.UpsertContact(nameEntry.Text, pubkeyEntry.Text);
                MessagingCenter.Send<NewContactPage>(this, "RefreshMainPage");
                await Navigation.PopAsync(true);
            }
            
        }

        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            if (this.Contact != null)
            {
                string contactName = this.Contact.Name;
                bool answer = await DisplayAlert("ยืนยัน", "ลบบัญชี "+contactName, "ใช่", "ไม่");
                if(answer)
                {                  
                    contactService.DeleteContact(this.Contact.Address);
                    MessagingCenter.Send<NewContactPage>(this, "RefreshMainPage");
                    await Navigation.PopAsync();
                }  
            }
        }

        private async void scanButton_Clicked(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            await Navigation.PushModalAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopModalAsync();

                    ///ตัดคำจากqrcode///
                    //string combinedText = result.Text;
                    //int point1 = combinedText.IndexOf('?'); //ค้นหาเครื่องหมาย
                    //int point2 = combinedText.IndexOf(';');

                    //string publickey = "";
                    //string amount = "";
                    //string contract = "";

                    //if (point1 != -1 && point2 != -1)
                    //{
                    //    publickey = combinedText.Substring(0, point1);
                    //    amount = combinedText.Substring(point1 + 1, point2 - (point1 + 1));
                    //    contract = combinedText.Substring(point2 + 1);
                    //}
                    //else if (point1 != -1 && point2 == -1)
                    //{
                    //    publickey = combinedText.Substring(0, point1);
                    //    amount = combinedText.Substring(point1 + 1);
                    //}
                    //else if (point1 == -1 && point2 != -1)
                    //{
                    //    publickey = combinedText.Substring(0, point2);
                    //    contract = combinedText.Substring(point2 + 1);
                    //}
                    //else
                    //{
                    //    publickey = combinedText.Substring(0);
                    //}
                    /////ตัดคำจากqrcode///

                    pubkeyEntry.Text = result.Text;


                });
            };
        }
    }
}