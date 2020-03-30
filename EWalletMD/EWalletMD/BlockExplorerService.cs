using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EWalletMD
{
    public class BlockExplorerService
    {
        
        private string _pathApiFile;
        private string _defaultAPI = "https://insight.thaismartcontract.com/";
        public BlockExplorerService()
        {
            _pathApiFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "APIBlock.txt");
        }
        public async Task<string> GetBlockExplorer()
        {

            if (File.Exists(_pathApiFile))
            {
                string apiAddress = File.ReadAllText(_pathApiFile);
                try
                {
                    using (var httpclient = new HttpClient())
                    {
                        string checkApi = apiAddress + "/api/sync";
                        httpclient.Timeout = TimeSpan.FromSeconds(3);
                        var result = await httpclient.GetStringAsync(checkApi);

                    }
                    return apiAddress;
                }
                catch (Exception e)
                {
                    //do something here to make the site unusable, e.g:

                    return _defaultAPI;
                }

            }
            return _defaultAPI;
        }

        public string GetDefaultBlockExplorer()
        {
            return _defaultAPI;
        }
    }
}
