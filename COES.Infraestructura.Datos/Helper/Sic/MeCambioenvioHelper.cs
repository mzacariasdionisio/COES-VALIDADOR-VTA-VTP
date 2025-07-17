using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_CAMBIOENVIO
    /// </summary>
    public class MeCambioenvioHelper : HelperBase
    {
        public MeCambioenvioHelper()
            : base(Consultas.MeCambioenvioSql)
        {
        }

        public MeCambioenvioDTO Create(IDataReader dr)
        {
            MeCambioenvioDTO entity = new MeCambioenvioDTO();

            int iCambenviocodi = dr.GetOrdinal(this.Camenviocodi);
            if (!dr.IsDBNull(iCambenviocodi)) entity.Camenviocodi = Convert.ToInt32(dr.GetValue(iCambenviocodi));

            int iHojacodi = dr.GetOrdinal(this.Hojacodi);
            if (!dr.IsDBNull(iHojacodi)) entity.Hojacodi = Convert.ToInt32(dr.GetValue(iHojacodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iEnviocodi = dr.GetOrdinal(this.Enviocodi);
            if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));

            int iCambenvfecha = dr.GetOrdinal(this.Cambenvfecha);
            if (!dr.IsDBNull(iCambenvfecha)) entity.Cambenvfecha = dr.GetDateTime(iCambenvfecha);

            int iCambenvdatos = dr.GetOrdinal(this.Cambenvdatos);
            if (!dr.IsDBNull(iCambenvdatos)) entity.Cambenvdatos = dr.GetString(iCambenvdatos);

            int iCambenvcolvar = dr.GetOrdinal(this.Cambenvcolvar);
            if (!dr.IsDBNull(iCambenvcolvar)) entity.Cambenvcolvar = dr.GetString(iCambenvcolvar);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iTipoptomedicodi = dr.GetOrdinal(this.Tipoptomedicodi);
            if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(iTipoptomedicodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Ptomedicodi = "PTOMEDICODI";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Enviocodi = "ENVIOCODI";
        public string Cambenvfecha = "CAMENVFECHA";
        public string Cambenvdatos = "CAMENVDATOS";
        public string Cambenvcolvar = "CAMENVCOLVAR";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Ptomedibarranomb = "Ptomedibarranomb";
        public string Tipoinfoabrev = "Tipoinfoabrev";
        public string Enviofechaperiodo = "Enviofechaperiodo";
        public string Emprabrev = "EMPRABREV";
        public string Hojacodi = "HOJACODI";
        public string Camenviocodi = "CAMENVCODI";
        public string Tipoptomedicodi = "TPTOMEDICODI";
        public string Formatcodi = "FORMATCODI";

        #endregion

        public string SqlGetAllCambioEnvio
        {
            get { return base.GetSqlXml("GetAllCambioEnvio"); }
        }

        public string SqlGetAllOrigenEnvio
        {
            get { return base.GetSqlXml("GetAllOrigenEnvio"); }
        }

        public string SqlListByEnvio
        {
            get { return base.GetSqlXml("ListByEnvio"); }
        }
    }
}
