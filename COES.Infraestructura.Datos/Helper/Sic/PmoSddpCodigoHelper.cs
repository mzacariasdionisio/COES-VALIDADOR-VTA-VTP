using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PMO_SDDP_CODIGO
    /// </summary>
    public class PmoSddpCodigoHelper : HelperBase
    {
        public PmoSddpCodigoHelper() : base(Consultas.PmoSddpCodigoSql)
        {
        }

        public PmoSddpCodigoDTO Create(IDataReader dr)
        {
            PmoSddpCodigoDTO entity = new PmoSddpCodigoDTO();

            int iTsddpcodi = dr.GetOrdinal(this.Tsddpcodi);
            if (!dr.IsDBNull(iTsddpcodi)) entity.Tsddpcodi = Convert.ToInt32(dr.GetValue(iTsddpcodi));

            int iSddpcodi = dr.GetOrdinal(this.Sddpcodi);
            if (!dr.IsDBNull(iSddpcodi)) entity.Sddpcodi = Convert.ToInt32(dr.GetValue(iSddpcodi));

            int iSddpnum = dr.GetOrdinal(this.Sddpnum);
            if (!dr.IsDBNull(iSddpnum)) entity.Sddpnum = Convert.ToInt32(dr.GetValue(iSddpnum));

            int iSddpnomb = dr.GetOrdinal(this.Sddpnomb);
            if (!dr.IsDBNull(iSddpnomb)) entity.Sddpnomb = dr.GetString(iSddpnomb);

            int iSddpestado = dr.GetOrdinal(this.Sddpestado);
            if (!dr.IsDBNull(iSddpestado)) entity.Sddpestado = dr.GetString(iSddpestado);

            int iSddpdesc = dr.GetOrdinal(this.Sddpdesc);
            if (!dr.IsDBNull(iSddpdesc)) entity.Sddpdesc = dr.GetString(iSddpdesc);

            int iSddpcomentario = dr.GetOrdinal(this.Sddpcomentario);
            if (!dr.IsDBNull(iSddpcomentario)) entity.Sddpcomentario = dr.GetString(iSddpcomentario);

            int iSddpusucreacion = dr.GetOrdinal(this.Sddpusucreacion);
            if (!dr.IsDBNull(iSddpusucreacion)) entity.Sddpusucreacion = dr.GetString(iSddpusucreacion);

            int iSddpfeccreacion = dr.GetOrdinal(this.Sddpfeccreacion);
            if (!dr.IsDBNull(iSddpfeccreacion)) entity.Sddpfeccreacion = dr.GetDateTime(iSddpfeccreacion);

            int iSddpusumodificacion = dr.GetOrdinal(this.Sddpusumodificacion);
            if (!dr.IsDBNull(iSddpusumodificacion)) entity.Sddpusumodificacion = dr.GetString(iSddpusumodificacion);

            int iSddpfecmodificacion = dr.GetOrdinal(this.Sddpfecmodificacion);
            if (!dr.IsDBNull(iSddpfecmodificacion)) entity.Sddpfecmodificacion = dr.GetDateTime(iSddpfecmodificacion);

            int iTptomedicodi = dr.GetOrdinal(this.Tptomedicodi);
            if (!dr.IsDBNull(iTptomedicodi)) entity.Tptomedicodi = Convert.ToInt32(dr.GetValue(iTptomedicodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Tsddpcodi = "TSDDPCODI";
        public string Sddpcodi = "SDDPCODI";
        public string Sddpnum = "SDDPNUM";
        public string Sddpnomb = "SDDPNOMB";
        public string Sddpestado = "SDDPESTADO";
        public string Sddpdesc = "SDDPDESC";
        public string Sddpcomentario = "SDDPCOMENTARIO";
        public string Sddpusucreacion = "SDDPUSUCREACION";
        public string Sddpfeccreacion = "SDDPFECCREACION";
        public string Sddpusumodificacion = "SDDPUSUMODIFICACION";
        public string Sddpfecmodificacion = "SDDPFECMODIFICACION";
        public string Tptomedicodi = "TPTOMEDICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Equicodi = "EQUICODI";
        public string Ptomedicodi = "PTOMEDICODI";

        public string Tsddpnomb = "TSDDPNOMB";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Ptomedielenomb = "PTOMEDIELENOMB";

        #endregion

        public string SqlGetByNumYTipo
        {
            get { return GetSqlXml("GetByNumYTipo"); }
        }
    }
}
