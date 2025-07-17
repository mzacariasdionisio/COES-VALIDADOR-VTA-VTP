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
    /// Clase de acceso a datos de la tabla WB_VERSION_MODPLAN
    /// </summary>
    public class WbVersionModplanRepository: RepositoryBase, IWbVersionModplanRepository
    {
        public WbVersionModplanRepository(string strConn): base(strConn)
        {
        }

        WbVersionModplanHelper helper = new WbVersionModplanHelper();

        public int Save(WbVersionModplanDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vermplcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vermpldesc, DbType.String, entity.Vermpldesc);
            dbProvider.AddInParameter(command, helper.Vermplestado, DbType.String, entity.Vermplestado);
            dbProvider.AddInParameter(command, helper.Vermplpadre, DbType.Int32, entity.Vermplpadre);
            dbProvider.AddInParameter(command, helper.Vermplusucreacion, DbType.String, entity.Vermplusucreacion);
            dbProvider.AddInParameter(command, helper.Vermplfeccreacion, DbType.DateTime, entity.Vermplfeccreacion);
            dbProvider.AddInParameter(command, helper.Vermplusumodificacion, DbType.String, entity.Vermplusumodificacion);
            dbProvider.AddInParameter(command, helper.Vermplfecmodificacion, DbType.DateTime, entity.Vermplfecmodificacion);
            dbProvider.AddInParameter(command, helper.Vermpltipo, DbType.Int32, entity.Vermpltipo);
        

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbVersionModplanDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vermpldesc, DbType.String, entity.Vermpldesc);
            dbProvider.AddInParameter(command, helper.Vermplestado, DbType.String, entity.Vermplestado);
            dbProvider.AddInParameter(command, helper.Vermplpadre, DbType.Int32, entity.Vermplpadre);
            dbProvider.AddInParameter(command, helper.Vermplusucreacion, DbType.String, entity.Vermplusucreacion);
            dbProvider.AddInParameter(command, helper.Vermplfeccreacion, DbType.DateTime, entity.Vermplfeccreacion);
            dbProvider.AddInParameter(command, helper.Vermpltipo, DbType.Int32, entity.Vermpltipo);
            dbProvider.AddInParameter(command, helper.Vermplcodi, DbType.Int32, entity.Vermplcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vermplcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vermplcodi, DbType.Int32, vermplcodi);
            dbProvider.AddInParameter(command, helper.Vermplcodi, DbType.Int32, vermplcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbVersionModplanDTO GetById(int vermplcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vermplcodi, DbType.Int32, vermplcodi);
            WbVersionModplanDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbVersionModplanDTO> List(int tipo)
        {
            List<WbVersionModplanDTO> entitys = new List<WbVersionModplanDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Vermpltipo, DbType.Int32, tipo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<WbVersionModplanDTO> GetByCriteria()
        {
            List<WbVersionModplanDTO> entitys = new List<WbVersionModplanDTO>();
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

        public List<WbVersionModplanDTO> ObtenerVersionPorPadre(int idPadre, int tipo)
        {
            List<WbVersionModplanDTO> entitys = new List<WbVersionModplanDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerVersionPorPadre);
            dbProvider.AddInParameter(command, helper.Vermplpadre, DbType.Int32, idPadre);
            dbProvider.AddInParameter(command, helper.Vermpltipo, DbType.Int32, tipo);

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
