using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ST_FACTOR
    /// </summary>
    public class StFactorRepository: RepositoryBase, IStFactorRepository
    {
        public StFactorRepository(string strConn): base(strConn)
        {
        }

        StFactorHelper helper = new StFactorHelper();

        public int Save(StFactorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Stfactcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, entity.Sistrncodi);
            dbProvider.AddInParameter(command, helper.Stfacttor, DbType.Decimal, entity.Stfacttor);
            dbProvider.AddInParameter(command, helper.Stfactusucreacion, DbType.String, entity.Stfactusucreacion);
            dbProvider.AddInParameter(command, helper.Stfactfeccreacion, DbType.DateTime, entity.Stfactfeccreacion);

            //dbProvider.AddInParameter(command, helper.Stfactcodi, DbType.Int32, entity.Stfactcodi);
            //dbProvider.AddInParameter(command, helper.Stfacttor, DbType.Decimal, entity.Stfacttor);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StFactorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, entity.Sistrncodi);
            dbProvider.AddInParameter(command, helper.Stfacttor, DbType.Decimal, entity.Stfacttor);
            dbProvider.AddInParameter(command, helper.Stfactusucreacion, DbType.String, entity.Stfactusucreacion);
            dbProvider.AddInParameter(command, helper.Stfactfeccreacion, DbType.DateTime, entity.Stfactfeccreacion);
            dbProvider.AddInParameter(command, helper.Stfactcodi, DbType.Int32, entity.Stfactcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteVersion(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteVersion);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public StFactorDTO GetById(int stfactcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Stfactcodi, DbType.Int32, stfactcodi);
            StFactorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public StFactorDTO GetBySisTrans(int sistrncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetBySisTrans);

            dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, sistrncodi);
            StFactorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StFactorDTO> List()
        {
            List<StFactorDTO> entitys = new List<StFactorDTO>();
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

        public List<StFactorDTO> GetByCriteria(int strecacodi)
        {
            List<StFactorDTO> entitys = new List<StFactorDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StFactorDTO entity = helper.Create(dr);

                    int iSistemTransnomb = dr.GetOrdinal(this.helper.SisTrnnombre);
                    if (!dr.IsDBNull(iSistemTransnomb)) entity.SisTrnnombre = dr.GetString(iSistemTransnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<StFactorDTO> ListByStFactorVersion(int strecacodi)
        {
            List<StFactorDTO> entitys = new List<StFactorDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByStFactorVersion);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StFactorDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
