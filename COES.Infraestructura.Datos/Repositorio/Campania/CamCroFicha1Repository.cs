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
    public class CamCroFicha1Repository : RepositoryBase, ICamCroFicha1Repository
    {

        public CamCroFicha1Repository(string strConn) : base(strConn) { }

        CamCroFicha1Helper Helper = new CamCroFicha1Helper();

        public List<CroFicha1DTO> GetCroFicha1ProyCodi(int proyCodi)
        {
            List<CroFicha1DTO> regHojaCDTOs = new List<CroFicha1DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCroFicha1ProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.String, proyCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CroFicha1DTO ob = new CroFicha1DTO();
                    ob.CroFicha1codi = !dr.IsDBNull(dr.GetOrdinal("CROFICHA1CODI")) ? dr.GetInt32(dr.GetOrdinal("CROFICHA1CODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Fecpuestaope = !dr.IsDBNull(dr.GetOrdinal("FECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("FECPUESTAOPE")) : (DateTime?)null;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    regHojaCDTOs.Add(ob);
                }
            }

            return regHojaCDTOs;
        }

        public bool SaveCroFicha1(CroFicha1DTO regHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCroFicha1);
            dbProvider.AddInParameter(dbCommand, "CROFICHA1CODI", DbType.Int32, ObtenerValorOrDefault(regHojaCDTO.CroFicha1codi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(regHojaCDTO.Proycodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "FECPUESTAOPE", DbType.DateTime, regHojaCDTO.Fecpuestaope);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(regHojaCDTO.Usucreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteCroFicha1ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCroFicha1ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastCroFicha1Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCroFicha1Id);
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

        public CroFicha1DTO GetCroFicha1ById(int id)
        {
            CroFicha1DTO ob = new CroFicha1DTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetCroFicha1ById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.CroFicha1codi = !dr.IsDBNull(dr.GetOrdinal("CROFICHA1CODI")) ? dr.GetInt32(dr.GetOrdinal("CROFICHA1CODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Fecpuestaope = !dr.IsDBNull(dr.GetOrdinal("FECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("FECPUESTAOPE")) : (DateTime?)null;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateCroFicha1(CroFicha1DTO regHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCroFicha1);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, regHojaCDTO.Proycodi);
            dbProvider.AddInParameter(dbCommand, "FECPUESTAOPE", DbType.DateTime, regHojaCDTO.Fecpuestaope);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, regHojaCDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "CROFICHA1CODI", DbType.Int32, regHojaCDTO.CroFicha1codi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public List<CroFicha1DetDTO> GetCroFicha1ByFilter(string plancodi, string empresa, string estado)
        {
            List<CroFicha1DetDTO> oblist = new List<CroFicha1DetDTO>();
            string query = $@"
                  SELECT CRONDET.*, CRON.PROYCODI, CRON.FECPUESTAOPE,
                  TR.EMPRESANOM, 
                  TR.PROYNOMBRE, 
                  TR.PROYDESCRIPCION, 
                  TR.PROYCONFIDENCIAL
                  FROM CAM_T3CROFICHA1 CRON
                 INNER JOIN CAM_T3CROFICHA1DET CRONDET ON CRON.CROFICHA1CODI = CRONDET.CROFICHA1CODI AND CRONDET.IND_DEL=0
                 INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CRON.PROYCODI
                 INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                    WHERE TR.PERICODI  IN ({plancodi}) AND 
                    PL.CODEMPRESA IN ({empresa})  AND 
                    CRON.IND_DEL = 0 AND 
                    PL.PLANESTADO ='{estado}'
                    ORDER BY TR.PERICODI, CRON.PROYCODI,PL.CODEMPRESA, CRON.CROFICHA1CODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CroFicha1DetDTO ob = new CroFicha1DetDTO();
                    ob.CroFicha1Detcodi = !dr.IsDBNull(dr.GetOrdinal("CROFICHA1DETCODI")) ? dr.GetInt32(dr.GetOrdinal("CROFICHA1DETCODI")) : 0;
                    ob.CroFicha1codi = !dr.IsDBNull(dr.GetOrdinal("CROFICHA1CODI")) ? dr.GetInt32(dr.GetOrdinal("CROFICHA1CODI")) : 0;
                    ob.Datacatcodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : string.Empty;
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetString(dr.GetOrdinal("PROYCODI")) : "";
                    ob.FecPuestaOpe = !dr.IsDBNull(dr.GetOrdinal("FECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("FECPUESTAOPE")) : (DateTime?)null;
                    ob.ProyNombre = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : "";
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
