using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_YUPCON_CFGDET
    /// </summary>
    public class CpYupconCfgdetHelper : HelperBase
    {
        public CpYupconCfgdetHelper() : base(Consultas.CpYupconCfgdetSql)
        {
        }

        public CpYupconCfgdetDTO Create(IDataReader dr)
        {
            CpYupconCfgdetDTO entity = new CpYupconCfgdetDTO();

            int iRecurcodi = dr.GetOrdinal(this.Recurcodi);
            if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iYupcfgcodi = dr.GetOrdinal(this.Yupcfgcodi);
            if (!dr.IsDBNull(iYupcfgcodi)) entity.Yupcfgcodi = Convert.ToInt32(dr.GetValue(iYupcfgcodi));

            int iYcdetcodi = dr.GetOrdinal(this.Ycdetcodi);
            if (!dr.IsDBNull(iYcdetcodi)) entity.Ycdetcodi = Convert.ToInt32(dr.GetValue(iYcdetcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iYcdetfactor = dr.GetOrdinal(this.Ycdetfactor);
            if (!dr.IsDBNull(iYcdetfactor)) entity.Ycdetfactor = dr.GetDecimal(iYcdetfactor);

            int iYcdetactivo = dr.GetOrdinal(this.Ycdetactivo);
            if (!dr.IsDBNull(iYcdetactivo)) entity.Ycdetactivo = Convert.ToInt32(dr.GetValue(iYcdetactivo));

            int iYcdetusuregistro = dr.GetOrdinal(this.Ycdetusuregistro);
            if (!dr.IsDBNull(iYcdetusuregistro)) entity.Ycdetusuregistro = dr.GetString(iYcdetusuregistro);

            int iYcdetusumodificacion = dr.GetOrdinal(this.Ycdetusumodificacion);
            if (!dr.IsDBNull(iYcdetusumodificacion)) entity.Ycdetusumodificacion = dr.GetString(iYcdetusumodificacion);

            int iYcdetfecregistro = dr.GetOrdinal(this.Ycdetfecregistro);
            if (!dr.IsDBNull(iYcdetfecregistro)) entity.Ycdetfecregistro = dr.GetDateTime(iYcdetfecregistro);

            int iYcdetfecmodificacion = dr.GetOrdinal(this.Ycdetfecmodificacion);
            if (!dr.IsDBNull(iYcdetfecmodificacion)) entity.Ycdetfecmodificacion = dr.GetDateTime(iYcdetfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Recurcodi = "RECURCODI";
        public string Topcodi = "TOPCODI";
        public string Yupcfgcodi = "YUPCFGCODI";
        public string Ycdetcodi = "YCDETCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Ycdetfactor = "YCDETFACTOR";
        public string Ycdetactivo = "YCDETACTIVO";
        public string Ycdetusuregistro = "YCDETUSUREGISTRO";
        public string Ycdetusumodificacion = "YCDETUSUMODIFICACION";
        public string Ycdetfecregistro = "YCDETFECREGISTRO";
        public string Ycdetfecmodificacion = "YCDETFECMODIFICACION";

        public string Famcodi = "FAMCODI";
        public string Ptomedielenomb = "PTOMEDIELENOMB";
        public string Recurnombre = "RECURNOMBRE";

        #endregion
    }
}
