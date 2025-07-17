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
    /// Clase de acceso a datos de la tabla FT_EXT_EVENTO
    /// </summary>
    public class FtExtEventoRepository : RepositoryBase, IFtExtEventoRepository
    {
        public FtExtEventoRepository(string strConn) : base(strConn)
        {
        }

        FtExtEventoHelper helper = new FtExtEventoHelper();

        public int Save(FtExtEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftevcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftevnombre, DbType.String, entity.Ftevnombre);
            dbProvider.AddInParameter(command, helper.Ftevfecvigenciaext, DbType.DateTime, entity.Ftevfecvigenciaext);
            dbProvider.AddInParameter(command, helper.Ftevestado, DbType.String, entity.Ftevestado);
            dbProvider.AddInParameter(command, helper.Ftevusucreacion, DbType.String, entity.Ftevusucreacion);
            dbProvider.AddInParameter(command, helper.Ftevfeccreacion, DbType.DateTime, entity.Ftevfeccreacion);
            dbProvider.AddInParameter(command, helper.Ftevusumodificacion, DbType.String, entity.Ftevusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftevfecmodificacion, DbType.DateTime, entity.Ftevfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ftevusumodificacionasig, DbType.String, entity.Ftevusumodificacionasig);
            dbProvider.AddInParameter(command, helper.Ftevfecmodificacionasig, DbType.DateTime, entity.Ftevfecmodificacionasig);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtExtEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);


            dbProvider.AddInParameter(command, helper.Ftevnombre, DbType.String, entity.Ftevnombre);
            dbProvider.AddInParameter(command, helper.Ftevfecvigenciaext, DbType.DateTime, entity.Ftevfecvigenciaext);
            dbProvider.AddInParameter(command, helper.Ftevestado, DbType.String, entity.Ftevestado);
            dbProvider.AddInParameter(command, helper.Ftevusucreacion, DbType.String, entity.Ftevusucreacion);
            dbProvider.AddInParameter(command, helper.Ftevfeccreacion, DbType.DateTime, entity.Ftevfeccreacion);
            dbProvider.AddInParameter(command, helper.Ftevusumodificacion, DbType.String, entity.Ftevusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftevfecmodificacion, DbType.DateTime, entity.Ftevfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ftevusumodificacionasig, DbType.String, entity.Ftevusumodificacionasig);
            dbProvider.AddInParameter(command, helper.Ftevfecmodificacionasig, DbType.DateTime, entity.Ftevfecmodificacionasig);

            dbProvider.AddInParameter(command, helper.Ftevcodi, DbType.Int32, entity.Ftevcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ftevcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftevcodi, DbType.Int32, ftevcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEventoDTO GetById(int ftevcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftevcodi, DbType.Int32, ftevcodi);
            FtExtEventoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEventoDTO> List()
        {
            List<FtExtEventoDTO> entitys = new List<FtExtEventoDTO>();
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

        public List<FtExtEventoDTO> GetByCriteria()
        {
            List<FtExtEventoDTO> entitys = new List<FtExtEventoDTO>();
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
