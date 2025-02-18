using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizDAL.Repository.Interface;


namespace GroupBoizDAL.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IAccountRepository AccountRepo { get; }
    ITokenRepository TokenRepo { get; }
    INewsRepository NewsRepo { get; }

    Task<int> SaveAsync();
    Task<bool> SaveChangeAsync();
}
