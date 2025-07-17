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
    /// Clase de acceso a datos de la tabla EQ_TIPOAREA
    /// </summary>
    public class EqTipoareaRepository: RepositoryBase, IEqTipoareaRepository
    {
        public EqTipoareaRepository(string strConn): base(strConn)
        {
        }

        EqTipoareaHelper helper = new EqTipoareaHelper();

        public int Save(EqTipoareaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tareacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tareaabrev, DbType.String, entity.Tareaabrev);
            dbProvider.AddInParameter(command, helper.Tareanomb, DbType.String, entity.Tareanomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EqTipoareaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tareacodi, DbType.Int32, entity.Tareacodi);
            dbProvider.AddInParameter(command, helper.Tareaabrev, DbType.String, entity.Tareaabrev);
            dbProvider.AddInParameter(command, helper.Tareanomb, DbType.String, entity.Tareanomb);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tareacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tareacodi, DbType.Int32, tareacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqTipoareaDTO GetById(int tareacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tareacodi, DbType.Int32, tareacodi);
            EqTipoareaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EqTipoareaDTO> List()
        {
            List<EqTipoareaDTO> entitys = new List<EqTipoareaDTO>();
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

        public List<EqTipoareaDTO> GetByCriteria()
        {
            List<EqTipoareaDTO> entitys = new List<EqTipoareaDTO>();
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

        #region GESPROTEC 10/01/2025
        public List<EqTipoareaDTO> ListProtecciones()
        {
            List<EqTipoareaDTO> entitys = new List<EqTipoareaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListProtecciones);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        #endregion
    }
}
