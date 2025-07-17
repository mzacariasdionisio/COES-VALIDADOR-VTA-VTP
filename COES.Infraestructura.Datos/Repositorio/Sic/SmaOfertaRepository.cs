using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SMA_OFERTA
    /// </summary>
    public class SmaOfertaRepository : RepositoryBase, ISmaOfertaRepository
    {
        private string strConexion;
        public SmaOfertaRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        SmaOfertaHelper helper = new SmaOfertaHelper();

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

        public int Save(SmaOfertaDTO entity, string urscodisEnvio, IDbConnection conn, DbTransaction tran)
        {
            //Cambiar Estado Ofertas del Dia registradas anteriormente

            string sqlUpdOferDia = string.Format(helper.SqlUpdateOferDia, urscodisEnvio, entity.Oferfuente, entity.Ofertipo, entity.Oferfechainicio.Value.ToString(ConstantesBase.FormatoFecha));

            DbCommand commandUpd = (DbCommand)conn.CreateCommand();
            commandUpd.CommandText = sqlUpdOferDia;
            commandUpd.Transaction = tran;
            commandUpd.Connection = (DbConnection)conn;
            commandUpd.ExecuteNonQuery();

            //Guardar nuevo registro
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            var result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            DbCommand command2 = (DbCommand)conn.CreateCommand();

            command2.CommandText = helper.SqlSave;
            command2.Transaction = tran;
            command2.Connection = (DbConnection)conn;

            IDbDataParameter param = command2.CreateParameter();
            param.ParameterName = helper.Ofertipo;
            param.Value = entity.Ofertipo;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Oferfechainicio;
            param.Value = entity.Oferfechainicio;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Oferfechafin;
            param.Value = entity.Oferfechafin;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Ofercodi;
            param.Value = id;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Oferusucreacion;
            param.Value = entity.Oferusucreacion;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Oferfechaenvio;
            param.Value = entity.Oferfechaenvio;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Ofercodi;
            param.Value = id;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Usercode;
            param.Value = entity.Usercode;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Oferfuente;
            param.Value = entity.Oferfuente;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Smapaccodi;
            param.Value = entity.Smapaccodi;
            command2.Parameters.Add(param);

            command2.ExecuteNonQuery();

            return id;

        }

        public void Update(SmaOfertaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ofertipo, DbType.Int32, entity.Ofertipo);
            dbProvider.AddInParameter(command, helper.Oferfechainicio, DbType.DateTime, entity.Oferfechainicio);
            dbProvider.AddInParameter(command, helper.Oferfechafin, DbType.DateTime, entity.Oferfechafin);
            dbProvider.AddInParameter(command, helper.Ofercodenvio, DbType.String, entity.Ofercodenvio);
            dbProvider.AddInParameter(command, helper.Oferestado, DbType.String, entity.Oferestado);
            dbProvider.AddInParameter(command, helper.Oferusumodificacion, DbType.String, entity.Oferusumodificacion);
            dbProvider.AddInParameter(command, helper.Ofercodi, DbType.Int32, entity.Ofercodi);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);

            dbProvider.ExecuteNonQuery(command);
        }

        public SmaOfertaDTO GetById(int ofercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ofercodi, DbType.Int32, ofercodi);
            SmaOfertaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SmaOfertaDTO> List(int ofertipo, DateTime oferfechaenvio, int usercode, int ofercodi, string oferestado, string oferfuente)
        {
            List<SmaOfertaDTO> entitys = new List<SmaOfertaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            dbProvider.AddInParameter(command, helper.Oferestado, DbType.String, oferestado);
            dbProvider.AddInParameter(command, helper.Oferestado, DbType.String, oferestado);
            dbProvider.AddInParameter(command, helper.Ofertipo, DbType.Int32, ofertipo);
            dbProvider.AddInParameter(command, helper.Ofertipo, DbType.Int32, ofertipo);
            dbProvider.AddInParameter(command, helper.Oferfechainicio, DbType.DateTime, oferfechaenvio);
            dbProvider.AddInParameter(command, helper.Oferfechainicio, DbType.DateTime, oferfechaenvio);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, usercode);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, usercode);
            dbProvider.AddInParameter(command, helper.Ofercodi, DbType.Int32, ofercodi);
            dbProvider.AddInParameter(command, helper.Ofercodi, DbType.Int32, ofercodi);
            dbProvider.AddInParameter(command, helper.Oferfuente, DbType.String, oferfuente);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateList(dr));
                }
            }

            return entitys;
        }

        public List<SmaOfertaDTO> ListInterna(int ofertipo, DateTime oferfechaInicio, DateTime oferfechaFin, int usercode, string ofercodi, string oferestado, int emprcodi, string urscodi, string oferfuente)
        {
            List<SmaOfertaDTO> entitys = new List<SmaOfertaDTO>();

            string sql = string.Format(helper.SqlListInterna, oferestado, ofertipo
                , oferfechaInicio.ToString(ConstantesBase.FormatoFecha)
                , oferfechaFin.ToString(ConstantesBase.FormatoFecha)
                , emprcodi, urscodi, usercode, ofercodi, oferfuente);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateInterna(dr));
                }
            }

            return entitys;
        }

        public List<SmaOfertaDTO> ListOfertasxDia(int ofertipo, DateTime oferfechaInicio, DateTime oferfechaFin, int usercode, int emprcodi, string urscodi, string oferfuente)
        {
            List<SmaOfertaDTO> entitys = new List<SmaOfertaDTO>();

            string sql = string.Format(helper.SqlListOfertas, ofertipo
                , oferfechaInicio.ToString(ConstantesBase.FormatoFecha)
                , oferfechaFin.ToString(ConstantesBase.FormatoFecha)
                , emprcodi, urscodi, usercode, oferfuente);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateOfertaDia(dr));
                }
            }

            return entitys;
        }

        public List<SmaOfertaSimetricaHorarioDTO> ListSmaOfertaSimetricaHorario()
        {
            List<SmaOfertaSimetricaHorarioDTO> registros = new List<SmaOfertaSimetricaHorarioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEntidadOfertaSimetricaHorario);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    registros.Add(helper.CrearEntidadOfertaSimetricaHorario(dr));
                }
            }

            return registros;
        }

        public void CrearSmaOfertaSimetricaHorario(string horarioInicio, string horarioFin)
        {            
            string queryEliminar = string.Format(helper.SqlEliminarParaCrearEntidadOfertaSimetricaHorario, horarioInicio.Split(' ')[0]);
            string queryCrear = string.Format(helper.SqlCrearEntidadOfertaSimetricaHorario, horarioInicio, horarioFin);

            DbCommand commandEliminar = dbProvider.GetSqlStringCommand(queryEliminar);
            DbCommand commandCrear = dbProvider.GetSqlStringCommand(queryCrear);

            dbProvider.ExecuteNonQuery(commandEliminar);
            dbProvider.ExecuteNonQuery(commandCrear);
        }

        public void RevertirEstadoSmaOfertaSimetricaHorario(string id, int estado)
        {
            string query = string.Format(helper.SqlRevertirEstadoEntidadOfertaSimetricaHorario, estado, id);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public bool ExisteVigenteSmaOfertaSimetricaHorario()
        {
            List<SmaOfertaSimetricaHorarioDTO> registros = new List<SmaOfertaSimetricaHorarioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerActivosEntidadOfertaSimetricaHorario);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    registros.Add(helper.CrearEntidadOfertaSimetricaHorario(dr));
                }
            }

            return registros.Count > 0;
        }

        public void ResetearOfertaDefecto(DateTime fechaIniMes, DateTime fechaFinMes)
        {
            string query = string.Format(helper.SqlResetearOfertaDefecto, fechaIniMes.ToString(ConstantesBase.FormatoFecha), fechaFinMes.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}
