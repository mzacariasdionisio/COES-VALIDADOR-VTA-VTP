using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
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
    /// Clase de acceso a datos de la tabla CP_SUBRESTRICCION
    /// </summary>
    public class CpSubrestriccionRepository : RepositoryBase, ICpSubrestriccionRepository
    {
        public CpSubrestriccionRepository(string strConn): base(strConn)
        {
        }

        CpSubrestriccionHelper helper = new CpSubrestriccionHelper();

        public CpSubrestriccionDTO GetById(int srestriccodi, int topcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Srestriccodi, DbType.Int32, srestriccodi);
            CpSubrestriccionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpSubrestriccionDTO> List()
        {
            List<CpSubrestriccionDTO> entitys = new List<CpSubrestriccionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            CpSubrestriccionDTO entity = new CpSubrestriccionDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpSubrestriccionDTO();
                    entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpSubrestriccionDTO> GetByCriteria(short restriccodi)
        {
            List<CpSubrestriccionDTO> entitys = new List<CpSubrestriccionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Restriccodi, DbType.Int32, restriccodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

    }
}
