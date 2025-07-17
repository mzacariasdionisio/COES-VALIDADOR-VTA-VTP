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
    /// Clase de acceso a datos de la tabla WB_PUBLICACION
    /// </summary>
    public class WbPublicacionRepository: RepositoryBase, IWbPublicacionRepository
    {
        public WbPublicacionRepository(string strConn): base(strConn)
        {
        }

        WbPublicacionHelper helper = new WbPublicacionHelper();

        public int Save(WbPublicacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Publiccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Publicnombre, DbType.String, entity.Publicnombre);
            dbProvider.AddInParameter(command, helper.Publicestado, DbType.String, entity.Publicestado);
            dbProvider.AddInParameter(command, helper.Publicplantilla, DbType.String, entity.Publicplantilla);
            dbProvider.AddInParameter(command, helper.Publicasunto, DbType.String, entity.Publicasunto);
            dbProvider.AddInParameter(command, helper.Publicemail, DbType.String, entity.Publicemail);
            dbProvider.AddInParameter(command, helper.Publicemail1, DbType.String, entity.Publicemail1);
            dbProvider.AddInParameter(command, helper.Publicemail2, DbType.String, entity.Publicemail2);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbPublicacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                        
            dbProvider.AddInParameter(command, helper.Publicnombre, DbType.String, entity.Publicnombre);
            dbProvider.AddInParameter(command, helper.Publicestado, DbType.String, entity.Publicestado);
            dbProvider.AddInParameter(command, helper.Publicplantilla, DbType.String, entity.Publicplantilla);
            dbProvider.AddInParameter(command, helper.Publicasunto, DbType.String, entity.Publicasunto);
            dbProvider.AddInParameter(command, helper.Publicemail, DbType.String, entity.Publicemail);
            dbProvider.AddInParameter(command, helper.Publicemail1, DbType.String, entity.Publicemail1);
            dbProvider.AddInParameter(command, helper.Publicemail2, DbType.String, entity.Publicemail2);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);
            dbProvider.AddInParameter(command, helper.Publiccodi, DbType.Int32, entity.Publiccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int publiccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Publiccodi, DbType.Int32, publiccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbPublicacionDTO GetById(int publiccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Publiccodi, DbType.Int32, publiccodi);
            WbPublicacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbPublicacionDTO> List()
        {
            List<WbPublicacionDTO> entitys = new List<WbPublicacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbPublicacionDTO entity = helper.Create(dr);

                    int iAreaname = dr.GetOrdinal(helper.Areaname);
                    if (!dr.IsDBNull(iAreaname)) entity.Areaname = dr.GetString(iAreaname);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<WbPublicacionDTO> GetByCriteria()
        {
            List<WbPublicacionDTO> entitys = new List<WbPublicacionDTO>();
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
