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
    /// Clase de acceso a datos de la tabla AF_EMPRESA
    /// </summary>
    public class AfEmpresaRepository : RepositoryBase, IAfEmpresaRepository
    {
        public AfEmpresaRepository(string strConn) : base(strConn)
        {
        }

        AfEmpresaHelper helper = new AfEmpresaHelper();

        public int Save(AfEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Afemprestado, DbType.Int32, entity.Afemprestado);
            dbProvider.AddInParameter(command, helper.Afemprosinergmin, DbType.String, entity.Afemprosinergmin);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Afemprusumodificacion, DbType.String, entity.Afemprusumodificacion);
            dbProvider.AddInParameter(command, helper.Afemprusucreacion, DbType.String, entity.Afemprusucreacion);
            dbProvider.AddInParameter(command, helper.Afemprfecmodificacion, DbType.DateTime, entity.Afemprfecmodificacion);
            dbProvider.AddInParameter(command, helper.Afemprfeccreacion, DbType.DateTime, entity.Afemprfeccreacion);
            dbProvider.AddInParameter(command, helper.Afemprnomb, DbType.String, entity.Afemprnomb);
            dbProvider.AddInParameter(command, helper.Afemprcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Afalerta, DbType.String, entity.Afalerta);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AfEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Afemprestado, DbType.Int32, entity.Afemprestado);
            dbProvider.AddInParameter(command, helper.Afemprosinergmin, DbType.String, entity.Afemprosinergmin);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Afemprusumodificacion, DbType.String, entity.Afemprusumodificacion);
            dbProvider.AddInParameter(command, helper.Afemprusucreacion, DbType.String, entity.Afemprusucreacion);
            dbProvider.AddInParameter(command, helper.Afemprfecmodificacion, DbType.DateTime, entity.Afemprfecmodificacion);
            dbProvider.AddInParameter(command, helper.Afemprfeccreacion, DbType.DateTime, entity.Afemprfeccreacion);
            dbProvider.AddInParameter(command, helper.Afemprnomb, DbType.String, entity.Afemprnomb);
            dbProvider.AddInParameter(command, helper.Afalerta, DbType.String, entity.Afalerta);
            dbProvider.AddInParameter(command, helper.Afemprcodi, DbType.Int32, entity.Afemprcodi);
            

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int afemprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Afemprcodi, DbType.Int32, afemprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AfEmpresaDTO GetById(int afemprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Afemprcodi, DbType.Int32, afemprcodi);
            AfEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iAFALERTA = dr.GetOrdinal(helper.Afalerta);
                    if (!dr.IsDBNull(iAFALERTA)) entity.Afalerta = dr.GetString(iAFALERTA);
                }
            }

            return entity;
        }

        public List<AfEmpresaDTO> List()
        {
            List<AfEmpresaDTO> entitys = new List<AfEmpresaDTO>();
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

        public List<AfEmpresaDTO> GetByCriteria()
        {
            List<AfEmpresaDTO> entitys = new List<AfEmpresaDTO>();
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
        public AfEmpresaDTO GetByIdxEmprcodi(int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdxEmprcodi);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            AfEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iAFALERTA = dr.GetOrdinal(helper.Afalerta);
                    if (!dr.IsDBNull(iAFALERTA)) entity.Afalerta = dr.GetString(iAFALERTA);
                }
            }

            return entity;
        }
    }
}
