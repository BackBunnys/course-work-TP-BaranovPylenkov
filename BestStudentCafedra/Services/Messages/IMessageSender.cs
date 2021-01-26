using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Services.Messages
{
    public interface IMessageSender<T, MessageT>
    {
        public Task SendAsync(T to, MessageT message);
    }
}
