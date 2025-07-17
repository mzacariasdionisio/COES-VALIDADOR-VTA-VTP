using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CB_CONCEPTOCOMB
    /// </summary>
    public class CbConceptocombRepository : RepositoryBase, ICbConceptocombRepository
    {
        public CbConceptocombRepository(string strConn) : base(strConn)
        {
        }

        readonly CbConceptocombHelper helper = new CbConceptocombHelper();

        public int Save(CbConceptocombDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ccombcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ccombnombre, DbType.String, entity.Ccombnombre);
            dbProvider.AddInParameter(command, helper.Ccombnombreficha, DbType.String, entity.Ccombnombreficha);
            dbProvider.AddInParameter(command, helper.Ccombunidad, DbType.String, entity.Ccombunidad);
            dbProvider.AddInParameter(command, helper.Ccomborden, DbType.Int32, entity.Ccomborden);
            dbProvider.AddInParameter(command, helper.Ccombabrev, DbType.String, entity.Ccombabrev);
            dbProvider.AddInParameter(command, helper.Ccombtipo, DbType.String, entity.Ccombtipo);
            dbProvider.AddInParameter(command, helper.Ccombreadonly, DbType.String, entity.Ccombreadonly);
            dbProvider.AddInParameter(command, helper.Ccombnumeral, DbType.Int32, entity.Ccombnumeral);
            dbProvider.AddInParameter(command, helper.Ccombtunidad, DbType.Int32, entity.Ccombtunidad);
            dbProvider.AddInParameter(command, helper.Ccombseparador, DbType.Int32, entity.Ccombseparador);
            dbProvider.AddInParameter(command, helper.Ccombnumdecimal, DbType.Int32, entity.Ccombnumdecimal);
            dbProvider.AddInParameter(command, helper.Ccombtipo2, DbType.Int32, entity.Ccombtipo2);
            dbProvider.AddInParameter(command, helper.Ccombunidad2, DbType.Int32, entity.Ccombunidad2);
            dbProvider.AddInParameter(command, helper.Ccombestado, DbType.Int32, entity.Ccombestado);
            dbProvider.AddInParameter(command, helper.Estcomcodi, DbType.Int32, entity.Estcomcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CbConceptocombDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ccombnombre, DbType.String, entity.Ccombnombre);
            dbProvider.AddInParameter(command, helper.Ccombnombreficha, DbType.String, entity.Ccombnombreficha);
            dbProvider.AddInParameter(command, helper.Ccombunidad, DbType.String, entity.Ccombunidad);
            dbProvider.AddInParameter(command, helper.Ccomborden, DbType.Int32, entity.Ccomborden);
            dbProvider.AddInParameter(command, helper.Ccombabrev, DbType.String, entity.Ccombabrev);
            dbProvider.AddInParameter(command, helper.Ccombtipo, DbType.String, entity.Ccombtipo);
            dbProvider.AddInParameter(command, helper.Ccombreadonly, DbType.String, entity.Ccombreadonly);
            dbProvider.AddInParameter(command, helper.Ccombnumeral, DbType.Int32, entity.Ccombnumeral);
            dbProvider.AddInParameter(command, helper.Ccombtunidad, DbType.Int32, entity.Ccombtunidad);
            dbProvider.AddInParameter(command, helper.Ccombseparador, DbType.Int32, entity.Ccombseparador);
            dbProvider.AddInParameter(command, helper.Ccombnumdecimal, DbType.Int32, entity.Ccombnumdecimal);
            dbProvider.AddInParameter(command, helper.Ccombtipo2, DbType.Int32, entity.Ccombtipo2);
            dbProvider.AddInParameter(command, helper.Ccombunidad2, DbType.Int32, entity.Ccombunidad2);
            dbProvider.AddInParameter(command, helper.Ccombestado, DbType.Int32, entity.Ccombestado);
            dbProvider.AddInParameter(command, helper.Estcomcodi, DbType.Int32, entity.Estcomcodi);

            dbProvider.AddInParameter(command, helper.Ccombcodi, DbType.Int32, entity.Ccombcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ccombcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ccombcodi, DbType.Int32, ccombcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbConceptocombDTO GetById(int ccombcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ccombcodi, DbType.Int32, ccombcodi);
            CbConceptocombDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbConceptocombDTO> List()
        {
            List<CbConceptocombDTO> entitys = new List<CbConceptocombDTO>();
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

        public List<CbConceptocombDTO> GetByCriteria(int estcomcodi)
        {
            List<CbConceptocombDTO> entitys = new List<CbConceptocombDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, estcomcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
