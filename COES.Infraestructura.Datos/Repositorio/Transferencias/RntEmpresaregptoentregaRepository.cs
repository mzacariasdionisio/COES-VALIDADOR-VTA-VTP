using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System.Data; 
using System.Data.Common; 

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RNT_EMPRESA_REGPTOENTREGA
    /// </summary>
    public class RntEmpresaregptoentregaRepository : RepositoryBase, IRntEmpresaRegptoentregaRepository
    {
        public RntEmpresaregptoentregaRepository(string strConn)
            : base(strConn)
        {
        }

        RntEmpresaRegptoentregaHelper helper = new RntEmpresaRegptoentregaHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(RntEmpresaRegptoentregaDTO entity, IDbConnection conn, DbTransaction tran, int corrId)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            if (corrId != 0)
                id = corrId + 1;
            else
            {
                object result = dbProvider.ExecuteScalar(command);
                if (result != null) id = Convert.ToInt32(result);
            }

            DbCommand command2 = (DbCommand)conn.CreateCommand();

            command2.CommandText = helper.SqlSave;
            command2.Transaction = tran;
            command2.Connection = (DbConnection)conn;

            IDbDataParameter param = command2.CreateParameter();
            param.ParameterName = helper.Empgencodi; param.Value = id; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Regpuntoentcodi; param.Value = entity.RegPuntoEntCodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Emprcodi; param.Value = entity.EmprCodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Regporcentaje; param.Value = entity.RegPorcentaje; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Peeusuariocreacion; param.Value = entity.PeeUsuarioCreacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Peefechacreacion; param.Value = entity.PeeFechaCreacion; command2.Parameters.Add(param);

            command2.ExecuteNonQuery();

            return id;
        }

        public void Update(RntEmpresaRegptoentregaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Regpuntoentcodi, DbType.Int32, entity.RegPuntoEntCodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.Regporcentaje, DbType.Decimal, entity.RegPorcentaje);
            dbProvider.AddInParameter(command, helper.Peeusuarioupdate, DbType.String, entity.PeeUsuarioUpdate);
            dbProvider.AddInParameter(command, helper.Peefechaupdate, DbType.DateTime, entity.PeeFechaUpdate);
            dbProvider.AddInParameter(command, helper.Empgencodi, DbType.Int32, entity.EmpGenCodi);


            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int empgencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Empgencodi, DbType.Int32, empgencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RntEmpresaRegptoentregaDTO GetById(int empgencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Empgencodi, DbType.Int32, empgencodi);
            RntEmpresaRegptoentregaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RntEmpresaRegptoentregaDTO> List(int key)
        {
            List<RntEmpresaRegptoentregaDTO> entitys = new List<RntEmpresaRegptoentregaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Regpuntoentcodi, DbType.Int32, key);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<RntEmpresaRegptoentregaDTO> GetByCriteria()
        {
            List<RntEmpresaRegptoentregaDTO> entitys = new List<RntEmpresaRegptoentregaDTO>();
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
