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
    /// Clase de acceso a datos de la tabla FT_EXT_ETEMPDETEQ
    /// </summary>
    public class FtExtEtempdeteqRepository : RepositoryBase, IFtExtEtempdeteqRepository
    {
        public FtExtEtempdeteqRepository(string strConn) : base(strConn)
        {
        }

        FtExtEtempdeteqHelper helper = new FtExtEtempdeteqHelper();

        public int Save(FtExtEtempdeteqDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Feeeqcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Fetempcodi, DbType.Int32, entity.Fetempcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Feeeqflagotraetapa, DbType.String, entity.Feeeqflagotraetapa);
            dbProvider.AddInParameter(command, helper.Feeeqflagsistema, DbType.String, entity.Feeeqflagsistema); 
            dbProvider.AddInParameter(command, helper.Feeequsucreacion, DbType.String, entity.Feeequsucreacion);
            dbProvider.AddInParameter(command, helper.Feeeqfeccreacion, DbType.DateTime, entity.Feeeqfeccreacion);
            dbProvider.AddInParameter(command, helper.Feeequsumodificacion, DbType.String, entity.Feeequsumodificacion);
            dbProvider.AddInParameter(command, helper.Feeeqfecmodificacion, DbType.DateTime, entity.Feeeqfecmodificacion);
            dbProvider.AddInParameter(command, helper.Feeeqestado, DbType.String, entity.Feeeqestado);
            dbProvider.AddInParameter(command, helper.Feeeqflagcentral, DbType.String, entity.Feeeqflagcentral);


            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(FtExtEtempdeteqDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeeqcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempcodi, DbType.Int32, entity.Fetempcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeeqflagotraetapa, DbType.String, entity.Feeeqflagotraetapa));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeeqflagsistema, DbType.String, entity.Feeeqflagsistema));                
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeequsucreacion, DbType.String, entity.Feeequsucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeeqfeccreacion, DbType.DateTime, entity.Feeeqfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeequsumodificacion, DbType.String, entity.Feeequsumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeeqfecmodificacion, DbType.DateTime, entity.Feeeqfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeeqestado, DbType.String, entity.Feeeqestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeeqflagcentral, DbType.String, entity.Feeeqflagcentral));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(FtExtEtempdeteqDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Fetempcodi, DbType.Int32, entity.Fetempcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Feeeqflagotraetapa, DbType.String, entity.Feeeqflagotraetapa);
            dbProvider.AddInParameter(command, helper.Feeeqflagsistema, DbType.String, entity.Feeeqflagsistema);
            dbProvider.AddInParameter(command, helper.Feeequsucreacion, DbType.String, entity.Feeequsucreacion);
            dbProvider.AddInParameter(command, helper.Feeeqfeccreacion, DbType.DateTime, entity.Feeeqfeccreacion);
            dbProvider.AddInParameter(command, helper.Feeequsumodificacion, DbType.String, entity.Feeequsumodificacion);
            dbProvider.AddInParameter(command, helper.Feeeqfecmodificacion, DbType.DateTime, entity.Feeeqfecmodificacion);
            dbProvider.AddInParameter(command, helper.Feeeqestado, DbType.String, entity.Feeeqestado);
            dbProvider.AddInParameter(command, helper.Feeeqflagcentral, DbType.String, entity.Feeeqflagcentral);
            dbProvider.AddInParameter(command, helper.Feeeqcodi, DbType.Int32, entity.Feeeqcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(FtExtEtempdeteqDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fetempcodi, DbType.Int32, entity.Fetempcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeeqflagotraetapa, DbType.String, entity.Feeeqflagotraetapa));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeeqflagsistema, DbType.String, entity.Feeeqflagsistema));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeequsucreacion, DbType.String, entity.Feeequsucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeeqfeccreacion, DbType.DateTime, entity.Feeeqfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeequsumodificacion, DbType.String, entity.Feeequsumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeeqfecmodificacion, DbType.DateTime, entity.Feeeqfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeeqestado, DbType.String, entity.Feeeqestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeeqflagcentral, DbType.String, entity.Feeeqflagcentral));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Feeeqcodi, DbType.Int32, entity.Feeeqcodi));

                dbCommand.ExecuteNonQuery();

            }
        }

        public void Delete(int feeeqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Feeeqcodi, DbType.Int32, feeeqcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEtempdeteqDTO GetById(int feeeqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Feeeqcodi, DbType.Int32, feeeqcodi);
            FtExtEtempdeteqDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEtempdeteqDTO> List()
        {
            List<FtExtEtempdeteqDTO> entitys = new List<FtExtEtempdeteqDTO>();
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

        public List<FtExtEtempdeteqDTO> GetByCriteria()
        {
            List<FtExtEtempdeteqDTO> entitys = new List<FtExtEtempdeteqDTO>();
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

        public FtExtEtempdeteqDTO GetByRelacionUnidadEmpresaEtapa(int? equicodi, int? grupocodi, int emprcodi, int ftetcodi)
        {
            FtExtEtempdeteqDTO entity = null;

            string query = "";

            if (equicodi != null)
                query = string.Format(helper.SqlGetByElementoEquipoEmpresaEtapa, equicodi.Value, emprcodi, ftetcodi);
            else
            {
                if (grupocodi != null)
                    query = string.Format(helper.SqlGetByElementoGrupoEmpresaEtapa, grupocodi.Value, emprcodi, ftetcodi);
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

        public List<FtExtEtempdeteqDTO> GetByEmpresaEtapa(int emprcodi, int ftetcodi)
        {
            List<FtExtEtempdeteqDTO> entitys = new List<FtExtEtempdeteqDTO>();

            string query = string.Format(helper.SqlGetByEmpresaEtapa, emprcodi, ftetcodi);
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

        public List<FtExtEtempdeteqDTO> ListarPorRelEmpresaEtapa(string estado, int fetempcodi)
        {
            List<FtExtEtempdeteqDTO> entitys = new List<FtExtEtempdeteqDTO>();

            string query = string.Format(helper.SqlListarPorRelEmpresaEtapa, estado, fetempcodi);
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
