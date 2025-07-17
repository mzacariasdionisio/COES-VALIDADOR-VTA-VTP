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
    public class CamLineasFichaARepository : RepositoryBase, ICamLineasFichaARepository
    {
        public CamLineasFichaARepository(string strConn) : base(strConn) { }

        CamLineasFichaAHelper Helper = new CamLineasFichaAHelper();
            
        public List<LineasFichaADTO> GetLineasFichaACodi(int proyCodi)
        {
            List<LineasFichaADTO> lineasFichaADTOs = new List<LineasFichaADTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLineasFichaA);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    LineasFichaADTO ob = new LineasFichaADTO
                    {
                        LinFichaACodi = !dr.IsDBNull(dr.GetOrdinal("FICHAACODI")) ? dr.GetInt32(dr.GetOrdinal("FICHAACODI")) : default(int),
                        ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : default(int),
                        NombreLinea = !dr.IsDBNull(dr.GetOrdinal("NOMBRELINEA")) ? dr.GetString(dr.GetOrdinal("NOMBRELINEA")) : null,
                        FecPuestaServ = !dr.IsDBNull(dr.GetOrdinal("FECPUESTASERV")) ? dr.GetString(dr.GetOrdinal("FECPUESTASERV")) : null,
                        SubInicio = !dr.IsDBNull(dr.GetOrdinal("SUBINICIO")) ? dr.GetString(dr.GetOrdinal("SUBINICIO")) : null,
                        OtroSubInicio = !dr.IsDBNull(dr.GetOrdinal("OTROSUBINICIO")) ? dr.GetString(dr.GetOrdinal("OTROSUBINICIO")) : null,
                        SubFin = !dr.IsDBNull(dr.GetOrdinal("SUBFIN")) ? dr.GetString(dr.GetOrdinal("SUBFIN")) : null,
                        OtroSubFin = !dr.IsDBNull(dr.GetOrdinal("OTROSUBFIN")) ? dr.GetString(dr.GetOrdinal("OTROSUBFIN")) : null,
                        EmpPropietaria = !dr.IsDBNull(dr.GetOrdinal("EMPPROPIETARIA")) ? dr.GetString(dr.GetOrdinal("EMPPROPIETARIA")) : null,
                        NivTension = !dr.IsDBNull(dr.GetOrdinal("NIVTENSION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NIVTENSION")) : null,
                        CapCorriente = !dr.IsDBNull(dr.GetOrdinal("CAPCORRIENTE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPCORRIENTE")) : null,
                        CapCorrienteA = !dr.IsDBNull(dr.GetOrdinal("CAPCORRIENTEA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPCORRIENTEA")) : null,
                        TpoSobreCarga = !dr.IsDBNull(dr.GetOrdinal("TPOSOBRECARGA")) ? (int?)dr.GetInt32(dr.GetOrdinal("TPOSOBRECARGA")) : null,                        
                        NumTemas = !dr.IsDBNull(dr.GetOrdinal("NUMTEMAS")) ? (decimal?)dr.GetInt32(dr.GetOrdinal("NUMTEMAS")) : null,
                        LongTotal = !dr.IsDBNull(dr.GetOrdinal("LONGTOTAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGTOTAL")) : null,
                        LongVanoPromedio = !dr.IsDBNull(dr.GetOrdinal("LONGVANOPROMEDIO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGVANOPROMEDIO")) : null,
                        TipMatSop = !dr.IsDBNull(dr.GetOrdinal("TIPMATSOP")) ? dr.GetString(dr.GetOrdinal("TIPMATSOP")) : null,
                        DesProtecPrincipal = !dr.IsDBNull(dr.GetOrdinal("DESPROTECPRINCIPAL")) ? dr.GetString(dr.GetOrdinal("DESPROTECPRINCIPAL")) : null,
                        DesProtecRespaldo = !dr.IsDBNull(dr.GetOrdinal("DESPROTECRESPALDO")) ? dr.GetString(dr.GetOrdinal("DESPROTECRESPALDO")) : null,
                        DesGenProyecto = !dr.IsDBNull(dr.GetOrdinal("DESGENPROYECTO")) ? dr.GetString(dr.GetOrdinal("DESGENPROYECTO")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : default(DateTime),
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : default(DateTime),
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null

                    };
                    lineasFichaADTOs.Add(ob);
                }
            }
            return lineasFichaADTOs;
        }
        public bool SaveLineasFichaA(LineasFichaADTO lineasFichaADTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveLineasFichaA);

            dbProvider.AddInParameter(dbCommand, "FICHAACODI", DbType.Int32, lineasFichaADTO.LinFichaACodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, lineasFichaADTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "NOMBRELINEA", DbType.String, lineasFichaADTO.NombreLinea);
            dbProvider.AddInParameter(dbCommand, "FECPUESTASERV", DbType.String, lineasFichaADTO.FecPuestaServ);
            dbProvider.AddInParameter(dbCommand, "SUBINICIO", DbType.String, lineasFichaADTO.SubInicio);
            dbProvider.AddInParameter(dbCommand, "OTROSUBINICIO", DbType.String, lineasFichaADTO.OtroSubInicio);
            dbProvider.AddInParameter(dbCommand, "SUBFIN", DbType.String, lineasFichaADTO.SubFin);
            dbProvider.AddInParameter(dbCommand, "OTROSUBFIN", DbType.String, lineasFichaADTO.OtroSubFin);
            dbProvider.AddInParameter(dbCommand, "EMPPROPIETARIA", DbType.String, lineasFichaADTO.EmpPropietaria);
            dbProvider.AddInParameter(dbCommand, "NIVTENSION", DbType.Decimal, lineasFichaADTO.NivTension);
            dbProvider.AddInParameter(dbCommand, "CAPCORRIENTE", DbType.Decimal, lineasFichaADTO.CapCorriente);
            dbProvider.AddInParameter(dbCommand, "CAPCORRIENTEA", DbType.Decimal, lineasFichaADTO.CapCorrienteA);
            dbProvider.AddInParameter(dbCommand, "TPOSOBRECARGA", DbType.Int32, (object)lineasFichaADTO.TpoSobreCarga ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "NUMTEMAS", DbType.Decimal, lineasFichaADTO.NumTemas);
            dbProvider.AddInParameter(dbCommand, "LONGTOTAL", DbType.Decimal, lineasFichaADTO.LongTotal);
            dbProvider.AddInParameter(dbCommand, "LONGVANOPROMEDIO", DbType.Decimal, lineasFichaADTO.LongVanoPromedio);
            dbProvider.AddInParameter(dbCommand, "TIPMATSOP", DbType.String, lineasFichaADTO.TipMatSop);
            dbProvider.AddInParameter(dbCommand, "DESPROTECPRINCIPAL", DbType.String, lineasFichaADTO.DesProtecPrincipal);
            dbProvider.AddInParameter(dbCommand, "DESPROTECRESPALDO", DbType.String, lineasFichaADTO.DesProtecRespaldo);
            dbProvider.AddInParameter(dbCommand, "DESGENPROYECTO", DbType.String, lineasFichaADTO.DesGenProyecto);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, lineasFichaADTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, lineasFichaADTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }
        public bool DeleteLineasFichaAById(int LinFichaACodi, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteLineasFichaAById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, LinFichaACodi);

            dbProvider.ExecuteNonQuery(command);
            return true;
        }
        public int GetLastLineasFichaAId()
        {
            int lastId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastLineasFichaAId);
            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                lastId = Convert.ToInt32(result) + 1;
            }
            else
            {
                lastId = 1;
            }

            return lastId;
        }
        public LineasFichaADTO GetLineasFichaAById(int LinFichaACodi)
        {
            LineasFichaADTO ob = new LineasFichaADTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLineasFichaAById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, LinFichaACodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ob = new LineasFichaADTO
                    {
                        LinFichaACodi = !dr.IsDBNull(dr.GetOrdinal("FICHAACODI")) ? dr.GetInt32(dr.GetOrdinal("FICHAACODI")) : default(int),
                        ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : default(int),
                        NombreLinea = !dr.IsDBNull(dr.GetOrdinal("NOMBRELINEA")) ? dr.GetString(dr.GetOrdinal("NOMBRELINEA")) : null,
                        FecPuestaServ = !dr.IsDBNull(dr.GetOrdinal("FECPUESTASERV")) ? dr.GetString(dr.GetOrdinal("FECPUESTASERV")) : null,
                        SubInicio = !dr.IsDBNull(dr.GetOrdinal("SUBINICIO")) ? dr.GetString(dr.GetOrdinal("SUBINICIO")) : null,
                        OtroSubInicio = !dr.IsDBNull(dr.GetOrdinal("OTROSUBINICIO")) ? dr.GetString(dr.GetOrdinal("OTROSUBINICIO")) : null,
                        SubFin = !dr.IsDBNull(dr.GetOrdinal("SUBFIN")) ? dr.GetString(dr.GetOrdinal("SUBFIN")) : null,
                        OtroSubFin = !dr.IsDBNull(dr.GetOrdinal("OTROSUBFIN")) ? dr.GetString(dr.GetOrdinal("OTROSUBFIN")) : null,
                        EmpPropietaria = !dr.IsDBNull(dr.GetOrdinal("EMPPROPIETARIA")) ? dr.GetString(dr.GetOrdinal("EMPPROPIETARIA")) : null,
                        NivTension = !dr.IsDBNull(dr.GetOrdinal("NIVTENSION")) ? dr.GetDecimal(dr.GetOrdinal("NIVTENSION")) : (decimal?)null,
                        CapCorriente = !dr.IsDBNull(dr.GetOrdinal("CAPCORRIENTE")) ? dr.GetDecimal(dr.GetOrdinal("CAPCORRIENTE")) : (decimal?)null,
                        CapCorrienteA = !dr.IsDBNull(dr.GetOrdinal("CAPCORRIENTEA")) ? dr.GetDecimal(dr.GetOrdinal("CAPCORRIENTEA")) : (decimal?)null,
                        TpoSobreCarga = !dr.IsDBNull(dr.GetOrdinal("TPOSOBRECARGA")) ? (int?)dr.GetInt32(dr.GetOrdinal("TPOSOBRECARGA")) : null,
                        NumTemas = !dr.IsDBNull(dr.GetOrdinal("NUMTEMAS")) ? (decimal?)dr.GetInt32(dr.GetOrdinal("NUMTEMAS")) : null,
                        LongTotal = !dr.IsDBNull(dr.GetOrdinal("LONGTOTAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGTOTAL")) : null,
                        LongVanoPromedio = !dr.IsDBNull(dr.GetOrdinal("LONGVANOPROMEDIO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGVANOPROMEDIO")) : null,
                        TipMatSop = !dr.IsDBNull(dr.GetOrdinal("TIPMATSOP")) ? dr.GetString(dr.GetOrdinal("TIPMATSOP")) : null,
                        DesProtecPrincipal = !dr.IsDBNull(dr.GetOrdinal("DESPROTECPRINCIPAL")) ? dr.GetString(dr.GetOrdinal("DESPROTECPRINCIPAL")) : null,
                        DesProtecRespaldo = !dr.IsDBNull(dr.GetOrdinal("DESPROTECRESPALDO")) ? dr.GetString(dr.GetOrdinal("DESPROTECRESPALDO")) : null,
                        DesGenProyecto = !dr.IsDBNull(dr.GetOrdinal("DESGENPROYECTO")) ? dr.GetString(dr.GetOrdinal("DESGENPROYECTO")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : default(DateTime),
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : default(DateTime),
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null

                    };
                }
            }
            return ob;
        }

        public List<LineasFichaADTO> GetLineasFichaAByFilter(string plancodi, string empresa, string estado)
        {
            List<LineasFichaADTO> oblist = new List<LineasFichaADTO>();
            string query = $@"
                SELECT LA.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  FROM CAM_LINEASFICHAA LA
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = LA.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                LA.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, LA.PROYCODI,PL.CODEMPRESA, LA.FICHAACODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    LineasFichaADTO ob = new LineasFichaADTO();
                    ob.LinFichaACodi = !dr.IsDBNull(dr.GetOrdinal("FICHAACODI")) ? dr.GetInt32(dr.GetOrdinal("FICHAACODI")) : default(int);
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : default(int);
                    ob.NombreLinea = !dr.IsDBNull(dr.GetOrdinal("NOMBRELINEA")) ? dr.GetString(dr.GetOrdinal("NOMBRELINEA")) : null;
                    ob.FecPuestaServ = !dr.IsDBNull(dr.GetOrdinal("FECPUESTASERV")) ? dr.GetString(dr.GetOrdinal("FECPUESTASERV")) : null;
                    ob.SubInicio = !dr.IsDBNull(dr.GetOrdinal("SUBINICIO")) ? dr.GetString(dr.GetOrdinal("SUBINICIO")) : null;
                    ob.OtroSubInicio = !dr.IsDBNull(dr.GetOrdinal("OTROSUBINICIO")) ? dr.GetString(dr.GetOrdinal("OTROSUBINICIO")) : null;
                    ob.SubFin = !dr.IsDBNull(dr.GetOrdinal("SUBFIN")) ? dr.GetString(dr.GetOrdinal("SUBFIN")) : null;
                    ob.OtroSubFin = !dr.IsDBNull(dr.GetOrdinal("OTROSUBFIN")) ? dr.GetString(dr.GetOrdinal("OTROSUBFIN")) : null;
                    ob.EmpPropietaria = !dr.IsDBNull(dr.GetOrdinal("EMPPROPIETARIA")) ? dr.GetString(dr.GetOrdinal("EMPPROPIETARIA")) : null;
                    ob.NivTension = !dr.IsDBNull(dr.GetOrdinal("NIVTENSION")) ? dr.GetDecimal(dr.GetOrdinal("NIVTENSION")) : (decimal?)null;
                    ob.CapCorriente = !dr.IsDBNull(dr.GetOrdinal("CAPCORRIENTE")) ? dr.GetDecimal(dr.GetOrdinal("CAPCORRIENTE")) : (decimal?)null;
                    ob.CapCorrienteA = !dr.IsDBNull(dr.GetOrdinal("CAPCORRIENTEA")) ? dr.GetDecimal(dr.GetOrdinal("CAPCORRIENTEA")) : (decimal?)null;
                    ob.TpoSobreCarga = !dr.IsDBNull(dr.GetOrdinal("TPOSOBRECARGA")) ? (int?)dr.GetInt32(dr.GetOrdinal("TPOSOBRECARGA")) : null;
                    ob.NumTemas = !dr.IsDBNull(dr.GetOrdinal("NUMTEMAS")) ? (decimal?)dr.GetInt32(dr.GetOrdinal("NUMTEMAS")) : null;
                    ob.LongTotal = !dr.IsDBNull(dr.GetOrdinal("LONGTOTAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGTOTAL")) : null;
                    ob.LongVanoPromedio = !dr.IsDBNull(dr.GetOrdinal("LONGVANOPROMEDIO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGVANOPROMEDIO")) : null;
                    ob.TipMatSop = !dr.IsDBNull(dr.GetOrdinal("TIPMATSOP")) ? dr.GetString(dr.GetOrdinal("TIPMATSOP")) : null;
                    ob.DesProtecPrincipal = !dr.IsDBNull(dr.GetOrdinal("DESPROTECPRINCIPAL")) ? dr.GetString(dr.GetOrdinal("DESPROTECPRINCIPAL")) : null;
                    ob.DesProtecRespaldo = !dr.IsDBNull(dr.GetOrdinal("DESPROTECRESPALDO")) ? dr.GetString(dr.GetOrdinal("DESPROTECRESPALDO")) : null;
                    ob.DesGenProyecto = !dr.IsDBNull(dr.GetOrdinal("DESGENPROYECTO")) ? dr.GetString(dr.GetOrdinal("DESGENPROYECTO")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : default(DateTime);
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null;
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : default(DateTime);
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.NombreProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;
                    ob.DetalleProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? dr.GetString(dr.GetOrdinal("PROYDESCRIPCION")) : string.Empty;
                    ob.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPONOMBRE")) : string.Empty;
                    ob.SubTipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOFINOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPOFINOMBRE")) : string.Empty;
                    ob.Condifencial = dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? null : dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL"));
                    oblist.Add(ob);
                }
            }
            return oblist;
        }

        public List<LineasFichaATramoDTO> GetLineasFichaATramoByFilter(string plancodi, string empresa, string estado)
        {
            List<LineasFichaATramoDTO> oblist = new List<LineasFichaATramoDTO>();
            string query = $@"
                SELECT LA.FICHAACODI, LA.PROYCODI, LA1.TRAMO AS TRAMOFINAL,LA1.*, LA2.*, LA.NOMBRELINEA, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION,  TR.PROYCONFIDENCIAL,TR.PERICODI,PL.CODEMPRESA
                FROM CAM_LINEASFICHAA LA
                LEFT JOIN CAM_LINEASFICHAADET1 LA1 
                    ON LA.FICHAACODI = LA1.FICHAACODI
                LEFT JOIN CAM_LINEASFICHAADET2 LA2 
                    ON LA.FICHAACODI = LA2.FICHAACODI 
                    AND LA1.TRAMO = LA2.TRAMO  
                INNER JOIN CAM_TRNSMPROYECTO TR 
                    ON TR.PROYCODI = LA.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL
                    ON PL.PLANCODI = TR.PLANCODI
                WHERE 
                    TR.PERICODI IN ({plancodi}) 
                    AND PL.CODEMPRESA IN ({empresa})  
                    AND LA.IND_DEL = 0  
                    AND PL.PLANESTADO = '{estado}' 
                    AND LA1.TRAMO IS NOT NULL 
                UNION
                SELECT LA.FICHAACODI, LA.PROYCODI, LA2.TRAMO AS TRAMOFINAL,LA1.*, LA2.*, LA.NOMBRELINEA, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION,  TR.PROYCONFIDENCIAL,TR.PERICODI,PL.CODEMPRESA 
                FROM CAM_LINEASFICHAA LA
                LEFT JOIN CAM_LINEASFICHAADET2 LA2 
                    ON LA.FICHAACODI = LA2.FICHAACODI
                LEFT JOIN CAM_LINEASFICHAADET1 LA1 
                    ON LA.FICHAACODI = LA1.FICHAACODI 
                    AND LA2.TRAMO = LA1.TRAMO
                INNER JOIN CAM_TRNSMPROYECTO TR 
                    ON TR.PROYCODI = LA.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL 
                    ON PL.PLANCODI = TR.PLANCODI
                WHERE 
                    TR.PERICODI IN ({plancodi}) 
                    AND PL.CODEMPRESA IN ({empresa})  
                    AND LA.IND_DEL = 0  
                    AND PL.PLANESTADO = '{estado}'
                    AND LA2.TRAMO IS NOT NULL 
                ORDER BY 
                    PERICODI, 
                    PROYCODI, 
                    CODEMPRESA, 
                    TRAMOFINAL ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    LineasFichaATramoDTO ob = new LineasFichaATramoDTO();
                    ob.LinFichaACodi = !dr.IsDBNull(dr.GetOrdinal("FICHAACODI")) ? dr.GetInt32(dr.GetOrdinal("FICHAACODI")) : default(int);
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : default(int);
                    ob.NombreLinea = !dr.IsDBNull(dr.GetOrdinal("NOMBRELINEA")) ? dr.GetString(dr.GetOrdinal("NOMBRELINEA")) : null;
                    ob.LinFichaADet1Codi = !dr.IsDBNull(dr.GetOrdinal("FICHAADET1CODI")) ? dr.GetInt32(dr.GetOrdinal("FICHAADET1CODI")) : default(int);
                    ob.Tramo = dr.IsDBNull(dr.GetOrdinal("TRAMOFINAL")) ? (int?)null : dr.GetInt32(dr.GetOrdinal("TRAMOFINAL"));
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : null;
                    ob.Longitud = !dr.IsDBNull(dr.GetOrdinal("LONGITUD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGITUD")) : null;
                    ob.MatConductor = !dr.IsDBNull(dr.GetOrdinal("MATCONDUCTOR")) ? dr.GetString(dr.GetOrdinal("MATCONDUCTOR")) : null;
                    ob.SecConductor = !dr.IsDBNull(dr.GetOrdinal("SECCONDUCTOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SECCONDUCTOR")) : null;
                    ob.ConductorFase = !dr.IsDBNull(dr.GetOrdinal("CONDUCTORFASE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CONDUCTORFASE")) : null;
                    ob.CapacidadTot = !dr.IsDBNull(dr.GetOrdinal("CAPACIDADTOT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPACIDADTOT")) : null;
                    ob.CabGuarda = !dr.IsDBNull(dr.GetOrdinal("CABGUARDA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CABGUARDA")) : null;
                    ob.ResistCabGuarda = !dr.IsDBNull(dr.GetOrdinal("RESISTCABGUARDA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("RESISTCABGUARDA")) : null;
                    ob.LinFichaADet2Codi = !dr.IsDBNull(dr.GetOrdinal("FICHAADET2CODI")) ? dr.GetInt32(dr.GetOrdinal("FICHAADET2CODI")) : default(int);
                    ob.R = !dr.IsDBNull(dr.GetOrdinal("R")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("R")) : null;
                    ob.X = !dr.IsDBNull(dr.GetOrdinal("X")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("X")) : null;
                    ob.B = !dr.IsDBNull(dr.GetOrdinal("B")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("B")) : null;
                    ob.G = !dr.IsDBNull(dr.GetOrdinal("G")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("G")) : null;
                    ob.R0 = !dr.IsDBNull(dr.GetOrdinal("R0")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("R0")) : null;
                    ob.X0 = !dr.IsDBNull(dr.GetOrdinal("X0")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("X0")) : null;
                    ob.B0 = !dr.IsDBNull(dr.GetOrdinal("B0")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("B0")) : null;
                    ob.G0 = !dr.IsDBNull(dr.GetOrdinal("G0")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("G0")) : null;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.NombreProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;
                    ob.DetalleProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? dr.GetString(dr.GetOrdinal("PROYDESCRIPCION")) : string.Empty;
                    ob.Condifencial = dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? null : dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL"));
                    oblist.Add(ob);
                }
            }
            return oblist;
        }
    }
}
