
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CP_TERMINAL
    /// </summary>
    public class TerminalRepository : RepositoryBase, ICpTerminalRepository
    {
        public TerminalRepository(string strConn): base(strConn)
        {
        }

        CpTerminalHelper helper = new CpTerminalHelper();

        
        public List<CpTerminalDTO> List()
        {
            List<CpTerminalDTO> entitys = new List<CpTerminalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }



        /// <summary>
        /// Obtiene el codigo del nodo topologico al cual el recurso esta conectado
        /// </summary>
        /// <param name="recurcodi"></param>
        /// <param name="topcodi"></param>
        /// <returns></returns>
        public int ObtenerNodoTopologico(int recurcodi,int ttermcodi, int topcodi)
        {
            string queryString = string.Format( helper.SqlGetNodoTopologico,recurcodi,ttermcodi,ConstantesBase.TerNodoT, topcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            object result = dbProvider.ExecuteScalar(command);
            int id = 0;
            //object ret = System.DBNull.Value;
            if (!DBNull.Value.Equals(result )) id = Convert.ToInt32(result);
            return id;
        }

        public void CrearCopia(int topcodi1, int topcodi2)
        {
            string query = string.Format(helper.SqlCrearCopia, topcodi1, topcodi2);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public void CrearCopiaNodoConectividad(int topcodi1, int topcodi2)
        {
            string query = string.Format(helper.SqlCrearCopiaNodoConectividad, topcodi1, topcodi2);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }


    }
}
