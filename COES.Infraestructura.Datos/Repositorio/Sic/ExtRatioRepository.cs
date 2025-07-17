using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data.Common;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class ExtRatioRepository : RepositoryBase, IExtRatioRepository
    {
        public ExtRatioRepository(string strConn) : base(strConn)
        {
        }

        ExtRatioHelper helper = new ExtRatioHelper();
        public int GetMaxCodi()
        {
            int maxcodigo=0;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetSqlXml("GetMaxCodi"));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    maxcodigo = dr.GetInt32(0);
                }
            }

            return maxcodigo;
        }
    }
}
