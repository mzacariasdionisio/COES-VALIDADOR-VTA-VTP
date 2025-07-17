using COES.Base.Core;
using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
//using COES.MVC.Intranet.ServiceReferenceMail;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.SIOSEIN;
using COES.MVC.Intranet.Areas.Siosein.Models;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using COES.Dominio.DTO.Transferencias;
 
namespace COES.MVC.Intranet.Areas.Siosein.Helper
{
    public class ToolsSiosein
    {
        public static List<TipoDatos> ObtenerListaTipoExcepcion()
        {
            List<TipoDatos> lista = new List<TipoDatos>();
            var elemento = new TipoDatos() { Codi = ConstantesSioSein.IdTipoexcepcionninguno, DetName = "Ninguno" };
            lista.Add(elemento);
            elemento = new TipoDatos() { Codi = ConstantesSioSein.IdTipoexcepcionSistAislado, DetName = "Sistema Aislado" };
            lista.Add(elemento);
            return lista;
        }

        /// <summary>
        /// Genera cabecera personalizada de Excel Web
        /// </summary>
        /// <param name="data"></param>
        /// <param name="col"></param>
        public static void GeneraCabeceraPrie01(string[][] data, int col)
        {
            data[0] = new string[col];
            data[0][0] = "Codigo Osinerg";
            data[0][1] = "Central";
            data[0][2] = "Unidad";
            data[0][3] = "Potencia Firme (MW)";
            data[0][4] = "Pto";
        }

        /// <summary>
        /// Genera cabecera personalizada de Excel Web
        /// </summary>
        /// <param name="data"></param>
        /// <param name="col"></param>
        public static void GeneraCabeceraPrie03(string[][] data, int col)
        {
            data[0] = new string[col];
            data[0][0] = "Nombre de Barra";
            for (int x = 1; x < col; x++)
            {
                data[0][x] = x.ToString();
            }
        }

        /// <summary>
        /// Genera cabecera personalizada de Excel Web
        /// </summary>
        /// <param name="data"></param>
        /// <param name="col"></param>
        /// <param name="fecha"></param>
        public static void GeneraCabeceraPrie26(string[][] data, int col, DateTime fecha)
        {
            data[0] = new string[col];
            data[0][0] = "Nuevo";
            data[0][1] = "Central / Fecha";
            for (int x = 2; x < col; x++)
            {
                data[0][x] = fecha.AddMonths(x-1).ToString("MM/yyyy");
            }
        }

