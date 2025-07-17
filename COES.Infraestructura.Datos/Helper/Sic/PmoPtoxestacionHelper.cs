using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PMO_PTOXESTACION
    /// </summary>
    public class PmoPtoxestacionHelper : HelperBase
    {
        public PmoPtoxestacionHelper(): base(Consultas.PmoPtoxestacionSql)
        {
        }

        public PmoPtoxestacionDTO Create(IDataReader dr)
        {
            PmoPtoxestacionDTO entity = new PmoPtoxestacionDTO();

            int iPmehcodi = dr.GetOrdinal(this.Pmehcodi);
            if (!dr.IsDBNull(iPmehcodi)) entity.Pmehcodi = Convert.ToInt32(dr.GetValue(iPmehcodi));

            int iPmpxehcodi = dr.GetOrdinal(this.Pmpxehcodi);
            if (!dr.IsDBNull(iPmpxehcodi)) entity.Pmpxehcodi = Convert.ToInt32(dr.GetValue(iPmpxehcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iPmpxehestado = dr.GetOrdinal(this.Pmpxehestado);
            if (!dr.IsDBNull(iPmpxehestado)) entity.Pmpxehestado = dr.GetString(iPmpxehestado);

            int iPmpxehfactor = dr.GetOrdinal(this.Pmpxehfactor);
            if (!dr.IsDBNull(iPmpxehfactor)) entity.Pmpxehfactor = dr.GetDecimal(iPmpxehfactor);

            int iPmpxehusucreacion = dr.GetOrdinal(this.Pmpxehusucreacion);
            if (!dr.IsDBNull(iPmpxehusucreacion)) entity.Pmpxehusucreacion = dr.GetString(iPmpxehusucreacion);

            int iPmpxehfeccreacion = dr.GetOrdinal(this.Pmpxehfeccreacion);
            if (!dr.IsDBNull(iPmpxehfeccreacion)) entity.Pmpxehfeccreacion = dr.GetDateTime(iPmpxehfeccreacion);

            int iPmpxehusumodificacion = dr.GetOrdinal(this.Pmpxehusumodificacion);
            if (!dr.IsDBNull(iPmpxehusumodificacion)) entity.Pmpxehusumodificacion = dr.GetString(iPmpxehusumodificacion);

            int iPmpxehfecmodificacion = dr.GetOrdinal(this.Pmpxehfecmodificacion);
            if (!dr.IsDBNull(iPmpxehfecmodificacion)) entity.Pmpxehfecmodificacion = dr.GetDateTime(iPmpxehfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Pmehcodi = "PMEHCODI";
        public string Pmpxehcodi = "PMPXEHCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Pmpxehestado = "PMPXEHESTADO";
        public string Pmpxehfactor = "PMPXEHFACTOR";
        public string Pmpxehusucreacion = "PMPXEHUSUCREACION";
        public string Pmpxehfeccreacion = "PMPXEHFECCREACION";
        public string Pmpxehusumodificacion = "PMPXEHUSUMODIFICACION";
        public string Pmpxehfecmodificacion = "PMPXEHFECMODIFICACION";

        public string Ptomedielenomb = "PTOMEDIELENOMB";
        public string Ptomedidesc = "PTOMEDIDESC";

        #endregion

        public string SqlUpdateEstado
        {
            get { return GetSqlXml("UpdateEstado"); }
        }
    }
}
