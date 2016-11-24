using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.MobileServices;
using Bot_Application1.Models;
using System.Threading.Tasks;

namespace Bot_Application1
{
    public class AzureManager
    {

        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<Branch> branchTable;
        private IMobileServiceTable<Creditcard> creditcardTable;

        private AzureManager()
        {
            this.client = new MobileServiceClient("https://contosork.azurewebsites.net/");
            this.branchTable = this.client.GetTable<Branch>();
            this.creditcardTable = this.client.GetTable<Creditcard>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }

        public async Task<List<Branch>> GetBranches()
        {
            return await this.branchTable.ToListAsync();
        }

        public async Task<List<Creditcard>> GetCreditcards()
        {
            return await this.creditcardTable.ToListAsync();
        }

    }
}