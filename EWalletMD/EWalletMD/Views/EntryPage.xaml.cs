using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EWalletMD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryPage : ContentPage
    {
        public string Password { get; set; }
        public EntryPage()
        {
            InitializeComponent();
            
        }
        async void go_WaringEntryPage(object sender, EventArgs e)
        {
            genkey.IsEnabled = false;
            await Navigation.PushModalAsync(new WaringEntryPage());
            genkey.IsEnabled = true;
        }
        async void go_RecoverPage(object sender, EventArgs e)
        {
            recovery.IsEnabled = false;
            await Navigation.PushModalAsync(new RecoverPage());
            recovery.IsEnabled = true;
        }
    }
}