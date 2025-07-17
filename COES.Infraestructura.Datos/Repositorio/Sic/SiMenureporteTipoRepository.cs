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
    /// Clase de acceso a datos de la tabla SI_MENUREPORTE_TIPO
    /// </summary>
    public class SiMenureporteTipoRepository: RepositoryBase, ISiMenureporteTipoRepository
    {
        public SiMenureporteTipoRepository(string strConn): base(strConn)
        {
        }

        SiMenureporteTipoHelper helper = new SiMenureporteTipoHelper();

        public int Save(SiMenureporteTipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mreptipcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mreptipdescripcion, DbType.String, entity.Mreptipdescripcion);
            dbProvider.AddInParameter(command, helper.Mprojcodi, DbType.Int32, entity.Mprojcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiMenureporteTipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mreptipcodi, DbType.Int32, entity.Mreptipcodi);
            dbProvider.AddInParameter(command, helper.Mreptipdescripcion, DbType.String, entity.Mreptipdescripcion);
            dbProvider.AddInParameter(command, helper.Mprojcodi, DbType.Int32, entity.Mprojcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mreptipcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mreptipcodi, DbType.Int32, mreptipcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMenureporteTipoDTO GetById(int mreptipcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mreptipcodi, DbType.Int32, mreptipcodi);
            SiMenureporteTipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMenureporteTipoDTO> List()
        {
            List<SiMenureporteTipoDTO> entitys = new List<SiMenureporteTipoDTO>();
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

        public List<SiMenureporteTipoDTO> GetByCriteria(int mprojcodi)
        {
            List<SiMenureporteTipoDTO> entitys = new List<SiMenureporteTipoDTO>();
            string query = string.Format(helper.SqlGetByCriteria, mprojcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
