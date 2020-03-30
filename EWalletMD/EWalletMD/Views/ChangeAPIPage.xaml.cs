using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EWalletMD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeAPIPage : ContentPage
    {
        //private string ApiAddress;
        private BlockExplorerService apiBlockExplorer;
        public ChangeAPIPage()
        {
            apiBlockExplorer = new BlockExplorerService();
            InitializeComponent();
            updateUI();
        }
        public void updateUI()
        {
            string pathApiFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "APIBlock.txt");

            if (File.Exists(pathApiFile))
            {
                AddressEntry.Text = File.ReadAllText(pathApiFile);
            }
            else
            {
                AddressEntry.Text = apiBlockExplorer.GetDefaultBlockExplorer();
            }
        }
        public async Task<bool> CheckAddress()
        {
            if (AddressEntry.Text == null)
            {
                return false;
            }
            else
            {
                try
                {
                    string apiAddress  = AddressEntry.Text + "/api/sync";

                    using (var httpclient = new HttpClient())
                    {
                        httpclient.Timeout = TimeSpan.FromSeconds(3);
                        var result = await httpclient.GetStringAsync(apiAddress);
                        
                    }

                    //System.Net.WebClient client = new System.Net.WebClient();
                    //string result = client.DownloadString(apiAddress);

                    return true;
                }
                catch (Exception e)
                {
                    //do something here to make the site unusable, e.g:

                    return false;

                }
            }
        }
        public void SaveAddress()
        {
            string pathApiFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "APIBlock.txt");

            if (File.Exists(pathApiFile))
            {
                File.Delete(pathApiFile);
            }
            File.WriteAllText(pathApiFile, AddressEntry.Text);
            DisplayAlert("แจ้งเตือน", "บันทึก Block Explorer สำเร็จ", "Ok");
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Indicator.IsVisible = true;
            if (await CheckAddress() == false)
            {
                //Indicator.IsVisible = false;
                await DisplayAlert("แจ้งเตือน", "Block Explorer ไม่ถูกต้องหรือไม่สามารถเชื่อมต่อได้", "Ok");
                Indicator.IsVisible = false;
            }
            else
            {
                SaveAddress();
                Indicator.IsVisible = false;
            }
        }
    }
}