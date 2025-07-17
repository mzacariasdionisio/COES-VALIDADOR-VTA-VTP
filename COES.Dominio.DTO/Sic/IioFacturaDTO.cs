using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IIO_FACTURA
    /// </summary>
    public class IioFacturaDTO
    {
        public int Psiclicodi { get; set; }
        public string Ufacmesfacturado { get; set; }
        public string Ufaccodbrg { get; set; }
        public string Ufaccodpuntosuministro { get; set; }
        public int Ufacidareademanda { get; set; }
        public string Ufacpagavad { get; set; }
        public decimal Ufacprecenergbrghp { get; set; }
        public decimal Ufacprecenergbrgfp { get; set; }
        public decimal Ufacprecpotenbrg { get; set; }
        public decimal Ufacconsenergactvhpps { get; set; }
        public decimal Ufacconsenergactvfpps { get; set; }
        public decimal Ufacmaxdemhpps { get; set; }
        public decimal Ufacmaxdemfpps { get; set; }
        public decimal Ufacpeajetransmprin { get; set; }
        public decimal Ufacpeajetransmsec { get; set; }
        public decimal Ufacfpmpoten { get; set; }
        public decimal Ufacfpmenerg { get; set; }
        public decimal Ufacfactgeneracion { get; set; }
        public decimal Ufacfacttransmprin { get; set; }
        public decimal Ufacfacttransmsec { get; set; }
        public decimal Ufacfactdistrib { get; set; }
        public decimal Ufacfactexcesopoten { get; set; }
        public decimal Ufacfacturaciontotal { get; set; }
        public decimal Ufacconsenergreacps { get; set; }
        public decimal Ufacppmt { get; set; }
        public decimal Ufacpemt { get; set; }
        public string Ufacfactptoref { get; set; }
        public decimal Ufacvadhp { get; set; }
        public decimal Ufacvadfp { get; set; }
        public decimal Ufaccargoelectrificacionrural { get; set; }
        public decimal Ufacotrosconceptosnoafectoigv { get; set; }
        public decimal Ufacotrosconceptosafectoigv { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodi { get; set; }
    }
}