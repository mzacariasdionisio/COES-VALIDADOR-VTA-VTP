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
    /// Clase de acceso a datos de la tabla PMO_MES
    /// </summary>
    public class PmoMesRepository: RepositoryBase, IPmoMesRepository
    {
        public PmoMesRepository(string strConn): base(strConn)
        {
        }

        PmoMesHelper helper = new PmoMesHelper();

        public int Save(PmoMesDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pmmescodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pmmesaniomes, DbType.Int32, entity.Pmmesaniomes);
            dbProvider.AddInParameter(command, helper.Pmmesestado, DbType.Int32, entity.Pmmesestado);
            dbProvider.AddInParameter(command, helper.Pmmesprocesado, DbType.Int32, entity.Pmmesprocesado);
            dbProvider.AddInParameter(command, helper.Pmmesfecini, DbType.DateTime, entity.Pmmesfecini);
            dbProvider.AddInParameter(command, helper.Pmmesfecinimes, DbType.DateTime, entity.Pmmesfecinimes);
            dbProvider.AddInParameter(command, helper.Pmanopcodi, DbType.Int32, entity.Pmanopcodi);
            dbProvider.AddInParameter(command, helper.Pmmesusucreacion, DbType.String, entity.Pmmesusucreacion);
            dbProvider.AddInParameter(command, helper.Pmmesfeccreacion, DbType.DateTime, entity.Pmmesfeccreacion);
            dbProvider.AddInParameter(command, helper.Pmusumodificacion, DbType.String, entity.Pmusumodificacion);
            dbProvider.AddInParameter(command, helper.Pmfecmodificacion, DbType.DateTime, entity.Pmfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PmoMesDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pmmescodi, DbType.Int32, entity.Pmmescodi);
            dbProvider.AddInParameter(command, helper.Pmmesaniomes, DbType.Int32, entity.Pmmesaniomes);
            dbProvider.AddInParameter(command, helper.Pmmesestado, DbType.Int32, entity.Pmmesestado);
            dbProvider.AddInParameter(command, helper.Pmmesprocesado, DbType.Int32, entity.Pmmesprocesado);
            dbProvider.AddInParameter(command, helper.Pmmesfecini, DbType.DateTime, entity.Pmmesfecini);
            dbProvider.AddInParameter(command, helper.Pmmesfecinimes, DbType.DateTime, entity.Pmmesfecinimes);
            dbProvider.AddInParameter(command, helper.Pmanopcodi, DbType.Int32, entity.Pmanopcodi);
            dbProvider.AddInParameter(command, helper.Pmmesusucreacion, DbType.String, entity.Pmmesusucreacion);
            dbProvider.AddInParameter(command, helper.Pmmesfeccreacion, DbType.DateTime, entity.Pmmesfeccreacion);
            dbProvider.AddInParameter(command, helper.Pmusumodificacion, DbType.String, entity.Pmusumodificacion);
            dbProvider.AddInParameter(command, helper.Pmfecmodificacion, DbType.DateTime, entity.Pmfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pmmescodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pmmescodi, DbType.Int32, pmmescodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PmoMesDTO GetById(int pmmescodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pmmescodi, DbType.Int32, pmmescodi);
            PmoMesDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PmoMesDTO> List()
        {
            List<PmoMesDTO> entitys = new List<PmoMesDTO>();
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

        public List<PmoMesDTO> GetByCriteria(int pmanopcodi)
        {
            List<PmoMesDTO> entitys = new List<PmoMesDTO>();
            string query = string.Format(helper.SqlGetByCriteria, pmanopcodi);
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

        public List<PmoMesDTO> GetByCriteriaXAnio(string anios)
        {
            List<PmoMesDTO> entitys = new List<PmoMesDTO>();
            string query = string.Format(helper.SqlGetByCriteriaXAnio, anios);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iPmanopfecini = dr.GetOrdinal(helper.Pmanopfecini);
                    if (!dr.IsDBNull(iPmanopfecini)) entity.Pmanopfecini = dr.GetDateTime(iPmanopfecini);

                    int iPmanopanio = dr.GetOrdinal(helper.Pmanopanio);
                    if (!dr.IsDBNull(iPmanopanio)) entity.Pmanopanio = Convert.ToInt32(dr.GetValue(iPmanopanio));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public PmoMesDTO GetByCriteriaXMes(DateTime fchaIni)
        {
            PmoMesDTO entity = null;
            string query = string.Format(helper.SqlGetByCriteriaXMes, fchaIni.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public int Save(PmoMesDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmmescodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmmesaniomes, DbType.Int32, entity.Pmmesaniomes));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmmesestado, DbType.Int32, entity.Pmmesestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmmesprocesado, DbType.Int32, entity.Pmmesprocesado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmmesfecini, DbType.DateTime, entity.Pmmesfecini));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmmesfecinimes, DbType.DateTime, entity.Pmmesfecinimes));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmanopcodi, DbType.Int32, entity.Pmanopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmmesusucreacion, DbType.String, entity.Pmmesusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmmesfeccreacion, DbType.DateTime, entity.Pmmesfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmusumodificacion, DbType.String, entity.Pmusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmfecmodificacion, DbType.DateTime, entity.Pmfecmodificacion));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void UpdateBajaPeriodoSemanaMes(PmoMesDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                var query = string.Format(helper.SqlUpdateEstadoBaja, entity.Pmmescodi);
                dbCommand.CommandText = query;

                dbCommand.ExecuteNonQuery();
            }
        }

        public void UpdateAprobar(PmoMesDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlUpdateAprobar;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmmesestado, DbType.String, entity.Pmmesestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmusumodificacion, DbType.String, entity.Pmusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmfecmodificacion, DbType.DateTime, entity.Pmfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Pmmescodi, DbType.Int32, entity.Pmmescodi));

                dbCommand.ExecuteNonQuery();

            }
        }

        public void UpdateEstadoProcesado(int aniomesIni, int aniomesFin, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                var query = string.Format(helper.SqlUpdateEstadoProcesado, aniomesIni, aniomesFin);
                dbCommand.CommandText = query;

                dbCommand.ExecuteNonQuery();
            }
        }
    }
}
