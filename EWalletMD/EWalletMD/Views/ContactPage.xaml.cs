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

namespace EWalletMD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactPage : ContentPage
    {
        private ContactService contactService;
        private ObservableCollection<Contact> itemStates;
        public ContactPage()
        {
            
            InitializeComponent();
            Title = "บัญชีผู้ใช้";
            AccountList.HasUnevenRows = true;
            
            InitializeContact();

            
        }

        private void InitializeContact()
        {
            
            //itemStates = new ObservableCollection<Contact>();
            contactService = new ContactService();
            var contacts = contactService.GetContact();
            List<AccountList> accountList = new List<AccountList>();
            foreach (var contact in contacts)
            {
                //itemStates.Add(contact);
                accountList.Add(new AccountList {
                    Name = contact.Name,
                    Address = contact.Address, 
                    
                    ImageIcon = "user.png",
                    ImageEdit = "gear.png"
                });
            }
            if(accountList.Count != 0)
            {
                indicateLabel.IsVisible = false;
                titleLabel.IsVisible = true;
                AccountList.IsEnabled = true;
                AccountList.IsVisible = true;
                AccountList.ItemsSource = accountList;
            }
            else
            {
                AccountList.IsVisible = false;
                titleLabel.IsVisible = false;
                indicateLabel.IsVisible = true;
                
                
                
            }

            MessagingCenter.Subscribe<NewContactPage>(this, "RefreshMainPage", (sender) =>
            {
                InitializeContact();
            });

        }
    
        async void AddItem_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new NavigationPage(new NewContactPage()));
            await Navigation.PushAsync(new NewContactPage());
        }

        private async void AccountList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectItem = e.SelectedItem as Contact;
            
            AccountList.SelectedItem = null;
            if(selectItem != null)
            {
                await Navigation.PushAsync(new NewContactPage(selectItem));
            }
        }


    }
    class AccountList : Contact
    {
        
        public string ImageIcon { get; set; }
        public string ImageEdit { get; set; }
    }

}