using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla MMM_VERSION
    /// </summary>
    public class MmmVersionRepository : RepositoryBase, IMmmVersionRepository
    {
        public MmmVersionRepository(string strConn)
            : base(strConn)
        {
        }

        MmmVersionHelper helper = new MmmVersionHelper();

        public int Save(MmmVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Vermmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vermmfechaperiodo, DbType.DateTime, entity.Vermmfechaperiodo);
            dbProvider.AddInParameter(command, helper.Vermmnumero, DbType.Int32, entity.Vermmnumero);
            dbProvider.AddInParameter(command, helper.Vermmusucreacion, DbType.String, entity.Vermmusucreacion);
            dbProvider.AddInParameter(command, helper.Vermmestado, DbType.Int32, entity.Vermmestado);
            dbProvider.AddInParameter(command, helper.Vermmfeccreacion, DbType.DateTime, entity.Vermmfeccreacion);
            dbProvider.AddInParameter(command, helper.Vermmusumodificacion, DbType.String, entity.Vermmusumodificacion);
            dbProvider.AddInParameter(command, helper.Vermmfecmodificacion, DbType.DateTime, entity.Vermmfecmodificacion);
            dbProvider.AddInParameter(command, helper.Vermmmotivoportal, DbType.Int32, entity.Vermmmotivoportal);
            dbProvider.AddInParameter(command, helper.Vermmfechageneracion, DbType.DateTime, entity.Vermmfechageneracion);
            dbProvider.AddInParameter(command, helper.Vermmfechaaprobacion, DbType.DateTime, entity.Vermmfechaaprobacion);
            dbProvider.AddInParameter(command, helper.Vermmmotivo, DbType.String, entity.Vermmmotivo);
            dbProvider.AddInParameter(command, helper.Vermmporcentaje, DbType.String, entity.Vermmporcentaje);
            dbProvider.AddInParameter(command, helper.Vermmmsjgeneracion, DbType.String, entity.Vermmmsjgeneracion);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MmmVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.Vermmfechaperiodo, DbType.DateTime, entity.Vermmfechaperiodo);
            dbProvider.AddInParameter(command, helper.Vermmnumero, DbType.Int32, entity.Vermmnumero);
            dbProvider.AddInParameter(command, helper.Vermmusucreacion, DbType.String, entity.Vermmusucreacion);
            dbProvider.AddInParameter(command, helper.Vermmestado, DbType.Int32, entity.Vermmestado);
            dbProvider.AddInParameter(command, helper.Vermmfeccreacion, DbType.DateTime, entity.Vermmfeccreacion);
            dbProvider.AddInParameter(command, helper.Vermmusumodificacion, DbType.String, entity.Vermmusumodificacion);
            dbProvider.AddInParameter(command, helper.Vermmfecmodificacion, DbType.DateTime, entity.Vermmfecmodificacion);
            dbProvider.AddInParameter(command, helper.Vermmmotivoportal, DbType.Int32, entity.Vermmmotivoportal);
            dbProvider.AddInParameter(command, helper.Vermmfechageneracion, DbType.DateTime, entity.Vermmfechageneracion);
            dbProvider.AddInParameter(command, helper.Vermmfechaaprobacion, DbType.DateTime, entity.Vermmfechaaprobacion);
            dbProvider.AddInParameter(command, helper.Vermmmotivo, DbType.String, entity.Vermmmotivo);
            dbProvider.AddInParameter(command, helper.Vermmcodi, DbType.Int32, entity.Vermmcodi);
            dbProvider.AddInParameter(command, helper.Vermmmsjgeneracion, DbType.String, entity.Vermmmsjgeneracion);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdatePorcentaje(MmmVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdatePorcentaje);
            dbProvider.AddInParameter(command, helper.Vermmporcentaje, DbType.Decimal, entity.Vermmporcentaje);
            dbProvider.AddInParameter(command, helper.Vermmfechageneracion, DbType.DateTime, entity.Vermmfechageneracion);
            dbProvider.AddInParameter(command, helper.Vermmmsjgeneracion, DbType.String, entity.Vermmmsjgeneracion);
            dbProvider.AddInParameter(command, helper.Vermmcodi, DbType.Int32, entity.Vermmcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateEstadoVersion(MmmVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateVersionEstado);
            dbProvider.AddInParameter(command, helper.Vermmestado, DbType.Int32, entity.Vermmestado);
            dbProvider.AddInParameter(command, helper.Vermmfechaaprobacion, DbType.DateTime, entity.Vermmfechageneracion);
            dbProvider.AddInParameter(command, helper.Vermmcodi, DbType.Int32, entity.Vermmcodi);
            dbProvider.ExecuteNonQuery(command);
        }


        public void Delete(int vermmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vermmcodi, DbType.Int32, vermmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MmmVersionDTO GetById(int vermmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vermmcodi, DbType.Int32, vermmcodi);
            MmmVersionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MmmVersionDTO> List()
        {
            List<MmmVersionDTO> entitys = new List<MmmVersionDTO>();
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

        public List<MmmVersionDTO> GetByCriteria()
        {
            List<MmmVersionDTO> entitys = new List<MmmVersionDTO>();
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
