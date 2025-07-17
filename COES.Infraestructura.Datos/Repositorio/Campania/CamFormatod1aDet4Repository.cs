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
    public class CamFormatoD1ADet4Repository : RepositoryBase, ICamFormatoD1ADET4Repository
    {
        public CamFormatoD1ADet4Repository(string strConn) : base(strConn) { }

        CamFormatoD1ADet4Helper Helper = new CamFormatoD1ADet4Helper();

        public List<FormatoD1ADet4DTO> GetFormatoD1ADET4Codi(int formatod1aCodi)
        {
            List<FormatoD1ADet4DTO> formatod1aDet4DTOs = new List<FormatoD1ADet4DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1ADet4Codi);
            dbProvider.AddInParameter(command, "FORMATOD1ACODI", DbType.Int32, formatod1aCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FormatoD1ADet4DTO ob = new FormatoD1ADet4DTO();
                    ob.FormatoD1ADet4Codi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1ADET4CODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1ADET4CODI")) : 0;
                    ob.FormatoD1ACodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1ACODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1ACODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.MontoInversion = !dr.IsDBNull(dr.GetOrdinal("MONTOINVERSION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("MONTOINVERSION")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    formatod1aDet4DTOs.Add(ob);
                }
            }
            return formatod1aDet4DTOs;
        }

        public bool SaveFormatoD1ADET4(FormatoD1ADet4DTO formatod1aDet4DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveFormatoD1ADet4);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1ADET4CODI", DbType.Int32, formatod1aDet4DTO.FormatoD1ADet4Codi);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1ACODI", DbType.Int32, formatod1aDet4DTO.FormatoD1ACodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, formatod1aDet4DTO.Anio);
            dbProvider.AddInParameter(dbCommand, "MONTOINVERSION", DbType.Decimal, formatod1aDet4DTO.MontoInversion);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, formatod1aDet4DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, formatod1aDet4DTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteFormatoD1ADET4ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteFormatoD1ADet4ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastFormatoD1ADET4Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastFormatoD1ADet4Id);
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

        public FormatoD1ADet4DTO GetFormatoD1ADET4ById(int id)
        {
            FormatoD1ADet4DTO ob = new FormatoD1ADet4DTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1ADet4ById);
            dbProvider.AddInParameter(commandHoja, "FORMATOD1ADET4CODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.FormatoD1ADet4Codi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1ADET4CODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1ADET4CODI")) : 0;
                    ob.FormatoD1ACodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1ACODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1ACODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.MontoInversion = !dr.IsDBNull(dr.GetOrdinal("MONTOINVERSION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("MONTOINVERSION")) : null;
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
