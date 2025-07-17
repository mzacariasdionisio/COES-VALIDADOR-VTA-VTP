using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_HORAOPERACION
    /// </summary>
    public class EveHoraoperacionHelper : HelperBase
    {
        public EveHoraoperacionHelper()
            : base(Consultas.EveHoraoperacionSql)
        {
        }

        public EveHoraoperacionDTO Create(IDataReader dr)
        {
            EveHoraoperacionDTO entity = new EveHoraoperacionDTO();

            int iHopcodi = dr.GetOrdinal(this.Hopcodi);
            if (!dr.IsDBNull(iHopcodi)) entity.Hopcodi = Convert.ToInt32(dr.GetValue(iHopcodi));

            int iHophorini = dr.GetOrdinal(this.Hophorini);
            if (!dr.IsDBNull(iHophorini)) entity.Hophorini = dr.GetDateTime(iHophorini);

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iHophorfin = dr.GetOrdinal(this.Hophorfin);
            if (!dr.IsDBNull(iHophorfin)) entity.Hophorfin = dr.GetDateTime(iHophorfin);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iHopdesc = dr.GetOrdinal(this.Hopdesc);
            if (!dr.IsDBNull(iHopdesc)) entity.Hopdesc = dr.GetString(iHopdesc);

            int iHophorordarranq = dr.GetOrdinal(this.Hophorordarranq);
            if (!dr.IsDBNull(iHophorordarranq)) entity.Hophorordarranq = dr.GetDateTime(iHophorordarranq);

            int iHophorparada = dr.GetOrdinal(this.Hophorparada);
            if (!dr.IsDBNull(iHophorparada)) entity.Hophorparada = dr.GetDateTime(iHophorparada);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iHopsaislado = dr.GetOrdinal(this.Hopsaislado);
            if (!dr.IsDBNull(iHopsaislado)) entity.Hopsaislado = Convert.ToInt32(dr.GetValue(iHopsaislado));

            int iHoplimtrans = dr.GetOrdinal(this.Hoplimtrans);
            if (!dr.IsDBNull(iHoplimtrans)) entity.Hoplimtrans = dr.GetString(iHoplimtrans);

            int iHopfalla = dr.GetOrdinal(this.Hopfalla);
            if (!dr.IsDBNull(iHopfalla)) entity.Hopfalla = dr.GetString(iHopfalla);

            int iHopcompordarrq = dr.GetOrdinal(this.Hopcompordarrq);
            if (!dr.IsDBNull(iHopcompordarrq)) entity.Hopcompordarrq = dr.GetString(iHopcompordarrq);

            int iHopcompordpard = dr.GetOrdinal(this.Hopcompordpard);
            if (!dr.IsDBNull(iHopcompordpard)) entity.Hopcompordpard = dr.GetString(iHopcompordpard);

            int iHopcausacodi = dr.GetOrdinal(this.Hopcausacodi);
            if (!dr.IsDBNull(iHopcausacodi)) entity.Hopcausacodi = Convert.ToInt32(dr.GetValue(iHopcausacodi));

            int iHopcodipadre = dr.GetOrdinal(this.Hopcodipadre);
            if (!dr.IsDBNull(iHopcodipadre)) entity.Hopcodipadre = Convert.ToInt32(dr.GetValue(iHopcodipadre));

            int iHopestado = dr.GetOrdinal(this.Hopestado);
            if (!dr.IsDBNull(iHopestado)) entity.Hopestado = dr.GetString(iHopestado);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            #region EMS

            int iHopnotifuniesp = dr.GetOrdinal(this.Hopnotifuniesp);
            if (!dr.IsDBNull(iHopnotifuniesp)) entity.Hopnotifuniesp = Convert.ToInt32(dr.GetValue(iHopnotifuniesp));

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

            int iHopobs = dr.GetOrdinal(this.Hopobs);
            if (!dr.IsDBNull(iHopobs)) entity.Hopobs = dr.GetString(iHopobs);

            #endregion

            int iHoparrqblackstart = dr.GetOrdinal(this.Hoparrqblackstart);
            if (!dr.IsDBNull(iHoparrqblackstart)) entity.Hoparrqblackstart = dr.GetString(iHoparrqblackstart);

            int iHopensayope = dr.GetOrdinal(this.Hopensayope);
            if (!dr.IsDBNull(iHopensayope)) entity.Hopensayope = dr.GetString(iHopensayope);

            int iHopensayopmin = dr.GetOrdinal(this.Hopensayopmin);
            if (!dr.IsDBNull(iHopensayopmin)) entity.Hopensayopmin = dr.GetString(iHopensayopmin);

            return entity;
        }


        #region Mapeo de Campos

        public string Hopcodi = "HOPCODI";
        public string Hophorini = "HOPHORINI";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Hophorfin = "HOPHORFIN";
        public string Equicodi = "EQUICODI";
        public string Hopdesc = "HOPDESC";
        public string Hophorordarranq = "HOPHORORDARRANQ";
        public string Hophorparada = "HOPHORPARADA";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Grupocodi = "GRUPOCODI";
        public string Hopsaislado = "HOPSAISLADO";
        public string Hoplimtrans = "HOPLIMTRANS";
        public string Hopfalla = "HOPFALLA";
        public string Hopcompordarrq = "HOPCOMPORDARRQ";
        public string Hopcompordpard = "HOPCOMPORDPARD";
        public string Hopcausacodi = "HOPCAUSACODI";
        public string Unidad = "UNIDAD";
        public string Gruponomb = "GRUPONOMB";
        public string Grupopadre = "GRUPOPADRE";
        public string Emprnomb = "EMPRNOMB";
        public string Subcausadesc = "SUBCAUSADESC";
        public string Equiponombre = "EQUIPONOMBRE";
        public string Padrenombre = "PADRENOMBRE";
        public string Hopcodipadre = "HOPCODIPADRE";
        public string Hopestado = "HOPESTADO";
        public string Grupourspadre = "GRUPOURSPADRE";
        public string Fenergcodi = "FENERGCODI";
        public string Fenergnomb = "FENERGNOMB";
        public string Fenercolor = "FENERCOLOR";
        public string Central = "CENTRAL";
        public string Equipadre = "EQUIPADRE";
        public string Emprcodi = "EMPRCODI";
        public string Hopnotifuniesp = "HOPNOTIFUNIESP";
        public string Evencodi = "EVENCODI";
        public string FlagTipoHo = "FLAGTIPOHO";
        public string Hopobs = "HOPOBS";
        public string Hoparrqblackstart = "HOPARRQBLACKSTART";
        public string Hopensayope = "HOPENSAYOPE";
        public string Hopensayopmin = "HOPENSAYOPMIN";
        public string HopPruebaExitosa = "HOPPRUEBAEXITOSA";

        public string Codipadre = "CODIPADRE";
        public string Grupotipomodo = "GRUPOTIPOMODO";

        #region MigracionSGOCOES-GrupoB
        public string Areanomb = "Areanomb";
        public string Areacodi = "Areacodi";
        public string Equiabrev = "Equiabrev";
        public string Ptomedicodi = "Ptomedicodi";
        public string Grupocomb = "GRUPOCOMB";
        public string Emprabrev = "Emprabrev";
        #endregion

        #region SIOSEIN
        public string Osinergcodi = "OSINERGCODI";
        #endregion

        #endregion
        #region PR5
        public string Grupoabrev = "GRUPOABREV";
        public string Famcodi = "FAMCODI";
        public string Famnomb = "FAMNOMB";
        #endregion

        public string SqlGetByDetalleHO
        {
            get { return base.GetSqlXml("GetByDetalleHO"); }
        }

        public string SqlGetByCriteriaXEmpresaxFecha
        {
            get { return base.GetSqlXml("GetByCriteriaXEmpresaxFecha"); }
        }

        public string SqlGetByCriteriaUnidadesXEmpresaxFecha
        {
            get { return base.GetSqlXml("GetByCriteriaUnidadesXEmpresaxFecha"); }
        }

        public string SqlListEquiposHorasOperacionxFormato
        {
            get { return base.GetSqlXml("ListEquiposHorasOperacionxFormato"); }
        }

        public string SqlListarHorasOperacxEmpresaxFechaxTipoOP
        {
            get { return base.GetSqlXml("ListarHorasOperacxEmpresaxFechaxTipoOP"); }
        }

        public string SqlListarHorasOperacxEquiposXEmpXTipoOPxFam
        {
            get { return base.GetSqlXml("ListarHorasOperacxEquiposXEmpXTipoOPxFam"); }
        }

        public string SqlListarHorasOperacxEquiposXEmpXTipoOPxFam2
        {
            get { return base.GetSqlXml("ListarHorasOperacxEquiposXEmpXTipoOPxFam2"); }
        }

        public string SqlGetCriteriaxPKCodis
        {
            get { return base.GetSqlXml("GetCriteriaxPKCodis"); }
        }

        public string SqlGetCriteriaUnidadesxPKCodis
        {
            get { return base.GetSqlXml("GetCriteriaUnidadesxPKCodis"); }
        }

        public string SqlGetHorasURS
        {
            get { return base.GetSqlXml("GetHorasURS"); }
        }

        #region EMS
        public string SqlListarHorasOperacionByCriteria
        {
            get { return base.GetSqlXml("ListarHorasOperacionByCriteria"); }
        }

        public string SqlListarHorasOperacionByCriteriaUnidades
        {
            get { return base.GetSqlXml("ListarHorasOperacionByCriteriaUnidades"); }
        }
        #endregion

        #region MigracionSGOCOES-GrupoB
        public string SqlListaEstadoOperacion
        {
            get { return base.GetSqlXml("ListaEstadoOperacion"); }
        }
        public string SqlListaEstadoOperacion90
        {
            get { return base.GetSqlXml("ListaEstadoOperacion90"); }
        }

        public string SqlListaProdTipCombustible
        {
            get { return base.GetSqlXml("ListaProdTipCombustible"); }
        }

        public string SqlListaOperacionTension
        {
            get { return base.GetSqlXml("ListaOperacionTension"); }
        }

        public string SqlListaLineasDesconectadas
        {
            get { return base.GetSqlXml("ListaLineasDesconectadas"); }
        }
        #endregion

        #region Numerales Datos Base
        public string Equinomb = "EQUINOMB";
        public string Osicodi = "OSICODI";
        public string Dia = "DIA";

        public string Osigrupocodi = "OSIGRUPOCODI";


        public string SqlDatosBase_5_1_2
        {
            get { return base.GetSqlXml("ListaDatosBase_5_1_2"); }
        }
        public string SqlDatosBase_5_6_2
        {
            get { return base.GetSqlXml("ListaDatosBase_5_6_2"); }
        }
        #endregion

        #region Mejoras CMgN

        public string SqlHorasOperacionComparativoCM
        {
            get { return base.GetSqlXml("HorasOperacionComparativoCM"); }
        }

        #endregion
    }
}
