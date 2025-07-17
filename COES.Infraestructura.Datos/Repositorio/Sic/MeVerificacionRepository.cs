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
    /// Clase de acceso a datos de la tabla ME_VERIFICACION
    /// </summary>
    public class MeVerificacionRepository : RepositoryBase, IMeVerificacionRepository
    {
        public MeVerificacionRepository(string strConn)
            : base(strConn)
        {
        }

        MeVerificacionHelper helper = new MeVerificacionHelper();

        public int Save(MeVerificacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Verifcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Verifnomb, DbType.String, entity.Verifnomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MeVerificacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Verifcodi, DbType.Int32, entity.Verifcodi);
            dbProvider.AddInParameter(command, helper.Verifnomb, DbType.String, entity.Verifnomb);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int verifcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Verifcodi, DbType.Int32, verifcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeVerificacionDTO GetById(int verifcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Verifcodi, DbType.Int32, verifcodi);
            MeVerificacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeVerificacionDTO> List()
        {
            List<MeVerificacionDTO> entitys = new List<MeVerificacionDTO>();
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

        public List<MeVerificacionDTO> GetByCriteria()
        {
            List<MeVerificacionDTO> entitys = new List<MeVerificacionDTO>();
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
