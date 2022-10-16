using hangfiredemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hangfiredemo.Repository
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(UserDetails users);
    }
}
