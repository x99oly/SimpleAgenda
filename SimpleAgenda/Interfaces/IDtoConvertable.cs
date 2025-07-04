using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAgenda.Interfaces
{
    internal interface IDtoConvertable<T>
    {
        T ConvertToInternalDto();
    }
}
