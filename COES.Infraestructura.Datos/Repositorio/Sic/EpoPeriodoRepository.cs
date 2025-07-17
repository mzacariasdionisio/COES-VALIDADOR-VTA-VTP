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
    /// Clase de acceso a datos de la tabla EPO_PERIODO
    /// </summary>
    public class EpoPeriodoRepository: RepositoryBase, IEpoPeriodoRepository
    {
        public EpoPeriodoRepository(string strConn): base(strConn)
        {
        }

        EpoPeriodoHelper helper = new EpoPeriodoHelper();

        public int Save(EpoPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Peranio, DbType.Int32, entity.Peranio);
            dbProvider.AddInParameter(command, helper.Permes, DbType.Int32, entity.Permes);
            dbProvider.AddInParameter(command, helper.Perestado, DbType.String, entity.Perestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EpoPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, entity.Percodi);
            dbProvider.AddInParameter(command, helper.Peranio, DbType.Int32, entity.Peranio);
            dbProvider.AddInParameter(command, helper.Permes, DbType.Int32, entity.Permes);
            dbProvider.AddInParameter(command, helper.Perestado, DbType.String, entity.Perestado);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int percodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, percodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EpoPeriodoDTO GetById(int percodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, percodi);
            EpoPeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EpoPeriodoDTO> List()
        {
            List<EpoPeriodoDTO> entitys = new List<EpoPeriodoDTO>();
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

        public List<EpoPeriodoDTO> GetByCriteria()
        {
            List<EpoPeriodoDTO> entitys = new List<EpoPeriodoDTO>();
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
    }
}
