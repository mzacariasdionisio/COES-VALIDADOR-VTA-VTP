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
    /// Clase de acceso a datos de la tabla IND_REPORTE_CALCULOS
    /// </summary>
    public class IndReporteCalculosRepository : RepositoryBase, IIndReporteCalculosRepository
    {
        public IndReporteCalculosRepository(string strConn) : base(strConn)
        {
        }

        IndReporteCalculosHelper helper = new IndReporteCalculosHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void Save(IndReporteCalculosDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irpcalcodi, DbType.Int32, entity.Irpcalcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotcodi, DbType.Int32, entity.Itotcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irpcaltipo, DbType.Int32, entity.Irpcaltipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D1, DbType.Decimal, entity.D1));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D2, DbType.Decimal, entity.D2));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D3, DbType.Decimal, entity.D3));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D4, DbType.Decimal, entity.D4));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D5, DbType.Decimal, entity.D5));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D6, DbType.Decimal, entity.D6));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D7, DbType.Decimal, entity.D7));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D8, DbType.Decimal, entity.D8));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D9, DbType.Decimal, entity.D9));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D10, DbType.Decimal, entity.D10));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D11, DbType.Decimal, entity.D11));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D12, DbType.Decimal, entity.D12));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D13, DbType.Decimal, entity.D13));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D14, DbType.Decimal, entity.D14));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D15, DbType.Decimal, entity.D15));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D16, DbType.Decimal, entity.D16));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D17, DbType.Decimal, entity.D17));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D18, DbType.Decimal, entity.D18));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D19, DbType.Decimal, entity.D19));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D20, DbType.Decimal, entity.D20));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D21, DbType.Decimal, entity.D21));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D22, DbType.Decimal, entity.D22));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D23, DbType.Decimal, entity.D23));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D24, DbType.Decimal, entity.D24));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D25, DbType.Decimal, entity.D25));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D26, DbType.Decimal, entity.D26));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D27, DbType.Decimal, entity.D27));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D28, DbType.Decimal, entity.D28));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D29, DbType.Decimal, entity.D29));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D30, DbType.Decimal, entity.D30));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.D31, DbType.Decimal, entity.D31));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irpcalusucreacion, DbType.String, entity.Irpcalusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irpcalfeccreacion, DbType.DateTime, entity.Irpcalfeccreacion));
            command.ExecuteNonQuery();
        }

        public void Delete(int irpcalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Irpcalcodi, DbType.Int32, irpcalcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndReporteCalculosDTO GetById(int irpcalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Irpcalcodi, DbType.Int32, irpcalcodi);
            IndReporteCalculosDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndReporteCalculosDTO> GetByCriteria(string itotcodi)
        {
            List<IndReporteCalculosDTO> entitys = new List<IndReporteCalculosDTO>();

            string query = string.Format(helper.SqlGetByCriteria, itotcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndReporteCalculosDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndReporteCalculosDTO> List()
        {
            List<IndReporteCalculosDTO> entitys = new List<IndReporteCalculosDTO>();
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

    }
}
