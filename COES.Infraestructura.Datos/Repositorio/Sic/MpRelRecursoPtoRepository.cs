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
    /// Clase de acceso a datos de la tabla MP_REL_RECURSO_PTO
    /// </summary>
    public class MpRelRecursoPtoRepository: RepositoryBase, IMpRelRecursoPtoRepository
    {
        public MpRelRecursoPtoRepository(string strConn): base(strConn)
        {
        }

        MpRelRecursoPtoHelper helper = new MpRelRecursoPtoHelper();

        public void Save(MpRelRecursoPtoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, entity.Mrecurcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Mrptohorizonte, DbType.String, entity.Mrptohorizonte);
            dbProvider.AddInParameter(command, helper.Tptomedicodi, DbType.Int32, entity.Tptomedicodi);
            dbProvider.AddInParameter(command, helper.Mrptofactor, DbType.Decimal, entity.Mrptofactor);
            dbProvider.AddInParameter(command, helper.Mrptoformato, DbType.Int32, entity.Mrptoformato);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Mrptovolumen, DbType.Decimal, entity.Mrptovolumen);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Save(MpRelRecursoPtoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi, DbType.Int32, entity.Mrecurcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Lectcodi, DbType.Int32, entity.Lectcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrptohorizonte, DbType.String, entity.Mrptohorizonte));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Tptomedicodi, DbType.Int32, entity.Tptomedicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrptofactor, DbType.Decimal, entity.Mrptofactor));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrptoformato, DbType.Int32, entity.Mrptoformato));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrptovolumen, DbType.Decimal, entity.Mrptovolumen));

                dbCommand.ExecuteNonQuery();
            }
        }

        public void Update(MpRelRecursoPtoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tptomedicodi, DbType.Int32, entity.Tptomedicodi);
            dbProvider.AddInParameter(command, helper.Mrptofactor, DbType.Decimal, entity.Mrptofactor);
            dbProvider.AddInParameter(command, helper.Mrptoformato, DbType.Int32, entity.Mrptoformato);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Mrptovolumen, DbType.Decimal, entity.Mrptovolumen);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, entity.Mrecurcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Mrptohorizonte, DbType.String, entity.Mrptohorizonte);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mtopcodi, int mrecurcodi, int ptomedicodi, int lectcodi, string mrptohorizonte)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, mrecurcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);
            dbProvider.AddInParameter(command, helper.Mrptohorizonte, DbType.String, mrptohorizonte);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mtopcodi, int mrecurcodi, int ptomedicodi, int lectcodi, string mrptohorizonte, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlDelete;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, mtopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi, DbType.Int32, mrecurcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ptomedicodi, DbType.Int32, ptomedicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Lectcodi, DbType.Int32, lectcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrptohorizonte, DbType.String, mrptohorizonte));
                

                dbCommand.ExecuteNonQuery();
            }
        }

        public MpRelRecursoPtoDTO GetById(int mtopcodi, int mrecurcodi, int ptomedicodi, int lectcodi, string mrptohorizonte)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, mrecurcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);
            dbProvider.AddInParameter(command, helper.Mrptohorizonte, DbType.String, mrptohorizonte);
            MpRelRecursoPtoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MpRelRecursoPtoDTO> List()
        {
            List<MpRelRecursoPtoDTO> entitys = new List<MpRelRecursoPtoDTO>();
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

        public List<MpRelRecursoPtoDTO> GetByCriteria()
        {
            List<MpRelRecursoPtoDTO> entitys = new List<MpRelRecursoPtoDTO>();
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

        public List<MpRelRecursoPtoDTO> ListarPorTopologia(int mtopcodi)
        {
            List<MpRelRecursoPtoDTO> entitys = new List<MpRelRecursoPtoDTO>();
            string query = string.Format(helper.SqlListarByTopologia, mtopcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MpRelRecursoPtoDTO> ListarPorTopologiaYRecurso(int mtopcodi, int mrecurcodi)
        {
            List<MpRelRecursoPtoDTO> entitys = new List<MpRelRecursoPtoDTO>();
            string query = string.Format(helper.SqlListarByTopologiaYRecurso, mtopcodi, mrecurcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iEquiNomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
    }
}
