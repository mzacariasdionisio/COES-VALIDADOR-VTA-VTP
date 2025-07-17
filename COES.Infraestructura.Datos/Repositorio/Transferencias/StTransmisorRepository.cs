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
    /// Clase de acceso a datos de la tabla ST_TRANSMISOR
    /// </summary>
    public class StTransmisorRepository : RepositoryBase, IStTransmisorRepository
    {
        public StTransmisorRepository(string strConn)
            : base(strConn)
        {
        }

        StTransmisorHelper helper = new StTransmisorHelper();

        public int Save(StTransmisorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Stranscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Stransusucreacion, DbType.String, entity.Stransusucreacion);
            dbProvider.AddInParameter(command, helper.Stransfeccreacion, DbType.DateTime, entity.Stransfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StTransmisorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Stransusucreacion, DbType.String, entity.Stransusucreacion);
            dbProvider.AddInParameter(command, helper.Stransfeccreacion, DbType.DateTime, entity.Stransfeccreacion);
            dbProvider.AddInParameter(command, helper.Stranscodi, DbType.Int32, entity.Stranscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int stranscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Stranscodi, DbType.Int32, stranscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteVersion(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteVersion);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public StTransmisorDTO GetById(int stranscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Stranscodi, DbType.Int32, stranscodi);
            StTransmisorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StTransmisorDTO> List(int strecacodi)
        {
            List<StTransmisorDTO> entitys = new List<StTransmisorDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.String, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StTransmisorDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<StTransmisorDTO> GetByCriteria()
        {
            List<StTransmisorDTO> entitys = new List<StTransmisorDTO>();
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


        public List<StTransmisorDTO> ListByStTransmisorVersion(int strecacodi)
        {
            List<StTransmisorDTO> entitys = new List<StTransmisorDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByStTransmisorVersion);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.String, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StTransmisorDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
    }
}
