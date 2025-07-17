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
    /// Clase de acceso a datos de la tabla SI_HISEMPGRUPO
    /// </summary>
    public class SiHisempgrupoRepository : RepositoryBase, ISiHisempgrupoRepository
    {
        public SiHisempgrupoRepository(string strConn) : base(strConn)
        {
        }

        SiHisempgrupoHelper helper = new SiHisempgrupoHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiHisempgrupoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodiold, DbType.Int32, entity.Grupocodiold));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hempgrcodi, DbType.Int32, entity.Hempgrcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hempgrfecha, DbType.DateTime, entity.Hempgrfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hempgrestado, DbType.String, entity.Hempgrestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hempgrdeleted, DbType.Int32, entity.Hempgrdeleted));

            command.ExecuteNonQuery();
            return entity.Hempgrcodi;
        }

        public void Update(SiHisempgrupoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Grupocodiold, DbType.Int32, entity.Grupocodiold);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, entity.Migracodi);
            dbProvider.AddInParameter(command, helper.Hempgrfecha, DbType.DateTime, entity.Hempgrfecha);
            dbProvider.AddInParameter(command, helper.Hempgrestado, DbType.String, entity.Hempgrestado);
            dbProvider.AddInParameter(command, helper.Hempgrcodi, DbType.Int32, entity.Hempgrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteLogico(SiHisempgrupoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteLogico);
            dbProvider.AddInParameter(command, helper.Hempgrdeleted, DbType.Int32, entity.Hempgrdeleted);
            dbProvider.AddInParameter(command, helper.Hempgrcodi, DbType.Int32, entity.Hempgrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hempgrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Hempgrcodi, DbType.Int32, hempgrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiHisempgrupoDTO GetById(int hempgrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Hempgrcodi, DbType.Int32, hempgrcodi);
            SiHisempgrupoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiHisempgrupoDTO> List()
        {
            List<SiHisempgrupoDTO> entitys = new List<SiHisempgrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiHisempgrupoDTO> GetByCriteria()
        {
            List<SiHisempgrupoDTO> entitys = new List<SiHisempgrupoDTO>();
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
        }

        public List<SiHisempgrupoDTO> ListGrupsXMigracion(int migracodi)
        {
            List<SiHisempgrupoDTO> entitys = new List<SiHisempgrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListGrupsXMigracion);

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

        public int ConsultarGrpsMigracion(int migracodi, int grupocodi, DateTime fechacorte)
        {
            List<SiHisempgrupoDTO> entitys = new List<SiHisempgrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlConsultarGrpsMigracion);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Migracodi, DbType.Int32, migracodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            var equipo = entitys.Find(x => x.Hempgrfecha >= fechacorte);
            int validar = equipo != null ? 1 : 0;
            return validar;
        }
    }
}