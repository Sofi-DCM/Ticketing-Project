using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Handlers._User
{
    public interface IValidateUserCredentialsHandler
    {
        Task<int> HandleAsync(string name, string password);
    }
}
