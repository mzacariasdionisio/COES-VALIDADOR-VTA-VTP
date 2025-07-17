using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EVE_MANTTO
    /// </summary>
    public class EveManttoRepository : RepositoryBase, IEveManttoRepository
    {
        #region INTERVENCIONES - CAMBIOS 1
        private string strConexion;
        public EveManttoRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }
        #endregion

        EveManttoHelper helper = new EveManttoHelper();

        #region INTERVENCIONES - CAMBIOS 2
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
        #endregion

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void Save(EveManttoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Manttocodi, DbType.Int32, entity.Manttocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, entity.Tipoevencodi);
            dbProvider.AddInParameter(command, helper.Compcode, DbType.Int32, entity.Compcode);
            dbProvider.AddInParameter(command, helper.Evenini, DbType.DateTime, entity.Evenini);
            dbProvider.AddInParameter(command, helper.Evenpreini, DbType.DateTime, entity.Evenpreini);
            dbProvider.AddInParameter(command, helper.Evenfin, DbType.DateTime, entity.Evenfin);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Evenprefin, DbType.DateTime, entity.Evenprefin);
            dbProvider.AddInParameter(command, helper.Evenmwindisp, DbType.Decimal, entity.Evenmwindisp);
            dbProvider.AddInParameter(command, helper.Evenpadre, DbType.Int32, entity.Evenpadre);
            dbProvider.AddInParameter(command, helper.Evenindispo, DbType.String, entity.Evenindispo);
            dbProvider.AddInParameter(command, helper.Eveninterrup, DbType.String, entity.Eveninterrup);
            dbProvider.AddInParameter(command, helper.Eventipoprog, DbType.String, entity.Eventipoprog);
            dbProvider.AddInParameter(command, helper.Evendescrip, DbType.String, entity.Evendescrip);
            dbProvider.AddInParameter(command, helper.Evenobsrv, DbType.String, entity.Evenobsrv);
            dbProvider.AddInParameter(command, helper.Evenestado, DbType.String, entity.Evenestado);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Evenrelevante, DbType.Int32, entity.Evenrelevante);
            dbProvider.AddInParameter(command, helper.Deleted, DbType.Int32, entity.Deleted);
            dbProvider.AddInParameter(command, helper.Mancodi, DbType.Int32, entity.Mancodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(EveManttoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Manttocodi, DbType.Int32, entity.Manttocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, entity.Tipoevencodi);
            dbProvider.AddInParameter(command, helper.Compcode, DbType.Int32, entity.Compcode);
            dbProvider.AddInParameter(command, helper.Evenini, DbType.DateTime, entity.Evenini);
            dbProvider.AddInParameter(command, helper.Evenpreini, DbType.DateTime, entity.Evenpreini);
            dbProvider.AddInParameter(command, helper.Evenfin, DbType.DateTime, entity.Evenfin);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Evenprefin, DbType.DateTime, entity.Evenprefin);
            dbProvider.AddInParameter(command, helper.Evenmwindisp, DbType.Decimal, entity.Evenmwindisp);
            dbProvider.AddInParameter(command, helper.Evenpadre, DbType.Int32, entity.Evenpadre);
            dbProvider.AddInParameter(command, helper.Evenindispo, DbType.String, entity.Evenindispo);
            dbProvider.AddInParameter(command, helper.Eveninterrup, DbType.String, entity.Eveninterrup);
            dbProvider.AddInParameter(command, helper.Eventipoprog, DbType.String, entity.Eventipoprog);
            dbProvider.AddInParameter(command, helper.Evendescrip, DbType.String, entity.Evendescrip);
            dbProvider.AddInParameter(command, helper.Evenobsrv, DbType.String, entity.Evenobsrv);
            dbProvider.AddInParameter(command, helper.Evenestado, DbType.String, entity.Evenestado);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Evenrelevante, DbType.Int32, entity.Evenrelevante);
            dbProvider.AddInParameter(command, helper.Deleted, DbType.Int32, entity.Deleted);
            dbProvider.AddInParameter(command, helper.Mancodi, DbType.Int32, entity.Mancodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int manttocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Manttocodi, DbType.Int32, manttocodi);
            dbProvider.AddInParameter(command, helper.Manttocodi, DbType.Int32, manttocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveManttoDTO GetById(int manttocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Manttocodi, DbType.Int32, manttocodi);
            dbProvider.AddInParameter(command, helper.Manttocodi, DbType.Int32, manttocodi);
            EveManttoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveManttoDTO> List()
        {
            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
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

        public List<EveManttoDTO> GetByCriteria()
        {
            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
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

        public List<EveManttoDTO> BuscarMantenimientos(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
            string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto, int nroPagina, int nroFilas)
        {

            String query = String.Format(helper.SqlGetByCriteria, idsTipoMantenimiento, idsEmpresa, idsTipoEquipo,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), nroPagina,
                nroFilas, idsTipoEmpresa, indInterrupcion, idstipoMantto, indispo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            EveManttoDTO entity = new EveManttoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iCausaevenabrev = dr.GetOrdinal(helper.Causaevenabrev);
                    if (!dr.IsDBNull(iCausaevenabrev)) entity.Causaevenabrev = dr.GetString(iCausaevenabrev);

                    int iEquitension = dr.GetOrdinal(helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iTipoevenabrev = dr.GetOrdinal(helper.Tipoevenabrev);
                    if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);

                    int iTipoevendesc = dr.GetOrdinal(helper.Tipoevendesc);
                    if (!dr.IsDBNull(iTipoevendesc)) entity.Tipoevendesc = dr.GetString(iTipoevendesc);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(iTipoemprcodi);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEvenclaseabrev = dr.GetOrdinal(helper.Evenclaseabrev);
                    if (!dr.IsDBNull(iEvenclaseabrev)) entity.Evenclaseabrev = dr.GetString(iEvenclaseabrev);

                    int ifamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(ifamabrev)) entity.Famabrev = dr.GetString(ifamabrev);

                    int iOsigrupocodi = dr.GetOrdinal(helper.Osigrupocodi);
                    if (!dr.IsDBNull(iOsigrupocodi)) entity.Osigrupocodi = dr.GetString(iOsigrupocodi);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    entitys.Add(entity);
                }
            }


            return entitys;
        }

        public int ObtenerNroRegistros(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
           string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto)
        {

            String query = String.Format(helper.SqlTotalRecords, idsTipoMantenimiento, idsEmpresa, idsTipoEquipo,
                    fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),
                     idsTipoEmpresa, indInterrupcion, idstipoMantto, indispo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public List<ReporteManttoDTO> ObtenerTotalManttoEmpresa(DateTime fechaInicio, DateTime fechaFin,
            string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto)
        {
            List<ReporteManttoDTO> entitys = new List<ReporteManttoDTO>();
            string query = String.Format(helper.SqlMantEmpresas, idsEmpresa, idsTipoEmpresa, idsTipoEquipo, indInterrupcion,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), idstipoMantto);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            ReporteManttoDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                int iEmprcodi = dr.GetOrdinal("emprcodi");
                int iEmprnomb = dr.GetOrdinal("emprnomb");
                int iTotalmantto = dr.GetOrdinal("totalmantto");
                while (dr.Read())
                {
                    entity = new ReporteManttoDTO();
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EveManttoDTO> ObtenerReporteMantenimientos(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin,
            string indispo, string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto)
        {
            String query = String.Format(helper.SqlReporteManttoIndisponibilidades, idsTipoMantenimiento, idsEmpresa, idsTipoEquipo,
               fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),
               idsTipoEmpresa, indInterrupcion, idstipoMantto, indispo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<EveManttoDTO> entitys = new List<EveManttoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveManttoDTO entity = helper.Create(dr);


                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);

                    int iEvenclaseabrev = dr.GetOrdinal(helper.Evenclaseabrev);
                    if (!dr.IsDBNull(iEvenclaseabrev)) entity.Evenclaseabrev = dr.GetString(iEvenclaseabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int ifamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(ifamabrev)) entity.Famabrev = dr.GetString(ifamabrev);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iCausaevenabrev = dr.GetOrdinal(helper.Causaevenabrev);
                    if (!dr.IsDBNull(iCausaevenabrev)) entity.Causaevenabrev = dr.GetString(iCausaevenabrev);

                    int iEquitension = dr.GetOrdinal(helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iTipoevenabrev = dr.GetOrdinal(helper.Tipoevenabrev);
                    if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);

                    int iTipoevendesc = dr.GetOrdinal(helper.Tipoevendesc);
                    if (!dr.IsDBNull(iTipoevendesc)) entity.Tipoevendesc = dr.GetString(iTipoevendesc);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iOsigrupocodi = dr.GetOrdinal(helper.Osigrupocodi);
                    if (!dr.IsDBNull(iOsigrupocodi)) entity.Osigrupocodi = dr.GetString(iOsigrupocodi);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);

                    int iEventipoindisp = dr.GetOrdinal(helper.Eventipoindisp);
                    if (!dr.IsDBNull(iEventipoindisp)) entity.Eventipoindisp = dr.GetString(iEventipoindisp);

                    int iEvenpr = dr.GetOrdinal(helper.Evenpr);
                    if (!dr.IsDBNull(iEvenpr)) entity.Evenpr = Convert.ToDecimal(dr.GetValue(iEvenpr));

                    int iEvenasocproc = dr.GetOrdinal(helper.Evenasocproc);
                    if (!dr.IsDBNull(iEvenasocproc)) entity.Evenasocproc = dr.GetString(iEvenasocproc);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveManttoDTO> ObtenerManttoEquipo(string idsEquipo, int evenClaseCodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            string query = string.Format(helper.SqlGetManttoEquipo, idsEquipo, evenClaseCodi, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            EveManttoDTO entity = new EveManttoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }


        public List<EveManttoDTO> ObtenerManttoEquipoClaseFecha(string idsEquipo, string fechaInicio, string fechaFin, int evenClase)
        {
            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            string query = string.Format(helper.SqlObtenerManttoEquipoClaseFecha, fechaInicio, fechaFin, evenClase, idsEquipo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            //EveManttoDTO entity = new EveManttoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    //entity = helper.Create(dr);
                    EveManttoDTO entity = new EveManttoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = Convert.ToDateTime(dr.GetValue(iEvenini));

                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = Convert.ToDateTime(dr.GetValue(iEvenfin));

                    int iEvenindispo = dr.GetOrdinal(helper.Evenindispo);
                    if (!dr.IsDBNull(iEvenindispo)) entity.Evenindispo = dr.GetString(iEvenindispo);

                    int iEvenmwindisp = dr.GetOrdinal(helper.Evenmwindisp);
                    if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = dr.GetDecimal(iEvenmwindisp);


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EveManttoDTO> ObtenerManttoEquipoSubcausaClaseFecha(string idsEquipo, string fechaInicio, string fechaFin, int evenClase, int subcausaCodi)
        {
            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            string query = string.Format(helper.SqlObtenerManttoEquipoSubcausaClaseFecha, fechaInicio, fechaFin, subcausaCodi, evenClase, idsEquipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveManttoDTO entity = new EveManttoDTO();

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = Convert.ToDateTime(dr.GetValue(iEvenini));

                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = Convert.ToDateTime(dr.GetValue(iEvenfin));

                    int iEvenindispo = dr.GetOrdinal(helper.Evenindispo);
                    if (!dr.IsDBNull(iEvenindispo)) entity.Evenindispo = dr.GetString(iEvenindispo);

                    int iEvenmwindisp = dr.GetOrdinal(helper.Evenmwindisp);
                    if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = dr.GetDecimal(iEvenmwindisp);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }


        public List<EveManttoDTO> ObtenerManttoEquipoPadreClaseFecha(string idsEquipo, string fechaInicio, string fechaFin, int evenClase)
        {
            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            string query = string.Format(helper.SqlObtenerManttoEquipoPadreClaseFecha, idsEquipo, fechaInicio, fechaFin, evenClase);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveManttoDTO entity = new EveManttoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = Convert.ToDateTime(dr.GetValue(iEvenini));

                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = Convert.ToDateTime(dr.GetValue(iEvenfin));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EveManttoDTO> ObtenerMantenimientosProgramados(DateTime fechaInicio, DateTime fechaFin)
        {
            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            string query = string.Format(helper.SqlObtenerMantenimientosProgramados, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveManttoDTO entity = new EveManttoDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iEvendescrip = dr.GetOrdinal(helper.Evendescrip);
                    if (!dr.IsDBNull(iEvendescrip)) entity.Evendescrip = dr.GetString(iEvendescrip);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EveManttoDTO> ObtenerMantenimientosProgramadosMovil(DateTime fechaInicio, DateTime fechaFin, int tipo)
        {
            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            string query = string.Format(helper.SqlObtenerMantenimientosProgramadosMovil, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), tipo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveManttoDTO entity = new EveManttoDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iEvendescrip = dr.GetOrdinal(helper.Evendescrip);
                    if (!dr.IsDBNull(iEvendescrip)) entity.Evendescrip = dr.GetString(iEvendescrip);

                    int iEvenindispo = dr.GetOrdinal(helper.Evenindispo);
                    if (!dr.IsDBNull(iEvenindispo)) entity.Evenindispo = dr.GetString(iEvenindispo);

                    int iMantipcodi = dr.GetOrdinal(helper.Mantipcodi);
                    if (!dr.IsDBNull(iMantipcodi)) entity.Mantipcodi = dr.GetString(iMantipcodi);


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #region PR5
        public List<EveManttoDTO> GetIndispUniGeneracion(string idsTipoMantenimiento, string indispo, string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, DateTime fechaInicio, DateTime fechaFin)
        {
            String query = String.Format(helper.SqlGetIndispUniGeneracion, idsTipoMantenimiento, idsEmpresa, idsTipoEmpresa, idsTipoEquipo, indispo, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            EveManttoDTO entity = new EveManttoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);

                    int iEvenclaseabrev = dr.GetOrdinal("EVENCLASEABREV");
                    if (!dr.IsDBNull(iEvenclaseabrev)) entity.Evenclaseabrev = dr.GetString(iEvenclaseabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iFamabrev = dr.GetOrdinal("FAMABREV");
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquitension = dr.GetOrdinal(helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iTipoevenabrev = dr.GetOrdinal(helper.Tipoevenabrev);
                    if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);

                    int iTipoevendesc = dr.GetOrdinal(helper.Tipoevendesc);
                    if (!dr.IsDBNull(iTipoevendesc)) entity.Tipoevendesc = dr.GetString(iTipoevendesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveManttoDTO> GetByFechaIni(DateTime fechaInicial, DateTime fechaFinal, string evenclasecodi, string famcodi)
        {
            String query = String.Format(helper.SqlGetByFechaIni, fechaInicial.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha), evenclasecodi, famcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            EveManttoDTO entity = new EveManttoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = dr.GetInt32(iFamcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region INDISPONIBILIDADES

        public EveManttoDTO GetById2(int manttocodi)
        {
            String query = String.Format(helper.SqlGetById2, manttocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            EveManttoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iCausaevenabrev = dr.GetOrdinal(helper.Causaevenabrev);
                    if (!dr.IsDBNull(iCausaevenabrev)) entity.Causaevenabrev = dr.GetString(iCausaevenabrev);

                    int iEquitension = dr.GetOrdinal(helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iTipoevenabrev = dr.GetOrdinal(helper.Tipoevenabrev);
                    if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);

                    int iTipoevendesc = dr.GetOrdinal(helper.Tipoevendesc);
                    if (!dr.IsDBNull(iTipoevendesc)) entity.Tipoevendesc = dr.GetString(iTipoevendesc);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(iTipoemprcodi);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEvenclaseabrev = dr.GetOrdinal(helper.Evenclaseabrev);
                    if (!dr.IsDBNull(iEvenclaseabrev)) entity.Evenclaseabrev = dr.GetString(iEvenclaseabrev);

                    int ifamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(ifamabrev)) entity.Famabrev = dr.GetString(ifamabrev);

                    int iOsigrupocodi = dr.GetOrdinal(helper.Osigrupocodi);
                    if (!dr.IsDBNull(iOsigrupocodi)) entity.Osigrupocodi = dr.GetString(iOsigrupocodi);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                }
            }

            return entity;
        }

        #endregion

        #region SIOSEIN

        public List<EveManttoDTO> GetListaHechosRelevantes(DateTime fechaInicio, DateTime fechaFin)
        {
            var entitys = new List<EveManttoDTO>();
            var query = string.Format(helper.SqlGetListaHechosRelevantes, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                EveManttoDTO entity = new EveManttoDTO();
                while (dr.Read())
                {
                    entity = new EveManttoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = dr.GetInt16(iTipoemprcodi);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEvenmwindisp = dr.GetOrdinal(helper.Evenmwindisp);
                    if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = dr.GetDecimal(iEvenmwindisp);

                    int iEvendescrip = dr.GetOrdinal(helper.Evendescrip);
                    if (!dr.IsDBNull(iEvendescrip)) entity.Evendescrip = dr.GetString(iEvendescrip);

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion

        #region MigracionSGOCOES-GrupoB
        public List<EveManttoDTO> ListaManttosDigsilent(string evenclasecodi, DateTime fecha)
        {
            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            string query = string.Format(helper.SqlListaManttosDigsilent, evenclasecodi, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveManttoDTO entity = new EveManttoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    //int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    //if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = Convert.ToDateTime(dr.GetValue(iEvenini));

                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = Convert.ToDateTime(dr.GetValue(iEvenfin));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EveManttoDTO> ListaMantenimientos25(int evenclasecodi, string evenclasedesc, DateTime fechaini, DateTime fechafin)
        {
            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            string query = string.Format(helper.SqlListaMantenimientos25, evenclasecodi, evenclasedesc, fechaini.ToString(ConstantesBase.FormatoFechaBase), fechafin.ToString(ConstantesBase.FormatoFechaBase));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveManttoDTO entity = new EveManttoDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iEvenclaseabrev = dr.GetOrdinal(helper.Evenclaseabrev);
                    if (!dr.IsDBNull(iEvenclaseabrev)) entity.Evenclaseabrev = dr.GetString(iEvenclaseabrev);

                    int iEvendescrip = dr.GetOrdinal(helper.Evendescrip);
                    if (!dr.IsDBNull(iEvendescrip)) entity.Evendescrip = dr.GetString(iEvendescrip);

                    int iTareacodi = dr.GetOrdinal(helper.Tareacodi);
                    if (!dr.IsDBNull(iTareacodi)) entity.Tareacodi = dr.GetInt32(iTareacodi);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion

        #region INTERVENCIONES

        public void Save(EveManttoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand commandTbEveMantto = (DbCommand)conn.CreateCommand();
            commandTbEveMantto.CommandText = helper.SqlSaveConIntervencion;
            commandTbEveMantto.Transaction = tran;
            commandTbEveMantto.Connection = (DbConnection)conn;

            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Manttocodi, DbType.Int32, entity.Manttocodi));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Equicodi, DbType.Int32, entity.Equicodi));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Tipoevencodi, DbType.Int32, entity.Tipoevencodi));

            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Compcode, DbType.Int32, entity.Compcode));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenini, DbType.DateTime, entity.Evenini));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenpreini, DbType.DateTime, entity.Evenpreini));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenfin, DbType.DateTime, entity.Evenfin));

            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenprefin, DbType.DateTime, entity.Evenprefin));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenmwindisp, DbType.Double, entity.Evenmwindisp));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenpadre, DbType.Int32, -1));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenindispo, DbType.String, entity.Evenindispo));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Eveninterrup, DbType.String, entity.Eveninterrup));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Eventipoprog, DbType.String, entity.Eventipoprog));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evendescrip, DbType.String, entity.Evendescrip));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenobsrv, DbType.String, entity.Evenobsrv));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenestado, DbType.String, entity.Evenestado));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Lastuser, DbType.String, entity.Lastuser));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Lastdate, DbType.DateTime, entity.Lastdate));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenrelevante, DbType.Int32, entity.Evenrelevante));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Deleted, DbType.Int32, entity.Deleted));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Mancodi, DbType.Int32, -1));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Equimantrelev, DbType.String, entity.Equimantrelev));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Mantrelevlastuser, DbType.String, entity.Mantrelevlastuser));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Mantrelevlastdate, DbType.DateTime, entity.Mantrelevlastuser));

            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Intercodi, DbType.Int32, entity.InterCodi));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Eventipoindisp, DbType.String, entity.Eventipoindisp));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenpr, DbType.Decimal, entity.Evenpr));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenasocproc, DbType.String, entity.Evenasocproc));

            commandTbEveMantto.ExecuteNonQuery();
        }

        public int DeleteByPrograma(IDbConnection conn, DbTransaction tran, int evenclasecodi, DateTime fechaIni, DateTime fechaFin)
        {
            DbCommand commandTbEveMantto = (DbCommand)conn.CreateCommand();
            commandTbEveMantto.CommandText = helper.SqlDeleteByPrograma;
            commandTbEveMantto.Transaction = tran;
            commandTbEveMantto.Connection = (DbConnection)conn;

            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenclasecodi, DbType.Int32, evenclasecodi));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenini, DbType.DateTime, fechaIni));
            commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Evenfin, DbType.DateTime, fechaFin));

            int id = commandTbEveMantto.ExecuteNonQuery();

            return id;
        }

        public int DeleteByIntercodi(IDbConnection conn, DbTransaction tran, int interCodi)
        {
            if (interCodi > 0)
            {
                DbCommand commandTbEveMantto = (DbCommand)conn.CreateCommand();
                commandTbEveMantto.CommandText = helper.SqlDeleteByIntercodi;
                commandTbEveMantto.Transaction = tran;
                commandTbEveMantto.Connection = (DbConnection)conn;

                commandTbEveMantto.Parameters.Add(dbProvider.CreateParameter(commandTbEveMantto, helper.Intercodi, DbType.Int32, interCodi));
                return commandTbEveMantto.ExecuteNonQuery();
            }

            return 0;
        }

        #endregion

        #region SIOSEIN2

        public List<EveManttoDTO> ObtenerManttoPorEquipoClaseFamilia(string evenclasecodi, string famcodi, DateTime eveniniInicio, DateTime eveiniFin)
        {
            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            string query = string.Format(helper.SqlObtenerMatenimientoPorEquipoClaseFamilia, evenclasecodi, famcodi, eveniniInicio.ToString(ConstantesBase.FormatoFecha), eveiniFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            EveManttoDTO entity = new EveManttoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);
                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));
                    int iCentral = dr.GetOrdinal(helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGrupointegrante = dr.GetOrdinal(this.helper.Grupointegrante);
                    if (!dr.IsDBNull(iGrupointegrante)) entity.Grupointegrante = dr.GetString(iGrupointegrante);
                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EveManttoDTO> ObtenerManttoEjecutadoProgramado(string evenclasecodi, string evenindispo, string tareacodi, string famcodi, string emprecodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            string query = string.Format(helper.SqlObtenerMatenimientoEjecutadoProgramado, evenclasecodi, evenindispo, tareacodi, famcodi, fechaInicio.ToString(ConstantesBase.FormatoFechaExtendido), fechaFin.ToString(ConstantesBase.FormatoFechaExtendido), emprecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveManttoDTO entity = new EveManttoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iEvendescrip = dr.GetOrdinal(helper.Evendescrip);
                    if (!dr.IsDBNull(iEvendescrip)) entity.Evendescrip = dr.GetString(iEvendescrip);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

        #region Numerales Datos Base
        public List<EveManttoDTO> ListaNumerales_DatosBase_5_6_1(string fechaIni, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_6_1, fechaIni, fechaFin);

            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveManttoDTO entity = new EveManttoDTO();

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iOsigrupocodi = dr.GetOrdinal(helper.Osigrupocodi);
                    if (!dr.IsDBNull(iOsigrupocodi)) entity.Osigrupocodi = dr.GetString(iOsigrupocodi);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iEvenmwindisp = dr.GetOrdinal(helper.Evenmwindisp);
                    if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = dr.GetDecimal(iEvenmwindisp);

                    int IGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(IGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(IGrupocodi));

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);

                    int IEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(IEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(IEquipadre));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int eOsigrupocodi = dr.GetOrdinal(helper.Osigrupocodi);
                    if (!dr.IsDBNull(eOsigrupocodi)) entity.Osigrupocodi = dr.GetString(eOsigrupocodi);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EveManttoDTO> ListaNumerales_DatosBase_5_6_7(string fechaIni, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_6_7, fechaIni, fechaFin);

            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveManttoDTO entity = new EveManttoDTO();

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iOsicodi = dr.GetOrdinal(helper.Osicodi);
                    if (!dr.IsDBNull(iOsicodi)) entity.Osicodi = dr.GetString(iOsicodi);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iEvenmwindisp = dr.GetOrdinal(helper.Evenmwindisp);
                    if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = dr.GetDecimal(iEvenmwindisp);

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);

                    int IGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(IGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(IGrupocodi));

                    int IEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(IEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(IEquipadre));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EveManttoDTO> ListaNumerales_DatosBase_5_6_8(string fechaIni, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_6_8, fechaIni, fechaFin);

            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveManttoDTO entity = new EveManttoDTO();

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iOsicodi = dr.GetOrdinal(helper.Osicodi);
                    if (!dr.IsDBNull(iOsicodi)) entity.Osicodi = dr.GetString(iOsicodi);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iEvenmwindisp = dr.GetOrdinal(helper.Evenmwindisp);
                    if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = dr.GetDecimal(iEvenmwindisp);

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);

                    int IGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(IGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(IGrupocodi));

                    int IEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(IEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(IEquipadre));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EveManttoDTO> ListaNumerales_DatosBase_5_6_9(string fechaIni, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_6_9, fechaIni, fechaFin);

            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveManttoDTO entity = new EveManttoDTO();

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iOsicodi = dr.GetOrdinal(helper.Osicodi);
                    if (!dr.IsDBNull(iOsicodi)) entity.Osicodi = dr.GetString(iOsicodi);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iEvenmwindisp = dr.GetOrdinal(helper.Evenmwindisp);
                    if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = dr.GetDecimal(iEvenmwindisp);

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);

                    int IGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(IGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(IGrupocodi));

                    int IEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(IEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(IEquipadre));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EveManttoDTO> ListaNumerales_DatosBase_5_6_10(string fechaIni, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_6_10, fechaIni, fechaFin);

            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveManttoDTO entity = new EveManttoDTO();

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iOsigrupocodi = dr.GetOrdinal(helper.Osigrupocodi);
                    if (!dr.IsDBNull(iOsigrupocodi)) entity.Osigrupocodi = dr.GetString(iOsigrupocodi);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iEvenmwindisp = dr.GetOrdinal(helper.Evenmwindisp);
                    if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = dr.GetDecimal(iEvenmwindisp);

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);

                    int IGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(IGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(IGrupocodi));

                    int IEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(IEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(IEquipadre));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

    }
}
