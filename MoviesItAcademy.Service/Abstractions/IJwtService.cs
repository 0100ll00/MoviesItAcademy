using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.Service.Abstractions
{
    public interface IJwtService
    {
        string GenerateSecurityToken(string userName);
    }
}
