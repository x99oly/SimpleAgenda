using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAgenda.Interfaces
{
    public interface IPublicDtoConvertable<T> where T : class
    {
        T ConvertToPublicDto();
    }
}
