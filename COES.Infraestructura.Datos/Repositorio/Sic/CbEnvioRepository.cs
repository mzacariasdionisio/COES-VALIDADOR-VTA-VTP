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
    /// Clase de acceso a datos de la tabla CB_ENVIO
    /// </summary>
    public class CbEnvioRepository : RepositoryBase, ICbEnvioRepository
    {
        private string strConexion;
        public CbEnvioRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        readonly CbEnvioHelper helper = new CbEnvioHelper();

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

        public int Save(CbEnvioDTO entity, IDbConnection conn, DbTransaction tran)
        {
            //cuando sea autoguardado asignarle un codigo negativo
            string sqlId = entity.Cbenvtipoenvio == 1 ? helper.SqlGetMaxIdAutoguardado : helper.SqlGetMaxId;

            DbCommand command = dbProvider.GetSqlStringCommand(sqlId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecsolicitud, DbType.DateTime, entity.Cbenvfecsolicitud));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvususolicitud, DbType.String, entity.Cbenvususolicitud));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecaprobacion, DbType.DateTime, entity.Cbenvfecaprobacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvusuaprobacion, DbType.String, entity.Cbenvusuaprobacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvestado, DbType.String, entity.Cbenvestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvplazo, DbType.String, entity.Cbenvplazo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecpreciovigente, DbType.DateTime, entity.Cbenvfecpreciovigente));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Estcomcodi, DbType.Int32, entity.Estcomcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvmoneda, DbType.String, entity.Cbenvmoneda));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvunidad, DbType.String, entity.Cbenvunidad));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecfinrptasolicitud, DbType.DateTime, entity.Cbenvfecfinrptasolicitud));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecfinsubsanarobs, DbType.DateTime, entity.Cbenvfecfinsubsanarobs));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecmodificacion, DbType.DateTime, entity.Cbenvfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvobs, DbType.String, entity.Cbenvobs));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecampl, DbType.DateTime, entity.Cbenvfecampl));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvitem106, DbType.String, entity.Cbenvitem106));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.CbenvTipoCentral, DbType.String, entity.Cbenvtipocentral));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.CbenvfechaPeriodo, DbType.DateTime, entity.Cbenvfechaperiodo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecsistema, DbType.DateTime, entity.Cbenvfecsistema));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbftcodi, DbType.Int32, entity.Cbftcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvtipocarga, DbType.String, entity.Cbenvtipocarga));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvusucarga, DbType.String, entity.Cbenvusucarga));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvtipoenvio, DbType.Int32, entity.Cbenvtipoenvio));

            command.ExecuteNonQuery();
            return id;
        }

        public void Update(CbEnvioDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecsolicitud, DbType.DateTime, entity.Cbenvfecsolicitud));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvususolicitud, DbType.String, entity.Cbenvususolicitud));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecaprobacion, DbType.DateTime, entity.Cbenvfecaprobacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvusuaprobacion, DbType.String, entity.Cbenvusuaprobacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvestado, DbType.String, entity.Cbenvestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvplazo, DbType.String, entity.Cbenvplazo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecpreciovigente, DbType.DateTime, entity.Cbenvfecpreciovigente));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Estcomcodi, DbType.Int32, entity.Estcomcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvmoneda, DbType.String, entity.Cbenvmoneda));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvunidad, DbType.String, entity.Cbenvunidad));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecfinrptasolicitud, DbType.DateTime, entity.Cbenvfecfinrptasolicitud));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecfinsubsanarobs, DbType.DateTime, entity.Cbenvfecfinsubsanarobs));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecmodificacion, DbType.DateTime, entity.Cbenvfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvobs, DbType.String, entity.Cbenvobs));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecampl, DbType.DateTime, entity.Cbenvfecampl));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvitem106, DbType.String, entity.Cbenvitem106));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.CbenvTipoCentral, DbType.String, entity.Cbenvtipocentral));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.CbenvfechaPeriodo, DbType.DateTime, entity.Cbenvfechaperiodo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvfecsistema, DbType.DateTime, entity.Cbenvfecsistema));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbftcodi, DbType.Int32, entity.Cbftcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvtipocarga, DbType.String, entity.Cbenvtipocarga));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvusucarga, DbType.String, entity.Cbenvusucarga));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvtipoenvio, DbType.Int32, entity.Cbenvtipoenvio));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvcodi, DbType.Int32, entity.Cbenvcodi));

            command.ExecuteNonQuery();
        }

        public void Delete(int cbenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbenvcodi, DbType.Int32, cbenvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbEnvioDTO GetById(int cbenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbenvcodi, DbType.Int32, cbenvcodi);
            CbEnvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEstenvnomb = dr.GetOrdinal(helper.Estenvnomb);
                    if (!dr.IsDBNull(iEstenvnomb)) entity.Estenvnomb = dr.GetString(iEstenvnomb);

                    int iEstcomnomb = dr.GetOrdinal(helper.Estcomnomb);
                    if (!dr.IsDBNull(iEstcomnomb)) entity.Estcomnomb = dr.GetString(iEstcomnomb);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                }
            }

            return entity;
        }

        public List<CbEnvioDTO> ListXFiltroPaginado(string emprcodi, string equicodis, int estenvcodi, DateTime fechaInicio, DateTime fechaFin, int nroPaginas, int pageSize)
        {
            List<CbEnvioDTO> entitys = new List<CbEnvioDTO>();
            string sql = string.Format(helper.SqlList, emprcodi, equicodis, estenvcodi
                                , fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), nroPaginas, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CbEnvioDTO entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEstenvnomb = dr.GetOrdinal(helper.Estenvnomb);
                    if (!dr.IsDBNull(iEstenvnomb)) entity.Estenvnomb = dr.GetString(iEstenvnomb);

                    int iEstcomnomb = dr.GetOrdinal(helper.Estcomnomb);
                    if (!dr.IsDBNull(iEstcomnomb)) entity.Estcomnomb = dr.GetString(iEstcomnomb);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerTotalXFiltro(string emprcodi, string equicodis, int estenvcodi, DateTime fechaInicio, DateTime fechaFin, string tipoCombustible, string omitirTipoCarga)
        {
            string sqlTotal = string.Format(helper.SqlObtenerTotalXFiltro, emprcodi, equicodis, estenvcodi
                                , fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), tipoCombustible, omitirTipoCarga);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlTotal);
            object result = dbProvider.ExecuteScalar(command);
            int total = 0;
            if (result != null) total = Convert.ToInt32(result);
            return total;
        }

        public List<CbEnvioDTO> GetByCriteria(string emprcodi, string equicodis, DateTime fechaInicio, DateTime fechaFin)
        {
            List<CbEnvioDTO> entitys = new List<CbEnvioDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, emprcodi, equicodis, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CbEnvioDTO entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEstenvnomb = dr.GetOrdinal(helper.Estenvnomb);
                    if (!dr.IsDBNull(iEstenvnomb)) entity.Estenvnomb = dr.GetString(iEstenvnomb);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CbEnvioDTO> ListXEstado(string estenvcodis, string equicodis, int fenergcodi)
        {
            List<CbEnvioDTO> entitys = new List<CbEnvioDTO>();

            string sql = string.Format(helper.SqlListXEstado, estenvcodis, equicodis, fenergcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CbEnvioDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEstenvnomb = dr.GetOrdinal(helper.Estenvnomb);
                    if (!dr.IsDBNull(iEstenvnomb)) entity.Estenvnomb = dr.GetString(iEstenvnomb);

                    int iEstcomnomb = dr.GetOrdinal(helper.Estcomnomb);
                    if (!dr.IsDBNull(iEstcomnomb)) entity.Estcomnomb = dr.GetString(iEstcomnomb);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CbEnvioDTO> ObtenerEnvios(string emprcodi, int estenvcodi, DateTime fechaInicio, DateTime fechaFin, string tipoCombustible, int tipoEnvio)
        {
            List<CbEnvioDTO> entitys = new List<CbEnvioDTO>();
            string sql = string.Format(helper.SqlListaEnvios, emprcodi, estenvcodi, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), tipoCombustible, tipoEnvio);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CbEnvioDTO entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEstenvnomb = dr.GetOrdinal(helper.Estenvnomb);
                    if (!dr.IsDBNull(iEstenvnomb)) entity.Estenvnomb = dr.GetString(iEstenvnomb);

                    int iEstcomnomb = dr.GetOrdinal(helper.Estcomnomb);
                    if (!dr.IsDBNull(iEstcomnomb)) entity.Estcomnomb = dr.GetString(iEstcomnomb);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public CbEnvioDTO GetByTipoCombustibleYVigenciaYTipocentral(int emprcodi, int estcomcodi, DateTime fechaVigencia, string tipoCentral, int estenvcodi)
        {
            string sql = string.Format(helper.SqlGetByTipoCombustibleYVigenciaYTipocentral, emprcodi, estcomcodi, fechaVigencia.ToString(ConstantesBase.FormatoFecha), tipoCentral, estenvcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            CbEnvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEstenvnomb = dr.GetOrdinal(helper.Estenvnomb);
                    if (!dr.IsDBNull(iEstenvnomb)) entity.Estenvnomb = dr.GetString(iEstenvnomb);

                    int iEstcomnomb = dr.GetOrdinal(helper.Estcomnomb);
                    if (!dr.IsDBNull(iEstcomnomb)) entity.Estcomnomb = dr.GetString(iEstcomnomb);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                }
            }

            return entity;
        }

        public List<CbEnvioDTO> ObtenerAutoguardados(string tipoCentral, string mesDeVigencia, int idEmpresa, int estenvcodi, int enviotipo)
        {
            List<CbEnvioDTO> entitys = new List<CbEnvioDTO>();
            string sql = string.Format(helper.SqlListaAutoguardados, tipoCentral, mesDeVigencia, idEmpresa, estenvcodi, enviotipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CbEnvioDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void CambiarEstadoEnvio(string enviocodis, string estado)
        {
            int resul = -1;
            string sql = string.Format(helper.SqlCambiarEstado, estado, enviocodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            resul = dbProvider.ExecuteNonQuery(command);
        }

        public List<CbEnvioDTO> ObtenerInformacionEnviosReporteCumplimiento(string strEnvioscodi)
        {
            List<CbEnvioDTO> entitys = new List<CbEnvioDTO>();
            string sql = string.Format(helper.SqlGetDatosReporteCumplimiento, strEnvioscodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CbEnvioDTO entity = helper.Create(dr);

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

        public List<CbEnvioDTO> ObtenerEnviosXPeriodo(string emprcodi, int estenvcodi, DateTime fechaInicio, DateTime fechaFin, string tipoCombustible, int tipoEnvio)
        {
            List<CbEnvioDTO> entitys = new List<CbEnvioDTO>();
            string sql = string.Format(helper.SqlListaEnviosXPeriodo, emprcodi, estenvcodi, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), tipoCombustible, tipoEnvio);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CbEnvioDTO entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEstenvnomb = dr.GetOrdinal(helper.Estenvnomb);
                    if (!dr.IsDBNull(iEstenvnomb)) entity.Estenvnomb = dr.GetString(iEstenvnomb);

                    int iEstcomnomb = dr.GetOrdinal(helper.Estcomnomb);
                    if (!dr.IsDBNull(iEstcomnomb)) entity.Estcomnomb = dr.GetString(iEstcomnomb);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
