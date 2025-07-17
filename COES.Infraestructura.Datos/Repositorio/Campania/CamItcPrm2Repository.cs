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
    public class CamItcPrm2Repository : RepositoryBase, ICamItcPrm2Repository
    {
        public CamItcPrm2Repository(string strConn) : base(strConn) { }

        CamItcPrm2Helper Helper = new CamItcPrm2Helper();

        public List<ItcPrm2Dto> GetItcPrm2Codi(int proyCodi)
        {
            List<ItcPrm2Dto> itcprm2DTOs = new List<ItcPrm2Dto>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamItcPrm2ByProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcPrm2Dto ob = new ItcPrm2Dto();
                    ob.ItcPrm2Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDPRM2CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDPRM2CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Transformador = !dr.IsDBNull(dr.GetOrdinal("TRANSFORMADOR")) ? dr.GetString(dr.GetOrdinal("TRANSFORMADOR")) : "";
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : "";
                    ob.Fases = !dr.IsDBNull(dr.GetOrdinal("FASES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("FASES")) :null;
                    ob.Ndvn = !dr.IsDBNull(dr.GetOrdinal("NDVN")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NDVN")) : null;
                    ob.Vnp = !dr.IsDBNull(dr.GetOrdinal("VNP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VNP")) : null;
                    ob.Vns = !dr.IsDBNull(dr.GetOrdinal("VNS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VNS")) : null;
                    ob.Vnt = !dr.IsDBNull(dr.GetOrdinal("VNT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VNT")) : null;
                    ob.Pnp = !dr.IsDBNull(dr.GetOrdinal("PNP")) ? dr.GetString(dr.GetOrdinal("PNP")) : "";
                    ob.Pns = !dr.IsDBNull(dr.GetOrdinal("PNS")) ? dr.GetString(dr.GetOrdinal("PNS")) : "";
                    ob.Pnt = !dr.IsDBNull(dr.GetOrdinal("PNT")) ? dr.GetString(dr.GetOrdinal("PNT")) : "";
                    ob.Tccps = !dr.IsDBNull(dr.GetOrdinal("TCCPS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TCCPS")) : null;
                    ob.Tccst = !dr.IsDBNull(dr.GetOrdinal("TCCST")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TCCST")) : null;
                    ob.Tcctp = !dr.IsDBNull(dr.GetOrdinal("TCCTP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TCCTP")) : null;
                    ob.Pcups = !dr.IsDBNull(dr.GetOrdinal("PCUPS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PCUPS")) : null;
                    ob.Pcust = !dr.IsDBNull(dr.GetOrdinal("PCUST")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PCUST")) : null;
                    ob.Pcutp = !dr.IsDBNull(dr.GetOrdinal("PCUTP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PCUTP")) : null;
                    ob.Pfe = !dr.IsDBNull(dr.GetOrdinal("PFE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PFE")) : null;
                    ob.Ivacio = !dr.IsDBNull(dr.GetOrdinal("IVACIO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IVACIO")) : null;
                    ob.Grpcnx = !dr.IsDBNull(dr.GetOrdinal("GRPCNX")) ? dr.GetString(dr.GetOrdinal("GRPCNX")) : "";
                    ob.Taptipo = !dr.IsDBNull(dr.GetOrdinal("TAPTIPO")) ? dr.GetString(dr.GetOrdinal("TAPTIPO")) : "";
                    ob.Taplado = !dr.IsDBNull(dr.GetOrdinal("TAPLADO")) ? dr.GetString(dr.GetOrdinal("TAPLADO")) : "";
                    ob.Tapdv = !dr.IsDBNull(dr.GetOrdinal("TAPDV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TAPDV")) : null;
                    ob.Tapmin = !dr.IsDBNull(dr.GetOrdinal("TAPMIN")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TAPMIN")) : null;
                    ob.Tapcnt = !dr.IsDBNull(dr.GetOrdinal("TAPCNT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TAPCNT")) : null;
                    ob.Tapmax = !dr.IsDBNull(dr.GetOrdinal("TAPMAX")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TAPMAX")) : null;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    itcprm2DTOs.Add(ob);
                }
            }

            return itcprm2DTOs;
        }

        public bool SaveItcPrm2(ItcPrm2Dto itcprm2DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCamItcPrm2);
            dbProvider.AddInParameter(dbCommand, "ITCDPRM2CODI", DbType.Int32, itcprm2DTO.ItcPrm2Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcprm2DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "TRANSFORMADOR", DbType.String, itcprm2DTO.Transformador);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, itcprm2DTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "FASES", DbType.Int32, itcprm2DTO.Fases);
            dbProvider.AddInParameter(dbCommand, "NDVN", DbType.Int32, itcprm2DTO.Ndvn);
            dbProvider.AddInParameter(dbCommand, "VNP", DbType.Decimal, itcprm2DTO.Vnp);
            dbProvider.AddInParameter(dbCommand, "VNS", DbType.Decimal, itcprm2DTO.Vns);
            dbProvider.AddInParameter(dbCommand, "VNT", DbType.Decimal, itcprm2DTO.Vnt);
            dbProvider.AddInParameter(dbCommand, "PNP", DbType.String, itcprm2DTO.Pnp);
            dbProvider.AddInParameter(dbCommand, "PNS", DbType.String, itcprm2DTO.Pns);
            dbProvider.AddInParameter(dbCommand, "PNT", DbType.String, itcprm2DTO.Pnt);
            dbProvider.AddInParameter(dbCommand, "TCCPS", DbType.Decimal, itcprm2DTO.Tccps);
            dbProvider.AddInParameter(dbCommand, "TCCST", DbType.Decimal, itcprm2DTO.Tccst);
            dbProvider.AddInParameter(dbCommand, "TCCTP", DbType.Decimal, itcprm2DTO.Tcctp);
            dbProvider.AddInParameter(dbCommand, "PCUPS", DbType.Decimal, itcprm2DTO.Pcups);
            dbProvider.AddInParameter(dbCommand, "PCUST", DbType.Decimal, itcprm2DTO.Pcust);
            dbProvider.AddInParameter(dbCommand, "PCUTP", DbType.Decimal, itcprm2DTO.Pcutp);
            dbProvider.AddInParameter(dbCommand, "PFE", DbType.Decimal, itcprm2DTO.Pfe);
            dbProvider.AddInParameter(dbCommand, "IVACIO", DbType.Decimal, itcprm2DTO.Ivacio);
            dbProvider.AddInParameter(dbCommand, "GRPCNX", DbType.String, itcprm2DTO.Grpcnx);
            dbProvider.AddInParameter(dbCommand, "TAPTIPO", DbType.String, itcprm2DTO.Taptipo);
            dbProvider.AddInParameter(dbCommand, "TAPLADO", DbType.String, itcprm2DTO.Taplado);
            dbProvider.AddInParameter(dbCommand, "TAPDV", DbType.Decimal, itcprm2DTO.Tapdv);
            dbProvider.AddInParameter(dbCommand, "TAPMIN", DbType.Int32, itcprm2DTO.Tapmin);
            dbProvider.AddInParameter(dbCommand, "TAPCNT", DbType.Int32, itcprm2DTO.Tapcnt);
            dbProvider.AddInParameter(dbCommand, "TAPMAX", DbType.Int32, itcprm2DTO.Tapmax);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcprm2DTO.Usucreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcPrm2ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCamItcPrm2ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcPrm2Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCamItcPrm2Id);
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

        public List<ItcPrm2Dto> GetItcPrm2ById(int id)
        {
            List<ItcPrm2Dto> itcprm2DTOs = new List<ItcPrm2Dto>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamItcPrm2ById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcPrm2Dto ob = new ItcPrm2Dto();
                    ob.ItcPrm2Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDPRM2CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDPRM2CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Transformador = !dr.IsDBNull(dr.GetOrdinal("TRANSFORMADOR")) ? dr.GetString(dr.GetOrdinal("TRANSFORMADOR")) : "";
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : "";
                    ob.Fases = !dr.IsDBNull(dr.GetOrdinal("FASES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("FASES")) : null;
                    ob.Ndvn = !dr.IsDBNull(dr.GetOrdinal("NDVN")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NDVN")) : null;
                    ob.Vnp = !dr.IsDBNull(dr.GetOrdinal("VNP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VNP")) : null;
                    ob.Vns = !dr.IsDBNull(dr.GetOrdinal("VNS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VNS")) : null;
                    ob.Vnt = !dr.IsDBNull(dr.GetOrdinal("VNT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VNT")) : null;
                    ob.Pnp = !dr.IsDBNull(dr.GetOrdinal("PNP")) ? dr.GetString(dr.GetOrdinal("PNP")) : "";
                    ob.Pns = !dr.IsDBNull(dr.GetOrdinal("PNS")) ? dr.GetString(dr.GetOrdinal("PNS")) : "";
                    ob.Pnt = !dr.IsDBNull(dr.GetOrdinal("PNT")) ? dr.GetString(dr.GetOrdinal("PNT")) : "";
                    ob.Tccps = !dr.IsDBNull(dr.GetOrdinal("TCCPS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TCCPS")) : null;
                    ob.Tccst = !dr.IsDBNull(dr.GetOrdinal("TCCST")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TCCST")) : null;
                    ob.Tcctp = !dr.IsDBNull(dr.GetOrdinal("TCCTP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TCCTP")) : null;
                    ob.Pcups = !dr.IsDBNull(dr.GetOrdinal("PCUPS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PCUPS")) : null;
                    ob.Pcust = !dr.IsDBNull(dr.GetOrdinal("PCUST")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PCUST")) : null;
                    ob.Pcutp = !dr.IsDBNull(dr.GetOrdinal("PCUTP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PCUTP")) : null;
                    ob.Pfe = !dr.IsDBNull(dr.GetOrdinal("PFE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PFE")) : null;
                    ob.Ivacio = !dr.IsDBNull(dr.GetOrdinal("IVACIO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IVACIO")) : null;
                    ob.Grpcnx = !dr.IsDBNull(dr.GetOrdinal("GRPCNX")) ? dr.GetString(dr.GetOrdinal("GRPCNX")) : "";
                    ob.Taptipo = !dr.IsDBNull(dr.GetOrdinal("TAPTIPO")) ? dr.GetString(dr.GetOrdinal("TAPTIPO")) : "";
                    ob.Taplado = !dr.IsDBNull(dr.GetOrdinal("TAPLADO")) ? dr.GetString(dr.GetOrdinal("TAPLADO")) : "";
                    ob.Tapdv = !dr.IsDBNull(dr.GetOrdinal("TAPDV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TAPDV")) : null;
                    ob.Tapmin = !dr.IsDBNull(dr.GetOrdinal("TAPMIN")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TAPMIN")) : null;
                    ob.Tapcnt = !dr.IsDBNull(dr.GetOrdinal("TAPCNT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TAPCNT")) : null;
                    ob.Tapmax = !dr.IsDBNull(dr.GetOrdinal("TAPMAX")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TAPMAX")) : null;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    itcprm2DTOs.Add(ob);
                }
            }

            return itcprm2DTOs;
        }

        public bool UpdateItcPrm2(ItcPrm2Dto itcprm2DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCamItcPrm2);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcprm2DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "TRANSFORMADOR", DbType.String, itcprm2DTO.Transformador);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, itcprm2DTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "FASES", DbType.Int32, itcprm2DTO.Fases);
            dbProvider.AddInParameter(dbCommand, "NDVN", DbType.Int32, itcprm2DTO.Ndvn);
            dbProvider.AddInParameter(dbCommand, "VNP", DbType.Decimal, itcprm2DTO.Vnp);
            dbProvider.AddInParameter(dbCommand, "VNS", DbType.Decimal, itcprm2DTO.Vns);
            dbProvider.AddInParameter(dbCommand, "VNT", DbType.Decimal, itcprm2DTO.Vnt);
            dbProvider.AddInParameter(dbCommand, "PNP", DbType.String, itcprm2DTO.Pnp);
            dbProvider.AddInParameter(dbCommand, "PNS", DbType.String, itcprm2DTO.Pns);
            dbProvider.AddInParameter(dbCommand, "PNT", DbType.String, itcprm2DTO.Pnt);
            dbProvider.AddInParameter(dbCommand, "TCCPS", DbType.Decimal, itcprm2DTO.Tccps);
            dbProvider.AddInParameter(dbCommand, "TCCST", DbType.Decimal, itcprm2DTO.Tccst);
            dbProvider.AddInParameter(dbCommand, "TCCTP", DbType.Decimal, itcprm2DTO.Tcctp);
            dbProvider.AddInParameter(dbCommand, "PCUPS", DbType.Decimal, itcprm2DTO.Pcups);
            dbProvider.AddInParameter(dbCommand, "PCUST", DbType.Decimal, itcprm2DTO.Pcust);
            dbProvider.AddInParameter(dbCommand, "PCUTP", DbType.Decimal, itcprm2DTO.Pcutp);
            dbProvider.AddInParameter(dbCommand, "PFE", DbType.Decimal, itcprm2DTO.Pfe);
            dbProvider.AddInParameter(dbCommand, "IVACIO", DbType.Decimal, itcprm2DTO.Ivacio);
            dbProvider.AddInParameter(dbCommand, "GRPCNX", DbType.String, itcprm2DTO.Grpcnx);
            dbProvider.AddInParameter(dbCommand, "TAPTIPO", DbType.String, itcprm2DTO.Taptipo);
            dbProvider.AddInParameter(dbCommand, "TAPLADO", DbType.String, itcprm2DTO.Taplado);
            dbProvider.AddInParameter(dbCommand, "TAPDV", DbType.Decimal, itcprm2DTO.Tapdv);
            dbProvider.AddInParameter(dbCommand, "TAPMIN", DbType.Int32, itcprm2DTO.Tapmin);
            dbProvider.AddInParameter(dbCommand, "TAPCNT", DbType.Int32, itcprm2DTO.Tapcnt);
            dbProvider.AddInParameter(dbCommand, "TAPMAX", DbType.Int32, itcprm2DTO.Tapmax);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcprm2DTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcprm2DTO.Fechamodificacion);
            dbProvider.AddInParameter(dbCommand, "ITCDPRM2CODI", DbType.Int32, itcprm2DTO.ItcPrm2Codi);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }


        public List<ItcPrm2Dto> GetItcPrm2ByFilter(string plancodi, string empresa, string estado)
        {
            List<ItcPrm2Dto> itcprm2DTOs = new List<ItcPrm2Dto>();
            string query = $@"
                SELECT CGB.*, TR.AREADEMANDA, TR.EMPRESANOM   
                FROM CAM_ITCPRM2 CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.ITCDPRM2CODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcPrm2Dto ob = new ItcPrm2Dto();
                    ob.ItcPrm2Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDPRM2CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDPRM2CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Transformador = !dr.IsDBNull(dr.GetOrdinal("TRANSFORMADOR")) ? dr.GetString(dr.GetOrdinal("TRANSFORMADOR")) : "";
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : "";
                    ob.Fases = !dr.IsDBNull(dr.GetOrdinal("FASES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("FASES")) : null;
                    ob.Ndvn = !dr.IsDBNull(dr.GetOrdinal("NDVN")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NDVN")) : null;
                    ob.Vnp = !dr.IsDBNull(dr.GetOrdinal("VNP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VNP")) : null;
                    ob.Vns = !dr.IsDBNull(dr.GetOrdinal("VNS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VNS")) : null;
                    ob.Vnt = !dr.IsDBNull(dr.GetOrdinal("VNT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VNT")) : null;
                    ob.Pnp = !dr.IsDBNull(dr.GetOrdinal("PNP")) ? dr.GetString(dr.GetOrdinal("PNP")) : "";
                    ob.Pns = !dr.IsDBNull(dr.GetOrdinal("PNS")) ? dr.GetString(dr.GetOrdinal("PNS")) : "";
                    ob.Pnt = !dr.IsDBNull(dr.GetOrdinal("PNT")) ? dr.GetString(dr.GetOrdinal("PNT")) : "";
                    ob.Tccps = !dr.IsDBNull(dr.GetOrdinal("TCCPS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TCCPS")) : null;
                    ob.Tccst = !dr.IsDBNull(dr.GetOrdinal("TCCST")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TCCST")) : null;
                    ob.Tcctp = !dr.IsDBNull(dr.GetOrdinal("TCCTP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TCCTP")) : null;
                    ob.Pcups = !dr.IsDBNull(dr.GetOrdinal("PCUPS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PCUPS")) : null;
                    ob.Pcust = !dr.IsDBNull(dr.GetOrdinal("PCUST")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PCUST")) : null;
                    ob.Pcutp = !dr.IsDBNull(dr.GetOrdinal("PCUTP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PCUTP")) : null;
                    ob.Pfe = !dr.IsDBNull(dr.GetOrdinal("PFE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PFE")) : null;
                    ob.Ivacio = !dr.IsDBNull(dr.GetOrdinal("IVACIO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IVACIO")) : null;
                    ob.Grpcnx = !dr.IsDBNull(dr.GetOrdinal("GRPCNX")) ? dr.GetString(dr.GetOrdinal("GRPCNX")) : "";
                    ob.Taptipo = !dr.IsDBNull(dr.GetOrdinal("TAPTIPO")) ? dr.GetString(dr.GetOrdinal("TAPTIPO")) : "";
                    ob.Taplado = !dr.IsDBNull(dr.GetOrdinal("TAPLADO")) ? dr.GetString(dr.GetOrdinal("TAPLADO")) : "";
                    ob.Tapdv = !dr.IsDBNull(dr.GetOrdinal("TAPDV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TAPDV")) : null;
                    ob.Tapmin = !dr.IsDBNull(dr.GetOrdinal("TAPMIN")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TAPMIN")) : null;
                    ob.Tapcnt = !dr.IsDBNull(dr.GetOrdinal("TAPCNT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TAPCNT")) : null;
                    ob.Tapmax = !dr.IsDBNull(dr.GetOrdinal("TAPMAX")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TAPMAX")) : null;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
                    ob.AreaDemanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetString(dr.GetOrdinal("AREADEMANDA")) : "";
                    itcprm2DTOs.Add(ob);
                }
            }

            return itcprm2DTOs;
        }

    }
}
