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
    public class CamItcdf104Repository : RepositoryBase, ICamItcdf104Repository
    {
        public CamItcdf104Repository(string strConn) : base(strConn) { }

        CamItcdf104Helper Helper = new CamItcdf104Helper();

        public List<Itcdf104DTO> GetItcdf104Codi(int proyCodi)
        {
            List<Itcdf104DTO> itcdf104DTOs = new List<Itcdf104DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdf104Codi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdf104DTO ob = new Itcdf104DTO();
                    ob.Itcdf104Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF104CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF104CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.MillonesolesPbi = !dr.IsDBNull(dr.GetOrdinal("MILLONESOLESPBI")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("MILLONESOLESPBI")) : null;
                    ob.TasaCrecimientoPbi = !dr.IsDBNull(dr.GetOrdinal("TASACRECIMIENTOPBI")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TASACRECIMIENTOPBI")) : null;
                    ob.NroClientesLibres = !dr.IsDBNull(dr.GetOrdinal("NROCLIENTESLIBRES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NROCLIENTESLIBRES")) : null;
                    ob.NroClientesRegulados = !dr.IsDBNull(dr.GetOrdinal("NROCLIENTESREGULADOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NROCLIENTESREGULADOS")) : null;
                    ob.NroHabitantes = !dr.IsDBNull(dr.GetOrdinal("NROHABITANTES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NROHABITANTES")) : null;
                    ob.TasaCrecimientoPoblacion = !dr.IsDBNull(dr.GetOrdinal("TASACRECIMIENTOPOBLACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TASACRECIMIENTOPOBLACION")) : null;
                    ob.MillonesClientesRegulados = !dr.IsDBNull(dr.GetOrdinal("MILLONESCLIENTESREGULADOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("MILLONESCLIENTESREGULADOS")) : null;
                    ob.ClientesReguladoSelectr = !dr.IsDBNull(dr.GetOrdinal("CLIENTESREGULADOSELECTR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CLIENTESREGULADOSELECTR")) : null;
                    ob.Usmwh = !dr.IsDBNull(dr.GetOrdinal("USMWH")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("USMWH")) : null;
                    ob.TasaCrecimientoEnergia = !dr.IsDBNull(dr.GetOrdinal("TASACRECIMIENTOENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TASACRECIMIENTOENERGIA")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    itcdf104DTOs.Add(ob);
                }
            }

            return itcdf104DTOs;
        }

        public bool SaveItcdf104(Itcdf104DTO itcdf104DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveItcdf104);
            dbProvider.AddInParameter(dbCommand, "ITCDF104CODI", DbType.Int32, itcdf104DTO.Itcdf104Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcdf104DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, itcdf104DTO.Anio);
            dbProvider.AddInParameter(dbCommand, "MILLONESOLESPBI", DbType.Decimal, itcdf104DTO.MillonesolesPbi);
            dbProvider.AddInParameter(dbCommand, "TASACRECIMIENTOPBI", DbType.Decimal, itcdf104DTO.TasaCrecimientoPbi);
            dbProvider.AddInParameter(dbCommand, "NROCLIENTESLIBRES", DbType.Decimal, itcdf104DTO.NroClientesLibres);
            dbProvider.AddInParameter(dbCommand, "NROCLIENTESREGULADOS", DbType.Decimal, itcdf104DTO.NroClientesRegulados);
            dbProvider.AddInParameter(dbCommand, "NROHABITANTES", DbType.Decimal, itcdf104DTO.NroHabitantes);
            dbProvider.AddInParameter(dbCommand, "TASACRECIMIENTOPOBLACION", DbType.Decimal, itcdf104DTO.TasaCrecimientoPoblacion);
            dbProvider.AddInParameter(dbCommand, "MILLONESCLIENTESREGULADOS", DbType.Decimal, itcdf104DTO.MillonesClientesRegulados);
            dbProvider.AddInParameter(dbCommand, "CLIENTESREGULADOSELECTR", DbType.Decimal, itcdf104DTO.ClientesReguladoSelectr);
            dbProvider.AddInParameter(dbCommand, "USMWH", DbType.Decimal, itcdf104DTO.Usmwh);
            dbProvider.AddInParameter(dbCommand, "TASACRECIMIENTOENERGIA", DbType.Decimal, itcdf104DTO.TasaCrecimientoEnergia);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcdf104DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, itcdf104DTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcdf104ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteItcdf104ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcdf104Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastItcdf104Id);
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

        public List<Itcdf104DTO> GetItcdf104ById(int id)
        {
            List<Itcdf104DTO> itcdf104DTOs = new List<Itcdf104DTO>();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdf104ById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    Itcdf104DTO ob = new Itcdf104DTO();
                    ob.Itcdf104Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF104CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF104CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.MillonesolesPbi = !dr.IsDBNull(dr.GetOrdinal("MILLONESOLESPBI")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("MILLONESOLESPBI")) : null;
                    ob.TasaCrecimientoPbi = !dr.IsDBNull(dr.GetOrdinal("TASACRECIMIENTOPBI")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TASACRECIMIENTOPBI")) : null;
                    ob.NroClientesLibres = !dr.IsDBNull(dr.GetOrdinal("NROCLIENTESLIBRES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NROCLIENTESLIBRES")) : null;
                    ob.NroClientesRegulados = !dr.IsDBNull(dr.GetOrdinal("NROCLIENTESREGULADOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NROCLIENTESREGULADOS")) : null;
                    ob.NroHabitantes = !dr.IsDBNull(dr.GetOrdinal("NROHABITANTES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NROHABITANTES")) : null;
                    ob.TasaCrecimientoPoblacion = !dr.IsDBNull(dr.GetOrdinal("TASACRECIMIENTOPOBLACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TASACRECIMIENTOPOBLACION")) : null;
                    ob.MillonesClientesRegulados = !dr.IsDBNull(dr.GetOrdinal("MILLONESCLIENTESREGULADOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("MILLONESCLIENTESREGULADOS")) : null;
                    ob.ClientesReguladoSelectr = !dr.IsDBNull(dr.GetOrdinal("CLIENTESREGULADOSELECTR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CLIENTESREGULADOSELECTR")) : null;
                    ob.Usmwh = !dr.IsDBNull(dr.GetOrdinal("USMWH")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("USMWH")) : null;
                    ob.TasaCrecimientoEnergia = !dr.IsDBNull(dr.GetOrdinal("TASACRECIMIENTOENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TASACRECIMIENTOENERGIA")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    itcdf104DTOs.Add(ob);
                }
            }

            return itcdf104DTOs;
        }

        public bool UpdateItcdf104(Itcdf104DTO itcdf104DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateItcdf104);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcdf104DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, itcdf104DTO.Anio);
            dbProvider.AddInParameter(dbCommand, "MILLONESOLESPBI", DbType.Decimal, itcdf104DTO.MillonesolesPbi);
            dbProvider.AddInParameter(dbCommand, "TASACRECIMIENTOPBI", DbType.Decimal, itcdf104DTO.TasaCrecimientoPbi);
            dbProvider.AddInParameter(dbCommand, "NROCLIENTESLIBRES", DbType.Decimal, itcdf104DTO.NroClientesLibres);
            dbProvider.AddInParameter(dbCommand, "NROCLIENTESREGULADOS", DbType.Decimal, itcdf104DTO.NroClientesRegulados);
            dbProvider.AddInParameter(dbCommand, "NROHABITANTES", DbType.Decimal, itcdf104DTO.NroHabitantes);
            dbProvider.AddInParameter(dbCommand, "TASACRECIMIENTOPOBLACION", DbType.Decimal, itcdf104DTO.TasaCrecimientoPoblacion);
            dbProvider.AddInParameter(dbCommand, "MILLONESCLIENTESREGULADOS", DbType.Decimal, itcdf104DTO.MillonesClientesRegulados);
            dbProvider.AddInParameter(dbCommand, "CLIENTESREGULADOSELECTR", DbType.Decimal, itcdf104DTO.ClientesReguladoSelectr);
            dbProvider.AddInParameter(dbCommand, "USMWH", DbType.Decimal, itcdf104DTO.Usmwh);
            dbProvider.AddInParameter(dbCommand, "TASACRECIMIENTOENERGIA", DbType.Decimal, itcdf104DTO.TasaCrecimientoEnergia);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcdf104DTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcdf104DTO.FecModificacion);
            dbProvider.AddInParameter(dbCommand, "ITCDF104CODI", DbType.Int32, itcdf104DTO.Itcdf104Codi);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public List<Itcdf104DTO> GetItcdf104ByFilter(string plancodi, string empresa, string estado)
        {
            List<Itcdf104DTO> itcdf104DTOs = new List<Itcdf104DTO>();
            string query = $@"
                SELECT CGB.*, TR.EMPRESANOM, TR.AREADEMANDA
                FROM CAM_ITCDF104 CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PLANCODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.ITCDF104CODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    Itcdf104DTO ob = new Itcdf104DTO();
                    ob.Itcdf104Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF104CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF104CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.MillonesolesPbi = !dr.IsDBNull(dr.GetOrdinal("MILLONESOLESPBI")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("MILLONESOLESPBI")) : null;
                    ob.TasaCrecimientoPbi = !dr.IsDBNull(dr.GetOrdinal("TASACRECIMIENTOPBI")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TASACRECIMIENTOPBI")) : null;
                    ob.NroClientesLibres = !dr.IsDBNull(dr.GetOrdinal("NROCLIENTESLIBRES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NROCLIENTESLIBRES")) : null;
                    ob.NroClientesRegulados = !dr.IsDBNull(dr.GetOrdinal("NROCLIENTESREGULADOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NROCLIENTESREGULADOS")) : null;
                    ob.NroHabitantes = !dr.IsDBNull(dr.GetOrdinal("NROHABITANTES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NROHABITANTES")) : null;
                    ob.TasaCrecimientoPoblacion = !dr.IsDBNull(dr.GetOrdinal("TASACRECIMIENTOPOBLACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TASACRECIMIENTOPOBLACION")) : null;
                    ob.MillonesClientesRegulados = !dr.IsDBNull(dr.GetOrdinal("MILLONESCLIENTESREGULADOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("MILLONESCLIENTESREGULADOS")) : null;
                    ob.ClientesReguladoSelectr = !dr.IsDBNull(dr.GetOrdinal("CLIENTESREGULADOSELECTR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CLIENTESREGULADOSELECTR")) : null;
                    ob.Usmwh = !dr.IsDBNull(dr.GetOrdinal("USMWH")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("USMWH")) : null;
                    ob.TasaCrecimientoEnergia = !dr.IsDBNull(dr.GetOrdinal("TASACRECIMIENTOENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TASACRECIMIENTOENERGIA")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
                    // Nuevos campos
                    ob.AreaDemanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetString(dr.GetOrdinal("AREADEMANDA")) : "";
                    
                    itcdf104DTOs.Add(ob);
                }
            }

            return itcdf104DTOs;
        }
    }

}
