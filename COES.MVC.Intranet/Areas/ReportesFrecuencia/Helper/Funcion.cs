using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Helper
{
    public class Funcion
    {
        //CONSTANTES
        public const int iGrabar = 1;
        public const int iEditar = 2;
        public const int iNuevo = 3;
        public const int iEliminar = 5;
        public const decimal dValorMax = 0.3135M;
        public const double dLimiteMaxEnergia = 350;
        public const int CODIEMPR_SINCONTRATO = -1001;
        public const string NOMBEMPR_SINCONTRATO = "RETIRO SIN CONTRATO";
        public const string MensajeSoles = "Importes Expresados en Nuevos Soles (S/.)";
        public const string ReporteDirectorio = "ReporteTransferencia";
        public const string RutaReporte = "Areas/Transferencias/Reporte/";
        public const string HojaReporteExcel = "REPORTE";
        public const string AppExcel = "application/vnd.ms-excel";
        public const string AppWord = "application/vnd.ms-word";
        public const string AppPdf = "application/pdf";
        public const string AppCSV = "application/CSV";
        public const string AppTxt = "application/txt";
        public const decimal dPorcentajePotenciaMaxima = 0.1M;
        public const int CodigoOrigenLecturaSICLI = 19;
        public const int CodigoOrigenLecturaML = 32;

        //NOMBRE DE LOS ARCHIVOS DE RESULTADO
        public const string NombreListaCompensacionExcel = "ListaCompensaciones.xlsx";
        public const string NombreReporteBarraExcel = "ReporteBarra.xlsx";
        public const string NombreReporteCodigoEntregaExcel = "ReporteCodigoEntrega.xlsx";
        public const string NombreReporteCodigoInfoBaseExcel = "ReporteCodigoInfoBase.xlsx";
        public const string NombreReporteCodigoRetiroExcel = "ReporteCodigoRetiro.xlsx";
        public const string NombreReporteCodigoRetiroSinContratoExcel = "ReporteCodigoRetiroSinContrato.xlsx";
        public const string NombreReporteCostoMarginalExcel = "ReporteCostoMarginal.xlsx";
        public const string NombreReporteCostoMarginalBarraExcel = "ReporteCostoMarginalBarra.xlsx";
        public const string NombreReporteIngresoCompensacionExcel = "ReporteIngresoCompensacion.xlsx";
        public const string NombreReporteIngresoPotenciaExcel = "ReporteIngresoPotencia.xlsx";
        public const string NombreReporteIngresoRetiroSCExcel = "ReporteIngresoRetiroSC.xlsx";
        public const string NombreReporteFormatoEntregaRetiroExcel = "ReporteFormatoEntregaRetiro.xlsx";
        public const string NombreReporteResumenEnergiaMensualExcel = "ReporteResumenEnergiaMensual.xlsx";
        public const string NombreReporteEntregasRetirosEnergiaValorizadosExcel = "EntregasRetirosEnergiaValorizados.xlsx";
        public const string NombreReporteEntregasRetirosEnergiaValorizadosExcel15min = "EntregasRetirosEnergiaValorizados15min.xlsx";
        public const string NombreDeterminacionSaldosTransferenciasExcel = "DeterminacionSaldosTransferencias.xlsx";
        public const string NombrePagosTransferenciasEnergíaActivaExcel = "PagosTransferenciasEnergíaActiva.xlsx";
        public const string NombreEntregasRetirosEnergiaExcel = "EntregasRetirosEnergia.xlsx";
        public const string NombreDesviacionRetirosExcel = "DesviacionRetirosEnergia.xlsx";
        public const string NombreReporteTipoContratoExcel = "ReporteTipoContrato.xlsx";
        public const string NombreReporteTipoUsuarioExcel = "ReporteTipoUsuario.xlsx";
        public const string NombreReporteInformacionBaseExcel = "ReporteInformacionBase.xlsx";
        public const string NombreReporteInformacionModeloExcel = "ReporteInformacionModelo.xlsx";
        public const string NombreReporteInfoDesbalanceExcel = "ReporteInfoDesbalance.xlsx";
        public const string NombreReporteInfoDesviacionExcel = "ReporteInfoDesviacion.xlsx";
        public const string NombreReporteInfoFaltanteExcel = "ReporteInfoFaltante.xlsx";
        public const string NombreReporteInfoFaltanteEntregaFueradeFechaExcel = "ReporteInfoEntregadaFueradeFecha.xlsx";
        public const string NombreHistoricoEntregasRetirosExcel = "ReporteHistoricoEntregasRetiros.xlsx";
        public const string NombreHistorico15minCodigoEntregaRetiroExcel = "ReporteHistorico15minCodigoEntregaRetiro.xlsx";
        public const string NombreReporteAuditoria = "ReporteAuditoria.xlsx";
        public const string NombreReporteCodigoEquivalenciaExcel = "ReporteCodigoEquivalencia.xlsx";
        public const string NombreReporteComparacionCoincidenteExcel = "ReporteComparacionCoincidente.xlsx";
        //ASSETEC 20181224
        public const string NombreReporteFactorPerdidaMediaExcel = "ReporteFactorPerdidaMedia.xlsx";
        //ASSETEC 20190104
        public const string NombreDesviacionEntregasExcel = "DesviacionEntregasEnergia.xlsx";
        public const string NOMBEMPR_NOCUBIERTO = "RETIRO NO CUBIERTO";
        public const int iNroGrupos = 11;

        public const string periodoCorte = "idPeriodo";

        //  Estados

        public const string EstadoPendiente = "PAP";
        public const string EstadoPendientePVT = "PVT";
        public const string EstadoActivo = "ACT";
        public const string EstadoInactivo = "INA";
        public const string EstadoBaja = "BAJ";
        public const string EstadoSolicitudBaja = "SBJ";
        public const string EstadoRechazado = "REC";

        //Paginación
        public const int PageSizeMercadoLibre = 10;
        public const int PageSizePeriodoRentaCongestion = 30;
        public const int PageSize = 20;
        public const int NroPageShow = 10;
        public const int PageSizeCodigoEntrega = 20;
        public const int PageSizeCodigoRetiro = 20;
        public const int PageSizeCodigoRetiroSC = 20;
        public const int PageSizeCodigoInfoBase = 20;
        public const int PageSizeVariacionEmpresa = 20;
        public const int PageSizeVariacionCodigo = 20;
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        //GMME
        public const string NombreRptInsumo = "Insumos.xlsx";
        public const string NombreRpt1 = "Resumen.xlsx";
        public const string NombreRpt2 = "Cuadro1.xlsx";
        public const string NombreRpt3 = "Cuadro2.xlsx";
        public const string NombreRpt4 = "Cuadro3.xlsx";
        public const string NombreRpt5 = "Cuadro4.xlsx";
        public const string NombreRpt6 = "Cuadro5.xlsx";

        //TIPO CONTRATO
        public const int TipoContratoLicitacion = 1;
        public const int TipoContratoBilateral = 5;
        public const int TipoContratoAutoconsumo = 6;

        public bool ValidarPermisoGrabar(object IdOpcion, string sUsuario)
        {
            int id = (IdOpcion != null) ? Convert.ToInt32(IdOpcion) : 0;
            return this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, iGrabar, sUsuario);
            //return true;
        }

        public bool ValidarPermisoEditar(object IdOpcion, string sUsuario)
        {
            int id = (IdOpcion != null) ? Convert.ToInt32(IdOpcion) : 0;
            return this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, iEditar, sUsuario);
            //return true;
        }

        public bool ValidarPermisoNuevo(object IdOpcion, string sUsuario)
        {
            int id = (IdOpcion != null) ? Convert.ToInt32(IdOpcion) : 0;
            return this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, iNuevo, sUsuario);
            //return true;
        }

        public bool ValidarPermisoEliminar(object IdOpcion, string sUsuario)
        {
            int id = (IdOpcion != null) ? Convert.ToInt32(IdOpcion) : 0;
            return this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, id, iEliminar, sUsuario);
            //return true;
        }

        //Retorna una lista de años tomando como inicio 2014 y finalizando 6 años mas al año actual
        public IEnumerable<SelectListItem> ObtenerAnio()
        {
            var ListaAnio = new List<SelectListItem>();
            int iAnioFinal = DateTime.Today.Year + 6;
            for (int i = 2014; i <= iAnioFinal; i++)
            {
                var list = new SelectListItem();
                list.Text = list.Value = i.ToString();
                ListaAnio.Add(list);
            }
            return ListaAnio;
        }

        //Retorna una lista de meses del año
        public IEnumerable<SelectListItem> ObtenerMes()
        {
            var ListaMes = new List<SelectListItem>();
            var list1 = new SelectListItem();
            list1.Text = "Enero"; list1.Value = "1"; ListaMes.Add(list1);
            var list2 = new SelectListItem();
            list2.Text = "Febrero"; list2.Value = "2"; ListaMes.Add(list2);
            var list3 = new SelectListItem();
            list3.Text = "Marzo"; list3.Value = "3"; ListaMes.Add(list3);
            var list4 = new SelectListItem();
            list4.Text = "Abril"; list4.Value = "4"; ListaMes.Add(list4);
            var list5 = new SelectListItem();
            list5.Text = "Mayo"; list5.Value = "5"; ListaMes.Add(list5);
            var list6 = new SelectListItem();
            list6.Text = "Junio"; list6.Value = "6"; ListaMes.Add(list6);
            var list7 = new SelectListItem();
            list7.Text = "Julio"; list7.Value = "7"; ListaMes.Add(list7);
            var list8 = new SelectListItem();
            list8.Text = "Agosto"; list8.Value = "8"; ListaMes.Add(list8);
            var list9 = new SelectListItem();
            list9.Text = "Setiembre"; list9.Value = "9"; ListaMes.Add(list9);
            var list10 = new SelectListItem();
            list10.Text = "Octubre"; list10.Value = "10"; ListaMes.Add(list10);
            var list11 = new SelectListItem();
            list11.Text = "Noviembre"; list11.Value = "11"; ListaMes.Add(list11);
            var list12 = new SelectListItem();
            list12.Text = "Diciembre"; list12.Value = "12"; ListaMes.Add(list12);
            return ListaMes;
        }

        //Retorna una lista de estados
        public IEnumerable<SelectListItem> ObtenerEstados()
        {
            var ListaEstado = new List<SelectListItem>();
            SelectListItem list = new SelectListItem();
            list.Text = "Pendiente";
            list.Value = "PAP"; 
            ListaEstado.Add(list);
            list = new SelectListItem();
            list.Text = "Pendiente de asignacion VTP";
            list.Value = "PVT";
            ListaEstado.Add(list);
            list = new SelectListItem();
            list.Text = "Solicitud de baja";
            list.Value = "SBJ";
            ListaEstado.Add(list);
            list = new SelectListItem();
            list.Text = "Activo";
            list.Value = "ACT";
            ListaEstado.Add(list);
            list = new SelectListItem();
            list.Text = "Baja";
            list.Value = "BAJ";
            ListaEstado.Add(list);
            list = new SelectListItem();
            list.Text = "Rechazado";
            list.Value = "REC";
            ListaEstado.Add(list);
            return ListaEstado.OrderBy(x=>x.Text);

        }

        //Retorna una lista de estados
        public IEnumerable<SelectListItem> ObtenerEstadosSolitudPendienteAprobacion()
        {
            var ListaEstado = new List<SelectListItem>();
            SelectListItem list = new SelectListItem();
            list.Text = "Pendiente - Solicitud de Cambio";
            list.Value = "PEN";
            ListaEstado.Add(list);
            return ListaEstado.OrderBy(x => x.Text);

        }

        //Almacena un archivo en excel en un data set
        public DataSet GeneraDataset(string RutaArchivo, int hoja)
        {
            FileInfo archivo = new FileInfo(RutaArchivo);
            ExcelPackage xlPackage = new ExcelPackage(archivo);
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[hoja];

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            for (int j = 1; j <= ws.Dimension.End.Column; j = j + 1)
            {
                string Columna = "";
                if (ws.Cells[1, j].Value != null) Columna = ws.Cells[1, j].Value.ToString();
                dt.Columns.Add(Columna);
            }

            for (int i = 2; i <= ws.Dimension.End.Row; i = i + 1)
            {
                DataRow row = dt.NewRow();
                for (int j = 1; j <= ws.Dimension.End.Column; j = j + 1)
                {
                    if (ws.Cells[i, j].Value == null)
                        row[j - 1] = "null";
                    else
                        row[j - 1] = ws.Cells[i, j].Value.ToString().Trim();
                }
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            xlPackage.Dispose();
            return ds;
        }

        //Valida que la información ingresada solo contenga numeros y letras
        public string CorregirCodigo(string str)
        {
            string strLimpio = "";
            str = str.ToUpper();
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] >= '0' && str[i] <= '9') ||
                     (str[i] >= 'A' && str[i] <= 'Z')
                )
                { strLimpio = strLimpio + str[i]; }
            }
            return strLimpio;
        }

        public static List<DatosTransferencia> TransformarData(DataTable dt, int nroDias, int idEmpresa,
               out List<string> erroresValor, out List<string> erroresDatos, out List<string> erroresRepetidos , out List<string>  erroresValoresNegativos)
        {
            List<string> codigosErrorValor = new List<string>();
            List<string> codigosErrorDatos = new List<string>();
            List<string> codigosNoRepetidos = new List<string>();
            List<string> codigosRepetidos = new List<string>();
            List<string> codigosValoresNegativos = new List<string>();
            
            List<int> excluirColumnas = new List<int>();
            string[] tipos = { "FINAL", "MEJOR INFORMACIÓN", "Mejor información" };
            try
            {
                List<DatosTransferencia> result = new List<DatosTransferencia>();
                if (dt.Rows.Count == nroDias * 96 + 2)
                {
                    int nroColumnas = dt.Columns.Count;

                    for (int t = 1; t <= nroColumnas - 1; t++)
                    {
                        if (!codigosNoRepetidos.Contains(dt.Rows[0][t].ToString()))
                        {
                            codigosNoRepetidos.Add(dt.Rows[0][t].ToString());
                        }
                        else
                        {
                            codigosRepetidos.Add(dt.Rows[0][t].ToString());
                            excluirColumnas.Add(t);
                        }
                    }

                    for (int indice = 1; indice <= nroDias; indice++)
                    {
                        for (int t = 1; t <= nroColumnas - 1; t++)
                        {
                            DatosTransferencia entity = new DatosTransferencia
                            {
                                Codigobarra = dt.Rows[0][t].ToString(),
                                Tipodato = (tipos.Contains(dt.Rows[1][t].ToString())) ? dt.Rows[1][t].ToString().ToUpper() : "MEJOR INFORMACIÓN",
                                Nrodia = indice,
                                Emprcodi = idEmpresa
                            };

                            bool valorMaximo = true;
                            bool valorError = true;
                            bool valorNegativo = true; 

                            decimal suma = 0;
                            for (int k = 1; k <= 96; k++)
                            {
                                object valor = dt.Rows[k - 1 + (indice - 1) * 96 + 2][t];
                                decimal dvalor = 0;
                                if (valor != null)
                                {
                                    string sValue = valor.ToString();

                                    if (decimal.TryParse(sValue, NumberStyles.Any, CultureInfo.InvariantCulture, out dvalor))
                                    {
                                        entity.GetType().GetProperty("H" + k).SetValue(entity, dvalor);
                                        suma = suma + dvalor;

                                        if (dvalor > (decimal)Funcion.dLimiteMaxEnergia)
                                        {
                                            valorMaximo = false;
                                        }

                                        if (dvalor < 0)
                                        {
                                            valorNegativo = false;
                                        }
                                    }
                                    else
                                    {
                                        valorError = false;
                                    }
                                }
                            }

                            entity.Sumadia = suma;
                            entity.Promedio = suma / 96;

                            if (!excluirColumnas.Contains(t))
                            {
                                result.Add(entity);

                                if (!valorMaximo)
                                    codigosErrorValor.Add(entity.Codigobarra);
                                if (!valorError)
                                    codigosErrorDatos.Add(entity.Codigobarra);
                                if (!valorNegativo)
                                    codigosValoresNegativos.Add(entity.Codigobarra);
                            }
                        }
                    }
                }

                //- Excluimos los valores con errores

                erroresValor = codigosErrorValor;
                erroresDatos = codigosErrorDatos.Distinct().ToList(); 
                erroresRepetidos = codigosRepetidos;
                erroresValoresNegativos = codigosValoresNegativos.Distinct().ToList();

                //List<DatosTransferencia> listCorrectos = result.Where(x => !codigosErrorValor.Any(y => x.Codigobarra == y)).ToList();
                //listCorrectos = listCorrectos.Where(x => !codigosErrorDatos.Any(y => x.Codigobarra == y)).ToList();
                //List<DatosTransferencia> listFinal = listCorrectos.Where(x => !codigosValoresNegativos.Any(y => x.Codigobarra == y)).ToList(); //ValoresNegativos
                //
                List<DatosTransferencia> listCorrectos = result.Where(x => !codigosErrorValor.Any(y => x.Codigobarra == y)).ToList();
                List<DatosTransferencia> listFinal = listCorrectos.Where(x => !codigosErrorDatos.Any(y => x.Codigobarra == y)).ToList(); 
                return listFinal;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public static List<DatosTransferencia> TransformarDataHanson(List<string[]> datos, int nroDias, int idEmpresa, out List<string> erroresValor, out List<string> erroresDatos, string sTipo)
        {
            List<string> codigosErrorValor = new List<string>();
            List<string> codigosErrorDatos = new List<string>();
            //string[] tipos = { "FINAL", "MEJOR INFORMACIÓN" };
            try
            {
                List<DatosTransferencia> result = new List<DatosTransferencia>();
                //Recorrer matriz para grabar detalle
                //Recorremos la matriz que se inicia en la fila 4
                for (int col = 1; col < datos[0].Count(); col++)
                {   //Por Fila
                    if (datos[2][col] == null)
                        break; //FIN - no existe dato en celda

                    for (int indice = 1; indice <= nroDias; indice++)
                    {
                        DatosTransferencia entity = new DatosTransferencia
                        {
                            Codigobarra = datos[0][col].ToString(),
                            Tipodato = sTipo, /*(tipos.Contains(datos[3][col].ToString())) ? datos[3][col].ToString() : "PRELIMINAR",*/
                            Nrodia = indice,
                            Emprcodi = idEmpresa
                        };

                        bool valorMaximo = true;
                        bool valorError = true;
                        decimal suma = 0;

                        for (int k = 1; k <= 96; k++)
                        {
                            object valor = datos[k + (indice - 1) * 96 + 1][col]; //3 por que empieza en la fila siguiente
                            decimal dvalor = 0;
                            if (valor != null)
                            {
                                string sValue = valor.ToString();

                                if (decimal.TryParse(sValue, NumberStyles.Any, CultureInfo.InvariantCulture, out dvalor))
                                {
                                    entity.GetType().GetProperty("H" + k).SetValue(entity, dvalor);
                                    suma = suma + dvalor;

                                    if (dvalor > (decimal)Funcion.dLimiteMaxEnergia)
                                    {
                                        valorMaximo = false;
                                    }
                                }
                                else
                                {
                                    valorError = false;
                                }
                            }
                            else
                                entity.GetType().GetProperty("H" + k).SetValue(entity, dvalor);
                        }

                        entity.Sumadia = suma;
                        entity.Promedio = suma / 96;

                        if (!string.IsNullOrEmpty(entity.Codigobarra))
                            result.Add(entity);

                        if (!valorMaximo && !string.IsNullOrEmpty(entity.Codigobarra))
                            codigosErrorValor.Add(entity.Codigobarra);
                        if (!valorError && !string.IsNullOrEmpty(entity.Codigobarra))
                            codigosErrorDatos.Add(entity.Codigobarra);
                    }

                }
                //- Excluimos los valores con errores
                erroresValor = codigosErrorValor;
                erroresDatos = codigosErrorDatos;
                List<DatosTransferencia> listCorrectos = result.Where(x => !codigosErrorValor.Any(y => x.Codigobarra == y)).ToList();
                List<DatosTransferencia> listFinal = listCorrectos.Where(x => !codigosErrorDatos.Any(y => x.Codigobarra == y)).ToList();
                return listFinal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inicializa las dimensiones de la matriz de valores del objeto excel web
        /// </summary>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public static string[][] InicializaMatrizExcelEspecial(int rowsHead, int nFil, int colsHead, int nCol)
        {
            string[][] matriz = new string[rowsHead][];
            for (int i = 0; i <= nFil; i++)
            {
                matriz[i] = new string[nCol + colsHead];
                for (int j = 0; j <= nCol; j++)
                {
                    matriz[i][j] = string.Empty;
                }
            }
            return matriz;
        }

        /// <summary>
        /// Convierte una lista de mediciones en una Matriz Excel Web
        /// </summary>
        /// <param name="data"></param>
        /// <param name="colHead"></param>
        /// <param name="nCol"></param>
        /// <param name="filHead"></param>
        /// <param name="nFil"></param>
        /// <returns></returns>
        public static string[][] GetMatrizExcel(List<string> data, int colHead, int nCol, int filHead, int nFil)
        {
            var inicio = 0;//(nCol + colHead - 1) * filHead;
            int col = 0;
            int fila = 0;
            string[][] arreglo = new string[nFil][];
            arreglo[0] = new string[nCol];
            for (int i = inicio; i < data.Count(); i++)
            {
                string valor = data[i];
                if (col == nCol)
                {
                    fila++;
                    col = 0;
                    arreglo[fila] = new string[nCol];
                }
                arreglo[fila][col] = valor;
                col++;
            }
            return arreglo;
        }

        /// <summary>
        /// Inicializa las dimensiones de la matriz de valores del objeto excel web
        /// </summary>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public static string[][] InicializaMatrizExcel(int rowsHead, int nFil, int colsHead, int nCol)
        {
            string[][] matriz = new string[nFil + rowsHead][];
            for (int i = 0; i <= nFil; i++)
            {

                matriz[i] = new string[nCol + colsHead];
                for (int j = 0; j <= nCol; j++)
                {
                    matriz[i][j] = string.Empty;
                }
            }
            return matriz;
        }
    }
}

