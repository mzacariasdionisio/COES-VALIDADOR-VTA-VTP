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
    /// Clase de acceso a datos de la tabla SMA_ACTIVACION_OFERTA
    /// </summary>
    public class SmaActivacionOfertaRepository: RepositoryBase, ISmaActivacionOfertaRepository
    {
        public SmaActivacionOfertaRepository(string strConn): base(strConn)
        {
        }

        SmaActivacionOfertaHelper helper = new SmaActivacionOfertaHelper();

        public int Save(SmaActivacionOfertaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Smapaccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Smapacfecha, DbType.DateTime, entity.Smapacfecha);
            dbProvider.AddInParameter(command, helper.Smapacestado, DbType.String, entity.Smapacestado);
            dbProvider.AddInParameter(command, helper.Smapacusucreacion, DbType.String, entity.Smapacusucreacion);
            dbProvider.AddInParameter(command, helper.Smapacfeccreacion, DbType.DateTime, entity.Smapacfeccreacion);
            dbProvider.AddInParameter(command, helper.Smapacusumodificacion, DbType.String, entity.Smapacusumodificacion);
            dbProvider.AddInParameter(command, helper.Smapacfecmodificacion, DbType.DateTime, entity.Smapacfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int SaveTransaccional(SmaActivacionOfertaDTO entity, IDbConnection connection, DbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smapaccodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smapacfecha, DbType.DateTime, entity.Smapacfecha));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smapacestado, DbType.String, entity.Smapacestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smapacusucreacion, DbType.String, entity.Smapacusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smapacfeccreacion, DbType.DateTime, entity.Smapacfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smapacusumodificacion, DbType.String, entity.Smapacusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smapacfecmodificacion, DbType.DateTime, entity.Smapacfecmodificacion));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(SmaActivacionOfertaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Smapaccodi, DbType.Int32, entity.Smapaccodi);
            dbProvider.AddInParameter(command, helper.Smapacfecha, DbType.DateTime, entity.Smapacfecha);
            dbProvider.AddInParameter(command, helper.Smapacestado, DbType.String, entity.Smapacestado);
            dbProvider.AddInParameter(command, helper.Smapacusucreacion, DbType.String, entity.Smapacusucreacion);
            dbProvider.AddInParameter(command, helper.Smapacfeccreacion, DbType.DateTime, entity.Smapacfeccreacion);
            dbProvider.AddInParameter(command, helper.Smapacusumodificacion, DbType.String, entity.Smapacusumodificacion);
            dbProvider.AddInParameter(command, helper.Smapacfecmodificacion, DbType.DateTime, entity.Smapacfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int smapaccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Smapaccodi, DbType.Int32, smapaccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SmaActivacionOfertaDTO GetById(int smapaccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Smapaccodi, DbType.Int32, smapaccodi);
            SmaActivacionOfertaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SmaActivacionOfertaDTO> List()
        {
            List<SmaActivacionOfertaDTO> entitys = new List<SmaActivacionOfertaDTO>();
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

        public List<SmaActivacionOfertaDTO> GetByCriteria()
        {
            List<SmaActivacionOfertaDTO> entitys = new List<SmaActivacionOfertaDTO>();
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

        public List<SmaActivacionOfertaDTO> ListarActivacionesPorRangoFechas(DateTime fechaIni, DateTime fechaFin)
        {
            List<SmaActivacionOfertaDTO> entitys = new List<SmaActivacionOfertaDTO>();

            string query = string.Format(helper.SqlListarPorFechas, fechaIni.ToString(ConstantesBase.FormatoFechaPE), fechaFin.ToString(ConstantesBase.FormatoFechaPE));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SmaActivacionOfertaDTO entity = helper.Create(dr);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
    }
}
