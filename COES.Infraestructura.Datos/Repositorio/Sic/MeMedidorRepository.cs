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
    /// Clase de acceso a datos de la tabla ME_MEDIDOR
    /// </summary>
    public class MeMedidorRepository: RepositoryBase, IMeMedidorRepository
    {
        public MeMedidorRepository(string strConn): base(strConn)
        {
        }

        MeMedidorHelper helper = new MeMedidorHelper();

        public int Save(MeMedidorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Medicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Medinombre, DbType.String, entity.Medinombre);           
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Meditipo, DbType.String, entity.Meditipo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MeMedidorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Medicodi, DbType.Int32, entity.Medicodi);
            dbProvider.AddInParameter(command, helper.Medinombre, DbType.String, entity.Medinombre);           
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Meditipo, DbType.String, entity.Meditipo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int medicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Medicodi, DbType.Int32, medicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeMedidorDTO GetById(int medicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Medicodi, DbType.Int32, medicodi);
            MeMedidorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeMedidorDTO> List()
        {
            List<MeMedidorDTO> entitys = new List<MeMedidorDTO>();
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

        public List<MeMedidorDTO> GetByCriteria()
        {
            List<MeMedidorDTO> entitys = new List<MeMedidorDTO>();
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
