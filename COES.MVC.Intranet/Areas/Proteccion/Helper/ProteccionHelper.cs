using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using OfficeOpenXml;
using System.IO;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Helper;
using System.Web.WebPages;
using System.IO.Compression;
using System.Configuration;
using DevExpress.XtraRichEdit.Commands;

namespace COES.MVC.Intranet.Areas.Proteccion.Helper
{
    public class ProteccionHelper
    {
        /// <summary>
        /// Permite obtener el tree de opciones
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ObtenerArbolGrupo(List<PrGrupoDTO> list)
        {
            int idPadre = -1;

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("[\n");
            try
            {
                List<PrGrupoDTO> listItem = list.Where(x => x.Grupopadre == idPadre).ToList();
                int contador = 0;
                foreach (PrGrupoDTO item in listItem)
                {
                    string icono = ObtenerIcono(item.Grupotipo, (int)item.Catecodi);
                    string clave = item.Grupocodi + "," + item.Catecodi;

                   
                    List<PrGrupoDTO> listHijos = list.Where(x => x.Grupopadre == item.Grupocodi && x.Catecodi == item.Catecodi+1).ToList();
                    if (listHijos.Count > 0)
                    {
                        strHtml.Append("   {'key': '" + clave + "', 'icon': '" + icono + "',  'title': '" + item.Gruponomb +
                            "' , 'expanded' : 'true', 'children':[\n");
                        strHtml.Append(ObtenerSubArbolGrupo(listHijos, list, "   "));
                        if (contador < listItem.Count - 1) strHtml.Append("   ]},\n");
                        else strHtml.Append("   ]}\n");
                    }
                    else
                    {
                        if (contador < listItem.Count - 1)
                            strHtml.Append("   {'key': '" + clave + "', 'icon': '" + icono + "', 'title': '" +
                                item.Gruponomb + "' },\n");
                        else
                            strHtml.Append("   {'key': '" + clave + "', 'icon': '" + icono + "','title': '" +
                                item.Gruponomb + "'}\n");
                    }
                    contador++;
                }
            }
            catch (Exception e)
            {
                var mensaje = "error: " + e.Message.ToString();
            }
            strHtml.Append("]");
            return strHtml.ToString();
        }

        /// <summary>
        /// Funcion recursiva para obtener el menu
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static string ObtenerSubArbolGrupo(List<PrGrupoDTO> list, List<PrGrupoDTO> listGeneral, string pad)
        {
            StringBuilder strHtml = new StringBuilder();

            int contador = 0;
            foreach (PrGrupoDTO item in list)
            {
                string icono = ObtenerIcono(item.Grupotipo, (int)item.Catecodi);
                string clave = item.Grupocodi + "," + item.Catecodi;

                List<PrGrupoDTO> listHijos = listGeneral.Where(x => x.Grupopadre == item.Grupocodi && x.Catecodi == item.Catecodi + 1).ToList();

                if (listHijos.Count > 0)
                {
                    strHtml.Append(pad + "    {'key': '" + clave + "' , 'icon': '" + icono + "', 'title': '" +
                        item.Gruponomb + "', 'children':[\n");
                    strHtml.Append(ObtenerSubArbolGrupo(listHijos, listGeneral, pad + "  "));
                    if (contador < list.Count - 1) strHtml.Append(pad + "    ]},\n");
                    else strHtml.Append(pad + "    ]}\n");
                }
                else
                {
                    if (contador < list.Count - 1)
                        strHtml.Append("   {'key': '" + clave + "', 'icon': '" + icono + "', 'title': '" +
                           item.Gruponomb + "' },\n");
                    else
                        strHtml.Append("   {'key': '" + clave + "', 'icon': '" + icono + "', 'title': '" +
                            item.Gruponomb + "' }\n");
                }
                contador++;
            }

            return strHtml.ToString();
        }

        /// <summary>
        /// Permite obtener el íconpo
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="categoria"></param>
        /// <returns></returns>
        private static string ObtenerIcono(string tipo, int categoria)
        {

            if (tipo == ConstantesProteccion.TipoTermico)
            {
                if (categoria == ConstantesProteccion.CategoriaCentralTermica)
                {
                    return "termica.png";
                }
                else if (categoria == ConstantesProteccion.CategoriaGrupoTermico)
                {
                    return "grupotermico.png";
                }
                else if (categoria == ConstantesProteccion.CategoriaModoTermico)
                {
                    return "modotermico.png";
                }
            }


            return "termica.png";
        }


