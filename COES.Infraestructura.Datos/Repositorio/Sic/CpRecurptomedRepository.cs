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
    /// Clase de acceso a datos de la tabla CP_RECURPTOMED
    /// </summary>
    public class CpRecurptomedRepository: RepositoryBase, ICpRecurptomedRepository
    {
        public CpRecurptomedRepository(string strConn): base(strConn)
        {
        }

        CpRecurptomedHelper helper = new CpRecurptomedHelper();

 

        public void CrearCopiaRecurptomed(int topcodiOrigen,int topcodiDestino)
        {
            string query = string.Format(helper.SqlCrearCopiaRecurptomed,topcodiOrigen,topcodiDestino);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public void CrearCopia(int topcodi1, int topcodi2)
        {
            string query = string.Format(helper.SqlCrearCopia, topcodi1, topcodi2);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<CpRecurptomedDTO> ListByTopcodi(int topcodi)
        {
            List<CpRecurptomedDTO> entitys = new List<CpRecurptomedDTO>();
            string sql = String.Format(helper.SqlListByTopcodi, topcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iCatnombre = dr.GetOrdinal(this.helper.Catnombre);
                    if (!dr.IsDBNull(iCatnombre)) entity.Catnombre = dr.GetString(iCatnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
