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
    public class CamItcRed1Repository : RepositoryBase, ICamItcRed1Repository
    {
        public CamItcRed1Repository(string strConn) : base(strConn) { }

        CamItcRed1Helper Helper = new CamItcRed1Helper();

        public List<ItcRed1Dto> GetItcRed1ByProyCodi(int proyCodi)
        {
            List<ItcRed1Dto> itcRed1DTOs = new List<ItcRed1Dto>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamItcRed1ByProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcRed1Dto ob = new ItcRed1Dto();
                    ob.ItcRed1Codi = !dr.IsDBNull(dr.GetOrdinal("ITCRED1CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCRED1CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Barra = !dr.IsDBNull(dr.GetOrdinal("BARRA")) ? dr.GetString(dr.GetOrdinal("BARRA")) : string.Empty;
                    ob.Vnpu = !dr.IsDBNull(dr.GetOrdinal("VNPU")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VNPU")) : null;
                    ob.Vopu = !dr.IsDBNull(dr.GetOrdinal("VOPU")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VOPU")) : null;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;

                    itcRed1DTOs.Add(ob);
                }
            }

            return itcRed1DTOs;
        }

        public bool SaveItcRed1(ItcRed1Dto itcRed1DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCamItcRed1);
            dbProvider.AddInParameter(dbCommand, "ITCRED1CODI", DbType.Int32, itcRed1DTO.ItcRed1Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcRed1DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "BARRA", DbType.String, itcRed1DTO.Barra);
            dbProvider.AddInParameter(dbCommand, "VNPU", DbType.Decimal, itcRed1DTO.Vnpu);
            dbProvider.AddInParameter(dbCommand, "VOPU", DbType.Decimal, itcRed1DTO.Vopu);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, itcRed1DTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcRed1DTO.Usucreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcRed1ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCamItcRed1ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcRed1Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCamItcRed1Id);
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

        public List<ItcRed1Dto> GetItcRed1ById(int id)
        {
            List<ItcRed1Dto> itcRed1DTOs = new List<ItcRed1Dto>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamItcRed1ById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcRed1Dto ob = new ItcRed1Dto();
                    ob.ItcRed1Codi = !dr.IsDBNull(dr.GetOrdinal("ITCRED1CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCRED1CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Barra = !dr.IsDBNull(dr.GetOrdinal("BARRA")) ? dr.GetString(dr.GetOrdinal("BARRA")) : string.Empty;
                    ob.Vnpu = !dr.IsDBNull(dr.GetOrdinal("VNPU")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VNPU")) : null;
                    ob.Vopu = !dr.IsDBNull(dr.GetOrdinal("VOPU")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VOPU")) : null;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    itcRed1DTOs.Add(ob);
                }
            }

            return itcRed1DTOs;
        }

        public bool UpdateItcRed1(ItcRed1Dto itcRed1DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCamItcRed1);
            
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcRed1DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "BARRA", DbType.String, itcRed1DTO.Barra);
            dbProvider.AddInParameter(dbCommand, "VNPU", DbType.Decimal, itcRed1DTO.Vnpu);
            dbProvider.AddInParameter(dbCommand, "VOPU", DbType.Decimal, itcRed1DTO.Vopu);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, itcRed1DTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcRed1DTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcRed1DTO.Fechamodificacion);
            dbProvider.AddInParameter(dbCommand, "ITCRED1CODI", DbType.Int32, itcRed1DTO.ItcRed1Codi);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public List<ItcRed1Dto> GetItcRed1ByFilter(string plancodi, string empresa, string estado)
        {
            List<ItcRed1Dto> itcRed1DTOs = new List<ItcRed1Dto>();
            string query = $@"
                SELECT CGB.*, TR.AREADEMANDA, TR.EMPRESANOM  
                FROM CAM_ITCRED1 CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.ITCRED1CODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcRed1Dto ob = new ItcRed1Dto();
                    ob.ItcRed1Codi = !dr.IsDBNull(dr.GetOrdinal("ITCRED1CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCRED1CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Barra = !dr.IsDBNull(dr.GetOrdinal("BARRA")) ? dr.GetString(dr.GetOrdinal("BARRA")) : string.Empty;
                    ob.Vnpu = !dr.IsDBNull(dr.GetOrdinal("VNPU")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VNPU")) : null;
                    ob.Vopu = !dr.IsDBNull(dr.GetOrdinal("VOPU")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VOPU")) : null;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
                    ob.AreaDemanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetString(dr.GetOrdinal("AREADEMANDA")) : "";
                    itcRed1DTOs.Add(ob);
                }
            }

            return itcRed1DTOs;
        }
    }
}
