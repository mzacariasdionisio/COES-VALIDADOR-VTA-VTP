using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Campania;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using COES.Infraestructura.Datos.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamRegHojaCRepository: RepositoryBase, ICamRegHojaCRepository
    {

        public CamRegHojaCRepository(string strConn) : base(strConn){}

        CamRegHojaCHelper Helper = new CamRegHojaCHelper();

        public List<RegHojaCDTO> GetRegHojaCProyCodi(int proyCodi)
        {
            List<RegHojaCDTO> regHojaCDTOs = new List<RegHojaCDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaCProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.String, proyCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RegHojaCDTO ob = new RegHojaCDTO();
                    ob.Fichaccodi = !dr.IsDBNull(dr.GetOrdinal("FICHACCODI")) ? dr.GetInt32(dr.GetOrdinal("FICHACCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Fecpuestaope = !dr.IsDBNull(dr.GetOrdinal("FECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("FECPUESTAOPE")) : (DateTime?)null;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    regHojaCDTOs.Add(ob);
                }
            }

            return regHojaCDTOs;
        }

        public bool SaveRegHojaC(RegHojaCDTO regHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveRegHojaC);
            dbProvider.AddInParameter(dbCommand, "FICHACCODI", DbType.Int32, ObtenerValorOrDefault(regHojaCDTO.Fichaccodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(regHojaCDTO.Proycodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "FECPUESTAOPE", DbType.DateTime, regHojaCDTO.Fecpuestaope);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(regHojaCDTO.Usucreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteRegHojaCById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteRegHojaCById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastRegHojaCId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastRegHojaCId);
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

        public RegHojaCDTO GetRegHojaCById(int id)
        {
            RegHojaCDTO ob = new RegHojaCDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaCById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Fichaccodi = !dr.IsDBNull(dr.GetOrdinal("FICHACCODI")) ? dr.GetInt32(dr.GetOrdinal("FICHACCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Fecpuestaope = !dr.IsDBNull(dr.GetOrdinal("FECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("FECPUESTAOPE")) : (DateTime?)null;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateRegHojaC(RegHojaCDTO regHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateRegHojaC);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, regHojaCDTO.Proycodi);
            dbProvider.AddInParameter(dbCommand, "FECPUESTAOPE", DbType.DateTime, regHojaCDTO.Fecpuestaope);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, regHojaCDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "FICHACCODI", DbType.Int32, regHojaCDTO.Fichaccodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public List<DetRegHojaCDTO> GetRegHojaCByFilter(string plancodi, string empresa, string estado)
        {
            List<DetRegHojaCDTO> oblist = new List<DetRegHojaCDTO>();
            string query = $@"
                  SELECT CRONDET.*, CRON.PROYCODI,
                  TR.EMPRESANOM, 
                  TR.PROYNOMBRE, 
                  TR.PROYDESCRIPCION, 
                  TR.PROYCONFIDENCIAL
                  FROM CAM_REGHOJAC CRON
                 INNER JOIN CAM_REGHOJACDET CRONDET ON CRON.FICHACCODI = CRONDET.FICHACCODI AND CRONDET.IND_DEL=0
                 INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CRON.PROYCODI
                 INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                    WHERE TR.PERICODI  IN ({plancodi}) AND 
                    PL.CODEMPRESA IN ({empresa})  AND 
                    CRON.IND_DEL = 0 AND 
                    PL.PLANESTADO ='{estado}'
                    ORDER BY TR.PERICODI, CRON.PROYCODI,PL.CODEMPRESA, CRON.FICHACCODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DetRegHojaCDTO ob = new DetRegHojaCDTO();
                    ob.Fichaccodi = !dr.IsDBNull(dr.GetOrdinal("FICHACCODI")) ? dr.GetInt32(dr.GetOrdinal("FICHACCODI")) : 0;
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
