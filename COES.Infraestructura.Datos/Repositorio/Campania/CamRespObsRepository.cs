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
    public class CamRespuestaObsRepository: RepositoryBase, ICamRespObsRepository
    {

        public CamRespuestaObsRepository(string strConn) : base(strConn) { }

        CamRespObsHelper Helper = new CamRespObsHelper();

        public List<RespuestaObsDTO> GetRespuestaObsByObs(int observacion)
        {
            List<RespuestaObsDTO> RespuestaObsDTOs = new List<RespuestaObsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetRespuestaObsByObs);
            dbProvider.AddInParameter(command, "CAM_OBSERVACIONID", DbType.Int32, observacion);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RespuestaObsDTO ob = new RespuestaObsDTO();
                    ob.RespuestaId = !dr.IsDBNull(dr.GetOrdinal("CAM_RESPUESTAID")) ? dr.GetInt32(dr.GetOrdinal("CAM_RESPUESTAID")) : 0;
                    ob.ObservacionId = !dr.IsDBNull(dr.GetOrdinal("CAM_OBSERVACIONID")) ? dr.GetInt32(dr.GetOrdinal("CAM_OBSERVACIONID")) : 0;
                    ob.FechaRespuesta = !dr.IsDBNull(dr.GetOrdinal("FECHA_RESPUESTA")) ? dr.GetDateTime(dr.GetOrdinal("FECHA_RESPUESTA")) : (DateTime?)null;
                    ob.Respuesta = !dr.IsDBNull(dr.GetOrdinal("RESPUESTA")) ? dr.GetString(dr.GetOrdinal("RESPUESTA")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    RespuestaObsDTOs.Add(ob);
                }
            }

            return RespuestaObsDTOs;
        }

        public bool SaveRespuestaObs(RespuestaObsDTO respuestaObsDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveRespuestaObs);
            dbProvider.AddInParameter(dbCommand, "CAM_OBSERVACIONID", DbType.Int32, ObtenerValorOrDefault(respuestaObsDTO.ObservacionId, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "FECHA_RESPUESTA", DbType.DateTime, ObtenerValorOrDefault(DateTime.Now, typeof(DateTime)));
            dbProvider.AddInParameter(dbCommand, "RESPUESTA", DbType.String, ObtenerValorOrDefault(respuestaObsDTO.Respuesta, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(respuestaObsDTO.UsuarioCreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteRespuestaObsById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteRespuestaObsById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastRespuestaObsId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastRespuestaObsId);
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

        public RespuestaObsDTO GetRespuestaObsById(int id)
        {
            RespuestaObsDTO ob = new RespuestaObsDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetRespuestaObsById);
            dbProvider.AddInParameter(commandHoja, "CAM_RESPUESTAID", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.RespuestaId = !dr.IsDBNull(dr.GetOrdinal("CAM_RESPUESTAID")) ? dr.GetInt32(dr.GetOrdinal("CAM_RESPUESTAID")) : 0;
                    ob.ObservacionId = !dr.IsDBNull(dr.GetOrdinal("CAM_OBSERVACIONID")) ? dr.GetInt32(dr.GetOrdinal("CAM_OBSERVACIONID")) : 0;
                    ob.FechaRespuesta = !dr.IsDBNull(dr.GetOrdinal("FECHA_RESPUESTA")) ? dr.GetDateTime(dr.GetOrdinal("FECHA_RESPUESTA")) : (DateTime?)null;
                    ob.Respuesta = !dr.IsDBNull(dr.GetOrdinal("RESPUESTA")) ? dr.GetString(dr.GetOrdinal("RESPUESTA")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateRespuestaObs(RespuestaObsDTO respuestaObsDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateRespuestaObs);
            dbProvider.AddInParameter(dbCommand, "CAM_OBSERVACIONID", DbType.Int32, ObtenerValorOrDefault(respuestaObsDTO.ObservacionId, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "FECHA_RESPUESTA", DbType.DateTime, respuestaObsDTO.FechaRespuesta);
            dbProvider.AddInParameter(dbCommand, "RESPUESTA", DbType.String, ObtenerValorOrDefault(respuestaObsDTO.Respuesta, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, respuestaObsDTO.UsuarioModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "CAM_RESPUESTAID", DbType.Int32, ObtenerValorOrDefault(respuestaObsDTO.RespuestaId, typeof(int)));
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

