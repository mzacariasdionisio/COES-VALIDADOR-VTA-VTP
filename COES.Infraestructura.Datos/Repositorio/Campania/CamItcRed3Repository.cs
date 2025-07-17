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
    public class CamItcRed3Repository : RepositoryBase, ICamItcRed3Repository
    {
        public CamItcRed3Repository(string strConn) : base(strConn) { }

        CamItcRed3Helper Helper = new CamItcRed3Helper();

        public List<ItcRed3Dto> GetItcRed3ByProyCodi(int proyCodi)
        {
            List<ItcRed3Dto> itcred3DTOs = new List<ItcRed3Dto>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamItcRed3ByProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcRed3Dto ob = new ItcRed3Dto();
                    ob.ItcRed3Codi = !dr.IsDBNull(dr.GetOrdinal("ITCRED3CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCRED3CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.IdCircuito = !dr.IsDBNull(dr.GetOrdinal("IDCIRCUITO")) ? dr.GetString(dr.GetOrdinal("IDCIRCUITO")) : string.Empty;
                    ob.BarraP = !dr.IsDBNull(dr.GetOrdinal("BARRAP")) ? dr.GetString(dr.GetOrdinal("BARRAP")) : string.Empty;
                    ob.BarraS = !dr.IsDBNull(dr.GetOrdinal("BARRAS")) ? dr.GetString(dr.GetOrdinal("BARRAS")) : string.Empty;
                    ob.BarraT = !dr.IsDBNull(dr.GetOrdinal("BARRAT")) ? dr.GetString(dr.GetOrdinal("BARRAT")) : string.Empty;
                    ob.CdgTrafo = !dr.IsDBNull(dr.GetOrdinal("CDGTRAFO")) ? dr.GetString(dr.GetOrdinal("CDGTRAFO")) : string.Empty;
                    ob.OprTap = !dr.IsDBNull(dr.GetOrdinal("OPRTAP")) ? dr.GetString(dr.GetOrdinal("OPRTAP")) : string.Empty;
                    ob.PosTap = !dr.IsDBNull(dr.GetOrdinal("POSTAP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POSTAP")) : null;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                     ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;

                    itcred3DTOs.Add(ob);
                }
            }

            return itcred3DTOs;
        }

        public bool SaveItcRed3(ItcRed3Dto itcred3DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCamItcRed3);
            dbProvider.AddInParameter(dbCommand, "ITCRED3CODI", DbType.Int32, itcred3DTO.ItcRed3Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcred3DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "IDCIRCUITO", DbType.String, itcred3DTO.IdCircuito);
            dbProvider.AddInParameter(dbCommand, "BARRAP", DbType.String, itcred3DTO.BarraP);
            dbProvider.AddInParameter(dbCommand, "BARRAS", DbType.String, itcred3DTO.BarraS);
            dbProvider.AddInParameter(dbCommand, "BARRAT", DbType.String, itcred3DTO.BarraT);
            dbProvider.AddInParameter(dbCommand, "CDGTRAFO", DbType.String, itcred3DTO.CdgTrafo);
            dbProvider.AddInParameter(dbCommand, "OPRTAP", DbType.String, itcred3DTO.OprTap);
            dbProvider.AddInParameter(dbCommand, "POSTAP", DbType.Int32, itcred3DTO.PosTap);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, itcred3DTO.Fechacreacion);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcRed3ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCamItcRed3ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcRed3Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCamItcRed3Id);
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

        public List<ItcRed3Dto> GetItcRed3ById(int id)
        {
            List<ItcRed3Dto> itcred3DTOs = new List<ItcRed3Dto>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamItcRed3ById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcRed3Dto ob = new ItcRed3Dto();
                    ob.ItcRed3Codi = !dr.IsDBNull(dr.GetOrdinal("ITCRED3CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCRED3CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.IdCircuito = !dr.IsDBNull(dr.GetOrdinal("IDCIRCUITO")) ? dr.GetString(dr.GetOrdinal("IDCIRCUITO")) : string.Empty;
                    ob.BarraP = !dr.IsDBNull(dr.GetOrdinal("BARRAP")) ? dr.GetString(dr.GetOrdinal("BARRAP")) : string.Empty;
                    ob.BarraS = !dr.IsDBNull(dr.GetOrdinal("BARRAS")) ? dr.GetString(dr.GetOrdinal("BARRAS")) : string.Empty;
                    ob.BarraT = !dr.IsDBNull(dr.GetOrdinal("BARRAT")) ? dr.GetString(dr.GetOrdinal("BARRAT")) : string.Empty;
                    ob.CdgTrafo = !dr.IsDBNull(dr.GetOrdinal("CDGTRAFO")) ? dr.GetString(dr.GetOrdinal("CDGTRAFO")) : string.Empty;
                    ob.OprTap = !dr.IsDBNull(dr.GetOrdinal("OPRTAP")) ? dr.GetString(dr.GetOrdinal("OPRTAP")) : string.Empty;
                    ob.PosTap = !dr.IsDBNull(dr.GetOrdinal("POSTAP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POSTAP")) : null;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    itcred3DTOs.Add(ob);
                 }
            }

            return itcred3DTOs;
        }

        public bool UpdateItcRed3(ItcRed3Dto itcred3DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCamItcRed3);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcred3DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "IDCIRCUITO", DbType.String, itcred3DTO.IdCircuito);
            dbProvider.AddInParameter(dbCommand, "BARRAP", DbType.String, itcred3DTO.BarraP);
            dbProvider.AddInParameter(dbCommand, "BARRAS", DbType.String, itcred3DTO.BarraS);
            dbProvider.AddInParameter(dbCommand, "BARRAT", DbType.String, itcred3DTO.BarraT);
            dbProvider.AddInParameter(dbCommand, "CDGTRAFO", DbType.String, itcred3DTO.CdgTrafo);
            dbProvider.AddInParameter(dbCommand, "OPRTAP", DbType.String, itcred3DTO.OprTap);
            dbProvider.AddInParameter(dbCommand, "POSTAP", DbType.Int32, itcred3DTO.PosTap);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcred3DTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcred3DTO.Fechacreacion);
            dbProvider.AddInParameter(dbCommand, "ITCRED3CODI", DbType.Int32, itcred3DTO.ItcRed3Codi);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public List<ItcRed3Dto> GetItcRed3ByFilter(string plancodi, string empresa, string estado)
        {
            List<ItcRed3Dto> itcred3DTOs = new List<ItcRed3Dto>();
            string query = $@"
                SELECT CGB.*, TR.AREADEMANDA, TR.EMPRESANOM   FROM CAM_ITCRED3 CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.ITCRED3CODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            //DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamItcRed3ById);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcRed3Dto ob = new ItcRed3Dto();
                    ob.ItcRed3Codi = !dr.IsDBNull(dr.GetOrdinal("ITCRED3CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCRED3CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.IdCircuito = !dr.IsDBNull(dr.GetOrdinal("IDCIRCUITO")) ? dr.GetString(dr.GetOrdinal("IDCIRCUITO")) : string.Empty;
                    ob.BarraP = !dr.IsDBNull(dr.GetOrdinal("BARRAP")) ? dr.GetString(dr.GetOrdinal("BARRAP")) : string.Empty;
                    ob.BarraS = !dr.IsDBNull(dr.GetOrdinal("BARRAS")) ? dr.GetString(dr.GetOrdinal("BARRAS")) : string.Empty;
                    ob.BarraT = !dr.IsDBNull(dr.GetOrdinal("BARRAT")) ? dr.GetString(dr.GetOrdinal("BARRAT")) : string.Empty;
                    ob.CdgTrafo = !dr.IsDBNull(dr.GetOrdinal("CDGTRAFO")) ? dr.GetString(dr.GetOrdinal("CDGTRAFO")) : string.Empty;
                    ob.OprTap = !dr.IsDBNull(dr.GetOrdinal("OPRTAP")) ? dr.GetString(dr.GetOrdinal("OPRTAP")) : string.Empty;
                    ob.PosTap = !dr.IsDBNull(dr.GetOrdinal("POSTAP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POSTAP")) : null;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
                    ob.AreaDemanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetString(dr.GetOrdinal("AREADEMANDA")) : "";
                    itcred3DTOs.Add(ob);
                }
            }

            return itcred3DTOs;
        }
    }
}
