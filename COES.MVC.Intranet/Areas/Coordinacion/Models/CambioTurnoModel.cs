using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Coordinacion.Models
{
    public class CambioTurnoModel
    {
        public int[] IndicesTitulo { get; set; }
        public int[] IndicesSubtitulo { get; set; }
        public int[] IndicesAgrupacion { get; set; }
        public int[] IndicesComentario { get; set; }
        public int[] IndicesAdicional { get; set; }
        public int[] IndicesFinal { get; set; }
        public int[] IndiceMantenimiento { get; set; }
        public string[][] Datos { get; set; }
        public MergeModel[] Merge { get; set; }
        public string Fecha { get; set; }
        public List<SiCambioTurnoDTO> ListaResponsables { get; set; }
        public int? IdPersona { get; set; }
        public List<ValidacionListaCelda> Validaciones { get; set; }
        public int Indicador { get; set; }
        public List<CeldaValidacionLongitud> Longitudes { get; set; }
        public List<AlineacionCelda> Derechos { get; set; }
        public List<AlineacionCelda> Centros { get; set; }
        public string FechaMaximo { get; set; }
        public List<SiCambioTurnoAuditDTO> ListaAuditoria { get; set; }
        public bool IndGrabar { get; set; }

        #region Reprogramas
        public int[] IndicesReprogramas { get; set; }
        public List<ReprogramaCelda> Reprog { get; set; }
        public string DataEnvio { get; set; }
        public int NumReprogramas { get; set; }
        public string StrMensaje { get; set; }
        #endregion
    }

    public class MergeModel 
    {
        public int row { get; set; }
        public int col { get; set; }
        public int rowspan { get; set; }
        public int colspan { get; set; }
    }

    public class AlineacionCelda
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }

    public class ValidacionListaCelda
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public List<string> Elementos { get; set; }
    }

    public class CeldaValidacionLongitud
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int Longitud { get; set; }
    }

    public class ReprogramaCelda
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }
}