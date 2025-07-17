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
    public class CamObservacionRepository: RepositoryBase, ICamObservacionRepository
    {

        public CamObservacionRepository(string strConn) : base(strConn) { }

        CamObservacionHelper Helper = new CamObservacionHelper();

        public List<ObservacionDTO> GetObservacionByProyCodi(int proyCodi)
        {
            List<ObservacionDTO> ObservacionDTOs = new List<ObservacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetObservacionByProyCodi);
            dbProvider.AddInParameter(command, "IND_DELR", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ObservacionDTO ob = new ObservacionDTO();
                    ob.ObservacionId = !dr.IsDBNull(dr.GetOrdinal("CAM_OBSERVACIONID")) ? dr.GetInt32(dr.GetOrdinal("CAM_OBSERVACIONID")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.FechaObservacion = !dr.IsDBNull(dr.GetOrdinal("FECHA_OBSERVACION")) ? dr.GetDateTime(dr.GetOrdinal("FECHA_OBSERVACION")) : (DateTime?)null;
                    ob.Descripcion = !dr.IsDBNull(dr.GetOrdinal("DESCRIPCION")) ? dr.GetString(dr.GetOrdinal("DESCRIPCION")) : string.Empty;
                    ob.Estado = !dr.IsDBNull(dr.GetOrdinal("ESTADO")) ? dr.GetString(dr.GetOrdinal("ESTADO")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.FechaRespuesta = !dr.IsDBNull(dr.GetOrdinal("FECHA_RESPUESTA")) ? dr.GetDateTime(dr.GetOrdinal("FECHA_RESPUESTA")) : (DateTime?)null;
                    ob.Respuesta = !dr.IsDBNull(dr.GetOrdinal("RESPUESTA")) ? dr.GetString(dr.GetOrdinal("RESPUESTA")) : string.Empty;
                    ObservacionDTOs.Add(ob);
                }
            }

            return ObservacionDTOs;
        }

        public int SaveObservacion(ObservacionDTO observacionDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveObservacion);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.String, ObtenerValorOrDefault(observacionDTO.ProyCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "FECHA_OBSERVACION", DbType.DateTime, ObtenerValorOrDefault(DateTime.Now, typeof(DateTime)));
            dbProvider.AddInParameter(dbCommand, "DESCRIPCION", DbType.String, ObtenerValorOrDefault(observacionDTO.Descripcion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ESTADO", DbType.String, ObtenerValorOrDefault(observacionDTO.Estado, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(observacionDTO.UsuarioCreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(dbCommand);
            dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlGetLastObservacionId);
            object lastInsertedId = dbProvider.ExecuteScalar(dbCommand);
            return Convert.ToInt32(lastInsertedId);
        }

        public bool DeleteObservacionById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteObservacionById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastObservacionId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastObservacionId);
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

        public ObservacionDTO GetObservacionById(int id)
        {
            ObservacionDTO ob = new ObservacionDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetObservacionById);
            dbProvider.AddInParameter(commandHoja, "CAM_OBSERVACIONID", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.ObservacionId = !dr.IsDBNull(dr.GetOrdinal("CAM_OBSERVACIONID")) ? dr.GetInt32(dr.GetOrdinal("CAM_OBSERVACIONID")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.FechaObservacion = !dr.IsDBNull(dr.GetOrdinal("FECHA_OBSERVACION")) ? dr.GetDateTime(dr.GetOrdinal("FECHA_OBSERVACION")) : (DateTime?)null;
                    ob.Descripcion = !dr.IsDBNull(dr.GetOrdinal("DESCRIPCION")) ? dr.GetString(dr.GetOrdinal("DESCRIPCION")) : string.Empty;
                    ob.Estado = !dr.IsDBNull(dr.GetOrdinal("ESTADO")) ? dr.GetString(dr.GetOrdinal("ESTADO")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateObservacion(ObservacionDTO observacionDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateObservacion);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.String, ObtenerValorOrDefault(observacionDTO.ProyCodi, typeof(int)));            
            dbProvider.AddInParameter(dbCommand, "FECHA_OBSERVACION", DbType.DateTime,ObtenerValorOrDefault(observacionDTO.FechaObservacion, typeof(DateTime)));
            dbProvider.AddInParameter(dbCommand, "DESCRIPCION", DbType.String, ObtenerValorOrDefault(observacionDTO.Descripcion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ESTADO", DbType.String, ObtenerValorOrDefault(observacionDTO.Estado, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, observacionDTO.UsuarioModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, ObtenerValorOrDefault(DateTime.Now, typeof(DateTime)));
            dbProvider.AddInParameter(dbCommand, "CAM_OBSERVACIONID", DbType.Int32, ObtenerValorOrDefault(observacionDTO.ObservacionId, typeof(int)));
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool EnviarObservacionByProyecto(int idProyecto)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlEnviarObservacionByProyecto);
            dbProvider.AddInParameter(dbCommand, "ESTADO", DbType.String, "Enviada");
            dbProvider.AddInParameter(dbCommand, "ESTADOP", DbType.String, "Pendiente");
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, idProyecto);            
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool GetObservacionByPlanCodi(int planCodi)
        {
            List<ObservacionDTO> ObservacionDTOs = new List<ObservacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetObservacionByPlanCodi);
            dbProvider.AddInParameter(command, "PLANCODI", DbType.Int32, planCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (!dr.Read()) return true; 
                else return false;
            }

            return false;
        }

        public bool UpdateObservacionByProyecto(int idProyecto, string estado)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateObservacionByProyecto);
            dbProvider.AddInParameter(dbCommand, "ESTADO", DbType.String, estado);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, idProyecto);            
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
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

