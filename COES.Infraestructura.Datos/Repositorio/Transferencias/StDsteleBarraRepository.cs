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
    /// Clase de acceso a datos de la tabla ST_DSTELE_BARRA
    /// </summary>
    public class StDsteleBarraRepository: RepositoryBase, IStDsteleBarraRepository
    {
        public StDsteleBarraRepository(string strConn): base(strConn)
        {
        }

        StDsteleBarraHelper helper = new StDsteleBarraHelper();

        public void Save(StDsteleBarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Dstelecodi, DbType.Int32, entity.Dstelecodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Delbarrpu, DbType.Decimal, entity.Delbarrpu);
            dbProvider.AddInParameter(command, helper.Delbarxpu, DbType.Decimal, entity.Delbarxpu);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(StDsteleBarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Delbarrpu, DbType.Decimal, entity.Delbarrpu);
            dbProvider.AddInParameter(command, helper.Delbarxpu, DbType.Decimal, entity.Delbarxpu);
            dbProvider.AddInParameter(command, helper.Dstelecodi, DbType.Int32, entity.Dstelecodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }
        
        public StDsteleBarraDTO GetById(int dstelecodi, int barrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Dstelecodi, DbType.Int32, dstelecodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, barrcodi);
            StDsteleBarraDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public StDsteleBarraDTO GetByCriterios(int strecacodi, int barr1, int barr2)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriterios);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);
            dbProvider.AddInParameter(command, helper.Barra1, DbType.Int32, barr1);
            dbProvider.AddInParameter(command, helper.Barra2, DbType.Int32, barr2);
            StDsteleBarraDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StDsteleBarraDTO> List()
        {
            List<StDsteleBarraDTO> entitys = new List<StDsteleBarraDTO>();
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

        public List<StDsteleBarraDTO> GetByCriteria()
        {
            List<StDsteleBarraDTO> entitys = new List<StDsteleBarraDTO>();
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

        public List<StDsteleBarraDTO> ListStDistEleBarrPorID(int id)
        {
            List<StDsteleBarraDTO> entitys = new List<StDsteleBarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListStDistEleBarrPorID);
            dbProvider.AddInParameter(command, helper.Dstelecodi, DbType.Int32, id);

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
