using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDeq
{
    public class CustomEventArgs<T>: EventArgs
    {
        public T Value { get; set; }
        public CustomEventArgs(T value)
        {
            Value = value;
        }
    }
}
