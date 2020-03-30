using System;
using System.Collections.Generic;
using System.Text;

namespace EWalletMD.Models
{
    public enum MenuItemType
    {
        Browse,
        Recover,
        Addcontract,
        MyAddress,
        ChangeAddress,
        Contact
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
        public string Image { get; set; }
    }
}
