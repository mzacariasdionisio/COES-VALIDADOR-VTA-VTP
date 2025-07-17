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
    public class CamFormatoD1ADet5Repository : RepositoryBase, ICamFormatoD1ADET5Repository
    {
        public CamFormatoD1ADet5Repository(string strConn) : base(strConn) { }

        CamFormatoD1ADet5Helper Helper = new CamFormatoD1ADet5Helper();

        public List<FormatoD1ADet5DTO> GetFormatoD1ADET5Codi(int formatoD1ACodi)
        {
            List<FormatoD1ADet5DTO> formatoD1ADet5DTOs = new List<FormatoD1ADet5DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1ADet5Codi);
            dbProvider.AddInParameter(command, "FORMATOD1ACODI", DbType.Int32, formatoD1ACodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FormatoD1ADet5DTO ob = new FormatoD1ADet5DTO();
                    ob.FormatoD1ADet5Codi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1ADET5CODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1ADET5CODI")) : 0;
                    ob.FormatoD1ACodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1ACODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1ACODI")) : 0;
                    ob.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.EnElaboracion = !dr.IsDBNull(dr.GetOrdinal("ENELABORACION")) ? dr.GetString(dr.GetOrdinal("ENELABORACION")) : "";
                    ob.Presentado = !dr.IsDBNull(dr.GetOrdinal("PRESENTADO")) ? dr.GetString(dr.GetOrdinal("PRESENTADO")) : "";
                    ob.EnTramite = !dr.IsDBNull(dr.GetOrdinal("ENTRAMITE")) ? dr.GetString(dr.GetOrdinal("ENTRAMITE")) : "";
                    ob.Aprobado = !dr.IsDBNull(dr.GetOrdinal("APROBADO")) ? dr.GetString(dr.GetOrdinal("APROBADO")) : "";
                    ob.Firmado = !dr.IsDBNull(dr.GetOrdinal("FIRMADO")) ? dr.GetString(dr.GetOrdinal("FIRMADO")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    formatoD1ADet5DTOs.Add(ob);
                }
            }
            return formatoD1ADet5DTOs;
        }

        public bool SaveFormatoD1ADET5(FormatoD1ADet5DTO formatoD1ADet5DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveFormatoD1ADet5);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1ADET5CODI", DbType.Int32, formatoD1ADet5DTO.FormatoD1ADet5Codi);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1ACODI", DbType.Int32, formatoD1ADet5DTO.FormatoD1ACodi);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, formatoD1ADet5DTO.DataCatCodi);
            dbProvider.AddInParameter(dbCommand, "ENELABORACION", DbType.String, formatoD1ADet5DTO.EnElaboracion);
            dbProvider.AddInParameter(dbCommand, "PRESENTADO", DbType.String, formatoD1ADet5DTO.Presentado);
            dbProvider.AddInParameter(dbCommand, "ENTRAMITE", DbType.String, formatoD1ADet5DTO.EnTramite);
            dbProvider.AddInParameter(dbCommand, "APROBADO", DbType.String, formatoD1ADet5DTO.Aprobado);
            dbProvider.AddInParameter(dbCommand, "FIRMADO", DbType.String, formatoD1ADet5DTO.Firmado);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, formatoD1ADet5DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, formatoD1ADet5DTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteFormatoD1ADET5ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteFormatoD1ADet5ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastFormatoD1ADET5Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastFormatoD1ADet5Id);
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

        public FormatoD1ADet5DTO GetFormatoD1ADET5ById(int id)
        {
            FormatoD1ADet5DTO ob = new FormatoD1ADet5DTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1ADet5ById);
            dbProvider.AddInParameter(command, "FORMATOD1ADET5CODI", DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ob.FormatoD1ADet5Codi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1ADET5CODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1ADET5CODI")) : 0;
                    ob.FormatoD1ACodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1ACODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1ACODI")) : 0;
                    ob.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.EnElaboracion = !dr.IsDBNull(dr.GetOrdinal("ENELABORACION")) ? dr.GetString(dr.GetOrdinal("ENELABORACION")) : "";
                    ob.Presentado = !dr.IsDBNull(dr.GetOrdinal("PRESENTADO")) ? dr.GetString(dr.GetOrdinal("PRESENTADO")) : "";
                    ob.EnTramite = !dr.IsDBNull(dr.GetOrdinal("ENTRAMITE")) ? dr.GetString(dr.GetOrdinal("ENTRAMITE")) : "";
                    ob.Aprobado = !dr.IsDBNull(dr.GetOrdinal("APROBADO")) ? dr.GetString(dr.GetOrdinal("APROBADO")) : "";
                    ob.Firmado = !dr.IsDBNull(dr.GetOrdinal("FIRMADO")) ? dr.GetString(dr.GetOrdinal("FIRMADO")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                }
            }
            return ob;
        }
    }
}
