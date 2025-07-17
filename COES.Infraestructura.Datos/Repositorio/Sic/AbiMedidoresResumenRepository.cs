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
    /// Clase de acceso a datos de la tabla ABI_MEDIDORES_RESUMEN
    /// </summary>
    public class AbiMedidoresResumenRepository: RepositoryBase, IAbiMedidoresResumenRepository
    {
        private string strConexion;
        public AbiMedidoresResumenRepository(string strConn): base(strConn)
        {
            strConexion = strConn;
        }

        AbiMedidoresResumenHelper helper = new AbiMedidoresResumenHelper();

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

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(AbiMedidoresResumenDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregencodi, DbType.Int32, entity.Mregencodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenfecha, DbType.DateTime, entity.Mregenfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregentotalsein, DbType.Decimal, entity.Mregentotalsein));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregentotalexp, DbType.Decimal, entity.Mregentotalexp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregentotalimp, DbType.Decimal, entity.Mregentotalimp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregentotalnorte, DbType.Decimal, entity.Mregentotalnorte));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregentotalcentro, DbType.Decimal, entity.Mregentotalcentro));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregentotalsur, DbType.Decimal, entity.Mregentotalsur));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenmdhora, DbType.DateTime, entity.Mregenmdhora));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenmdsein, DbType.Decimal, entity.Mregenmdsein));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenmdexp, DbType.Decimal, entity.Mregenmdexp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenmdimp, DbType.Decimal, entity.Mregenmdimp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenmdhidro, DbType.Decimal, entity.Mregenmdhidro));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenmdtermo, DbType.Decimal, entity.Mregenmdtermo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenmdeolico, DbType.Decimal, entity.Mregenmdeolico));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenmdsolar, DbType.Decimal, entity.Mregenmdsolar));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenhphora, DbType.DateTime, entity.Mregenhphora));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenhpsein, DbType.Decimal, entity.Mregenhpsein));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenhpexp, DbType.Decimal, entity.Mregenhpexp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenhpimp, DbType.Decimal, entity.Mregenhpimp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenfhphora, DbType.DateTime, entity.Mregenfhphora));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenfhpsein, DbType.Decimal, entity.Mregenfhpsein));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenfhpexp, DbType.Decimal, entity.Mregenfhpexp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenfhpimp, DbType.Decimal, entity.Mregenfhpimp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenmdnoiihora, DbType.DateTime, entity.Mregenmdnoiihora));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mregenmdnoiisein, DbType.Decimal, entity.Mregenmdnoiisein));

            command.ExecuteNonQuery();
            return entity.Mregencodi;
        }

        public void Update(AbiMedidoresResumenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mregencodi, DbType.Int32, entity.Mregencodi);
            dbProvider.AddInParameter(command, helper.Mregenfecha, DbType.DateTime, entity.Mregenfecha);
            dbProvider.AddInParameter(command, helper.Mregentotalsein, DbType.Decimal, entity.Mregentotalsein);
            dbProvider.AddInParameter(command, helper.Mregentotalexp, DbType.Decimal, entity.Mregentotalexp);
            dbProvider.AddInParameter(command, helper.Mregentotalimp, DbType.Decimal, entity.Mregentotalimp);
            dbProvider.AddInParameter(command, helper.Mregentotalnorte, DbType.Decimal, entity.Mregentotalnorte);
            dbProvider.AddInParameter(command, helper.Mregentotalcentro, DbType.Decimal, entity.Mregentotalcentro);
            dbProvider.AddInParameter(command, helper.Mregentotalsur, DbType.Decimal, entity.Mregentotalsur);
            dbProvider.AddInParameter(command, helper.Mregenmdhora, DbType.DateTime, entity.Mregenmdhora);
            dbProvider.AddInParameter(command, helper.Mregenmdsein, DbType.Decimal, entity.Mregenmdsein);
            dbProvider.AddInParameter(command, helper.Mregenmdexp, DbType.Decimal, entity.Mregenmdexp);
            dbProvider.AddInParameter(command, helper.Mregenmdimp, DbType.Decimal, entity.Mregenmdimp);
            dbProvider.AddInParameter(command, helper.Mregenmdhidro, DbType.Decimal, entity.Mregenmdhidro);
            dbProvider.AddInParameter(command, helper.Mregenmdtermo, DbType.Decimal, entity.Mregenmdtermo);
            dbProvider.AddInParameter(command, helper.Mregenmdeolico, DbType.Decimal, entity.Mregenmdeolico);
            dbProvider.AddInParameter(command, helper.Mregenmdsolar, DbType.Decimal, entity.Mregenmdsolar);
            dbProvider.AddInParameter(command, helper.Mregenmdnoiihora, DbType.DateTime, entity.Mregenmdnoiihora);
            dbProvider.AddInParameter(command, helper.Mregenmdnoiisein, DbType.Decimal, entity.Mregenmdnoiisein);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByRango(DateTime fechaIni, DateTime fechaFin, IDbConnection conn, DbTransaction tran)
        {
            string sql = string.Format(helper.SqlDelete, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = sql;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.ExecuteNonQuery();
        }

        public AbiMedidoresResumenDTO GetById(int mregencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mregencodi, DbType.Int32, mregencodi);
            AbiMedidoresResumenDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AbiMedidoresResumenDTO> List()
        {
            List<AbiMedidoresResumenDTO> entitys = new List<AbiMedidoresResumenDTO>();
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

        public List<AbiMedidoresResumenDTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin)
        {
            List<AbiMedidoresResumenDTO> entitys = new List<AbiMedidoresResumenDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
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
