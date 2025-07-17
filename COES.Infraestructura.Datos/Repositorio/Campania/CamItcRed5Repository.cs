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
    public class CamItcRed5Repository : RepositoryBase, ICamItcRed5Repository
    {
        public CamItcRed5Repository(string strConn) : base(strConn) { }

        CamItcRed5Helper Helper = new CamItcRed5Helper();

        public List<ItcRed5Dto> GetItcRed5ByProyCodi(int proyCodi)
        {
            List<ItcRed5Dto> itcred5DTOs = new List<ItcRed5Dto>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamItcRed5ByProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcRed5Dto ob = new ItcRed5Dto();
                    ob.ItcRed5Codi = !dr.IsDBNull(dr.GetOrdinal("ITCRED5CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCRED5CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.CaiGen = !dr.IsDBNull(dr.GetOrdinal("CAIGEN")) ? dr.GetString(dr.GetOrdinal("CAIGEN")) : string.Empty;
                    ob.IdGen = !dr.IsDBNull(dr.GetOrdinal("IDGEN")) ? dr.GetString(dr.GetOrdinal("IDGEN")) : string.Empty;
                    ob.Barra = !dr.IsDBNull(dr.GetOrdinal("BARRA")) ? dr.GetString(dr.GetOrdinal("BARRA")) : string.Empty;
                    ob.PdMw = !dr.IsDBNull(dr.GetOrdinal("PDMW")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PDMW")) : null;
                    ob.PnMw = !dr.IsDBNull(dr.GetOrdinal("PNMW")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PNMW")) : null;
                    ob.QnMin = !dr.IsDBNull(dr.GetOrdinal("QNMIN")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("QNMIN")) : null;
                    ob.QnMa = !dr.IsDBNull(dr.GetOrdinal("QNMA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("QNMA")) : null;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;

                    itcred5DTOs.Add(ob);
                }
            }

            return itcred5DTOs;
        }

        public bool SaveItcRed5(ItcRed5Dto itcred5DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCamItcRed5);
            dbProvider.AddInParameter(dbCommand, "ITCRED5CODI", DbType.Int32, itcred5DTO.ItcRed5Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcred5DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "CAIGEN", DbType.String, itcred5DTO.CaiGen);
            dbProvider.AddInParameter(dbCommand, "IDGEN", DbType.String, itcred5DTO.IdGen);
            dbProvider.AddInParameter(dbCommand, "BARRA", DbType.String, itcred5DTO.Barra);
            dbProvider.AddInParameter(dbCommand, "PDMW", DbType.Decimal, itcred5DTO.PdMw);
            dbProvider.AddInParameter(dbCommand, "PNMW", DbType.Decimal, itcred5DTO.PnMw);
            dbProvider.AddInParameter(dbCommand, "QNMIN", DbType.Decimal, itcred5DTO.QnMin);
            dbProvider.AddInParameter(dbCommand, "QNMA", DbType.Decimal, itcred5DTO.QnMa);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcred5DTO.Usucreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);


            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcRed5ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCamItcRed5ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcRed5Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCamItcRed5Id);
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

        public List<ItcRed5Dto> GetItcRed5ById(int id)
        {
            List<ItcRed5Dto> itcred5DTOs = new List<ItcRed5Dto>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamItcRed5ById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcRed5Dto ob = new ItcRed5Dto();
                    ob.ItcRed5Codi = !dr.IsDBNull(dr.GetOrdinal("ITCRED5CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCRED5CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.CaiGen = !dr.IsDBNull(dr.GetOrdinal("CAIGEN")) ? dr.GetString(dr.GetOrdinal("CAIGEN")) : string.Empty;
                    ob.IdGen = !dr.IsDBNull(dr.GetOrdinal("IDGEN")) ? dr.GetString(dr.GetOrdinal("IDGEN")) : string.Empty;
                    ob.Barra = !dr.IsDBNull(dr.GetOrdinal("BARRA")) ? dr.GetString(dr.GetOrdinal("BARRA")) : string.Empty;
                    ob.PdMw = !dr.IsDBNull(dr.GetOrdinal("PDMW")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PDMW")) : null;
                    ob.PnMw = !dr.IsDBNull(dr.GetOrdinal("PNMW")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PNMW")) : null;
                    ob.QnMin = !dr.IsDBNull(dr.GetOrdinal("QNMIN")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("QNMIN")) : null;
                    ob.QnMa = !dr.IsDBNull(dr.GetOrdinal("QNMA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("QNMA")) : null;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    itcred5DTOs.Add(ob);
               }
            }

            return itcred5DTOs;
        }

        public bool UpdateItcRed5(ItcRed5Dto itcred5DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCamItcRed5);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcred5DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "CAIGEN", DbType.String, itcred5DTO.CaiGen);
            dbProvider.AddInParameter(dbCommand, "IDGEN", DbType.String, itcred5DTO.IdGen);
            dbProvider.AddInParameter(dbCommand, "BARRA", DbType.String, itcred5DTO.Barra);
            dbProvider.AddInParameter(dbCommand, "PDMW", DbType.Decimal, itcred5DTO.PdMw);
            dbProvider.AddInParameter(dbCommand, "PNMW", DbType.Decimal, itcred5DTO.PnMw);
            dbProvider.AddInParameter(dbCommand, "QNMIN", DbType.Decimal, itcred5DTO.QnMin);
            dbProvider.AddInParameter(dbCommand, "QNMA", DbType.Decimal, itcred5DTO.QnMa);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcred5DTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcred5DTO.Fechamodificacion);
            dbProvider.AddInParameter(dbCommand, "ITCRED5CODI", DbType.Int32, itcred5DTO.ItcRed5Codi);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public List<ItcRed5Dto> GetItcRed5ByFilter(string plancodi, string empresa, string estado)
        {
            List<ItcRed5Dto> itcred5DTOs = new List<ItcRed5Dto>();
            string query = $@"
                SELECT CGB.*, TR.AREADEMANDA, TR.EMPRESANOM    FROM CAM_ITCRED5 CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.ITCRED5CODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcRed5Dto ob = new ItcRed5Dto();
                    ob.ItcRed5Codi = !dr.IsDBNull(dr.GetOrdinal("ITCRED5CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCRED5CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.CaiGen = !dr.IsDBNull(dr.GetOrdinal("CAIGEN")) ? dr.GetString(dr.GetOrdinal("CAIGEN")) : string.Empty;
                    ob.IdGen = !dr.IsDBNull(dr.GetOrdinal("IDGEN")) ? dr.GetString(dr.GetOrdinal("IDGEN")) : string.Empty;
                    ob.Barra = !dr.IsDBNull(dr.GetOrdinal("BARRA")) ? dr.GetString(dr.GetOrdinal("BARRA")) : string.Empty;
                    ob.PdMw = !dr.IsDBNull(dr.GetOrdinal("PDMW")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PDMW")) : null;
                    ob.PnMw = !dr.IsDBNull(dr.GetOrdinal("PNMW")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PNMW")) : null;
                    ob.QnMin = !dr.IsDBNull(dr.GetOrdinal("QNMIN")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("QNMIN")) : null;
                    ob.QnMa = !dr.IsDBNull(dr.GetOrdinal("QNMA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("QNMA")) : null;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
                    ob.AreaDemanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetString(dr.GetOrdinal("AREADEMANDA")) : "";
                    itcred5DTOs.Add(ob);
                }
            }

            return itcred5DTOs;
        }
    }
}
