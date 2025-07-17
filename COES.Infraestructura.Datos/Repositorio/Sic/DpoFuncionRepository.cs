using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class DpoFuncionRepository : RepositoryBase, IDpoFuncionRepository
    {
        public DpoFuncionRepository(string strConn) : base(strConn)
        {
        }

        DpoFuncionHelper helper = new DpoFuncionHelper();

        public void Save(DpoFuncionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Dpofnccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Dpofncnombre, DbType.String, entity.Dpofncnombre);
            dbProvider.AddInParameter(command, helper.Dpofnctipo, DbType.String, entity.Dpofnctipo);
            dbProvider.AddInParameter(command, helper.Dpofncdescripcion, DbType.String, entity.Dpofncdescripcion);
            dbProvider.AddInParameter(command, helper.Dpofncusucreacion, DbType.String, entity.Dpofncusucreacion);
            dbProvider.AddInParameter(command, helper.Dpofncfeccreacion, DbType.DateTime, entity.Dpofncfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(DpoFuncionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Dpofncnombre, DbType.String, entity.Dpofncnombre);
            dbProvider.AddInParameter(command, helper.Dpofnctipo, DbType.String, entity.Dpofnctipo);
            dbProvider.AddInParameter(command, helper.Dpofncdescripcion, DbType.String, entity.Dpofncdescripcion);
            dbProvider.AddInParameter(command, helper.Dpofncusumodificacion, DbType.String, entity.Dpofncusumodificacion);
            dbProvider.AddInParameter(command, helper.Dpofncfecmodificacion, DbType.DateTime, entity.Dpofncfecmodificacion);
            dbProvider.AddInParameter(command, helper.Dpofnccodi, DbType.Int32, entity.Dpofnccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dpofnccodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public DpoFuncionDTO GetById(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Dpofnccodi, DbType.Int32, id);
            
            DpoFuncionDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<DpoFuncionDTO> List()
        {
            List<DpoFuncionDTO> entitys = new List<DpoFuncionDTO>();

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
