using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Framework.Base.Tools;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla AUD_PLANAUDITORIA
    /// </summary>
    public class AudPlanauditoriaRepository: RepositoryBase, IAudPlanauditoriaRepository
    {
        public AudPlanauditoriaRepository(string strConn): base(strConn)
        {
        }

        AudPlanauditoriaHelper helper = new AudPlanauditoriaHelper();

        public int Save(AudPlanauditoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Plancodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Planano, DbType.String, entity.Planano);
            dbProvider.AddInParameter(command, helper.Plananovigencia, DbType.String, entity.Plananovigencia);
            dbProvider.AddInParameter(command, helper.Planactivo, DbType.String, entity.Planactivo);
            dbProvider.AddInParameter(command, helper.Planhistorico, DbType.String, entity.Planhistorico);
            dbProvider.AddInParameter(command, helper.Planusucreacion, DbType.String, entity.Planusucreacion);
            dbProvider.AddInParameter(command, helper.Planfeccreacion, DbType.DateTime, entity.Planfeccreacion);
            dbProvider.AddInParameter(command, helper.Planusumodificacion, DbType.String, entity.Planusumodificacion);
            dbProvider.AddInParameter(command, helper.Planfecmodificacion, DbType.DateTime, entity.Planfecmodificacion);

            string codigo = string.Format("PAA-{0}", entity.Plananovigencia);
            dbProvider.AddInParameter(command, helper.Plancodigo, DbType.String, codigo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudPlanauditoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Planano, DbType.String, entity.Planano);
            dbProvider.AddInParameter(command, helper.Plananovigencia, DbType.String, entity.Plananovigencia);
            dbProvider.AddInParameter(command, helper.Planhistorico, DbType.String, entity.Planhistorico);
            dbProvider.AddInParameter(command, helper.Planusucreacion, DbType.String, entity.Planusucreacion);
            dbProvider.AddInParameter(command, helper.Planfeccreacion, DbType.DateTime, entity.Planfeccreacion);
            dbProvider.AddInParameter(command, helper.Planusumodificacion, DbType.String, entity.Planusumodificacion);
            dbProvider.AddInParameter(command, helper.Planfecmodificacion, DbType.DateTime, entity.Planfecmodificacion);
            dbProvider.AddInParameter(command, helper.Plancodi, DbType.Int32, entity.Plancodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(AudPlanauditoriaDTO planAuditoria)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Planusumodificacion, DbType.String, planAuditoria.Planusumodificacion);
            dbProvider.AddInParameter(command, helper.Planfecmodificacion, DbType.DateTime, planAuditoria.Planfecmodificacion);
            dbProvider.AddInParameter(command, helper.Plancodi, DbType.Int32, planAuditoria.Plancodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudPlanauditoriaDTO GetById(int plancodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Plancodi, DbType.Int32, plancodi);
            AudPlanauditoriaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AudPlanauditoriaDTO> List()
        {
            List<AudPlanauditoriaDTO> entitys = new List<AudPlanauditoriaDTO>();
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

        public List<AudPlanauditoriaDTO> GetByCriteria(AudPlanauditoriaDTO planAuditoria)
        {
            string sql = string.Format(helper.SqlGetByCriteria, planAuditoria.Planactivo, planAuditoria.Plananovigencia, planAuditoria.nroPagina, planAuditoria.nroFilas);

            List<AudPlanauditoriaDTO> entitys = new List<AudPlanauditoriaDTO>();
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

        public int ObtenerNroRegistrosBusqueda(AudPlanauditoriaDTO planAuditoria)
        {
            string query = string.Format(helper.SqlObtenerNroRegistroBusqueda, planAuditoria.Planactivo, planAuditoria.Plananovigencia);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public string GetByPlanValidacion(int plancodi)
        {
            string validacionMensaje = string.Empty;

            string query = string.Format(helper.SqlGetByPlanValidacion, plancodi);

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
