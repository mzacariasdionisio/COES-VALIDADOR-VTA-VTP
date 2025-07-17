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
    /// Clase de acceso a datos de la tabla CB_ENVIO_CENTRAL
    /// </summary>
    public class CbEnvioCentralRepository : RepositoryBase, ICbEnvioCentralRepository
    {
        public CbEnvioCentralRepository(string strConn) : base(strConn)
        {
        }

        CbEnvioCentralHelper helper = new CbEnvioCentralHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(CbEnvioCentralDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbcentcodi, DbType.Int32, entity.Cbcentcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command,  helper.Cbcentestado, DbType.String, entity.Cbcentestado));
            command.Parameters.Add(dbProvider.CreateParameter(command,  helper.Cbvercodi, DbType.Int32, entity.Cbvercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command,  helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));

            command.ExecuteNonQuery();
            return entity.Cbcentcodi;
        }

        public void Update(CbEnvioCentralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbcentcodi, DbType.Int32, entity.Cbcentcodi);
            dbProvider.AddInParameter(command, helper.Cbcentestado, DbType.String, entity.Cbcentestado);
            dbProvider.AddInParameter(command, helper.Cbvercodi, DbType.Int32, entity.Cbvercodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbcentcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbcentcodi, DbType.Int32, cbcentcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbEnvioCentralDTO GetById(int cbcentcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbcentcodi, DbType.Int32, cbcentcodi);
            CbEnvioCentralDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbEnvioCentralDTO> List()
        {
            List<CbEnvioCentralDTO> entitys = new List<CbEnvioCentralDTO>();
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

        public List<CbEnvioCentralDTO> GetByCriteria(int cbvercodi)
        {
            List<CbEnvioCentralDTO> entitys = new List<CbEnvioCentralDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, cbvercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CbEnvioCentralDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CbEnvioCentralDTO> ObtenerCentrales(int cbenvcodi)
        {
            List<CbEnvioCentralDTO> entitys = new List<CbEnvioCentralDTO>();            
            string query = string.Format(helper.SqlGetCentralesPorEnvio, cbenvcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CbEnvioCentralDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CbEnvioCentralDTO> GetCentralesConInfoEnviada(int mes, int anio)
        {
            List<CbEnvioCentralDTO> entitys = new List<CbEnvioCentralDTO>();
            string query = string.Format(helper.SqlGetCentralesConInfoEnviada, mes, anio);
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

        public List<CbEnvioCentralDTO> ListarCentralUltimoEnvioXMes(int mes, int anio)
        {
            List<CbEnvioCentralDTO> entitys = new List<CbEnvioCentralDTO>();
            string query = string.Format(helper.SqlListarCentralUltimoEnvioXMes, mes, anio);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iCbenvfechaPeriodo = dr.GetOrdinal(helper.CbenvfechaPeriodo);
                    if (!dr.IsDBNull(iCbenvfechaPeriodo)) entity.Cbenvfechaperiodo = dr.GetDateTime(iCbenvfechaPeriodo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CbEnvioCentralDTO> ListarCentralUltimoEnvioXDato(string equicodis, DateTime fechaPeriodo, int ccombcodi, string valor)
        {
            List<CbEnvioCentralDTO> entitys = new List<CbEnvioCentralDTO>();
            string query = string.Format(helper.SqlListarCentralUltimoEnvioXDato, fechaPeriodo.ToString(ConstantesBase.FormatoFecha), ccombcodi, valor, equicodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iCbenvfechaPeriodo = dr.GetOrdinal(helper.CbenvfechaPeriodo);
                    if (!dr.IsDBNull(iCbenvfechaPeriodo)) entity.Cbenvfechaperiodo = dr.GetDateTime(iCbenvfechaPeriodo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CbEnvioCentralDTO> ListarCentralNuevaUltimoEnvioXDato(string equicodis, DateTime fechaPeriodo, int ccombcodi)
        {
            List<CbEnvioCentralDTO> entitys = new List<CbEnvioCentralDTO>();
            string query = string.Format(helper.SqlListarCentralNuevaUltimoEnvioXDato, fechaPeriodo.ToString(ConstantesBase.FormatoFecha), ccombcodi, equicodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iCbenvfechaPeriodo = dr.GetOrdinal(helper.CbenvfechaPeriodo);
                    if (!dr.IsDBNull(iCbenvfechaPeriodo)) entity.Cbenvfechaperiodo = dr.GetDateTime(iCbenvfechaPeriodo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CbEnvioCentralDTO> GetByEstadoYVersion(string estado, string versioncodis)
        {
            List<CbEnvioCentralDTO> entitys = new List<CbEnvioCentralDTO>();
            string query = string.Format(helper.SqlGetByEstadoYVersion, estado, versioncodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    //entitys.Add(helper.Create(dr));
                    var entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CbEnvioCentralDTO> ListarCentralUltimoEnvioXDia(DateTime fechaPeriodo, string centrales)
        {
            List<CbEnvioCentralDTO> entitys = new List<CbEnvioCentralDTO>();
            string query = string.Format(helper.SqlListarCentralUltimoEnvioXDia, centrales, fechaPeriodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iCbenvfechaPeriodo = dr.GetOrdinal(helper.CbenvfechaPeriodo);
                    if (!dr.IsDBNull(iCbenvfechaPeriodo)) entity.Cbenvfechaperiodo = dr.GetDateTime(iCbenvfechaPeriodo);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CbEnvioCentralDTO> ListarCentralXRangoMes(DateTime fechaIniPeriodo, DateTime fechaFinPeriodo, string centrales)
        {
            List<CbEnvioCentralDTO> entitys = new List<CbEnvioCentralDTO>();
            string query = string.Format(helper.SqlListarCentralPorRangoMes, centrales, fechaIniPeriodo.ToString(ConstantesBase.FormatoFecha), fechaFinPeriodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iCbenvfechaPeriodo = dr.GetOrdinal(helper.CbenvfechaPeriodo);
                    if (!dr.IsDBNull(iCbenvfechaPeriodo)) entity.Cbenvfechaperiodo = dr.GetDateTime(iCbenvfechaPeriodo);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CbEnvioCentralDTO> ListarPorIds(string cbcentcodis)
        {
            List<CbEnvioCentralDTO> entitys = new List<CbEnvioCentralDTO>();
            string query = string.Format(helper.SqlListarPorIds, cbcentcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
