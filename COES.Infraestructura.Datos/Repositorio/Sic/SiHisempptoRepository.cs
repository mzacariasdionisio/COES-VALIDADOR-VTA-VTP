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
    /// Clase de acceso a datos de la tabla SI_HISEMPPTO
    /// </summary>
    public class SiHisempptoRepository : RepositoryBase, ISiHisempptoRepository
    {
        public SiHisempptoRepository(string strConn) : base(strConn)
        {
        }

        SiHisempptoHelper helper = new SiHisempptoHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiHisempptoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hempptcodi, DbType.Int32, entity.Hempptcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hempptfecha, DbType.DateTime, entity.Hempptfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ptomedicodiold, DbType.Int32, entity.Ptomedicodiold));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hempptestado, DbType.String, entity.Hempptestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hempptdeleted, DbType.Int32, entity.Hempptdeleted));

            command.ExecuteNonQuery();
            return entity.Hempptcodi;
        }

        public void Update(SiHisempptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Hempptcodi, DbType.Int32, entity.Hempptcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Hempptfecha, DbType.DateTime, entity.Hempptfecha);
            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodiold, DbType.Int32, entity.Ptomedicodiold);
            dbProvider.AddInParameter(command, helper.Hempptestado, DbType.String, entity.Hempptestado);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteLogico(SiHisempptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteLogico);
            dbProvider.AddInParameter(command, helper.Hempptdeleted, DbType.Int32, entity.Hempptdeleted);
            dbProvider.AddInParameter(command, helper.Hempptcodi, DbType.Int32, entity.Hempptcodi);


            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hempptcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Hempptcodi, DbType.Int32, hempptcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiHisempptoDTO GetById(int hempptcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Hempptcodi, DbType.Int32, hempptcodi);
            SiHisempptoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiHisempptoDTO> List()
        {
            List<SiHisempptoDTO> entitys = new List<SiHisempptoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iPtomedidesc = dr.GetOrdinal(this.helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiHisempptoDTO> GetByCriteria()
        {
            List<SiHisempptoDTO> entitys = new List<SiHisempptoDTO>();
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

        public List<SiHisempptoDTO> ListPtsXMigracion(int migracodi)
        {
            List<SiHisempptoDTO> entitys = new List<SiHisempptoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPtsXMigracion);

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

        public int ConsultarPtosMigracion(int migracodi, int ptomedicodi, DateTime fechacorte)
        {
            List<SiHisempptoDTO> entitys = new List<SiHisempptoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlConsultarPtosMigracion);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, migracodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            var equipo = entitys.Find(x => x.Hempptfecha >= fechacorte);
            int validar = equipo != null ? 1 : 0;
            return validar;
        }
    }
}