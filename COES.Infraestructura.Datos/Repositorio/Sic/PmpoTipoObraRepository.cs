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
    /// Clase de acceso a datos de la tabla PMPO_TIPO_OBRA
    /// </summary>
    public class PmpoTipoObraRepository : RepositoryBase, IPmpoTipoObraRepository
    {
        public PmpoTipoObraRepository(string strConn)
            : base(strConn)
        {
        }

        PmpoTipoObraHelper helper = new PmpoTipoObraHelper();

        public List<PmpoTipoObraDTO> List()
        {
            List<PmpoTipoObraDTO> entitys = new List<PmpoTipoObraDTO>();
            string queryString = string.Format(helper.SqlList);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmpoTipoObraDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
