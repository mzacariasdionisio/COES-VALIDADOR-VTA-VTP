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
    /// Clase de acceso a datos de la tabla FT_FICTECNOTA
    /// </summary>
    public class FtFictecNotaRepository : RepositoryBase, IFtFictecNotaRepository
    {
        public FtFictecNotaRepository(string strConn)
            : base(strConn)
        {
        }

        FtFictecNotaHelper helper = new FtFictecNotaHelper();

        public int Save(FtFictecNotaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftnotacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftnotanum, DbType.Int32, entity.Ftnotanum);
            dbProvider.AddInParameter(command, helper.Ftnotdesc, DbType.String, entity.Ftnotdesc);
            dbProvider.AddInParameter(command, helper.Ftnotausucreacion, DbType.String, entity.Ftnotausucreacion);
            dbProvider.AddInParameter(command, helper.Ftnotafeccreacion, DbType.DateTime, entity.Ftnotafeccreacion);
            dbProvider.AddInParameter(command, helper.Ftnotausumodificacion, DbType.String, entity.Ftnotausumodificacion);
            dbProvider.AddInParameter(command, helper.Ftnotafecmodificacion, DbType.DateTime, entity.Ftnotafecmodificacion);
            dbProvider.AddInParameter(command, helper.Ftnotestado, DbType.String, entity.Ftnotestado);
            dbProvider.AddInParameter(command, helper.Fteqcodi, DbType.Int32, entity.Fteqcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtFictecNotaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftnotanum, DbType.Int32, entity.Ftnotanum);
            dbProvider.AddInParameter(command, helper.Ftnotdesc, DbType.String, entity.Ftnotdesc);
            dbProvider.AddInParameter(command, helper.Ftnotausumodificacion, DbType.String, entity.Ftnotausumodificacion);
            dbProvider.AddInParameter(command, helper.Ftnotafecmodificacion, DbType.DateTime, entity.Ftnotafecmodificacion);

            dbProvider.AddInParameter(command, helper.Ftnotacodi, DbType.Int32, entity.Ftnotacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(FtFictecNotaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftnotausumodificacion, DbType.String, entity.Ftnotausumodificacion);
            dbProvider.AddInParameter(command, helper.Ftnotafecmodificacion, DbType.DateTime, entity.Ftnotafecmodificacion);

            dbProvider.AddInParameter(command, helper.Ftnotacodi, DbType.Int32, entity.Ftnotacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtFictecNotaDTO GetById(int ftnotacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftnotacodi, DbType.Int32, ftnotacodi);
            FtFictecNotaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtFictecNotaDTO> List()
        {
            List<FtFictecNotaDTO> entitys = new List<FtFictecNotaDTO>();
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

        public List<FtFictecNotaDTO> ListByFteqcodi(int fteqcodi)
        {
            List<FtFictecNotaDTO> entitys = new List<FtFictecNotaDTO>();

            string query = string.Format(helper.SqlListByFteqcodi, fteqcodi);

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

        public List<FtFictecNotaDTO> GetByCriteria()
        {
            List<FtFictecNotaDTO> entitys = new List<FtFictecNotaDTO>();
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
