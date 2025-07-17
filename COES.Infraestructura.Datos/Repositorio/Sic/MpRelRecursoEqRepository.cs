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
    /// Clase de acceso a datos de la tabla MP_REL_RECURSO_EQ
    /// </summary>
    public class MpRelRecursoEqRepository: RepositoryBase, IMpRelRecursoEqRepository
    {
        public MpRelRecursoEqRepository(string strConn): base(strConn)
        {
        }

        MpRelRecursoEqHelper helper = new MpRelRecursoEqHelper();

        public void Save(MpRelRecursoEqDTO entity, IDbConnection connection, IDbTransaction transaction)
        {            
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi, DbType.Int32, entity.Mrecurcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mreqfactor, DbType.Decimal, entity.Mreqfactor));

                dbCommand.ExecuteNonQuery();
            }
        }

        public void Update(MpRelRecursoEqDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, entity.Mrecurcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Mreqfactor, DbType.Decimal, entity.Mreqfactor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mtopcodi, int mrecurcodi, int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, mrecurcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mtopcodi, int mrecurcodi, int equicodi, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlDelete;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, mtopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi, DbType.Int32, mrecurcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, equicodi));                

                dbCommand.ExecuteNonQuery();
            }
        }
        public MpRelRecursoEqDTO GetById(int mtopcodi, int mrecurcodi, int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, mrecurcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            MpRelRecursoEqDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MpRelRecursoEqDTO> List()
        {
            List<MpRelRecursoEqDTO> entitys = new List<MpRelRecursoEqDTO>();
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

        public List<MpRelRecursoEqDTO> GetByCriteria()
        {
            List<MpRelRecursoEqDTO> entitys = new List<MpRelRecursoEqDTO>();
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

        public List<MpRelRecursoEqDTO> ListarPorTopologia(int mtopcodi)
        {
            List<MpRelRecursoEqDTO> entitys = new List<MpRelRecursoEqDTO>();
            string query = string.Format(helper.SqlListarByTopologia, mtopcodi);
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

        public List<MpRelRecursoEqDTO> ListarPorTopologiaYRecurso(int mtopcodi, int mrecurcodi)
        {
            List<MpRelRecursoEqDTO> entitys = new List<MpRelRecursoEqDTO>();
            string query = string.Format(helper.SqlListarByTopologiaYRecurso, mtopcodi, mrecurcodi);
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
        
    }
}
