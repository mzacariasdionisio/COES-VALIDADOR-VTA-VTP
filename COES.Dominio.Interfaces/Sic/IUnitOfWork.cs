using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IUnitOfWork
    {
        IDbConnection BeginConnection();
        IDbTransaction StartTransaction(IDbConnection conn);
    }
}
