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
    public class ExtEstadoEnvioRepository : RepositoryBase, IExtEstadoEnvioRepository
    {
        public ExtEstadoEnvioRepository(string strConn)
            : base(strConn)
        {
        }

        ExtEstadoEnvioHelper helper = new ExtEstadoEnvioHelper();

        public List<ExtEstadoEnvioDTO> List()
        {
            List<ExtEstadoEnvioDTO> entitys = new List<ExtEstadoEnvioDTO>();
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
    }
}
