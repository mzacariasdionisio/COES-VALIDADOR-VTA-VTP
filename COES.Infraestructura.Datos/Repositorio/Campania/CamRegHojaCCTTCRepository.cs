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
    public class CamRegHojaCCTTCRepository: RepositoryBase, ICamRegHojaCCTTCRepository
    {

        public CamRegHojaCCTTCRepository(string strConn) : base(strConn){}

        CamRegHojaCCTTCHelper Helper = new CamRegHojaCCTTCHelper();

        public List<RegHojaCCTTCDTO> GetRegHojaCCTTCProyCodi(int proyCodi)
        {
            List<RegHojaCCTTCDTO> regHojaCCTTCDTOs = new List<RegHojaCCTTCDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaCCTTCProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.String, proyCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RegHojaCCTTCDTO ob = new RegHojaCCTTCDTO();
                    ob.Centralcodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Turbfecpuestaope = !dr.IsDBNull(dr.GetOrdinal("TURBFECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("TURBFECPUESTAOPE")) : (DateTime?)null;
                    ob.Cicfecpuestaope = !dr.IsDBNull(dr.GetOrdinal("CICFECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("CICFECPUESTAOPE")) : (DateTime?)null;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    regHojaCCTTCDTOs.Add(ob);
                }
            }

            return regHojaCCTTCDTOs;
        }

        public bool SaveRegHojaCCTTC(RegHojaCCTTCDTO regHojaCCTTCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveRegHojaCCTTC);
            dbProvider.AddInParameter(dbCommand, "CENTRALCODI", DbType.Int32, ObtenerValorOrDefault(regHojaCCTTCDTO.Centralcodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(regHojaCCTTCDTO.Proycodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "TURBFECPUESTAOPE", DbType.DateTime, regHojaCCTTCDTO.Turbfecpuestaope);
            dbProvider.AddInParameter(dbCommand, "CICFECPUESTAOPE", DbType.DateTime, regHojaCCTTCDTO.Cicfecpuestaope);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(regHojaCCTTCDTO.Usucreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteRegHojaCCTTCById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteRegHojaCCTTCById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastRegHojaCCTTCId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastRegHojaCCTTCId);
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

        public RegHojaCCTTCDTO GetRegHojaCCTTCById(int id)
        {
            RegHojaCCTTCDTO ob = new RegHojaCCTTCDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaCCTTCById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Centralcodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Turbfecpuestaope = !dr.IsDBNull(dr.GetOrdinal("TURBFECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("TURBFECPUESTAOPE")) : (DateTime?)null;
                    ob.Cicfecpuestaope = !dr.IsDBNull(dr.GetOrdinal("CICFECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("CICFECPUESTAOPE")) : (DateTime?)null;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateRegHojaCCTTC(RegHojaCCTTCDTO regHojaCCTTCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateRegHojaCCTTC);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, regHojaCCTTCDTO.Proycodi);
            dbProvider.AddInParameter(dbCommand, "TURBFECPUESTAOPE", DbType.DateTime, regHojaCCTTCDTO.Turbfecpuestaope);
            dbProvider.AddInParameter(dbCommand, "CICFECPUESTAOPE", DbType.DateTime, regHojaCCTTCDTO.Cicfecpuestaope);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, regHojaCCTTCDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "CENTRALCODI", DbType.Int32, regHojaCCTTCDTO.Centralcodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public List<Det1RegHojaCCTTCDTO> GetRegHojaCCTTCByFilter(string plancodi, string empresa, string estado)
        {
            List<Det1RegHojaCCTTCDTO> oblist = new List<Det1RegHojaCCTTCDTO>();
            string query = $@"
                  SELECT CRONDET.*, CRON.PROYCODI,
                  TR.EMPRESANOM, 
                  TR.PROYNOMBRE, 
                  TR.PROYDESCRIPCION, 
                  TR.PROYCONFIDENCIAL
                  FROM CAM_CENTERMOHOJAC CRON
                 INNER JOIN CAM_CENTERMOHOJACDET1 CRONDET ON CRON.CENTRALCODI = CRONDET.CENTRALCODI AND CRONDET.IND_DEL=0
                 INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CRON.PROYCODI
                 INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                    WHERE TR.PERICODI  IN ({plancodi}) AND 
                    PL.CODEMPRESA IN ({empresa})  AND 
                    CRON.IND_DEL = 0 AND 
                    PL.PLANESTADO ='{estado}'
                    ORDER BY TR.PERICODI, CRON.PROYCODI,PL.CODEMPRESA, CRON.CENTRALCODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Det1RegHojaCCTTCDTO ob = new Det1RegHojaCCTTCDTO();
                    ob.Centralcodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCODI")) : 0;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : "";
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
        public List<Det2RegHojaCCTTCDTO> GetRegHojaCCTTC2ByFilter(string plancodi, string empresa, string estado)
        {
            List<Det2RegHojaCCTTCDTO> oblist = new List<Det2RegHojaCCTTCDTO>();
            string query = $@"
                  SELECT CRONDET.*, CRON.PROYCODI,
                  TR.EMPRESANOM, 
                  TR.PROYNOMBRE, 
                  TR.PROYDESCRIPCION, 
                  TR.PROYCONFIDENCIAL
                  FROM CAM_CENTERMOHOJAC CRON
                 INNER JOIN CAM_CENTERMOHOJACDET2 CRONDET ON CRON.CENTRALCODI = CRONDET.CENTRALCODI AND CRONDET.IND_DEL=0
                 INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CRON.PROYCODI
                 INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                    WHERE TR.PERICODI  IN ({plancodi}) AND 
                    PL.CODEMPRESA IN ({empresa})  AND 
                    CRON.IND_DEL = 0 AND 
                    PL.PLANESTADO ='{estado}'
                    ORDER BY TR.PERICODI, CRON.PROYCODI,PL.CODEMPRESA, CRON.CENTRALCODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Det2RegHojaCCTTCDTO ob = new Det2RegHojaCCTTCDTO();
                    ob.Centralcodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCODI")) : 0;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : "";
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
