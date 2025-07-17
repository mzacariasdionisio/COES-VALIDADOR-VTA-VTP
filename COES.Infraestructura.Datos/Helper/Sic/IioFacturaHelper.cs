using System;
using System.Data;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IIO_FACTURA
    /// </summary>
    public class IioFacturaHelper : HelperBase
    {
        public IioFacturaHelper() : base(Consultas.IioFacturaSql)
        {        
        }

        #region Mapeo de Campos

        public string TableName = "IIO_FACTURA";
        public string Psiclicodi = "PSICLICODI";
        public string Ufacmesfacturado = "UFACMESFACTURADO";
        public string Ufaccodbrg = "UFACCODBRG";
        public string Ufaccodpuntosuministro = "UFACCODPUNTOSUMINISTRO";
        public string Ufacidareademanda = "UFACIDAREADEMANDA";
        public string Ufacpagavad = "UFACPAGAVAD";
        public string Ufacprecenergbrghp = "UFACPRECENERGBRGHP";
        public string Ufacprecenergbrgfp = "UFACPRECENERGBRGFP";
        public string Ufacprecpotenbrg = "UFACPRECPOTENBRG";
        public string Ufacconsenergactvhpps = "UFACCONSENERGACTVHPPS";
        public string Ufacconsenergactvfpps = "UFACCONSENERGACTVFPPS";
        public string Ufacmaxdemhpps = "UFACMAXDEMHPPS";
        public string Ufacmaxdemfpps = "UFACMAXDEMFPPS";
        public string Ufacpeajetransmprin = "UFACPEAJETRANSMPRIN";
        public string Ufacpeajetransmsec = "UFACPEAJETRANSMSEC";
        public string Ufacfpmpoten = "UFACFPMPOTEN";
        public string Ufacfpmenerg = "UFACFPMENERG";
        public string Ufacfactgeneracion = "UFACFACTGENERACION";
        public string Ufacfacttransmprin = "UFACFACTTRANSMPRIN";
        public string Ufacfacttransmsec = "UFACFACTTRANSMSEC";
        public string Ufacfactdistrib = "UFACFACTDISTRIB";
        public string Ufacfactexcesopoten = "UFACFACTEXCESOPOTEN";
        public string Ufacfacturaciontotal = "UFACFACTURACIONTOTAL";
        public string Ufacconsenergreacps = "UFACCONSENERGREACPS";
        public string Ufacppmt = "UFACPPMT";
        public string Ufacpemt = "UFACPEMT";
        public string Ufacfactptoref = "UFACFACTPTOREF";
        public string Ufacvadhp = "UFACVADHP";
        public string Ufacvadfp = "UFACVADFP";
        public string Ufaccargoelectrificacionrural = "UFACCARGOELECTRIFICACIONRURAL";
        public string Ufacotrosconceptosnoafectoigv = "UFACOTROSCONCEPTOSNOAFECTOIGV";
        public string Ufacotrosconceptosafectoigv = "UFACOTROSCONCEPTOSAFECTOIGV";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";

        #endregion


    }
}