using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ABI_ENERGIAXFUENTENERGIA
    /// </summary>
    public class AbiEnergiaxfuentenergiaRepository : RepositoryBase, IAbiEnergiaxfuentenergiaRepository
    {
        public AbiEnergiaxfuentenergiaRepository(string strConn) : base(strConn)
        {
        }

        AbiEnergiaxfuentenergiaHelper helper = new AbiEnergiaxfuentenergiaHelper();

        public int Save(AbiEnergiaxfuentenergiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mdfecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);
            dbProvider.AddInParameter(command, helper.Mdfefecha, DbType.DateTime, entity.Mdfefecha);
            dbProvider.AddInParameter(command, helper.Mdfevalor, DbType.Decimal, entity.Mdfevalor);
            dbProvider.AddInParameter(command, helper.Mdfemes, DbType.DateTime, entity.Mdfemes);
            dbProvider.AddInParameter(command, helper.Mdfeusumodificacion, DbType.String, entity.Mdfeusumodificacion);
            dbProvider.AddInParameter(command, helper.Mdfefecmodificacion, DbType.DateTime, entity.Mdfefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AbiEnergiaxfuentenergiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mdfecodi, DbType.Int32, entity.Mdfecodi);
            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);
            dbProvider.AddInParameter(command, helper.Mdfefecha, DbType.DateTime, entity.Mdfefecha);
            dbProvider.AddInParameter(command, helper.Mdfevalor, DbType.Decimal, entity.Mdfevalor);
            dbProvider.AddInParameter(command, helper.Mdfemes, DbType.DateTime, entity.Mdfemes);
            dbProvider.AddInParameter(command, helper.Mdfeusumodificacion, DbType.String, entity.Mdfeusumodificacion);
            dbProvider.AddInParameter(command, helper.Mdfefecmodificacion, DbType.DateTime, entity.Mdfefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mdfecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mdfecodi, DbType.Int32, mdfecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByMes(DateTime fechaPeriodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByMes);

            dbProvider.AddInParameter(command, helper.Mdfemes, DbType.DateTime, fechaPeriodo);

            dbProvider.ExecuteNonQuery(command);
        }

        public AbiEnergiaxfuentenergiaDTO GetById(int mdfecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mdfecodi, DbType.Int32, mdfecodi);
            AbiEnergiaxfuentenergiaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AbiEnergiaxfuentenergiaDTO> List()
        {
            List<AbiEnergiaxfuentenergiaDTO> entitys = new List<AbiEnergiaxfuentenergiaDTO>();
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

        public List<AbiEnergiaxfuentenergiaDTO> GetByCriteria()
        {
            List<AbiEnergiaxfuentenergiaDTO> entitys = new List<AbiEnergiaxfuentenergiaDTO>();
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
