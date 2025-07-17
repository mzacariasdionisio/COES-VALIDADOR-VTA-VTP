using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_GERCSV
    /// </summary>
    public class CpaGercsvRepository : RepositoryBase, ICpaGercsvRepository
    {
        public CpaGercsvRepository(string strConn)
            : base(strConn)
        {
        }

        CpaGercsvHelper helper = new CpaGercsvHelper();

        public int Save(CpaGercsvDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpagercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpsddpcodi, DbType.Int32, entity.Cpsddpcodi);
            dbProvider.AddInParameter(command, helper.Cpagergndarchivo, DbType.String, entity.Cpagergndarchivo);
            dbProvider.AddInParameter(command, helper.Cpagerhidarchivo, DbType.String, entity.Cpagerhidarchivo);
            dbProvider.AddInParameter(command, helper.Cpagerterarchivo, DbType.String, entity.Cpagerterarchivo);
            dbProvider.AddInParameter(command, helper.Cpagerdurarchivo, DbType.String, entity.Cpagerdurarchivo);
            dbProvider.AddInParameter(command, helper.Cpagerusucreacion, DbType.String, entity.Cpagerusucreacion);
            dbProvider.AddInParameter(command, helper.Cpagerfeccreacion, DbType.DateTime, entity.Cpagerfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpaGercsvDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpsddpcodi, DbType.Int32, entity.Cpsddpcodi);
            dbProvider.AddInParameter(command, helper.Cpagergndarchivo, DbType.String, entity.Cpagergndarchivo);
            dbProvider.AddInParameter(command, helper.Cpagerhidarchivo, DbType.String, entity.Cpagerhidarchivo);
            dbProvider.AddInParameter(command, helper.Cpagerterarchivo, DbType.String, entity.Cpagerterarchivo);
            dbProvider.AddInParameter(command, helper.Cpagerdurarchivo, DbType.String, entity.Cpagerdurarchivo);
            dbProvider.AddInParameter(command, helper.Cpagerusucreacion, DbType.String, entity.Cpagerusucreacion);
            dbProvider.AddInParameter(command, helper.Cpagerfeccreacion, DbType.DateTime, entity.Cpagerfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpagercodi, DbType.Int32, entity.Cpagercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpaGercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpagercodi, DbType.Int32, cpaGercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpaGercsvDTO GetById(int Cpsddpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpsddpcodi, DbType.Int32, Cpsddpcodi);
            CpaGercsvDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaGercsvDTO> List()
        {
            List<CpaGercsvDTO> entities = new List<CpaGercsvDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }
    }
}
