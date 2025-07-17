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
    /// Clase de acceso a datos de la tabla CP_DETALLEETAPA
    /// </summary>
    public class CpDetalleetapaRepository: RepositoryBase, ICpDetalleetapaRepository
    {
        public CpDetalleetapaRepository(string strConn): base(strConn)
        {
        }

        CpDetalleetapaHelper helper = new CpDetalleetapaHelper();


        public List<CpDetalleetapaDTO> List(int topcodi)
        {
            List<CpDetalleetapaDTO> entitys = new List<CpDetalleetapaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void CrearCopia(int topcodi1, int topcodi2)
        {
            string query = string.Format(helper.SqlCrearCopia, topcodi1, topcodi2);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<CpDetalleetapaDTO> Listar(string topcodis)
        {
            List<CpDetalleetapaDTO> entitys = new List<CpDetalleetapaDTO>();

            string query = string.Format(helper.SqlListarPorTopologia, topcodis);
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
    }
}
