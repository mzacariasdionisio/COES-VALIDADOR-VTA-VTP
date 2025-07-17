using COES.MVC.Extranet.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using OfficeOpenXml;
using System.IO;
using COES.Dominio.DTO.Transferencias;
using System.Globalization;

namespace COES.MVC.Extranet.Areas.Transferencias.Helper
{
    public class Funcion
    {
        //CONSTANTES
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

        //NOMBRE DE LOS ARCHIVOS DE RESULTADO
        public const string NombreReporteFormatoEntregaRetiroExcel = "ReporteFormatoEntregaRetiro.xlsx";
        public const string NombreReporteRatioCumplimientoExcel = "ReporteRatioCumplimiento.xlsx";
        public const string NombreReporteInformacionBaseExcel = "ReporteInformacionBase.xlsx";
        public const string NombreReporteCostoMarginalExcel = "ReporteCostoMarginal.xlsx";
        public const string NombreReporteCostoMarginalBarraExcel = "ReporteCostoMarginalBarra.xlsx";
        public const string NombreReporteEntregasRetirosEnergiaValorizadosExcel = "EntregasRetirosEnergiaValorizados.xlsx";
        public const string NombreReporteResumenEnergiaMensualExcel = "ReporteResumenEnergiaMensual.xlsx";
        public const string NombreDeterminacionSaldosTransferenciasExcel = "DeterminacionSaldosTransferencias.xlsx";
        public const string NombrePagosTransferenciasEnergíaActivaExcel = "PagosTransferenciasEnergíaActiva.xlsx";
        public const string NombreEntregasRetirosEnergiaExcel = "EntregasRetirosEnergia.xlsx";
        public const string NombreReporteIngresoCompensacionExcel = "ReporteIngresoCompensacion.xlsx";
        public const string NombreReporteCodigoRetiroExcel = "ReporteCodigoRetiro.xlsx";
        public const string NombreReporteEntregasRetirosEnergiaValorizadosExcel15min = "ReporteEntregasRetirosEnergiaValorizadosExcel15min.xlsx";

        //NOMBRE DE LOS ARCHIVOS DE RESULTADO PDF
        public const string NombreConstanciaEnvioInformacionPDF = "ConstanciaEnvio.pdf";

        //PAGINACION
        public const int PageSizeMercadoLibre = 10;
        public const int PageSizePeriodoRentaCongestion = 30;
        public const int PageSize = 20;
        public const int NroPageShow = 10;
        public const int PageSizeCodigoEntrega = 20;
        public const int PageSizeCodigoRetiro = 20;
        public const int PageSizeCodigoRetiroSC = 20;
        public const int PageSizeCodigoInfoBase = 20;

        //ESTADOS
        public const string EstadoPendiente = "PAP";
        public const string EstadoPendientePVT = "PVT";
        public const string EstadoActivo = "ACT";
        public const string EstadoInactivo = "INA";
        public const string EstadoBaja = "BAJ";
        public const string EstadoSolicitudBaja = "SBJ";
        public const string EstadoRechazado = "REC";

        //ASSETEC 202001
        public const string PlantillaPotenciasContratadasExcel = "PlantillaPotenciasContratadas.xlsx";
        public const string FormatoFechaHora = "dd/MM/yyyy HH:mm";
        //ASSETEC 202002 - Paginacion del Handsontable
        public const int iNroGrupos = 11;

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
                if (i == ws.Dimension.End.Row) {
                    for (int j = 1; j <= ws.Dimension.End.Column; j = j + 1)
                    {
                        if (ws.Cells[i, j].Value != null)
                        {
                            string k = "ss";
                        }
                    }
                }
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

        public static List<SeguridadServicio.EmpresaDTO> ObtenerEmpresasPorUsuario(string usuario)
        {
            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
            List<SeguridadServicio.EmpresaDTO> list = new List<SeguridadServicio.EmpresaDTO>();

            if (usuario == "camila.ayllon" || usuario == "raul.castro" || usuario == "lvasquez" || usuario == "ppajan" || usuario == "maritza.arapa")
            {
                list = seguridad.ListarEmpresas().ToList().Where(x => x.TIPOEMPRCODI == 3 || x.EMPRCODI == 11772 || x.EMPRCODI == 13).ToList();

             
            }
            else
            {
                list = seguridad.ObtenerEmpresasActivasPorUsuario(usuario).ToList();

                if (list.Where(x => x.EMPRCODI == 12242).Count() > 0)
                {
                    SeguridadServicio.EmpresaDTO emp = new SeguridadServicio.EmpresaDTO()
                    {
                        EMPRCODI = 11088,
                        EMPRNOMB = "ABENGOA TRANSMISION SUR",
                        TIPOEMPRCODI = 1
                    };

                    list.Add(emp);
                }

                
            }

            return list.OrderBy(X => X.EMPRNOMB).ToList();
        }

