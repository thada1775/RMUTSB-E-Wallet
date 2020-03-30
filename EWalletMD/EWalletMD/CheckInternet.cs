using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;


namespace EWalletMD
{
    public static class CheckInternet
    {
        public static bool CheckForInternetConnection()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
            //try
            //{
            //    using (var client = new WebClient())
            //    using (client.OpenRead("http://clients3.google.com/generate_204"))
            //    {
            //        return true;
            //    }
            //}
            //catch
            //{
            //    return false;
            //}
        }
        
    }
}
