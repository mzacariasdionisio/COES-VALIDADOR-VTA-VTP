using Novacode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper.TablaHTML;

namespace COES.Servicios.Aplicacion.Helper
{
    /// <summary>
    /// Utileria del Novacode
    /// </summary>
    public static class UtilWord
    {
        //public static double FactorWidth = 1.05; //pixel a unidad del word

        public static void FormatearFilaTablaWord(ref Table tabla, int fila, List<WordCelda> listaColumna, FontFamily fontDoc)
        {
            int nroColumn = listaColumna.Count;
            for (int x = 0; x < nroColumn; x++)
            {
                var objCol = listaColumna[x];

                //agregar texto
                tabla.Rows[fila].Cells[x].Paragraphs[0].Append(objCol.Nombre);

                //formato de celda
                tabla.Rows[fila].Cells[x].FillColor = System.Drawing.ColorTranslator.FromHtml(objCol.Color);

                tabla.Rows[fila].Cells[x].Paragraphs[0].Alignment = objCol.AlignmentCabecera;

                tabla.Rows[fila].Cells[x].Paragraphs[0].Bold();
                tabla.Rows[fila].Cells[x].Paragraphs[0].Font(fontDoc);
                tabla.Rows[fila].Cells[x].Paragraphs[0].FontSize(objCol.SizeCabecera);

                tabla.Rows[fila].Cells[x].VerticalAlignment = VerticalAlignment.Center;
            }
        }

        public static void BodyTablaWord(ref Table tabla, int filaIni, int colIni, int filaFin, int colFin, List<WordCelda> listaColumna, FontFamily fontDoc)
        {
            for (int index = filaIni; index <= filaFin; index++)
            {
                for (int x = colIni; x <= colFin; x++)
                {
                    var objCol = listaColumna[x];

                    tabla.Rows[index].Cells[x].Paragraphs[0].Font(fontDoc);
                    tabla.Rows[index].Cells[x].Paragraphs[0].Alignment = objCol.AlignmentCuerpo;
                    tabla.Rows[index].Cells[x].Paragraphs[0].FontSize(objCol.SizeCuerpo);

                    tabla.Rows[index].Cells[x].Width = (double)(objCol.AnchoPixel);
                }
            }
        }

        public static void BodyTablaWordVerticalCentrado(ref Table tabla, int filaIni, int colIni, int filaFin, int colFin, List<WordCelda> listaColumna, FontFamily fontDoc)
        {
            for (int index = filaIni; index <= filaFin; index++)
            {
                for (int x = colIni; x <= colFin; x++)
                {
                    var objCol = listaColumna[x];

                    tabla.Rows[index].Cells[x].Paragraphs[0].Font(fontDoc);
                    tabla.Rows[index].Cells[x].Paragraphs[0].Alignment = objCol.AlignmentCuerpo;
                    tabla.Rows[index].Cells[x].Paragraphs[0].FontSize(objCol.SizeCuerpo);

                    tabla.Rows[index].Cells[x].Width = (double)(objCol.AnchoPixel);
                    tabla.Rows[index].Cells[x].VerticalAlignment = VerticalAlignment.Center;
                }
            }
        }

        public static void FormatearColumna1TablaWord(ref Table tabla, List<WordCelda> listaFila, FontFamily fontDoc)
        {
            int nrofilas = listaFila.Count;

            for (int x = 0; x < nrofilas; x++)
            {
                var objFila = listaFila[x];


                tabla.Rows[x + 1].Cells[0].Paragraphs[0].Append(objFila.Nombre);
                tabla.Rows[x + 1].Cells[0].FillColor = System.Drawing.ColorTranslator.FromHtml(objFila.Color);

                //tabla.Rows[x + 1].Cells[0].Paragraphs[0].Alignment = objFila.AlignmentCabecera;

                tabla.Rows[x + 1].Cells[0].Paragraphs[0].Bold();
                tabla.Rows[x + 1].Cells[0].Paragraphs[0].Font(fontDoc);
                //tabla.Rows[x + 1].Cells[0].VerticalAlignment = VerticalAlignment.Center;
            }
        }

    }

    public class WordCelda
    {
        public string Nombre { get; set; }
        public decimal AnchoPixel { get; set; }
        public Alignment AlignmentCabecera { get; set; }
        //public List<int> ListaFilaCabecera { get; set; }
        public string Color { get; set; }
        public Alignment AlignmentCuerpo { get; set; }
        public int SizeCabecera { get; set; }
        public int SizeCuerpo { get; set; }

        public WordCelda(string nombre, decimal anchoPixel, int sizeCab, int sizeCuerpo, Alignment aliCuerpo)
        {
            Nombre = nombre;
            AnchoPixel = anchoPixel;
            AlignmentCuerpo = aliCuerpo;

            SizeCabecera = sizeCab;
            SizeCuerpo = sizeCuerpo;

            Color = "#DDDDDD";
            AlignmentCabecera = Alignment.center;
        }
    }
}
