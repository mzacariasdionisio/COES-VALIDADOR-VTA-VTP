using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RI_SOLICITUD
    /// </summary>
    public class RiSolicitudRepository : RepositoryBase, IRiSolicitudRepository
    {
        public RiSolicitudRepository(string strConn)
            : base(strConn)
        {
        }

        RiSolicitudHelper helper = new RiSolicitudHelper();

        public int Save(RiSolicitudDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Solicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Soliestado, DbType.String, entity.Soliestado);
            dbProvider.AddInParameter(command, helper.SoliestadoInterno, DbType.String, entity.SoliestadoInterno);
            dbProvider.AddInParameter(command, helper.Solienviado, DbType.String, entity.Solienviado);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Solifecsolicitud, DbType.DateTime, entity.Solifecsolicitud);
            dbProvider.AddInParameter(command, helper.Soliusucreacion, DbType.String, entity.Soliusucreacion);
            dbProvider.AddInParameter(command, helper.Solifeccreacion, DbType.DateTime, entity.Solifeccreacion);
            dbProvider.AddInParameter(command, helper.Soliusumodificacion, DbType.String, entity.Soliusumodificacion);
            dbProvider.AddInParameter(command, helper.Solifecmodificacion, DbType.DateTime, entity.Solifecmodificacion);
            dbProvider.AddInParameter(command, helper.Tisocodi, DbType.Int32, entity.Tisocodi);
            dbProvider.AddInParameter(command, helper.Solifecproceso, DbType.DateTime, entity.Solifecproceso);
            dbProvider.AddInParameter(command, helper.Soliususolicitud, DbType.Int32, entity.Soliususolicitud);
            dbProvider.AddInParameter(command, helper.Soliusuproceso, DbType.Int32, entity.Soliusuproceso);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RiSolicitudDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Soliestado, DbType.String, entity.Soliestado);
            dbProvider.AddInParameter(command, helper.Solienviado, DbType.String, entity.Solienviado);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Solifecsolicitud, DbType.DateTime, entity.Solifecsolicitud);
            dbProvider.AddInParameter(command, helper.Soliusucreacion, DbType.String, entity.Soliusucreacion);
            dbProvider.AddInParameter(command, helper.Solifeccreacion, DbType.DateTime, entity.Solifeccreacion);
            dbProvider.AddInParameter(command, helper.Soliusumodificacion, DbType.String, entity.Soliusumodificacion);
            dbProvider.AddInParameter(command, helper.Solifecmodificacion, DbType.DateTime, entity.Solifecmodificacion);
            dbProvider.AddInParameter(command, helper.Tisocodi, DbType.Int32, entity.Tisocodi);
            dbProvider.AddInParameter(command, helper.Solifecproceso, DbType.DateTime, entity.Solifecproceso);
            dbProvider.AddInParameter(command, helper.Soliususolicitud, DbType.Int32, entity.Soliususolicitud);
            dbProvider.AddInParameter(command, helper.Soliusuproceso, DbType.Int32, entity.Soliusuproceso);
            dbProvider.AddInParameter(command, helper.Solicodi, DbType.Int32, entity.Solicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int solicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Solicodi, DbType.Int32, solicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RiSolicitudDTO GetById(int solicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Solicodi, DbType.Int32, solicodi);
            RiSolicitudDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RiSolicitudDTO> List()
        {
            List<RiSolicitudDTO> entitys = new List<RiSolicitudDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<RiSolicitudDTO> GetByCriteria()
        {
            List<RiSolicitudDTO> entitys = new List<RiSolicitudDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        /// <summary>
        /// Devuelve un listado de solicitudes segun el estado
        /// </summary>
        /// <param name="soliestado">Estado de la solicitud</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<RiSolicitudDTO> ListPend(string soliestado, int nroPage, int pageSize)
        {
            List<RiSolicitudDTO> entitys = new List<RiSolicitudDTO>();
            String query = String.Format(helper.SqlListPend, soliestado, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreatePend(dr));
                }
            }

            return entitys;
        }


        /// <summary>
        /// Devuelve un listado de solicitudes segun el estado
        /// </summary>
        /// <param name="soliestado">Estado de la solicitud</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalListPend(string soliestado)
        {
            String query = String.Format(helper.SqlNroRegListPend, soliestado);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        /// <summary>
        /// Devuelve un listado de solicitudes segun el estado
        /// </summary>
        /// <param name="soliestado">Estado de la solicitud</param>
        /// <param name="emprcodi">codigo de empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<RiSolicitudDTO> ListPendporEmpresa(string soliestado, int nroPage, int pageSize, int emprcodi)
        {
            List<RiSolicitudDTO> entitys = new List<RiSolicitudDTO>();
            String query = String.Format(helper.SqlListPendporEmpresa, soliestado, nroPage, pageSize, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreatePend(dr));
                }
            }

            return entitys;
        }


        /// <summary>
        /// Devuelve un listado de solicitudes segun el estado
        /// </summary>
        /// <param name="soliestado">Estado de la solicitud</param>
        /// <param name="emprcodi">codigo de empresa</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalListPendporEmpresa(string soliestado, int emprcodi)
        {
            String query = String.Format(helper.SqlNroRegListPendporEmpresa, soliestado, emprcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public int DarConformidad(int solicodi)
        {
            String query = String.Format(helper.SqlDarConformidad, solicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int count = dbProvider.ExecuteNonQuery(command);
            return count;
        }

        public int DarNotificar(int solicodi)
        {
            String query = String.Format(helper.SqlDarNotificar, solicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int count = dbProvider.ExecuteNonQuery(command);
            return count;
        }


        public int FinalizarSolicitud(int solicodi, string estado, string observacion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlFinalizarSolicitud);

            dbProvider.AddInParameter(command, helper.SoliestadoInterno, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Soliobservacion, DbType.String, observacion);
            dbProvider.AddInParameter(command, helper.Solicodi, DbType.Int32, solicodi);

            int count = dbProvider.ExecuteNonQuery(command);
            return count;
        }

        public int ActualizarFechaProceso(int solicodi, DateTime fecha, int usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarFechaProcesoSolicitud);


            dbProvider.AddInParameter(command, helper.Solifecproceso, DbType.Date, fecha);
            dbProvider.AddInParameter(command, helper.Soliusuproceso, DbType.Int32, usuario);
            dbProvider.AddInParameter(command, helper.Solicodi, DbType.Int32, solicodi);

            int count = dbProvider.ExecuteNonQuery(command);
            return count;
        }

        public int SolicitudEnCurso(int emprcodi, int codigoTipoSolicitud)
        {
            String query = String.Format(helper.SqlSolicitudEnCurso, emprcodi, codigoTipoSolicitud);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }
    }
}
