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
    /// Clase de acceso a datos de la tabla CP_PROPIEDAD
    /// </summary>
    public class CpPropiedadRepository : RepositoryBase, ICpPropiedadRepository
    {
        public CpPropiedadRepository(string strConn): base(strConn)
        {
         }

        CpPropiedadHelper helper = new CpPropiedadHelper();



        public List<CpPropiedadDTO> List()
        {
            List<CpPropiedadDTO> entitys = new List<CpPropiedadDTO>();
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

        public List<CpPropiedadDTO> GetByCriteria(int pCategoria)
        {
            List<CpPropiedadDTO> entitys = new List<CpPropiedadDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Catcodi, DbType.Int32, pCategoria);
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