        /// <summary>
        /// Permite obtener la carpeta principal de Intervenciones
        /// </summary>
        /// <returns></returns>
        public string GetPathPrincipal()
        {
            //- Definimos la carpeta raiz (termina con /)
            string pathRaiz = FileServer.GetDirectory();
            return pathRaiz;
        }

        /// <summary>
        /// Descarga el archivo adjuntado desde el fileserver
        /// </summary>
        /// <param name="epsubecodi"></param>
        /// <param name="fileName"></param>
        /// <param name="rutaBase"></param>
        /// <param name="rutaArchivo"></param>
        /// <returns></returns>
        public byte[] GetBufferArchivoAdjunto(int epsubecodi, string fileName, string rutaBase, string rutaArchivo)
        {
            string pathAlternativo = GetPathPrincipal();

                string rutaDestino = string.Format("{0}\\{1}\\", rutaBase, rutaArchivo);

                if (FileServer.VerificarExistenciaFile(rutaDestino, fileName, pathAlternativo))
                {
                    return FileServer.DownloadToArrayByte(rutaDestino + "\\" + fileName, pathAlternativo);
                }
           
            return null;
        }

        public void GenerarExportacionEquipoCOES(string pathLogo, string nameFile, List<EqEquipoDTO> lExportar, string rutaBase)
        {
            FileServer.CreateFolder(rutaBase, ConstantesProteccion.FolderGestProtec, "");
            FileServer.CreateFolder(rutaBase, ConstantesProteccion.FolderReporte, "");
            
            ////Descargo archivo segun requieran
            string rutaFile = FileServer.GetDirectory() + rutaBase + "/"+ConstantesProteccion.FolderReporte+ "/" + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarArchivoExcelEquipoCOES(xlPackage, pathLogo, lExportar);

                xlPackage.Save();
            }
        }

        private void GenerarArchivoExcelEquipoCOES(ExcelPackage xlPackage, string pathLogo, List<EqEquipoDTO> lExportar)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            string nameWS = "REPORTE";
            string titulo = "LISTADO DE EQUIPAMIENTO SGOCOES";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;

            UtilExcel.AddImageLocal(ws, 1, 0, pathLogo, 120, 70);

            #region  Filtros y Cabecera

            int colIniTitulo = 2;
            int rowIniTitulo = 4;

            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 2;

