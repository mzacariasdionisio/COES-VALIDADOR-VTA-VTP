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
    public class CamFormatoD1BRepository : RepositoryBase, ICamFormatoD1BRepository
    {
        public CamFormatoD1BRepository(string strConn) : base(strConn) { }

        CamFormatoD1BHelper Helper = new CamFormatoD1BHelper();

        public List<FormatoD1BDTO> GetFormatoD1BCodi(int proycodi)
        {
            List<FormatoD1BDTO> formatoD1BDTOs = new List<FormatoD1BDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1BCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proycodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FormatoD1BDTO ob = new FormatoD1BDTO();
                    ob.FormatoD1BCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1BCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1BCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.NombreCarga = !dr.IsDBNull(dr.GetOrdinal("NOMBRECARGA")) ? dr.GetString(dr.GetOrdinal("NOMBRECARGA")) : "";
                    ob.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : "";
                    ob.FechaIngreso = !dr.IsDBNull(dr.GetOrdinal("FECHAINGRESO")) ? dr.GetString(dr.GetOrdinal("FECHAINGRESO")) : "";
                    ob.BarraConexion = !dr.IsDBNull(dr.GetOrdinal("BARRACONEXION")) ? dr.GetString(dr.GetOrdinal("BARRACONEXION")) : "";
                    ob.NivelTension = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSION")) ? dr.GetString(dr.GetOrdinal("NIVELTENSION")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    formatoD1BDTOs.Add(ob);
                }
            }
            return formatoD1BDTOs;
        }

        public bool SaveFormatoD1B(FormatoD1BDTO formatoD1BDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveFormatoD1B);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1BCODI", DbType.Int32, formatoD1BDTO.FormatoD1BCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, formatoD1BDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "NOMBRECARGA", DbType.String, formatoD1BDTO.NombreCarga);
            dbProvider.AddInParameter(dbCommand, "PROPIETARIO", DbType.String, formatoD1BDTO.Propietario);
            dbProvider.AddInParameter(dbCommand, "FECHAINGRESO", DbType.String, formatoD1BDTO.FechaIngreso);
            dbProvider.AddInParameter(dbCommand, "BARRACONEXION", DbType.String, formatoD1BDTO.BarraConexion);
            dbProvider.AddInParameter(dbCommand, "NIVELTENSION", DbType.String, formatoD1BDTO.NivelTension);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, formatoD1BDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, formatoD1BDTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteFormatoD1BById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteFormatoD1BById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastFormatoD1BId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastFormatoD1BId);
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

        public FormatoD1BDTO GetFormatoD1BById(int id)
        {
            FormatoD1BDTO ob = new FormatoD1BDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1BById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.Int32, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ob.FormatoD1BCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1BCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1BCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.NombreCarga = !dr.IsDBNull(dr.GetOrdinal("NOMBRECARGA")) ? dr.GetString(dr.GetOrdinal("NOMBRECARGA")) : "";
                    ob.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : "";
                    ob.FechaIngreso = !dr.IsDBNull(dr.GetOrdinal("FECHAINGRESO")) ? dr.GetString(dr.GetOrdinal("FECHAINGRESO")) : "";
                    ob.BarraConexion = !dr.IsDBNull(dr.GetOrdinal("BARRACONEXION")) ? dr.GetString(dr.GetOrdinal("BARRACONEXION")) : "";
                    ob.NivelTension = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSION")) ? dr.GetString(dr.GetOrdinal("NIVELTENSION")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                }
            }
            return ob;
        }


        public List<FormatoD1BDTO> GetFormatoD1BByFilter(string plancodi, string empresa, string estado)
        {
            List<FormatoD1BDTO> oblist = new List<FormatoD1BDTO>();
            string query = $@"
                  SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  FROM CAM_FORMATOD1B CGB
                 INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                 INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                 INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                 LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                    WHERE TR.PERICODI  IN ({plancodi}) AND 
                    PL.CODEMPRESA IN ({empresa})  AND 
                    CGB.IND_DEL = 0 AND 
                    PL.PLANESTADO ='{estado}'
                    ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.FORMATOD1BCODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FormatoD1BDTO ob = new FormatoD1BDTO();
                    ob.FormatoD1BCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1BCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1BCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.NombreCarga = !dr.IsDBNull(dr.GetOrdinal("NOMBRECARGA")) ? dr.GetString(dr.GetOrdinal("NOMBRECARGA")) : "";
                    ob.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : "";
                    ob.FechaIngreso = !dr.IsDBNull(dr.GetOrdinal("FECHAINGRESO")) ? dr.GetString(dr.GetOrdinal("FECHAINGRESO")) : "";
                    ob.BarraConexion = !dr.IsDBNull(dr.GetOrdinal("BARRACONEXION")) ? dr.GetString(dr.GetOrdinal("BARRACONEXION")) : "";
                    ob.NivelTension = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSION")) ? dr.GetString(dr.GetOrdinal("NIVELTENSION")) : "";
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
