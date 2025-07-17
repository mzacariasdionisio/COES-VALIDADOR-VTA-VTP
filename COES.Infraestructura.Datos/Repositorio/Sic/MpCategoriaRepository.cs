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
    /// Clase de acceso a datos de la tabla MP_CATEGORIA
    /// </summary>
    public class MpCategoriaRepository: RepositoryBase, IMpCategoriaRepository
    {
        public MpCategoriaRepository(string strConn): base(strConn)
        {
        }

        MpCategoriaHelper helper = new MpCategoriaHelper();

        public int Save(MpCategoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mcatcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mcatnomb, DbType.String, entity.Mcatnomb);
            dbProvider.AddInParameter(command, helper.Mcatabrev, DbType.String, entity.Mcatabrev);
            dbProvider.AddInParameter(command, helper.Mcattipo, DbType.Int32, entity.Mcattipo);
            dbProvider.AddInParameter(command, helper.Mcatdesc, DbType.String, entity.Mcatdesc);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MpCategoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mcatcodi, DbType.Int32, entity.Mcatcodi);
            dbProvider.AddInParameter(command, helper.Mcatnomb, DbType.String, entity.Mcatnomb);
            dbProvider.AddInParameter(command, helper.Mcatabrev, DbType.String, entity.Mcatabrev);
            dbProvider.AddInParameter(command, helper.Mcattipo, DbType.Int32, entity.Mcattipo);
            dbProvider.AddInParameter(command, helper.Mcatdesc, DbType.String, entity.Mcatdesc);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mcatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mcatcodi, DbType.Int32, mcatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MpCategoriaDTO GetById(int mcatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mcatcodi, DbType.Int32, mcatcodi);
            MpCategoriaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MpCategoriaDTO> List()
        {
            List<MpCategoriaDTO> entitys = new List<MpCategoriaDTO>();
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

        public List<MpCategoriaDTO> GetByCriteria()
        {
            List<MpCategoriaDTO> entitys = new List<MpCategoriaDTO>();
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
