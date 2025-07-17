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
    /// Clase de acceso a datos de la tabla SPO_CLASIFICACION
    /// </summary>
    public class SpoClasificacionRepository: RepositoryBase, ISpoClasificacionRepository
    {
        public SpoClasificacionRepository(string strConn): base(strConn)
        {
        }

        SpoClasificacionHelper helper = new SpoClasificacionHelper();

        public int Save(SpoClasificacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Clasicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Clasinombre, DbType.String, entity.Clasinombre);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SpoClasificacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Clasicodi, DbType.Int32, entity.Clasicodi);
            dbProvider.AddInParameter(command, helper.Clasinombre, DbType.String, entity.Clasinombre);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int clasicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Clasicodi, DbType.Int32, clasicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SpoClasificacionDTO GetById(int clasicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Clasicodi, DbType.Int32, clasicodi);
            SpoClasificacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SpoClasificacionDTO> List()
        {
            List<SpoClasificacionDTO> entitys = new List<SpoClasificacionDTO>();
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

        public List<SpoClasificacionDTO> GetByCriteria()
        {
            List<SpoClasificacionDTO> entitys = new List<SpoClasificacionDTO>();
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
