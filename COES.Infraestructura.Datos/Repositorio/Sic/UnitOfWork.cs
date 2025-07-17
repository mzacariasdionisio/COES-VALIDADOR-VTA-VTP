using COES.Base.Core;
using COES.Dominio.Interfaces.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class UnitOfWork : RepositoryBase, IUnitOfWork
    {
        private readonly string strConexion;
        public UnitOfWork(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public IDbTransaction StartTransaction(IDbConnection conn)
        {
            return conn.BeginTransaction();
        }
    }
}
