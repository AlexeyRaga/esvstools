using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.VS.Tools.Infrastructure
{
    public interface IConsume<T> 
    {
        void Consume(T message);
    }
}
