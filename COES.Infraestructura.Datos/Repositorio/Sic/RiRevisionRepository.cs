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
    /// Clase de acceso a datos de la tabla RI_REVISION
    /// </summary>
    public class RiRevisionRepository : RepositoryBase, IRiRevisionRepository
    {
        public RiRevisionRepository(string strConn)
            : base(strConn)
        {
        }

        RiRevisionHelper helper = new RiRevisionHelper();
        public int Save(RiRevisionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            int id = 0;
            if (entity.Revicodi == 0)
            {
                DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
                object result = dbProvider.ExecuteScalar(command);
                id = 1;
                if (result != null) id = Convert.ToInt32(result);
            }
            else
            {
                id = entity.Revicodi + 1;
            }

            DbCommand command2 = (DbCommand)conn.CreateCommand();

            command2.CommandText = helper.SqlSave;
            command2.Transaction = tran;
            command2.Connection = (DbConnection)conn;

            IDbDataParameter param = command2.CreateParameter();
            param.ParameterName = helper.Revicodi;
            param.Value = id;
            command2.Parameters.Add(param);

            param = command2.CreateParameter(); param.ParameterName = helper.Reviiteracion; param.Value = entity.Reviiteracion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Reviestado; param.Value = entity.Reviestado; command2.Parameters.Add(param);

            param = command2.CreateParameter(); param.ParameterName = helper.Emprcodi; param.Value = entity.Emprcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Etrvcodi; param.Value = entity.Etrvcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Reviestadoregistro; param.Value = entity.Reviestadoregistro; command2.Parameters.Add(param);

            param = command2.CreateParameter(); param.ParameterName = helper.Reviusucreacion; param.Value = entity.Reviusucreacion == null ? "" : entity.Reviusucreacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Revifeccreacion; param.Value = entity.Revifeccreacion; command2.Parameters.Add(param);

            param = command2.CreateParameter(); param.ParameterName = helper.Reviusumodificacion; param.Value = entity.Reviusumodificacion == null ? "" : entity.Reviusumodificacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Revifecmodificacion;
            if (entity.Revifecmodificacion == null)
                param.Value = DBNull.Value;
            else
                param.Value = entity.Revifecmodificacion;

            command2.Parameters.Add(param);


            param = command2.CreateParameter(); param.ParameterName = helper.Reviusurevision;
            if (entity.Reviusurevision == null)
                param.Value = DBNull.Value;
            else
                param.Value = entity.Reviusurevision;

            command2.Parameters.Add(param);

            param = command2.CreateParameter(); param.ParameterName = helper.Revifecrevision;
            if (entity.Revifecrevision == null)
                param.Value = DBNull.Value;
            else
                param.Value = entity.Revifecrevision;

            command2.Parameters.Add(param);


            param = command2.CreateParameter(); param.ParameterName = helper.Revifinalizado; param.Value = entity.Revifinalizado; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Revifecfinalizado;
            if (entity.Revifecfinalizado == null)
                param.Value = DBNull.Value;
            else
                param.Value = entity.Revifecfinalizado;

            command2.Parameters.Add(param);

            param = command2.CreateParameter(); param.ParameterName = helper.Revinotificado; param.Value = entity.Revinotificado; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Revifecnotificado;
            if (entity.Revifecnotificado == null)
                param.Value = DBNull.Value;
            else
                param.Value = entity.Revifecnotificado;

            command2.Parameters.Add(param);

            param = command2.CreateParameter(); param.ParameterName = helper.Reviterminado; param.Value = entity.Reviterminado; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Revifecterminado;
            if (entity.Revifecterminado == null)
                param.Value = DBNull.Value;
            else
                param.Value = entity.Revifecterminado;

            command2.Parameters.Add(param);

            param = command2.CreateParameter(); param.ParameterName = helper.Revienviado; param.Value = entity.Revienviado; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Revifecenviado;
            if (entity.Revifecenviado == null)
                param.Value = DBNull.Value;
            else
                param.Value = entity.Revifecenviado;

            command2.Parameters.Add(param);


            command2.ExecuteNonQuery();

            return id;
        }
        public void Update(RiRevisionDTO entity, IDbConnection conn, DbTransaction tran)
        {

            DbCommand command = (DbCommand)conn.CreateCommand();

            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            IDbDataParameter param = null;

            param = command.CreateParameter(); param.ParameterName = helper.Reviusurevision;
            if (entity.Reviusurevision == null)
                param.Value = DBNull.Value;
            else
                param.Value = entity.Reviusurevision;

            command.Parameters.Add(param);
            param = command.CreateParameter(); param.ParameterName = helper.Revifecrevision; param.Value = entity.Revifecrevision; command.Parameters.Add(param);

            param = command.CreateParameter(); param.ParameterName = helper.Revifinalizado; param.Value = entity.Revifinalizado; command.Parameters.Add(param);
            param = command.CreateParameter(); param.ParameterName = helper.Revifecfinalizado;
            if (entity.Revifecfinalizado == null)
                param.Value = DBNull.Value;
            else
                param.Value = entity.Revifecfinalizado;

            command.Parameters.Add(param);

            param = command.CreateParameter(); param.ParameterName = helper.Revinotificado; param.Value = entity.Revinotificado; command.Parameters.Add(param);
            param = command.CreateParameter(); param.ParameterName = helper.Revifecnotificado;
            if (entity.Revifecnotificado == null)
                param.Value = DBNull.Value;
            else
                param.Value = entity.Revifecnotificado;

            command.Parameters.Add(param);

            param = command.CreateParameter(); param.ParameterName = helper.Reviterminado; param.Value = entity.Reviterminado; command.Parameters.Add(param);
            param = command.CreateParameter(); param.ParameterName = helper.Revifecterminado;
            if (entity.Revifecterminado == null)
                param.Value = DBNull.Value;
            else
                param.Value = entity.Revifecterminado;

            command.Parameters.Add(param);

            param = command.CreateParameter(); param.ParameterName = helper.Revienviado; param.Value = entity.Revienviado; command.Parameters.Add(param);
            param = command.CreateParameter(); param.ParameterName = helper.Revifecenviado;
            if (entity.Revifecenviado == null)
                param.Value = DBNull.Value;
            else
                param.Value = entity.Revifecenviado;

            command.Parameters.Add(param);

            param = command.CreateParameter(); param.ParameterName = helper.Reviusumodificacion; param.Value = entity.Reviusumodificacion; command.Parameters.Add(param);
            param = command.CreateParameter(); param.ParameterName = helper.Revifecmodificacion; param.Value = entity.Revifecmodificacion; command.Parameters.Add(param);
            param = command.CreateParameter(); param.ParameterName = helper.Reviestado; param.Value = entity.Reviestado; command.Parameters.Add(param);
            param = command.CreateParameter(); param.ParameterName = helper.Revicodi; param.Value = entity.Revicodi; command.Parameters.Add(param);


            command.ExecuteNonQuery();
        }
        public void UpdateEstadoRegistroInactivo(RiRevisionDTO entity, IDbConnection conn, DbTransaction tran)
        {

            DbCommand command = (DbCommand)conn.CreateCommand();

            command.CommandText = helper.SqlUpdateEstadoRegistroInactivo;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            IDbDataParameter param = null;

            param = command.CreateParameter(); param.ParameterName = helper.emprcodi; param.Value = entity.Emprcodi; command.Parameters.Add(param);
            param = command.CreateParameter(); param.ParameterName = helper.Etrvcodi; param.Value = entity.Etrvcodi; command.Parameters.Add(param);

            command.ExecuteNonQuery();
        }
        /// <summary>
        /// Permite finalizar una revisión
        /// </summary>
        /// <param name="entity"></param>
        public void Finalizar(RiRevisionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIteracion);
            object result = dbProvider.ExecuteScalar(command);
            int reviiteracion = 1;
            if (result != null) reviiteracion = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Revifinalizado, DbType.String, entity.Revifinalizado);
            dbProvider.AddInParameter(command, helper.Reviiteracion, DbType.Int32, reviiteracion);
            dbProvider.AddInParameter(command, helper.Reviestado, DbType.String, entity.Reviestado);
            dbProvider.AddInParameter(command, helper.Revifecrevision, DbType.DateTime, entity.Revifecrevision);
            dbProvider.AddInParameter(command, helper.Reviusumodificacion, DbType.String, entity.Reviusumodificacion);
            dbProvider.AddInParameter(command, helper.Revifecmodificacion, DbType.DateTime, entity.Revifecmodificacion);
            dbProvider.AddInParameter(command, helper.Reviusurevision, DbType.Int32, entity.Reviusurevision);
            dbProvider.AddInParameter(command, helper.Revicodi, DbType.Int32, entity.Revicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int revicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Revicodi, DbType.Int32, revicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RiRevisionDTO GetById(int revicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Revicodi, DbType.Int32, revicodi);
            RiRevisionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RiRevisionDTO> List()
        {
            List<RiRevisionDTO> entitys = new List<RiRevisionDTO>();
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

        //

        public List<SiEmpresaDTO> ListByEstadoAndTipEmp(string estado, int tipemprcodi, string nombre, int nroPage, int pageSize)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(String.Format(helper.SqlListByEstadoAndTipEmp, estado, tipemprcodi, nombre, nroPage, pageSize));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateList(dr));
                }
            }

            return entitys;
        }

        public int DarConformidad(int revicodi)
        {
            String query = String.Format(helper.SqlDarConformidad, revicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int count = dbProvider.ExecuteNonQuery(command);
            return count;
        }

        public int DarNotificar(int revicodi)
        {
            String query = String.Format(helper.SqlDarNotificar, revicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int count = dbProvider.ExecuteNonQuery(command);
            return count;
        }

        public int DarTerminar(int revicodi)
        {
            String query = String.Format(helper.SqlDarTerminar, revicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int count = dbProvider.ExecuteNonQuery(command);
            return count;
        }
        //
        public int RevAsistente(int revicodi)
        {
            String query = String.Format(helper.SqlRevAsistente, revicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int count = dbProvider.ExecuteNonQuery(command);
            return count;
        }

        public int ObtenerTotalListByEstadoAndTipEmp(string estado, int tipemprcodi, string nombre)
        {
            String query = String.Format(helper.SqlGetTotalRowsListByEstadoAndTipEmp, estado, tipemprcodi, nombre);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public List<RiRevisionDTO> GetByCriteria()
        {
            List<RiRevisionDTO> entitys = new List<RiRevisionDTO>();
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

        public RiRevisionDTO GetByEtapa(int etrvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByEtapa);

            dbProvider.AddInParameter(command, helper.Etrvcodi, DbType.Int32, etrvcodi);
            RiRevisionDTO entity = null;

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
