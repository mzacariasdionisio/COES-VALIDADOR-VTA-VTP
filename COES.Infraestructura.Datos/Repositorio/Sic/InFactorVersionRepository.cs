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
    /// Clase de acceso a datos de la tabla IN_FACTOR_VERSION
    /// </summary>
    public class InFactorVersionRepository : RepositoryBase, IInFactorVersionRepository
    {
        public InFactorVersionRepository(string strConn) : base(strConn)
        {
        }

        InFactorVersionHelper helper = new InFactorVersionHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(InFactorVersionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infvercodi, DbType.Int32, entity.Infvercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infverfechaperiodo, DbType.DateTime, entity.Infverfechaperiodo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infvertipoeq, DbType.String, entity.Infvertipoeq));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infverdisp, DbType.String, entity.Infverdisp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infverflagfinal, DbType.String, entity.Infverflagfinal));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infverflagdefinitivo, DbType.String, entity.Infverflagdefinitivo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infverf1, DbType.Decimal, entity.Infverf1));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infverf2, DbType.Decimal, entity.Infverf2));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infverusucreacion, DbType.String, entity.Infverusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infverfeccreacion, DbType.DateTime, entity.Infverfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infvernro, DbType.Int32, entity.Infvernro));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infvercumpl, DbType.Decimal, entity.Infvercumpl));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infverhorizonte, DbType.Int32, entity.Infverhorizonte));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infvermodulo, DbType.Int32, entity.Infvermodulo));

            command.ExecuteNonQuery();
            return entity.Infvercodi;
        }

        public void Update(InFactorVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Infvercodi, DbType.Int32, entity.Infvercodi);
            dbProvider.AddInParameter(command, helper.Infverfechaperiodo, DbType.DateTime, entity.Infverfechaperiodo);
            dbProvider.AddInParameter(command, helper.Infvertipoeq, DbType.String, entity.Infvertipoeq);
            dbProvider.AddInParameter(command, helper.Infverdisp, DbType.String, entity.Infverdisp);
            dbProvider.AddInParameter(command, helper.Infverflagfinal, DbType.String, entity.Infverflagfinal);
            dbProvider.AddInParameter(command, helper.Infverflagdefinitivo, DbType.String, entity.Infverflagdefinitivo);
            dbProvider.AddInParameter(command, helper.Infverf1, DbType.Decimal, entity.Infverf1);
            dbProvider.AddInParameter(command, helper.Infverf2, DbType.Decimal, entity.Infverf2);
            dbProvider.AddInParameter(command, helper.Infverusucreacion, DbType.String, entity.Infverusucreacion);
            dbProvider.AddInParameter(command, helper.Infverfeccreacion, DbType.DateTime, entity.Infverfeccreacion);
            dbProvider.AddInParameter(command, helper.Infvernro, DbType.Int32, entity.Infvernro);
            dbProvider.AddInParameter(command, helper.Infvercumpl, DbType.Decimal, entity.Infvercumpl);
            dbProvider.AddInParameter(command, helper.Infverhorizonte, DbType.Int32, entity.Infverhorizonte);
            dbProvider.AddInParameter(command, helper.Infvermodulo, DbType.Int32, entity.Infvermodulo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateByFecha(DateTime fechaPeriodo, int modulo, int horizonte, IDbConnection conn, DbTransaction tran)
        {
            var fecha = fechaPeriodo.ToString(ConstantesBase.FormatoFecha);

            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdateByFecha;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infverfechaperiodo, DbType.String, fecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infvermodulo, DbType.Int32, modulo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infverhorizonte, DbType.Int32, horizonte));

            command.ExecuteNonQuery();
        }

        public void Delete(int infvercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Infvercodi, DbType.Int32, infvercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InFactorVersionDTO GetById(int infvercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Infvercodi, DbType.Int32, infvercodi);
            InFactorVersionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InFactorVersionDTO> List()
        {
            List<InFactorVersionDTO> entitys = new List<InFactorVersionDTO>();
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

        public List<InFactorVersionDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin)
        {
            List<InFactorVersionDTO> entitys = new List<InFactorVersionDTO>();

            string sqlFechaInicio = fechaInicio.ToString(ConstantesBase.FormatoFecha);
            string sqlFechaFin = fechaFin.ToString(ConstantesBase.FormatoFecha);

            string sql = string.Format(helper.SqlGetByCriteria, sqlFechaInicio, sqlFechaFin);

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

        public List<InFactorVersionDTO> GetByFecha(DateTime fechaPeriodo, int modulo)
        {
            List<InFactorVersionDTO> entitys = new List<InFactorVersionDTO>();
            var fecha = fechaPeriodo.ToString(ConstantesBase.FormatoFecha);
            string sql = string.Format(helper.SqlGetByFecha, fecha, modulo);

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
