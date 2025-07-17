using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

using Microsoft.Practices.EnterpriseLibrary.Data; //STS: Conexion para DB

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SMA_OFERTA
    /// </summary>
    public class SmaReporteRepository: RepositoryBase, ISmaReporteRepository
    {

        public SmaReporteRepository(string strConn): base(strConn)
        {
        }

        SmaReporteHelper helper = new SmaReporteHelper();


        public List<SmaReporteDTO> List()
        {
            List<SmaReporteDTO> entitys = new List<SmaReporteDTO>();
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
