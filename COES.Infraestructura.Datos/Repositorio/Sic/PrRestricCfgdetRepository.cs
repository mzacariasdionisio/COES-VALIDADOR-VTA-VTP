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
    /// Clase de acceso a datos de la tabla PR_RESTRIC_CFGDET
    /// </summary>
    public class PrRestricCfgdetRepository: RepositoryBase, IPrRestricCfgdetRepository
    {
        public PrRestricCfgdetRepository(string strConn): base(strConn)
        {
        }

        PrRestricCfgdetHelper helper = new PrRestricCfgdetHelper();

        public int Save(PrRestricCfgdetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Resdetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Resdetfuente, DbType.String, entity.Resdetfuente);
            dbProvider.AddInParameter(command, helper.Resdetcodigo, DbType.Int32, entity.Resdetcodigo);
            dbProvider.AddInParameter(command, helper.Resdetestado, DbType.String, entity.Resdetestado);
            dbProvider.AddInParameter(command, helper.Rescfgcodi, DbType.Int32, entity.Rescfgcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrRestricCfgdetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Resdetfuente, DbType.String, entity.Resdetfuente);
            dbProvider.AddInParameter(command, helper.Resdetcodigo, DbType.Int32, entity.Resdetcodigo);
            dbProvider.AddInParameter(command, helper.Resdetestado, DbType.String, entity.Resdetestado);
            dbProvider.AddInParameter(command, helper.Rescfgcodi, DbType.Int32, entity.Rescfgcodi);
            dbProvider.AddInParameter(command, helper.Resdetcodi, DbType.Int32, entity.Resdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int resdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Resdetcodi, DbType.Int32, resdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrRestricCfgdetDTO GetById(int resdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Resdetcodi, DbType.Int32, resdetcodi);
            PrRestricCfgdetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrRestricCfgdetDTO> List()
        {
            List<PrRestricCfgdetDTO> entitys = new List<PrRestricCfgdetDTO>();
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

        public List<PrRestricCfgdetDTO> GetByCriteria(string codigos)
        {
            List<PrRestricCfgdetDTO> entitys = new List<PrRestricCfgdetDTO>();
            string query = string.Format(helper.SqlGetByCriteria, codigos);
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

    }
}
