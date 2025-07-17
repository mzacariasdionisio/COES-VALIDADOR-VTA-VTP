using System;
using System.Collections.Generic;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class DpoEstadoVersionRepository : RepositoryBase, IDpoEstadoVersionRepository
    {
        public DpoEstadoVersionRepository(string strConn) : base(strConn)
        {
        }

        DpoEstadoVersionHelper helper = new DpoEstadoVersionHelper();

        public void Save(DpoEstadoVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Dpoevscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vergrpcodi, DbType.Int32, entity.Vergrpcodi);
            dbProvider.AddInParameter(command, helper.Dpoevspadre, DbType.Int32, entity.Dpoevspadre);
            dbProvider.AddInParameter(command, helper.Dpoevsrepvegt, DbType.String, entity.Dpoevsrepvegt);
            dbProvider.AddInParameter(command, helper.Dpoevsrepindt, DbType.String, entity.Dpoevsrepindt);
            dbProvider.AddInParameter(command, helper.Dpoevsrepdesp, DbType.String, entity.Dpoevsrepdesp);
            dbProvider.AddInParameter(command, helper.Dpoevsusucreacion, DbType.String, entity.Dpoevsusucreacion);
            dbProvider.AddInParameter(command, helper.Dpoevsfeccreacion, DbType.DateTime, entity.Dpoevsfeccreacion);
            dbProvider.AddInParameter(command, helper.Dpoevsusumodificacion, DbType.String, entity.Dpoevsusumodificacion);
            dbProvider.AddInParameter(command, helper.Dpoevsfecmodificacion, DbType.DateTime, entity.Dpoevsfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(DpoEstadoVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Dpoevsrepvegt, DbType.String, entity.Dpoevsrepvegt);
            dbProvider.AddInParameter(command, helper.Dpoevsrepindt, DbType.String, entity.Dpoevsrepindt);
            dbProvider.AddInParameter(command, helper.Dpoevsrepdesp, DbType.String, entity.Dpoevsrepdesp);
            dbProvider.AddInParameter(command, helper.Dpoevsusumodificacion, DbType.String, entity.Dpoevsusumodificacion);
            dbProvider.AddInParameter(command, helper.Dpoevsfecmodificacion, DbType.DateTime, entity.Dpoevsfecmodificacion);
            
            dbProvider.AddInParameter(command, helper.Dpoevscodi, DbType.Int32, entity.Dpoevscodi);

            dbProvider.ExecuteNonQuery(command);
        }
        public void Delete(int dpoevscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dpoevscodi, DbType.Int32, dpoevscodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public DpoEstadoVersionDTO GetById(int dpoevscodi)
        {
            DpoEstadoVersionDTO entity = new DpoEstadoVersionDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<DpoEstadoVersionDTO> List()
        {
            List<DpoEstadoVersionDTO> entitys = new List<DpoEstadoVersionDTO>();
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
    }
}
