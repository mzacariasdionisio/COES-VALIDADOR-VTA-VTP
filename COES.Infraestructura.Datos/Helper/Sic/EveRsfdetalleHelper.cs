using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_RSFDETALLE
    /// </summary>
    public class EveRsfdetalleHelper : HelperBase
    {
        public EveRsfdetalleHelper(): base(Consultas.EveRsfdetalleSql)
        {
        }

        public EveRsfdetalleDTO Create(IDataReader dr)
        {
            EveRsfdetalleDTO entity = new EveRsfdetalleDTO();

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iRsfhorcodi = dr.GetOrdinal(this.Rsfhorcodi);
            if (!dr.IsDBNull(iRsfhorcodi)) entity.Rsfhorcodi = Convert.ToInt32(dr.GetValue(iRsfhorcodi));

            int iRsfdetcodi = dr.GetOrdinal(this.Rsfdetcodi);
            if (!dr.IsDBNull(iRsfdetcodi)) entity.Rsfdetcodi = Convert.ToInt32(dr.GetValue(iRsfdetcodi));

            int iRsfdetvalman = dr.GetOrdinal(this.Rsfdetvalman);
            if (!dr.IsDBNull(iRsfdetvalman)) entity.Rsfdetvalman = dr.GetDecimal(iRsfdetvalman);

            int iRsfdetvalaut = dr.GetOrdinal(this.Rsfdetvalaut);
            if (!dr.IsDBNull(iRsfdetvalaut)) entity.Rsfdetvalaut = dr.GetDecimal(iRsfdetvalaut);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iRsfdetindope = dr.GetOrdinal(this.Rsfdetindope);
            if (!dr.IsDBNull(iRsfdetindope)) entity.Rsfdetindope = dr.GetString(iRsfdetindope);

            int iRsfdetsub = dr.GetOrdinal(this.Rsfdetsub);
            if (!dr.IsDBNull(iRsfdetsub)) entity.Rsfdetsub = dr.GetDecimal(iRsfdetsub);

            int iRsfdetbaj = dr.GetOrdinal(this.Rsfdetbaj);
            if (!dr.IsDBNull(iRsfdetbaj)) entity.Rsfdetbaj = dr.GetDecimal(iRsfdetbaj);

            int iRsfdetdesp = dr.GetOrdinal(this.Rsfdetdesp);
            if (!dr.IsDBNull(iRsfdetdesp)) entity.Rsfdetdesp = dr.GetDecimal(iRsfdetdesp);

            int iRsfdetload = dr.GetOrdinal(this.Rsfdetload);
            if (!dr.IsDBNull(iRsfdetload)) entity.Rsfdetload = dr.GetDecimal(iRsfdetload);

            int iRsfdetmingen = dr.GetOrdinal(this.Rsfdetmingen);
            if (!dr.IsDBNull(iRsfdetmingen)) entity.Rsfdetmingen = dr.GetDecimal(iRsfdetmingen);

            int iRsfdetmaxgen = dr.GetOrdinal(this.Rsfdetmaxgen);
            if (!dr.IsDBNull(iRsfdetmaxgen)) entity.Rsfdetmaxgen = dr.GetDecimal(iRsfdetmaxgen);

            return entity;
        }


        #region Mapeo de Campos

        public string Grupocodi = "GRUPOCODI";
        public string Rsfhorcodi = "RSFHORCODI";
        public string Rsfdetcodi = "RSFDETCODI";
        public string Rsfdetvalman = "RSFDETVALMAN";
        public string Rsfdetvalaut = "RSFDETVALAUT";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Ursnomb = "URSNOMB";
        public string Gruponomb = "GRUPONOMB";
        public string Emprnomb = "EMPRNOMB";
        public string Grupotipo = "GRUPOTIPO";
        public string Equicodi = "EQUICODI";
        public string Famcodi = "FAMCODI";
        public string Equipadre = "EQUIPADRE";
        public string Rsfdetindope = "RSFDETINDOPE";
        public string Rsfdetsub = "RSFDETSUB";
        public string Rsfdetbaj = "RSFDETBAJ";
        public string Rsfdetdesp = "RSFDETDESP";
        public string Rsfdetload = "RSFDETLOAD";
        public string Rsfdetmingen = "RSFDETMINGEN";
        public string Rsfdetmaxgen = "RSFDETMAXGEN";
        public string Emprcodi = "EMPRCODI";

        public string SqlObtenerConfiguracion
        {
            get { return base.GetSqlXml("ObtenerConfiguracion"); }
        }
        #endregion

        #region "COSTO OPORTUNIDAD"
        public string SqlObtenerConfiguracionCO
        {
            get { return base.GetSqlXml("ObtenerConfiguracionCO"); }
        }
        public string SqlObtenerDetalleFrecuencia
        {
            get { return base.GetSqlXml("ObtenerDetalleFrecuencia"); }
        }

        public string SqlObtenerUnidadesRSF
        {
            get { return base.GetSqlXml("ObtenerUnidadesRSF"); }
        }

        #endregion

        public string SqlDeletePorId
        {
            get { return base.GetSqlXml("DeletePorId"); }
        }
        public string SqlUpdate2
        {
            get { return GetSqlXml("Update2"); }
        }

        #region Modificación_RSF_05012021

        public string SqlObtenerDetalleXML
        {
            get { return base.GetSqlXml("ObtenerDetalleXML"); }

        }

        #endregion
    }
}
