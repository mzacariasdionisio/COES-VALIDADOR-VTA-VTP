using COES.Dominio.DTO.Transferencias;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.PrimasRER.Helper
{
    public class RerExcelHoja
    {
        public string NombreHoja { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo1 { get; set; }
        public string Subtitulo2 { get; set; }
        public List<int> ListaAnchoColumna { get; set; }
        public List<RerExcelModelo>[] ListaCabeceras { get; set; }
        public RerExcelCuerpo CuerpoOculto { get; set; }
        public RerExcelCuerpo Cuerpo { get; set; }
        public List<RerExcelModelo>[] ListaPies { get; set; }
    }

    public class RerExcelModelo
    {
        public string Nombre { get; set; }
        public int NumColumnas { get; set; }
        public int NumFilas { get; set; }
        public string AlineaHorizontal { get; set; }
    }

    public class RerExcelEstilo
    {
        public string NumberformatFormat { get; set; } 
        public bool? FontBold { get; set; }
        public Color? FontColor { get; set; }
        public string FillBackgroundColor { get; set; }
        public string BorderColor { get; set; }
        public List<string> ListaRangoFilas { get; set; }
        public List<RerExcelEstilo> ListaEstilo { get; set; }
    }

    public class RerExcelCuerpo
    {
        public List<string>[] ListaRegistros { get; set; }
        public List<string> ListaAlineaHorizontal { get; set; }
        public List<string> ListaTipo { get; set; }
        public List<RerExcelEstilo> ListaEstilo { get; set; }
    }

    public class RerValorTipico
    {
        public DateTime FechaInicio { get; set; }
        public string HoraInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string HoraFin { get; set; }
    }

    public class RerEnergiaEstimada
    {
        public string OrigenDatos { get; set; }
        public RerValorTipico ValorTipico { get; set; }
        public List<RerComparativoDetDTO> ListCDTExcel { get; set; }
    }
}
