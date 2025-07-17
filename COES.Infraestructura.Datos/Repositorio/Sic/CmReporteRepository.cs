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
    /// Clase de acceso a datos de la tabla CM_REPORTE
    /// </summary>
    public class CmReporteRepository: RepositoryBase, ICmReporteRepository
    {
        public CmReporteRepository(string strConn): base(strConn)
        {
        }

        CmReporteHelper helper = new CmReporteHelper();

        public int Save(CmReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmrepcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cmpercodi, DbType.Int32, entity.Cmpercodi);
            dbProvider.AddInParameter(command, helper.Cmurcodi, DbType.Int32, entity.Cmurcodi);
            dbProvider.AddInParameter(command, helper.Cmrepversion, DbType.Int32, entity.Cmrepversion);
            dbProvider.AddInParameter(command, helper.Cmrepfecha, DbType.DateTime, entity.Cmrepfecha);
            dbProvider.AddInParameter(command, helper.Cmrepestado, DbType.String, entity.Cmrepestado);
            dbProvider.AddInParameter(command, helper.Cmrepusucreacion, DbType.String, entity.Cmrepusucreacion);
            dbProvider.AddInParameter(command, helper.Cmrepfeccreacion, DbType.DateTime, entity.Cmrepfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmrepusumodificacion, DbType.String, entity.Cmrepusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmrepfecmodificacion, DbType.DateTime, entity.Cmrepfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmpercodi, DbType.Int32, entity.Cmpercodi);
            dbProvider.AddInParameter(command, helper.Cmurcodi, DbType.Int32, entity.Cmurcodi);
            dbProvider.AddInParameter(command, helper.Cmrepversion, DbType.Int32, entity.Cmrepversion);
            dbProvider.AddInParameter(command, helper.Cmrepfecha, DbType.DateTime, entity.Cmrepfecha);
            dbProvider.AddInParameter(command, helper.Cmrepestado, DbType.String, entity.Cmrepestado);
            dbProvider.AddInParameter(command, helper.Cmrepusucreacion, DbType.String, entity.Cmrepusucreacion);
            dbProvider.AddInParameter(command, helper.Cmrepfeccreacion, DbType.DateTime, entity.Cmrepfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmrepusumodificacion, DbType.String, entity.Cmrepusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmrepfecmodificacion, DbType.DateTime, entity.Cmrepfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cmrepcodi, DbType.Int32, entity.Cmrepcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmrepcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmrepcodi, DbType.Int32, cmrepcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmReporteDTO GetById(int cmrepcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmrepcodi, DbType.Int32, cmrepcodi);
            CmReporteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmReporteDTO> List()
        {
            List<CmReporteDTO> entitys = new List<CmReporteDTO>();
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

        public List<CmReporteDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin)
        {
            List<CmReporteDTO> entitys = new List<CmReporteDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmReporteDTO entity = helper.Create(dr);

                    entity.FechaReporte = (entity.Cmrepfecha != null) ?
                        ((DateTime)entity.Cmrepfecha).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entity.FechaModificacion = (entity.Cmrepfecmodificacion != null) ?
                        ((DateTime)entity.Cmrepfecmodificacion).ToString(ConstantesBase.FormatFechaFull) : string.Empty;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroVersion(DateTime fecha)
        {
            string sql = string.Format(helper.SqlObtenerNroVersion, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                int count = Convert.ToInt32(result);

                return count + 1;
            }

            return 1;
        }
    }
}
