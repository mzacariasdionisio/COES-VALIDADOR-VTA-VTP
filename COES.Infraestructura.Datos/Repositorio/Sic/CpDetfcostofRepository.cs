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
    /// Clase de acceso a datos de la tabla CP_DETFCOSTOF
    /// </summary>
    public class CpDetfcostofRepository: RepositoryBase, ICpDetfcostofRepository
    {
        public CpDetfcostofRepository(string strConn): base(strConn)
        {
        }

        CpDetfcostofHelper helper = new CpDetfcostofHelper();

        public void Save(CpDetfcostofDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Detfcfncorte, DbType.Int32, entity.Detfcfncorte);
            dbProvider.AddInParameter(command, helper.Detfcfvalores, DbType.String, entity.Detfcfvalores);

            dbProvider.ExecuteNonQuery(command);
        }

        public void CrearCopia(int topcodi1, int topcodi2)
        {
            string query = string.Format(helper.SqlCrearCopia, topcodi1, topcodi2);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int topcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            dbProvider.ExecuteNonQuery(command);
        }

 
        public List<CpDetfcostofDTO> GetByCriteria(int topcodi)
        {
            string sqlQuery = string.Format(helper.SqlGetByCriteria, topcodi);
            List<CpDetfcostofDTO> entitys = new List<CpDetfcostofDTO>();
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
    }
}
