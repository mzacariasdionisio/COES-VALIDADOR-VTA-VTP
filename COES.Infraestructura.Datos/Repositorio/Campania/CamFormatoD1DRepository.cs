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
    public class CamFormatoD1DRepository : RepositoryBase, ICamFormatoD1DRepository
    {
        public CamFormatoD1DRepository(string strConn) : base(strConn) { }

        CamFormatoD1DHelper Helper = new CamFormatoD1DHelper();

        public List<FormatoD1DDTO> GetFormatoD1DCodi(int proyCodi)
        {
            List<FormatoD1DDTO> formatoD1DDTOs = new List<FormatoD1DDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1DCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FormatoD1DDTO ob = new FormatoD1DDTO();
                    ob.FormatoD1DCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1DCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1DCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : "";
                    ob.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : "";
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    formatoD1DDTOs.Add(ob);
                }
            }
            return formatoD1DDTOs;
        }

        public bool SaveFormatoD1D(FormatoD1DDTO formatoD1DDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveFormatoD1D);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1DCODI", DbType.Int32, formatoD1DDTO.FormatoD1DCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, formatoD1DDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, formatoD1DDTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, formatoD1DDTO.DataCatCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, formatoD1DDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, formatoD1DDTO.Trimestre);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, formatoD1DDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, formatoD1DDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, formatoD1DDTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteFormatoD1DById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteFormatoD1DById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastFormatoD1DId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastFormatoD1DId);
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

        public List<FormatoD1DDTO> GetFormatoD1DById(int id)
        {
            List<FormatoD1DDTO> lista = new List<FormatoD1DDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1DById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FormatoD1DDTO ob = new FormatoD1DDTO();
                    ob.FormatoD1DCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1DCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1DCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : "";
                    ob.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : "";
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    lista.Add(ob);
                }
            }
            return lista;
        }

        public List<FormatoD1DDTO> GetFormatoD1DByFilter(string plancodi, string empresa, string estado)
        {
            List<FormatoD1DDTO> oblist = new List<FormatoD1DDTO>();
            string query = $@"
                  SELECT CGB.*, 
                  TR.EMPRESANOM, 
                  TR.PROYNOMBRE, 
                  TR.PROYDESCRIPCION, 
                  TP.TIPONOMBRE, 
                  TF.TIPOFINOMBRE,
                  TR.PROYCONFIDENCIAL

                  FROM CAM_FORMATOD1D CGB
                 
                 INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                 INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                 INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                 LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                    WHERE TR.PERICODI  IN ({plancodi}) AND 
                    PL.CODEMPRESA IN ({empresa})  AND 
                    CGB.IND_DEL = 0 AND 
                    PL.PLANESTADO ='{estado}'
                    ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.FORMATOD1DCODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FormatoD1DDTO ob = new FormatoD1DDTO();
                    ob.FormatoD1DCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1DCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1DCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : "";
                    ob.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : "";
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : "";
                    ob.ProyNombre = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : "";

                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    oblist.Add(ob);
                }
            }
            return oblist;
           
        }
    }
}
