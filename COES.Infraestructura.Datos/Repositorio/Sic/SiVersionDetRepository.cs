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
    /// Clase de acceso a datos de la tabla SI_VERSIONIEOD_DET
    /// </summary>
    public class SiVersionDetRepository : RepositoryBase, ISiVersionDetRepository
    {
        public SiVersionDetRepository(string strConn)
            : base(strConn)
        {
        }

        SiVersionDetHelper helper = new SiVersionDetHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiVersionDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Versdtcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mrepcodi, DbType.Int32, entity.Mrepcodi);
            dbProvider.AddInParameter(command, helper.Verscodi, DbType.Int32, entity.Verscodi);
            dbProvider.AddInParameter(command, helper.Versdtnroreporte, DbType.Decimal, entity.Versdtnroreporte);
            dbProvider.AddInParameter(command, helper.Versdtdatos, DbType.Binary, entity.Versdtdatos);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int SaveTransaccional(SiVersionDetDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Versdtcodi, DbType.Int32, entity.Versdtcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mrepcodi, DbType.Int32, entity.Mrepcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Verscodi, DbType.Int32, entity.Verscodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Versdtnroreporte, DbType.Decimal, entity.Versdtnroreporte));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Versdtdatos, DbType.Binary, entity.Versdtdatos));

            command.ExecuteNonQuery();
            return entity.Versdtcodi;
        }

        public SiVersionDTO GetByVersionDetIEOD(int verscodi, decimal nroReporte)
        {
            string query = string.Format(helper.SqlGetByVersionDetIEOD, verscodi, nroReporte);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            SiVersionDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new SiVersionDTO();

                    int iVerscodi = dr.GetOrdinal(this.helper.Verscodi);
                    if (!dr.IsDBNull(iVerscodi)) entity.Verscodi = Convert.ToInt32(dr.GetValue(iVerscodi));

                    int iVersnroreporte = dr.GetOrdinal(this.helper.Versdtnroreporte);
                    if (!dr.IsDBNull(iVersnroreporte)) entity.Versnroreporte = Convert.ToInt32(dr.GetValue(iVersnroreporte));

                    int iVersdatos = dr.GetOrdinal(this.helper.Versdtdatos);
                    if (!dr.IsDBNull(iVersdatos)) entity.Versdatos = (byte[])dr.GetValue(iVersdatos);
                }
            }

            return entity;
        }

        public SiVersionDetDTO GetByIdVersionYNumeral(int verscodi, int mrepcodi)
        {
            string sql = string.Format(helper.SqlGetById, verscodi, mrepcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            SiVersionDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
