using COES.Base.Core;
using COES.Infraestructura.Datos.Helper;
using COES.Dominio.DTO.Campania;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using COES.Infraestructura.Datos.Helper.Campania;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamRegHojaDRepository : RepositoryBase, ICamRegHojaDRepository
    {
        public CamRegHojaDRepository(string strConn) : base(strConn) { }

        CamRegHojaDHelper Helper = new CamRegHojaDHelper();

        public List<RegHojaDDTO> GetRegHojaDProyCodi(int proyCodi)
        {
            List<RegHojaDDTO> regHojaDDTOs = new List<RegHojaDDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaDProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RegHojaDDTO ob = new RegHojaDDTO();

                    ob.Hojadcodi = !dr.IsDBNull(dr.GetOrdinal("HOJADCODI")) ? dr.GetString(dr.GetOrdinal("HOJADCODI")) : "";
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Cuenca = !dr.IsDBNull(dr.GetOrdinal("CUENCA")) ? dr.GetString(dr.GetOrdinal("CUENCA")) : string.Empty;
                    ob.Caudal = !dr.IsDBNull(dr.GetOrdinal("CAUDAL")) ? dr.GetString(dr.GetOrdinal("CAUDAL")) : string.Empty;
                    ob.Estado = !dr.IsDBNull(dr.GetOrdinal("ESTADO")) ? dr.GetString(dr.GetOrdinal("ESTADO")) : string.Empty;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USUCREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FECHACREACION")) : DateTime.MinValue;
                    ob.Usumodificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USUMODIFICACION")) : string.Empty;
                    ob.Fechamodificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FECHAMODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;

                    regHojaDDTOs.Add(ob);
                }
            }

            return regHojaDDTOs;
        }

        public bool SaveRegHojaD(RegHojaDDTO regHojaDDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveRegHojaD);

            dbProvider.AddInParameter(dbCommand, "HOJADCODI", DbType.String, regHojaDDTO.Hojadcodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, regHojaDDTO.Proycodi);
            dbProvider.AddInParameter(dbCommand, "CUENCA", DbType.String, regHojaDDTO.Cuenca);
            dbProvider.AddInParameter(dbCommand, "CAUDAL", DbType.String, regHojaDDTO.Caudal);
            dbProvider.AddInParameter(dbCommand, "ESTADO", DbType.String, regHojaDDTO.Estado);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, regHojaDDTO.Usucreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, regHojaDDTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }


        public bool DeleteRegHojaDById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteRegHojaDById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.String, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }
        public bool DeleteRegHojaDById2(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteRegHojaDById2);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "HOJADCODI", DbType.String, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastRegHojaDId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastRegHojaDId);
            object result = dbProvider.ExecuteScalar(command);
            count = Convert.ToInt32(result);
            return count;
        }

        public List<RegHojaDDTO> GetRegHojaDById(int id)
        {
           List<RegHojaDDTO> regHojaDDTOs = new List<RegHojaDDTO>();
           DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaDById);
            dbProvider.AddInParameter(commandHoja, "HOJADCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    RegHojaDDTO ob = new RegHojaDDTO();

                    ob.Hojadcodi = !dr.IsDBNull(dr.GetOrdinal("HOJADCODI")) ? dr.GetString(dr.GetOrdinal("HOJADCODI")) : "";
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Cuenca = !dr.IsDBNull(dr.GetOrdinal("CUENCA")) ? dr.GetString(dr.GetOrdinal("CUENCA")) : string.Empty;
                    ob.Caudal = !dr.IsDBNull(dr.GetOrdinal("CAUDAL")) ? dr.GetString(dr.GetOrdinal("CAUDAL")) : string.Empty;
                    ob.Estado = !dr.IsDBNull(dr.GetOrdinal("ESTADO")) ? dr.GetString(dr.GetOrdinal("ESTADO")) : string.Empty;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.Usumodificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : string.Empty;
                    ob.Fechamodificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;

                    regHojaDDTOs.Add(ob);
                }
            }

            return regHojaDDTOs;
        }


        public bool UpdateRegHojaD(RegHojaDDTO regHojaDDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateRegHojaD);

            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, regHojaDDTO.Proycodi);
            dbProvider.AddInParameter(dbCommand, "CUENCA", DbType.String, regHojaDDTO.Cuenca);
            dbProvider.AddInParameter(dbCommand, "CAUDAL", DbType.String, regHojaDDTO.Caudal);
            dbProvider.AddInParameter(dbCommand, "ESTADO", DbType.String, regHojaDDTO.Estado);   
            dbProvider.AddInParameter(dbCommand, "USUCREACION", DbType.String, regHojaDDTO.Usucreacion);
            dbProvider.AddInParameter(dbCommand, "FECHACREACION", DbType.DateTime, regHojaDDTO.Fechacreacion);
            dbProvider.AddInParameter(dbCommand, "USUMODIFICACION", DbType.String, regHojaDDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FECHAMODIFICACION", DbType.DateTime, regHojaDDTO.Fechamodificacion);
            dbProvider.AddInParameter(dbCommand, "INDDEL", DbType.String, regHojaDDTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }


        public List<RegHojaDDTO> GetRegHojaDByFilter(string plancodi, string empresa, string estado)
        {
            List<RegHojaDDTO> regHojaDDTOs = new List<RegHojaDDTO>();
            string query = $@"
                 SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE FROM CAM_REGHOJAD CGB
                 INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                 INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                 INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                 LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.HOJADCODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            //DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaDById);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    RegHojaDDTO ob = new RegHojaDDTO();

                    ob.Hojadcodi = !dr.IsDBNull(dr.GetOrdinal("HOJADCODI")) ? dr.GetString(dr.GetOrdinal("HOJADCODI")) : "";
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Cuenca = !dr.IsDBNull(dr.GetOrdinal("CUENCA")) ? dr.GetString(dr.GetOrdinal("CUENCA")) : string.Empty;
                    ob.Caudal = !dr.IsDBNull(dr.GetOrdinal("CAUDAL")) ? dr.GetString(dr.GetOrdinal("CAUDAL")) : string.Empty;
                    ob.Estado = !dr.IsDBNull(dr.GetOrdinal("ESTADO")) ? dr.GetString(dr.GetOrdinal("ESTADO")) : string.Empty;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.Usumodificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : string.Empty;
                    ob.Fechamodificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.Proyecto = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;

                    regHojaDDTOs.Add(ob);
                }
            }

            return regHojaDDTOs;
        }
    }


}
