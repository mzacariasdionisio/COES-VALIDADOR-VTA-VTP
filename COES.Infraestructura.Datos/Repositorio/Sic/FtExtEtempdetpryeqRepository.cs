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
    /// Clase de acceso a datos de la tabla FT_EXT_ETEMPDETPRYEQ
    /// </summary>
    public class FtExtEtempdetpryeqRepository : RepositoryBase, IFtExtEtempdetpryeqRepository
    {
        public FtExtEtempdetpryeqRepository(string strConn) : base(strConn)
        {
        }

        FtExtEtempdetpryeqHelper helper = new FtExtEtempdetpryeqHelper();

        public int Save(FtExtEtempdetpryeqDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Feepeqcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Feeprycodi, DbType.Int32, entity.Feeprycodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Feepeqflagotraetapa, DbType.String, entity.Feepeqflagotraetapa);
            dbProvider.AddInParameter(command, helper.Feepeqflagsistema, DbType.String, entity.Feepeqflagsistema);
            dbProvider.AddInParameter(command, helper.Feepequsucreacion, DbType.String, entity.Feepequsucreacion);
            dbProvider.AddInParameter(command, helper.Feepeqfeccreacion, DbType.DateTime, entity.Feepeqfeccreacion);
            dbProvider.AddInParameter(command, helper.Feepequsumodificacion, DbType.String, entity.Feepequsumodificacion);
            dbProvider.AddInParameter(command, helper.Feepeqfecmodificacion, DbType.DateTime, entity.Feepeqfecmodificacion);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Feepeqestado, DbType.String, entity.Feepeqestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(FtExtEtempdetpryeqDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepeqcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeprycodi, DbType.Int32, entity.Feeprycodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepeqflagotraetapa, DbType.String, entity.Feepeqflagotraetapa));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepeqflagsistema, DbType.String, entity.Feepeqflagsistema));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepequsucreacion, DbType.String, entity.Feepequsucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepeqfeccreacion, DbType.DateTime, entity.Feepeqfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepequsumodificacion, DbType.String, entity.Feepequsumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepeqfecmodificacion, DbType.DateTime, entity.Feepeqfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepeqestado, DbType.String, entity.Feepeqestado));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(FtExtEtempdetpryeqDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Feeprycodi, DbType.Int32, entity.Feeprycodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Feepeqflagotraetapa, DbType.String, entity.Feepeqflagotraetapa);
            dbProvider.AddInParameter(command, helper.Feepeqflagsistema, DbType.String, entity.Feepeqflagsistema);
            dbProvider.AddInParameter(command, helper.Feepequsucreacion, DbType.String, entity.Feepequsucreacion);
            dbProvider.AddInParameter(command, helper.Feepeqfeccreacion, DbType.DateTime, entity.Feepeqfeccreacion);
            dbProvider.AddInParameter(command, helper.Feepequsumodificacion, DbType.String, entity.Feepequsumodificacion);
            dbProvider.AddInParameter(command, helper.Feepeqfecmodificacion, DbType.DateTime, entity.Feepeqfecmodificacion);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Feepeqestado, DbType.String, entity.Feepeqestado);
            dbProvider.AddInParameter(command, helper.Feepeqcodi, DbType.Int32, entity.Feepeqcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(FtExtEtempdetpryeqDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeprycodi, DbType.Int32, entity.Feeprycodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepeqflagotraetapa, DbType.String, entity.Feepeqflagotraetapa));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepeqflagsistema, DbType.String, entity.Feepeqflagsistema));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepequsucreacion, DbType.String, entity.Feepequsucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepeqfeccreacion, DbType.DateTime, entity.Feepeqfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepequsumodificacion, DbType.String, entity.Feepequsumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepeqfecmodificacion, DbType.DateTime, entity.Feepeqfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepeqestado, DbType.String, entity.Feepeqestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feepeqcodi, DbType.Int32, entity.Feepeqcodi));


                dbCommand.ExecuteNonQuery();

            }
        }

        public void Delete(int feepeqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Feepeqcodi, DbType.Int32, feepeqcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEtempdetpryeqDTO GetById(int feepeqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Feepeqcodi, DbType.Int32, feepeqcodi);
            FtExtEtempdetpryeqDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEtempdetpryeqDTO> List()
        {
            List<FtExtEtempdetpryeqDTO> entitys = new List<FtExtEtempdetpryeqDTO>();
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

        public List<FtExtEtempdetpryeqDTO> GetByCriteria()
        {
            List<FtExtEtempdetpryeqDTO> entitys = new List<FtExtEtempdetpryeqDTO>();
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

        public List<FtExtEtempdetpryeqDTO> ListarPorRelProyectoEtapaEmpresa(int feeprycodi, string feepeqestado)
        {
            List<FtExtEtempdetpryeqDTO> entitys = new List<FtExtEtempdetpryeqDTO>();

            string query = string.Format(helper.SqlListarPorRelProyectoEtapaEmpresa, feeprycodi, feepeqestado);
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

        public FtExtEtempdetpryeqDTO GetByProyectoYUnidad(int feeprycodi, int? equicodi, int? grupocodi, string feepeqestado)
        {
            FtExtEtempdetpryeqDTO entity = null;
            string query = "";

            if (equicodi != null)
                query = string.Format(helper.SqlGetByProyectoYEquipo, feeprycodi, equicodi.Value, feepeqestado);
            else
            {
                if (grupocodi != null)
                    query = string.Format(helper.SqlGetByProyectoYGrupo, feeprycodi, grupocodi.Value, feepeqestado);
            }

            if (query != "")
            {
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        entity = helper.Create(dr);
                    }
                }
            }

            return entity;
        }

        public FtExtEtempdetpryeqDTO GetByProyectoUnidadEmpresaEtapa(int? equicodi, int? grupocodi, int ftprycodi, int emprcodi, int ftetcodi, string feepeqestado)
        {
            FtExtEtempdetpryeqDTO entity = null;

            string query = "";

            if (equicodi != null)
                query = string.Format(helper.SqlGetByProyectoEquipoEmpresaEtapa, equicodi.Value, ftprycodi, emprcodi, ftetcodi, feepeqestado);
            else
            {
                if (grupocodi != null)
                    query = string.Format(helper.SqlGetByProyectoGrupoEmpresaEtapa, grupocodi.Value, ftprycodi, emprcodi, ftetcodi, feepeqestado);
            }

            if (query != "")
            {
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        entity = helper.Create(dr);
                    }
                }
            }

            return entity;
        }

        public List<FtExtEtempdetpryeqDTO> ListarDetallesPorRelEmpresaEtapaProyecto(string feepryestado, string feepeqestado, int feeprycodi)
        {
            List<FtExtEtempdetpryeqDTO> entitys = new List<FtExtEtempdetpryeqDTO>();

            string query = string.Format(helper.SqlListarDetallesPorRelEmpresaEtapaProyecto, feepryestado, feepeqestado, feeprycodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iCatenomb = dr.GetOrdinal(helper.Catenomb);
                    if (!dr.IsDBNull(iCatenomb)) entity.Catenomb = dr.GetString(iCatenomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        
    }
}
