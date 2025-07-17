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
    /// Clase de acceso a datos de la tabla SI_MENUREPORTEDET
    /// </summary>
    public class SiMenureportedetRepository: RepositoryBase, ISiMenureportedetRepository
    {
        public SiMenureportedetRepository(string strConn): base(strConn)
        {
        }

        SiMenureportedetHelper helper = new SiMenureportedetHelper();

        public int Save(SiMenureportedetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mrepdcodigo, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mrepcodi, DbType.Int32, entity.Mrepcodi);
            dbProvider.AddInParameter(command, helper.Mrepdtitulo, DbType.String, entity.Mrepdtitulo);
            dbProvider.AddInParameter(command, helper.Mrepdestado, DbType.Int32, entity.Mrepdestado);
            dbProvider.AddInParameter(command, helper.Mrepdorden, DbType.Int32, entity.Mrepdorden);
            dbProvider.AddInParameter(command, helper.Mrepdusucreacion, DbType.String, entity.Mrepdusucreacion);
            dbProvider.AddInParameter(command, helper.Mrepdfeccreacion, DbType.DateTime, entity.Mrepdfeccreacion);
            dbProvider.AddInParameter(command, helper.Mrepdusumodificacion, DbType.String, entity.Mrepdusumodificacion);
            dbProvider.AddInParameter(command, helper.Mrepdfecmodificacion, DbType.DateTime, entity.Mrepdfecmodificacion);
            dbProvider.AddInParameter(command, helper.Mrepddescripcion, DbType.String, entity.Mrepddescripcion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiMenureportedetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mrepdcodigo, DbType.Int32, entity.Mrepdcodigo);
            dbProvider.AddInParameter(command, helper.Mrepcodi, DbType.Int32, entity.Mrepcodi);
            dbProvider.AddInParameter(command, helper.Mrepdtitulo, DbType.String, entity.Mrepdtitulo);
            dbProvider.AddInParameter(command, helper.Mrepdestado, DbType.Int32, entity.Mrepdestado);
            dbProvider.AddInParameter(command, helper.Mrepdorden, DbType.Int32, entity.Mrepdorden);
            dbProvider.AddInParameter(command, helper.Mrepdusucreacion, DbType.String, entity.Mrepdusucreacion);
            dbProvider.AddInParameter(command, helper.Mrepdfeccreacion, DbType.DateTime, entity.Mrepdfeccreacion);
            dbProvider.AddInParameter(command, helper.Mrepdusumodificacion, DbType.String, entity.Mrepdusumodificacion);
            dbProvider.AddInParameter(command, helper.Mrepdfecmodificacion, DbType.DateTime, entity.Mrepdfecmodificacion);
            dbProvider.AddInParameter(command, helper.Mrepddescripcion, DbType.String, entity.Mrepddescripcion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mrepdcodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mrepdcodigo, DbType.Int32, mrepdcodigo);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMenureportedetDTO GetById(int mrepdcodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mrepdcodigo, DbType.Int32, mrepdcodigo);
            SiMenureportedetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMenureportedetDTO> List()
        {
            List<SiMenureportedetDTO> entitys = new List<SiMenureportedetDTO>();
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

        public List<SiMenureportedetDTO> GetByCriteria(int tmrepcodi)
        {
            List<SiMenureportedetDTO> entitys = new List<SiMenureportedetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Tmrepcodi, DbType.Int32, tmrepcodi);

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
