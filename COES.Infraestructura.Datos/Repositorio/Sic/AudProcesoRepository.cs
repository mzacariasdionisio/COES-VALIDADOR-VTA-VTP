using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla AUD_PROCESO
    /// </summary>
    public class AudProcesoRepository: RepositoryBase, IAudProcesoRepository
    {
        public AudProcesoRepository(string strConn): base(strConn)
        {
        }

        AudProcesoHelper helper = new AudProcesoHelper();
        AreaHelper areaHelper = new AreaHelper();

        public int Save(AudProcesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Proccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Proccodigo, DbType.String, entity.Proccodigo);
            dbProvider.AddInParameter(command, helper.Procdescripcion, DbType.String, entity.Procdescripcion);
            dbProvider.AddInParameter(command, helper.Proctienesuperior, DbType.Int32, entity.Proctienesuperior);
            dbProvider.AddInParameter(command, helper.Procprocesosuperior, DbType.Int32, entity.Procprocesosuperior);
            dbProvider.AddInParameter(command, helper.Procactivo, DbType.String, entity.Procactivo);
            dbProvider.AddInParameter(command, helper.Prochistorico, DbType.String, entity.Prochistorico);
            dbProvider.AddInParameter(command, helper.Procusucreacion, DbType.String, entity.Procusucreacion);
            dbProvider.AddInParameter(command, helper.Procfeccreacion, DbType.DateTime, entity.Procfeccreacion);
            dbProvider.AddInParameter(command, helper.Procusumodificacion, DbType.String, entity.Procusumodificacion);
            dbProvider.AddInParameter(command, helper.Procfecmodificacion, DbType.DateTime, entity.Procfecmodificacion);
            dbProvider.AddInParameter(command, helper.Procsuperior, DbType.String, entity.Procsuperior);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudProcesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Proccodigo, DbType.String, entity.Proccodigo);
            dbProvider.AddInParameter(command, helper.Procdescripcion, DbType.String, entity.Procdescripcion);
            dbProvider.AddInParameter(command, helper.Proctienesuperior, DbType.Int32, entity.Proctienesuperior);
            dbProvider.AddInParameter(command, helper.Procprocesosuperior, DbType.Int32, entity.Procprocesosuperior);
            dbProvider.AddInParameter(command, helper.Prochistorico, DbType.String, entity.Prochistorico);
            dbProvider.AddInParameter(command, helper.Procusumodificacion, DbType.String, entity.Procusumodificacion);
            dbProvider.AddInParameter(command, helper.Procfecmodificacion, DbType.DateTime, entity.Procfecmodificacion);
            dbProvider.AddInParameter(command, helper.Procsuperior, DbType.String, entity.Procsuperior);
            dbProvider.AddInParameter(command, helper.Proccodi, DbType.Int32, entity.Proccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(AudProcesoDTO proceso)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Procusumodificacion, DbType.String, proceso.Procusumodificacion);
            dbProvider.AddInParameter(command, helper.Procfecmodificacion, DbType.DateTime, proceso.Procfecmodificacion);
            dbProvider.AddInParameter(command, helper.Proccodi, DbType.Int32, proceso.Proccodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public AudProcesoDTO GetById(int proccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Proccodi, DbType.Int32, proccodi);
            AudProcesoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iExisterelacion = dr.GetOrdinal(helper.Existerelacion);
                    if (!dr.IsDBNull(iExisterelacion)) entity.Existerelacion = Convert.ToInt32(dr.GetValue(iExisterelacion));
                }
            }

            return entity;
        }

        public AudProcesoDTO GetByCodigo(string proccodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCodigo);

            dbProvider.AddInParameter(command, helper.Proccodigo, DbType.String, proccodigo);
            AudProcesoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AudProcesoDTO> List()
        {
            List<AudProcesoDTO> entitys = new List<AudProcesoDTO>();
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

        public List<AudProcesoDTO> GetByCriteria(AudProcesoDTO proceso)
        {
            proceso.Procdescripcion = string.IsNullOrEmpty(proceso.Procdescripcion) ? "" : proceso.Procdescripcion.ToLower();

            List<AudProcesoDTO> entitys = new List<AudProcesoDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, proceso.Areacodi, proceso.Procdescripcion, proceso.nroPagina, proceso.nroFilas);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudProcesoDTO entity = helper.Create(dr);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iProcsuperiordescripcion = dr.GetOrdinal(helper.Procsuperiordescripcion);
                    if (!dr.IsDBNull(iProcsuperiordescripcion)) entity.Procsuperiordescripcion = dr.GetString(iProcsuperiordescripcion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroRegistrosBusqueda(AudProcesoDTO proceso)
        {
            proceso.Procdescripcion = string.IsNullOrEmpty(proceso.Procdescripcion) ? "" : proceso.Procdescripcion;

            string query = string.Format(helper.SqlObtenerNroRegistroBusqueda, proceso.Areacodi, proceso.Procdescripcion);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<AudProcesoDTO> ListProcesoSuperior(int proccodi, int areacodi)
        {
            List<AudProcesoDTO> entitys = new List<AudProcesoDTO>();
            string sql = string.Format(helper.SqlListProcesoSuperior, proccodi, areacodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<AudProcesoDTO> GetByProcesoPorEstado(string estado)
        {
            List<AudProcesoDTO> entitys = new List<AudProcesoDTO>();

            string sql = string.Format(helper.SqlGetByProcesoPorEstado,estado);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<AudProcesoDTO> GetByProcesoPorArea(string areacodi)
        {
            List<AudProcesoDTO> entitys = new List<AudProcesoDTO>();

            string query = string.Format(helper.SqlGetByProcesoPorArea, areacodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public string GetByProcesoValidacion(int proccodi, string procdescripcion = "")
        {
            string validacionMensaje = string.Empty;

            string query = string.Format(helper.SqlGetByProcesoValidacion, proccodi, procdescripcion);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iValidacionmensaje = dr.GetOrdinal(helper.Validacionmensaje);
                    if (!dr.IsDBNull(iValidacionmensaje)) validacionMensaje = dr.GetString(iValidacionmensaje);
                }
            }

            return validacionMensaje;
        }
    }
}
