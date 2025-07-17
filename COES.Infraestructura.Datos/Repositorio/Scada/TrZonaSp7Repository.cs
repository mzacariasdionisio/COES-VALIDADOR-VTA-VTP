using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;
using System.Data.Odbc;

namespace COES.Infraestructura.Datos.Repositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla TR_ZONA_SP7
    /// </summary>
    public class TrZonaSp7Repository: RepositoryBase, ITrZonaSp7Repository
    {
        public TrZonaSp7Repository(string strConn): base(strConn)
        {
        }

        TrZonaSp7Helper helper = new TrZonaSp7Helper();

        public int Save(TrZonaSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Zonacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Zonanomb, DbType.String, entity.Zonanomb);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Zonaabrev, DbType.String, entity.Zonaabrev);
            dbProvider.AddInParameter(command, helper.Zonasiid, DbType.Int32, entity.Zonasiid);
            dbProvider.AddInParameter(command, helper.Zonausucreacion, DbType.String, entity.Zonausucreacion);
            dbProvider.AddInParameter(command, helper.Zonafeccreacion, DbType.DateTime, entity.Zonafeccreacion);
            dbProvider.AddInParameter(command, helper.Zonausumodificacion, DbType.String, entity.Zonausumodificacion);
            dbProvider.AddInParameter(command, helper.Zonafecmodificacion, DbType.DateTime, entity.Zonafecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrZonaSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Zonanomb, DbType.String, entity.Zonanomb);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Zonaabrev, DbType.String, entity.Zonaabrev);
            dbProvider.AddInParameter(command, helper.Zonasiid, DbType.Int32, entity.Zonasiid);
            dbProvider.AddInParameter(command, helper.Zonausucreacion, DbType.String, entity.Zonausucreacion);
            dbProvider.AddInParameter(command, helper.Zonafeccreacion, DbType.DateTime, entity.Zonafeccreacion);
            dbProvider.AddInParameter(command, helper.Zonausumodificacion, DbType.String, entity.Zonausumodificacion);
            dbProvider.AddInParameter(command, helper.Zonafecmodificacion, DbType.DateTime, entity.Zonafecmodificacion);
            dbProvider.AddInParameter(command, helper.Zonacodi, DbType.Int32, entity.Zonacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int zonacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Zonacodi, DbType.Int32, zonacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrZonaSp7DTO GetById(int zonacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Zonacodi, DbType.Int32, zonacodi);
            TrZonaSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrZonaSp7DTO> List()
        {
            List<TrZonaSp7DTO> entitys = new List<TrZonaSp7DTO>();
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

        public List<TrZonaSp7DTO> GetByCriteria(string emprcodi)
        {
            List<TrZonaSp7DTO> entitys = new List<TrZonaSp7DTO>();
            string query = string.Format(helper.SqlGetByCriteria, emprcodi);
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
        /// <summary>
        /// Graba los datos de la tabla TR_ZONA_SP7
        /// </summary>
        public int SaveTrZonaSp7Id(TrZonaSp7DTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Zonacodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Zonacodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<TrZonaSp7DTO> ListByEmpresa(int emprcodi)
        {
            List<TrZonaSp7DTO> entitys = new List<TrZonaSp7DTO>();

            string sql = string.Format(helper.SqlListByEmpresa, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrZonaSp7DTO entity = new TrZonaSp7DTO();

                    int iZonacodi = dr.GetOrdinal(this.helper.Zonacodi);
                    if (!dr.IsDBNull(iZonacodi)) entity.Zonacodi = Convert.ToInt32(dr.GetValue(iZonacodi));

                    int iZonanomb = dr.GetOrdinal(this.helper.Zonanomb);
                    if (!dr.IsDBNull(iZonanomb)) entity.Zonanomb = dr.GetString(iZonanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
