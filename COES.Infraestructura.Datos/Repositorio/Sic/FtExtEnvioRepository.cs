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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO
    /// </summary>
    public class FtExtEnvioRepository : RepositoryBase, IFtExtEnvioRepository
    {
        private string strConexion;
        public FtExtEnvioRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        FtExtEnvioHelper helper = new FtExtEnvioHelper();

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

        public int Save(FtExtEnvioDTO entity, IDbConnection conn, DbTransaction tran)
        {
            //cuando sea autoguardado asignarle un codigo negativo
            string sqlId = entity.Ftenvtipoenvio == 1 ? helper.SqlGetMaxId : helper.SqlGetMaxIdAutoguardado;
            DbCommand command = dbProvider.GetSqlStringCommand(sqlId);

            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            entity.Ftenvcodi = id;

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvcodi, DbType.Int32, entity.Ftenvcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftetcodi, DbType.Int32, entity.Ftetcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftprycodi, DbType.Int32, entity.Ftprycodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecsolicitud, DbType.DateTime, entity.Ftenvfecsolicitud));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvususolicitud, DbType.String, entity.Ftenvususolicitud));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecaprobacion, DbType.DateTime, entity.Ftenvfecaprobacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvusuaprobacion, DbType.String, entity.Ftenvusuaprobacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecfinrptasolicitud, DbType.DateTime, entity.Ftenvfecfinrptasolicitud));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecfinsubsanarobs, DbType.DateTime, entity.Ftenvfecfinsubsanarobs));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvtipoenvio, DbType.Int32, entity.Ftenvtipoenvio));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftevcodi, DbType.Int32, entity.Ftevcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvusumodificacion, DbType.String, entity.Ftenvusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecmodificacion, DbType.DateTime, entity.Ftenvfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvtipoformato, DbType.Int32, entity.Ftenvtipoformato));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvobs, DbType.String, entity.Ftenvobs));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecvigencia, DbType.DateTime, entity.Ftenvfecvigencia));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecsistema, DbType.DateTime, entity.Ftenvfecsistema));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecampliacion, DbType.DateTime, entity.Ftenvfecampliacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecobservacion, DbType.DateTime, entity.Ftenvfecobservacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvenlacesint, DbType.String, entity.Ftenvenlacesint));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvenlacecarta, DbType.String, entity.Ftenvenlacecarta));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvenlaceotro, DbType.String, entity.Ftenvenlaceotro));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecinirev1, DbType.DateTime, entity.Ftenvfecinirev1));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecinirev2, DbType.DateTime, entity.Ftenvfecinirev2));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvtipocasoesp, DbType.Int32, entity.Ftenvtipocasoesp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvflaghabeq, DbType.String, entity.Ftenvflaghabeq));

            command.ExecuteNonQuery();
            return entity.Ftenvcodi;
        }

        public void Update(FtExtEnvioDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftetcodi, DbType.Int32, entity.Ftetcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftprycodi, DbType.Int32, entity.Ftprycodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecsolicitud, DbType.DateTime, entity.Ftenvfecsolicitud));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvususolicitud, DbType.String, entity.Ftenvususolicitud));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecaprobacion, DbType.DateTime, entity.Ftenvfecaprobacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvusuaprobacion, DbType.String, entity.Ftenvusuaprobacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecfinrptasolicitud, DbType.DateTime, entity.Ftenvfecfinrptasolicitud));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecfinsubsanarobs, DbType.DateTime, entity.Ftenvfecfinsubsanarobs));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvtipoenvio, DbType.Int32, entity.Ftenvtipoenvio));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftevcodi, DbType.Int32, entity.Ftevcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvusumodificacion, DbType.String, entity.Ftenvusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecmodificacion, DbType.DateTime, entity.Ftenvfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvtipoformato, DbType.Int32, entity.Ftenvtipoformato));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvobs, DbType.String, entity.Ftenvobs));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecvigencia, DbType.DateTime, entity.Ftenvfecvigencia));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecsistema, DbType.DateTime, entity.Ftenvfecsistema));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecampliacion, DbType.DateTime, entity.Ftenvfecampliacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecobservacion, DbType.DateTime, entity.Ftenvfecobservacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvenlacesint, DbType.String, entity.Ftenvenlacesint));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvenlacecarta, DbType.String, entity.Ftenvenlacecarta));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvenlaceotro, DbType.String, entity.Ftenvenlaceotro));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecinirev1, DbType.DateTime, entity.Ftenvfecinirev1));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvfecinirev2, DbType.DateTime, entity.Ftenvfecinirev2));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvtipocasoesp, DbType.Int32, entity.Ftenvtipocasoesp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvflaghabeq, DbType.String, entity.Ftenvflaghabeq));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftenvcodi, DbType.Int32, entity.Ftenvcodi));

            command.ExecuteNonQuery();
        }

        public void Delete(int ftenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftenvcodi, DbType.Int32, ftenvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEnvioDTO GetById(int ftenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftenvcodi, DbType.Int32, ftenvcodi);
            FtExtEnvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFtetnombre = dr.GetOrdinal(helper.Ftetnombre);
                    if (!dr.IsDBNull(iFtetnombre)) entity.Ftetnombre = dr.GetString(iFtetnombre);

                    int iFtprynombre = dr.GetOrdinal(helper.Ftprynombre);
                    if (!dr.IsDBNull(iFtprynombre)) entity.Ftprynombre = dr.GetString(iFtprynombre);
                }
            }

            return entity;
        }

        public List<FtExtEnvioDTO> List()
        {
            List<FtExtEnvioDTO> entitys = new List<FtExtEnvioDTO>();
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

        public List<FtExtEnvioDTO> GetByCriteria()
        {
            List<FtExtEnvioDTO> entitys = new List<FtExtEnvioDTO>();
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

        public int ObtenerTotalXFiltro(string emprcodi, int estenvcodi, DateTime fechaInicio, DateTime fechaFin, int ftetcodi)
        {
            string sqlTotal = string.Format(helper.SqlObtenerTotalXFiltro, emprcodi, estenvcodi
                                , fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), ftetcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlTotal);
            object result = dbProvider.ExecuteScalar(command);
            int total = 0;
            if (result != null) total = Convert.ToInt32(result);
            return total;
        }

        public List<FtExtEnvioDTO> ObtenerEnviosEtapas(string emprcodi, int estenvcodi, DateTime fechaInicio, DateTime fechaFin, string ftetcodi)
        {
            List<FtExtEnvioDTO> entitys = new List<FtExtEnvioDTO>();
            string sql = string.Format(helper.SqlListaEnvios, emprcodi, estenvcodi, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), ftetcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioDTO entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFtetnombre = dr.GetOrdinal(helper.Ftetnombre);
                    if (!dr.IsDBNull(iFtetnombre)) entity.Ftetnombre = dr.GetString(iFtetnombre);

                    int iFtprynombre = dr.GetOrdinal(helper.Ftprynombre);
                    if (!dr.IsDBNull(iFtprynombre)) entity.Ftprynombre = dr.GetString(iFtprynombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<FtExtEnvioDTO> ObtenerEnviosPorEstado(string emprcodi, int estenvcodi, int ftetcodi)
        {
            List<FtExtEnvioDTO> entitys = new List<FtExtEnvioDTO>();
            string sql = string.Format(helper.SqlListaEnviosPorEstado, emprcodi, estenvcodi, ftetcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioDTO entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFtetnombre = dr.GetOrdinal(helper.Ftetnombre);
                    if (!dr.IsDBNull(iFtetnombre)) entity.Ftetnombre = dr.GetString(iFtetnombre);

                    int iFtprynombre = dr.GetOrdinal(helper.Ftprynombre);
                    if (!dr.IsDBNull(iFtprynombre)) entity.Ftprynombre = dr.GetString(iFtprynombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioDTO> ListarEnvioAutoguardado(int emprcodi, int estenvcodi, int ftetcodi, int ftenvtipoenvio)
        {
            List<FtExtEnvioDTO> entitys = new List<FtExtEnvioDTO>();
            string sql = string.Format(helper.SqlListarEnvioAutoguardado, emprcodi, estenvcodi, ftetcodi, ftenvtipoenvio);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioDTO> ListarEnviosYEqNoSeleccionable(string emprcodis, int ftetcodi)
        {
            List<FtExtEnvioDTO> entitys = new List<FtExtEnvioDTO>();
            string sql = string.Format(helper.SqlListarEnviosYEqNoSeleccionable, emprcodis, ftetcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioDTO entity = helper.Create(dr);

                    int iFteeqcodi = dr.GetOrdinal(helper.Fteeqcodi);
                    if (!dr.IsDBNull(iFteeqcodi)) entity.Fteeqcodi = Convert.ToInt32(dr.GetValue(iFteeqcodi));
                    int iFtevercodi = dr.GetOrdinal(helper.Ftevercodi);
                    if (!dr.IsDBNull(iFtevercodi)) entity.Ftevercodi = Convert.ToInt32(dr.GetValue(iFtevercodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iNombreelemento = dr.GetOrdinal(helper.Nombreelemento);
                    if (!dr.IsDBNull(iNombreelemento)) entity.Nombreelemento = dr.GetString(iNombreelemento);

                    int iAbrevelemento = dr.GetOrdinal(helper.Abrevelemento);
                    if (!dr.IsDBNull(iAbrevelemento)) entity.Abrevelemento = dr.GetString(iAbrevelemento);

                    int iEstadoelemento = dr.GetOrdinal(helper.Estadoelemento);
                    if (!dr.IsDBNull(iEstadoelemento)) entity.Estadoelemento = dr.GetString(iEstadoelemento);

                    int iTipoelemento = dr.GetOrdinal(helper.Tipoelemento);
                    if (!dr.IsDBNull(iTipoelemento)) entity.Tipoelemento = dr.GetString(iTipoelemento);

                    int iIdelemento = dr.GetOrdinal(helper.Idelemento);
                    if (!dr.IsDBNull(iIdelemento)) entity.Idelemento = Convert.ToInt32(dr.GetValue(iIdelemento));

                    int iAreaelemento = dr.GetOrdinal(helper.Areaelemento);
                    if (!dr.IsDBNull(iAreaelemento)) entity.Areaelemento = dr.GetString(iAreaelemento);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioDTO> ListarEnviosYEqAprobado(string emprcodis, int ftetcodi)
        {
            List<FtExtEnvioDTO> entitys = new List<FtExtEnvioDTO>();
            string sql = string.Format(helper.SqlListarEnviosYEqAprobado, emprcodis, ftetcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioDTO entity = helper.Create(dr);

                    int iFteeqcodi = dr.GetOrdinal(helper.Fteeqcodi);
                    if (!dr.IsDBNull(iFteeqcodi)) entity.Fteeqcodi = Convert.ToInt32(dr.GetValue(iFteeqcodi));
                    int iFtevercodi = dr.GetOrdinal(helper.Ftevercodi);
                    if (!dr.IsDBNull(iFtevercodi)) entity.Ftevercodi = Convert.ToInt32(dr.GetValue(iFtevercodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iNombreelemento = dr.GetOrdinal(helper.Nombreelemento);
                    if (!dr.IsDBNull(iNombreelemento)) entity.Nombreelemento = dr.GetString(iNombreelemento);

                    int iAbrevelemento = dr.GetOrdinal(helper.Abrevelemento);
                    if (!dr.IsDBNull(iAbrevelemento)) entity.Abrevelemento = dr.GetString(iAbrevelemento);

                    int iEstadoelemento = dr.GetOrdinal(helper.Estadoelemento);
                    if (!dr.IsDBNull(iEstadoelemento)) entity.Estadoelemento = dr.GetString(iEstadoelemento);

                    int iTipoelemento = dr.GetOrdinal(helper.Tipoelemento);
                    if (!dr.IsDBNull(iTipoelemento)) entity.Tipoelemento = dr.GetString(iTipoelemento);

                    int iIdelemento = dr.GetOrdinal(helper.Idelemento);
                    if (!dr.IsDBNull(iIdelemento)) entity.Idelemento = Convert.ToInt32(dr.GetValue(iIdelemento));

                    int iAreaelemento = dr.GetOrdinal(helper.Areaelemento);
                    if (!dr.IsDBNull(iAreaelemento)) entity.Areaelemento = dr.GetString(iAreaelemento);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioDTO> ObtenerEnviosEtapasParaAreas(string emprcodi, string ftetcodi, DateTime fechaInicio, DateTime fechaFin, string envarestado, string faremcodis)
        {
            List<FtExtEnvioDTO> entitys = new List<FtExtEnvioDTO>();
            string sql = string.Format(helper.SqlListaEnviosAreas, emprcodi, ftetcodi, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), envarestado, faremcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioDTO entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFtevercodi = dr.GetOrdinal(helper.Ftevercodi);
                    if (!dr.IsDBNull(iFtevercodi)) entity.Ftevercodi = Convert.ToInt32(dr.GetValue(iFtevercodi));

                    int iEstenvcodiversion = dr.GetOrdinal(helper.Estenvcodiversion);
                    if (!dr.IsDBNull(iEstenvcodiversion)) entity.Estenvcodiversion = Convert.ToInt32(dr.GetValue(iEstenvcodiversion));

                    int iFtetnombre = dr.GetOrdinal(helper.Ftetnombre);
                    if (!dr.IsDBNull(iFtetnombre)) entity.Ftetnombre = dr.GetString(iFtetnombre);

                    int iFtprynombre = dr.GetOrdinal(helper.Ftprynombre);
                    if (!dr.IsDBNull(iFtprynombre)) entity.Ftprynombre = dr.GetString(iFtprynombre);

                    int iEnvarestado = dr.GetOrdinal(helper.Envarestado);
                    if (!dr.IsDBNull(iEnvarestado)) entity.Envarestado = dr.GetString(iEnvarestado);

                    int iFaremnombre = dr.GetOrdinal(helper.Faremnombre);
                    if (!dr.IsDBNull(iFaremnombre)) entity.Faremnombre = dr.GetString(iFaremnombre);

                    int iFaremcodi = dr.GetOrdinal(helper.Faremcodi);
                    if (!dr.IsDBNull(iFaremcodi)) entity.Faremcodi = Convert.ToInt32(dr.GetValue(iFaremcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioDTO> ListarEnviosDerivadosPorCarpetaYEstado(int estenvcodi, int ftetcodi, string estadoRevision)
        {
            List<FtExtEnvioDTO> entitys = new List<FtExtEnvioDTO>();
            string sql = string.Format(helper.SqlListarEnviosDerivadosPorCarpetaYEstado, estenvcodi, ftetcodi, estadoRevision);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioDTO entity = helper.Create(dr);

                    int iEnvarfecmaxrpta = dr.GetOrdinal(helper.Envarfecmaxrpta);
                    if (!dr.IsDBNull(iEnvarfecmaxrpta)) entity.Envarfecmaxrpta = dr.GetDateTime(iEnvarfecmaxrpta);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<FtExtEnvioDTO> ListarRelacionEnvioVersionArea(int estenvcodi, int ftetcodi, int ftevertipo)
        {
            List<FtExtEnvioDTO> entitys = new List<FtExtEnvioDTO>();
            string sql = string.Format(helper.SqlListarRelacionEnvioVersionArea, estenvcodi, ftetcodi, ftevertipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioDTO entity = helper.Create(dr);

                    int iFtevercodi = dr.GetOrdinal(helper.Ftevercodi);
                    if (!dr.IsDBNull(iFtevercodi)) entity.Ftevercodi = Convert.ToInt32(dr.GetValue(iFtevercodi));

                    int iFaremcodi = dr.GetOrdinal(helper.Faremcodi);
                    if (!dr.IsDBNull(iFaremcodi)) entity.Faremcodi = Convert.ToInt32(dr.GetValue(iFaremcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        

    }
}
