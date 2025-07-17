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
    public class CamItcdf123Repository : RepositoryBase, ICamItcdf123Repository
    {
        public CamItcdf123Repository(string strConn) : base(strConn) { }

        CamItcdf123Helper Helper = new CamItcdf123Helper();

        public List<Itcdf123DTO> GetItcdf123Codi(int proyCodi)
        {
            List<Itcdf123DTO> itcdf123DTOs = new List<Itcdf123DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdf123Codi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdf123DTO ob = new Itcdf123DTO();
                    ob.Itcdf123Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF123CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF123CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.UtmEste = !dr.IsDBNull(dr.GetOrdinal("UTMESTE")) ? dr.GetString(dr.GetOrdinal("UTMESTE")) : null;
                    ob.UtmNorte = !dr.IsDBNull(dr.GetOrdinal("UTMNORTE")) ? dr.GetString(dr.GetOrdinal("UTMNORTE")) : null;
                    ob.UtmZona = !dr.IsDBNull(dr.GetOrdinal("UTMZONA")) ? dr.GetString(dr.GetOrdinal("UTMZONA")) : null;
                    ob.Anio1 = !dr.IsDBNull(dr.GetOrdinal("ANIO1")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ANIO1")) : null;
                    ob.Anio2 = !dr.IsDBNull(dr.GetOrdinal("ANIO2")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ANIO2")) : null;
                    ob.Anio3 = !dr.IsDBNull(dr.GetOrdinal("ANIO3")) ? (decimal?)(decimal?)dr.GetDecimal(dr.GetOrdinal("ANIO3")) : null;
                    ob.Anio4 = !dr.IsDBNull(dr.GetOrdinal("ANIO4")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ANIO4")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null;
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null;
                    itcdf123DTOs.Add(ob);
                }
            }

            return itcdf123DTOs;
        }

        public bool SaveItcdf123(Itcdf123DTO itcdf123DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveItcdf123);
            dbProvider.AddInParameter(dbCommand, "ITCDF123CODI", DbType.Int32, itcdf123DTO.Itcdf123Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcdf123DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "UTMESTE", DbType.String, itcdf123DTO.UtmEste);
            dbProvider.AddInParameter(dbCommand, "UTMNORTE", DbType.String, itcdf123DTO.UtmNorte);
            dbProvider.AddInParameter(dbCommand, "UTMZONA", DbType.String, itcdf123DTO.UtmZona);
            dbProvider.AddInParameter(dbCommand, "ANIO1", DbType.Decimal, itcdf123DTO.Anio1);
            dbProvider.AddInParameter(dbCommand, "ANIO2", DbType.Decimal, itcdf123DTO.Anio2);
            dbProvider.AddInParameter(dbCommand, "ANIO3", DbType.Decimal, itcdf123DTO.Anio3);
            dbProvider.AddInParameter(dbCommand, "ANIO4", DbType.Decimal, itcdf123DTO.Anio4);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcdf123DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, itcdf123DTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcdf123ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteItcdf123ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcdf123Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastItcdf123Id);
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

        public List<Itcdf123DTO> GetItcdf123ById(int id)
        {
            List<Itcdf123DTO> itcdf123DTOs = new List<Itcdf123DTO>();
           
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdf123ById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdf123DTO ob = new Itcdf123DTO();
                    ob.Itcdf123Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF123CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF123CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.UtmEste = !dr.IsDBNull(dr.GetOrdinal("UTMESTE")) ? dr.GetString(dr.GetOrdinal("UTMESTE")) : null;
                    ob.UtmNorte = !dr.IsDBNull(dr.GetOrdinal("UTMNORTE")) ? dr.GetString(dr.GetOrdinal("UTMNORTE")) : null;
                    ob.UtmZona = !dr.IsDBNull(dr.GetOrdinal("UTMZONA")) ? dr.GetString(dr.GetOrdinal("UTMZONA")) : null;
                    ob.Anio1 = !dr.IsDBNull(dr.GetOrdinal("ANIO1")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ANIO1")) : null;
                    ob.Anio2 = !dr.IsDBNull(dr.GetOrdinal("ANIO2")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ANIO2")) : null;
                    ob.Anio3 = !dr.IsDBNull(dr.GetOrdinal("ANIO3")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ANIO3")) : null;
                    ob.Anio4 = !dr.IsDBNull(dr.GetOrdinal("ANIO4")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ANIO4")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null;
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null;
                    itcdf123DTOs.Add(ob);
                }
            }

            return itcdf123DTOs;
        }

        public bool UpdateItcdf123(Itcdf123DTO itcdf123DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateItcdf123);
            dbProvider.AddInParameter(dbCommand, "ITCDF123CODI", DbType.Int32, itcdf123DTO.Itcdf123Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcdf123DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "UTMESTE", DbType.String, itcdf123DTO.UtmEste);
            dbProvider.AddInParameter(dbCommand, "UTMNORTE", DbType.String, itcdf123DTO.UtmNorte);
            dbProvider.AddInParameter(dbCommand, "UTMZONA", DbType.String, itcdf123DTO.UtmZona);
            dbProvider.AddInParameter(dbCommand, "ANIO1", DbType.Decimal, itcdf123DTO.Anio1);
            dbProvider.AddInParameter(dbCommand, "ANIO2", DbType.Decimal, itcdf123DTO.Anio2);
            dbProvider.AddInParameter(dbCommand, "ANIO3", DbType.Decimal, itcdf123DTO.Anio3);
            dbProvider.AddInParameter(dbCommand, "ANIO4", DbType.Decimal, itcdf123DTO.Anio4);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcdf123DTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcdf123DTO.FecModificacion);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public List<Itcdf123DTO> GetItcdf123ByFilter(string plancodi, string empresa, string estado)
        {
            List<Itcdf123DTO> itcdf123DTOs = new List<Itcdf123DTO>();
            string query = $@"
                SELECT CGB.*, TR.AREADEMANDA, TR.EMPRESANOM  
                FROM CAM_ITCDF123 CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PLANCODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.ITCDF123CODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdf123DTO ob = new Itcdf123DTO();
                    ob.Itcdf123Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF123CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF123CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.UtmEste = !dr.IsDBNull(dr.GetOrdinal("UTMESTE")) ? dr.GetString(dr.GetOrdinal("UTMESTE")) : null;
                    ob.UtmNorte = !dr.IsDBNull(dr.GetOrdinal("UTMNORTE")) ? dr.GetString(dr.GetOrdinal("UTMNORTE")) : null;
                    ob.UtmZona = !dr.IsDBNull(dr.GetOrdinal("UTMZONA")) ? dr.GetString(dr.GetOrdinal("UTMZONA")) : null;
                    ob.Anio1 = !dr.IsDBNull(dr.GetOrdinal("ANIO1")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ANIO1")) : null;
                    ob.Anio2 = !dr.IsDBNull(dr.GetOrdinal("ANIO2")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ANIO2")) : null;
                    ob.Anio3 = !dr.IsDBNull(dr.GetOrdinal("ANIO3")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ANIO3")) : null;
                    ob.Anio4 = !dr.IsDBNull(dr.GetOrdinal("ANIO4")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ANIO4")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null;
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
                    // Nuevos campos
                    ob.AreaDemanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetString(dr.GetOrdinal("AREADEMANDA")) : "";
                    itcdf123DTOs.Add(ob);
                }
            }

            return itcdf123DTOs;
        }

    }
}
