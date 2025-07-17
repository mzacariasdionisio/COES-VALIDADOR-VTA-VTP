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
    public class CamItcdf108Repository : RepositoryBase, ICamItcdf108Repository
    {
        public CamItcdf108Repository(string strConn) : base(strConn) { }

        CamItcdf108Helper Helper = new CamItcdf108Helper();

        public List<Itcdf108DTO> GetItcdf108Codi(int proyCodi)
        {
            List<Itcdf108DTO> itcdf108DTOs = new List<Itcdf108DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdf108Codi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdf108DTO ob = new Itcdf108DTO();
                    ob.Itcdf108Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF108CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF108CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.Atval = !dr.IsDBNull(dr.GetOrdinal("ATVAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ATVAL")) : null;
                    ob.Mtval = !dr.IsDBNull(dr.GetOrdinal("MTVAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("MTVAL")) : null;
                    ob.Btval = !dr.IsDBNull(dr.GetOrdinal("BTVAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("BTVAL")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    itcdf108DTOs.Add(ob);
                }
            }

            return itcdf108DTOs;
        }

        public bool SaveItcdf108(Itcdf108DTO itcdf108DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveItcdf108);
            dbProvider.AddInParameter(dbCommand, "ITCDF108CODI", DbType.Int32, itcdf108DTO.Itcdf108Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcdf108DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, itcdf108DTO.Anio);
            dbProvider.AddInParameter(dbCommand, "ATVAL", DbType.Decimal, itcdf108DTO.Atval);
            dbProvider.AddInParameter(dbCommand, "MTVAL", DbType.Decimal, itcdf108DTO.Mtval);
            dbProvider.AddInParameter(dbCommand, "BTVAL", DbType.Decimal, itcdf108DTO.Btval);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcdf108DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, itcdf108DTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcdf108ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteItcdf108ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcdf108Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastItcdf108Id);
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

        public List<Itcdf108DTO> GetItcdf108ById(int id)
        {
            List<Itcdf108DTO> itcdf108DTOs = new List<Itcdf108DTO>();
            
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdf108ById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    Itcdf108DTO ob = new Itcdf108DTO();
                    ob.Itcdf108Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF108CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF108CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.Atval = !dr.IsDBNull(dr.GetOrdinal("ATVAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ATVAL")) : null;
                    ob.Mtval = !dr.IsDBNull(dr.GetOrdinal("MTVAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("MTVAL")) : null;
                    ob.Btval = !dr.IsDBNull(dr.GetOrdinal("BTVAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("BTVAL")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    itcdf108DTOs.Add(ob);
                }
            }

            return itcdf108DTOs;
        }

        public bool UpdateItcdf108(Itcdf108DTO itcdf108DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateItcdf108);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, itcdf108DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, itcdf108DTO.Anio);
            dbProvider.AddInParameter(dbCommand, "ATVAL", DbType.Decimal, itcdf108DTO.Atval);
            dbProvider.AddInParameter(dbCommand, "MTVAL", DbType.Decimal, itcdf108DTO.Mtval);
            dbProvider.AddInParameter(dbCommand, "BTVAL", DbType.Decimal, itcdf108DTO.Btval);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcdf108DTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcdf108DTO.FecModificacion);
            dbProvider.AddInParameter(dbCommand, "ITCDF108CODI", DbType.Int32, itcdf108DTO.Itcdf108Codi);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public List<Itcdf108DTO> GetItcdf108ByFilter(string plancodi, string empresa, string estado)
        {
            List<Itcdf108DTO> itcdf108DTOs = new List<Itcdf108DTO>();
            string query = $@"
                SELECT CGB.*, TR.AREADEMANDA, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL   
                FROM CAM_ITCDF108 CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
 INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
        LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PLANCODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.ITCDF108CODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
          

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    Itcdf108DTO ob = new Itcdf108DTO();
                    ob.Itcdf108Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF108CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF108CODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.Atval = !dr.IsDBNull(dr.GetOrdinal("ATVAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ATVAL")) : null;
                    ob.Mtval = !dr.IsDBNull(dr.GetOrdinal("MTVAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("MTVAL")) : null;
                    ob.Btval = !dr.IsDBNull(dr.GetOrdinal("BTVAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("BTVAL")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ob.AreaDemanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetString(dr.GetOrdinal("AREADEMANDA")) : "";
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
                    // Nuevos campos
              
                    itcdf108DTOs.Add(ob);
                }
            }

            return itcdf108DTOs;
        }
    }

}