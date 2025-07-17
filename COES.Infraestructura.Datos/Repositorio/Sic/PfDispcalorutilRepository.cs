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
    /// Clase de acceso a datos de la tabla PF_DISPCALORUTIL
    /// </summary>
    public class PfDispcalorutilRepository : RepositoryBase, IPfDispcalorutilRepository
    {
        public PfDispcalorutilRepository(string strConn) : base(strConn)
        {
        }

        PfDispcalorutilHelper helper = new PfDispcalorutilHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(PfDispcalorutilDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfcucodi, DbType.Int32, entity.Pfcucodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfcufecha, DbType.DateTime, entity.Pfcufecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfcuh, DbType.Int32, entity.Pfcuh));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfcutienedisp, DbType.Int32, entity.Pfcutienedisp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptcodi, DbType.Int32, entity.Irptcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfcumin, DbType.Int32, entity.Pfcumin));

            command.ExecuteNonQuery();
            return entity.Pfcucodi;
        }

        public void Update(PfDispcalorutilDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfcufecha, DbType.DateTime, entity.Pfcufecha);
            dbProvider.AddInParameter(command, helper.Pfcuh, DbType.Int32, entity.Pfcuh);
            dbProvider.AddInParameter(command, helper.Pfcutienedisp, DbType.Int32, entity.Pfcutienedisp);
            dbProvider.AddInParameter(command, helper.Irptcodi, DbType.Int32, entity.Irptcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);

            dbProvider.AddInParameter(command, helper.Pfcucodi, DbType.Int32, entity.Pfcucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfcucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfcucodi, DbType.Int32, pfcucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfDispcalorutilDTO GetById(int pfcucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfcucodi, DbType.Int32, pfcucodi);
            PfDispcalorutilDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfDispcalorutilDTO> List()
        {
            List<PfDispcalorutilDTO> entitys = new List<PfDispcalorutilDTO>();
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

        public List<PfDispcalorutilDTO> GetByCriteria(int irptcodi)
        {
            List<PfDispcalorutilDTO> entitys = new List<PfDispcalorutilDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, irptcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
