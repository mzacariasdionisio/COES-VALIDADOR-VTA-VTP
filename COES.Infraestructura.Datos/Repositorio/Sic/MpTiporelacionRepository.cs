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
    /// Clase de acceso a datos de la tabla MP_TIPORELACION
    /// </summary>
    public class MpTiporelacionRepository: RepositoryBase, IMpTiporelacionRepository
    {
        public MpTiporelacionRepository(string strConn): base(strConn)
        {
        }

        MpTiporelacionHelper helper = new MpTiporelacionHelper();

        public int Save(MpTiporelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mtrelcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mtrelnomb, DbType.String, entity.Mtrelnomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MpTiporelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mtrelcodi, DbType.Int32, entity.Mtrelcodi);
            dbProvider.AddInParameter(command, helper.Mtrelnomb, DbType.String, entity.Mtrelnomb);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mtrelcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mtrelcodi, DbType.Int32, mtrelcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MpTiporelacionDTO GetById(int mtrelcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mtrelcodi, DbType.Int32, mtrelcodi);
            MpTiporelacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MpTiporelacionDTO> List()
        {
            List<MpTiporelacionDTO> entitys = new List<MpTiporelacionDTO>();
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

        public List<MpTiporelacionDTO> GetByCriteria()
        {
            List<MpTiporelacionDTO> entitys = new List<MpTiporelacionDTO>();
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
