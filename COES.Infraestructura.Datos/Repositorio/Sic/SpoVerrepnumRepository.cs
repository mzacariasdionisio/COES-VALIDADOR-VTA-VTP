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
    /// Clase de acceso a datos de la tabla SPO_VERREPNUM
    /// </summary>
    public class SpoVerrepnumRepository: RepositoryBase, ISpoVerrepnumRepository
    {
        public SpoVerrepnumRepository(string strConn): base(strConn)
        {
        }

        SpoVerrepnumHelper helper = new SpoVerrepnumHelper();

        public int Save(SpoVerrepnumDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Verrcodi, DbType.Int32, entity.Verrcodi);
            dbProvider.AddInParameter(command, helper.Verncodi, DbType.Int32, entity.Verncodi);
            dbProvider.AddInParameter(command, helper.Verrncodi, DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SpoVerrepnumDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Verrcodi, DbType.Int32, entity.Verrcodi);
            dbProvider.AddInParameter(command, helper.Verncodi, DbType.Int32, entity.Verncodi);
            dbProvider.AddInParameter(command, helper.Verrncodi, DbType.Int32, entity.Verrncodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int verrncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Verrncodi, DbType.Int32, verrncodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SpoVerrepnumDTO GetById(int verrncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Verrncodi, DbType.Int32, verrncodi);
            SpoVerrepnumDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SpoVerrepnumDTO> List()
        {
            List<SpoVerrepnumDTO> entitys = new List<SpoVerrepnumDTO>();
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

        public List<SpoVerrepnumDTO> GetByCriteria()
        {
            List<SpoVerrepnumDTO> entitys = new List<SpoVerrepnumDTO>();
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

        public List<SpoVerrepnumDTO> GetByVersionReporte(int verrcodi)
        {
            List<SpoVerrepnumDTO> entitys = new List<SpoVerrepnumDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByVersionReporte);
            dbProvider.AddInParameter(command, helper.Verrcodi, DbType.Int32, verrcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iVernnro = dr.GetOrdinal(helper.Vernnro);
                    if (!dr.IsDBNull(iVernnro)) entity.Vernnro = Convert.ToInt32(dr.GetValue(iVernnro));

                    int iNumecodi = dr.GetOrdinal(helper.Numecodi);
                    if (!dr.IsDBNull(iNumecodi)) entity.Numecodi = Convert.ToInt32(dr.GetValue(iNumecodi));

                    int iNumhisabrev = dr.GetOrdinal(helper.Numhisabrev);
                    if (!dr.IsDBNull(iVernnro)) entity.Numhisabrev = dr.GetString(iNumhisabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
