using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EWalletMD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WarningRecoverPage : ContentPage
    {
        public WarningRecoverPage()
        {
            InitializeComponent();
        }



        private async void gotoRecover_Clicked(object sender, EventArgs e)
        {
            string display = "เมื่อดำเนินการต่อ กระเป๋าเงินเดิมจะถูกลบ \nดำเนินการต่อหรือไม่";
            bool answer = await DisplayAlert("แจ้งเตือน", display, "Yes", "No");
            Debug.WriteLine("Answer: " + answer);
            if (answer == true)
            {
                string pathKeydb = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "key.db");
                string pathTscWallet = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "tsc-wallet.db");
                if (File.Exists(pathKeydb) && File.Exists(pathTscWallet))
                {
                    File.Delete(pathKeydb);
                    File.Delete(pathTscWallet);
                    if (!File.Exists(pathKeydb) && !File.Exists(pathTscWallet))
                    {
                        Application.Current.MainPage = new EntryPage();
                    }
                    else
                    {
                        await DisplayAlert("แจ้งเตือน", "ลบกระเป๋าเงินไม่สำเร็จ", "ตกลง");
                    }

                }
                else
                {
                    await DisplayAlert("แจ้งเตือน", "ไม่สามารถลบกระเป๋าได้", "ตกลง");
                }
            }
           
            
        }
        
    }
    
}