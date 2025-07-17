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
    public class CamItcdf110Repository : RepositoryBase, ICamItcdf110Repository
    {
        public CamItcdf110Repository(string strConn) : base(strConn) { }

        CamItcdf110Helper Helper = new CamItcdf110Helper();

        public List<Itcdf110DTO> GetItcdf110Codi(int proyCodi)
        {
            List<Itcdf110DTO> itcdf110DTOs = new List<Itcdf110DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdf110Codi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdf110DTO ob = new Itcdf110DTO();
                    ob.Itcdf110Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF110CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF110CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.AreaDemanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetString(dr.GetOrdinal("AREA_DEMANDA")) : "";
                    ob.Sistema = !dr.IsDBNull(dr.GetOrdinal("SISTEMA")) ? dr.GetString(dr.GetOrdinal("SISTEMA")) : "";
                    ob.Subestacion = !dr.IsDBNull(dr.GetOrdinal("SUBESTACION")) ? dr.GetString(dr.GetOrdinal("SUBESTACION")) : "";
                    ob.Tension = !dr.IsDBNull(dr.GetOrdinal("TENSION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TENSION")) : null; 
                    ob.Barra = !dr.IsDBNull(dr.GetOrdinal("BARRA")) ? dr.GetString(dr.GetOrdinal("BARRA")) : "";
                    ob.IdCarga = !dr.IsDBNull(dr.GetOrdinal("IDCARGA")) ? dr.GetString(dr.GetOrdinal("IDCARGA")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";

                    itcdf110DTOs.Add(ob);
                }
            }

            return itcdf110DTOs;
        }

        public bool SaveItcdf110(Itcdf110DTO itcdf110DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveItcdf110);
            dbProvider.AddInParameter(dbCommand, "ITCDF110CODI", DbType.Int32, itcdf110DTO.Itcdf110Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcdf110DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "AREADEMANDA", DbType.String, itcdf110DTO.AreaDemanda);
            dbProvider.AddInParameter(dbCommand, "SISTEMA", DbType.String, itcdf110DTO.Sistema);
            dbProvider.AddInParameter(dbCommand, "SUBESTACION", DbType.String, itcdf110DTO.Subestacion);
            dbProvider.AddInParameter(dbCommand, "TENSION", DbType.Decimal, itcdf110DTO.Tension);
            dbProvider.AddInParameter(dbCommand, "BARRA", DbType.String, itcdf110DTO.Barra);
            dbProvider.AddInParameter(dbCommand, "IDCARGA", DbType.String, itcdf110DTO.IdCarga);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcdf110DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, itcdf110DTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcdf110ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteItcdf110ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcdf110Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastItcdf110Id);
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

        public List<Itcdf110DTO> GetItcdf110ById(int id)
        {
            List<Itcdf110DTO> itcdf110DTOs = new List<Itcdf110DTO>();
           
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdf110ById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    Itcdf110DTO ob = new Itcdf110DTO();
                    ob.Itcdf110Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF110CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF110CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.AreaDemanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetString(dr.GetOrdinal("AREADEMANDA")) : "";
                    ob.Sistema = !dr.IsDBNull(dr.GetOrdinal("SISTEMA")) ? dr.GetString(dr.GetOrdinal("SISTEMA")) : "";
                    ob.Subestacion = !dr.IsDBNull(dr.GetOrdinal("SUBESTACION")) ? dr.GetString(dr.GetOrdinal("SUBESTACION")) : "";
                    ob.Tension = !dr.IsDBNull(dr.GetOrdinal("TENSION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TENSION")) : null;
                    ob.Barra = !dr.IsDBNull(dr.GetOrdinal("BARRA")) ? dr.GetString(dr.GetOrdinal("BARRA")) : "";
                    ob.IdCarga = !dr.IsDBNull(dr.GetOrdinal("IDCARGA")) ? dr.GetString(dr.GetOrdinal("IDCARGA")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    itcdf110DTOs.Add(ob);
                }
            }

            return itcdf110DTOs;
        }

        public bool UpdateItcdf110(Itcdf110DTO itcdf110DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateItcdf110);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcdf110DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "AREADEMANDA", DbType.String, itcdf110DTO.AreaDemanda);
            dbProvider.AddInParameter(dbCommand, "SISTEMA", DbType.String, itcdf110DTO.Sistema);
            dbProvider.AddInParameter(dbCommand, "SUBESTACION", DbType.String, itcdf110DTO.Subestacion);
            dbProvider.AddInParameter(dbCommand, "TENSION", DbType.Decimal, itcdf110DTO.Tension);
            dbProvider.AddInParameter(dbCommand, "BARRA", DbType.String, itcdf110DTO.Barra);
            dbProvider.AddInParameter(dbCommand, "IDCARGA", DbType.String, itcdf110DTO.IdCarga);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcdf110DTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcdf110DTO.FecModificacion);
            dbProvider.AddInParameter(dbCommand, "ITCDF110CODI", DbType.Int32, itcdf110DTO.Itcdf110Codi);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public List<Itcdf110DTO> GetItcdf110ByFilter(string plancodi, string empresa, string estado)
        {
            List<Itcdf110DTO> itcdf110DTOs = new List<Itcdf110DTO>();
            string query = $@"
                SELECT CGB.*, TR.AREADEMANDA, TR.EMPRESANOM  
                FROM CAM_ITCDF110 CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                 AND NOT (
                       CGB.AREADEMANDA IS NULL 
                       AND CGB.SISTEMA IS NULL 
                       AND CGB.SUBESTACION IS NULL 
                       AND CGB.TENSION IS NULL 
                       AND CGB.BARRA IS NULL 
                       AND CGB.IDCARGA IS NULL
                  )   
                ORDER BY TR.PLANCODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.ITCDF110CODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    Itcdf110DTO ob = new Itcdf110DTO();
                    ob.Itcdf110Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF110CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF110CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.AreaDemanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetString(dr.GetOrdinal("AREADEMANDA")) : "";
                    ob.Sistema = !dr.IsDBNull(dr.GetOrdinal("SISTEMA")) ? dr.GetString(dr.GetOrdinal("SISTEMA")) : "";
                    ob.Subestacion = !dr.IsDBNull(dr.GetOrdinal("SUBESTACION")) ? dr.GetString(dr.GetOrdinal("SUBESTACION")) : "";
                    ob.Tension = !dr.IsDBNull(dr.GetOrdinal("TENSION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TENSION")) : null;
                    ob.Barra = !dr.IsDBNull(dr.GetOrdinal("BARRA")) ? dr.GetString(dr.GetOrdinal("BARRA")) : "";
                    ob.IdCarga = !dr.IsDBNull(dr.GetOrdinal("IDCARGA")) ? dr.GetString(dr.GetOrdinal("IDCARGA")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";

                    itcdf110DTOs.Add(ob);
                }
            }

            return itcdf110DTOs;
        }


    }

}