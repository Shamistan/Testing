using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Utility
{
    public class TransResult<T>
    {
        public T Object { get; set; }
        public bool IsSuccess { get; set; }
    }
}
