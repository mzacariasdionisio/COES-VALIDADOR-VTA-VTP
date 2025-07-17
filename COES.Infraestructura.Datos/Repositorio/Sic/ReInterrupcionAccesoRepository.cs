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
    /// Clase de acceso a datos de la tabla RE_INTERRUPCION_ACCESO
    /// </summary>
    public class ReInterrupcionAccesoRepository: RepositoryBase, IReInterrupcionAccesoRepository
    {
        public ReInterrupcionAccesoRepository(string strConn): base(strConn)
        {
        }

        ReInterrupcionAccesoHelper helper = new ReInterrupcionAccesoHelper();

        public int Save(ReInterrupcionAccesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reinaccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Reinacptoentrega, DbType.String, entity.Reinacptoentrega);
            dbProvider.AddInParameter(command, helper.Reinacrechazocarga, DbType.String, entity.Reinacrechazocarga);
            dbProvider.AddInParameter(command, helper.Reinacusucreacion, DbType.String, entity.Reinacusucreacion);
            dbProvider.AddInParameter(command, helper.Reinacfeccreacion, DbType.DateTime, entity.Reinacfeccreacion);
            dbProvider.AddInParameter(command, helper.Reinacusumodificacion, DbType.String, entity.Reinacusumodificacion);
            dbProvider.AddInParameter(command, helper.Reinacfecmodificacion, DbType.DateTime, entity.Reinacfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int GetIdMax()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
           
            return id;
        }

        public void Update(ReInterrupcionAccesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Reinacptoentrega, DbType.String, entity.Reinacptoentrega);
            dbProvider.AddInParameter(command, helper.Reinacrechazocarga, DbType.String, entity.Reinacrechazocarga);
            dbProvider.AddInParameter(command, helper.Reinacusucreacion, DbType.String, entity.Reinacusucreacion);
            dbProvider.AddInParameter(command, helper.Reinacfeccreacion, DbType.DateTime, entity.Reinacfeccreacion);
            dbProvider.AddInParameter(command, helper.Reinacusumodificacion, DbType.String, entity.Reinacusumodificacion);
            dbProvider.AddInParameter(command, helper.Reinacfecmodificacion, DbType.DateTime, entity.Reinacfecmodificacion);
            dbProvider.AddInParameter(command, helper.Reinaccodi, DbType.Int32, entity.Reinaccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reinaccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reinaccodi, DbType.Int32, reinaccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReInterrupcionAccesoDTO GetById(int reinaccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reinaccodi, DbType.Int32, reinaccodi);
            ReInterrupcionAccesoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReInterrupcionAccesoDTO> List()
        {
            List<ReInterrupcionAccesoDTO> entitys = new List<ReInterrupcionAccesoDTO>();
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

        public List<ReInterrupcionAccesoDTO> GetByCriteria()
        {
            List<ReInterrupcionAccesoDTO> entitys = new List<ReInterrupcionAccesoDTO>();
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

        public void DeletePorPeriodo(int repercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeletePeriodo);

            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, repercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<ReInterrupcionAccesoDTO> ListByPeriodo(int repercodi)
        {
            List<ReInterrupcionAccesoDTO> entitys = new List<ReInterrupcionAccesoDTO>();
            string sql = string.Format(helper.SqlListarPorPeriodo, repercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReInterrupcionAccesoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void GrabarEnGrupo(List<ReInterrupcionAccesoDTO> entitys)
        {
            dbProvider.AddColumnMapping(helper.Reinaccodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Emprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Repercodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Reinacptoentrega, DbType.String);
            dbProvider.AddColumnMapping(helper.Reinacrechazocarga, DbType.String);
            dbProvider.AddColumnMapping(helper.Reinacusucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Reinacfeccreacion, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Reinacusumodificacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Reinacfecmodificacion, DbType.DateTime);

            dbProvider.BulkInsert<ReInterrupcionAccesoDTO>(entitys, helper.TableName);
        }

    }
}
