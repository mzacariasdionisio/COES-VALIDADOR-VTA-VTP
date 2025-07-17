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
    /// Clase de acceso a datos de la tabla PMO_QN_MEDICION
    /// </summary>
    public class PmoQnMedicionRepository : RepositoryBase, IPmoQnMedicionRepository
    {
        public PmoQnMedicionRepository(string strConn) : base(strConn)
        {
        }

        PmoQnMedicionHelper helper = new PmoQnMedicionHelper();

        public int Save(PmoQnMedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Qnmedcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi);
            dbProvider.AddInParameter(command, helper.Qnlectcodi, DbType.Int32, entity.Qnlectcodi);
            dbProvider.AddInParameter(command, helper.Qnbenvcodi, DbType.Int32, entity.Qnbenvcodi);
            dbProvider.AddInParameter(command, helper.Qnmedfechaini, DbType.DateTime, entity.Qnmedfechaini);
            dbProvider.AddInParameter(command, helper.Qnmedfechafin, DbType.DateTime, entity.Qnmedfechafin);
            dbProvider.AddInParameter(command, helper.Qnmedsemini, DbType.Int32, entity.Qnmedsemini);
            dbProvider.AddInParameter(command, helper.Qnmedsemfin, DbType.Int32, entity.Qnmedsemfin);
            dbProvider.AddInParameter(command, helper.Qnmedanio, DbType.DateTime, entity.Qnmedanio);
            dbProvider.AddInParameter(command, helper.Qnmedh1, DbType.Decimal, entity.Qnmedh1);
            dbProvider.AddInParameter(command, helper.Qnmedh2, DbType.Decimal, entity.Qnmedh2);
            dbProvider.AddInParameter(command, helper.Qnmedh3, DbType.Decimal, entity.Qnmedh3);
            dbProvider.AddInParameter(command, helper.Qnmedh4, DbType.Decimal, entity.Qnmedh4);
            dbProvider.AddInParameter(command, helper.Qnmedh6, DbType.Decimal, entity.Qnmedh6);
            dbProvider.AddInParameter(command, helper.Qnmedh5, DbType.Decimal, entity.Qnmedh5);
            dbProvider.AddInParameter(command, helper.Qnmedh7, DbType.Decimal, entity.Qnmedh7);
            dbProvider.AddInParameter(command, helper.Qnmedh8, DbType.Decimal, entity.Qnmedh8);
            dbProvider.AddInParameter(command, helper.Qnmedh9, DbType.Decimal, entity.Qnmedh9);
            dbProvider.AddInParameter(command, helper.Qnmedh10, DbType.Decimal, entity.Qnmedh10);
            dbProvider.AddInParameter(command, helper.Qnmedh11, DbType.Decimal, entity.Qnmedh11);
            dbProvider.AddInParameter(command, helper.Qnmedh12, DbType.Decimal, entity.Qnmedh12);
            dbProvider.AddInParameter(command, helper.Qnmedh13, DbType.Decimal, entity.Qnmedh13);
            dbProvider.AddInParameter(command, helper.Qnmedo1, DbType.Int32, entity.Qnmedo1);
            dbProvider.AddInParameter(command, helper.Qnmedo2, DbType.Int32, entity.Qnmedo2);
            dbProvider.AddInParameter(command, helper.Qnmedo3, DbType.Int32, entity.Qnmedo3);
            dbProvider.AddInParameter(command, helper.Qnmedo4, DbType.Int32, entity.Qnmedo4);
            dbProvider.AddInParameter(command, helper.Qnmedo5, DbType.Int32, entity.Qnmedo5);
            dbProvider.AddInParameter(command, helper.Qnmedo6, DbType.Int32, entity.Qnmedo6);
            dbProvider.AddInParameter(command, helper.Qnmedo7, DbType.Int32, entity.Qnmedo7);
            dbProvider.AddInParameter(command, helper.Qnmedo8, DbType.Int32, entity.Qnmedo8);
            dbProvider.AddInParameter(command, helper.Qnmedo9, DbType.Int32, entity.Qnmedo9);
            dbProvider.AddInParameter(command, helper.Qnmedo10, DbType.Int32, entity.Qnmedo10);
            dbProvider.AddInParameter(command, helper.Qnmedo11, DbType.Int32, entity.Qnmedo11);
            dbProvider.AddInParameter(command, helper.Qnmedo12, DbType.Int32, entity.Qnmedo12);
            dbProvider.AddInParameter(command, helper.Qnmedo13, DbType.Int32, entity.Qnmedo13);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PmoQnMedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi);
            dbProvider.AddInParameter(command, helper.Qnlectcodi, DbType.Int32, entity.Qnlectcodi);
            dbProvider.AddInParameter(command, helper.Qnbenvcodi, DbType.Int32, entity.Qnbenvcodi);
            dbProvider.AddInParameter(command, helper.Qnmedfechaini, DbType.DateTime, entity.Qnmedfechaini);
            dbProvider.AddInParameter(command, helper.Qnmedfechafin, DbType.DateTime, entity.Qnmedfechafin);
            dbProvider.AddInParameter(command, helper.Qnmedsemini, DbType.Int32, entity.Qnmedsemini);
            dbProvider.AddInParameter(command, helper.Qnmedsemfin, DbType.Int32, entity.Qnmedsemfin);
            dbProvider.AddInParameter(command, helper.Qnmedanio, DbType.DateTime, entity.Qnmedanio);
            dbProvider.AddInParameter(command, helper.Qnmedh1, DbType.Decimal, entity.Qnmedh1);
            dbProvider.AddInParameter(command, helper.Qnmedh2, DbType.Decimal, entity.Qnmedh2);
            dbProvider.AddInParameter(command, helper.Qnmedh3, DbType.Decimal, entity.Qnmedh3);
            dbProvider.AddInParameter(command, helper.Qnmedh4, DbType.Decimal, entity.Qnmedh4);
            dbProvider.AddInParameter(command, helper.Qnmedh6, DbType.Decimal, entity.Qnmedh6);
            dbProvider.AddInParameter(command, helper.Qnmedh5, DbType.Decimal, entity.Qnmedh5);
            dbProvider.AddInParameter(command, helper.Qnmedh7, DbType.Decimal, entity.Qnmedh7);
            dbProvider.AddInParameter(command, helper.Qnmedh8, DbType.Decimal, entity.Qnmedh8);
            dbProvider.AddInParameter(command, helper.Qnmedh9, DbType.Decimal, entity.Qnmedh9);
            dbProvider.AddInParameter(command, helper.Qnmedh10, DbType.Decimal, entity.Qnmedh10);
            dbProvider.AddInParameter(command, helper.Qnmedh11, DbType.Decimal, entity.Qnmedh11);
            dbProvider.AddInParameter(command, helper.Qnmedh12, DbType.Decimal, entity.Qnmedh12);
            dbProvider.AddInParameter(command, helper.Qnmedh13, DbType.Decimal, entity.Qnmedh13);
            dbProvider.AddInParameter(command, helper.Qnmedo1, DbType.Int32, entity.Qnmedo1);
            dbProvider.AddInParameter(command, helper.Qnmedo2, DbType.Int32, entity.Qnmedo2);
            dbProvider.AddInParameter(command, helper.Qnmedo3, DbType.Int32, entity.Qnmedo3);
            dbProvider.AddInParameter(command, helper.Qnmedo4, DbType.Int32, entity.Qnmedo4);
            dbProvider.AddInParameter(command, helper.Qnmedo5, DbType.Int32, entity.Qnmedo5);
            dbProvider.AddInParameter(command, helper.Qnmedo6, DbType.Int32, entity.Qnmedo6);
            dbProvider.AddInParameter(command, helper.Qnmedo7, DbType.Int32, entity.Qnmedo7);
            dbProvider.AddInParameter(command, helper.Qnmedo8, DbType.Int32, entity.Qnmedo8);
            dbProvider.AddInParameter(command, helper.Qnmedo9, DbType.Int32, entity.Qnmedo9);
            dbProvider.AddInParameter(command, helper.Qnmedo10, DbType.Int32, entity.Qnmedo10);
            dbProvider.AddInParameter(command, helper.Qnmedo11, DbType.Int32, entity.Qnmedo11);
            dbProvider.AddInParameter(command, helper.Qnmedo12, DbType.Int32, entity.Qnmedo12);
            dbProvider.AddInParameter(command, helper.Qnmedo13, DbType.Int32, entity.Qnmedo13);
            dbProvider.AddInParameter(command, helper.Qnmedcodi, DbType.Int32, entity.Qnmedcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int qnmedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Qnmedcodi, DbType.Int32, qnmedcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PmoQnMedicionDTO GetById(int qnmedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Qnmedcodi, DbType.Int32, qnmedcodi);
            PmoQnMedicionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PmoQnMedicionDTO> List()
        {
            List<PmoQnMedicionDTO> entitys = new List<PmoQnMedicionDTO>();
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

        public List<PmoQnMedicionDTO> GetByCriteria(int enviocodi)
        {
            //List<PmoQnMedicionDTO> entitys = new List<PmoQnMedicionDTO>();
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            List<PmoQnMedicionDTO> entitys = new List<PmoQnMedicionDTO>();
            string query = string.Format(helper.SqlGetByCriteria, enviocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int Save(PmoQnMedicionDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnlectcodi, DbType.Int32, entity.Qnlectcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnbenvcodi, DbType.Int32, entity.Qnbenvcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedfechaini, DbType.DateTime, entity.Qnmedfechaini));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedfechafin, DbType.DateTime, entity.Qnmedfechafin));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedsemini, DbType.Int32, entity.Qnmedsemini));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedsemfin, DbType.Int32, entity.Qnmedsemfin));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedanio, DbType.DateTime, entity.Qnmedanio));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedh1, DbType.Decimal, entity.Qnmedh1));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedh2, DbType.Decimal, entity.Qnmedh2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedh3, DbType.Decimal, entity.Qnmedh3));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedh4, DbType.Decimal, entity.Qnmedh4));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedh6, DbType.Decimal, entity.Qnmedh6));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedh5, DbType.Decimal, entity.Qnmedh5));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedh7, DbType.Decimal, entity.Qnmedh7));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedh8, DbType.Decimal, entity.Qnmedh8));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedh9, DbType.Decimal, entity.Qnmedh9));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedh10, DbType.Decimal, entity.Qnmedh10));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedh11, DbType.Decimal, entity.Qnmedh11));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedh12, DbType.Decimal, entity.Qnmedh12));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedh13, DbType.Decimal, entity.Qnmedh13));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedo1, DbType.Int32, entity.Qnmedo1));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedo2, DbType.Int32, entity.Qnmedo2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedo3, DbType.Int32, entity.Qnmedo3));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedo4, DbType.Int32, entity.Qnmedo4));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedo5, DbType.Int32, entity.Qnmedo5));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedo6, DbType.Int32, entity.Qnmedo6));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedo7, DbType.Int32, entity.Qnmedo7));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedo8, DbType.Int32, entity.Qnmedo8));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedo9, DbType.Int32, entity.Qnmedo9));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedo10, DbType.Int32, entity.Qnmedo10));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedo11, DbType.Int32, entity.Qnmedo11));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedo12, DbType.Int32, entity.Qnmedo12));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Qnmedo13, DbType.Int32, entity.Qnmedo13));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void DeleteMedicionXEnvio(int qnbenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeletexEnvio);

            dbProvider.AddInParameter(command, helper.Qnbenvcodi, DbType.Int32, qnbenvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

    }
}
