using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_DATOS_DETALLE
    /// </summary>
    public class CbDatosDetalleHelper : HelperBase
    {
        public CbDatosDetalleHelper() : base(Consultas.CbDatosDetalleSql)
        {
        }

        public CbDatosDetalleDTO Create(IDataReader dr)
        {
            CbDatosDetalleDTO entity = new CbDatosDetalleDTO();

            int iCbdetcodi = dr.GetOrdinal(this.Cbdetcodi);
            if (!dr.IsDBNull(iCbdetcodi)) entity.Cbdetcodi = Convert.ToInt32(dr.GetValue(iCbdetcodi));

            int iCbevdacodi = dr.GetOrdinal(this.Cbevdacodi);
            if (!dr.IsDBNull(iCbevdacodi)) entity.Cbevdacodi = Convert.ToInt32(dr.GetValue(iCbevdacodi));

            int iCbdetcompago = dr.GetOrdinal(this.Cbdetcompago);
            if (!dr.IsDBNull(iCbdetcompago)) entity.Cbdetcompago = dr.GetString(iCbdetcompago);

            int iCbdettipo = dr.GetOrdinal(this.Cbdettipo);
            if (!dr.IsDBNull(iCbdettipo)) entity.Cbdettipo = Convert.ToInt32(dr.GetValue(iCbdettipo));

            int iCbdetfechaemision = dr.GetOrdinal(this.Cbdetfechaemision);
            if (!dr.IsDBNull(iCbdetfechaemision)) entity.Cbdetfechaemision = dr.GetDateTime(iCbdetfechaemision);

            int iCbdettipocambio = dr.GetOrdinal(this.Cbdettipocambio);
            if (!dr.IsDBNull(iCbdettipocambio)) entity.Cbdettipocambio = dr.GetDecimal(iCbdettipocambio);

            int iCbdetmoneda = dr.GetOrdinal(this.Cbdetmoneda);
            if (!dr.IsDBNull(iCbdetmoneda)) entity.Cbdetmoneda = dr.GetString(iCbdetmoneda);

            int iCbdetvolumen = dr.GetOrdinal(this.Cbdetvolumen);
            if (!dr.IsDBNull(iCbdetvolumen)) entity.Cbdetvolumen = dr.GetDecimal(iCbdetvolumen);

            int iCbdetcosto = dr.GetOrdinal(this.Cbdetcosto);
            if (!dr.IsDBNull(iCbdetcosto)) entity.Cbdetcosto = dr.GetDecimal(iCbdetcosto);

            int iCbdetcosto2 = dr.GetOrdinal(this.Cbdetcosto2);
            if (!dr.IsDBNull(iCbdetcosto2)) entity.Cbdetcosto2 = dr.GetDecimal(iCbdetcosto2);

            int iCbdetimpuesto = dr.GetOrdinal(this.Cbdetimpuesto);
            if (!dr.IsDBNull(iCbdetimpuesto)) entity.Cbdetimpuesto = dr.GetDecimal(iCbdetimpuesto);

            int iCbdetdmrg = dr.GetOrdinal(this.Cbdetdmrg);
            if (!dr.IsDBNull(iCbdetdmrg)) entity.Cbdetdmrg = dr.GetDecimal(iCbdetdmrg);

            return entity;
        }


        #region Mapeo de Campos

        public string Cbdetcodi = "CBDETCODI";
        public string Cbevdacodi = "CBEVDACODI";
        public string Cbdetcompago = "CBDETCOMPAGO";
        public string Cbdettipo = "CBDETTIPO";
        public string Cbdetfechaemision = "CBDETFECHAEMISION";
        public string Cbdettipocambio = "CBDETTIPOCAMBIO";
        public string Cbdetmoneda = "CBDETMONEDA";
        public string Cbdetvolumen = "CBDETVOLUMEN";
        public string Cbdetcosto = "CBDETCOSTO";
        public string Cbdetcosto2 = "CBDETCOSTO2";
        public string Cbdetimpuesto = "CBDETIMPUESTO";
        public string Cbdetdmrg = "CBDETDMRG";

        #endregion
    }
}
