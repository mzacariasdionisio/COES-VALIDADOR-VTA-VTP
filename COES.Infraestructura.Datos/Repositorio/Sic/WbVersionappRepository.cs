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
    /// Clase de acceso a datos de la tabla WB_VERSIONAPP
    /// </summary>
    public class WbVersionappRepository: RepositoryBase, IWbVersionappRepository
    {
        public WbVersionappRepository(string strConn): base(strConn)
        {
        }

        WbVersionappHelper helper = new WbVersionappHelper();

        public int Save(WbVersionappDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Verappcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Verappios, DbType.Decimal, entity.Verappios);
            dbProvider.AddInParameter(command, helper.Verappandroid, DbType.Decimal, entity.Verappandroid);
            dbProvider.AddInParameter(command, helper.Verappdescripcion, DbType.String, entity.Verappdescripcion);
            dbProvider.AddInParameter(command, helper.Verappusucreacion, DbType.String, entity.Verappusucreacion);
            dbProvider.AddInParameter(command, helper.Verappfeccreacion, DbType.DateTime, entity.Verappfeccreacion);
            dbProvider.AddInParameter(command, helper.Verappusumodificacion, DbType.String, entity.Verappusumodificacion);
            dbProvider.AddInParameter(command, helper.Verappfecmodificacion, DbType.DateTime, entity.Verappfecmodificacion);
            dbProvider.AddInParameter(command, helper.Verappvigente, DbType.String, entity.Verappvigente);
            dbProvider.AddInParameter(command, helper.Verappupdate, DbType.Int32, entity.Verappupdate);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbVersionappDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Verappios, DbType.Decimal, entity.Verappios);
            dbProvider.AddInParameter(command, helper.Verappandroid, DbType.Decimal, entity.Verappandroid);
            dbProvider.AddInParameter(command, helper.Verappdescripcion, DbType.String, entity.Verappdescripcion);
            dbProvider.AddInParameter(command, helper.Verappusucreacion, DbType.String, entity.Verappusucreacion);
            dbProvider.AddInParameter(command, helper.Verappfeccreacion, DbType.DateTime, entity.Verappfeccreacion);
            dbProvider.AddInParameter(command, helper.Verappusumodificacion, DbType.String, entity.Verappusumodificacion);
            dbProvider.AddInParameter(command, helper.Verappfecmodificacion, DbType.DateTime, entity.Verappfecmodificacion);
            dbProvider.AddInParameter(command, helper.Verappvigente, DbType.String, entity.Verappvigente);
            dbProvider.AddInParameter(command, helper.Verappupdate, DbType.Int32, entity.Verappupdate);
            dbProvider.AddInParameter(command, helper.Verappcodi, DbType.Int32, entity.Verappcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int verappcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Verappcodi, DbType.Int32, verappcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbVersionappDTO GetById(int verappcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Verappcodi, DbType.Int32, verappcodi);
            WbVersionappDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbVersionappDTO> List()
        {
            List<WbVersionappDTO> entitys = new List<WbVersionappDTO>();
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

        public List<WbVersionappDTO> GetByCriteria()
        {
            List<WbVersionappDTO> entitys = new List<WbVersionappDTO>();
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

        public WbVersionappDTO ObtenerVersionActual()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerVersionActual);
                        
            WbVersionappDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
