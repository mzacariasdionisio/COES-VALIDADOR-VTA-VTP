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
    public class CamDetRegHojaDRepository : RepositoryBase, ICamDetRegHojaDRepository
    {

        public CamDetRegHojaDRepository(string strConn) : base(strConn) { }

        CamDetRegHojaDHelper Helper = new CamDetRegHojaDHelper();

        public List<DetRegHojaDDTO> GetDetRegHojaDFichaCCodi(string fichaccodi)
        {
            List<DetRegHojaDDTO> detRegHojaDDTOs = new List<DetRegHojaDDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetDetRegHojaDCodi);
            dbProvider.AddInParameter(command, "FICHADCODI", DbType.String, fichaccodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DetRegHojaDDTO ob = new DetRegHojaDDTO();
                    ob.Detreghdcodi = !dr.IsDBNull(dr.GetOrdinal("DETREGHDCODI")) ? dr.GetString(dr.GetOrdinal("DETREGHDCODI")) : "";
                    ob.Hojadcodi = !dr.IsDBNull(dr.GetOrdinal("HOJADCODI")) ? dr.GetString(dr.GetOrdinal("HOJADCODI")) : "";
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : string.Empty;
                    ob.Mes = !dr.IsDBNull(dr.GetOrdinal("MES")) ? dr.GetString(dr.GetOrdinal("MES")) : string.Empty;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VALOR")) : null;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    detRegHojaDDTOs.Add(ob);
                }

            }

            return detRegHojaDDTOs;
        }

        public bool SaveDetRegHojaD(DetRegHojaDDTO detRegHojaDDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveDetRegHojaD);
            dbProvider.AddInParameter(dbCommand, "DETAREGHDCODI", DbType.String, ObtenerValorOrDefault(detRegHojaDDTO.Detreghdcodi, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "HOJADCODI", DbType.String, ObtenerValorOrDefault(detRegHojaDDTO.Hojadcodi, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, ObtenerValorOrDefault(detRegHojaDDTO.Anio, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "MES", DbType.String, ObtenerValorOrDefault(detRegHojaDDTO.Mes, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.Decimal, detRegHojaDDTO.Valor ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(detRegHojaDDTO.Usucreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteDetRegHojaDById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteDetRegHojaDById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastDetRegHojaDId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastDetRegHojaDId);
            object result = dbProvider.ExecuteScalar(command);
            count = Convert.ToInt32(result) ;
            return count;
        }

        public DetRegHojaDDTO GetDetRegHojaDById(string id)
        {
            DetRegHojaDDTO ob = new DetRegHojaDDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetDetRegHojaDById);
            dbProvider.AddInParameter(commandHoja, "DETREGHDCODI", DbType.String, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Detreghdcodi = !dr.IsDBNull(dr.GetOrdinal("DETAREGHDCODI")) ? dr.GetString(dr.GetOrdinal("DETAREGHDCODI")) : "";
                    ob.Hojadcodi = !dr.IsDBNull(dr.GetOrdinal("HOJADCODI")) ? dr.GetString(dr.GetOrdinal("HOJADCODI")) : "";
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : string.Empty;
                    ob.Mes = !dr.IsDBNull(dr.GetOrdinal("MES")) ? dr.GetString(dr.GetOrdinal("MES")) : string.Empty;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VALOR")) : null;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateDetRegHojaD(DetRegHojaDDTO detRegHojaDDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateDetRegHojaD);
            dbProvider.AddInParameter(dbCommand, "HOJADCODI", DbType.Int32, detRegHojaDDTO.Hojadcodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, detRegHojaDDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "MES", DbType.String, detRegHojaDDTO.Mes);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.Decimal, detRegHojaDDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, detRegHojaDDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "DETAREGHDCODI", DbType.Int32, detRegHojaDDTO.Detreghdcodi);
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
                else if (tipo == typeof(Decimal) || tipo == typeof(Decimal?))
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
