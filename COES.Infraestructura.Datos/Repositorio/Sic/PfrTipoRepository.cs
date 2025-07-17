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
    /// Clase de acceso a datos de la tabla PFR_TIPO
    /// </summary>
    public class PfrTipoRepository: RepositoryBase, IPfrTipoRepository
    {
        public PfrTipoRepository(string strConn): base(strConn)
        {
        }

        PfrTipoHelper helper = new PfrTipoHelper();

        public int Save(PfrTipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pfrcatcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pfrcatnomb, DbType.String, entity.Pfrcatnomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfrTipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrcatcodi, DbType.Int32, entity.Pfrcatcodi);
            dbProvider.AddInParameter(command, helper.Pfrcatnomb, DbType.String, entity.Pfrcatnomb);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrcatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrcatcodi, DbType.Int32, pfrcatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfrTipoDTO GetById(int pfrcatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrcatcodi, DbType.Int32, pfrcatcodi);
            PfrTipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfrTipoDTO> List()
        {
            List<PfrTipoDTO> entitys = new List<PfrTipoDTO>();
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

        public List<PfrTipoDTO> GetByCriteria()
        {
            List<PfrTipoDTO> entitys = new List<PfrTipoDTO>();
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
