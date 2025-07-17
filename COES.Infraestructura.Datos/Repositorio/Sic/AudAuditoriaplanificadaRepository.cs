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
    /// Clase de acceso a datos de la tabla AUD_AUDITORIAPLANIFICADA
    /// </summary>
    public class AudAuditoriaplanificadaRepository: RepositoryBase, IAudAuditoriaplanificadaRepository
    {
        public AudAuditoriaplanificadaRepository(string strConn): base(strConn)
        {
        }

        AudAuditoriaplanificadaHelper helper = new AudAuditoriaplanificadaHelper();

        public int Save(AudAuditoriaplanificadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Audpcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Plancodi, DbType.Int32, entity.Plancodi);
            dbProvider.AddInParameter(command, helper.Audpnombre, DbType.String, entity.Audpnombre);

            string codigo = string.Format("AUD-{0}-{1}", id.ToString().PadLeft(2, '0'), entity.Aniovigencia);
            dbProvider.AddInParameter(command, helper.Audpcodigo, DbType.String, codigo);

            dbProvider.AddInParameter(command, helper.Audpmesinicio, DbType.String, entity.Audpmesinicio);
            dbProvider.AddInParameter(command, helper.Audpmesfin, DbType.String, entity.Audpmesfin);
            dbProvider.AddInParameter(command, helper.Audpdactivo, DbType.String, entity.Audpdactivo);
            dbProvider.AddInParameter(command, helper.Audphistorico, DbType.String, entity.Audphistorico);
            dbProvider.AddInParameter(command, helper.Audpusucreacion, DbType.String, entity.Audpusucreacion);  
            dbProvider.AddInParameter(command, helper.Audpfeccreacion, DbType.DateTime, entity.Audpfeccreacion);
            dbProvider.AddInParameter(command, helper.Audpusumodificacion, DbType.String, entity.Audpusumodificacion);
            dbProvider.AddInParameter(command, helper.Audpfecmodificacion, DbType.DateTime, entity.Audpfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudAuditoriaplanificadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Plancodi, DbType.Int32, entity.Plancodi);
            dbProvider.AddInParameter(command, helper.Audpnombre, DbType.String, entity.Audpnombre);
            dbProvider.AddInParameter(command, helper.Audpcodigo, DbType.String, entity.Audpcodigo);
            dbProvider.AddInParameter(command, helper.Audpmesinicio, DbType.String, entity.Audpmesinicio);
            dbProvider.AddInParameter(command, helper.Audpmesfin, DbType.String, entity.Audpmesfin);
            dbProvider.AddInParameter(command, helper.Audphistorico, DbType.String, entity.Audphistorico);
            dbProvider.AddInParameter(command, helper.Audpusumodificacion, DbType.String, entity.Audpusumodificacion);
            dbProvider.AddInParameter(command, helper.Audpfecmodificacion, DbType.DateTime, entity.Audpfecmodificacion);
            dbProvider.AddInParameter(command, helper.Audpcodi, DbType.Int32, entity.Audpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(AudAuditoriaplanificadaDTO auditoriaplanificada)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Audpusumodificacion, DbType.String, auditoriaplanificada.Audpusumodificacion);
            dbProvider.AddInParameter(command, helper.Audpfecmodificacion, DbType.DateTime, auditoriaplanificada.Audpfecmodificacion);
            dbProvider.AddInParameter(command, helper.Audpcodi, DbType.Int32, auditoriaplanificada.Audpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByAudPlan(AudAuditoriaplanificadaDTO auditoriaPlanificada)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByAudPlan);

            dbProvider.AddInParameter(command, helper.Audpusumodificacion, DbType.String, auditoriaPlanificada.Audpusumodificacion);
            dbProvider.AddInParameter(command, helper.Audpfecmodificacion, DbType.DateTime, auditoriaPlanificada.Audpfecmodificacion);
            dbProvider.AddInParameter(command, helper.Plancodi, DbType.Int32, auditoriaPlanificada.Plancodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudAuditoriaplanificadaDTO GetById(int audpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Audpcodi, DbType.Int32, audpcodi);
            AudAuditoriaplanificadaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iExisteaudiproceso = dr.GetOrdinal(helper.Existeaudiproceso);
                    if (!dr.IsDBNull(iExisteaudiproceso)) entity.Existeaudiproceso = Convert.ToInt32(dr.GetValue(iExisteaudiproceso));
                }
            }

            return entity;
        }

        public List<AudAuditoriaplanificadaDTO> List()
        {
            List<AudAuditoriaplanificadaDTO> entitys = new List<AudAuditoriaplanificadaDTO>();
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

        public List<AudAuditoriaplanificadaDTO> GetByCriteria(int plancodi, string audphistorico)
        {
            List<AudAuditoriaplanificadaDTO> entitys = new List<AudAuditoriaplanificadaDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, plancodi, !string.IsNullOrEmpty(audphistorico) ? "and ap.audphistorico is null" : "");

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudAuditoriaplanificadaDTO auditoriaPlanificada = helper.Create(dr);

                    int iProcesos = dr.GetOrdinal(helper.Procesos);
                    if (!dr.IsDBNull(iProcesos)) auditoriaPlanificada.Procesos = dr.GetString(iProcesos);

                    int iProcesoareas = dr.GetOrdinal(helper.Procesoareas);
                    if (!dr.IsDBNull(iProcesoareas)) auditoriaPlanificada.ProcesoAreas = dr.GetString(iProcesoareas);

                    entitys.Add(auditoriaPlanificada);
                }
            }

            return entitys;
        }

        public string GetByAudPlanificadaValidacion(int audpcodi)
        {
            string validacionMensaje = string.Empty;

            string query = string.Format(helper.SqlGetByAudPlanificadaValidacion, audpcodi);

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
