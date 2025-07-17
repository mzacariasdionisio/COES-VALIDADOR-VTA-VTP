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
    public class CamFormatoD1CRepository : RepositoryBase, ICamFormatoD1CRepository
    {
        public CamFormatoD1CRepository(string strConn) : base(strConn) { }

        CamFormatoD1CHelper Helper = new CamFormatoD1CHelper();

        public List<FormatoD1CDTO> GetFormatoD1CCodi(int proycodi)
        {
            List<FormatoD1CDTO> formatoD1CDTOs = new List<FormatoD1CDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1CCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proycodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FormatoD1CDTO ob = new FormatoD1CDTO();
                    ob.FormatoD1CCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1CCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1CCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.PlanReducir = !dr.IsDBNull(dr.GetOrdinal("PLANREDUCIR")) ? dr.GetString(dr.GetOrdinal("PLANREDUCIR")) : "";
                    ob.Alternativa = !dr.IsDBNull(dr.GetOrdinal("ALTERNATIVA")) ? dr.GetString(dr.GetOrdinal("ALTERNATIVA")) : "";
                    ob.Otro = !dr.IsDBNull(dr.GetOrdinal("OTRO")) ? dr.GetString(dr.GetOrdinal("OTRO")) : "";
                   
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    formatoD1CDTOs.Add(ob);
                }
            }
            return formatoD1CDTOs;
        }

        public bool SaveFormatoD1C(FormatoD1CDTO formatoD1CDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveFormatoD1C);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1CCODI", DbType.Int32, formatoD1CDTO.FormatoD1CCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, formatoD1CDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "PLANREDUCIR", DbType.String, formatoD1CDTO.PlanReducir);
            dbProvider.AddInParameter(dbCommand, "ALTERNATIVA", DbType.String, formatoD1CDTO.Alternativa);
            dbProvider.AddInParameter(dbCommand, "OTRO", DbType.String, formatoD1CDTO.Otro);

            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, formatoD1CDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, formatoD1CDTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteFormatoD1CById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteFormatoD1CById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastFormatoD1CId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastFormatoD1CId);
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

        public FormatoD1CDTO GetFormatoD1CById(int id)
        {
            FormatoD1CDTO ob = new FormatoD1CDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1CById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.Int32, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ob.FormatoD1CCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1CCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1CCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.PlanReducir = !dr.IsDBNull(dr.GetOrdinal("PLANREDUCIR")) ? dr.GetString(dr.GetOrdinal("PLANREDUCIR")) : "";
                    ob.Alternativa = !dr.IsDBNull(dr.GetOrdinal("ALTERNATIVA")) ? dr.GetString(dr.GetOrdinal("ALTERNATIVA")) : "";
                    ob.Otro = !dr.IsDBNull(dr.GetOrdinal("OTRO")) ? dr.GetString(dr.GetOrdinal("OTRO")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                }
            }
            return ob;
        }


        public List<FormatoD1CDTO> GetFormatoD1CByFilter(string plancodi, string empresa, string estado)
        {
            List<FormatoD1CDTO> oblist = new List<FormatoD1CDTO>();
            string query = $@"
                  SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  FROM CAM_FORMATOD1C CGB
                 INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                 INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                 INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                 LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                    WHERE TR.PERICODI  IN ({plancodi}) AND 
                    PL.CODEMPRESA IN ({empresa})  AND 
                    CGB.IND_DEL = 0 AND 
                    PL.PLANESTADO ='{estado}'
                    ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.FORMATOD1CCODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FormatoD1CDTO ob = new FormatoD1CDTO();
                    ob.FormatoD1CCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1CCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1CCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.PlanReducir = !dr.IsDBNull(dr.GetOrdinal("PLANREDUCIR")) ? dr.GetString(dr.GetOrdinal("PLANREDUCIR")) : "";
                    ob.Alternativa = !dr.IsDBNull(dr.GetOrdinal("ALTERNATIVA")) ? dr.GetString(dr.GetOrdinal("ALTERNATIVA")) : "";
                    ob.Otro = !dr.IsDBNull(dr.GetOrdinal("OTRO")) ? dr.GetString(dr.GetOrdinal("OTRO")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    oblist.Add(ob);
                }
            }
            return oblist;
        }

    }
}
