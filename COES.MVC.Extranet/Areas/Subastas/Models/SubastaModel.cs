using System;
using System.Collections.Generic;
using COES.MVC.Extranet.Areas.Subastas.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;

namespace COES.MVC.Extranet.Areas.Subastas.Models
{
    #region SubastaModel

    public class SubastaModel
    {
        public string MensajeError { get; set; }
        public string FechaOfertaDesc { get; set; }
        public Constantes.TipoOferta TipoOferta { get; set; }
        public OfertaListItem[] ListaTabSubir { get; set; }
        public OfertaListItem[] ListaTabBajar { get; set; }
        public EnvioListItem[] ListaEnvio { get; set; }

        public string FechaOferta { get; set; }
        public string OferfechaenvioDesc { get; set; }
        public int OferCodi { get; set; }
        public string OferCodenvio { get; set; }
        public string OferEstado { get; set; }

        public int Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public Parametros ObjParametros { get; set; }

        #region SubastaListItem

        public class SubastaListItem
        {
            public int GrupoCodigo { get; set; }

            public string CodigoEnvio { get; set; }

            public DateTime? FechaEnvio { get; set; }
        }

        #endregion

        #region OfertaMantenimientoListItem

        public class OfertaMantenimientoListItem
        {
            public string Fecha { get; set; }

            public string URS { get; set; }

            public string ModoOperacion { get; set; }

            public decimal? CapacidadMaximaRSF { get; set; }

            public bool MantenimientoProgramado { get; set; }

            public string[,] IntervaloMantenimieto { get; set; }
        }

        #endregion

        #region OfertaListItem

        public class OfertaListItem
        {
            public string HoraInicio { get; set; }
            public string HoraFin { get; set; }
            public int URS { get; set; }
            public string ModoOperacion { get; set; }
            public decimal PotenciaOfertada { get; set; }
            public decimal PotenciaMaxima { get; set; }
            public string Precio { get; set; }
            public int Grupocodi { get; set; }
            public decimal? Indice { get; set; }
            public decimal? Cantidad { get; set; }

            public decimal BandaCalificada { get; set; }
            public decimal BandaDisponible { get; set; }
        }

        #endregion

        #region EnvioListItem

        public class EnvioListItem
        {
            public int Codigo { get; set; }
            public string CodigoEnvio { get; set; }
            public string FechaEnvio { get; set; }
            public string FechaOferta { get; set; }
        }

        #endregion

        #region URSListItem

        public class URSListItem
        {
            public int ID { get; set; }
            public string Text { get; set; }
            public ModoOperacionListItem[] OperationModes { get; set; }
            public bool TieneBandaCalificada { get; set; }
        }

        #endregion

        #region ModoOperacionListItem

        public class ModoOperacionListItem
        {
            public int ID { get; set; }
            public string Text { get; set; }

            public decimal? Pot { get; set; }

            public string[,] IntvMant { get; set; }

            public decimal? Indice { get; set; }

            public decimal? Cantidad { get; set; }

            public bool EsReservaFirme { get; set; }


            public decimal? BandaDisponible { get; set; }
            public decimal? BandaCalificada { get; set; }
            public decimal? BandaAdjudicada { get; set; }
        }

        #endregion

        public class Parametros
        {
            public bool TieneOfertaPorDefecto { get; set; }

            public bool TieneExcelWebHabilitado { get; set; }

            public double HoraActual { get; set; }

            public double HoraInicioParaOfertar { get; set; }

            public double HoraFinParaOfertar { get; set; }

            public URSListItem[] URSs { get; set; }

            public decimal PrecioMaximo { get; set; }
            
            public decimal PrecioMinimo { get; set; }
            
            public decimal PrecioDefectoMaximo { get; set; }

            public decimal BandaCalificadaTotalUrs { get; set; }
            public decimal BandaAdjudicadaTotalUrs { get; set; }

            public decimal PotenciaMinimaMan { get; set; }
        }

        public class RangosFechas
        {
            public List<DateTime> Rango { get; set; }
            public int CodigoRango { get; set; }

            public string NombreUrs { get; set; }
            public string Hoja { get; set; }
        }
    }

    #endregion
}