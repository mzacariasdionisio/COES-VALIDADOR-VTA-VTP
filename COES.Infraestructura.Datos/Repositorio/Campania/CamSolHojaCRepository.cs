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
    public class CamSolHojaCRepository : RepositoryBase, ICamSolHojaCRepository
    {

        public CamSolHojaCRepository(string strConn) : base(strConn) { }

        CamSolHojaCHelper Helper = new CamSolHojaCHelper();

        public List<SolHojaCDTO> GetSolHojaCProyCodi(int proyCodi)
        {
            List<SolHojaCDTO> regHojaCDTOs = new List<SolHojaCDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetSolHojaCProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.String, proyCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SolHojaCDTO ob = new SolHojaCDTO();
                    ob.Solhojaccodi = !dr.IsDBNull(dr.GetOrdinal("SOLHOJACCODI")) ? dr.GetInt32(dr.GetOrdinal("SOLHOJACCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Fecpuestaope = !dr.IsDBNull(dr.GetOrdinal("FECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("FECPUESTAOPE")) : (DateTime?)null;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    regHojaCDTOs.Add(ob);
                }
            }

            return regHojaCDTOs;
        }

        public bool SaveSolHojaC(SolHojaCDTO regHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveSolHojaC);
            dbProvider.AddInParameter(dbCommand, "SOLHOJACCODI", DbType.Int32, ObtenerValorOrDefault(regHojaCDTO.Solhojaccodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(regHojaCDTO.Proycodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "FECPUESTAOPE", DbType.DateTime, regHojaCDTO.Fecpuestaope);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(regHojaCDTO.Usucreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteSolHojaCById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteSolHojaCById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastSolHojaCId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastSolHojaCId);
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

        public SolHojaCDTO GetSolHojaCById(int id)
        {
            SolHojaCDTO ob = new SolHojaCDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetSolHojaCById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Solhojaccodi = !dr.IsDBNull(dr.GetOrdinal("SOLHOJACCODI")) ? dr.GetInt32(dr.GetOrdinal("SOLHOJACCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Fecpuestaope = !dr.IsDBNull(dr.GetOrdinal("FECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("FECPUESTAOPE")) : (DateTime?)null;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateSolHojaC(SolHojaCDTO regHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateSolHojaC);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, regHojaCDTO.Proycodi);
            dbProvider.AddInParameter(dbCommand, "FECPUESTAOPE", DbType.DateTime, regHojaCDTO.Fecpuestaope);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, regHojaCDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "SOLHOJACCODI", DbType.Int32, regHojaCDTO.Solhojaccodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public List<DetSolHojaCDTO> GetSolHojaCByFilter(string plancodi, string empresa, string estado)
        {
            List<DetSolHojaCDTO> oblist = new List<DetSolHojaCDTO>();
            string query = $@"
                  SELECT CRONDET.*, CRON.PROYCODI,
                  TR.EMPRESANOM, 
                  TR.PROYNOMBRE, 
                  TR.PROYDESCRIPCION, 
                  TR.PROYCONFIDENCIAL
                  FROM CAM_SOLHOJAC CRON
                 INNER JOIN CAM_SOLHOJACDET CRONDET ON CRON.SOLHOJACCODI = CRONDET.SOLHOJACCODI AND CRONDET.IND_DEL=0
                 INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CRON.PROYCODI
                 INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                    WHERE TR.PERICODI  IN ({plancodi}) AND 
                    PL.CODEMPRESA IN ({empresa})  AND 
                    CRON.IND_DEL = 0 AND 
                    PL.PLANESTADO ='{estado}'
                    ORDER BY TR.PERICODI, CRON.PROYCODI,PL.CODEMPRESA, CRON.SOLHOJACCODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DetSolHojaCDTO ob = new DetSolHojaCDTO();
                    ob.Solhojaccodi = !dr.IsDBNull(dr.GetOrdinal("SOLHOJACCODI")) ? dr.GetInt32(dr.GetOrdinal("SOLHOJACCODI")) : 0;
                    ob.Datacatcodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : "";
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : "";
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetString(dr.GetOrdinal("PROYCODI")) : "";
                    ob.ProyNombre = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : "";
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.Usumodificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
                    ob.Fechamodificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
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
