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
    public class CamLineasFichaBRepository : RepositoryBase, ICamLineasFichaBRepository
    {
    public CamLineasFichaBRepository(string strConn) : base(strConn) { }

    CamLineasFichaBHelper Helper = new CamLineasFichaBHelper();

        public List<LineasFichaBDTO> GetLineasFichaB(int proyCodi)
        {
            List<LineasFichaBDTO> lineasFichaBDTOs = new List<LineasFichaBDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLineasFichaB);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.String, proyCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    LineasFichaBDTO ob = new LineasFichaBDTO();
                    ob.FichaBCodi = !dr.IsDBNull(dr.GetOrdinal("FICHABCODI")) ? dr.GetInt32(dr.GetOrdinal("FICHABCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.FecPuestaOpe = !dr.IsDBNull(dr.GetOrdinal("FECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("FECPUESTAOPE")) : (DateTime?)null;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    lineasFichaBDTOs.Add(ob);
                }
            }

            return lineasFichaBDTOs;
        }

        public bool SaveLineasFichaB(LineasFichaBDTO lineasFichaBDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveLineasFichaB);
            dbProvider.AddInParameter(dbCommand, "FICHABCODI", DbType.Int32, ObtenerValorOrDefault(lineasFichaBDTO.FichaBCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(lineasFichaBDTO.ProyCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "FECPUESTAOPE", DbType.DateTime, lineasFichaBDTO.FecPuestaOpe);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(lineasFichaBDTO.UsuCreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteLineasFichaBById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteLineasFichaBById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastLineasFichaBCodi()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastLineasFichaBId);
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

        public LineasFichaBDTO GetLineasFichaBById(int id)
        {
            LineasFichaBDTO ob = new LineasFichaBDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetLineasFichaBById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.FichaBCodi = !dr.IsDBNull(dr.GetOrdinal("FICHABCODI")) ? dr.GetInt32(dr.GetOrdinal("FICHABCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.FecPuestaOpe = !dr.IsDBNull(dr.GetOrdinal("FECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("FECPUESTAOPE")) : (DateTime?)null;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateLineasFichaB(LineasFichaBDTO lineasFichaBDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateLineasFichaB);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, lineasFichaBDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "FECPUESTAOPE", DbType.DateTime, lineasFichaBDTO.FecPuestaOpe);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, lineasFichaBDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "FICHABCODI", DbType.Int32, lineasFichaBDTO.FichaBCodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public List<LineasFichaBDetDTO> GetLineasFichaBByFilter(string plancodi, string empresa, string estado)
        {
            List<LineasFichaBDetDTO> oblist = new List<LineasFichaBDetDTO>();
            string query = $@"
                  SELECT CRONDET.*, CRON.PROYCODI, CRON.FECPUESTAOPE,
                  TR.EMPRESANOM, 
                  TR.PROYNOMBRE, 
                  TR.PROYDESCRIPCION, 
                  TR.PROYCONFIDENCIAL
                  FROM CAM_LINEASFICHAB CRON
                 INNER JOIN CAM_LINEASFICHABDET CRONDET ON CRON.FICHABCODI = CRONDET.FICHABCODI AND CRONDET.IND_DEL=0
                 INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CRON.PROYCODI
                 INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                    WHERE TR.PERICODI  IN ({plancodi}) AND 
                    PL.CODEMPRESA IN ({empresa})  AND 
                    CRON.IND_DEL = 0 AND 
                    PL.PLANESTADO ='{estado}'
                    ORDER BY TR.PERICODI, CRON.PROYCODI,PL.CODEMPRESA, CRON.FICHABCODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    LineasFichaBDetDTO ob = new LineasFichaBDetDTO();
                    ob.FichaBCodi = !dr.IsDBNull(dr.GetOrdinal("FICHABCODI")) ? dr.GetInt32(dr.GetOrdinal("FICHABCODI")) : 0;
                    ob.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : "";
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : "";
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetString(dr.GetOrdinal("PROYCODI")) : "";
                    ob.FecPuestaOpe = !dr.IsDBNull(dr.GetOrdinal("FECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("FECPUESTAOPE")) : (DateTime?)null;
                    ob.ProyNombre = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    oblist.Add(ob);
                }
            }
            return oblist;
           
        }

        object ObtenerValorOrDefault(object valor, Type tipo)
        {
            DateTime fechaMinimaValida = DateTime.Now;
            if (valor == null || (valor is DateTime && (DateTime)valor == DateTime.MinValue))
            {
                if (tipo == typeof(int) || tipo == typeof(int?))
                {
                    return 0;
                }
                else if (tipo == typeof(string))
                {
                    return "";
                }
                else if (tipo == typeof(DateTime) || tipo == typeof(DateTime?))
                {
                    return fechaMinimaValida;
                }
            }
            return valor;
        }

    }
}
