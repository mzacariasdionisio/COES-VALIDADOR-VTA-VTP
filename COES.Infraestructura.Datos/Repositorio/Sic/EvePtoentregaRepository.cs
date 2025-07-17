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
    /// Clase de acceso a datos de la tabla EVE_PTOENTREGA
    /// </summary>
    public class EvePtoentregaRepository: RepositoryBase, IEvePtoentregaRepository
    {
        public EvePtoentregaRepository(string strConn): base(strConn)
        {
        }

        EvePtoentregaHelper helper = new EvePtoentregaHelper();

        public int Save(EvePtoentregaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ptoentregacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Clientecodi, DbType.Int32, entity.Clientecodi);
            dbProvider.AddInParameter(command, helper.Ptoentrenomb, DbType.String, entity.Ptoentrenomb);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EvePtoentregaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Clientecodi, DbType.Int32, entity.Clientecodi);
            dbProvider.AddInParameter(command, helper.Ptoentregacodi, DbType.Int32, entity.Ptoentregacodi);
            dbProvider.AddInParameter(command, helper.Ptoentrenomb, DbType.String, entity.Ptoentrenomb);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ptoentregacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ptoentregacodi, DbType.Int32, ptoentregacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EvePtoentregaDTO GetById(int ptoentregacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ptoentregacodi, DbType.Int32, ptoentregacodi);
            EvePtoentregaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EvePtoentregaDTO> List()
        {
            List<EvePtoentregaDTO> entitys = new List<EvePtoentregaDTO>();
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

        public List<EvePtoentregaDTO> GetByCriteria()
        {
            List<EvePtoentregaDTO> entitys = new List<EvePtoentregaDTO>();
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
