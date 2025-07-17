using COES.Dominio.DTO.Transferencias;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.CPPA.Helper
{
    public class CpaExcelHoja
    {
        public string NombreHoja { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo1 { get; set; }
        public string Subtitulo2 { get; set; }
        public List<CpaExcelEstilo> ListaEstilosTitulos { get; set; }
        public List<int> ListaAnchoColumna { get; set; }
        public List<CpaExcelModelo>[] ListaCabeceras { get; set; }
        public CpaExcelCuerpo CuerpoOculto { get; set; }
        public CpaExcelCuerpo Cuerpo { get; set; }
        public List<CpaExcelModelo>[] ListaPies { get; set; }
    }

    public class CpaExcelModelo
    {
        public string Nombre { get; set; }
        public int NumColumnas { get; set; }
        public int NumFilas { get; set; }
        public string AlineaHorizontal { get; set; }
    }

    public class CpaExcelEstilo
    {
        public string NumberformatFormat { get; set; }
        public bool? FontBold { get; set; }
        public Color? FontColor { get; set; }
        public string FillBackgroundColor { get; set; }
        public string BorderColor { get; set; }
        public float? FontSize { get; set; }
        public List<string> ListaRangoFilas { get; set; }
        public List<CpaExcelEstilo> ListaEstilo { get; set; }
    }

    public class CpaExcelCuerpo
    {
        public List<string>[] ListaRegistros { get; set; }
        public List<string> ListaAlineaHorizontal { get; set; }
        public List<string> ListaTipo { get; set; }
        public List<CpaExcelEstilo> ListaEstilo { get; set; }
    }

    #region CU03
    public class CPPAFormatoExcel
    {
        public string Titulo { get; set; }
        public string Subtitulo1 { get; set; }
        public string Subtitulo2 { get; set; }
        public string[] Cabecera { get; set; }
        public string[][] Contenido { get; set; }
        public int[] AnchoColumnas { get; set; }
        public string NombreLibro { get; set; }
        public List<CPPAExcelHeader> NestedHeader1 { get; set; }
        public List<CPPAExcelHeader> NestedHeader2 { get; set; }
        public List<CPPAExcelHeader> NestedHeader3 { get; set; }
        public List<CPPAExcelHeader> NestedHeader4 { get; set; }
        public string[][] ColorByCells { get; set; }
    }

    public class CPPAExcelHeader
    {
        public int Columnas { get; set; }
        public string Etiqueta { get; set; }
    }

    #endregion
}