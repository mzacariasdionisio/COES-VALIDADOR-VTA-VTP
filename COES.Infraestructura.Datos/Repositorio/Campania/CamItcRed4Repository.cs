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
    public class CamItcRed4Repository : RepositoryBase, ICamItcRed4Repository
    {
        public CamItcRed4Repository(string strConn) : base(strConn) { }

        CamItcRed4Helper Helper = new CamItcRed4Helper();

        public List<ItcRed4Dto> GetItcRed4ByProyCodi(int proyCodi)
        {
            List<ItcRed4Dto> itcred4DTOs = new List<ItcRed4Dto>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamItcRed4ByProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcRed4Dto ob = new ItcRed4Dto();
                    ob.ItcRed4Codi = !dr.IsDBNull(dr.GetOrdinal("ITCRED4CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCRED4CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.IdCmp = !dr.IsDBNull(dr.GetOrdinal("IDCMP")) ? dr.GetString(dr.GetOrdinal("IDCMP")) : string.Empty;
                    ob.Barra = !dr.IsDBNull(dr.GetOrdinal("BARRA")) ? dr.GetString(dr.GetOrdinal("BARRA")) : string.Empty;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.Vnkv = !dr.IsDBNull(dr.GetOrdinal("VNKV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VNKV")) : null;
                    ob.CapmVar = !dr.IsDBNull(dr.GetOrdinal("CAPMVAR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPMVAR")) : null;
                    ob.Npasos = !dr.IsDBNull(dr.GetOrdinal("NPASOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NPASOS")) : null;
                    ob.PasoAct = !dr.IsDBNull(dr.GetOrdinal("PASOACT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PASOACT")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : string.Empty;
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;

                    itcred4DTOs.Add(ob);
                }
            }

            return itcred4DTOs;
        }

        public bool SaveItcRed4(ItcRed4Dto itcred4DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCamItcRed4);
            dbProvider.AddInParameter(dbCommand, "ITCRED4CODI", DbType.Int32, itcred4DTO.ItcRed4Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcred4DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "IDCMP", DbType.String, itcred4DTO.IdCmp);
            dbProvider.AddInParameter(dbCommand, "BARRA", DbType.String, itcred4DTO.Barra);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, itcred4DTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "VNKV", DbType.Decimal, itcred4DTO.Vnkv);
            dbProvider.AddInParameter(dbCommand, "CAPMVAR", DbType.Decimal, itcred4DTO.CapmVar);
            dbProvider.AddInParameter(dbCommand, "NPASOS", DbType.Int32, itcred4DTO.Npasos);
            dbProvider.AddInParameter(dbCommand, "PASOACT", DbType.Int32, itcred4DTO.PasoAct);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, itcred4DTO.FecCreacion);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcRed4ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCamItcRed4ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcRed4Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCamItcRed4Id);
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

        public List<ItcRed4Dto> GetItcRed4ById(int id)
        {
            List<ItcRed4Dto> itcred4DTOs = new List<ItcRed4Dto>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamItcRed4ById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcRed4Dto ob = new ItcRed4Dto();
                    ob.ItcRed4Codi = !dr.IsDBNull(dr.GetOrdinal("ITCRED4CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCRED4CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.IdCmp = !dr.IsDBNull(dr.GetOrdinal("IDCMP")) ? dr.GetString(dr.GetOrdinal("IDCMP")) : string.Empty;
                    ob.Barra = !dr.IsDBNull(dr.GetOrdinal("BARRA")) ? dr.GetString(dr.GetOrdinal("BARRA")) : string.Empty;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.Vnkv = !dr.IsDBNull(dr.GetOrdinal("VNKV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VNKV")) : null;
                    ob.CapmVar = !dr.IsDBNull(dr.GetOrdinal("CAPMVAR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPMVAR")) : null;
                    ob.Npasos = !dr.IsDBNull(dr.GetOrdinal("NPASOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NPASOS")) : null;
                    ob.PasoAct = !dr.IsDBNull(dr.GetOrdinal("PASOACT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PASOACT")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : string.Empty;
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    itcred4DTOs.Add(ob);
                }
            }

            return itcred4DTOs;
        }

        public List<ItcRed4Dto> GetItcRed4ByFilter(string plancodi, string empresa, string estado)
        {
            List<ItcRed4Dto> itcred4DTOs = new List<ItcRed4Dto>();
            string query = $@"
                SELECT CGB.*, TR.AREADEMANDA, TR.EMPRESANOM   FROM CAM_ITCRED4 CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.ITCRED4CODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ItcRed4Dto ob = new ItcRed4Dto();
                    ob.ItcRed4Codi = !dr.IsDBNull(dr.GetOrdinal("ITCRED4CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCRED4CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.IdCmp = !dr.IsDBNull(dr.GetOrdinal("IDCMP")) ? dr.GetString(dr.GetOrdinal("IDCMP")) : string.Empty;
                    ob.Barra = !dr.IsDBNull(dr.GetOrdinal("BARRA")) ? dr.GetString(dr.GetOrdinal("BARRA")) : string.Empty;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.Vnkv = !dr.IsDBNull(dr.GetOrdinal("VNKV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VNKV")) : null;
                    ob.CapmVar = !dr.IsDBNull(dr.GetOrdinal("CAPMVAR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPMVAR")) : null;
                    ob.Npasos = !dr.IsDBNull(dr.GetOrdinal("NPASOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NPASOS")) : null;
                    ob.PasoAct = !dr.IsDBNull(dr.GetOrdinal("PASOACT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PASOACT")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : string.Empty;
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
                    ob.AreaDemanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetString(dr.GetOrdinal("AREADEMANDA")) : "";
                    itcred4DTOs.Add(ob);
                }
            }

            return itcred4DTOs;
        }

        public bool UpdateItcRed4(ItcRed4Dto itcred4DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCamItcRed4);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcred4DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "IDCMP", DbType.String, itcred4DTO.IdCmp);
            dbProvider.AddInParameter(dbCommand, "BARRA", DbType.String, itcred4DTO.Barra);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, itcred4DTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "VNKV", DbType.Decimal, itcred4DTO.Vnkv);
            dbProvider.AddInParameter(dbCommand, "CAPMVAR", DbType.Decimal, itcred4DTO.CapmVar);
            dbProvider.AddInParameter(dbCommand, "NPASOS", DbType.Int32, itcred4DTO.Npasos);
            dbProvider.AddInParameter(dbCommand, "PASOACT", DbType.Int32, itcred4DTO.PasoAct);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcred4DTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcred4DTO.FecModificacion);
            dbProvider.AddInParameter(dbCommand, "ITCRED4CODI", DbType.Int32, itcred4DTO.ItcRed4Codi);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }
    }
}
