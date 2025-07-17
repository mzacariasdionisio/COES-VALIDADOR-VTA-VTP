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
    /// Clase de acceso a datos de la tabla EQ_AREANIVEL
    /// </summary>
    public class EqAreanivelRepository: RepositoryBase, IEqAreanivelRepository
    {
        public EqAreanivelRepository(string strConn): base(strConn)
        {
        }

        EqAreanivelHelper helper = new EqAreanivelHelper();

        public int Save(EqAreaNivelDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Anivelnomb, DbType.String, entity.ANivelNomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EqAreaNivelDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, entity.ANivelCodi);
            dbProvider.AddInParameter(command, helper.Anivelnomb, DbType.String, entity.ANivelNomb);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int anivelcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, anivelcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqAreaNivelDTO GetById(int anivelcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, anivelcodi);
            EqAreaNivelDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EqAreaNivelDTO> List()
        {
            List<EqAreaNivelDTO> entitys = new List<EqAreaNivelDTO>();
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

        public List<EqAreaNivelDTO> GetByCriteria()
        {
            List<EqAreaNivelDTO> entitys = new List<EqAreaNivelDTO>();
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
