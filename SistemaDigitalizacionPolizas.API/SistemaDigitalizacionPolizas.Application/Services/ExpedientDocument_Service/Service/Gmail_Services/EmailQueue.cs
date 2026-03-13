using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service.Gmail_Services
{
    public class EmailQueue : IEmailQueue
    {
        private readonly ConcurrentQueue<Func<Task>> _queue = new();


        public void Enqueue(Func<Task> emailTask)
        {
            _queue.Enqueue(emailTask);
        }

        public bool TryDequeue(out Func<Task>? emailTask)
        {
            return _queue.TryDequeue(out emailTask);
        }
    }
}
