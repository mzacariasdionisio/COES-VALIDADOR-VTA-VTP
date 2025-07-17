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
    /// Clase de acceso a datos de la tabla SI_VERSION
    /// </summary>
    public class SiVersionRepository : RepositoryBase, ISiVersionRepository
    {
        public SiVersionRepository(string strConn)
            : base(strConn)
        {
        }

        SiVersionHelper helper = new SiVersionHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Verscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Versnombre, DbType.String, entity.Versnombre);
            dbProvider.AddInParameter(command, helper.Verscorrelativo, DbType.Int32, entity.Verscorrelativo);
            dbProvider.AddInParameter(command, helper.Versfechaperiodo, DbType.DateTime, entity.Versfechaperiodo);
            dbProvider.AddInParameter(command, helper.Versfechaversion, DbType.DateTime, entity.Versfechaversion);
            dbProvider.AddInParameter(command, helper.Versusucreacion, DbType.String, entity.Versusucreacion);
            dbProvider.AddInParameter(command, helper.Versfeccreacion, DbType.DateTime, entity.Versfeccreacion);
            dbProvider.AddInParameter(command, helper.Mprojcodi, DbType.Int32, entity.Mprojcodi);
            dbProvider.AddInParameter(command, helper.Tmrepcodi, DbType.Int32, entity.Tmrepcodi);
            dbProvider.AddInParameter(command, helper.Versmotivo, DbType.String, entity.Versmotivo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int SaveTransaccional(SiVersionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Verscodi, DbType.Int32, entity.Verscodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Versnombre, DbType.String, entity.Versnombre));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Verscorrelativo, DbType.Int32, entity.Verscorrelativo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Versfechaperiodo, DbType.DateTime, entity.Versfechaperiodo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Versfechaversion, DbType.DateTime, entity.Versfechaversion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Versusucreacion, DbType.String, entity.Versusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Versfeccreacion, DbType.DateTime, entity.Versfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mprojcodi, DbType.Int32, entity.Mprojcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Tmrepcodi, DbType.Int32, entity.Tmrepcodi));
            dbProvider.AddInParameter(command, helper.Versmotivo, DbType.String, entity.Versmotivo);

            command.ExecuteNonQuery();
            return entity.Verscodi;
        }

        public void Update(SiVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Verscodi, DbType.Int32, entity.Verscodi);
            dbProvider.AddInParameter(command, helper.Versnombre, DbType.String, entity.Versnombre);
            dbProvider.AddInParameter(command, helper.Verscorrelativo, DbType.Int32, entity.Verscorrelativo);
            dbProvider.AddInParameter(command, helper.Versfechaperiodo, DbType.DateTime, entity.Versfechaperiodo);
            dbProvider.AddInParameter(command, helper.Versfechaversion, DbType.DateTime, entity.Versfechaversion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public SiVersionDTO GetById(int verscodi)
        {
            string query = string.Format(helper.SqlGetById, verscodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            SiVersionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiVersionDTO> List()
        {
            List<SiVersionDTO> entitys = new List<SiVersionDTO>();
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

        public List<SiVersionDTO> GetByCriteria(DateTime fecha, int tmrepcodi)
        {
            List<SiVersionDTO> entitys = new List<SiVersionDTO>();

            string query = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha), tmrepcodi);
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

        public int MaximoXFecha(DateTime fecha, int tmrepcodi)
        {
            string query = string.Format(helper.SqlMaximoXFecha, fecha.ToString(ConstantesBase.FormatoFecha), tmrepcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            SiVersionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new SiVersionDTO();

                    int iVerscorrelativo = dr.GetOrdinal(this.helper.Verscorrelativo);
                    if (!dr.IsDBNull(iVerscorrelativo)) entity.Verscorrelativo = Convert.ToInt32(dr.GetValue(iVerscorrelativo));
                }
            }

            return (int)entity.Verscorrelativo;
        }

    }
}
