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
    /// Clase de acceso a datos de la tabla CB_REP_PROPIEDAD
    /// </summary>
    public class CbRepPropiedadRepository: RepositoryBase, ICbRepPropiedadRepository
    {
        public CbRepPropiedadRepository(string strConn): base(strConn)
        {
        }

        CbRepPropiedadHelper helper = new CbRepPropiedadHelper();

        public int Save(CbRepPropiedadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cbrprocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cbrpronombre, DbType.String, entity.Cbrpronombre);
            dbProvider.AddInParameter(command, helper.Cbrprovalor, DbType.String, entity.Cbrprovalor);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CbRepPropiedadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbrprocodi, DbType.Int32, entity.Cbrprocodi);
            dbProvider.AddInParameter(command, helper.Cbrpronombre, DbType.String, entity.Cbrpronombre);
            dbProvider.AddInParameter(command, helper.Cbrprovalor, DbType.String, entity.Cbrprovalor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbrprocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbrprocodi, DbType.Int32, cbrprocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbRepPropiedadDTO GetById(int cbrprocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbrprocodi, DbType.Int32, cbrprocodi);
            CbRepPropiedadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbRepPropiedadDTO> List()
        {
            List<CbRepPropiedadDTO> entitys = new List<CbRepPropiedadDTO>();
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

        public List<CbRepPropiedadDTO> GetByCriteria()
        {
            List<CbRepPropiedadDTO> entitys = new List<CbRepPropiedadDTO>();
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
