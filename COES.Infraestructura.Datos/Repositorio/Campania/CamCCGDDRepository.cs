using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamCCGDDRepository : RepositoryBase, ICamCCGDDRepository
    {
        public CamCCGDDRepository(string strConn) : base(strConn) { }

        CamCCGDDHelper Helper = new CamCCGDDHelper();

        public List<CCGDDDTO> GetCamCCGDD(int proyCodi)
        {
            List<CCGDDDTO> ccgddDTOs = new List<CCGDDDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamCCGDD);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CCGDDDTO ob = new CCGDDDTO();
                    ob.CcgddCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDDCODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDDCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Hora = !dr.IsDBNull(dr.GetOrdinal("HORA")) ? dr.GetString(dr.GetOrdinal("HORA")) : "";
                    ob.Demanda = !dr.IsDBNull(dr.GetOrdinal("DEMANDA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDA")) : null;
                    ob.Generacion = !dr.IsDBNull(dr.GetOrdinal("GENERACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACION")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ccgddDTOs.Add(ob);
                }
            }
            return ccgddDTOs;
        }

        public bool SaveCamCCGDD(CCGDDDTO ccgddDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCamCCGDD);
            dbProvider.AddInParameter(dbCommand, "CCGDDCODI", DbType.Int32, ccgddDTO.CcgddCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ccgddDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "HORA", DbType.String, ccgddDTO.Hora);
            dbProvider.AddInParameter(dbCommand, "DEMANDA", DbType.Decimal, ccgddDTO.Demanda);
            dbProvider.AddInParameter(dbCommand, "GENERACION", DbType.Decimal, ccgddDTO.Generacion);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ccgddDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, ccgddDTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteCamCCGDDById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCamCCGDDById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastCamCCGDDCodi()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCamCCGDDCodi);
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

        public List<CCGDDDTO> GetCamCCGDDById(int id)
        {
            List<CCGDDDTO> list = new List<CCGDDDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamCCGDDById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CCGDDDTO ob = new CCGDDDTO();
                    ob.CcgddCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDDCODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDDCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Hora = !dr.IsDBNull(dr.GetOrdinal("HORA")) ? dr.GetString(dr.GetOrdinal("HORA")) : "";
                    ob.Demanda = !dr.IsDBNull(dr.GetOrdinal("DEMANDA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDA")) : null;
                    ob.Generacion = !dr.IsDBNull(dr.GetOrdinal("GENERACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACION")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    list.Add(ob);
                }
            }
            return list;
        }

        public bool UpdateCamCCGDD(CCGDDDTO ccgddDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCamCCGDD);
            dbProvider.AddInParameter(dbCommand, "CCGDDCODI", DbType.Int32, ccgddDTO.CcgddCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ccgddDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "HORA", DbType.String, ccgddDTO.Hora);
            dbProvider.AddInParameter(dbCommand, "DEMANDA", DbType.Decimal, ccgddDTO.Demanda);
            dbProvider.AddInParameter(dbCommand, "GENERACION", DbType.Decimal, ccgddDTO.Generacion);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, ccgddDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, ccgddDTO.FecModificacion);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public List<CCGDDDTO> GetCamCCGDDByFilter(string plancodi, string empresa, string estado)
        {
            List<CCGDDDTO> list = new List<CCGDDDTO>();
            string query = $@"
                SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  FROM CAM_CCGDD CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.CCGDDCODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
   

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CCGDDDTO ob = new CCGDDDTO();
                    ob.CcgddCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDDCODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDDCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Hora = !dr.IsDBNull(dr.GetOrdinal("HORA")) ? dr.GetString(dr.GetOrdinal("HORA")) : "";
                    ob.Demanda = !dr.IsDBNull(dr.GetOrdinal("DEMANDA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDA")) : null;
                    ob.Generacion = !dr.IsDBNull(dr.GetOrdinal("GENERACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACION")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.NombreProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;
                    ob.DetalleProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? dr.GetString(dr.GetOrdinal("PROYDESCRIPCION")) : string.Empty;
                    ob.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPONOMBRE")) : string.Empty;
                    ob.SubTipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOFINOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPOFINOMBRE")) : string.Empty;
                    ob.Confidencial = dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? null : dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL"));
                    list.Add(ob);
                }
            }
            return list;
        }

    }
}
