using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CP_PARAMETRO
    /// </summary>
    public class CpParametroRepository: RepositoryBase, ICpParametroRepository
    {
        public CpParametroRepository(string strConn): base(strConn)
        {
        }

        CpParametroHelper helper = new CpParametroHelper();



        public CpParametroDTO GetById(int paramcodi, int topcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Paramcodi, DbType.Int32, paramcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            CpParametroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpParametroDTO> List()
        {
            List<CpParametroDTO> entitys = new List<CpParametroDTO>();
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

        public List<CpParametroDTO> GetByCriteria(int topcodi)
        {
            string sqlQuery = string.Format(helper.SqlGetByCriteria, topcodi);
            List<CpParametroDTO> entitys = new List<CpParametroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void CopiarParametroAEscenario(int topOrigen, int topDestino)
        {
            string sqlCopiar = string.Format(helper.SqlCopiarParametroAEscenario, topOrigen, topDestino);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlCopiar);

            dbProvider.ExecuteNonQuery(command);
        }

    }
}
