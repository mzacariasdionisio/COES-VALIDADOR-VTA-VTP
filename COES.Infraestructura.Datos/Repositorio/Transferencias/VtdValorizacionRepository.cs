using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;
using Microsoft.Practices.EnterpriseLibrary.Data;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VTD_VALORIZACION
    /// </summary>
    public class VtdValorizacionRepository : RepositoryBase, IVtdValorizacionRepository
    {
        private string strConexion;
        public VtdValorizacionRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        VtdValorizacionHelper helper = new VtdValorizacionHelper();
        VtpRecalculoPotenciaHelper helperRP = new VtpRecalculoPotenciaHelper();
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

        public int Save(VtdValorizacionDTO entity)
        {
            DbCommand commandMax = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(commandMax);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Valocodi, DbType.Decimal, id);
            dbProvider.AddInParameter(command, helper.Valofecha, DbType.DateTime, entity.Valofecha);
            dbProvider.AddInParameter(command, helper.Valomr, DbType.Decimal, entity.Valomr);
            dbProvider.AddInParameter(command, helper.Valopreciopotencia, DbType.Decimal, entity.Valopreciopotencia);
            dbProvider.AddInParameter(command, helper.Valodemandacoes, DbType.Decimal, entity.Valodemandacoes);
            dbProvider.AddInParameter(command, helper.Valofactorreparto, DbType.Decimal, entity.Valofactorreparto);
            dbProvider.AddInParameter(command, helper.Valoporcentajeperdida, DbType.Decimal, entity.Valoporcentajeperdida);
            dbProvider.AddInParameter(command, helper.Valofrectotal, DbType.Decimal, entity.Valofrectotal);
            dbProvider.AddInParameter(command, helper.Valootrosequipos, DbType.Decimal, entity.Valootrosequipos);
            dbProvider.AddInParameter(command, helper.Valocostofuerabanda, DbType.Decimal, entity.Valocostofuerabanda);
            dbProvider.AddInParameter(command, helper.Valoco, DbType.Decimal, entity.Valoco);
            dbProvider.AddInParameter(command, helper.Valora, DbType.Decimal, entity.Valora);
            dbProvider.AddInParameter(command, helper.ValoraSub, DbType.Decimal, entity.ValoraSub);
            dbProvider.AddInParameter(command, helper.ValoraBaj, DbType.Decimal, entity.ValoraBaj );
            dbProvider.AddInParameter(command, helper.Valoofmax, DbType.Decimal, entity.Valoofmax);
            dbProvider.AddInParameter(command, helper.ValoofmaxBaj, DbType.Decimal, entity.ValoofmaxBaj);
            dbProvider.AddInParameter(command, helper.Valocompcostosoper, DbType.Decimal, entity.Valocompcostosoper);
            dbProvider.AddInParameter(command, helper.Valocomptermrt, DbType.Decimal, entity.Valocomptermrt);
            dbProvider.AddInParameter(command, helper.Valoestado, DbType.String, entity.Valoestado);
            dbProvider.AddInParameter(command, helper.Valousucreacion, DbType.String, entity.Valousucreacion);
            dbProvider.AddInParameter(command, helper.Valofeccreacion, DbType.DateTime, entity.Valofeccreacion);
            dbProvider.AddInParameter(command, helper.Valousumodificacion, DbType.String, entity.Valousumodificacion);
            dbProvider.AddInParameter(command, helper.Valofecmodificacion, DbType.DateTime, entity.Valofecmodificacion);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }


        public int Save(VtdValorizacionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
                object result = dbProvider.ExecuteScalar(command);
                int id = 1;
                if (result != null) id = Convert.ToInt32(result);

                DbCommand commandUpd = (DbCommand)conn.CreateCommand();
                commandUpd.CommandText = helper.SqlSave;
                commandUpd.Transaction = tran;
                commandUpd.Connection = (DbConnection)conn;

                IDbDataParameter paramUpd = commandUpd.CreateParameter();
                paramUpd.ParameterName = helper.Valocodi;
                paramUpd.Value = id;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valofecha;
                paramUpd.Value = entity.Valofecha;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valomr;
                paramUpd.Value = entity.Valomr;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valopreciopotencia;
                paramUpd.Value = entity.Valopreciopotencia;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valodemandacoes;
                paramUpd.Value = entity.Valodemandacoes;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valofactorreparto;
                paramUpd.Value = entity.Valofactorreparto;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valoporcentajeperdida;
                paramUpd.Value = entity.Valoporcentajeperdida;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valofrectotal;
                paramUpd.Value = entity.Valofrectotal;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valootrosequipos;
                paramUpd.Value = entity.Valootrosequipos;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valocostofuerabanda;
                paramUpd.Value = entity.Valocostofuerabanda;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valoco;
                paramUpd.Value = entity.Valoco;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valora;
                paramUpd.Value = entity.Valora;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valoofmax;
                paramUpd.Value = entity.Valoofmax;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valocompcostosoper;
                paramUpd.Value = entity.Valocompcostosoper;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valocomptermrt;
                paramUpd.Value = entity.Valocomptermrt;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valoestado;
                paramUpd.Value = entity.Valoestado;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valousucreacion;
                paramUpd.Value = entity.Valousucreacion;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valofeccreacion;
                paramUpd.Value = entity.Valofeccreacion;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valousumodificacion;
                paramUpd.Value = entity.Valousumodificacion;
                commandUpd.Parameters.Add(paramUpd);

                paramUpd.ParameterName = helper.Valofecmodificacion;
                paramUpd.Value = entity.Valofecmodificacion;
                commandUpd.Parameters.Add(paramUpd);

                commandUpd.ExecuteNonQuery();
                return id;

            }
            catch (Exception)
            {

                return -1;
            }
        }

        public bool Update(VtdValorizacionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                DbCommand command2 = (DbCommand)conn.CreateCommand();
                command2.CommandText = helper.SqlUpdate;
                command2.Transaction = tran;
                command2.Connection = (DbConnection)conn;

                IDbDataParameter param = null;
                
                param = command2.CreateParameter(); param.ParameterName = helper.Valofecha; param.Value = entity.Valofecha; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valomr; param.Value = entity.Valomr; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valopreciopotencia; param.Value = entity.Valopreciopotencia; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valodemandacoes; param.Value = entity.Valodemandacoes; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valofactorreparto; param.Value = entity.Valofactorreparto; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valoporcentajeperdida; param.Value = entity.Valoporcentajeperdida; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valofrectotal; param.Value = entity.Valofrectotal; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valootrosequipos; param.Value = entity.Valootrosequipos; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valocostofuerabanda; param.Value = entity.Valocostofuerabanda; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valoco; param.Value = entity.Valoco; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valora; param.Value = entity.Valora; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.ValoraSub; param.Value = entity.ValoraSub; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.ValoraBaj; param.Value = entity.ValoraBaj ; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valoofmax; param.Value = entity.Valoofmax; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.ValoofmaxBaj; param.Value = entity.ValoofmaxBaj; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valocompcostosoper; param.Value = entity.Valocompcostosoper; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valocomptermrt; param.Value = entity.Valocomptermrt; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valoestado; param.Value = entity.Valoestado; param.DbType = DbType.String; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valousucreacion; param.Value = entity.Valousucreacion; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valofeccreacion; param.Value = entity.Valofeccreacion; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valousumodificacion; param.Value = entity.Valousumodificacion; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valofecmodificacion; param.Value = entity.Valofecmodificacion; command2.Parameters.Add(param);
                param = command2.CreateParameter(); param.ParameterName = helper.Valocodi; param.Value = entity.Valocodi; command2.Parameters.Add(param);

                command2.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {

                return false;
            }
        }



        public void UpdateEstado(VtdValorizacionDTO entity, IDbConnection conn, DbTransaction tran, String Emprcodi)
        {
            try
            {
                DbCommand command2 = (DbCommand)conn.CreateCommand();
                command2.CommandText = string.Format(helper.SqlUpdateEstadoPorEmpresa, ((DateTime)entity.Valofecha).ToString(ConstantesBase.FormatoFecha), Emprcodi);
                //command2.CommandText = string.Format(helper.SqlUpdateEstado, ((DateTime)entity.Valofecha).ToString(ConstantesBase.FormatoFecha));
                command2.Transaction = tran;
                command2.Connection = (DbConnection)conn;

                //IDbDataParameter param = null;

                //param = command2.CreateParameter(); param.ParameterName = helper.Valofecha; param.Value = 
                //    ((DateTime)entity.Valofecha).ToString(ConstantesBase.FormatoFecha); command2.Parameters.Add(param);
                

                command2.ExecuteNonQuery();
                

            }
            catch (Exception ex)
            {
                
            }
        }

        public void Update(VtdValorizacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Valocodi, DbType.Decimal, entity.Valocodi);
            dbProvider.AddInParameter(command, helper.Valofecha, DbType.DateTime, entity.Valofecha);
            dbProvider.AddInParameter(command, helper.Valomr, DbType.Decimal, entity.Valomr);
            dbProvider.AddInParameter(command, helper.Valopreciopotencia, DbType.Decimal, entity.Valopreciopotencia);
            dbProvider.AddInParameter(command, helper.Valodemandacoes, DbType.Decimal, entity.Valodemandacoes);
            dbProvider.AddInParameter(command, helper.Valofactorreparto, DbType.Decimal, entity.Valofactorreparto);
            dbProvider.AddInParameter(command, helper.Valoporcentajeperdida, DbType.Decimal, entity.Valoporcentajeperdida);
            dbProvider.AddInParameter(command, helper.Valofrectotal, DbType.Decimal, entity.Valofrectotal);
            dbProvider.AddInParameter(command, helper.Valootrosequipos, DbType.Decimal, entity.Valootrosequipos);
            dbProvider.AddInParameter(command, helper.Valocostofuerabanda, DbType.Decimal, entity.Valocostofuerabanda);
            dbProvider.AddInParameter(command, helper.Valoco, DbType.Decimal, entity.Valoco);
            dbProvider.AddInParameter(command, helper.Valora, DbType.Decimal, entity.Valora);
            dbProvider.AddInParameter(command, helper.Valoofmax, DbType.Decimal, entity.Valoofmax);
            dbProvider.AddInParameter(command, helper.Valocompcostosoper, DbType.Decimal, entity.Valocompcostosoper);
            dbProvider.AddInParameter(command, helper.Valocomptermrt, DbType.Decimal, entity.Valocomptermrt);
            dbProvider.AddInParameter(command, helper.Valoestado, DbType.String, entity.Valoestado);
            dbProvider.AddInParameter(command, helper.Valousucreacion, DbType.String, entity.Valousucreacion);
            dbProvider.AddInParameter(command, helper.Valofeccreacion, DbType.DateTime, entity.Valofeccreacion);
            dbProvider.AddInParameter(command, helper.Valousumodificacion, DbType.String, entity.Valousumodificacion);
            dbProvider.AddInParameter(command, helper.Valofecmodificacion, DbType.DateTime, entity.Valofecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Valocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Valocodi, DbType.Decimal, Valocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtdValorizacionDTO GetById(int Valocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Valocodi, DbType.Int32, Valocodi);
            VtdValorizacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtdValorizacionDTO> List()
        {
            List<VtdValorizacionDTO> entitys = new List<VtdValorizacionDTO>();
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

        public List<VtdValorizacionDTO> GetByCriteria()
        {
            List<VtdValorizacionDTO> entitys = new List<VtdValorizacionDTO>();
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

        public VtdValorizacionDTO ObtenerPrecioPotencia(int pericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helperRP.SqlGetPrecioPotencia);

            dbProvider.AddInParameter(command, "pericodi", DbType.Int32, pericodi);
            VtdValorizacionDTO entity = new VtdValorizacionDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iValopreciopotencia = dr.GetOrdinal("VALOPRECIOPOTENCIA");
                    if (!dr.IsDBNull(iValopreciopotencia)) entity.Valopreciopotencia = Convert.ToDecimal(dr.GetValue(iValopreciopotencia));
                }
            }

            return entity;
        }

        public List<SiEmpresaDTO> ObtenerEmpresas()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
