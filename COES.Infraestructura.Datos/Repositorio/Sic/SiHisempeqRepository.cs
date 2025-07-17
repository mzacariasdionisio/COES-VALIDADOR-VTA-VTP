using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SI_HISEMPEQ
    /// </summary>
    public class SiHisempeqRepository : RepositoryBase, ISiHisempeqRepository
    {
        public SiHisempeqRepository(string strConn) : base(strConn)
        {
        }

        SiHisempeqHelper helper = new SiHisempeqHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiHisempeqDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hempeqcodi, DbType.Int32, entity.Hempeqcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hempeqfecha, DbType.DateTime, entity.Hempeqfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodiold, DbType.Int32, entity.Equicodiold));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hempeqestado, DbType.String, entity.Hempeqestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hempeqdeleted, DbType.Int32, entity.Hempeqdeleted));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Operadoremprcodi, DbType.Int32, entity.Operadoremprcodi));

            command.ExecuteNonQuery();
            return entity.Hempeqcodi;
        }

        public void Update(SiHisempeqDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Hempeqcodi, DbType.Int32, entity.Hempeqcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Hempeqfecha, DbType.DateTime, entity.Hempeqfecha);
            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi);
            dbProvider.AddInParameter(command, helper.Equicodiold, DbType.Int32, entity.Equicodiold);
            dbProvider.AddInParameter(command, helper.Hempeqestado, DbType.String, entity.Hempeqestado);
            dbProvider.AddInParameter(command, helper.Operadoremprcodi, DbType.Int32, entity.Operadoremprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteLogico(SiHisempeqDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteLogico);
            dbProvider.AddInParameter(command, helper.Hempeqdeleted, DbType.Int32, entity.Hempeqdeleted);
            dbProvider.AddInParameter(command, helper.Hempeqcodi, DbType.Int32, entity.Hempeqcodi);


            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hempeqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Hempeqcodi, DbType.Int32, hempeqcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiHisempeqDTO GetById(int hempeqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Hempeqcodi, DbType.Int32, hempeqcodi);
            SiHisempeqDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiHisempeqDTO> List()
        {
            List<SiHisempeqDTO> entitys = new List<SiHisempeqDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiHisempeqDTO> GetByCriteria()
        {
            List<SiHisempeqDTO> entitys = new List<SiHisempeqDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region anular migración

        //Métodos para eliminar por anulación de transferencia
        public void UpdateAnularTransf(int migracodi, IDbConnection conn, DbTransaction tran)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlupdateAnular);
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migracodi, DbType.Int32, migracodi));

            command.ExecuteNonQuery();

            //DbCommand command = (DbCommand)conn.CreateCommand();
            //command.CommandText = helper.SqlupdateAnular;
            //command.Transaction = tran;
            //command.Connection = (DbConnection)conn;
            //command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migracodi, DbType.Int32, migracodi));
            //command.ExecuteNonQuery();
        }

        public List<SiHisempeqDTO> ListEquiposXMigracion(int migracodi)
        {
            List<SiHisempeqDTO> entitys = new List<SiHisempeqDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEquiposXMigracion);

            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, migracodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public int ConsultarEquipMigracion(int migracodi, int equicodi, DateTime fechacorte)
        {
            List<SiHisempeqDTO> entitys = new List<SiHisempeqDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlConsultarEquipMigracion);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, migracodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            var equipo = entitys.Find(x => x.Hempeqfecha >= fechacorte);
            int validar = equipo != null ? 1 : 0;
            return validar;
        }

        #endregion

    }
}