        /// <summary>
        /// Genera cabecera personalizada de Excel Web
        /// </summary>
        /// <param name="data"></param>
        /// <param name="col"></param>
        /// <param name="fecha"></param>
        /// <param name="cabecera"></param>
        public static void GeneraCabeceraPrie27(string[][] data, int col, DateTime fecha, List<MePtomedicionDTO> cabecera)
        {
            data[0] = new string[col];
            data[0][0] = "SUBESTACION";
            data[0][1] = "Bloques";
            data[0][2] = "PtoMed";
            for (int x = 3; x < col; x++)
            {
                data[0][x] = fecha.AddMonths(x - 2).ToString("MM/yyyy");
            }

            if (cabecera != null)
            {
                // imprime la subestacion y bloques
                for (var k = 0; k < cabecera.Count; k++)
                {
                    data[k + 1][0] = cabecera[k].Ptomedielenomb;
                    data[k + 1][1] = cabecera[k].Ptomedidesc;
                    data[k + 1][2] = cabecera[k].Ptomedicodi.ToString();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="col"></param>
        /// <param name="fecha"></param>
        /// <param name="cabecera"></param>
        public static void GeneraCabeceraPrie28(string[][] data, int col, DateTime fecha, List<MePtomedicionDTO> cabecera)
        {
            data[0] = new string[col];
            data[0][0] = "COSTO k$";
            data[0][1] = "PtoMed";
            for (int x = 2; x < col; x++)
            {
                data[0][x] = fecha.AddMonths(x-1).ToString("MM/yyyy");
            }

            if (cabecera != null)
            {
                // imprime la subestacion o bloque
                for (var k = 0; k < cabecera.Count; k++)
                {
                    data[k + 1][0] = cabecera[k].Ptomedielenomb;
                    data[k + 1][1] = cabecera[k].Ptomedicodi.ToString();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="col"></param>
        /// <param name="fecha"></param>
        /// <param name="cabecera"></param>
        public static void GeneraCabeceraPrie29(string[][] data, int col, DateTime fecha, List<MePtomedicionDTO> cabecera)
        {
            data[0] = new string[col];
            data[0][0] = "EMBALSES";
            data[0][1] = "MEDIDA";
            data[0][2] = "PtoMed";
            for (int x = 3; x < col; x++)
            {
                data[0][x] = fecha.AddMonths(x - 2).ToString("MM/yyyy");
            }

            if (cabecera != null)
            {
                // imprime la subestacion o bloque
                for (var k = 0; k < cabecera.Count; k++)
                {
                    data[k + 1][0] = cabecera[k].Ptomedielenomb;
                    data[k + 1][1] = cabecera[k].Ptomedidesc;
                    data[k + 1][2] = cabecera[k].Ptomedicodi.ToString();
                }
            }
        }

        /// <summary>
        /// Carga Matriz de datos con informacion de envio por fecha
        /// </summary>
        /// <param name="data"></param>
        /// <param name="medicion"></param>
        /// <param name="col"></param>
        public static void LoadMatrizExcelMeMedicion1(string[][] data, int col, List<MeMedicion1DTO> list)
        {
            int i = 1;
            if (list != null)
            {
                foreach (var reg in list)
                {
                    data[i] = new string[col];
                    data[i][0] = reg.Osicodi;
                    data[i][1] = reg.Equinomb;
                    data[i][2] = reg.Gruponomb;
                    data[i][3] = reg.H1.ToString();
                    data[i][4] = reg.Ptomedicodi.ToString();
                    i++;
                }
            }
        }

        /// <summary>
        /// Carga Matriz de datos con informacion de envio por subestacion y bloque
        /// </summary>
        /// <param name="data"></param>
        /// <param name="col"></param>
        /// <param name="list"></param>
        /// <param name="cabecera"></param>
        /// <param name="dFecha"></param>
        public static void LoadMatrizExcelMeMedicionXIntervalo(string[][] data, int col, List<MeMedicionxintervaloDTO> list, List<MePtomedicionDTO> cabecera, DateTime dFecha)
        {
            if (list != null)
            {
                for (int i = 0; i < cabecera.Count; i++)
                {
                    var entity = list.Where(x => x.Ptomedicodi == cabecera[i].Ptomedicodi && x.Medintfechaini == dFecha).ToList();
                    if (entity.Count > 0)
                    {
                        for (int j = 0; j <= entity.Count; j++)
                        {
                            data[i + 1][j] = entity[j].Medinth1.ToString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Inicializamos matriz para el excel web
        /// </summary>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public static string[][] InicializaMatriz(int rowsHead, int nFil, int nCol)
        {
            string[][] matriz = new string[nFil + rowsHead][];
            for (int i = 0; i < nFil + rowsHead; i++)
            {
                matriz[i] = new string[nCol];
                for (int j = 0; j < nCol; 
                    j++)
                {
                    matriz[i][j] = string.Empty;               
                    
                }
            }
            return matriz;
        }

        /// <summary>
        /// Carga Matriz de datos con informacion de envio por fecha
        /// </summary>
        /// <param name="data"></param>
        /// <param name="medicion"></param>
        /// <param name="col"></param>
        public static void LoadMatrizExcelPrie26(string[][] data,  int col, List<MeMedicionxintervaloDTO> medicion)
        {
            int i = 1;
            int cantidad = medicion.Count / 12;
            int ptomedicion = 0;

            for (int r = 0; r < medicion.Count; r=r+12)
            {
                ptomedicion = medicion[r].Ptomedicodi;
                data[i] = new string[col];
                //data[i][0] = "false";
                data[i][0] = medicion[r].Ptomedicodi.ToString();

                for (int x = 1; x < 13; x++) {
                    data[i][x] = medicion[r+x-1].Medinth1.ToString();
                }

                i++;
            }

        }

        /// <summary>
        /// Carga Matriz de datos con informacion de envio por subestacion y bloque
        /// </summary>
        /// <param name="data"></param>
        /// <param name="col"></param>
        /// <param name="list"></param>
        /// <param name="cabecera"></param>
        /// <param name="dFecha"></param>
        public static void LoadMatrizExcelPrie27(string[][] data, int col, List<MeMedicionxintervaloDTO> list, List<MePtomedicionDTO> cabecera, DateTime dFecha)
        {
            if (list != null)
            {
                int i = 1;
                int cantidad = list.Count / 12;
                int ptomedicion = 0;

                for (int r = 0; r < list.Count; r = r + 12)
                {
                    ptomedicion = list[r].Ptomedicodi;
                    string dato1 = data[i][0];
                    string dato2 = data[i][1];
                    string dato3 = data[i][2];
                    data[i] = new string[col];
                    data[i][0] = dato1;
                    data[i][1] = dato2;
                    data[i][2] = dato3;

                    for (int x = 3; x < 15; x++)
                    {
                        data[i][x] = list[r + x - 3].Medinth1.ToString();
                    }

                    i++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="col"></param>
        /// <param name="list"></param>
        /// <param name="cabecera"></param>
        /// <param name="dFecha"></param>
        public static void LoadMatrizExcelPrie28(string[][] data, int col, List<MeMedicionxintervaloDTO> list, List<MePtomedicionDTO> cabecera, DateTime dFecha)
        {
            if (list != null)
            {
                int i = 1;
                int cantidad = list.Count / 12;
                int ptomedicion = 0;

                for (int r = 0; r < list.Count; r = r + 12)
                {
                    ptomedicion = list[r].Ptomedicodi;
                    string dato1 = data[i][0];
                    string dato2 = data[i][1];                    
                    data[i] = new string[col];
                    data[i][0] = dato1;
                    data[i][1] = dato2;                    

                    for (int x = 2; x < 14; x++)
                    {
                        data[i][x] = list[r + x - 2].Medinth1.ToString();
                    }

                    i++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="col"></param>
        /// <param name="list"></param>
        /// <param name="cabecera"></param>
        /// <param name="dFecha"></param>
        public static void LoadMatrizExcelPrie29(string[][] data, int col, List<MeMedicionxintervaloDTO> list, List<MePtomedicionDTO> cabecera, DateTime dFecha)
        {
            if (list != null)
            {               
                int i = 1;
                int cantidad = list.Count / 12;
                int ptomedicion = 0;

                for (int r = 0; r < list.Count; r = r + 12)
                {
                    ptomedicion = list[r].Ptomedicodi;
                    string dato1 = data[i][0];
                    string dato2 = data[i][1];
                    string dato3 = data[i][2];
                    data[i] = new string[col];
                    data[i][0] = dato1;
                    data[i][1] = dato2;
                    data[i][2] = dato3;

                    for (int x = 3; x < 15; x++)
                    {
                        data[i][x] = list[r + x - 3].Medinth1.ToString();
                    }

                    i++;
                }
            }
        }
        /// <summary>
        /// Retorna el numero de columnas de un archivo excel de costo marginal
        /// </summary>
        /// <param name="ws"></param>
        /// <returns></returns>
        public static int GetNroColumnasExcelCmg(ExcelWorksheet ws)
        {
            bool salir = false;
            string celda = string.Empty;
            int ncol = ws.Dimension.End.Column;
            while (!salir)
            {
                celda = ws.Cells[3,ncol].Text;
                if (!string.IsNullOrEmpty(celda))
                {
                    salir = true;
                }
                else{
                    ncol--;
                }
                
            }
            return ncol;
        }

        /// <summary>
        /// Encuentra el codigo de barra en la tabla Barras a traves del nombre ingresado en el archivo excel
        /// </summary>
        /// <param name="barras"></param>
        /// <param name="barra"></param>
        /// <returns></returns>
        public static int GetIdBarra(List<BarraDTO> barras, string barra)
        {
            int idBarra = 0;
            var val = barras.Find(x => x.BarrNombre.Replace(" ", "").Trim().ToUpper() == barra.Replace(" ", "").Trim().ToUpper());            
            if (val != null) { 
                
                idBarra = val.BarrCodi;
            }
                else
                {
                    val = barras.Where(x => x.BarrNombBarrTran != null).ToList().Find(x => x.BarrNombBarrTran.Replace(" ", "").Trim().ToUpper() == barra.Replace(" ", "").Trim().ToUpper());
                    if (val != null) {
                        idBarra = val.BarrCodi;
                    }
                }
            return idBarra;
        }
    }

    public class TipoDatos
    {
        public int Codi { get; set; }
        public string DetName { get; set; }
    }
 
    public class DiaValoresCostoMarginal
    {
        public DateTime FechaDia { get; set; }
        public int[] IdCabecera { get; set; }
        public string[][] Matriz1 { get; set; }
        public string[][] Matriz2 { get; set; }
        public string[][] Matriz3 { get; set; }
    }
}