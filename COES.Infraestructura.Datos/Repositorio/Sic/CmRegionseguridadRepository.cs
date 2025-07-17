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
    /// Clase de acceso a datos de la tabla CM_REGIONSEGURIDAD
    /// </summary>
    public class CmRegionseguridadRepository: RepositoryBase, ICmRegionseguridadRepository
    {
        public CmRegionseguridadRepository(string strConn): base(strConn)
        {
        }

        CmRegionseguridadHelper helper = new CmRegionseguridadHelper();

        public int Save(CmRegionseguridadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Regsegnombre, DbType.String, entity.Regsegnombre);
            dbProvider.AddInParameter(command, helper.Regsegvalorm, DbType.Decimal, entity.Regsegvalorm);
            dbProvider.AddInParameter(command, helper.Regsegdirec, DbType.String, entity.Regsegdirec);
            dbProvider.AddInParameter(command, helper.Regsegestado, DbType.String, entity.Regsegestado);
            dbProvider.AddInParameter(command, helper.Regsegusucreacion, DbType.String, entity.Regsegusucreacion);
            dbProvider.AddInParameter(command, helper.Regsegfeccreacion, DbType.DateTime, entity.Regsegfeccreacion);
            dbProvider.AddInParameter(command, helper.Regsegusumodificacion, DbType.String, entity.Regsegusumodificacion);
            dbProvider.AddInParameter(command, helper.Regsegfecmodificacion, DbType.DateTime, entity.Regsegfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmRegionseguridadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Regsegnombre, DbType.String, entity.Regsegnombre);
            dbProvider.AddInParameter(command, helper.Regsegvalorm, DbType.Decimal, entity.Regsegvalorm);
            dbProvider.AddInParameter(command, helper.Regsegdirec, DbType.String, entity.Regsegdirec);
            dbProvider.AddInParameter(command, helper.Regsegestado, DbType.String, entity.Regsegestado);
            dbProvider.AddInParameter(command, helper.Regsegusucreacion, DbType.String, entity.Regsegusucreacion);
            dbProvider.AddInParameter(command, helper.Regsegfeccreacion, DbType.DateTime, entity.Regsegfeccreacion);
            dbProvider.AddInParameter(command, helper.Regsegusumodificacion, DbType.String, entity.Regsegusumodificacion);
            dbProvider.AddInParameter(command, helper.Regsegfecmodificacion, DbType.DateTime, entity.Regsegfecmodificacion);
            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, entity.Regsegcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int regsegcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, regsegcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmRegionseguridadDTO GetById(int regsegcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, regsegcodi);
            CmRegionseguridadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmRegionseguridadDTO> List()
        {
            List<CmRegionseguridadDTO> entitys = new List<CmRegionseguridadDTO>();
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

        public List<CmRegionseguridadDTO> GetByCriteria()
        {
            List<CmRegionseguridadDTO> entitys = new List<CmRegionseguridadDTO>();
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

        public List<CmRegionseguridadDTO> GetByCriteriaCoordenada(int regcodi)
        {
            List<CmRegionseguridadDTO> entitys = new List<CmRegionseguridadDTO>();
            string strSql = string.Format(helper.SqlGetByCriteriaCoordenada, regcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(strSql);

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
