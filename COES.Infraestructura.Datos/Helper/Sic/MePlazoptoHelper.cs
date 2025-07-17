using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_PLAZOPTO
    /// </summary>
    public class MePlazoptoHelper : HelperBase
    {
        public MePlazoptoHelper() : base(Consultas.MePlazoptoSql)
        {
        }

        public MePlazoptoDTO Create(IDataReader dr)
        {
            MePlazoptoDTO entity = new MePlazoptoDTO();

            int iPlzptocodi = dr.GetOrdinal(this.Plzptocodi);
            if (!dr.IsDBNull(iPlzptocodi)) entity.Plzptocodi = Convert.ToInt32(dr.GetValue(iPlzptocodi));

            int iPlzptodiafinplazo = dr.GetOrdinal(this.Plzptodiafinplazo);
            if (!dr.IsDBNull(iPlzptodiafinplazo)) entity.Plzptodiafinplazo = Convert.ToInt32(dr.GetValue(iPlzptodiafinplazo));

            int iPlzptominfinplazo = dr.GetOrdinal(this.Plzptominfinplazo);
            if (!dr.IsDBNull(iPlzptominfinplazo)) entity.Plzptominfinplazo = Convert.ToInt32(dr.GetValue(iPlzptominfinplazo));

            int iPlzptofechavigencia = dr.GetOrdinal(this.Plzptofecvigencia);
            if (!dr.IsDBNull(iPlzptofechavigencia)) entity.Plzptofechavigencia = dr.GetDateTime(iPlzptofechavigencia);

            int iPlzptofecharegistro = dr.GetOrdinal(this.Plzptofeccreacion);
            if (!dr.IsDBNull(iPlzptofecharegistro)) entity.Plzptofecharegistro = dr.GetDateTime(iPlzptofecharegistro);

            int iPlzptominfila = dr.GetOrdinal(this.Plzptominfila);
            if (!dr.IsDBNull(iPlzptominfila)) entity.Plzptominfila = Convert.ToInt32(dr.GetValue(iPlzptominfila));

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Plzptocodi = "PLZPTOCODI";
        public string Plzptodiafinplazo = "PLZPTODIAFINPLAZO";
        public string Plzptominfinplazo = "PLZPTOMINFINPLAZO";
        public string Plzptofecvigencia = "PLZPTOFECVIGENCIA";
        public string Plzptofeccreacion = "PLZPTOFECCREACION";
        public string Plzptominfila = "PLZPTOMINFILA";
        public string Formatcodi = "FORMATCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Emprcodi = "EMPRCODI";
        public string Plzptousucreacion = "PLZPTOUSUCREACION";
        public string Plzptofecmodificacion = "PLZPTOFECMODIFICACION";
        public string Plzptousumodificacion = "PLZPTOUSUMODIFICACION";
        #endregion
    }
}
