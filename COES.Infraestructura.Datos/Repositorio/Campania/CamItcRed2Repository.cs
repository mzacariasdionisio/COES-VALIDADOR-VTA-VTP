using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamItcRed2Repository : RepositoryBase, ICamItcRed2Repository
    {
        public CamItcRed2Repository(string strConn) : base(strConn) { }

        CamItcRed2Helper Helper = new CamItcRed2Helper();

        public List<ItcRed2Dto> GetCamItcRed2ByProyCodi(int proyCodi)
        {
            List<ItcRed2Dto> itcred2DTOs = new List<ItcRed2Dto>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamItcRed2ByProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcRed2Dto ob = new ItcRed2Dto();
                    ob.ItcRed2Codi = !dr.IsDBNull(dr.GetOrdinal("ITCRED2CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCRED2CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Linea = !dr.IsDBNull(dr.GetOrdinal("LINEA")) ? dr.GetString(dr.GetOrdinal("LINEA")) : string.Empty;
                    ob.BarraE = !dr.IsDBNull(dr.GetOrdinal("BARRAE")) ? dr.GetString(dr.GetOrdinal("BARRAE")) : string.Empty;
                    ob.BarraR = !dr.IsDBNull(dr.GetOrdinal("BARRAR")) ? dr.GetString(dr.GetOrdinal("BARRAR")) : string.Empty;
                    ob.Nternas = !dr.IsDBNull(dr.GetOrdinal("NTERNAS")) ? (decimal?) dr.GetDecimal(dr.GetOrdinal("NTERNAS")) : null;
                    ob.Tramo = !dr.IsDBNull(dr.GetOrdinal("TRAMO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TRAMO")) : null;
                    ob.Electroducto = !dr.IsDBNull(dr.GetOrdinal("ELECTRODUCTO")) ? dr.GetString(dr.GetOrdinal("ELECTRODUCTO")) : string.Empty;
                    ob.Longitud = !dr.IsDBNull(dr.GetOrdinal("LONGITUD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGITUD")) : null;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                     ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                   itcred2DTOs.Add(ob);
                }
            }

            return itcred2DTOs;
        }

        public bool SaveCamItcRed2(ItcRed2Dto itcred2DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCamItcRed2);
            dbProvider.AddInParameter(dbCommand, "ITCRED2CODI", DbType.Int32, itcred2DTO.ItcRed2Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcred2DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "LINEA", DbType.String, itcred2DTO.Linea);
            dbProvider.AddInParameter(dbCommand, "BARRAE", DbType.String, itcred2DTO.BarraE);
            dbProvider.AddInParameter(dbCommand, "BARRAR", DbType.String, itcred2DTO.BarraR);
            dbProvider.AddInParameter(dbCommand, "NTERNAS", DbType.Int32, itcred2DTO.Nternas);
            dbProvider.AddInParameter(dbCommand, "TRAMO", DbType.Int32, itcred2DTO.Tramo);
            dbProvider.AddInParameter(dbCommand, "ELECTRODUCTO", DbType.String, itcred2DTO.Electroducto);
            dbProvider.AddInParameter(dbCommand, "LONGITUD", DbType.Decimal, itcred2DTO.Longitud);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcred2DTO.Usucreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteCamItcRed2ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCamItcRed2ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ITCRED2CODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastCamItcRed2Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCamItcRed2Id);
            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                count = Convert.ToInt32(result) + 1;
            }
            else
            {
                count = 1;
            }
            return count;
        }

        public List<ItcRed2Dto> GetCamItcRed2ById(int id)
        {
            List<ItcRed2Dto> itcred2DTOs = new List<ItcRed2Dto>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamItcRed2ById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcRed2Dto ob = new ItcRed2Dto();
                    ob.ItcRed2Codi = !dr.IsDBNull(dr.GetOrdinal("ITCRED2CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCRED2CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Linea = !dr.IsDBNull(dr.GetOrdinal("LINEA")) ? dr.GetString(dr.GetOrdinal("LINEA")) : string.Empty;
                    ob.BarraE = !dr.IsDBNull(dr.GetOrdinal("BARRAE")) ? dr.GetString(dr.GetOrdinal("BARRAE")) : string.Empty;
                    ob.BarraR = !dr.IsDBNull(dr.GetOrdinal("BARRAR")) ? dr.GetString(dr.GetOrdinal("BARRAR")) : string.Empty;
                    ob.Nternas = !dr.IsDBNull(dr.GetOrdinal("NTERNAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NTERNAS")) : null;
                    ob.Tramo = !dr.IsDBNull(dr.GetOrdinal("TRAMO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TRAMO")) : null;
                    ob.Electroducto = !dr.IsDBNull(dr.GetOrdinal("ELECTRODUCTO")) ? dr.GetString(dr.GetOrdinal("ELECTRODUCTO")) : string.Empty;
                    ob.Longitud = !dr.IsDBNull(dr.GetOrdinal("LONGITUD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGITUD")) : null;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    itcred2DTOs.Add(ob);
                }
            }

            return itcred2DTOs;
        }

        public bool UpdateCamItcRed2(ItcRed2Dto itcred2DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCamItcRed2);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcred2DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "LINEA", DbType.String, itcred2DTO.Linea);
            dbProvider.AddInParameter(dbCommand, "BARRAE", DbType.String, itcred2DTO.BarraE);
            dbProvider.AddInParameter(dbCommand, "BARRAR", DbType.String, itcred2DTO.BarraR);
            dbProvider.AddInParameter(dbCommand, "NTERNAS", DbType.Int32, itcred2DTO.Nternas);
            dbProvider.AddInParameter(dbCommand, "TRAMO", DbType.Int32, itcred2DTO.Tramo);
            dbProvider.AddInParameter(dbCommand, "ELECTRODUCTO", DbType.String, itcred2DTO.Electroducto);
            dbProvider.AddInParameter(dbCommand, "LONGITUD", DbType.Decimal, itcred2DTO.Longitud);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcred2DTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcred2DTO.Fechamodificacion);
            dbProvider.AddInParameter(dbCommand, "ITCRED2CODI", DbType.Int32, itcred2DTO.ItcRed2Codi);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public List<ItcRed2Dto> GetCamItcRed2ByFilter(string plancodi, string empresa, string estado)
        {
            List<ItcRed2Dto> itcred2DTOs = new List<ItcRed2Dto>();
            string query = $@"
                SELECT CGB.*, TR.AREADEMANDA, TR.EMPRESANOM   FROM CAM_ITCRED2 CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.ITCRED2CODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
           

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcRed2Dto ob = new ItcRed2Dto();
                    ob.ItcRed2Codi = !dr.IsDBNull(dr.GetOrdinal("ITCRED2CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCRED2CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Linea = !dr.IsDBNull(dr.GetOrdinal("LINEA")) ? dr.GetString(dr.GetOrdinal("LINEA")) : string.Empty;
                    ob.BarraE = !dr.IsDBNull(dr.GetOrdinal("BARRAE")) ? dr.GetString(dr.GetOrdinal("BARRAE")) : string.Empty;
                    ob.BarraR = !dr.IsDBNull(dr.GetOrdinal("BARRAR")) ? dr.GetString(dr.GetOrdinal("BARRAR")) : string.Empty;
                    ob.Nternas = !dr.IsDBNull(dr.GetOrdinal("NTERNAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NTERNAS")) : null;
                    ob.Tramo = !dr.IsDBNull(dr.GetOrdinal("TRAMO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TRAMO")) : null;
                    ob.Electroducto = !dr.IsDBNull(dr.GetOrdinal("ELECTRODUCTO")) ? dr.GetString(dr.GetOrdinal("ELECTRODUCTO")) : string.Empty;
                    ob.Longitud = !dr.IsDBNull(dr.GetOrdinal("LONGITUD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGITUD")) : null;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
                    ob.AreaDemanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetString(dr.GetOrdinal("AREADEMANDA")) : "";
                    itcred2DTOs.Add(ob);
                }
            }

            return itcred2DTOs;
        }
    }
}
