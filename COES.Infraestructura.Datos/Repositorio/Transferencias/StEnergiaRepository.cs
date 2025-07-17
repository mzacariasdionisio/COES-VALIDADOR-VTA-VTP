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
    /// Clase de acceso a datos de la tabla ST_ENERGIA
    /// </summary>
    public class StEnergiaRepository : RepositoryBase, IStEnergiaRepository
    {
        public StEnergiaRepository(string strConn)
            : base(strConn)
        {
        }

        StEnergiaHelper helper = new StEnergiaHelper();

        public int Save(StEnergiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Stenrgcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Stenrgrgia, DbType.Decimal, entity.Stenrgrgia);
            dbProvider.AddInParameter(command, helper.Stenrgusucreacion, DbType.String, entity.Stenrgusucreacion);
            dbProvider.AddInParameter(command, helper.Stenrgfeccreacion, DbType.DateTime, entity.Stenrgfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StEnergiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Stenrgrgia, DbType.Decimal, entity.Stenrgrgia);
            dbProvider.AddInParameter(command, helper.Stenrgusucreacion, DbType.String, entity.Stenrgusucreacion);
            dbProvider.AddInParameter(command, helper.Stenrgfeccreacion, DbType.DateTime, entity.Stenrgfeccreacion);
            dbProvider.AddInParameter(command, helper.Stenrgcodi, DbType.Int32, entity.Stenrgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int stenrgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, stenrgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public StEnergiaDTO GetById(int stenrgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Stenrgcodi, DbType.Int32, stenrgcodi);
            StEnergiaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StEnergiaDTO> List()
        {
            List<StEnergiaDTO> entitys = new List<StEnergiaDTO>();
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

        public List<StEnergiaDTO> GetByCriteria(int strecacodi)
        {
            List<StEnergiaDTO> entitys = new List<StEnergiaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StEnergiaDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public StEnergiaDTO GetByCentralCodi(int strecacodi, int stcntgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCentralCodi);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);
            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, stcntgcodi);
            StEnergiaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StEnergiaDTO> ListByStEnergiaVersion(int strecacodi)
        {
            List<StEnergiaDTO> entitys = new List<StEnergiaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByStEnergiaVersion);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StEnergiaDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
