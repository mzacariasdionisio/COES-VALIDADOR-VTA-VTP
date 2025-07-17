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
    /// Clase de acceso a datos de la tabla ME_CAMBIOENVIO
    /// </summary>
    public class MeCambioenvioRepository : RepositoryBase, IMeCambioenvioRepository
    {
        public MeCambioenvioRepository(string strConn)
            : base(strConn)
        {
        }

        MeCambioenvioHelper helper = new MeCambioenvioHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        //inicio agregado
        public void Save(MeCambioenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Camenviocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Tipoptomedicodi, DbType.Int32, entity.Tipoptomedicodi);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);

            dbProvider.AddInParameter(command, helper.Cambenvfecha, DbType.DateTime, entity.Cambenvfecha);
            dbProvider.AddInParameter(command, helper.Cambenvdatos, DbType.String, entity.Cambenvdatos);
            dbProvider.AddInParameter(command, helper.Cambenvcolvar, DbType.String, entity.Cambenvcolvar);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);

            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Hojacodi, DbType.Int32, entity.Hojacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public int SaveTransaccional(MeCambioenvioDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Camenviocodi, DbType.Int32, entity.Camenviocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Tipoptomedicodi, DbType.Int32, entity.Tipoptomedicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Enviocodi, DbType.Int32, entity.Enviocodi));

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cambenvfecha, DbType.DateTime, entity.Cambenvfecha));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cambenvdatos, DbType.String, entity.Cambenvdatos));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cambenvcolvar, DbType.String, entity.Cambenvcolvar));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Lastuser, DbType.String, entity.Lastuser));

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Lastdate, DbType.DateTime, entity.Lastdate));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Hojacodi, DbType.Int32, entity.Hojacodi));

                dbCommand.ExecuteNonQuery();

                return entity.Camenviocodi;
            }
        }

        public void Update(MeCambioenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Cambenvfecha, DbType.DateTime, entity.Cambenvfecha);
            dbProvider.AddInParameter(command, helper.Cambenvdatos, DbType.String, entity.Cambenvdatos);
            dbProvider.AddInParameter(command, helper.Cambenvcolvar, DbType.String, entity.Cambenvcolvar);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Hojacodi, DbType.Int32, entity.Hojacodi);
            dbProvider.AddInParameter(command, helper.Camenviocodi, DbType.Int32, entity.Camenviocodi);
            dbProvider.AddInParameter(command, helper.Tipoptomedicodi, DbType.Int32, entity.Tipoptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }
        //fin agregado

        public void Delete(int enviocodi)
        {
            string query = string.Format(helper.SqlDelete, enviocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }


        public List<MeCambioenvioDTO> GetById(int enviocodi)
        {
            string sqlQuery = string.Format(helper.SqlGetById, enviocodi);
            List<MeCambioenvioDTO> entitys = new List<MeCambioenvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public List<MeCambioenvioDTO> List(int idPto, int idTipoInfo, int idFormato, DateTime fecha)
        {
            string sqlQuery = string.Format(helper.SqlList, idPto, idTipoInfo, idFormato, fecha.ToString(ConstantesBase.FormatoFecha));
            List<MeCambioenvioDTO> entitys = new List<MeCambioenvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeCambioenvioDTO> GetAllCambioEnvio(int idFormato, DateTime fechaInicio, DateTime fechaFin, int idEnvio, int idEmpresa)
        {
            string sqlQuery = string.Format(helper.SqlGetAllCambioEnvio, idFormato, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), idEnvio, idEmpresa);
            List<MeCambioenvioDTO> entitys = new List<MeCambioenvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFormatcodi = dr.GetOrdinal(this.helper.Formatcodi);
                    if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeCambioenvioDTO> GetByCriteria(string idsEmpresa, DateTime fecha, int idFormato)
        {
            string sqlQuery = string.Format(helper.SqlGetByCriteria, idsEmpresa, idFormato, fecha.ToString(ConstantesBase.FormatoFecha));
            List<MeCambioenvioDTO> entitys = new List<MeCambioenvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeCambioenvioDTO entity = helper.Create(dr);

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);
                    int iTipoinfoabrev = dr.GetOrdinal(helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);
                    int iEnviofechaperiodo = dr.GetOrdinal(helper.Enviofechaperiodo);
                    if (!dr.IsDBNull(iEnviofechaperiodo)) entity.Enviofechaperiodo = dr.GetDateTime(iEnviofechaperiodo);
                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeCambioenvioDTO> GetAllOrigenEnvio(int idFormato, DateTime fechaInicio, DateTime fechaFin, DateTime fechaPeriodo, int idEmpresa)
        {
            string sqlQuery = string.Format(helper.SqlGetAllOrigenEnvio, idFormato, idEmpresa, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), fechaPeriodo.ToString(ConstantesBase.FormatoFecha));
            List<MeCambioenvioDTO> entitys = new List<MeCambioenvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeCambioenvioDTO> ListByEnvio(string enviocodis)
        {
            string sqlQuery = string.Format(helper.SqlListByEnvio, enviocodis);
            List<MeCambioenvioDTO> entitys = new List<MeCambioenvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

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
