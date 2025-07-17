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
    /// Clase de acceso a datos de la tabla FT_FICTEC_VISUALENTIDAD
    /// </summary>
    public class FtFictecVisualentidadRepository : RepositoryBase, IFtFictecVisualentidadRepository
    {
        public FtFictecVisualentidadRepository(string strConn) : base(strConn)
        {
        }

        FtFictecVisualentidadHelper helper = new FtFictecVisualentidadHelper();

        public int Save(FtFictecVisualentidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftvercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftverusucreacion, DbType.String, entity.Ftverusucreacion);
            dbProvider.AddInParameter(command, helper.Ftverocultoportal, DbType.String, entity.Ftverocultoportal);
            dbProvider.AddInParameter(command, helper.Ftverfecmodificacion, DbType.DateTime, entity.Ftverfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ftverfeccreacion, DbType.DateTime, entity.Ftverfeccreacion);
            dbProvider.AddInParameter(command, helper.Ftverusumodificacion, DbType.String, entity.Ftverusumodificacion);
            dbProvider.AddInParameter(command, helper.Fteqcodi, DbType.Int32, entity.Fteqcodi);
            dbProvider.AddInParameter(command, helper.Ftvercodisicoes, DbType.Int32, entity.Ftvercodisicoes);
            dbProvider.AddInParameter(command, helper.Ftvertipoentidad, DbType.String, entity.Ftvertipoentidad);
            dbProvider.AddInParameter(command, helper.Ftverocultoextranet, DbType.String, entity.Ftverocultoextranet);
            dbProvider.AddInParameter(command, helper.Ftverocultointranet, DbType.String, entity.Ftverocultointranet);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtFictecVisualentidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftverusucreacion, DbType.String, entity.Ftverusucreacion);
            dbProvider.AddInParameter(command, helper.Ftverocultoportal, DbType.String, entity.Ftverocultoportal);
            dbProvider.AddInParameter(command, helper.Ftverfecmodificacion, DbType.DateTime, entity.Ftverfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ftverfeccreacion, DbType.DateTime, entity.Ftverfeccreacion);
            dbProvider.AddInParameter(command, helper.Ftverusumodificacion, DbType.String, entity.Ftverusumodificacion);
            dbProvider.AddInParameter(command, helper.Fteqcodi, DbType.Int32, entity.Fteqcodi);
            dbProvider.AddInParameter(command, helper.Ftvercodisicoes, DbType.Int32, entity.Ftvercodisicoes);
            dbProvider.AddInParameter(command, helper.Ftvertipoentidad, DbType.String, entity.Ftvertipoentidad);
            dbProvider.AddInParameter(command, helper.Ftverocultoextranet, DbType.String, entity.Ftverocultoextranet);
            dbProvider.AddInParameter(command, helper.Ftverocultointranet, DbType.String, entity.Ftverocultointranet);
            dbProvider.AddInParameter(command, helper.Ftvercodi, DbType.Int32, entity.Ftvercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ftvercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftvercodi, DbType.Int32, ftvercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtFictecVisualentidadDTO GetById(int ftvercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftvercodi, DbType.Int32, ftvercodi);
            FtFictecVisualentidadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtFictecVisualentidadDTO> List()
        {
            List<FtFictecVisualentidadDTO> entitys = new List<FtFictecVisualentidadDTO>();
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

        public List<FtFictecVisualentidadDTO> GetByCriteria()
        {
            List<FtFictecVisualentidadDTO> entitys = new List<FtFictecVisualentidadDTO>();
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
