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
    /// Clase de acceso a datos de la tabla Eq_equipo, Eq_area, Si_empresa 
    /// </summary>
    public class SiListarAreaSorteoRepository: RepositoryBase, ISiListarAreaSorteoRepository
    {
        public SiListarAreaSorteoRepository(string strConn): base(strConn)
        {
        }

        SiListarAreaSorteoHelper helper = new SiListarAreaSorteoHelper();

        public List<SiListarAreasDTO> List()
        {
            List<SiListarAreasDTO> entitys = new List<SiListarAreasDTO>();
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
