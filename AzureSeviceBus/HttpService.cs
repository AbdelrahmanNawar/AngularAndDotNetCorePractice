using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
namespace AzureSeviceBus
{
    class HttpService
    {
        HttpClient client;

        public HttpService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:64127/api/orders/");
        }

        public async void SendUpdateRequest(int id, int status)
        {
            var sendStatus = new { Id = id, Status = status };

            var response = await client.PutAsJsonAsync("", sendStatus);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Order: {id} has been updated its status");
            }
            else
            {
                Console.WriteLine(response.ToString());
            }
        }

    }
}
