using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_MAILS
    /// </summary>
    public class EveMailsHelper : HelperBase
    {
        public EveMailsHelper(): base(Consultas.EveMailsSql)
        {
        }

        public EveMailsDTO Create(IDataReader dr)
        {
            EveMailsDTO entity = new EveMailsDTO();

            int iMailcodi = dr.GetOrdinal(this.Mailcodi);
            if (!dr.IsDBNull(iMailcodi)) entity.Mailcodi = Convert.ToInt32(dr.GetValue(iMailcodi));

            int iMailturnonum = dr.GetOrdinal(this.Mailturnonum);
            if (!dr.IsDBNull(iMailturnonum)) entity.Mailturnonum = Convert.ToInt32(dr.GetValue(iMailturnonum));

            int iMailreprogcausa = dr.GetOrdinal(this.Mailreprogcausa);
            if (!dr.IsDBNull(iMailreprogcausa)) entity.Mailreprogcausa = dr.GetString(iMailreprogcausa);

            int iMailcheck1 = dr.GetOrdinal(this.Mailcheck1);
            if (!dr.IsDBNull(iMailcheck1)) entity.Mailcheck1 = dr.GetString(iMailcheck1);

            int iMailhoja = dr.GetOrdinal(this.Mailhoja);
            if (!dr.IsDBNull(iMailhoja)) entity.Mailhoja = dr.GetString(iMailhoja);

            int iMailprogramador = dr.GetOrdinal(this.Mailprogramador);
            if (!dr.IsDBNull(iMailprogramador)) entity.Mailprogramador = dr.GetString(iMailprogramador);

            int iMailbloquehorario = dr.GetOrdinal(this.Mailbloquehorario);
            if (!dr.IsDBNull(iMailbloquehorario)) entity.Mailbloquehorario = Convert.ToInt32(dr.GetValue(iMailbloquehorario));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iMailfecha = dr.GetOrdinal(this.Mailfecha);
            if (!dr.IsDBNull(iMailfecha)) entity.Mailfecha = dr.GetDateTime(iMailfecha);

            int iMailcheck2 = dr.GetOrdinal(this.Mailcheck2);
            if (!dr.IsDBNull(iMailcheck2)) entity.Mailcheck2 = dr.GetString(iMailcheck2);

            int iMailemitido = dr.GetOrdinal(this.Mailemitido);
            if (!dr.IsDBNull(iMailemitido)) entity.Mailemitido = dr.GetString(iMailemitido);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iMailfechaini = dr.GetOrdinal(this.Mailfechaini);
            if (!dr.IsDBNull(iMailfechaini)) entity.Mailfechaini = dr.GetDateTime(iMailfechaini);

            int iMailfechafin = dr.GetOrdinal(this.Mailfechafin);
            if (!dr.IsDBNull(iMailfechafin)) entity.Mailfechafin = dr.GetDateTime(iMailfechafin);

            int iLastuserproc = dr.GetOrdinal(this.Lastuserproc);
            if (!dr.IsDBNull(iLastuserproc)) entity.Lastuserproc = dr.GetString(iLastuserproc);

            int iMailespecialista = dr.GetOrdinal(this.Mailespecialista);
            if (!dr.IsDBNull(iMailespecialista)) entity.Mailespecialista = dr.GetString(iMailespecialista);

            int iMailtipoprograma = dr.GetOrdinal(this.Mailtipoprograma);
            if (!dr.IsDBNull(iMailtipoprograma)) entity.Mailtipoprograma = Convert.ToInt32(dr.GetValue(iMailtipoprograma));

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iMailhora = dr.GetOrdinal(this.Mailhora);
            if (!dr.IsDBNull(iMailhora)) entity.Mailhora = dr.GetDateTime(iMailhora);

            int iMailconsecuencia = dr.GetOrdinal(this.Mailconsecuencia);
            if (!dr.IsDBNull(iMailconsecuencia)) entity.Mailconsecuencia = dr.GetString(iMailconsecuencia);

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iCoordinador = dr.GetOrdinal(this.CoordinadorTurno);
            if (!dr.IsDBNull(iCoordinador)) entity.CoordinadorTurno = dr.GetString(iCoordinador);

            return entity;
        }


        #region Mapeo de Campos

        public string Mailcodi = "MAILCODI";
        public string Mailturnonum = "MAILTURNONUM";
        public string Mailreprogcausa = "MAILREPROGCAUSA";
        public string Mailcheck1 = "MAILCHECK1";
        public string Mailhoja = "MAILHOJA";
        public string Mailprogramador = "MAILPROGRAMADOR";
        public string Mailbloquehorario = "MAILBLOQUEHORARIO";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Mailfecha = "MAILFECHA";
        public string Mailcheck2 = "MAILCHECK2";
        public string Mailemitido = "MAILEMITIDO";
        public string Equicodi = "EQUICODI";
        public string Mailfechaini = "MAILFECHAINI";
        public string Mailfechafin = "MAILFECHAFIN";
        public string Lastuserproc = "LASTUSERPROC";
        public string Mailespecialista = "MAILESPECIALISTA";
        public string Mailcoordinador = "MAILCOORDINADOR";
        public string Mailtipoprograma = "MAILTIPOPROGRAMA";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Subcausadesc = "SUBCAUSADESC";
        public string T = "T";
        public string Mailhora = "MAILHORA";
        public string Mailconsecuencia = "MAILCONSECUENCIA";
        public string Topcodi = "TOPCODI";
        public string CoordinadorTurno = "MAILCOORDINADOR";

        #endregion

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string ObtenerListadoReProgramasPorFecha
        {
            get { return base.GetSqlXml("ObtenerListadoReProgramasPorFecha"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string SqlExportarEnvioCorreos
        {
            get { return base.GetSqlXml("ExportarEnvioCorreos"); }
        }

        #region "COSTO OPORTUNIDAD"
        public string SqlGetListaReprogramado
        {
            get { return base.GetSqlXml("GetListaReprogramado"); }
        }

        
        public string SqlGetFechaMaxProgramaEmitido
        {
            get { return base.GetSqlXml("GetFechaMaxProgramaEmitido"); }
        }

        #endregion
    }
}
