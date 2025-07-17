using System;
using System.Data;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IIO_OSIG_FACTURA_UL
    /// </summary>
    public class IioOsigFacturaUlHelper : HelperBase
    {
        public IioOsigFacturaUlHelper() : base(Consultas.IioOsigFacturaUlSql)
        {        
        }

        #region Mapeo de Campos

        public string TableName = "IIO_OSIG_FACTURA_UL";

        public string Psiclicodi = "PSICLICODI";
        public string Ulfactcodempresa = "ULFACTCODEMPRESA";
        public string Ulfactcodsuministro = "ULFACTCODSUMINISTRO";
        public string Ulfactmesfacturado = "ULFACTMESFACTURADO";
        public string Ulfactcodbrg = "ULFACTCODBRG";
        public string Ulfactcodpuntosuministro = "ULFACTCODPUNTOSUMINISTRO";
        public string Ulfactcodareademanda = "ULFACTCODAREADEMANDA";
        public string Ulfactpagavad = "ULFACTPAGAVAD";
        public string Ulfactprecenergbrghp = "ULFACTPRECENERGBRGHP";
        public string Ulfactprecenergbrgfp = "ULFACTPRECENERGBRGFP";
        public string Ulfactprecpotenbrg = "ULFACTPRECPOTENBRG";
        public string Ulfactconsenergactvhpps = "ULFACTCONSENERGACTVHPPS";
        public string Ulfactconsenergactvfpps = "ULFACTCONSENERGACTVFPPS";
        public string Ulfactmaxdemhpps = "ULFACTMAXDEMHPPS";
        public string Ulfactmaxdemfpps = "ULFACTMAXDEMFPPS";
        public string Ulfactpeajetransmprin = "ULFACTPEAJETRANSMPRIN";
        public string Ulfactpeajetransmsec = "ULFACTPEAJETRANSMSEC";
        public string Ulfactfpmpoten = "ULFACTFPMPOTEN";
        public string Ulfactfpmenerg = "ULFACTFPMENERG";
        public string Ulfactfactgeneracion = "ULFACTFACTGENERACION";
        public string Ulfactfacttransmprin = "ULFACTFACTTRANSMPRIN";
        public string Ulfactfacttransmsec = "ULFACTFACTTRANSMSEC";
        public string Ulfactfactdistrib = "ULFACTFACTDISTRIB";
        public string Ulfactfactexcesopoten = "ULFACTFACTEXCESOPOTEN";
        public string Ulfactfacturaciontotal = "ULFACTFACTURACIONTOTAL";
        public string Ulfactconsenergreacps = "ULFACTCONSENERGREACPS";
        public string Ulfactppmt = "ULFACTPPMT";
        public string Ulfactpemt = "ULFACTPEMT";
        public string Ulfactfactptoref = "ULFACTFACTPTOREF";
        public string Ulfactvadhp = "ULFACTVADHP";
        public string Ulfactvadfp = "ULFACTVADFP";
        public string Ulfactcargoelectrificarural = "ULFACTCARGOELECTRIFICARURAL";
        public string Ulfactotrosconceptosnoafecigv = "ULFACTOTROSCONCEPTOSNOAFECIGV";
        public string Ulfactotrosconceptosafectoigv = "ULFACTOTROSCONCEPTOSAFECTOIGV";
        public string Emprcodi = "EMPRCODI";
        public string Ptomedicodi = "PTOMEDICODI";

        public string Ulfactbarrcodibrg = "ULFACTBARRCODIBRG";
        public string Ulfactbarrcodiptosumin = "ULFACTBARRCODIPTOSUMIN";

        public string Ulfactusucreacion = "ULFACTUSUCREACION";
        public string Ulfactfeccreacion = "ULFACTFECCREACION";

        public string Emprcodisuministrador = "EMPRCODISUMINISTRADOR";

        public string Correlativo = "CORRELATIVO";
        public string Registros = "REGISTROS";

        #endregion

        public string SqlUpdateOsigFactura
        {
            get { return GetSqlXml("UpdateOsigFactura"); }
        }
        public string SqlValidarPuntoMedicion
        {
            get { return GetSqlXml("ValidarPuntoMedicion"); }
        }
        public string SqlSaveIioFactura
        {
            get { return GetSqlXml("SaveIioFactura"); }
        }

        public string SqlDeleteIioFactura
        {
            get { return GetSqlXml("DeleteIioFactura"); }
        }

        public string SqlUpdateEmprcodiOsigFactura
        {
            get { return GetSqlXml("UpdateOsigFacturaEmpresa"); }
        }

        public string SqlGetMaxIdIioLogImportacion
        {
            get { return GetSqlXml("GetMaxIdIioLogImportacion"); }
        }

        public string SqlRegistrarLogimportacionPorEmpresa
        {
            get { return GetSqlXml("RegistrarLogimportacionPorEmpresa"); }
        }

        public string SqlRegistrarLogimportacionPtoMedicion
        {
            get { return GetSqlXml("RegistrarLogimportacionPtoMedicion"); }
        }

        public string SqlActualizarControlImportacionNoOK
        {
            get { return GetSqlXml("ActualizarControlImportacionNoOK"); }
        }

        public string SqlActualizarControlImportacionOK
        {
            get { return GetSqlXml("ActualizarControlImportacionOK"); }
        }

        public string SqlActualizarCantidadRegistrosImportacionCoes
        {
            get { return GetSqlXml("ActualizarCantidadRegistrosImportacionCoes"); }
        }
        
        public string SqlActualizarPeriodoFechaSincCoes
        {
            get { return GetSqlXml("ActualizarPeriodoFechaSincCoes"); }
        }
        public string SqlValidarTablaEmpresas
        {
            get { return GetSqlXml("ValidarTablaEmpresas"); }
        }
    }
}