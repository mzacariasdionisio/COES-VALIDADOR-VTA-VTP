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
    /// Clase de acceso a datos de la tabla FT_EXT_RELEMPETAPA
    /// </summary>
    public class FtExtRelempetapaRepository : RepositoryBase, IFtExtRelempetapaRepository
    {
        public FtExtRelempetapaRepository(string strConn) : base(strConn)
        {
        }

        FtExtRelempetapaHelper helper = new FtExtRelempetapaHelper();

        public int Save(FtExtRelempetapaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Fetempcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ftetcodi, DbType.Int32, entity.Ftetcodi);
            dbProvider.AddInParameter(command, helper.Fetempusucreacion, DbType.String, entity.Fetempusucreacion);
            dbProvider.AddInParameter(command, helper.Fetempfeccreacion, DbType.DateTime, entity.Fetempfeccreacion);
            dbProvider.AddInParameter(command, helper.Fetempusumodificacion, DbType.String, entity.Fetempusumodificacion);
            dbProvider.AddInParameter(command, helper.Fetempfecmodificacion, DbType.DateTime, entity.Fetempfecmodificacion);
            dbProvider.AddInParameter(command, helper.Fetempestado, DbType.String, entity.Fetempestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(FtExtRelempetapaDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftetcodi, DbType.Int32, entity.Ftetcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempusucreacion, DbType.String, entity.Fetempusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempfeccreacion, DbType.DateTime, entity.Fetempfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempusumodificacion, DbType.String, entity.Fetempusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempfecmodificacion, DbType.DateTime, entity.Fetempfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempestado, DbType.String, entity.Fetempestado));


                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(FtExtRelempetapaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ftetcodi, DbType.Int32, entity.Ftetcodi);
            dbProvider.AddInParameter(command, helper.Fetempusucreacion, DbType.String, entity.Fetempusucreacion);
            dbProvider.AddInParameter(command, helper.Fetempfeccreacion, DbType.DateTime, entity.Fetempfeccreacion);
            dbProvider.AddInParameter(command, helper.Fetempusumodificacion, DbType.String, entity.Fetempusumodificacion);
            dbProvider.AddInParameter(command, helper.Fetempfecmodificacion, DbType.DateTime, entity.Fetempfecmodificacion);
            dbProvider.AddInParameter(command, helper.Fetempestado, DbType.String, entity.Fetempestado);
            dbProvider.AddInParameter(command, helper.Fetempcodi, DbType.Int32, entity.Fetempcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(FtExtRelempetapaDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftetcodi, DbType.Int32, entity.Ftetcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempusucreacion, DbType.String, entity.Fetempusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempfeccreacion, DbType.DateTime, entity.Fetempfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempusumodificacion, DbType.String, entity.Fetempusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempfecmodificacion, DbType.DateTime, entity.Fetempfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempestado, DbType.String, entity.Fetempestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempcodi, DbType.Int32, entity.Fetempcodi));


                dbCommand.ExecuteNonQuery();

            }
        }

        public void Delete(int fetempcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Fetempcodi, DbType.Int32, fetempcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtRelempetapaDTO GetById(int fetempcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fetempcodi, DbType.Int32, fetempcodi);
            FtExtRelempetapaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtRelempetapaDTO> List()
        {
            List<FtExtRelempetapaDTO> entitys = new List<FtExtRelempetapaDTO>();
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

        public List<FtExtRelempetapaDTO> GetByCriteria()
        {
            List<FtExtRelempetapaDTO> entitys = new List<FtExtRelempetapaDTO>();
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


        public List<FtExtRelempetapaDTO> GetByCriteriaProyAsigByFiltros(string sEmpresa, int idetapa)
        {
            List<FtExtRelempetapaDTO> entitys = new List<FtExtRelempetapaDTO>();
            string sqlQuery = string.Format(helper.SqlGetByCriteriaProyAsigByFiltros, sEmpresa, idetapa);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtRelempetapaDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFtetnombre = dr.GetOrdinal(helper.Ftetnombre);
                    if (!dr.IsDBNull(iFtetnombre)) entity.Ftetnombre = dr.GetString(iFtetnombre);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<FtExtRelempetapaDTO> GetByEtapasPorElemento(int? equicodi, int? grupocodi)
        {
            List<FtExtRelempetapaDTO> entitys = new List<FtExtRelempetapaDTO>();

            string query = "";
            if(equicodi != null)
            {
                query = string.Format(helper.SqlGetEtapasPorEquicodi, equicodi);
            }
            else
            {
                if (grupocodi != null)
                {
                    query = string.Format(helper.SqlGetEtapasPorGrupocodi, grupocodi);
                }
            }

            if (query != "")
            {
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        FtExtRelempetapaDTO entity = new FtExtRelempetapaDTO();

                        int iFtetnombre = dr.GetOrdinal(helper.Ftetnombre);
                        if (!dr.IsDBNull(iFtetnombre)) entity.Ftetnombre = dr.GetString(iFtetnombre);

                        int iFtetcodi = dr.GetOrdinal(helper.Ftetcodi);
                        if (!dr.IsDBNull(iFtetcodi)) entity.Ftetcodi = Convert.ToInt32(dr.GetValue(iFtetcodi));

                        int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                        if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                        int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                        if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                        entitys.Add(entity);

                    }
                }
            }

            return entitys;
        }

        public List<FtExtRelempetapaDTO> GetByProyectos(string ftprycodis)
        {
            List<FtExtRelempetapaDTO> entitys = new List<FtExtRelempetapaDTO>();
            string query = string.Format(helper.SqlGetByProyectos, ftprycodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtRelempetapaDTO entity = helper.Create(dr);

                    int iFtrpycodi = dr.GetOrdinal(helper.Ftrpycodi);
                    if (!dr.IsDBNull(iFtrpycodi)) entity.Ftprycodi = Convert.ToInt32(dr.GetValue(iFtrpycodi));

                    entitys.Add(entity);

                }
            }


            return entitys;
        }
        


    }
}
