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
    /// Clase de acceso a datos de la tabla MP_REL_RECURSO_SDDP
    /// </summary>
    public class MpRelRecursoSddpRepository: RepositoryBase, IMpRelRecursoSddpRepository
    {
        public MpRelRecursoSddpRepository(string strConn): base(strConn)
        {
        }

        MpRelRecursoSddpHelper helper = new MpRelRecursoSddpHelper();
        
        public void Save(MpRelRecursoSddpDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;                

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi, DbType.Int32, entity.Mrecurcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrsddpfactor, DbType.Decimal, entity.Mrsddpfactor));
                
                dbCommand.ExecuteNonQuery();                
            }
        }

        public void Update(MpRelRecursoSddpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, entity.Mrecurcodi);
            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi);
            dbProvider.AddInParameter(command, helper.Mrsddpfactor, DbType.Decimal, entity.Mrsddpfactor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MpRelRecursoSddpDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, entity.Mtopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi, DbType.Int32, entity.Mrecurcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrsddpfactor, DbType.Decimal, entity.Mrsddpfactor));

                dbCommand.ExecuteNonQuery();
            }
        }

        public void Delete(int mtopcodi, int mrecurcodi, int sddpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, mrecurcodi);
            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, sddpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mtopcodi, int mrecurcodi, int sddpcodi, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlDelete;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mtopcodi, DbType.Int32, mtopcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Mrecurcodi, DbType.Int32, mrecurcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Sddpcodi, DbType.Int32, sddpcodi));

                dbCommand.ExecuteNonQuery();
            }
        }

        public MpRelRecursoSddpDTO GetById(int mtopcodi, int mrecurcodi, int sddpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mtopcodi, DbType.Int32, mtopcodi);
            dbProvider.AddInParameter(command, helper.Mrecurcodi, DbType.Int32, mrecurcodi);
            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, sddpcodi);
            MpRelRecursoSddpDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MpRelRecursoSddpDTO> List()
        {
            List<MpRelRecursoSddpDTO> entitys = new List<MpRelRecursoSddpDTO>();
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

        public List<MpRelRecursoSddpDTO> GetByCriteria()
        {
            List<MpRelRecursoSddpDTO> entitys = new List<MpRelRecursoSddpDTO>();
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

        public List<MpRelRecursoSddpDTO> ListarPorTopologia(int mtopcodi)
        {           
            List<MpRelRecursoSddpDTO> entitys = new List<MpRelRecursoSddpDTO>();
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

        public List<MpRelRecursoSddpDTO> ListarPorTopologiaYRecurso(int mtopcodi, int mrecurcodi)
        {
            List<MpRelRecursoSddpDTO> entitys = new List<MpRelRecursoSddpDTO>();
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
