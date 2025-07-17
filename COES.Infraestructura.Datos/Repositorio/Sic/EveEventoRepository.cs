using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EVE_EVENTO
    /// </summary>
    public class EveEventoRepository: RepositoryBase, IEveEventoRepository
    {
        private string strConexion;
        public EveEventoRepository(string strConn): base(strConn)
        {
            strConexion = strConn;
        }

        EveEventoHelper helper = new EveEventoHelper();

        public int Save(EveEventoDTO entity)
        {
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            //object result = dbProvider.ExecuteScalar(command);
            //int id = 1;
            //if (result != null)id = Convert.ToInt32(result);

            int id = entity.Evencodi;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Evencomentarios, DbType.String, entity.Evencomentarios);
            dbProvider.AddInParameter(command, helper.Evenperturbacion, DbType.String, entity.Evenperturbacion);
            dbProvider.AddInParameter(command, helper.Twitterenviado, DbType.String, entity.Twitterenviado);
            dbProvider.AddInParameter(command, helper.Emprcodirespon, DbType.Int32, entity.Emprcodirespon);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, entity.Tipoevencodi);
            dbProvider.AddInParameter(command, helper.Evenini, DbType.DateTime, entity.Evenini);
            dbProvider.AddInParameter(command, helper.Evenmwindisp, DbType.Decimal, entity.Evenmwindisp);
            dbProvider.AddInParameter(command, helper.Evenfin, DbType.DateTime, entity.Evenfin);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Evenasunto, DbType.String, entity.Evenasunto);
            dbProvider.AddInParameter(command, helper.Evenpadre, DbType.Int32, entity.Evenpadre);
            dbProvider.AddInParameter(command, helper.Eveninterrup, DbType.String, entity.Eveninterrup);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Evenpreini, DbType.DateTime, entity.Evenpreini);
            dbProvider.AddInParameter(command, helper.Evenpostfin, DbType.DateTime, entity.Evenpostfin);
            dbProvider.AddInParameter(command, helper.Evendesc, DbType.String, entity.Evendesc);
            dbProvider.AddInParameter(command, helper.Eventension, DbType.Decimal, entity.Eventension);
            dbProvider.AddInParameter(command, helper.Evenaopera, DbType.String, entity.Evenaopera);
            dbProvider.AddInParameter(command, helper.Evenpreliminar, DbType.String, entity.Evenpreliminar);
            dbProvider.AddInParameter(command, helper.Evenrelevante, DbType.Int32, entity.Evenrelevante);
            dbProvider.AddInParameter(command, helper.Evenctaf, DbType.String, entity.Evenctaf);
            dbProvider.AddInParameter(command, helper.Eveninffalla, DbType.String, entity.Eveninffalla);
            dbProvider.AddInParameter(command, helper.Eveninffallan2, DbType.String, entity.Eveninffallan2);
            dbProvider.AddInParameter(command, helper.Deleted, DbType.String, entity.Deleted);
            dbProvider.AddInParameter(command, helper.Eventipofalla, DbType.String, entity.Eventipofalla);
            dbProvider.AddInParameter(command, helper.Eventipofallafase, DbType.String, entity.Eventipofallafase);
            dbProvider.AddInParameter(command, helper.Smsenviado, DbType.String, entity.Smsenviado);
            dbProvider.AddInParameter(command, helper.Smsenviar, DbType.String, entity.Smsenviar);
            dbProvider.AddInParameter(command, helper.Evenactuacion, DbType.String, entity.Evenactuacion);
            dbProvider.AddInParameter(command, helper.Tiporegistro, DbType.String, entity.Tiporegistro);
            dbProvider.AddInParameter(command, helper.Valtiporegistro, DbType.String, entity.Valtiporegistro);
            dbProvider.AddInParameter(command, helper.Subcausacodiop, DbType.Int32, entity.Subcausacodiop);
            dbProvider.AddInParameter(command, helper.Evenmwgendescon, DbType.Decimal, entity.Evenmwgendescon);
            dbProvider.AddInParameter(command, helper.Evengendescon, DbType.String, entity.Evengendescon);
            dbProvider.AddInParameter(command, helper.Evenasegoperacion, DbType.String, entity.Evenasegoperacion);
            dbProvider.AddInParameter(command, helper.EveAdjunto, DbType.String, entity.EveAdjunto);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Evencomentarios, DbType.String, entity.Evencomentarios);
            dbProvider.AddInParameter(command, helper.Evenperturbacion, DbType.String, entity.Evenperturbacion);
            dbProvider.AddInParameter(command, helper.Twitterenviado, DbType.String, entity.Twitterenviado);            
            dbProvider.AddInParameter(command, helper.Emprcodirespon, DbType.Int32, entity.Emprcodirespon);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, entity.Tipoevencodi);
            dbProvider.AddInParameter(command, helper.Evenini, DbType.DateTime, entity.Evenini);
            dbProvider.AddInParameter(command, helper.Evenmwindisp, DbType.Decimal, entity.Evenmwindisp);
            dbProvider.AddInParameter(command, helper.Evenfin, DbType.DateTime, entity.Evenfin);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Evenasunto, DbType.String, entity.Evenasunto);
            dbProvider.AddInParameter(command, helper.Evenpadre, DbType.Int32, entity.Evenpadre);
            dbProvider.AddInParameter(command, helper.Eveninterrup, DbType.String, entity.Eveninterrup);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Evenpreini, DbType.DateTime, entity.Evenpreini);
            dbProvider.AddInParameter(command, helper.Evenpostfin, DbType.DateTime, entity.Evenpostfin);
            dbProvider.AddInParameter(command, helper.Evendesc, DbType.String, entity.Evendesc);
            dbProvider.AddInParameter(command, helper.Eventension, DbType.Decimal, entity.Eventension);
            dbProvider.AddInParameter(command, helper.Evenaopera, DbType.String, entity.Evenaopera);
            dbProvider.AddInParameter(command, helper.Evenpreliminar, DbType.String, entity.Evenpreliminar);
            dbProvider.AddInParameter(command, helper.Evenrelevante, DbType.Int32, entity.Evenrelevante);
            //dbProvider.AddInParameter(command, helper.Evenctaf, DbType.String, entity.Evenctaf);
            dbProvider.AddInParameter(command, helper.Eveninffalla, DbType.String, entity.Eveninffalla);
            dbProvider.AddInParameter(command, helper.Eveninffallan2, DbType.String, entity.Eveninffallan2);
            dbProvider.AddInParameter(command, helper.Deleted, DbType.String, entity.Deleted);
            dbProvider.AddInParameter(command, helper.Eventipofalla, DbType.String, entity.Eventipofalla);
            dbProvider.AddInParameter(command, helper.Eventipofallafase, DbType.String, entity.Eventipofallafase);
            dbProvider.AddInParameter(command, helper.Smsenviado, DbType.String, entity.Smsenviado);
            dbProvider.AddInParameter(command, helper.Smsenviar, DbType.String, entity.Smsenviar);
            dbProvider.AddInParameter(command, helper.Evenactuacion, DbType.String, entity.Evenactuacion);
            dbProvider.AddInParameter(command, helper.Tiporegistro, DbType.String, entity.Tiporegistro);
            dbProvider.AddInParameter(command, helper.Valtiporegistro, DbType.String, entity.Valtiporegistro);
            dbProvider.AddInParameter(command, helper.Subcausacodiop, DbType.Int32, entity.Subcausacodiop);
            dbProvider.AddInParameter(command, helper.Evenmwgendescon, DbType.Decimal, entity.Evenmwgendescon);
            dbProvider.AddInParameter(command, helper.Evengendescon, DbType.String, entity.Evengendescon);
            dbProvider.AddInParameter(command, helper.Evenasegoperacion, DbType.String, entity.Evenasegoperacion);
            dbProvider.AddInParameter(command, helper.Evenasegoperacion, DbType.String, entity.EveAdjunto);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int evencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, evencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int evencodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, evencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarEventoAseguramiento(int idEvento)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarEventoAseguramiento);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, idEvento);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveEventoDTO GetById(int evencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, evencodi);
            EveEventoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iTiporegistro = dr.GetOrdinal(helper.Tiporegistro);
                    if (!dr.IsDBNull(iTiporegistro)) entity.Tiporegistro = dr.GetString(iTiporegistro);

                    int iValtiporegistro = dr.GetOrdinal(helper.Valtiporegistro);
                    if (!dr.IsDBNull(iValtiporegistro)) entity.Valtiporegistro = dr.GetString(iValtiporegistro);

                    int iEvenasegoperacion = dr.GetOrdinal(helper.Evenasegoperacion);
                    if (!dr.IsDBNull(iEvenasegoperacion)) entity.Evenasegoperacion = dr.GetString(iEvenasegoperacion);

                    int iEvenrcmctaf = dr.GetOrdinal(helper.Evenrcmctaf);
                    if (!dr.IsDBNull(iEvenrcmctaf)) entity.Evenrcmctaf = dr.GetString(iEvenrcmctaf);
                }
            }

            return entity;
        }

        public List<EveEventoDTO> List()
        {
            List<EveEventoDTO> entitys = new List<EveEventoDTO>();
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

        public List<EveEventoDTO> ListEventos(int idPeriodo)
        {
            List<EveEventoDTO> entitys = new List<EveEventoDTO>();
            String query = String.Format(helper.SqlListEventosCarga, idPeriodo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveEventoDTO entity = new EveEventoDTO();

                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

                    int iCodEve = dr.GetOrdinal(this.helper.Codeve);
                    if (!dr.IsDBNull(iCodEve)) entity.CodEve = dr.GetString(iCodEve);                                       

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveEventoDTO> GetByCriteria()
        {
            List<EveEventoDTO> entitys = new List<EveEventoDTO>();
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

        public int ObtenerNroRegistrosConsultaExtranet(DateTime fechaInicio, DateTime fechaFin, int? idTipoEvento)
        {
            String sql = String.Format(this.helper.SqlObtenerNroRegistrosConsultaExtranet, idTipoEvento, 
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            object result = dbProvider.ExecuteScalar(command);

            if (result != null) 
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<EveEventoDTO> ConsultaEventoExtranet(DateTime fechaInicio, DateTime fechaFin,
            int? idTipoEvento, int nroPage, int pageSize)
        {
            List<EveEventoDTO> entitys = new List<EveEventoDTO>();
            String sql = String.Format(this.helper.SqlObtenerConsultaExtranet, idTipoEvento, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveEventoDTO entity = new EveEventoDTO();                  

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iTipoevenabrev = dr.GetOrdinal(this.helper.Tipoevenabrev);
                    if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTareaabrev = dr.GetOrdinal(this.helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEvenini = dr.GetOrdinal(this.helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenfin = dr.GetOrdinal(this.helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iEvenasunto = dr.GetOrdinal(this.helper.Evenasunto);
                    if (!dr.IsDBNull(iEvenasunto)) entity.Evenasunto = dr.GetString(iEvenasunto);

                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public EveEventoDTO GetDetalleEvento(int idEvento)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetDetalleEvento);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, idEvento);
            EveEventoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EveEventoDTO();

                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

                    int iEvenini = dr.GetOrdinal(this.helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenfin = dr.GetOrdinal(this.helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iEvendesc = dr.GetOrdinal(this.helper.Evendesc);
                    if (!dr.IsDBNull(iEvendesc)) entity.Evendesc = dr.GetString(iEvendesc);

                    int iEvenasunto = dr.GetOrdinal(this.helper.Evenasunto);
                    if (!dr.IsDBNull(iEvenasunto)) entity.Evenasunto = dr.GetString(iEvenasunto);

                    int iTipoevenabrev = dr.GetOrdinal(this.helper.Tipoevenabrev);
                    if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);

                    int iIndinforme = dr.GetOrdinal(this.helper.Indinforme);
                    if (!dr.IsDBNull(iIndinforme)) entity.Indinforme = dr.GetString(iIndinforme);
                }
            }

            return entity;
        }

        public void CambiarVersion(int idEvento, string version, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCambiarVersion);
                        
            dbProvider.AddInParameter(command, helper.Evenpreliminar, DbType.String, version);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, idEvento);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<EveEventoDTO> ListarResumenEventosWeb(DateTime fecha)
        {
            List<EveEventoDTO> entitys = new List<EveEventoDTO>();
            string query = String.Format(helper.SqlListarResumenEventosWeb,
                fecha.AddDays(-1).ToString(ConstantesBase.FormatoFecha), fecha.AddDays(1).ToString(ConstantesBase.FormatoFecha));            
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            try
            {
                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        EveEventoDTO entity = new EveEventoDTO();

                        int iEvenini = dr.GetOrdinal(this.helper.Evenini);
                        if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                        int iEvenasunto = dr.GetOrdinal(this.helper.Evenasunto);
                        if (!dr.IsDBNull(iEvenasunto)) entity.Evenasunto = dr.GetString(iEvenasunto);

                        int iEvendesc = dr.GetOrdinal(this.helper.Evendesc);
                        if (!dr.IsDBNull(iEvendesc)) entity.Evendesc = dr.GetString(iEvendesc);

                        int iEveninterrup = dr.GetOrdinal(this.helper.Eveninterrup);
                        if (!dr.IsDBNull(iEveninterrup)) entity.Eveninterrup = dr.GetString(iEveninterrup);

                        int iEvenmwindisp = dr.GetOrdinal(this.helper.Evenmwindisp);
                        if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = dr.GetDecimal(iEvenmwindisp);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al obtener los reports: " + e.Message);
                Console.WriteLine("StackTrace: " + e.StackTrace);
                return entitys;
            }



           

            return entitys;
        }

        #region PR5
        public List<EveEventoDTO> ObtenerEventoEquipo(string idsEquipo, DateTime fechaInicio, DateTime fechaFin, int evenclasecodi)
        {
            List<EveEventoDTO> entitys = new List<EveEventoDTO>();
            string query = string.Format(helper.SqlGeEventoEquipo, idsEquipo, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), evenclasecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveEventoDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveEventoDTO> GetEventosCausaSubCausa(string tipoequipo, string causaeven, DateTime fechaInicio, DateTime fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlGetEventosCausaSubCausa, tipoequipo, causaeven, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            List<EveEventoDTO> entitys = new List<EveEventoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            EveEventoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EveEventoDTO();

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = dr.GetInt32(iFamcodi);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iCausaevencodi = dr.GetOrdinal(helper.Causaevencodi);
                    if (!dr.IsDBNull(iCausaevencodi)) entity.Causaevencodi = dr.GetInt32(iCausaevencodi);

                    int iCausaevenabrev = dr.GetOrdinal(helper.Causaevenabrev);
                    if (!dr.IsDBNull(iCausaevenabrev)) entity.Causaevenabrev = dr.GetString(iCausaevenabrev);

                    int iCausaevendesc = dr.GetOrdinal(helper.Causaevendesc);
                    if (!dr.IsDBNull(iCausaevendesc)) entity.Causaevendesc = dr.GetString(iCausaevendesc); ;

                    int iEvenIni = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenIni)) entity.Evenini = dr.GetDateTime(iEvenIni);

                    int iEvenFin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenFin)) entity.Evenfin = dr.GetDateTime(iEvenFin);

                    int iEveninterrup = dr.GetOrdinal(helper.Eveninterrupmw);
                    if (!dr.IsDBNull(iEveninterrup)) entity.Eveninterrupmw = dr.GetDecimal(iEveninterrup);

                    int iEvenenergia = dr.GetOrdinal(helper.Evenenergia);
                    if (!dr.IsDBNull(iEvenenergia)) entity.Evenenergia = dr.GetDecimal(iEvenenergia);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveEventoDTO> ListarReporteEventoIOED(string idEmpresa, string idUbicacion, string idCentral, string tipoevencodi, DateTime fechaIni, DateTime fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlListarReporteEventoIOED, tipoevencodi, idEmpresa, idCentral, idUbicacion, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            List<EveEventoDTO> entitys = new List<EveEventoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveEventoDTO entity = new EveEventoDTO();

                    int iEmprCodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    int iEmprNomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iAreaNom = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreaNom)) entity.Areanomb = dr.GetString(iAreaNom);

                    int iEquiCodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEquiNom = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquiNom)) entity.Equinomb = dr.GetString(iEquiNom);

                    int iEvenIni = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenIni)) entity.Evenini = dr.GetDateTime(iEvenIni);

                    int iEvenFin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenFin)) entity.Evenfin = dr.GetDateTime(iEvenFin);

                    int iEvenDesc = dr.GetOrdinal(helper.Evendesc);
                    if (!dr.IsDBNull(iEvenDesc)) entity.Evendesc = dr.GetString(iEvenDesc);

                    int iEvenmwindisp = dr.GetOrdinal(helper.Evenmwindisp);
                    if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = dr.GetDecimal(iEvenmwindisp);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region SIOSEIN

        public List<EveEventoDTO> ObtenerEventosConInterrupciones(DateTime fechaInicio, DateTime fechaFin)
        {
            List<EveEventoDTO> entitys = new List<EveEventoDTO>();
            string query = string.Format(helper.SqlObtenerEventosConInterrupciones, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveEventoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprsein= dr.GetOrdinal(helper.Emprsein);                                  //SIOSEIN-PRIE-2021
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprsein = dr.GetString(iEmprsein);         //SIOSEIN-PRIE-2021

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = dr.GetInt32(iTipoemprcodi);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iInterrminu = dr.GetOrdinal(helper.Interrminu);
                    if (!dr.IsDBNull(iInterrminu)) entity.Interrminu = dr.GetDecimal(iInterrminu);

                    int iInterrmw = dr.GetOrdinal(helper.Interrmw);
                    if (!dr.IsDBNull(iInterrmw)) entity.Interrmw = dr.GetDecimal(iInterrmw);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EveEventoDTO> GetListaHechosRelevantes(DateTime fechaInicio, DateTime fechaFin)
        {
            List<EveEventoDTO> entitys = new List<EveEventoDTO>();
            string query = string.Format(helper.SqlGetListaHechosRelevantes, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                EveEventoDTO entity;
                while (dr.Read())
                {
                    entity = new EveEventoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenmwindisp = dr.GetOrdinal(helper.Evenmwindisp);
                    if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = dr.GetDecimal(iEvenmwindisp);

                    int iEvenfin = dr.GetOrdinal(helper.Evenfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

                    int iEvenasunto = dr.GetOrdinal(helper.Evenasunto);
                    if (!dr.IsDBNull(iEvenasunto)) entity.Evenasunto = dr.GetString(iEvenasunto);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = dr.GetInt32(iTipoemprcodi);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion

        #region MigracionSGOCOES-GrupoB
        public List<EveEventoDTO> ListaEventosImportantes(DateTime fecIni)
        {
            List<EveEventoDTO> entitys = new List<EveEventoDTO>();
            string query = String.Format(helper.SqlListaEventosImportantes, fecIni.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveEventoDTO entity = new EveEventoDTO();

                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

                    int iEvenini = dr.GetOrdinal(this.helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEmprabrev = dr.GetOrdinal(this.helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEvenasunto = dr.GetOrdinal(this.helper.Evenasunto);
                    if (!dr.IsDBNull(iEvenasunto)) entity.Evenasunto = dr.GetString(iEvenasunto);

                    int iEvendesc = dr.GetOrdinal(this.helper.Evendesc);
                    if (!dr.IsDBNull(iEvendesc)) entity.Evendesc = dr.GetString(iEvendesc);

                    int iEveninterrup = dr.GetOrdinal(this.helper.Eveninterrup);
                    if (!dr.IsDBNull(iEveninterrup)) entity.Eveninterrup = dr.GetString(iEveninterrup);

                    int iEveninterrupmw = dr.GetOrdinal(this.helper.Eveninterrupmw);
                    if (!dr.IsDBNull(iEveninterrupmw)) entity.Eveninterrupmw = dr.GetDecimal(iEveninterrupmw);

                    int iEvenbajomw = dr.GetOrdinal(this.helper.Evenbajomw);
                    if (!dr.IsDBNull(iEvenbajomw)) entity.Evenbajomw = dr.GetDecimal(iEvenbajomw);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region Mejoras CTAF
        public List<EveEventoDTO> ListadoEventoSco(DateTime fechaInicio, DateTime fechaFin)
        {
            List<EveEventoDTO> entitys = new List<EveEventoDTO>();
            String sql = String.Format(this.helper.SqlListadoEventoSco, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveEventoDTO entity = new EveEventoDTO();

                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEvenini = dr.GetOrdinal(this.helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.EvenIni = dr.GetDateTime(iEvenini);

                    int iEvenasunto = dr.GetOrdinal(this.helper.Evenasunto);
                    if (!dr.IsDBNull(iEvenasunto)) entity.Evenasunto = dr.GetString(iEvenasunto); //Descripción(Resúmen)

                    int iEvendesc = dr.GetOrdinal(this.helper.Evendesc);
                    if (!dr.IsDBNull(iEvendesc)) entity.Evendesc = dr.GetString(iEvendesc); //Detalle adicional

                    int iEveninffalla = dr.GetOrdinal(this.helper.Eveninffalla);
                    if (!dr.IsDBNull(iEveninffalla)) entity.Eveninffalla = dr.GetString(iEveninffalla);

                    int iEveninffallan2 = dr.GetOrdinal(this.helper.Eveninffallan2);
                    if (!dr.IsDBNull(iEveninffallan2)) entity.Eveninffallan2 = dr.GetString(iEveninffallan2);

                    int iEveninfplazodiasipi = dr.GetOrdinal(this.helper.Eveninfplazodiasipi);
                    if (!dr.IsDBNull(iEveninfplazodiasipi)) entity.Eveninfplazodiasipi = Convert.ToInt32(dr.GetValue(iEveninfplazodiasipi));

                    int iEveninfplazodiasif = dr.GetOrdinal(this.helper.Eveninfplazodiasif);
                    if (!dr.IsDBNull(iEveninfplazodiasif)) entity.Eveninfplazodiasif = Convert.ToInt32(dr.GetValue(iEveninfplazodiasif));

                    int iEveninfplazohoraipi = dr.GetOrdinal(this.helper.Eveninfplazohoraipi);
                    if (!dr.IsDBNull(iEveninfplazohoraipi)) entity.Eveninfplazohoraipi = Convert.ToInt32(dr.GetValue(iEveninfplazohoraipi));

                    int iEveninfplazohoraif = dr.GetOrdinal(this.helper.Eveninfplazohoraif);
                    if (!dr.IsDBNull(iEveninfplazohoraif)) entity.Eveninfplazohoraif = Convert.ToInt32(dr.GetValue(iEveninfplazohoraif));

                    int iEveninfplazominipi = dr.GetOrdinal(this.helper.Eveninfplazominipi);
                    if (!dr.IsDBNull(iEveninfplazominipi)) entity.Eveninfplazominipi = Convert.ToInt32(dr.GetValue(iEveninfplazominipi));

                    int iEveninfplazominif = dr.GetOrdinal(this.helper.Eveninfplazominif);
                    if (!dr.IsDBNull(iEveninfplazominif)) entity.Eveninfplazominif = Convert.ToInt32(dr.GetValue(iEveninfplazominif));

                    int iEveninfplazodiasipi_N2 = dr.GetOrdinal(this.helper.Eveninfplazodiasipi_N2);
                    if (!dr.IsDBNull(iEveninfplazodiasipi_N2)) entity.Eveninfplazodiasipi_N2 = Convert.ToInt32(dr.GetValue(iEveninfplazodiasipi_N2));

                    int iEveninfplazodiasif_N2 = dr.GetOrdinal(this.helper.Eveninfplazodiasif_N2);
                    if (!dr.IsDBNull(iEveninfplazodiasif_N2)) entity.Eveninfplazodiasif_N2 = Convert.ToInt32(dr.GetValue(iEveninfplazodiasif_N2));

                    int iEveninfplazohoraipi_N2 = dr.GetOrdinal(this.helper.Eveninfplazohoraipi_N2);
                    if (!dr.IsDBNull(iEveninfplazohoraipi_N2)) entity.Eveninfplazohoraipi_N2 = Convert.ToInt32(dr.GetValue(iEveninfplazohoraipi_N2));

                    int iEveninfplazohoraif_N2 = dr.GetOrdinal(this.helper.Eveninfplazohoraif_N2);
                    if (!dr.IsDBNull(iEveninfplazohoraif_N2)) entity.Eveninfplazohoraif_N2 = Convert.ToInt32(dr.GetValue(iEveninfplazohoraif_N2));

                    int iEveninfplazominipi_N2 = dr.GetOrdinal(this.helper.Eveninfplazominipi_N2);
                    if (!dr.IsDBNull(iEveninfplazominipi_N2)) entity.Eveninfplazominipi_N2 = Convert.ToInt32(dr.GetValue(iEveninfplazominipi_N2));

                    int iEveninfplazominif_N2 = dr.GetOrdinal(this.helper.Eveninfplazominif_N2);
                    if (!dr.IsDBNull(iEveninfplazominif_N2)) entity.Eveninfplazominif_N2 = Convert.ToInt32(dr.GetValue(iEveninfplazominif_N2));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public void UpdateEventoCtaf(int evencodi, string estado)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateEventoCtaf);         
            dbProvider.AddInParameter(command, helper.Evenctaf, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Evenpreliminar, DbType.String, "N");
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, evencodi);
            dbProvider.ExecuteNonQuery(command);

            //String sql = String.Format(this.helper.SqlUpdateEventoCtaf, evencodi, estado);

            //DbCommand command1 = dbProvider.GetSqlStringCommand(sql);
            //dbProvider.ExecuteScalar(command1);


        }
        public void insertarEventoEvento(int evencodi, int evencodi_as)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlinsertarEventoEvento);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, evencodi);
            dbProvider.AddInParameter(command, helper.Evencodi_as, DbType.Int32, evencodi_as);
            dbProvider.ExecuteNonQuery(command);
            //IDbConnection conn = this.BeginConnection();
            //DbProviderFactory factory = DbProviderFactories.GetFactory("Oracle.ManagedDataAccess.Client");
            //using (DbConnection dbConnection = factory.CreateConnection())
            //{
            //    if (dbConnection == null)
            //    {
            //        throw new InvalidOperationException("No se pudo crear la conexión a la base de datos.");
            //    }

            //    dbConnection.ConnectionString = conn.ConnectionString;
            //    dbConnection.Open();
            //    using (DbTransaction transaction = dbConnection.BeginTransaction())
            //    {
            //        try
            //        {
            //            using (DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlinsertarEventoEvento))
            //            {
            //                command.Transaction = transaction;
            //                dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, evencodi);
            //                dbProvider.AddInParameter(command, helper.Evencodi_as, DbType.Int32, evencodi_as);
            //                dbProvider.ExecuteNonQuery(command);
            //            }
            //            // Si todo sale bien, confirma la transacción
            //            transaction.Commit();
            //        }
            //        catch (Exception ex)
            //        {
            //            // Si ocurre un error, haz rollback
            //            transaction.Rollback();
            //        }
            //    }
            //}
        }

        public List<EveEventoDTO> ListadoEventosAsoCtaf(int evencodi)
        {
            List<EveEventoDTO> entitys = new List<EveEventoDTO>();
            String sql = String.Format(this.helper.SqlListadoEventosAsoCtaf, evencodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveEventoDTO entity = new EveEventoDTO();

                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public int ObtieneCantFileEnviadosSco(int evencodi)
        {
            String sql = String.Format(this.helper.SqlObtieneCantFileEnviadosSco, evencodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }
        #endregion
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

        public int insertarEventoEventoR(int evencodi, int evencodi_as, IDbConnection conn, DbTransaction tran)
        {
            DbCommand commandUp = (DbCommand)conn.CreateCommand();
            commandUp.CommandText = helper.SqlinsertarEventoEvento;
            commandUp.Transaction = tran;
            commandUp.Connection = (DbConnection)conn;

            IDbDataParameter param = null;

            param = commandUp.CreateParameter(); param.ParameterName = helper.Evencodi; param.Value = evencodi; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Evencodi_as; param.Value = evencodi_as; commandUp.Parameters.Add(param);
  

            try
            {
                commandUp.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return -1;
            }

            return 1;
        }

        public int UpdateEventoCtafR(int evencodi, string estado, IDbConnection conn, DbTransaction tran)
        {
            DbCommand commandUp = (DbCommand)conn.CreateCommand();
            commandUp.CommandText = helper.SqlUpdateEventoCtaf;
            commandUp.Transaction = tran;
            commandUp.Connection = (DbConnection)conn;

            IDbDataParameter param = null;

            param = commandUp.CreateParameter(); param.ParameterName = helper.Evenctaf; param.Value = estado; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Evenpreliminar; param.Value = "N"; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Evencodi; param.Value = evencodi; commandUp.Parameters.Add(param);

            try
            {
                commandUp.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return -1;
            }

            return 1;

        }
    }
}
