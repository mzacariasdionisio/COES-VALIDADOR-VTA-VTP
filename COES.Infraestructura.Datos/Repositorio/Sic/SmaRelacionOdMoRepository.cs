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
    /// Clase de acceso a datos de la tabla SMA_RELACION_OD_MO
    /// </summary>
    public class SmaRelacionOdMoRepository : RepositoryBase, ISmaRelacionOdMoRepository
    {
        public SmaRelacionOdMoRepository(string strConn)
            : base(strConn)
        {
        }

        SmaRelacionOdMoHelper helper = new SmaRelacionOdMoHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(int oferdecodi, SmaRelacionOdMoDTO entity, IDbConnection conn, DbTransaction tran, int numID)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            if (numID != 0) id = numID + 1;
            else
            {
                object result = dbProvider.ExecuteScalar(command);
                if (result != null) id = Convert.ToInt32(result);
            }

            DbCommand command2 = (DbCommand)conn.CreateCommand();

            command2.CommandText = helper.SqlSave; // = db.GetSqlStringCommand(helper.SqlSave);
            command2.Transaction = tran;
            command2.Connection = (DbConnection)conn;

            IDbDataParameter param = command2.CreateParameter();
            param.ParameterName = helper.Odmousucreacion;
            param.Value = entity.Odmousucreacion;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Ofdecodi;
            param.Value = oferdecodi;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Odmocodi;
            param.Value = id;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Grupocodi;
            param.Value = entity.Grupocodi;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Odmopotmaxofer;
            param.Value = entity.Odmopotmaxofer;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Odmobndcalificada;
            param.Value = entity.Odmobndcalificada;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Odmobnddisponible;
            param.Value = entity.Odmobnddisponible;
            command2.Parameters.Add(param);

            try
            {
                command2.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return -1;
            }

            return id;
        }

        public void Update(SmaRelacionOdMoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Odmousucreacion, DbType.String, entity.Odmousucreacion);
            dbProvider.AddInParameter(command, helper.Odmofeccreacion, DbType.DateTime, entity.Odmofeccreacion);
            dbProvider.AddInParameter(command, helper.Odmousumodificacion, DbType.String, entity.Odmousumodificacion);
            dbProvider.AddInParameter(command, helper.Odmofecmodificacion, DbType.DateTime, entity.Odmofecmodificacion);
            dbProvider.AddInParameter(command, helper.Ofdecodi, DbType.Int32, entity.Ofdecodi);
            dbProvider.AddInParameter(command, helper.Odmocodi, DbType.Int32, entity.Odmocodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int odmocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Odmocodi, DbType.Int32, odmocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SmaRelacionOdMoDTO GetById(int odmocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Odmocodi, DbType.Int32, odmocodi);
            SmaRelacionOdMoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SmaRelacionOdMoDTO> List()
        {
            List<SmaRelacionOdMoDTO> entitys = new List<SmaRelacionOdMoDTO>();
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

        public List<SmaRelacionOdMoDTO> GetByCriteria()
        {
            List<SmaRelacionOdMoDTO> entitys = new List<SmaRelacionOdMoDTO>();
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
    }
}
