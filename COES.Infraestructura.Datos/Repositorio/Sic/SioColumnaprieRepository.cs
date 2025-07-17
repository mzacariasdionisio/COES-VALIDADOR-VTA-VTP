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
    /// Clase de acceso a datos de la tabla SIO_COLUMNAPRIE
    /// </summary>
    public class SioColumnaprieRepository: RepositoryBase, ISioColumnaprieRepository
    {
        public SioColumnaprieRepository(string strConn): base(strConn)
        {
        }

        SioColumnaprieHelper helper = new SioColumnaprieHelper();

        public int Save(SioColumnaprieDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpriecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cprienombre, DbType.String, entity.Cprienombre);
            dbProvider.AddInParameter(command, helper.Cpriedescripcion, DbType.String, entity.Cpriedescripcion);
            dbProvider.AddInParameter(command, helper.Cprietipo, DbType.Int32, entity.Cprietipo);
            dbProvider.AddInParameter(command, helper.Cprielong1, DbType.Int32, entity.Cprielong1);
            dbProvider.AddInParameter(command, helper.Cprielong2, DbType.Int32, entity.Cprielong2);
            dbProvider.AddInParameter(command, helper.Tpriecodi, DbType.Int32, entity.Tpriecodi);
            dbProvider.AddInParameter(command, helper.Cprieusumodificacion, DbType.String, entity.Cprieusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpriefecmodificacion, DbType.DateTime, entity.Cpriefecmodificacion);
            dbProvider.AddInParameter(command, helper.Cprieusucreacion, DbType.String, entity.Cprieusucreacion);
            dbProvider.AddInParameter(command, helper.Cpriefeccreacion, DbType.DateTime, entity.Cpriefeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SioColumnaprieDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpriecodi, DbType.Int32, entity.Cpriecodi);
            dbProvider.AddInParameter(command, helper.Cprienombre, DbType.String, entity.Cprienombre);
            dbProvider.AddInParameter(command, helper.Cpriedescripcion, DbType.String, entity.Cpriedescripcion);
            dbProvider.AddInParameter(command, helper.Cprietipo, DbType.Int32, entity.Cprietipo);
            dbProvider.AddInParameter(command, helper.Cprielong1, DbType.Int32, entity.Cprielong1);
            dbProvider.AddInParameter(command, helper.Cprielong2, DbType.Int32, entity.Cprielong2);
            dbProvider.AddInParameter(command, helper.Tpriecodi, DbType.Int32, entity.Tpriecodi);
            dbProvider.AddInParameter(command, helper.Cprieusumodificacion, DbType.String, entity.Cprieusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpriefecmodificacion, DbType.DateTime, entity.Cpriefecmodificacion);
            dbProvider.AddInParameter(command, helper.Cprieusucreacion, DbType.String, entity.Cprieusucreacion);
            dbProvider.AddInParameter(command, helper.Cpriefeccreacion, DbType.DateTime, entity.Cpriefeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpriecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpriecodi, DbType.Int32, cpriecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SioColumnaprieDTO GetById(int cpriecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpriecodi, DbType.Int32, cpriecodi);
            SioColumnaprieDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SioColumnaprieDTO> List()
        {
            List<SioColumnaprieDTO> entitys = new List<SioColumnaprieDTO>();
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

        public List<SioColumnaprieDTO> GetByCriteria(int tpriecodi)
        {
            List<SioColumnaprieDTO> entitys = new List<SioColumnaprieDTO>();
            string query = string.Format(helper.SqlGetByCriteria, tpriecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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