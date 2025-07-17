using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.CortoPlazo.Modelo.Helper
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_ESCTOPOLOGIA
    /// </summary>
    public class CpTopologiaHelper : HelperBase
    {
        public CpTopologiaHelper()
            : base(Consultas.CpTopologiaSql)
        {
        }

        public CpTopologiaDTO Create(IDataReader dr)
        {
            CpTopologiaDTO entity = new CpTopologiaDTO();

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iTopnombre = dr.GetOrdinal(this.Topnombre);
            if (!dr.IsDBNull(iTopnombre)) entity.Topnombre = dr.GetString(iTopnombre);

            int iTopfecha = dr.GetOrdinal(this.Topfecha);
            if (!dr.IsDBNull(iTopfecha)) entity.Topfecha = dr.GetDateTime(iTopfecha);

            int iTopinicio = dr.GetOrdinal(this.Topinicio);
            if (!dr.IsDBNull(iTopinicio)) entity.Topinicio = Convert.ToInt16(dr.GetValue(iTopinicio));

            int iTophorizonte = dr.GetOrdinal(this.Tophorizonte);
            if (!dr.IsDBNull(iTophorizonte)) entity.Tophorizonte = Convert.ToInt16(dr.GetValue(iTophorizonte));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iTophora = dr.GetOrdinal(this.Tophora);
            if (!dr.IsDBNull(iTophora)) entity.Tophora = Convert.ToInt16(dr.GetValue(iTophora));

            int iToptipo = dr.GetOrdinal(this.Toptipo);
            if (!dr.IsDBNull(iToptipo)) entity.Toptipo = Convert.ToInt16(dr.GetValue(iToptipo));

            int iTopdiasproc = dr.GetOrdinal(this.Topdiasproc);
            if (!dr.IsDBNull(iTopdiasproc)) entity.Topdiasproc = Convert.ToInt16(dr.GetValue(iTopdiasproc));

            int iTopfinal = dr.GetOrdinal(this.Topfinal);
            if (!dr.IsDBNull(iTopfinal)) entity.Topfinal = Convert.ToInt16(dr.GetValue(iTopfinal));

            int iTopuserfinal = dr.GetOrdinal(this.Topuserfinal);
            if (!dr.IsDBNull(iTopuserfinal)) entity.Topuserfinal = dr.GetString(iTopuserfinal);

            int iTopfechafinal = dr.GetOrdinal(this.Topfechafinal);
            if (!dr.IsDBNull(iTopfechafinal)) entity.Topfechafinal = dr.GetDateTime(iTopfechafinal);

            int iTopusersicoes = dr.GetOrdinal(this.Topusersicoes);
            if (!dr.IsDBNull(iTopusersicoes)) entity.Topusersicoes = dr.GetString(iTopusersicoes);

            int iTopfechasicoes = dr.GetOrdinal(this.Topfechasicoes);
            if (!dr.IsDBNull(iTopfechasicoes)) entity.Topfechasicoes = dr.GetDateTime(iTopfechasicoes);

            int iTopiniciohora = dr.GetOrdinal(this.Topiniciohora);
            if (!dr.IsDBNull(iTopiniciohora)) entity.Topiniciohora = Convert.ToInt16(dr.GetValue(iTopiniciohora));

            int iTopsinrsf = dr.GetOrdinal(this.Topsinrsf);
            if (!dr.IsDBNull(iTopsinrsf)) entity.Topsinrsf = Convert.ToInt16(dr.GetValue(iTopsinrsf));

            int iTopfechadespacho = dr.GetOrdinal(this.Topfechadespacho);
            if (!dr.IsDBNull(iTopfechadespacho)) entity.Topfechadespacho = dr.GetDateTime(iTopfechadespacho);

            int iTopuserdespacho = dr.GetOrdinal(this.Topuserdespacho);
            if (!dr.IsDBNull(iTopuserdespacho)) entity.Topuserdespacho = dr.GetString(iTopuserdespacho);

            return entity;
        }


        #region Mapeo de Campos

        public string Topcodi = "TOPCODI";
        public string Topnombre = "TOPNOMBRE";
        public string Topfecha = "TOPFECHA";
        public string Topinicio = "TOPINICIO";
        public string Tophorizonte = "TOPHORIZONTE";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Tophora = "TOPHORA";
        public string Toptipo = "TOPTIPO";
        public string Topdiasproc = "TOPDIASPROC";

        public string Topfinal = "TOPFINAL";
        public string Topuserfinal = "TOPUSERFINAL";
        public string Topfechafinal = "TOPFECHAFINAL";
        public string Topusersicoes = "TOPUSERSICOES";
        public string Topfechasicoes = "TOPFECHASICOES";
        public string Topiniciohora = "TOPINICIOHORA";
        public string Topsinrsf = "TOPSINRSF";
        public string Topfechadespacho = "TOPFECHADESPACHO";
        public string Topuserdespacho = "TOPUSERDESPACHO";
        public string Fverscodi = "FVERSCODI";
        public string Avercodi = "AVERCODI";
        #endregion

        #region Sentencias sql

        public string SqlObtenerTopologiaFinal
        {
            get { return GetSqlXml("ObtenerTopologiaFinal"); }
        }

        public string SqlGetByIdRsf
        {
            get { return base.GetSqlXml("GetByIdRsf"); }
        }

        public string SqlListNombre
        {
            get { return base.GetSqlXml("ListNombre"); }
        }

        public string SqlListReprogramados
        {
            get { return base.GetSqlXml("ListReprogramados"); }
        }

        public string SqlObtenerEscenarioPorDia
        {
            get { return base.GetSqlXml("ObtenerEscenarioPorDia"); }
        }

        public string SqlObtenerEscenarioPorDiaConsulta
        {
            get { return base.GetSqlXml("ObtenerEscenarioPorDiaConsulta"); }
        }

        public string SqlObtenerTipoRestricciones
        {
            get { return base.GetSqlXml("ObtenerTipoRestricciones"); }
        }

        public string SqlObtenerUltimoEscenarioReprogramado
        {
            get { return base.GetSqlXml("ObtenerUltimoEscenarioReprogramado"); }
        }

        public string SqlListEscenarioReprograma
        {
            get { return base.GetSqlXml("ListEscenarioReprograma"); }
        }

        // Ticket IMME
        public string SqlListaTopFinalDiario
        {
            get { return base.GetSqlXml("ListaTopFinalDiario"); }
        }

        #endregion
        public string SqlObtenerTopologia
        {
            get { return base.GetSqlXml("ObtenerTopologia"); }
        }
    }
}
