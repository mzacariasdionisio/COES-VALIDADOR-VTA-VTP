using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_INFORMEFALLA_N2
    /// </summary>
    public class EveInformefallaN2Helper : HelperBase
    {
        public EveInformefallaN2Helper(): base(Consultas.EveInformefallaN2Sql)
        {
        }

        public EveInformefallaN2DTO Create(IDataReader dr)
        {
            EveInformefallaN2DTO entity = new EveInformefallaN2DTO();

            int iEveninfn2codi = dr.GetOrdinal(this.Eveninfn2codi);
            if (!dr.IsDBNull(iEveninfn2codi)) entity.Eveninfn2codi = Convert.ToInt32(dr.GetValue(iEveninfn2codi));

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

            int iEvenanio = dr.GetOrdinal(this.Evenanio);
            if (!dr.IsDBNull(iEvenanio)) entity.Evenanio = Convert.ToInt32(dr.GetValue(iEvenanio));

            int iEvenn2corr = dr.GetOrdinal(this.Evenn2corr);
            if (!dr.IsDBNull(iEvenn2corr)) entity.Evenn2corr = Convert.ToInt32(dr.GetValue(iEvenn2corr));

            int iEveninfpin2fechemis = dr.GetOrdinal(this.Eveninfpin2fechemis);
            if (!dr.IsDBNull(iEveninfpin2fechemis)) entity.Eveninfpin2fechemis = dr.GetDateTime(iEveninfpin2fechemis);

            int iEveninfpin2emitido = dr.GetOrdinal(this.Eveninfpin2emitido);
            if (!dr.IsDBNull(iEveninfpin2emitido)) entity.Eveninfpin2emitido = dr.GetString(iEveninfpin2emitido);

            int iEveninfpin2elab = dr.GetOrdinal(this.Eveninfpin2elab);
            if (!dr.IsDBNull(iEveninfpin2elab)) entity.Eveninfpin2elab = dr.GetString(iEveninfpin2elab);

            int iEveninffn2emitido = dr.GetOrdinal(this.Eveninffn2emitido);
            if (!dr.IsDBNull(iEveninffn2emitido)) entity.Eveninffn2emitido = dr.GetString(iEveninffn2emitido);

            int iEveninffn2elab = dr.GetOrdinal(this.Eveninffn2elab);
            if (!dr.IsDBNull(iEveninffn2elab)) entity.Eveninffn2elab = dr.GetString(iEveninffn2elab);

            int iEveninfn2lastuser = dr.GetOrdinal(this.Eveninfn2lastuser);
            if (!dr.IsDBNull(iEveninfn2lastuser)) entity.Eveninfn2lastuser = dr.GetString(iEveninfn2lastuser);

            int iEveninfn2lastdate = dr.GetOrdinal(this.Eveninfn2lastdate);
            if (!dr.IsDBNull(iEveninfn2lastdate)) entity.Eveninfn2lastdate = dr.GetDateTime(iEveninfn2lastdate);

            int iEveninffn2fechemis = dr.GetOrdinal(this.Eveninffn2fechemis);
            if (!dr.IsDBNull(iEveninffn2fechemis)) entity.Eveninffn2fechemis = dr.GetDateTime(iEveninffn2fechemis);

            int iEvenipiEN2emitido = dr.GetOrdinal(this.EvenipiEN2emitido);
            if (!dr.IsDBNull(iEvenipiEN2emitido)) entity.EvenipiEN2emitido = dr.GetString(iEvenipiEN2emitido);

            int iEvenipiEN2elab = dr.GetOrdinal(this.EvenipiEN2elab);
            if (!dr.IsDBNull(iEvenipiEN2elab)) entity.EvenipiEN2elab = dr.GetString(iEvenipiEN2elab);

            int iEvenipiEN2fechem = dr.GetOrdinal(this.EvenipiEN2fechem);
            if (!dr.IsDBNull(iEvenipiEN2fechem)) entity.EvenipiEN2fechem = dr.GetDateTime(iEvenipiEN2fechem);

            int iEvenifEN2emitido = dr.GetOrdinal(this.EvenifEN2emitido);
            if (!dr.IsDBNull(iEvenifEN2emitido)) entity.EvenifEN2emitido = dr.GetString(iEvenifEN2emitido);

            int iEvenifEN2elab = dr.GetOrdinal(this.EvenifEN2elab);
            if (!dr.IsDBNull(iEvenifEN2elab)) entity.EvenifEN2elab = dr.GetString(iEvenifEN2elab);

            int iEvenifEN2fechem = dr.GetOrdinal(this.EvenifEN2fechem);
            if (!dr.IsDBNull(iEvenifEN2fechem)) entity.EvenifEN2fechem = dr.GetDateTime(iEvenifEN2fechem);
           

            return entity;
        }


        #region Mapeo de Campos

        public string Eveninfn2codi = "EVENINFN2CODI";
        public string Evencodi = "EVENCODI";
        public string Evenanio = "EVENANIO";
        public string Evenn2corr = "EVENN2CORR";
        public string Eveninfpin2fechemis = "EVENINFPIN2FECHEMIS";
        public string Eveninfpin2emitido = "EVENINFPIN2EMITIDO";
        public string Eveninfpin2elab = "EVENINFPIN2ELAB";
        public string Eveninffn2emitido = "EVENINFFN2EMITIDO";
        public string Eveninffn2elab = "EVENINFFN2ELAB";
        public string Eveninfn2lastuser = "EVENINFN2LASTUSER";
        public string Eveninfn2lastdate = "EVENINFN2LASTDATE";
        public string Eveninffn2fechemis = "EVENINFFN2FECHEMIS";
        public string EvenipiEN2emitido = "EVENIPI_E_N2EMITIDO";
        public string EvenipiEN2elab = "EVENIPI_E_N2ELAB";
        public string EvenipiEN2fechem = "EVENIPI_E_N2FECHEM";
        public string EvenifEN2emitido = "EVENIF_E_N2EMITIDO";
        public string EvenifEN2elab = "EVENIF_E_N2ELAB";
        public string EvenifEN2fechem = "EVENIF_E_N2FECHEM";
        public string Emprnomb = "EMPRNOMB";
        public string Tareaabrev = "TAREAABREV";
        public string Areanomb = "AREANOMB";
        public string Famabrev = "FAMABREV";
        public string Equiabrev = "EQUIABREV";
        public string Evenmwindisp = "EVENMWINDISP";
        public string Evenini = "EVENINI";
        public string ObsPrelimIni = "OBSPRELIMINI";
        public string ObsFinal = "OBSFINAL";
        public string Eveninfplazodiasipi = "EVENINFPLAZODIASIPI";
        public string Eveninfplazodiasif = "EVENINFPLAZODIASIF";
        public string Eveninfplazohoraipi = "EVENINFPLAZOHORAIPI";
        public string Eveninfplazohoraif = "EVENINFPLAZOHORAIF";
        public string Eveninfplazominipi = "EVENINFPLAZOMINIPI";
        public string Eveninfplazominif = "EVENINFPLAZOMINIF";
        public string SqlValidarInformeFallaN2
        {
            get { return base.GetSqlXml("ValidarInformeFallaN2"); }
        }

        public string SqlEliminarInformeFallaN2
        {
            get { return base.GetSqlXml("EliminarInformeFallaN2"); }
        }

        public string SqlObtenerCorrelativoInformeFallaN2
        {
            get { return base.GetSqlXml("ObtenerCorrelativoInformeFallaN2"); }
        }

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string SqlSaveEvento
        {
            get { return base.GetSqlXml("SaveEvento"); }
        }

        public string SqlListarEventoInformeFallaN2
        {
            get { return base.GetSqlXml("MostrarEventoInformeFallaN2"); }
        }

        public string SqlActualizarAmpliacion
        {
            get { return base.GetSqlXml("ActualizarAmpliacion"); }
        }
        #endregion
    }
}
