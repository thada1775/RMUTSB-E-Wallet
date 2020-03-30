using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EWalletMD.Models
{
    public class LedgerTran
    {
        public string Operation { get; set; }
        public string Address { get; set; }
        public string Amount { get; set; }
        public string Time { get; set; }
        public bool VisibleReference { get; set; }
        public string ReferenceCode { get; set; }
        public Color MyTextColor { get; set; }
        public string TxId { get; set; }
    }
}
