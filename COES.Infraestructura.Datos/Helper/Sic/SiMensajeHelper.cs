using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

using System.Text;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MENSAJE
    /// </summary>
    public class SiMensajeHelper : HelperBase
    {
        public SiMensajeHelper()
            : base(Consultas.SiMensajeSQL)
        {

        }

        public SiMensajeDTO Create(IDataReader dr)
        {
            SiMensajeDTO entity = new SiMensajeDTO();

            int iMsgfecmodificacion = dr.GetOrdinal(this.Msgfecmodificacion);
            if (!dr.IsDBNull(iMsgfecmodificacion)) entity.Msgfecmodificacion = dr.GetDateTime(iMsgfecmodificacion);

            int iMsgusumodificacion = dr.GetOrdinal(this.Msgusumodificacion);
            if (!dr.IsDBNull(iMsgusumodificacion)) entity.Msgusumodificacion = dr.GetString(iMsgusumodificacion);

            int iMsgfeccreacion = dr.GetOrdinal(this.Msgfeccreacion);
            if (!dr.IsDBNull(iMsgfeccreacion)) entity.Msgfeccreacion = dr.GetDateTime(iMsgfeccreacion);

            int iMsgusucreacion = dr.GetOrdinal(this.Msgusucreacion);
            if (!dr.IsDBNull(iMsgusucreacion)) entity.Msgusucreacion = dr.GetString(iMsgusucreacion);

            int iMsgtipo = dr.GetOrdinal(this.Msgtipo);
            if (!dr.IsDBNull(iMsgtipo)) entity.Msgtipo = Convert.ToInt32(dr.GetValue(iMsgtipo));

            int iMsgestado = dr.GetOrdinal(this.Msgestado);
            if (!dr.IsDBNull(iMsgestado)) entity.Msgestado = dr.GetString(iMsgestado);

            int iBandcodi = dr.GetOrdinal(this.Bandcodi);
            if (!dr.IsDBNull(iBandcodi)) entity.Bandcodi = Convert.ToInt32(dr.GetValue(iBandcodi));

            int iMsgflagadj = dr.GetOrdinal(this.Msgflagadj);
            if (!dr.IsDBNull(iMsgflagadj)) entity.Msgflagadj = Convert.ToInt32(dr.GetValue(iMsgflagadj));

            int iMsgfromname = dr.GetOrdinal(this.Msgfromname);
            if (!dr.IsDBNull(iMsgfromname)) entity.Msgfromname = dr.GetString(iMsgfromname);

            int iMsgfrom = dr.GetOrdinal(this.Msgfrom);
            if (!dr.IsDBNull(iMsgfrom)) entity.Msgfrom = dr.GetString(iMsgfrom);

            int iMsgto = dr.GetOrdinal(this.Msgto);
            if (!dr.IsDBNull(iMsgto)) entity.Msgto = dr.GetString(iMsgto);

            int iMsgcc = dr.GetOrdinal(this.Msgcc);
            if (!dr.IsDBNull(iMsgcc)) entity.Msgcc = dr.GetString(iMsgcc);

            int iMsgbcc = dr.GetOrdinal(this.Msgbcc);
            if (!dr.IsDBNull(iMsgbcc)) entity.Msgbcc = dr.GetString(iMsgbcc);

            int iMsgasunto = dr.GetOrdinal(this.Msgasunto);
            if (!dr.IsDBNull(iMsgasunto)) entity.Msgasunto = dr.GetString(iMsgasunto);

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iModcodi = dr.GetOrdinal(this.Modcodi);
            if (!dr.IsDBNull(iModcodi)) entity.Modcodi = Convert.ToInt32(dr.GetValue(iModcodi));

            int iMsgfechaperiodo = dr.GetOrdinal(this.Msgfechaperiodo);
            if (!dr.IsDBNull(iMsgfechaperiodo)) entity.Msgfechaperiodo = dr.GetDateTime(iMsgfechaperiodo);

            int iMsgcontenido = dr.GetOrdinal(this.Msgcontenido);
            if (!dr.IsDBNull(iMsgcontenido)) entity.Msgcontenido = dr.GetString(iMsgcontenido);

            int iEstmsgcodi = dr.GetOrdinal(this.Estmsgcodi);
            if (!dr.IsDBNull(iEstmsgcodi)) entity.Estmsgcodi = Convert.ToInt32(dr.GetValue(iEstmsgcodi));

            int iTmsgcodi = dr.GetOrdinal(this.Tmsgcodi);
            if (!dr.IsDBNull(iTmsgcodi)) entity.Tmsgcodi = Convert.ToInt32(dr.GetValue(iTmsgcodi));

            int iFdatcodi = dr.GetOrdinal(this.Fdatcodi);
            if (!dr.IsDBNull(iFdatcodi)) entity.Fdatcodi = Convert.ToInt32(dr.GetValue(iFdatcodi));

            int iMsgfecha = dr.GetOrdinal(this.Msgfecha);
            if (!dr.IsDBNull(iMsgfecha)) entity.Msgfecha = dr.GetDateTime(iMsgfecha);

            int iMsgcodi = dr.GetOrdinal(this.Msgcodi);
            if (!dr.IsDBNull(iMsgcodi)) entity.Msgcodi = Convert.ToInt32(dr.GetValue(iMsgcodi));

            return entity;
        }

        #region INTERVENCIONES - CREARMENSAJE
        public SiMensajeDTO CrearMensaje(IDataReader dr)
        {
            SiMensajeDTO entity = new SiMensajeDTO();

            int iMsgcodi = dr.GetOrdinal(this.Msgcodi);
            if (!dr.IsDBNull(iMsgcodi)) entity.Msgcodi = Convert.ToInt32(dr.GetValue(iMsgcodi));

            int iMsgfecha = dr.GetOrdinal(this.Msgfecha);
            if (!dr.IsDBNull(iMsgfecha)) entity.Msgfecha = dr.GetDateTime(iMsgfecha);

            int iFdatcodi = dr.GetOrdinal(this.Fdatcodi);
            if (!dr.IsDBNull(iFdatcodi)) entity.Fdatcodi = Convert.ToInt32(dr.GetValue(iFdatcodi));

            int iTmsgcodi = dr.GetOrdinal(this.Tmsgcodi);
            if (!dr.IsDBNull(iTmsgcodi)) entity.Tmsgcodi = Convert.ToInt32(dr.GetValue(iTmsgcodi));

            int iEstmsgcodi = dr.GetOrdinal(this.Estmsgcodi);
            if (!dr.IsDBNull(iEstmsgcodi)) entity.Estmsgcodi = Convert.ToInt32(dr.GetValue(iEstmsgcodi));

            int iMsgcontenido = dr.GetOrdinal(this.Msgcontenido);
            if (!dr.IsDBNull(iMsgcontenido)) entity.Msgcontenido = dr.GetString(iMsgcontenido);

            int iMsgfechaperiodo = dr.GetOrdinal(this.Msgfechaperiodo);
            if (!dr.IsDBNull(iMsgfechaperiodo)) entity.Msgfechaperiodo = dr.GetDateTime(iMsgfechaperiodo);

            int iModcodi = dr.GetOrdinal(this.Modcodi);
            if (!dr.IsDBNull(iModcodi)) entity.Modcodi = Convert.ToInt32(dr.GetValue(iModcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iFormatCodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatCodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatCodi));

            int iMsgAsunto = dr.GetOrdinal(this.Msgasunto);
            if (!dr.IsDBNull(iMsgAsunto)) entity.Msgasunto = dr.GetString(iMsgAsunto);

            int iMsgTo = dr.GetOrdinal(this.Msgto);
            if (!dr.IsDBNull(iMsgTo)) entity.Msgto = dr.GetString(iMsgTo);

            int iMsgFrom = dr.GetOrdinal(this.Msgfrom);
            if (!dr.IsDBNull(iMsgFrom)) entity.Msgfrom = dr.GetString(iMsgFrom);

            int iMsgcc = dr.GetOrdinal(this.Msgcc);
            if (!dr.IsDBNull(iMsgcc)) entity.Msgcc = dr.GetString(iMsgcc);

            int iMsgbcc = dr.GetOrdinal(this.Msgbcc);
            if (!dr.IsDBNull(iMsgbcc)) entity.Msgbcc = dr.GetString(iMsgbcc);

            int iMsgFromName = dr.GetOrdinal(this.Msgfromname);
            if (!dr.IsDBNull(iMsgFromName)) entity.Msgfromname = dr.GetString(iMsgFromName);

            int iMsgFlagAdj = dr.GetOrdinal(this.Msgflagadj);
            if (!dr.IsDBNull(iMsgFlagAdj)) entity.Msgflagadj = Convert.ToInt32(dr.GetValue(iMsgFlagAdj));

            int iBandCodi = dr.GetOrdinal(this.Bandcodi);
            if (!dr.IsDBNull(iBandCodi)) entity.Bandcodi = Convert.ToInt32(dr.GetValue(iBandCodi));

            int iMsgEstado = dr.GetOrdinal(this.Msgestado);
            if (!dr.IsDBNull(iMsgEstado)) entity.Msgestado = dr.GetString(iMsgEstado);

            int iMsgTipo = dr.GetOrdinal(this.Msgtipo);
            if (!dr.IsDBNull(iMsgTipo)) entity.Msgtipo = dr.GetInt32(iMsgTipo);

            int iMsgusucreacion = dr.GetOrdinal(this.Msgusucreacion);
            if (!dr.IsDBNull(iMsgusucreacion)) entity.Msgusucreacion = dr.GetString(iMsgusucreacion);

            int iMsgfeccreacion = dr.GetOrdinal(this.Msgfeccreacion);
            if (!dr.IsDBNull(iMsgfeccreacion)) entity.Msgfeccreacion = dr.GetDateTime(iMsgfeccreacion);

            int iMsgfecmodificacion = dr.GetOrdinal(this.Msgfecmodificacion);
            if (!dr.IsDBNull(iMsgfecmodificacion)) entity.Msgfecmodificacion = dr.GetDateTime(iMsgfecmodificacion);

            int iMsgusumodificacion = dr.GetOrdinal(this.Msgusumodificacion);
            if (!dr.IsDBNull(iMsgusumodificacion)) entity.Msgusumodificacion = dr.GetString(iMsgusumodificacion);

            return entity;
        }
        #endregion



        #region
        public string Msgcodi = "MSGCODI";
        public string Msgfecha = "MSGFECHA";
        public string Fdatcodi = "FDATCODI";
        public string Tmsgcodi = "TMSGCODI";
        public string Estmsgcodi = "ESTMSGCODI";
        public string Msgcontenido = "MSGCONTENIDO";
        public string Msgfechaperiodo = "MSGFECHAPERIODO";
        public string Modcodi = "MODCODI";
        public string Emprcodi = "EMPRCODI";
        public string Formatcodi = "FORMATCODI";
        public string Msgusumodificacion = "MSGUSUMODIFICACION";
        public string Msgfecmodificacion = "MSGFECMODIFICACION";
        public string Msgusucreacion = "MSGUSUCREACION";
        public string Msgto = "MSGTO";
        public string Msgfrom = "MSGFROM";
        public string Msgcc = "MSGCC";
        public string Msgbcc = "MSGBCC";
        public string Msgfromname = "MSGFROMNAME";
        public string Msgasunto = "MSGASUNTO";
        public string Msgflagadj = "MSGFLAGADJ";

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 09/10/2017: NUEVOS CAMPOS MAPEADOS
        //--------------------------------------------------------------------------------
        public string Msgestado = "MSGESTADO";
        public string Bandcodi = "BANDCODI";
        public string Carcodi = "CARCODI";
        public string Cmsgcodi = "CMSGCODI";
        public string Msgtipo = "MSGTIPO";
        public string Msgfeccreacion = "MSGFECCREACION";
        public string Intercodi = "INTERCODI";
        public string Progrcodi = "PROGRCODI";
        public string Emprnomb = "EMPRNOMB";

        //--------------------------------------------------------------------------------

        #region Mejoras Intervenciones
        public string Intercodivigente = "INTERCODIVIGENTE";
        public string Programacion = "PROGRAMACION";
        public string Msglectura = "MSGLECTURA";
        #endregion

        #endregion

        #region Campos para paginado
        public string NROPAGINA = "NROPAGINA";
        public string PAGESIZE = "PAGESIZE";
        #endregion



        public string SqlListar
        {
            get { return base.GetSqlXml("Listar"); }
        }

        public string SqlListarEnviados
        {
            get { return base.GetSqlXml("ListarEnviados"); }
        }

        public string SqlUpdateEstado
        {
            get { return base.GetSqlXml("UpdateEstado"); }
        }

        public string SqlInsertAmpl
        {
            get { return base.GetSqlXml("InsertAmpl"); }
        }

        public string SqlTotalListaRecibidos
        {
            get { return base.GetSqlXml("TotalListaRecibidos"); }
        }

        public string SqlTotalListaEnviados
        {
            get { return base.GetSqlXml("TotalListaEnviados"); }
        }

        #region Siosein
        public string SqlTotalListaEnviadosSiosein
        {
            get { return base.GetSqlXml("TotalListaEnviadosSiosein"); }
        }

        public string SqlTotalListaRecibidosSiosein
        {
            get { return base.GetSqlXml("TotalListaRecibidosSiosein"); }
        }

        public string SqlListarxUsuario
        {
            get { return base.GetSqlXml("ListarXUsuario"); }
        }

        public string SqlListarxUsuarioEnv
        {
            get { return base.GetSqlXml("ListarXUsuarioEnv"); }
        }

        public string SqlSaveCorreoSiosein
        {
            get { return base.GetSqlXml("SaveCorreoSiosein"); }
        }

        public string SqlUpdateCarpeta
        {
            get { return base.GetSqlXml("UpdateCarpeta"); }
        }

        public string SqlUpdateEstadoEliminado
        {
            get { return base.GetSqlXml("UpdateEstadoEliminado"); }
        }
        #endregion

        #region INTERVENCIONES       
        public string SqlEnviar
        {
            get { return base.GetSqlXml("Enviar"); }
        }

        public string SqlListMensajeIntervencion
        {
            get { return base.GetSqlXml("ListMensajeIntervencion"); }
        }

        public string SqlRptConsultasMensajes
        {
            get { return base.GetSqlXml("RptConsultasMensajes"); }
        }

        public string SqlBusquedaSiMensajesIntervencion
        {
            get { return base.GetSqlXml("BusquedaSiMensajesIntervencion"); }
        }

        #endregion
    }
}
