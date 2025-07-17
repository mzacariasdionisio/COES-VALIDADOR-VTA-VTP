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
    /// Clase de acceso a datos de la tabla SPO_VERSIONREP
    /// </summary>
    public class SpoVersionrepRepository : RepositoryBase, ISpoVersionrepRepository
    {
        public SpoVersionrepRepository(string strConn) : base(strConn)
        {
        }

        SpoVersionrepHelper helper = new SpoVersionrepHelper();

        public int Save(SpoVersionrepDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Verrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, entity.Repcodi);
            dbProvider.AddInParameter(command, helper.Verrfechaperiodo, DbType.DateTime, entity.Verrfechaperiodo);
            dbProvider.AddInParameter(command, helper.Verrusucreacion, DbType.String, entity.Verrusucreacion);
            dbProvider.AddInParameter(command, helper.Verrestado, DbType.Int16, entity.Verrestado);
            dbProvider.AddInParameter(command, helper.Verrnro, DbType.Int32, entity.Verrnro);
            dbProvider.AddInParameter(command, helper.Verrfeccreacion, DbType.DateTime, entity.Verrfeccreacion);
            dbProvider.AddInParameter(command, helper.Verrusumodificacion, DbType.String, entity.Verrusumodificacion);
            dbProvider.AddInParameter(command, helper.Verrfecmodificacion, DbType.DateTime, entity.Verrfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int GetMaxIdVersion(DateTime periodo)
        {
            string sqlQuery = string.Format(helper.SqlGetMaxIdVersion, periodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            var result = dbProvider.ExecuteScalar(command);
            int idVersion = 1;
            if (result != null) idVersion = Convert.ToInt32(result);
            return idVersion;
        }

        public void Update(SpoVersionrepDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Verrcodi, DbType.Int32, entity.Verrcodi);
            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, entity.Repcodi);
            dbProvider.AddInParameter(command, helper.Verrfechaperiodo, DbType.String, entity.Verrfechaperiodo);
            dbProvider.AddInParameter(command, helper.Verrusucreacion, DbType.String, entity.Verrusucreacion);
            dbProvider.AddInParameter(command, helper.Verrestado, DbType.String, entity.Verrestado);
            dbProvider.AddInParameter(command, helper.Verrnro, DbType.Int32, entity.Verrnro);
            dbProvider.AddInParameter(command, helper.Verrfeccreacion, DbType.String, entity.Verrfeccreacion);
            dbProvider.AddInParameter(command, helper.Verrusumodificacion, DbType.String, entity.Verrusumodificacion);
            dbProvider.AddInParameter(command, helper.Verrfecmodificacion, DbType.String, entity.Verrfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int verrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Verrcodi, DbType.Int32, verrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SpoVersionrepDTO GetById(int verrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Verrcodi, DbType.Int32, verrcodi);
            SpoVersionrepDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SpoVersionrepDTO> List()
        {
            List<SpoVersionrepDTO> entitys = new List<SpoVersionrepDTO>();
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

        public List<SpoVersionrepDTO> GetByCriteria(DateTime fecha)
        {
            string sqlQuery = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha));
            List<SpoVersionrepDTO> entitys = new List<SpoVersionrepDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void UpdateEstado(SpoVersionrepDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateEstado);

            dbProvider.AddInParameter(command, helper.Verrestado, DbType.Int16, entity.Verrestado);
            dbProvider.AddInParameter(command, helper.Verrusumodificacion, DbType.String, entity.Verrusumodificacion);
            dbProvider.AddInParameter(command, helper.Verrfecmodificacion, DbType.DateTime, entity.Verrfecmodificacion);
            dbProvider.AddInParameter(command, helper.Verrcodi, DbType.Int32, entity.Verrcodi);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}
