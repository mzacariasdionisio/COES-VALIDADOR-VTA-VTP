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
    /// Clase de acceso a datos de la tabla ST_COMPMENSUAL
    /// </summary>
    public class StCompmensualRepository : RepositoryBase, IStCompmensualRepository
    {
        public StCompmensualRepository(string strConn)
            : base(strConn)
        {
        }

        StCompmensualHelper helper = new StCompmensualHelper();

        public int Save(StCompmensualDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmpmencodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            //dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, entity.Sistrncodi);
            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, entity.Stcntgcodi);
            dbProvider.AddInParameter(command, helper.Cmpmenusucreacion, DbType.String, entity.Cmpmenusucreacion);
            dbProvider.AddInParameter(command, helper.Cmpmenfeccreacion, DbType.DateTime, entity.Cmpmenfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StCompmensualDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            //dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, entity.Sistrncodi);
            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, entity.Stcntgcodi);
            dbProvider.AddInParameter(command, helper.Cmpmenusucreacion, DbType.String, entity.Cmpmenusucreacion);
            dbProvider.AddInParameter(command, helper.Cmpmenfeccreacion, DbType.DateTime, entity.Cmpmenfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmpmencodi, DbType.Int32, entity.Cmpmencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int recacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, recacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public StCompmensualDTO GetById(int cmpmencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmpmencodi, DbType.Int32, cmpmencodi);
            StCompmensualDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StCompmensualDTO> List()
        {
            List<StCompmensualDTO> entitys = new List<StCompmensualDTO>();
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

        public List<StCompmensualDTO> GetByCriteria(int recacodi)
        {
            List<StCompmensualDTO> entitys = new List<StCompmensualDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, recacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StCompmensualDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    //int iSisNombre = dr.GetOrdinal(this.helper.Sistrnnombre);
                    //if (!dr.IsDBNull(iSisNombre)) entity.Sistrnnombre = dr.GetString(iSisNombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<StCompmensualDTO> ListByStCompMensualVersion(int strecacodi)
        {
            List<StCompmensualDTO> entitys = new List<StCompmensualDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByStCompMensualVersion);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

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
