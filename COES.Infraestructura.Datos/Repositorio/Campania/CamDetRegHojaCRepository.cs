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
    public class CamDetRegHojaCRepository: RepositoryBase, ICamDetRegHojaCRepository
    {

        public CamDetRegHojaCRepository(string strConn) : base(strConn){}

        CamDetRegHojaCHelper Helper = new CamDetRegHojaCHelper();

        public List<DetRegHojaCDTO> GetDetRegHojaCFichaCCodi(int fichaccodi)
        {
            List<DetRegHojaCDTO> detRegHojaCDTOs = new List<DetRegHojaCDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetDetRegHojaCFichaCCodi);
            dbProvider.AddInParameter(command, "FICHACCODI", DbType.String, fichaccodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DetRegHojaCDTO ob = new DetRegHojaCDTO();
                    ob.Detareghccodi = !dr.IsDBNull(dr.GetOrdinal("DETAREGHCCODI")) ? dr.GetInt32(dr.GetOrdinal("DETAREGHCCODI")) : 0;
                    ob.Fichaccodi = !dr.IsDBNull(dr.GetOrdinal("FICHACCODI")) ? dr.GetInt32(dr.GetOrdinal("FICHACCODI")) : 0;
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

        public bool SaveDetRegHojaC(DetRegHojaCDTO detRegHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveDetRegHojaC);
            dbProvider.AddInParameter(dbCommand, "DETAREGHCCODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaCDTO.Detareghccodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "FICHACCODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaCDTO.Fichaccodi, typeof(int)));
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

        public bool DeleteDetRegHojaCById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteDetRegHojaCById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastDetRegHojaCId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastDetRegHojaCId);
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

        public DetRegHojaCDTO GetDetRegHojaCById(int id)
        {
            DetRegHojaCDTO ob = new DetRegHojaCDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetDetRegHojaCById);
            dbProvider.AddInParameter(commandHoja, "DETAREGHCCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Detareghccodi = !dr.IsDBNull(dr.GetOrdinal("DETAREGHCCODI")) ? dr.GetInt32(dr.GetOrdinal("DETAREGHCCODI")) : 0;
                    ob.Fichaccodi = !dr.IsDBNull(dr.GetOrdinal("FICHACCODI")) ? dr.GetInt32(dr.GetOrdinal("FICHACCODI")) : 0;
                    ob.Datacatcodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : string.Empty;
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateDetRegHojaC(DetRegHojaCDTO detRegHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateDetRegHojaC);
            dbProvider.AddInParameter(dbCommand, "FICHACCODI", DbType.Int32, detRegHojaCDTO.Fichaccodi);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, detRegHojaCDTO.Datacatcodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, detRegHojaCDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, detRegHojaCDTO.Trimestre);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, detRegHojaCDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, detRegHojaCDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "DETAREGHCCODI", DbType.Int32, detRegHojaCDTO.Detareghccodi);
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
