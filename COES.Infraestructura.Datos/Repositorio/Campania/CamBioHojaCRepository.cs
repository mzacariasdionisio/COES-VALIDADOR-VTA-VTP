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
    public class CamBioHojaCRepository : RepositoryBase, ICamBioHojaCRepository
    {

        public CamBioHojaCRepository(string strConn) : base(strConn) { }

        CamBioHojaCHelper Helper = new CamBioHojaCHelper();

        public List<BioHojaCDTO> GetBioHojaCProyCodi(int proyCodi)
        {
            List<BioHojaCDTO> regHojaCDTOs = new List<BioHojaCDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetBioHojaCProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.String, proyCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BioHojaCDTO ob = new BioHojaCDTO();
                    ob.Biohojaccodi = !dr.IsDBNull(dr.GetOrdinal("BIOHOJACCODI")) ? dr.GetInt32(dr.GetOrdinal("BIOHOJACCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Fecpuestaope = !dr.IsDBNull(dr.GetOrdinal("FECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("FECPUESTAOPE")) : (DateTime?)null;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    regHojaCDTOs.Add(ob);
                }
            }

            return regHojaCDTOs;
        }

        public bool SaveBioHojaC(BioHojaCDTO regHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveBioHojaC);
            dbProvider.AddInParameter(dbCommand, "BIOHOJACCODI", DbType.Int32, ObtenerValorOrDefault(regHojaCDTO.Biohojaccodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(regHojaCDTO.Proycodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "FECPUESTAOPE", DbType.DateTime,regHojaCDTO.Fecpuestaope);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(regHojaCDTO.Usucreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteBioHojaCById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteBioHojaCById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastBioHojaCId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastBioHojaCId);
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

        public BioHojaCDTO GetBioHojaCById(int id)
        {
            BioHojaCDTO ob = new BioHojaCDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetBioHojaCById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Biohojaccodi = !dr.IsDBNull(dr.GetOrdinal("BIOHOJACCODI")) ? dr.GetInt32(dr.GetOrdinal("BIOHOJACCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Fecpuestaope = !dr.IsDBNull(dr.GetOrdinal("FECPUESTAOPE")) ? dr.GetDateTime(dr.GetOrdinal("FECPUESTAOPE")) : (DateTime?)null;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateBioHojaC(BioHojaCDTO regHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateBioHojaC);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, regHojaCDTO.Proycodi);
            dbProvider.AddInParameter(dbCommand, "FECPUESTAOPE", DbType.DateTime, regHojaCDTO.Fecpuestaope);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, regHojaCDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "BIOHOJACCODI", DbType.Int32, regHojaCDTO.Biohojaccodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
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


        public List<DetBioHojaCDTO> GetBioHojaCByFilter(string plancodi, string empresa, string estado)
        {
            List<DetBioHojaCDTO> oblist = new List<DetBioHojaCDTO>();
            string query = $@"
                SELECT CRONDET.*, CRON.PROYCODI,
                TR.EMPRESANOM, 
                TR.PROYNOMBRE, 
                TR.PROYDESCRIPCION, 
                TR.PROYCONFIDENCIAL
                FROM CAM_BIOHOJAC CRON
                INNER JOIN CAM_BIOHOJACDET CRONDET ON CRON.BIOHOJACCODI = CRONDET.BIOHOJACCODI AND CRONDET.IND_DEL=0
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CRON.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CRON.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PLANCODI, CRON.PROYCODI,PL.CODEMPRESA, CRON.BIOHOJACCODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    DetBioHojaCDTO ob = new DetBioHojaCDTO();
                    ob.Biohojaccodi = !dr.IsDBNull(dr.GetOrdinal("BIOHOJACCODI")) ? dr.GetInt32(dr.GetOrdinal("BIOHOJACCODI")) : 0;
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
    }
}
