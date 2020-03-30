using EWalletMD.ViewModels;
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
    public partial class AllCurrencyPage : ContentPage
    {
        private readonly AllCurrencyViewModel model;
        public AllCurrencyPage()
        {
            model = new AllCurrencyViewModel(Navigation);
            BindingContext = model;
            InitializeComponent();
            Title = "เงินทั้งหมด";
            Currenlst.HasUnevenRows = true;
            
            
            Currenlst.ItemTapped += (object sender, ItemTappedEventArgs e) =>
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
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (!model.IsInitialized)
            {
                await model.InitializeEMoney();
            }
            model.triggerStartTimer = true;
            //if(!(BindingContext as AllCurrencyViewModel).IsTimerRun)
            //{
            //    //(BindingContext as AllCurrencyViewModel).triggerStartTimer = false;
            //    //(BindingContext as AllCurrencyViewModel).StartTimmer();
            //}
            //else
            //{
            //    //(BindingContext as AllCurrencyViewModel).triggerStartTimer = true;
            //await (BindingContext as AllCurrencyViewModel).GenerateEMoney();
            //}
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (model.IsInitialized)
            {
                model.triggerStartTimer = false;
            }
        }
    }
}