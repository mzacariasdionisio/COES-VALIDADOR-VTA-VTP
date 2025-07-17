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
    /// Clase de acceso a datos de la tabla SPO_NUMHISTORIA
    /// </summary>
    public class SpoNumhistoriaRepository: RepositoryBase, ISpoNumhistoriaRepository
    {
        public SpoNumhistoriaRepository(string strConn): base(strConn)
        {
        }

        SpoNumhistoriaHelper helper = new SpoNumhistoriaHelper();

        public int Save(SpoNumhistoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Numhiscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Numecodi, DbType.Int32, entity.Numecodi);
            dbProvider.AddInParameter(command, helper.Numhisdescripcion, DbType.String, entity.Numhisdescripcion);
            dbProvider.AddInParameter(command, helper.Numhisabrev, DbType.String, entity.Numhisabrev);
            dbProvider.AddInParameter(command, helper.Numhisfecha, DbType.DateTime, entity.Numhisfecha);
            dbProvider.AddInParameter(command, helper.Numhisusuario, DbType.String, entity.Numhisusuario);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SpoNumhistoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Numhiscodi, DbType.Int32, entity.Numhiscodi);
            dbProvider.AddInParameter(command, helper.Numecodi, DbType.Int32, entity.Numecodi);
            dbProvider.AddInParameter(command, helper.Numhisdescripcion, DbType.String, entity.Numhisdescripcion);
            dbProvider.AddInParameter(command, helper.Numhisabrev, DbType.String, entity.Numhisabrev);
            dbProvider.AddInParameter(command, helper.Numhisfecha, DbType.DateTime, entity.Numhisfecha);
            dbProvider.AddInParameter(command, helper.Numhisusuario, DbType.String, entity.Numhisusuario);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int numhiscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Numhiscodi, DbType.Int32, numhiscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SpoNumhistoriaDTO GetById(int numhiscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Numhiscodi, DbType.Int32, numhiscodi);
            SpoNumhistoriaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SpoNumhistoriaDTO> List()
        {
            List<SpoNumhistoriaDTO> entitys = new List<SpoNumhistoriaDTO>();
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

        public List<SpoNumhistoriaDTO> GetByCriteria()
        {
            List<SpoNumhistoriaDTO> entitys = new List<SpoNumhistoriaDTO>();
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
