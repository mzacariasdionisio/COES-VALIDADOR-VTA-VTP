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
    /// Clase de acceso a datos de la tabla SI_FUENTEDATOS
    /// </summary>
    public class SiFuentedatosRepository : RepositoryBase, ISiFuentedatosRepository
    {
        public SiFuentedatosRepository(string strConn): base(strConn)
        {
        }

        SiFuentedatosHelper helper = new SiFuentedatosHelper();

        public SiFuentedatosDTO GetById(int fdatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fdatcodi, DbType.Int32, fdatcodi);
            SiFuentedatosDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }
            return entity;
        }

        #region PR5
        public List<SiFuentedatosDTO> GetByModulo(int ModCodi)
        {
            List<SiFuentedatosDTO> entitys = new List<SiFuentedatosDTO>();

            string query = string.Format(helper.SqlGetByModulo, ModCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public List<SiFuentedatosDTO> List()
        {
            List<SiFuentedatosDTO> entitys = new List<SiFuentedatosDTO>();

            string query = string.Format(helper.SqlList);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        #endregion
    }
}
