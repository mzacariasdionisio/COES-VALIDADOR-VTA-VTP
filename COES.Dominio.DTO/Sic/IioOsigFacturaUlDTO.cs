using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IIO_OSIG_FACTURA_UL
    /// </summary>
    public class IioOsigFacturaUlDTO
    {
        public int Psiclicodi { get; set; }
        public string Ulfactcodempresa { get; set; }
        public string Ulfactcodsuministro { get; set; }
        public string Ulfactmesfacturado { get; set; }
        public string Ulfactcodbrg { get; set; }
        public string Ulfactcodpuntosuministro { get; set; }
        public int Ulfactcodareademanda { get; set; }
        public string Ulfactpagavad { get; set; }
        public decimal Ulfactprecenergbrghp { get; set; }
        public decimal Ulfactprecenergbrgfp { get; set; }
        public decimal Ulfactprecpotenbrg { get; set; }
        public decimal Ulfactconsenergactvhpps { get; set; }
        public decimal Ulfactconsenergactvfpps { get; set; }
        public decimal Ulfactmaxdemhpps { get; set; }
        public decimal Ulfactmaxdemfpps { get; set; }
        public decimal Ulfactpeajetransmprin { get; set; }
        public decimal Ulfactpeajetransmsec { get; set; }
        public decimal Ulfactfpmpoten { get; set; }
        public decimal Ulfactfpmenerg { get; set; }
        public decimal Ulfactfactgeneracion { get; set; }
        public decimal Ulfactfacttransmprin { get; set; }
        public decimal Ulfactfacttransmsec { get; set; }
        public decimal Ulfactfactdistrib { get; set; }
        public decimal Ulfactfactexcesopoten { get; set; }
        public decimal Ulfactfacturaciontotal { get; set; }
        public decimal Ulfactconsenergreacps { get; set; }
        public decimal Ulfactppmt { get; set; }
        public decimal Ulfactpemt { get; set; }
        public string Ulfactfactptoref { get; set; }
        public decimal Ulfactvadhp { get; set; }
        public decimal Ulfactvadfp { get; set; }
        public decimal Ulfactcargoelectrificarural { get; set; }
        public decimal Ulfactotrosconceptosnoafecigv { get; set; }
        public decimal Ulfactotrosconceptosafectoigv { get; set; }
        public int Emprcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Ulfactbarrcodibrg { get; set; }
        public int Ulfactbarrcodiptosumin { get; set; }

        public string Ulfactusucreacion { get; set; }
        public DateTime Ulfactfeccreacion { get; set; }
        public string Ulfactusumodificacion { get; set; }
        public DateTime Ulfactfecmodificacion { get; set; }
    }
}