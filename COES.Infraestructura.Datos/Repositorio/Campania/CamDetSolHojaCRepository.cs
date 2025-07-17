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
    public class CamDetSolHojaCRepository : RepositoryBase, ICamDetSolHojaCRepository
    {

        public CamDetSolHojaCRepository(string strConn) : base(strConn) { }

        CamDetSolHojaCHelper Helper = new CamDetSolHojaCHelper();
         
        public List<DetSolHojaCDTO> GetDetSolHojaCCodi(int fichaccodi)
        {
            List<DetSolHojaCDTO> detRegHojaCDTOs = new List<DetSolHojaCDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetDetSolHojaCProyCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(command, "SOLHOJACCODI", DbType.String, fichaccodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DetSolHojaCDTO ob = new DetSolHojaCDTO();
                    ob.Detasolhccodi = !dr.IsDBNull(dr.GetOrdinal("DETASOLHCCODI")) ? dr.GetInt32(dr.GetOrdinal("DETASOLHCCODI")) : 0;
                    ob.Solhojaccodi = !dr.IsDBNull(dr.GetOrdinal("SOLHOJACCODI")) ? dr.GetInt32(dr.GetOrdinal("SOLHOJACCODI")) : 0;
                    ob.Datacatcodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : string.Empty;
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    detRegHojaCDTOs.Add(ob);
                }
            }

            return detRegHojaCDTOs;
        }

        public bool SaveDetSolHojaC(DetSolHojaCDTO detRegHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveDetSolHojaC);
            dbProvider.AddInParameter(dbCommand, "DETASOLHCCODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaCDTO.Detasolhccodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "SOLHOJACCODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaCDTO.Solhojaccodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaCDTO.Datacatcodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, ObtenerValorOrDefault(detRegHojaCDTO.Anio, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, ObtenerValorOrDefault(detRegHojaCDTO.Trimestre, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, ObtenerValorOrDefault(detRegHojaCDTO.Valor, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(detRegHojaCDTO.Usucreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteDetSolHojaCById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteDetSolHojaCById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastDetSolHojaCId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastDetSolHojaCId);
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

        public DetSolHojaCDTO GetDetSolHojaCById(int id)
        {
            DetSolHojaCDTO ob = new DetSolHojaCDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetDetSolHojaCById);
            dbProvider.AddInParameter(commandHoja, "DETASOLHCCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Detasolhccodi = !dr.IsDBNull(dr.GetOrdinal("DETASOLHCCODI")) ? dr.GetInt32(dr.GetOrdinal("DETASOLHCCODI")) : 0;
                    ob.Solhojaccodi = !dr.IsDBNull(dr.GetOrdinal("SOLHOJACCODI")) ? dr.GetInt32(dr.GetOrdinal("SOLHOJACCODI")) : 0;
                    ob.Datacatcodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : string.Empty;
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateDetSolHojaC(DetSolHojaCDTO detRegHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateDetSolHojaC);
            dbProvider.AddInParameter(dbCommand, "SOLHOJACCODI", DbType.Int32, detRegHojaCDTO.Solhojaccodi);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, detRegHojaCDTO.Datacatcodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, detRegHojaCDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, detRegHojaCDTO.Trimestre);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, detRegHojaCDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, detRegHojaCDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "DETASOLHCCODI", DbType.Int32, detRegHojaCDTO.Detasolhccodi);
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

    }
}
