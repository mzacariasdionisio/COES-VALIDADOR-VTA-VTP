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
    /// Clase de acceso a datos de la tabla CO_VERSIONBASE
    /// </summary>
    public class CoVersionbaseRepository: RepositoryBase, ICoVersionbaseRepository
    {
        public CoVersionbaseRepository(string strConn): base(strConn)
        {
        }

        CoVersionbaseHelper helper = new CoVersionbaseHelper();

        public int Save(CoVersionbaseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Covebacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Covebadesc, DbType.String, entity.Covebadesc);
            dbProvider.AddInParameter(command, helper.Covebatipo, DbType.String, entity.Covebatipo);
            dbProvider.AddInParameter(command, helper.Covebadiainicio, DbType.Int32, entity.Covebadiainicio);
            dbProvider.AddInParameter(command, helper.Covebadiafin, DbType.Int32, entity.Covebadiafin);
            dbProvider.AddInParameter(command, helper.Covebausucreacion, DbType.String, entity.Covebausucreacion);
            dbProvider.AddInParameter(command, helper.Covebafeccreacion, DbType.DateTime, entity.Covebafeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoVersionbaseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Covebadesc, DbType.String, entity.Covebadesc);
            dbProvider.AddInParameter(command, helper.Covebatipo, DbType.String, entity.Covebatipo);
            dbProvider.AddInParameter(command, helper.Covebadiainicio, DbType.Int32, entity.Covebadiainicio);
            dbProvider.AddInParameter(command, helper.Covebadiafin, DbType.Int32, entity.Covebadiafin);
            dbProvider.AddInParameter(command, helper.Covebausucreacion, DbType.String, entity.Covebausucreacion);
            dbProvider.AddInParameter(command, helper.Covebafeccreacion, DbType.DateTime, entity.Covebafeccreacion);
            dbProvider.AddInParameter(command, helper.Covebacodi, DbType.Int32, entity.Covebacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int covebacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Covebacodi, DbType.Int32, covebacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoVersionbaseDTO GetById(int covebacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Covebacodi, DbType.Int32, covebacodi);
            CoVersionbaseDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoVersionbaseDTO> List()
        {
            List<CoVersionbaseDTO> entitys = new List<CoVersionbaseDTO>();
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

        public List<CoVersionbaseDTO> GetByCriteria()
        {
            List<CoVersionbaseDTO> entitys = new List<CoVersionbaseDTO>();
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
