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
    /// Clase de acceso a datos de la tabla CM_RESTRICCION
    /// </summary>
    public class CmRestriccionRepository: RepositoryBase, ICmRestriccionRepository
    {
        public CmRestriccionRepository(string strConn): base(strConn)
        {
        }

        CmRestriccionHelper helper = new CmRestriccionHelper();

        public int Save(CmRestriccionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmrestcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, entity.Cmgncorrelativo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmRestriccionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, entity.Cmgncorrelativo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Cmrestcodi, DbType.Int32, entity.Cmrestcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmrestcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmrestcodi, DbType.Int32, cmrestcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmRestriccionDTO GetById(int cmrestcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmrestcodi, DbType.Int32, cmrestcodi);
            CmRestriccionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmRestriccionDTO> List()
        {
            List<CmRestriccionDTO> entitys = new List<CmRestriccionDTO>();
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

        public List<CmRestriccionDTO> GetByCriteria()
        {
            List<CmRestriccionDTO> entitys = new List<CmRestriccionDTO>();
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

        public List<CmRestriccionDTO> ObtenerRestriccionPorCorrida(int correlativo)
        {
            List<CmRestriccionDTO> entitys = new List<CmRestriccionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerRestriccionPorCorrida);
            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, correlativo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmRestriccionDTO entity = helper.Create(dr);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iSubcausaabrev = dr.GetOrdinal(helper.Subcausaabrev);
                    if (!dr.IsDBNull(iSubcausaabrev)) entity.Subcausaabrev = dr.GetString(iSubcausaabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
