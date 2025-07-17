using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AF_ERACMF_ZONA
    /// </summary>
    public class AfEracmfZonaHelper : HelperBase
    {
        public AfEracmfZonaHelper() : base(Consultas.AfEracmfZonaSql)
        {
        }

        public AfEracmfZonaDTO Create(IDataReader dr)
        {
            AfEracmfZonaDTO entity = new AfEracmfZonaDTO();

            int iAferacfeccreacion = dr.GetOrdinal(this.Aferacfeccreacion);
            if (!dr.IsDBNull(iAferacfeccreacion)) entity.Aferacfeccreacion = dr.GetDateTime(iAferacfeccreacion);

            int iAferacusucreacion = dr.GetOrdinal(this.Aferacusucreacion);
            if (!dr.IsDBNull(iAferacusucreacion)) entity.Aferacusucreacion = dr.GetString(iAferacusucreacion);

            int iAferacdertemp = dr.GetOrdinal(this.Aferacdertemp);
            if (!dr.IsDBNull(iAferacdertemp)) entity.Aferacdertemp = dr.GetDecimal(iAferacdertemp);

            int iAferacderpend = dr.GetOrdinal(this.Aferacderpend);
            if (!dr.IsDBNull(iAferacderpend)) entity.Aferacderpend = dr.GetDecimal(iAferacderpend);

            int iAferacderarrq = dr.GetOrdinal(this.Aferacderarrq);
            if (!dr.IsDBNull(iAferacderarrq)) entity.Aferacderarrq = dr.GetDecimal(iAferacderarrq);

            int iAferacumbraltemp = dr.GetOrdinal(this.Aferacumbraltemp);
            if (!dr.IsDBNull(iAferacumbraltemp)) entity.Aferacumbraltemp = dr.GetDecimal(iAferacumbraltemp);

            int iAferacumbralarrq = dr.GetOrdinal(this.Aferacumbralarrq);
            if (!dr.IsDBNull(iAferacumbralarrq)) entity.Aferacumbralarrq = dr.GetDecimal(iAferacumbralarrq);

            int iAferacporcrechazo = dr.GetOrdinal(this.Aferacporcrechazo);
            if (!dr.IsDBNull(iAferacporcrechazo)) entity.Aferacporcrechazo = dr.GetDecimal(iAferacporcrechazo);

            int iAferacnumetapa = dr.GetOrdinal(this.Aferacnumetapa);
            if (!dr.IsDBNull(iAferacnumetapa)) entity.Aferacnumetapa = Convert.ToInt32(dr.GetValue(iAferacnumetapa));

            int iAferaczona = dr.GetOrdinal(this.Aferaczona);
            if (!dr.IsDBNull(iAferaczona)) entity.Aferaczona = dr.GetString(iAferaczona);

            int iAferacfechaperiodo = dr.GetOrdinal(this.Aferacfechaperiodo);
            if (!dr.IsDBNull(iAferacfechaperiodo)) entity.Aferacfechaperiodo = dr.GetDateTime(iAferacfechaperiodo);

            int iAferaccodi = dr.GetOrdinal(this.Aferaccodi);
            if (!dr.IsDBNull(iAferaccodi)) entity.Aferaccodi = Convert.ToInt32(dr.GetValue(iAferaccodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Aferacfeccreacion = "AFERACFECCREACION";
        public string Aferacusucreacion = "AFERACUSUCREACION";
        public string Aferacdertemp = "AFERACDERTEMP";
        public string Aferacderpend = "AFERACDERPEND";
        public string Aferacderarrq = "AFERACDERARRQ";
        public string Aferacumbraltemp = "AFERACUMBRALTEMP";
        public string Aferacumbralarrq = "AFERACUMBRALARRQ";
        public string Aferacporcrechazo = "AFERACPORCRECHAZO";
        public string Aferacnumetapa = "AFERACNUMETAPA";
        public string Aferaczona = "AFERACZONA";
        public string Aferacfechaperiodo = "AFERACFECHAPERIODO";
        public string Aferaccodi = "AFERACCODI";

        #endregion
    }
}
