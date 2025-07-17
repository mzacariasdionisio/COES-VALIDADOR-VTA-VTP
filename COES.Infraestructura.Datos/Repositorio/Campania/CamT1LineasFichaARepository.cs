using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamT1LineasFichaARepository : RepositoryBase, ICamT1LineasFichaARepository
    {
        public CamT1LineasFichaARepository(string strConn) : base(strConn) { }

        CamT1LinFichaAHelper Helper = new CamT1LinFichaAHelper();

        public List<T1LinFichaADTO> GetLineasFichaACodi(int proyCodi)
        {
            List<T1LinFichaADTO> T1LinFichaADTOs = new List<T1LinFichaADTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLinfichaA);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    T1LinFichaADTO ob = new T1LinFichaADTO
                    {
                        LinFichaACodi = !dr.IsDBNull(dr.GetOrdinal("LINFICHAACODI")) ? dr.GetInt32(dr.GetOrdinal("LINFICHAACODI")) : default(int),
                        ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : default(int),
                        NombreLinea = !dr.IsDBNull(dr.GetOrdinal("NOMBRELINEA")) ? dr.GetString(dr.GetOrdinal("NOMBRELINEA")) : null,
                        FecPuestaServ = !dr.IsDBNull(dr.GetOrdinal("FECPUESTASERV")) ? dr.GetString(dr.GetOrdinal("FECPUESTASERV")) : null,
                        SubInicio = !dr.IsDBNull(dr.GetOrdinal("SUBINICIO")) ? dr.GetString(dr.GetOrdinal("SUBINICIO")) : null,
                        OtroSubInicio = !dr.IsDBNull(dr.GetOrdinal("OTROSUBINICIO")) ? dr.GetString(dr.GetOrdinal("OTROSUBINICIO")) : null,
                        SubFin = !dr.IsDBNull(dr.GetOrdinal("SUBFIN")) ? dr.GetString(dr.GetOrdinal("SUBFIN")) : null,
                        OtroSubFin = !dr.IsDBNull(dr.GetOrdinal("OTROSUBFIN")) ? dr.GetString(dr.GetOrdinal("OTROSUBFIN")) : null,
                        EmpPropietaria = !dr.IsDBNull(dr.GetOrdinal("EMPPROPIETARIA")) ? dr.GetString(dr.GetOrdinal("EMPPROPIETARIA")) : null,
                        NivTension = !dr.IsDBNull(dr.GetOrdinal("NIVTENSION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NIVTENSION")) : default(decimal),
                        CapCorriente = !dr.IsDBNull(dr.GetOrdinal("CAPCORRIENTE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPCORRIENTE")) : default(decimal),
                        CapCorrienteA = !dr.IsDBNull(dr.GetOrdinal("CAPCORRIENTEA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPCORRIENTEA")) : default(decimal),
                        TpoSobreCarga = !dr.IsDBNull(dr.GetOrdinal("TPOSOBRECARGA")) ? (int?)dr.GetInt32(dr.GetOrdinal("TPOSOBRECARGA")) : null,
                        NumTemas = !dr.IsDBNull(dr.GetOrdinal("NUMTEMAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NUMTEMAS")) : null,
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
                    T1LinFichaADTOs.Add(ob);
                }
            }
            return T1LinFichaADTOs;
        }
        public bool SaveLineasFichaA(T1LinFichaADTO T1LinFichaADTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveLinfichaA);

            dbProvider.AddInParameter(dbCommand, "LINFICHAACODI", DbType.Int32, T1LinFichaADTO.LinFichaACodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, T1LinFichaADTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "NOMBRELINEA", DbType.String, T1LinFichaADTO.NombreLinea);
            dbProvider.AddInParameter(dbCommand, "FECPUESTASERV", DbType.String, T1LinFichaADTO.FecPuestaServ);
            dbProvider.AddInParameter(dbCommand, "SUBINICIO", DbType.String, T1LinFichaADTO.SubInicio);
            dbProvider.AddInParameter(dbCommand, "OTROSUBINICIO", DbType.String, T1LinFichaADTO.OtroSubInicio);
            dbProvider.AddInParameter(dbCommand, "SUBFIN", DbType.String, T1LinFichaADTO.SubFin);
            dbProvider.AddInParameter(dbCommand, "OTROSUBFIN", DbType.String, T1LinFichaADTO.OtroSubFin);
            dbProvider.AddInParameter(dbCommand, "EMPPROPIETARIA", DbType.String, T1LinFichaADTO.EmpPropietaria);
            dbProvider.AddInParameter(dbCommand, "NIVTENSION", DbType.Decimal, T1LinFichaADTO.NivTension);
            dbProvider.AddInParameter(dbCommand, "CAPCORRIENTE", DbType.Decimal, T1LinFichaADTO.CapCorriente);
            dbProvider.AddInParameter(dbCommand, "CAPCORRIENTEA", DbType.Decimal, T1LinFichaADTO.CapCorrienteA);
            dbProvider.AddInParameter(dbCommand, "TPOSOBRECARGA", DbType.Int32, (object)T1LinFichaADTO.TpoSobreCarga ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "NUMTEMAS", DbType.Decimal, T1LinFichaADTO.NumTemas);
            dbProvider.AddInParameter(dbCommand, "LONGTOTAL", DbType.Decimal, T1LinFichaADTO.LongTotal);
            dbProvider.AddInParameter(dbCommand, "LONGVANOPROMEDIO", DbType.Decimal, T1LinFichaADTO.LongVanoPromedio);
            dbProvider.AddInParameter(dbCommand, "TIPMATSOP", DbType.String, T1LinFichaADTO.TipMatSop);
            dbProvider.AddInParameter(dbCommand, "DESPROTECPRINCIPAL", DbType.String, T1LinFichaADTO.DesProtecPrincipal);
            dbProvider.AddInParameter(dbCommand, "DESPROTECRESPALDO", DbType.String, T1LinFichaADTO.DesProtecRespaldo);
            dbProvider.AddInParameter(dbCommand, "DESGENPROYECTO", DbType.String, T1LinFichaADTO.DesGenProyecto);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, T1LinFichaADTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, T1LinFichaADTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }
        public bool DeleteLineasFichaAById(int LinFichaACodi, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteLinfichaAById);
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
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastLinfichaAId);
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
        public T1LinFichaADTO GetLineasFichaAById(int LinFichaACodi)
        {
            T1LinFichaADTO ob = new T1LinFichaADTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLinfichaAById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, LinFichaACodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ob = new T1LinFichaADTO
                    {
                        LinFichaACodi = !dr.IsDBNull(dr.GetOrdinal("LINFICHAACODI")) ? dr.GetInt32(dr.GetOrdinal("LINFICHAACODI")) : default(int),
                        ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : default(int),
                        NombreLinea = !dr.IsDBNull(dr.GetOrdinal("NOMBRELINEA")) ? dr.GetString(dr.GetOrdinal("NOMBRELINEA")) : null,
                        FecPuestaServ = !dr.IsDBNull(dr.GetOrdinal("FECPUESTASERV")) ? dr.GetString(dr.GetOrdinal("FECPUESTASERV")) : null,
                        SubInicio = !dr.IsDBNull(dr.GetOrdinal("SUBINICIO")) ? dr.GetString(dr.GetOrdinal("SUBINICIO")) : null,
                        OtroSubInicio = !dr.IsDBNull(dr.GetOrdinal("OTROSUBINICIO")) ? dr.GetString(dr.GetOrdinal("OTROSUBINICIO")) : null,
                        SubFin = !dr.IsDBNull(dr.GetOrdinal("SUBFIN")) ? dr.GetString(dr.GetOrdinal("SUBFIN")) : null,
                        OtroSubFin = !dr.IsDBNull(dr.GetOrdinal("OTROSUBFIN")) ? dr.GetString(dr.GetOrdinal("OTROSUBFIN")) : null,
                        EmpPropietaria = !dr.IsDBNull(dr.GetOrdinal("EMPPROPIETARIA")) ? dr.GetString(dr.GetOrdinal("EMPPROPIETARIA")) : null,
                        NivTension = !dr.IsDBNull(dr.GetOrdinal("NIVTENSION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NIVTENSION")) : (decimal?)null,
                        CapCorriente = !dr.IsDBNull(dr.GetOrdinal("CAPCORRIENTE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPCORRIENTE")) : (decimal?)null,
                        CapCorrienteA = !dr.IsDBNull(dr.GetOrdinal("CAPCORRIENTEA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPCORRIENTEA")) : (decimal?)null,
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
    }
}