            int colZona = colIniTable;
            int colTitular = colIniTable + 1;
            int colSubestacion = colIniTable + 2;
            int colCelda = colIniTable + 3;

            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colCelda);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colCelda);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colCelda, "Centro");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colCelda, "Calibri", 16);

            ws.Row(rowIniTabla).Height = 25;
            ws.Cells[rowIniTabla, colZona].Value = "Zona";
            ws.Cells[rowIniTabla, colTitular].Value = "Titular";
            ws.Cells[rowIniTabla, colSubestacion].Value = "Subestación";
            ws.Cells[rowIniTabla, colCelda].Value = "Celda";

            //Estilos cabecera
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colZona, rowIniTabla, colCelda, "Calibri", 11);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colZona, rowIniTabla, colCelda, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colZona, rowIniTabla, colCelda, "Centro");
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colZona, rowIniTabla, colCelda, "#2980B9");
            servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colZona, rowIniTabla, colCelda, "#FFFFFF");
            servFormato.BorderCeldas2(ws, rowIniTabla, colZona, rowIniTabla, colCelda);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTabla, colZona, rowIniTabla, colCelda);

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in lExportar)
            {
                ws.Cells[rowData, colZona].Value = item.Areanomb;
                ws.Cells[rowData, colTitular].Value = item.Emprnomb;
                ws.Cells[rowData, colSubestacion].Value = item.Subestacion;
                ws.Cells[rowData, colCelda].Value = item.Celda;

                rowData++;
            }

            if (!lExportar.Any()) rowData++;

            //Estilos registros
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colZona, rowData - 1, colCelda, "Calibri", 8);
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla + 1, colZona, rowData - 1, colCelda, "Centro");
            servFormato.BorderCeldas2(ws, rowIniTabla + 1, colZona, rowData - 1, colCelda);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colZona, rowData - 1, colCelda, "Centro");

            #endregion

            //filter           
            ws.Cells[rowIniTabla, colZona, rowData, colCelda].AutoFitColumns();
            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(rowIniTabla + 1, 1);
        }

        public void GenerarExportacionProyecto(string pathLogo, string nameFile, List<EprEquipoDTO> lExportar, string rutaBase, string titulo)
        {
            FileServer.CreateFolder(rutaBase, ConstantesProteccion.FolderGestProtec, "");
            FileServer.CreateFolder(rutaBase, ConstantesProteccion.FolderReporte, "");

            ////Descargo archivo segun requieran
            string rutaFile = FileServer.GetDirectory() + rutaBase + "/" + ConstantesProteccion.FolderReporte + "/" + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarArchivoExcelProyecto(xlPackage, pathLogo, lExportar, titulo);

                xlPackage.Save();
            }
        }

        public void GenerarExportacionRele(string pathLogo, string nameFile, List<EprEquipoDTO> lExportar, string rutaBase, string titulo)
        {
            FileServer.CreateFolder(rutaBase, ConstantesProteccion.FolderGestProtec, "");
            FileServer.CreateFolder(rutaBase, ConstantesProteccion.FolderReporte, "");

            ////Descargo archivo segun requieran
            string rutaFile = FileServer.GetDirectory() + rutaBase + "/" + ConstantesProteccion.FolderReporte + "/" + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarArchivoExcelRele(xlPackage, pathLogo, lExportar, titulo);

                xlPackage.Save();
            }
        }
        private void GenerarArchivoExcelProyecto(ExcelPackage xlPackage, string pathLogo, List<EprEquipoDTO> lExportar, string titulo)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            string nameWS = "REPORTE";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;

            UtilExcel.AddImageLocal(ws, 1, 0, pathLogo, 120, 70);

            #region  Filtros y Cabecera

            int colIniTitulo = 2;
            int rowIniTitulo = 4;

            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 2;

            int colEquicodi = colIniTable;
            int colEquinonb = colIniTable + 1;
            int colCelda = colIniTable + 2;
            int colTension = colIniTable + 3;
            int colSistema = colIniTable + 4;
            int colFabricante = colIniTable + 5;
            int colModelo = colIniTable + 6;
            int colTipoUso = colIniTable + 7;
            int colEstado = colIniTable + 8;
            int colRTCIP = colIniTable + 9;
            int colRTCIS = colIniTable + 10;
            int colRTTVP = colIniTable + 11;

            int colRTTVS = colIniTable + 12;
            int colProtCondinable = colIniTable + 13;
            int colAjusteSincroActivo = colIniTable + 14;
            int colInterruptorComan = colIniTable + 15;
            int colDeltaTension = colIniTable + 16;
            int colDeltaAngulo = colIniTable + 17;
            int colDeltaFrecuencia = colIniTable + 18;
            int colAjusteSobreTActivo = colIniTable + 19;
            int colSobreTU = colIniTable + 20;
            int colSobreTT = colIniTable + 21;
            int colSobreTUU = colIniTable + 22;
            int colSobreTTT = colIniTable + 23;
            int colajusteSobreCActivo = colIniTable + 24;
            int colSobreCI = colIniTable + 25;
            int colPMUCheckActivo = colIniTable + 26;
            int colPMUAccion = colIniTable + 27;
            int colIdInterruptorMS = colIniTable + 28;
            int colIdMandoSincronizado = colIniTable + 29;
            int colMedidaMitigacion = colIniTable + 30;
            int colReleTorsImpl = colIniTable + 31;
            int colRelePMUAccion = colIniTable + 32;
            int colRelePMUImpl = colIniTable + 33;
            int colMemoriaCalculo = colIniTable + 34;
            int colAccion = colIniTable + 35;

            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colModelo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colModelo);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colModelo, "Izquierda");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colModelo, "Calibri", 16);

            ws.Row(rowIniTabla).Height = 25;
            ws.Cells[rowIniTabla, colEquicodi].Value = "Id del Relé";
            ws.Cells[rowIniTabla, colEquinonb].Value = "Nombre del Relé";
            ws.Cells[rowIniTabla, colCelda].Value = "Celda";
            ws.Cells[rowIniTabla, colTension].Value = "Tensión";
            ws.Cells[rowIniTabla, colSistema].Value = "Sistema";
            ws.Cells[rowIniTabla, colFabricante].Value = "Fabricante";
            ws.Cells[rowIniTabla, colModelo].Value = "Modelo";
            ws.Cells[rowIniTabla, colTipoUso].Value = "Tipo de Uso";
            ws.Cells[rowIniTabla, colEstado].Value = "Estado";
            ws.Cells[rowIniTabla, colRTCIP].Value = "RTC Ip";
            ws.Cells[rowIniTabla, colRTCIS].Value = "RTC Is";
            ws.Cells[rowIniTabla, colRTTVP].Value = "RTT Vp";
            ws.Cells[rowIniTabla, colRTTVS].Value = "RTT Vs";
            ws.Cells[rowIniTabla, colProtCondinable].Value = "Protecciones Coordinables";
            ws.Cells[rowIniTabla, colAjusteSincroActivo].Value = "Ajuste de sincronismo Activo";
            ws.Cells[rowIniTabla, colInterruptorComan].Value = "Interruptor que comanda";
            ws.Cells[rowIniTabla, colDeltaTension].Value = "Delta de tensión";
            ws.Cells[rowIniTabla, colDeltaAngulo].Value = "Delta de Ángulo";
            ws.Cells[rowIniTabla, colDeltaFrecuencia].Value = "Delta de Frecuencia";
            ws.Cells[rowIniTabla, colAjusteSobreTActivo].Value = "Ajuste de sobretensión Activo";
            ws.Cells[rowIniTabla, colSobreTU].Value = "U>[p.u.]";
            ws.Cells[rowIniTabla, colSobreTT].Value = "t>[s]";
            ws.Cells[rowIniTabla, colSobreTUU].Value = "U>>[p.u.]";
            ws.Cells[rowIniTabla, colSobreTTT].Value = "t>>[s]";
            ws.Cells[rowIniTabla, colajusteSobreCActivo].Value = "Umbral de sobrecorrientes Activo";
            ws.Cells[rowIniTabla, colSobreCI].Value = "I>[A]";
            ws.Cells[rowIniTabla, colPMUCheckActivo].Value = "Ajuste PMU Activo";
            ws.Cells[rowIniTabla, colPMUAccion].Value = "Acción";
            ws.Cells[rowIniTabla, colIdInterruptorMS].Value = "Interruptor Mando Sincronizado";
            ws.Cells[rowIniTabla, colIdMandoSincronizado].Value = "Mando Sincronizado";
            ws.Cells[rowIniTabla, colMedidaMitigacion].Value = "Medida Mitigación (Relé Torsional)";
            ws.Cells[rowIniTabla, colReleTorsImpl].Value = "Relé Torsional Implementado";
            ws.Cells[rowIniTabla, colRelePMUAccion].Value = "Relé PMU(Acción)";
            ws.Cells[rowIniTabla, colRelePMUImpl].Value = "Relé PMU Implementado";
            ws.Cells[rowIniTabla, colMemoriaCalculo].Value = "Memoria de Cálculo";
            ws.Cells[rowIniTabla, colAccion].Value = "Acción";

            //Estilos cabecera
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colEquicodi, rowIniTabla, colAccion, "Calibri", 11);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colEquicodi, rowIniTabla, colAccion, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colEquicodi, rowIniTabla, colAccion, "Centro");
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colEquicodi, rowIniTabla, colAccion, "#2980B9");
            servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colEquicodi, rowIniTabla, colAccion, "#FFFFFF");
            servFormato.BorderCeldas2(ws, rowIniTabla, colEquicodi, rowIniTabla, colAccion);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTabla, colEquicodi, rowIniTabla, colAccion);

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in lExportar)
            {
                ws.Cells[rowData, colEquicodi].Value = item.Equicodi;
                ws.Cells[rowData, colEquinonb].Value = item.Equinomb;
                ws.Cells[rowData, colCelda].Value = item.Celda;
                ws.Cells[rowData, colTension].Value = item.Tension;
                ws.Cells[rowData, colSistema].Value = item.Sistema;
                ws.Cells[rowData, colFabricante].Value = item.Marca;
                ws.Cells[rowData, colModelo].Value = item.Modelo;
                ws.Cells[rowData, colTipoUso].Value = item.TipoUso;
                ws.Cells[rowData, colEstado].Value = item.Estado;
                ws.Cells[rowData, colRTCIP].Value = item.RtcPrimario;
                ws.Cells[rowData, colRTCIS].Value = item.RtcSecundario;
                ws.Cells[rowData, colRTTVP].Value = item.RttPrimario;
                ws.Cells[rowData, colRTTVS].Value = item.RttSecundario;
                ws.Cells[rowData, colProtCondinable].Value = item.ProtCondinables;
                ws.Cells[rowData, colAjusteSincroActivo].Value = item.SincroCheckActivo;
                ws.Cells[rowData, colInterruptorComan].Value = item.IdInterruptor;
                ws.Cells[rowData, colDeltaTension].Value = item.DeltaTension;
                ws.Cells[rowData, colDeltaAngulo].Value = item.DeltaAngulo;
                ws.Cells[rowData, colDeltaFrecuencia].Value = item.DeltaFrecuencia;
                ws.Cells[rowData, colAjusteSobreTActivo].Value = item.SobreTCheckActivo;
                ws.Cells[rowData, colSobreTU].Value = item.SobreTU;
                ws.Cells[rowData, colSobreTT].Value = item.SobreTT;
                ws.Cells[rowData, colSobreTUU].Value = item.SobreTUU;
                ws.Cells[rowData, colSobreTTT].Value = item.SobreTTT;
                ws.Cells[rowData, colajusteSobreCActivo].Value = item.SobreCCheckActivo;
                ws.Cells[rowData, colSobreCI].Value = item.SobreCI;
                ws.Cells[rowData, colPMUCheckActivo].Value = item.PmuCheckActivo;
                ws.Cells[rowData, colPMUAccion].Value = item.PmuAccion;
                ws.Cells[rowData, colIdInterruptorMS].Value = item.IdInterruptorMS;
                ws.Cells[rowData, colIdMandoSincronizado].Value = item.IdMandoSincronizado;
                ws.Cells[rowData, colMedidaMitigacion].Value = item.MedidaMitigacion;
                ws.Cells[rowData, colReleTorsImpl].Value = item.ReleTorsImpl;
                ws.Cells[rowData, colRelePMUAccion].Value = item.RelePmuAccion;
                ws.Cells[rowData, colRelePMUImpl].Value = item.RelePmuImpl;
                ws.Cells[rowData, colMemoriaCalculo].Value = ProteccionHelper.modificarNombreArchivo(item.MemoriaCalculo);
                ws.Cells[rowData, colAccion].Value = item.Accion;
                rowData++;
            }

            if (!lExportar.Any()) rowData++;

            //Estilos registros
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colEquicodi, rowData - 1, colAccion, "Calibri", 8);
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla + 1, colEquicodi, rowData - 1, colAccion, "Centro");
            servFormato.BorderCeldas2(ws, rowIniTabla + 1, colEquicodi, rowData - 1, colAccion);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colEquicodi, rowData - 1, colAccion, "Centro");

            #endregion

            //filter           
            ws.Cells[rowIniTabla, colEquicodi, rowData, colAccion].AutoFitColumns();
            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(rowIniTabla + 1, 1);
        }

        private void GenerarArchivoExcelRele(ExcelPackage xlPackage, string pathLogo, List<EprEquipoDTO> lExportar, string titulo)
        {
            PR5ReportesAppServicio servFormato = new PR5ReportesAppServicio();

            string nameWS = "REPORTE";

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            ws.Row(1).Height = 25;
            ws.Row(2).Height = 25;

            UtilExcel.AddImageLocal(ws, 1, 0, pathLogo, 120, 70);

            #region  Filtros y Cabecera

            int colIniTitulo = 2;
            int rowIniTitulo = 4;

            int colIniTable = colIniTitulo;
            int rowIniTabla = rowIniTitulo + 2;

            int colEquicodi = colIniTable;
            int colEquinonb = colIniTable + 1;
            int colCelda = colIniTable + 2;
            int colTension = colIniTable + 3;
            int colSistema = colIniTable + 4;
            int colFabricante = colIniTable + 5;
            int colModelo = colIniTable + 6;
            int colTipoUso = colIniTable + 7;
            int colEstado = colIniTable + 8;
            int colRTCIP = colIniTable + 9;
            int colRTCIS = colIniTable + 10;
            int colRTTVP = colIniTable + 11;

            int colRTTVS = colIniTable + 12;
            int colProtCondinable = colIniTable + 13;
            int colAjusteSincroActivo = colIniTable + 14;
            int colInterruptorComan = colIniTable + 15;
            int colDeltaTension = colIniTable + 16;
            int colDeltaAngulo = colIniTable + 17;
            int colDeltaFrecuencia = colIniTable + 18;
            int colAjusteSobreTActivo = colIniTable + 19;
            int colSobreTU = colIniTable + 20;
            int colSobreTT = colIniTable + 21;
            int colSobreTUU = colIniTable + 22;
            int colSobreTTT = colIniTable + 23;
            int colajusteSobreCActivo = colIniTable + 24;
            int colSobreCI = colIniTable + 25;
            int colPMUCheckActivo = colIniTable + 26;
            int colPMUAccion = colIniTable + 27;
            int colIdInterruptorMS = colIniTable + 28;
            int colIdMandoSincronizado = colIniTable + 29;
            int colMedidaMitigacion = colIniTable + 30;
            int colReleTorsImpl = colIniTable + 31;
            int colRelePMUAccion = colIniTable + 32;
            int colRelePMUImpl = colIniTable + 33;
            int colMemoriaCalculo = colIniTable + 34;
            int colProyectoCreador = colIniTable + 35;
            int colFechaCreacion = colIniTable + 36;
            int colProyectoModificador = colIniTable + 37;
            int colFechaModificacion = colIniTable + 38;


            ws.Cells[rowIniTitulo, colIniTitulo].Value = titulo;
            servFormato.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colModelo);
            servFormato.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colModelo);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colModelo, "Izquierda");
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colModelo, "Calibri", 16);

            ws.Row(rowIniTabla).Height = 25;
            ws.Cells[rowIniTabla, colEquicodi].Value = "Id del Relé";
            ws.Cells[rowIniTabla, colEquinonb].Value = "Nombre del Relé";
            ws.Cells[rowIniTabla, colCelda].Value = "Celda";
            ws.Cells[rowIniTabla, colTension].Value = "Tensión";
            ws.Cells[rowIniTabla, colSistema].Value = "Sistema";
            ws.Cells[rowIniTabla, colFabricante].Value = "Fabricante";
            ws.Cells[rowIniTabla, colModelo].Value = "Modelo";
            ws.Cells[rowIniTabla, colTipoUso].Value = "Tipo de Uso";
            ws.Cells[rowIniTabla, colEstado].Value = "Estado";
            ws.Cells[rowIniTabla, colRTCIP].Value = "RTC Ip";
            ws.Cells[rowIniTabla, colRTCIS].Value = "RTC Is";
            ws.Cells[rowIniTabla, colRTTVP].Value = "RTT Vp";
            ws.Cells[rowIniTabla, colRTTVS].Value = "RTT Vs";
            ws.Cells[rowIniTabla, colProtCondinable].Value = "Protecciones Coordinables";
            ws.Cells[rowIniTabla, colAjusteSincroActivo].Value = "Ajuste de sincronismo Activo";
            ws.Cells[rowIniTabla, colInterruptorComan].Value = "Interruptor que comanda";
            ws.Cells[rowIniTabla, colDeltaTension].Value = "Delta de tensión";
            ws.Cells[rowIniTabla, colDeltaAngulo].Value = "Delta de Ángulo";
            ws.Cells[rowIniTabla, colDeltaFrecuencia].Value = "Delta de Frecuencia";
            ws.Cells[rowIniTabla, colAjusteSobreTActivo].Value = "Ajuste de sobretensión Activo";
            ws.Cells[rowIniTabla, colSobreTU].Value = "U>[p.u.]";
            ws.Cells[rowIniTabla, colSobreTT].Value = "t>[s]";
            ws.Cells[rowIniTabla, colSobreTUU].Value = "U>>[p.u.]";
            ws.Cells[rowIniTabla, colSobreTTT].Value = "t>>[s]";
            ws.Cells[rowIniTabla, colajusteSobreCActivo].Value = "Umbral de sobrecorrientes Activo";
            ws.Cells[rowIniTabla, colSobreCI].Value = "I>[A]";
            ws.Cells[rowIniTabla, colPMUCheckActivo].Value = "Ajuste PMU Activo";
            ws.Cells[rowIniTabla, colPMUAccion].Value = "Acción";
            ws.Cells[rowIniTabla, colIdInterruptorMS].Value = "Interruptor Mando Sincronizado";
            ws.Cells[rowIniTabla, colIdMandoSincronizado].Value = "Mando Sincronizado";
            ws.Cells[rowIniTabla, colMedidaMitigacion].Value = "Medida Mitigación (Relé Torsional)";
            ws.Cells[rowIniTabla, colReleTorsImpl].Value = "Relé Torsional Implementado";
            ws.Cells[rowIniTabla, colRelePMUAccion].Value = "Relé PMU(Acción)";
            ws.Cells[rowIniTabla, colRelePMUImpl].Value = "Relé PMU Implementado";
            ws.Cells[rowIniTabla, colMemoriaCalculo].Value = "Memoria de Cálculo";
            ws.Cells[rowIniTabla, colProyectoCreador].Value = "Proyecto Creador";
            ws.Cells[rowIniTabla, colFechaCreacion].Value = "Fecha Creación";
            ws.Cells[rowIniTabla, colProyectoModificador].Value = "Proyecto Modificador";
            ws.Cells[rowIniTabla, colFechaModificacion].Value = "Fecha Últ. Modificación";


            //Estilos cabecera
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colEquicodi, rowIniTabla, colFechaModificacion, "Calibri", 11);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colEquicodi, rowIniTabla, colFechaModificacion, "Centro");
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colEquicodi, rowIniTabla, colFechaModificacion, "Centro");
            servFormato.CeldasExcelColorFondo(ws, rowIniTabla, colEquicodi, rowIniTabla, colFechaModificacion, "#2980B9");
            servFormato.CeldasExcelColorTexto(ws, rowIniTabla, colEquicodi, rowIniTabla, colFechaModificacion, "#FFFFFF");
            servFormato.BorderCeldas2(ws, rowIniTabla, colEquicodi, rowIniTabla, colFechaModificacion);
            servFormato.CeldasExcelEnNegrita(ws, rowIniTabla, colEquicodi, rowIniTabla, colFechaModificacion);

            #endregion

            #region Cuerpo Principal

            int rowData = rowIniTabla + 1;

            foreach (var item in lExportar)
            {
                ws.Cells[rowData, colEquicodi].Value = item.Equicodi;
                ws.Cells[rowData, colEquinonb].Value = item.Equinomb;
                ws.Cells[rowData, colCelda].Value = item.Celda;
                ws.Cells[rowData, colTension].Value = item.Tension;
                ws.Cells[rowData, colSistema].Value = item.Sistema;
                ws.Cells[rowData, colFabricante].Value = item.Marca;
                ws.Cells[rowData, colModelo].Value = item.Modelo;
                ws.Cells[rowData, colTipoUso].Value = item.TipoUso;
                ws.Cells[rowData, colEstado].Value = item.Estado;
                ws.Cells[rowData, colRTCIP].Value = item.RtcPrimario;
                ws.Cells[rowData, colRTCIS].Value = item.RtcSecundario;
                ws.Cells[rowData, colRTTVP].Value = item.RttPrimario;
                ws.Cells[rowData, colRTTVS].Value = item.RttSecundario;
                ws.Cells[rowData, colProtCondinable].Value = item.ProtCondinables;
                ws.Cells[rowData, colAjusteSincroActivo].Value = item.SincroCheckActivo;
                ws.Cells[rowData, colInterruptorComan].Value = item.IdInterruptor;
                ws.Cells[rowData, colDeltaTension].Value = item.DeltaTension;
                ws.Cells[rowData, colDeltaAngulo].Value = item.DeltaAngulo;
                ws.Cells[rowData, colDeltaFrecuencia].Value = item.DeltaFrecuencia;
                ws.Cells[rowData, colAjusteSobreTActivo].Value = item.SobreTCheckActivo;
                ws.Cells[rowData, colSobreTU].Value = item.SobreTU;
                ws.Cells[rowData, colSobreTT].Value = item.SobreTT;
                ws.Cells[rowData, colSobreTUU].Value = item.SobreTUU;
                ws.Cells[rowData, colSobreTTT].Value = item.SobreTTT;
                ws.Cells[rowData, colajusteSobreCActivo].Value = item.SobreCCheckActivo;
                ws.Cells[rowData, colSobreCI].Value = item.SobreCI;
                ws.Cells[rowData, colPMUCheckActivo].Value = item.PmuCheckActivo;
                ws.Cells[rowData, colPMUAccion].Value = item.PmuAccion;
                ws.Cells[rowData, colIdInterruptorMS].Value = item.IdInterruptorMS;
                ws.Cells[rowData, colIdMandoSincronizado].Value = item.IdMandoSincronizado;
                ws.Cells[rowData, colMedidaMitigacion].Value = item.MedidaMitigacion;
                ws.Cells[rowData, colReleTorsImpl].Value = item.ReleTorsImpl;
                ws.Cells[rowData, colRelePMUAccion].Value = item.RelePmuAccion;
                ws.Cells[rowData, colRelePMUImpl].Value = item.RelePmuImpl;
                ws.Cells[rowData, colMemoriaCalculo].Value = ProteccionHelper.modificarNombreArchivo(item.MemoriaCalculo);
                ws.Cells[rowData, colProyectoCreador].Value = item.ProyectoCreador;
                ws.Cells[rowData, colFechaCreacion].Value = item.Fechacreacionstr;
                ws.Cells[rowData, colProyectoModificador].Value = item.ProyectoActualizador;
                ws.Cells[rowData, colFechaModificacion].Value = item.Fechamodificacionstr;

                rowData++;
            }

            if (!lExportar.Any()) rowData++;

            //Estilos registros
            servFormato.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colEquicodi, rowData - 1, colFechaModificacion, "Calibri", 8);
            servFormato.CeldasExcelAlinearVerticalmente(ws, rowIniTabla + 1, colEquicodi, rowData - 1, colFechaModificacion, "Centro");
            servFormato.BorderCeldas2(ws, rowIniTabla + 1, colEquicodi, rowData - 1, colFechaModificacion);
            servFormato.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colEquicodi, rowData - 1, colFechaModificacion, "Centro");
            
            #endregion

            //filter           
            ws.Cells[rowIniTabla, colEquicodi, rowData, colFechaModificacion].AutoFitColumns();
            ws.View.ShowGridLines = false;
            ws.View.ZoomScale = 100;
            ws.View.FreezePanes(rowIniTabla + 1, 1);
        }

        public void GenerarArchizoZipHC(string nameFile, List<EprEquipoDTO> lExportar, string rutaBase, string CarpetaBorrar)
        {
            FileServer.CreateFolder(rutaBase, ConstantesProteccion.FolderGestProtec, "");
            FileServer.CreateFolder(rutaBase, ConstantesProteccion.FolderArchivoZIP, "");
            FileServer.CreateFolder(rutaBase, ConstantesProteccion.FolderTemporal, "");
            var folderTemporalBorrar = ConstantesProteccion.FolderTemporal + "/" + CarpetaBorrar;
            FileServer.CreateFolder(rutaBase, folderTemporalBorrar, "");
            
            ////Descargo archivo segun requieran
            string rutaFile = FileServer.GetDirectory() + rutaBase + "/" + folderTemporalBorrar + "/" + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            foreach (var item in lExportar)
            {
                if (item.Zona != null && !item.Zona.IsEmpty()) {
                    var rutaZona = folderTemporalBorrar + "/" + item.Zona;
                    FileServer.CreateFolder(rutaBase, rutaZona, "");

                    if (item.SubestacionNomb != null && !item.SubestacionNomb.IsEmpty()) {
                        var rutaSubestacion = rutaZona + "/" + item.SubestacionNomb;
                        FileServer.CreateFolder(rutaBase, rutaSubestacion, "");

                        if (item.Celda != null && !item.Celda.IsEmpty())
                        {
                            var rutaCelda = rutaSubestacion + "/" + item.Celda;
                            FileServer.CreateFolder(rutaBase, rutaCelda, "");

                            switch (item.Tipo)
                            {
                                case "SE":
                                    FileServer.CopiarFileRename(ConstantesProteccion.FolderSubestacion + "/", rutaCelda + "/", item.MemoriaCalculo, rutaBase, item.NombreArchivo);
                                    break;
                                case "CE":
                                    FileServer.CopiarFileRename(ConstantesProteccion.FolderRele + "/", rutaCelda + "/", item.MemoriaCalculo, rutaBase, item.NombreArchivo);
                                    break;
                            }
                            
                        }
                    }
                }


            }

            CompressFolder(folderTemporalBorrar, ConstantesProteccion.FolderArchivoZIP + "/"+ nameFile, rutaBase, "");

        }

        static void CompressFolder(string folderPath, string zipPath,string rutaBase, string pathAlternativo)
        {

            string LocalDirectory = ConfigurationManager.AppSettings["LocalDirectory"];
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            if (!string.IsNullOrEmpty(rutaBase)) rutaBase = rutaBase + "/";
            string pathZipCompleta = LocalDirectory + rutaBase + zipPath;
            string folderPathCompleta = LocalDirectory + rutaBase + folderPath;

            // Eliminar el archivo ZIP si ya existe
            if (File.Exists(pathZipCompleta))
            {
                File.Delete(pathZipCompleta);
            }

            // Crear el archivo ZIP
            using (FileStream zipFile = new FileStream(pathZipCompleta, FileMode.Create))
            using (ZipArchive archive = new ZipArchive(zipFile, ZipArchiveMode.Create))
            {
                // Recorrer los archivos de la carpeta
                string[] files = Directory.GetFiles(folderPathCompleta, "*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    // Obtener la ruta relativa dentro del ZIP
                    string relativePath = GetRelativePath(folderPathCompleta, file);

                    // Agregar el archivo al ZIP
                    archive.CreateEntryFromFile(file, relativePath, System.IO.Compression.CompressionLevel.Optimal);
                }
            }
        }

        static string GetRelativePath(string basePath, string fullPath)
        {
            Uri baseUri = new Uri(basePath.EndsWith(Path.DirectorySeparatorChar.ToString())
                ? basePath
                : basePath + Path.DirectorySeparatorChar);
            Uri fullUri = new Uri(fullPath);
            return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fullUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }
        public static string modificarNombreArchivo(string nombre)
        {
            string nuevoNombre = "";
            if (nombre != null && nombre != "")
            {
                if (nombre.Split('.').Length >= 2)
                {
                    string extension = nombre.Split('.').Last();
                    var textFecha = nombre.Split('_').Last();
                    nuevoNombre = nombre.Replace("_" + textFecha, "") + "." + extension;
                }
            }
            return nuevoNombre;
        }
    }
}