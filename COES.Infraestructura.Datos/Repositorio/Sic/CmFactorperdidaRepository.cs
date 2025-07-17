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
    /// Clase de acceso a datos de la tabla CM_FACTORPERDIDA
    /// </summary>
    public class CmFactorperdidaRepository: RepositoryBase, ICmFactorperdidaRepository
    {
        public CmFactorperdidaRepository(string strConn): base(strConn)
        {
        }

        CmFactorperdidaHelper helper = new CmFactorperdidaHelper();

        public int Save(CmFactorperdidaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmfpmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cmpercodi, DbType.Int32, entity.Cmpercodi);
            dbProvider.AddInParameter(command, helper.Cmfpmfecha, DbType.DateTime, entity.Cmfpmfecha);
            dbProvider.AddInParameter(command, helper.Cmfpmestado, DbType.String, entity.Cmfpmestado);
            dbProvider.AddInParameter(command, helper.Cmfpmusucreacion, DbType.String, entity.Cmfpmusucreacion);
            dbProvider.AddInParameter(command, helper.Cmfpmfeccreacion, DbType.DateTime, entity.Cmfpmfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmfpmusumodificacion, DbType.String, entity.Cmfpmusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmfpmfecmodificacion, DbType.DateTime, entity.Cmfpmfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmFactorperdidaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmpercodi, DbType.Int32, entity.Cmpercodi);
            dbProvider.AddInParameter(command, helper.Cmfpmfecha, DbType.DateTime, entity.Cmfpmfecha);
            dbProvider.AddInParameter(command, helper.Cmfpmestado, DbType.String, entity.Cmfpmestado);
            dbProvider.AddInParameter(command, helper.Cmfpmusucreacion, DbType.String, entity.Cmfpmusucreacion);
            dbProvider.AddInParameter(command, helper.Cmfpmfeccreacion, DbType.DateTime, entity.Cmfpmfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmfpmusumodificacion, DbType.String, entity.Cmfpmusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmfpmfecmodificacion, DbType.DateTime, entity.Cmfpmfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cmfpmcodi, DbType.Int32, entity.Cmfpmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmfpmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmfpmcodi, DbType.Int32, cmfpmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmFactorperdidaDTO GetById(int cmfpmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmfpmcodi, DbType.Int32, cmfpmcodi);
            CmFactorperdidaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmFactorperdidaDTO> List()
        {
            List<CmFactorperdidaDTO> entitys = new List<CmFactorperdidaDTO>();
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

        public List<CmFactorperdidaDTO> GetByCriteria(DateTime fecha)
        {
            List<CmFactorperdidaDTO> entitys = new List<CmFactorperdidaDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {                    
                    CmFactorperdidaDTO entity = helper.Create(dr);                   

                    entity.FechaModificacion = (entity.Cmfpmfecmodificacion != null) ?
                    ((DateTime)entity.Cmfpmfecmodificacion).ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;

                    entity.FechaDatos = (entity.Cmfpmfecha != null) ?
                       ((DateTime)entity.Cmfpmfecha).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