        public static List<SeguridadServicio.EmpresaDTO> ObtenerEmpresasPorUsuario(string usuario, List<int> idsEmpresas)
        {
            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
            List<SeguridadServicio.EmpresaDTO> list = new List<SeguridadServicio.EmpresaDTO>();

            if (usuario == "camila.ayllon" || usuario == "raul.castro" || usuario == "lvasquez" || usuario == "ppajan" || usuario == "maritza.arapa")
            {
                list = seguridad.ListarEmpresas().ToList().Where(x => x.TIPOEMPRCODI == 3 || x.EMPRCODI == 11772 || x.EMPRCODI == 13 || x.EMPRCODI == 12439 || 
                idsEmpresas.Contains(x.EMPRCODI)).ToList();

                
            }
            else
            {
                list = seguridad.ObtenerEmpresasActivasPorUsuario(usuario).ToList();

                if (list.Where(x => x.EMPRCODI == 12242).Count() > 0)
                {
                    SeguridadServicio.EmpresaDTO emp = new SeguridadServicio.EmpresaDTO()
                    {
                        EMPRCODI = 11088,
                        EMPRNOMB = "ABENGOA TRANSMISION SUR",
                        TIPOEMPRCODI = 1
                    };
                    list.Add(emp);
                }

               
            }

            return list.OrderBy(X => X.EMPRNOMB).ToList();
        }

        public static List<DatosTransferencia> TransformarData(DataTable dt, int nroDias, int idEmpresa,
            out List<string> erroresValor, out List<string> erroresDatos)
        {
            List<string> codigosErrorValor = new List<string>();
            List<string> codigosErrorDatos = new List<string>();
            string[] tipos = { "FINAL", "MEJOR INFORMACIÓN" };
            try
            {
                List<DatosTransferencia> result = new List<DatosTransferencia>();
                if (dt.Rows.Count == nroDias * 96 + 2)
                {
                    int nroColumnas = dt.Columns.Count;

                    for (int indice = 1; indice <= nroDias; indice++)
                    {
                        for (int t = 1; t <= nroColumnas - 1; t++)
                        {
                            DatosTransferencia entity = new DatosTransferencia
                            {
                                Codigobarra = dt.Rows[0][t].ToString(),
                                Tipodato = (tipos.Contains(dt.Rows[1][t].ToString())) ? dt.Rows[1][t].ToString() : "PRELIMINAR",
                                Nrodia = indice,
                                Emprcodi = idEmpresa
                            };

                            bool valorMaximo = true;
                            bool valorError = true;

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
                                    }
                                    else
                                    {
                                        valorError = false;
                                    }
                                }
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
                            Codigobarra = datos[2][col].ToString(),
                            Tipodato = sTipo, /*(tipos.Contains(datos[3][col].ToString())) ? datos[3][col].ToString() : "PRELIMINAR",*/
                            Nrodia = indice,
                            Emprcodi = idEmpresa
                        };

                        bool valorMaximo = true;
                        bool valorError = true;
                        decimal suma = 0;

                        for (int k = 1; k <= 96; k++)
                        {
                            object valor = datos[k + (indice - 1) * 96 + 3][col]; //3 por que empieza en la fila siguiente
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

        //ASSETEC 202001
        /// <summary>
        /// Valida que la información ingresada sea un numero valido, caso contrario devuelve cero
        /// /// </summary>
        /// <param name="sValor">Cadena de texto</param>
        public static decimal ValidarNumero(string sValor)
        {
            decimal dNumero;
            if (!sValor.Equals("") && (Decimal.TryParse(sValor, out dNumero)))
            {
                return dNumero;
            }
            else
            {
                return 0;
            }
        }
    }
}
