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
    /// Clase de acceso a datos de la tabla SPO_CONCEPTO
    /// </summary>
    public class SpoConceptoRepository: RepositoryBase, ISpoConceptoRepository
    {
        public SpoConceptoRepository(string strConn): base(strConn)
        {
        }

        SpoConceptoHelper helper = new SpoConceptoHelper();

        public int Save(SpoConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Sconcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Numccodi, DbType.Int32, entity.Numccodi);
            dbProvider.AddInParameter(command, helper.Sconnomb, DbType.String, entity.Sconnomb);
            dbProvider.AddInParameter(command, helper.Sconactivo, DbType.Int32, entity.Sconactivo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SpoConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Sconcodi, DbType.Int32, entity.Sconcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Numccodi, DbType.Int32, entity.Numccodi);
            dbProvider.AddInParameter(command, helper.Sconnomb, DbType.String, entity.Sconnomb);
            dbProvider.AddInParameter(command, helper.Sconactivo, DbType.Int32, entity.Sconactivo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int sconcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Sconcodi, DbType.Int32, sconcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SpoConceptoDTO GetById(int sconcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Sconcodi, DbType.Int32, sconcodi);
            SpoConceptoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SpoConceptoDTO> List()
        {
            List<SpoConceptoDTO> entitys = new List<SpoConceptoDTO>();
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

        public List<SpoConceptoDTO> GetByCriteria(int numecodi)
        {
            List<SpoConceptoDTO> entitys = new List<SpoConceptoDTO>();
            string sqlQuery = string.Format(helper.SqlGetByCriteria, numecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            SpoConceptoDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iPtomedicalculado1 = dr.GetOrdinal(helper.Ptomedicalculado1);
                    if (!dr.IsDBNull(iPtomedicalculado1)) entity.Ptomedicalculado1 = dr.GetString(iPtomedicalculado1);

                    int iPtomedicalculado2 = dr.GetOrdinal(helper.Ptomedicalculado2);
                    if (!dr.IsDBNull(iPtomedicalculado2)) entity.Ptomedicalculado2 = dr.GetString(iPtomedicalculado2);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
