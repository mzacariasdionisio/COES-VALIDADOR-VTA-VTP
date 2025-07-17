using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SI_EQUIPOMIGRAR
    /// </summary>
    public class SiEquipomigrarRepository: RepositoryBase, ISiEquipoMigrarRepository
    {
        private string strConexion;
        public SiEquipomigrarRepository(string strConn): base(strConn)
        {
            strConexion = strConn;
        }

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction)conn.BeginTransaction();
        }

        SiEquipomigrarHelper helper = new SiEquipomigrarHelper();


        public int GetMaxId() {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result=dbProvider.ExecuteScalar(command);
            if(result!=null) id=Convert.ToInt32(result);

            return id;
        }

//TEST con transaccion
        //public int Save(SiEquipomigrarDTO entity, IDbConnection conn, DbTransaction tran)
        //{
        //    DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
        //    object result = dbProvider.ExecuteScalar(command);
        //    int id = 1;
        //    if (result != null)id = Convert.ToInt32(result);

        //    command = (DbCommand)conn.CreateCommand();
        //    command.CommandText = helper.SqlSave;
        //    command.Transaction = tran;
        //    command.Connection = (DbConnection)conn;

        //    command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equmigcodi, DbType.Int32, id));
        //    command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migempcodi, DbType.Int32, entity.Migempcodi));
        //    command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodimigra, DbType.Int32, entity.Equicodimigra));
        //    command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodibajanuevo, DbType.Int32, entity.Equicodibajanuevo));
        //    command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equmigusucreacion, DbType.String, entity.Equmigusucreacion));
        //    command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equmigfeccreacion, DbType.DateTime, entity.Equmigfeccreacion));
        //    command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equmigusumodificacion, DbType.String, entity.Equmigusumodificacion));
        //    command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equmigfecmodificacion, DbType.DateTime, entity.Equmigfecmodificacion));
        //    command.ExecuteNonQuery();
        //    return id;
        //}

        public int Save(SiEquipomigrarDTO entity, IDbConnection conn, DbTransaction tran)
        {
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            //int id = 1;

            //if (corrEquipoMigrar != 0)
            //{
            //    id = corrEquipoMigrar + 1;
            //}
            //else
            //{
            //    object result = dbProvider.ExecuteScalar(command);
            //    if (result != null) id = Convert.ToInt32(result);            
            //}

            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equmigcodi, DbType.Int32, entity.Equmigcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migempcodi, DbType.Int32, entity.Migempcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodimigra, DbType.Int32, entity.Equicodimigra));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodibajanuevo, DbType.Int32, entity.Equicodibajanuevo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equmigusucreacion, DbType.String, entity.Equmigusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equmigfeccreacion, DbType.DateTime, entity.Equmigfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equmigusumodificacion, DbType.String, entity.Equmigusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equmigfecmodificacion, DbType.DateTime, entity.Equmigfecmodificacion));

            command.ExecuteNonQuery();
            return 1;
        }
//////////FIN TEST con transaccion

        public void Update(SiEquipomigrarDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Equmigcodi, DbType.Int32, entity.Equmigcodi);
            dbProvider.AddInParameter(command, helper.Migempcodi, DbType.Int32, entity.Migempcodi);
            dbProvider.AddInParameter(command, helper.Equicodimigra, DbType.Int32, entity.Equicodimigra);
            dbProvider.AddInParameter(command, helper.Equicodibajanuevo, DbType.Int32, entity.Equicodibajanuevo);
            dbProvider.AddInParameter(command, helper.Equmigusucreacion, DbType.String, entity.Equmigusucreacion);
            dbProvider.AddInParameter(command, helper.Equmigfeccreacion, DbType.DateTime, entity.Equmigfeccreacion);
            dbProvider.AddInParameter(command, helper.Equmigusumodificacion, DbType.String, entity.Equmigusumodificacion);
            dbProvider.AddInParameter(command, helper.Equmigfecmodificacion, DbType.DateTime, entity.Equmigfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int equmigcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Equmigcodi, DbType.Int32, equmigcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiEquipomigrarDTO GetById(int equmigcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Equmigcodi, DbType.Int32, equmigcodi);
            SiEquipomigrarDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiEquipomigrarDTO> List()
        {
            List<SiEquipomigrarDTO> entitys = new List<SiEquipomigrarDTO>();
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

        public List<SiEquipomigrarDTO> GetByCriteria()
        {
            List<SiEquipomigrarDTO> entitys = new List<SiEquipomigrarDTO>();
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
    }
}
