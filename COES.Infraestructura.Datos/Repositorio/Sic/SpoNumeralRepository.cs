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
    /// Clase de acceso a datos de la tabla SPO_NUMERAL
    /// </summary>
    public class SpoNumeralRepository: RepositoryBase, ISpoNumeralRepository
    {
        public SpoNumeralRepository(string strConn): base(strConn)
        {
        }

        SpoNumeralHelper helper = new SpoNumeralHelper();

        public int Save(SpoNumeralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Numecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Numediaplazo, DbType.Int32, entity.Numediaplazo);
            dbProvider.AddInParameter(command, helper.Numeusucreacion, DbType.String, entity.Numeusucreacion);
            dbProvider.AddInParameter(command, helper.Numefeccreacion, DbType.DateTime, entity.Numefeccreacion);
            dbProvider.AddInParameter(command, helper.Numeactivo, DbType.Int32, entity.Numeactivo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SpoNumeralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Numecodi, DbType.Int32, entity.Numecodi);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Numediaplazo, DbType.Int32, entity.Numediaplazo);
            dbProvider.AddInParameter(command, helper.Numeusucreacion, DbType.String, entity.Numeusucreacion);
            dbProvider.AddInParameter(command, helper.Numefeccreacion, DbType.DateTime, entity.Numefeccreacion);
            dbProvider.AddInParameter(command, helper.Numeactivo, DbType.Int32, entity.Numeactivo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int numecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Numecodi, DbType.Int32, numecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SpoNumeralDTO GetById(int numecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Numecodi, DbType.Int32, numecodi);
            SpoNumeralDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SpoNumeralDTO> List()
        {
            List<SpoNumeralDTO> entitys = new List<SpoNumeralDTO>();
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

        public List<SpoNumeralDTO> GetByCriteria()
        {
            List<SpoNumeralDTO> entitys = new List<SpoNumeralDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iNumhisabrev = dr.GetOrdinal(helper.Numhisabrev);
                    if (!dr.IsDBNull(iNumhisabrev)) entity.Numhisabrev = dr.GetString(iNumhisabrev);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
