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
    public class CamCroFicha1DetRepository : RepositoryBase, ICamCroFicha1DetRepository
    {

        public CamCroFicha1DetRepository(string strConn) : base(strConn) { }

        CamCroFicha1DetHelper Helper = new CamCroFicha1DetHelper();

        public List<CroFicha1DetDTO> GetCroFicha1DetCodi(int fichaccodi)
        {
            List<CroFicha1DetDTO> detRegHojaCDTOs = new List<CroFicha1DetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCroFicha1DetProyCodi);
            dbProvider.AddInParameter(command, "CROFICHA1CODI", DbType.String, fichaccodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
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
                    detRegHojaCDTOs.Add(ob);
                }
            }

            return detRegHojaCDTOs;
        }

        public bool SaveCroFicha1Det(CroFicha1DetDTO detRegHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCroFicha1Det);
            dbProvider.AddInParameter(dbCommand, "CROFICHA1DETCODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaCDTO.CroFicha1Detcodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "CROFICHA1CODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaCDTO.CroFicha1codi, typeof(int)));
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

        public bool DeleteCroFicha1DetById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCroFicha1DetById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastCroFicha1DetId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCroFicha1DetId);
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

        public CroFicha1DetDTO GetCroFicha1DetById(int id)
        {
            CroFicha1DetDTO ob = new CroFicha1DetDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetCroFicha1DetById);
            dbProvider.AddInParameter(commandHoja, "CROFICHA1DETCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.CroFicha1Detcodi = !dr.IsDBNull(dr.GetOrdinal("CROFICHA1DETCODI")) ? dr.GetInt32(dr.GetOrdinal("CROFICHA1DETCODI")) : 0;
                    ob.CroFicha1codi = !dr.IsDBNull(dr.GetOrdinal("CROFICHA1CODI")) ? dr.GetInt32(dr.GetOrdinal("CROFICHA1CODI")) : 0;
                    ob.Datacatcodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : string.Empty;
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateCroFicha1Det(CroFicha1DetDTO detRegHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCroFicha1Det);
            dbProvider.AddInParameter(dbCommand, "CROFICHA1CODI", DbType.Int32, detRegHojaCDTO.CroFicha1codi);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, detRegHojaCDTO.Datacatcodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, detRegHojaCDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, detRegHojaCDTO.Trimestre);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, detRegHojaCDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, detRegHojaCDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "CROFICHA1DETCODI", DbType.Int32, detRegHojaCDTO.CroFicha1Detcodi);
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
