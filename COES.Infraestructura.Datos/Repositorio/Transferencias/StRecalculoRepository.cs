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
    /// Clase de acceso a datos de la tabla ST_RECALCULO
    /// </summary>
    public class StRecalculoRepository : RepositoryBase, IStRecalculoRepository
    {
        public StRecalculoRepository(string strConn)
            : base(strConn)
        {
        }

        StRecalculoHelper helper = new StRecalculoHelper();

        public int Save(StRecalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Stpercodi, DbType.Int32, entity.Stpercodi);
            dbProvider.AddInParameter(command, helper.Sstversion, DbType.Int32, entity.Sstversion);
            dbProvider.AddInParameter(command, helper.Strecanombre, DbType.String, entity.Strecanombre);
            dbProvider.AddInParameter(command, helper.Strecainforme, DbType.String, entity.Strecainforme);
            dbProvider.AddInParameter(command, helper.Strecafacajuste, DbType.Decimal, entity.Strecafacajuste);
            dbProvider.AddInParameter(command, helper.Strecacomentario, DbType.String, entity.Strecacomentario);
            dbProvider.AddInParameter(command, helper.Strecausucreacion, DbType.String, entity.Strecausucreacion);
            dbProvider.AddInParameter(command, helper.Strecafeccreacion, DbType.DateTime, entity.Strecafeccreacion);
            dbProvider.AddInParameter(command, helper.Strecausumodificacion, DbType.String, entity.Strecausumodificacion);
            dbProvider.AddInParameter(command, helper.Strecafecmodificacion, DbType.DateTime, entity.Strecafecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StRecalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Stpercodi, DbType.Int32, entity.Stpercodi);
            dbProvider.AddInParameter(command, helper.Strecanombre, DbType.String, entity.Strecanombre);
            dbProvider.AddInParameter(command, helper.Strecainforme, DbType.String, entity.Strecainforme);
            dbProvider.AddInParameter(command, helper.Strecafacajuste, DbType.Decimal, entity.Strecafacajuste);
            dbProvider.AddInParameter(command, helper.Strecacomentario, DbType.String, entity.Strecacomentario);
            dbProvider.AddInParameter(command, helper.Strecausumodificacion, DbType.String, entity.Strecausumodificacion);
            dbProvider.AddInParameter(command, helper.Strecafecmodificacion, DbType.DateTime, entity.Strecafecmodificacion);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public StRecalculoDTO GetById(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);
            StRecalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iStpernombre = dr.GetOrdinal(this.helper.Stpernombre);
                    if (!dr.IsDBNull(iStpernombre)) entity.Stpernombre = dr.GetString(iStpernombre);
                }
            }

            return entity;
        }

        public List<StRecalculoDTO> List(int id)
        {
            List<StRecalculoDTO> entitys = new List<StRecalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Stpercodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<StRecalculoDTO> GetByCriteria()
        {
            List<StRecalculoDTO> entitys = new List<StRecalculoDTO>();
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

        public List<StRecalculoDTO> ListByStPericodi(int stpercodi)
        {
            List<StRecalculoDTO> entitys = new List<StRecalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByStPericodi);
            dbProvider.AddInParameter(command, helper.Stpercodi, DbType.Int32, stpercodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public StRecalculoDTO GetByIdView(int stpercodi, int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdView);

            dbProvider.AddInParameter(command, helper.Stpercodi, DbType.Int32, stpercodi);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);
            StRecalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                }
            }

            return entity;
        }
    }
}
