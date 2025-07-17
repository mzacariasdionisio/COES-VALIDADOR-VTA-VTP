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
    /// Clase de acceso a datos de la tabla CM_GENERADOR_POTENCIAGEN
    /// </summary>
    public class CmGeneradorPotenciagenRepository: RepositoryBase, ICmGeneradorPotenciagenRepository
    {
        public CmGeneradorPotenciagenRepository(string strConn): base(strConn)
        {
        }

        CmGeneradorPotenciagenHelper helper = new CmGeneradorPotenciagenHelper();

        public int Save(CmGeneradorPotenciagenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Genpotcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, entity.Relacioncodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Genpotvalor, DbType.Decimal, entity.Genpotvalor);
            dbProvider.AddInParameter(command, helper.Genpotusucreacion, DbType.String, entity.Genpotusucreacion);
            dbProvider.AddInParameter(command, helper.Genpotfeccreacion, DbType.DateTime, entity.Genpotfeccreacion);
            dbProvider.AddInParameter(command, helper.Genpotusumodificacion, DbType.String, entity.Genpotusumodificacion);
            dbProvider.AddInParameter(command, helper.Genpotfecmodificacion, DbType.DateTime, entity.Genpotfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmGeneradorPotenciagenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, entity.Relacioncodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Genpotvalor, DbType.Decimal, entity.Genpotvalor);
            dbProvider.AddInParameter(command, helper.Genpotusucreacion, DbType.String, entity.Genpotusucreacion);
            dbProvider.AddInParameter(command, helper.Genpotfeccreacion, DbType.DateTime, entity.Genpotfeccreacion);
            dbProvider.AddInParameter(command, helper.Genpotusumodificacion, DbType.String, entity.Genpotusumodificacion);
            dbProvider.AddInParameter(command, helper.Genpotfecmodificacion, DbType.DateTime, entity.Genpotfecmodificacion);
            dbProvider.AddInParameter(command, helper.Genpotcodi, DbType.Int32, entity.Genpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int genpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Genpotcodi, DbType.Int32, genpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmGeneradorPotenciagenDTO GetById(int genpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Genpotcodi, DbType.Int32, genpotcodi);
            CmGeneradorPotenciagenDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmGeneradorPotenciagenDTO> List()
        {
            List<CmGeneradorPotenciagenDTO> entitys = new List<CmGeneradorPotenciagenDTO>();
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

        public List<CmGeneradorPotenciagenDTO> GetByCriteria(int relacionCodi)
        {
            List<CmGeneradorPotenciagenDTO> entitys = new List<CmGeneradorPotenciagenDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, relacionCodi);

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
