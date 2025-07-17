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
    /// Clase de acceso a datos de la tabla EN_ENSAYOMODEQUI
    /// </summary>
    public class EnEnsayomodequiRepository : RepositoryBase, IEnEnsayomodequiRepository
    {
        public EnEnsayomodequiRepository(string strConn)
            : base(strConn)
        {
        }

        EnEnsayomodequiHelper helper = new EnEnsayomodequiHelper();

        public int Save(EnEnsayomodequiDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Enmodocodi, DbType.Int32, entity.Enmodocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Enmoeqcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EnEnsayomodequiDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Enmodocodi, DbType.Int32, entity.Enmodocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Enmoeqcodi, DbType.Int32, entity.Enmoeqcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int enmoeqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Enmoeqcodi, DbType.Int32, enmoeqcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EnEnsayomodequiDTO GetById(int enmoeqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Enmoeqcodi, DbType.Int32, enmoeqcodi);
            EnEnsayomodequiDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EnEnsayomodequiDTO> List()
        {
            List<EnEnsayomodequiDTO> entitys = new List<EnEnsayomodequiDTO>();
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

        public List<EnEnsayomodequiDTO> GetByCriteria()
        {
            List<EnEnsayomodequiDTO> entitys = new List<EnEnsayomodequiDTO>();
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
