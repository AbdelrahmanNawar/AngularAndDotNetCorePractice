using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackendApi.Models
{
    public interface IServiceMessage
    {
        public void SendMessageAsync(string message);
    }
}
