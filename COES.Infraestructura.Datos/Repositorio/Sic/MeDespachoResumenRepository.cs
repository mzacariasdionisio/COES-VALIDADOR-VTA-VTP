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
    /// Clase de acceso a datos de la tabla ME_DESPACHO_RESUMEN
    /// </summary>
    public class MeDespachoResumenRepository : RepositoryBase, IMeDespachoResumenRepository
    {
        private string strConexion;
        public MeDespachoResumenRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        MeDespachoResumenHelper helper = new MeDespachoResumenHelper();

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

        public int Save(MeDespachoResumenDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregencodi, DbType.Int32, entity.Dregencodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenfecha, DbType.DateTime, entity.Dregenfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregentipo, DbType.Int32, entity.Dregentipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregentotalsein, DbType.Decimal, entity.Dregentotalsein));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregentotalexp, DbType.Decimal, entity.Dregentotalexp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregentotalimp, DbType.Decimal, entity.Dregentotalimp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenmdhora, DbType.DateTime, entity.Dregenmdhora));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenmdsein, DbType.Decimal, entity.Dregenmdsein));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenmdexp, DbType.Decimal, entity.Dregenmdexp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenmdimp, DbType.Decimal, entity.Dregenmdimp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregentotalnorte, DbType.Decimal, entity.Dregentotalnorte));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregentotalcentro, DbType.Decimal, entity.Dregentotalcentro));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregentotalsur, DbType.Decimal, entity.Dregentotalsur));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenmdhidro, DbType.Decimal, entity.Dregenmdhidro));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenmdtermo, DbType.Decimal, entity.Dregenmdtermo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenmdeolico, DbType.Decimal, entity.Dregenmdeolico));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenmdsolar, DbType.Decimal, entity.Dregenmdsolar));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenhphora, DbType.DateTime, entity.Dregenhphora));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenhpsein, DbType.Decimal, entity.Dregenhpsein));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenhpexp, DbType.Decimal, entity.Dregenhpexp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenhpimp, DbType.Decimal, entity.Dregenhpimp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenfhphora, DbType.DateTime, entity.Dregenfhphora));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenfhpsein, DbType.Decimal, entity.Dregenfhpsein));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenfhpexp, DbType.Decimal, entity.Dregenfhpexp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenfhpimp, DbType.Decimal, entity.Dregenfhpimp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenmdnoiihora, DbType.DateTime, entity.Dregenmdnoiihora));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Dregenmdnoiisein, DbType.Decimal, entity.Dregenmdnoiisein));

            command.ExecuteNonQuery();
            return entity.Dregencodi;
        }

        public void Update(MeDespachoResumenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Dregencodi, DbType.Int32, entity.Dregencodi);
            dbProvider.AddInParameter(command, helper.Dregenfecha, DbType.DateTime, entity.Dregenfecha);
            dbProvider.AddInParameter(command, helper.Dregentipo, DbType.Int32, entity.Dregentipo);
            dbProvider.AddInParameter(command, helper.Dregentotalsein, DbType.Decimal, entity.Dregentotalsein);
            dbProvider.AddInParameter(command, helper.Dregentotalexp, DbType.Decimal, entity.Dregentotalexp);
            dbProvider.AddInParameter(command, helper.Dregentotalimp, DbType.Decimal, entity.Dregentotalimp);
            dbProvider.AddInParameter(command, helper.Dregenmdhora, DbType.DateTime, entity.Dregenmdhora);
            dbProvider.AddInParameter(command, helper.Dregenmdsein, DbType.Decimal, entity.Dregenmdsein);
            dbProvider.AddInParameter(command, helper.Dregenmdexp, DbType.Decimal, entity.Dregenmdexp);
            dbProvider.AddInParameter(command, helper.Dregenmdimp, DbType.Decimal, entity.Dregenmdimp);
            dbProvider.AddInParameter(command, helper.Dregentotalnorte, DbType.Decimal, entity.Dregentotalnorte);
            dbProvider.AddInParameter(command, helper.Dregentotalcentro, DbType.Decimal, entity.Dregentotalcentro);
            dbProvider.AddInParameter(command, helper.Dregentotalsur, DbType.Decimal, entity.Dregentotalsur);
            dbProvider.AddInParameter(command, helper.Dregenmdhidro, DbType.Decimal, entity.Dregenmdhidro);
            dbProvider.AddInParameter(command, helper.Dregenmdtermo, DbType.Decimal, entity.Dregenmdtermo);
            dbProvider.AddInParameter(command, helper.Dregenmdeolico, DbType.Decimal, entity.Dregenmdeolico);
            dbProvider.AddInParameter(command, helper.Dregenmdsolar, DbType.Decimal, entity.Dregenmdsolar);
            dbProvider.AddInParameter(command, helper.Dregenmdnoiihora, DbType.DateTime, entity.Dregenmdnoiihora);
            dbProvider.AddInParameter(command, helper.Dregenmdnoiisein, DbType.Decimal, entity.Dregenmdnoiisein);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tipoDato, DateTime fechaIni, DateTime fechaFin, IDbConnection conn, DbTransaction tran)
        {
            string sql = string.Format(helper.SqlDelete, tipoDato, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = sql;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.ExecuteNonQuery();
        }

        public MeDespachoResumenDTO GetById(int dregencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Dregencodi, DbType.Int32, dregencodi);
            MeDespachoResumenDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeDespachoResumenDTO> List()
        {
            List<MeDespachoResumenDTO> entitys = new List<MeDespachoResumenDTO>();
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

        public List<MeDespachoResumenDTO> GetByCriteria(int tipoDato, DateTime fechaIni, DateTime fechaFin)
        {
            List<MeDespachoResumenDTO> entitys = new List<MeDespachoResumenDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, tipoDato, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
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
