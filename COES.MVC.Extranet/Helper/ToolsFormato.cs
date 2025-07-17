using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Configuration;

namespace COES.MVC.Extranet.Helper
{
    public class ToolsFormato
    {
        /// <summary>
        /// Verifica id de formato en el archivo excel que se requiere cargar en el grid web
        /// </summary>
        /// <param name="file"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public static int VerificarIdsFormato(string file, int idEmpresa, int idFormato)
        {
            int retorno = 1;
            int idEmpresaArchivo;
            int idFormatoEmpresa;
            FileInfo fileInfo = new FileInfo(file);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                int posIdEmpresa = ConstantesFormato.ColEmpresaExtranet;
                int posIdFormato = ConstantesFormato.ColFormatoExtranet;
                if (ConstantesFormatoMedicion.IdFormatoGeneracionRERDiario == idFormato || ConstantesFormatoMedicion.IdFormatoGeneracionRERSemanal == idFormato)
                {
                    posIdEmpresa = ConstantesFormato.ColEmpresaExtranetNuevo;
                    posIdFormato = ConstantesFormato.ColFormatoExtranetNuevo;
                }

                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                string valorEmp = string.Empty;
                if (ws.Cells[1, posIdEmpresa].Value != null)
                    valorEmp = ws.Cells[1, posIdEmpresa].Value.ToString();
                if (!int.TryParse(valorEmp, NumberStyles.Any, CultureInfo.InvariantCulture, out idEmpresaArchivo))
                    idEmpresa = 0;
                string valorFormato = string.Empty;
                if (ws.Cells[1, posIdFormato].Value != null)
                    valorFormato = ws.Cells[1, posIdFormato].Value.ToString();
                if (!int.TryParse(valorFormato, NumberStyles.Any, CultureInfo.InvariantCulture, out idFormatoEmpresa))
                    idFormatoEmpresa = 0;
            }

            if (idEmpresaArchivo != idEmpresa)
            {
                retorno = -1;
            }
            if (idFormatoEmpresa != idFormato)
            {
                retorno = -2;
            }

            return retorno;
        }

        public static DateTime GetFechaProcesoAnterior(int? periodo, DateTime fechaProcesoActual)
        {
            DateTime fechaAnterior = fechaProcesoActual;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoDiario:
                    fechaAnterior = fechaProcesoActual.AddDays(-1);
                    break;
                case ParametrosFormato.PeriodoSemanal:
                    fechaAnterior = fechaProcesoActual.AddDays(-7);
                    break;
                case ParametrosFormato.PeriodoMensualSemana:
                    break;
                case ParametrosFormato.PeriodoMensual:
                    fechaAnterior = fechaProcesoActual.AddMonths(-1);
                    break;
            }

            return fechaAnterior;
        }

        /// <summary>
        /// Carga Informacion de Despacho diarios en el model para visualizacion de la pagina web
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lista96"></param>
        /// <param name="listaCambios"></param>
        /// <param name="fecha"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public static string[][] ObtenerListaExcelDataM96(FormatoModel model, List<MeMedicion96DTO> lista96, List<MeCambioenvioDTO> listaCambios, string fecha, int idEnvio)
        {
            DateTime fechaIni = model.Formato.FechaInicio;
            string celdaFechaHora = string.Empty;
            int nFil = model.FilasCabecera + model.Formato.Formathorizonte * model.Formato.RowPorDia;
            int kFilDatos = model.FilasCabecera - 1; //fila de inicio de datos de las matriz
            var listaPto = model.ListaHojaPto;
            int nCol = listaPto.Count + 1;

            //inicializar matriz excel
            string[][] lista = new string[nFil][];
            for (int i = 0; i < nFil; i++)
                lista[i] = new string[nCol];

            for (int i = 0; i < nFil; i++)
                for (int j = 0; j < nCol; j++)
                    lista[i][j] = "";


            int idPto = 0;
            int ptoInfoCodi = 0;
            int tipoPtomedicodi = 0;
            int? famcodi = -1;

            //recorrer los días
            //Medifecha

            model.ListaCambios = new List<CeldaCambios>();
            var contador = 0;
            var kfilasIniDia = kFilDatos;
            DateTime mediFechaTmp = DateTime.Now;
            var listaFecha = GetAllDatesByFormatoAndFecha(model.Formato);
            foreach (var f in listaFecha)
            {
                mediFechaTmp = (DateTime)f;

                /// establecer correctamente los puntos de los cambios
                if (idEnvio > 0)
                {
                    for (var z = 1; z < nCol; z++)
                    {
                        idPto = listaPto[z - 1].Ptomedicodi;
                        ptoInfoCodi = listaPto[z - 1].Tipoinfocodi;
                        tipoPtomedicodi = listaPto[z - 1].Tptomedicodi;

                        var regCambio = listaCambios.Where(x => x.Ptomedicodi == idPto && x.Cambenvfecha.Date == mediFechaTmp && x.Tipoinfocodi == ptoInfoCodi && x.Tipoptomedicodi == tipoPtomedicodi && x.Cambenvcolvar != null).FirstOrDefault();
                        if (regCambio != null)
                        {
                            var cambios = regCambio.Cambenvcolvar.Split(',');
                            for (var i = 0; i < cambios.Count(); i++)
                            {
                                var col = z;
                                var row = contador * 96 + model.FilasCabecera + int.Parse(cambios[i]) - 1;
                                model.ListaCambios.Add(new CeldaCambios()
                                {
                                    Row = row,
                                    Col = col
                                });
                            }
                        }
                    }
                }

                ///
                for (var z = 0; z < nCol; z++)
                {
                    MeMedicion96DTO find = null;
                    if (z > 0)
                    {
                        idPto = listaPto[z - 1].Ptomedicodi;
                        ptoInfoCodi = listaPto[z - 1].Tipoinfocodi;
                        tipoPtomedicodi = listaPto[z - 1].Tptomedicodi;
                        famcodi = -1;
                        find = lista96.Find(x => x.Ptomedicodi == idPto && x.Tipoinfocodi == ptoInfoCodi && x.Tipoptomedicodi == tipoPtomedicodi && x.Medifecha == mediFechaTmp);

                        //para medidores de generacion, validacion de solares
                        if (model.UtilizaFiltroCentral && model.ValidaMedidorGeneracion)
                        {
                            var equicodiHoja = model.ListaHojaPto.Where(x => x.Ptomedicodi == idPto).FirstOrDefault().Equipadre;
                            var equipotmp = model.ListaEquipo.Where(x => x.Equicodi == equicodiHoja).FirstOrDefault();
                            famcodi = equipotmp != null ? equipotmp.Famcodi : -1;
                        }
                    }
                    for (int k = 1; k <= 96; k++)
                    {
                        if (z == 0) // imprimimos 1ra columna
                        {
                            celdaFechaHora = FormatoHelper.ObtenerCeldaFecha((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, 1, 0, k, model.Formato.TipoAgregarTiempoAdicional, mediFechaTmp);
                            lista[kfilasIniDia + k][0] = celdaFechaHora;
                        }
                        else // imprimimos datos 
                        {
                            if (find != null)
                            {
                                decimal? valor = (decimal?)find.GetType().GetProperty("H" + k).GetValue(find, null);
                                if (valor != null)
                                {
                                    lista[kfilasIniDia + k][z] = valor.ToString();
                                }
                            }

                            //if (model.UtilizaFiltroCentral && model.ValidaMedidorGeneracion)
                            //{
                            //    if (famcodi == ConstantesHorasOperacion.IdTipoSolar &&
                            //        ((k >= 1 && k < model.ParamSolar.HInicio) || (k > model.ParamSolar.HFin && k <= 96)))
                            //    {
                            //        lista[kfilasIniDia + k][z] = "0";
                            //    }
                            //}
                        }
                    }
                }

                contador++;
                kfilasIniDia = kFilDatos + contador * model.Formato.RowPorDia;
            }


            return lista;
        }

        /// <summary>
        /// Carga Informacion de medicion48 en el model para visualizacion de la pagina web
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lista48"></param>
        /// <param name="listaCambios"></param>
        /// <param name="fecha"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public static string[][] ObtenerListaExcelDataM48(FormatoModel model, List<MeMedicion48DTO> lista48, List<MeCambioenvioDTO> listaCambios, string fecha, int idEnvio, List<MeScadaSp7DTO> listaScada = null)
        {
            DateTime fechaIni = model.Formato.FechaInicio;
            string celdaFechaHora = string.Empty;
            int nFil = model.FilasCabecera + model.Formato.Formathorizonte * model.Formato.RowPorDia;
            int kFilDatos = model.FilasCabecera - 1; //fila de inicio de datos de las matriz
            var listaPto = model.ListaHojaPto;
            int nCol = listaPto.Count + 1;

            //inicializar matriz excel
            string[][] lista = new string[nFil][];
            for (int i = 0; i < nFil; i++)
                lista[i] = new string[nCol];

            for (int i = 0; i < nFil; i++)
                for (int j = 0; j < nCol; j++)
                    lista[i][j] = "";


            int idPto = 0;
            int ptoInfoCodi = 0;
            int hojacodi = 0;

            //
            model.ListaCambios = new List<CeldaCambios>();

            //media hora actual
            DateTime dt2 = DateTime.Now;
            int hActual = (dt2.TimeOfDay.TotalMinutes == 0) ? 48 : (int)dt2.TimeOfDay.TotalMinutes / 30;
            hActual = hActual < 48 ? hActual + 1 : 48;

            var contador = 0;
            var kfilasIniDia = kFilDatos;
            DateTime mediFechaTmp = DateTime.Now;
            var listaFecha = GetAllDatesByFormatoAndFecha(model.Formato);
            //lista de valores de H
            List<int> valores = new List<int>();

            foreach (var f in listaFecha)
            {
                mediFechaTmp = (DateTime)f;

                /// establecer correctamente los puntos de los cambios
                if (idEnvio > 0)
                {
                    for (var z = 1; z < nCol; z++)
                    {
                        idPto = listaPto[z - 1].Ptomedicodi;
                        ptoInfoCodi = listaPto[z - 1].Tipoinfocodi;
                        hojacodi = listaPto[z - 1].Hojacodi;
                        var flagCheck = listaPto[z - 1].Hptoindcheck;

                        if (flagCheck != "S")
                        {
                            MeCambioenvioDTO regCambio = null;
                            if (!model.UtilizaHoja)
                            {
                                regCambio = listaCambios.Where(x => x.Ptomedicodi == idPto && x.Cambenvfecha.Date == mediFechaTmp && x.Tipoinfocodi == ptoInfoCodi && x.Cambenvcolvar != null).FirstOrDefault();
                            }
                            else
                            {
                                regCambio = listaCambios.Where(x => x.Ptomedicodi == idPto && x.Cambenvfecha.Date == mediFechaTmp && x.Tipoinfocodi == ptoInfoCodi && x.Cambenvcolvar != null && x.Hojacodi == hojacodi).FirstOrDefault();
                            }
                            if (regCambio != null)
                            {
                                var cambios = regCambio.Cambenvcolvar.Split(',');
                                for (var i = 0; i < cambios.Count(); i++)
                                {
                                    var col = z;
                                    var row = contador * 48 + model.FilasCabecera + int.Parse(cambios[i]) - 1;
                                    model.ListaCambios.Add(new CeldaCambios()
                                    {
                                        Row = row,
                                        Col = col
                                    });
                                }
                            }
                        }
                    }
                }

                ///
                for (var z = 0; z < nCol; z++)
                {
                    MeMedicion48DTO find = null;
                    if (z > 0)
                    {
                        idPto = listaPto[z - 1].Ptomedicodi;
                        ptoInfoCodi = listaPto[z - 1].Tipoinfocodi;
                        hojacodi = listaPto[z - 1].Hojacodi;
                        find = !model.UtilizaHoja ? lista48.Find(x => x.Ptomedicodi == idPto && x.Tipoinfocodi == ptoInfoCodi && x.Medifecha == mediFechaTmp)
                            : lista48.Find(x => x.Ptomedicodi == idPto && x.Tipoinfocodi == ptoInfoCodi && x.Medifecha == mediFechaTmp && x.Hojacodi == hojacodi);

                        if (find != null)
                        {
                            for (int i = 48; i >= 1; i--)
                            {
                                decimal? valor = (decimal?)find.GetType().GetProperty("H" + i.ToString()).GetValue(find, null);
                                if (valor != null)
                                {
                                    valores.Add(i);
                                    break;
                                }
                            }
                        }
                    }
                    for (int k = 1; k <= 48; k++)
                    {
                        if (z == 0) // imprimimos 1ra columna
                        {
                            celdaFechaHora = FormatoHelper.ObtenerCeldaFecha((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, 1, 0, k, model.Formato.TipoAgregarTiempoAdicional, mediFechaTmp);
                            lista[kfilasIniDia + k][0] = celdaFechaHora;
                        }
                        else // imprimimos datos 
                        {
                            var regPto = listaPto[z - 1];

                            //columna datos
                            if (regPto.Hptoindcheck != "S")
                            {
                                if (find != null)
                                {
                                    decimal? valor = (decimal?)find.GetType().GetProperty("H" + k).GetValue(find, null);
                                    if (valor != null)
                                        lista[kfilasIniDia + k][z] = valor.ToString();

                                }
                            }

                            #region Check para horario seleccionado

                            if (regPto.Hptoindcheck == "S")
                            {
                                string sCheck = ConstantesFormato.TieneSinCheckExtranet; //default unchecked
                                if (regPto.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW) //caudal
                                {
                                    sCheck = ConstantesFormato.TieneConCheckExtranet; //default checked para caudal
                                    if (hActual <= k) sCheck = ConstantesFormato.TieneSinCheckExtranet; //uncheck
                                }

                                if (find != null)
                                {
                                    string valorHCheck = (string)find.GetType().GetProperty("E" + k).GetValue(find, null);
                                    sCheck = ConstantesFormato.TieneSinCheckExtranet; //unchecked
                                    if (valorHCheck == ConstantesFormato.TieneConCheckExtranet) sCheck = ConstantesFormato.TieneConCheckExtranet;//check
                                }

                                //valor por defecto sino se usa el importado en excel
                                if (string.IsNullOrEmpty(lista[kfilasIniDia + k][z]))
                                    lista[kfilasIniDia + k][z] = sCheck;

                                if (regPto.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW && hActual <= k)
                                    lista[kfilasIniDia + k][z] = ConstantesFormato.TieneSinCheckExtranet;
                            }

                            #endregion
                        }
                    }
                }

                contador++;
                kfilasIniDia = kFilDatos + contador * model.Formato.RowPorDia;
            }

            //métofo para completar datos con Scada
            if (idEnvio == 0 && model.ValidaTiempoReal)
                lista = GetCompletaScada(model, listaScada, lista, valores, kFilDatos, fecha);
            return lista;
        }

        /// <summary>
        /// Completar datos con informacion scada
        /// </summary>
        /// <param name="model"></param>
        /// <param name="listaScada"></param>
        /// <param name="lista"></param>
        /// <param name="valores"></param>
        /// <param name="kFilDatos"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static string[][] GetCompletaScada(FormatoModel model, List<MeScadaSp7DTO> listaScada, string[][] lista, List<int> valores, int kFilDatos, string fecha)
        {
            int mayor = valores.Count > 0 ? valores.Max() + 1 : 1;
            model.Handson.HMaximoData48Enviado = mayor;
            var listaPto = model.ListaHojaPto;
            int nCol = listaPto.Count + 1;
            int idPto = 0;
            int ptoInfoCodi = 0;
            DateTime fechaIni = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            for (var z = 0; z < nCol; z++)
            {
                if (z > 0)
                {
                    idPto = listaPto[z - 1].Ptomedicodi;
                    ptoInfoCodi = listaPto[z - 1].Tipoinfocodi;
                }
                for (int k = mayor; k <= 48; k++)
                {
                    if (z != 0)
                    {
                        // coordenadas de celda: [kFilDatos + k][z]
                        if (model.Handson.MatrizTipoEstado[kFilDatos + k][z] == 1) // si la celda esta activada 
                        {
                            //valor = encontrar valor de la listaScada
                            var find = listaScada.Find(x => x.Ptomedicodi == idPto && x.Tipoinfocodi == ptoInfoCodi && x.Medifecha == fechaIni);
                            if (find != null)
                            {
                                var Hk = 2 * k;
                                decimal? valor = (decimal?)find.GetType().GetProperty("H" + Hk).GetValue(find, null);
                                if (valor != null)
                                    lista[kFilDatos + k][z] = valor.ToString();
                            }
                        }
                    }
                }
            }

            //Indicar que tiene data scada para tal empresa y día
            model.TieneDataScada = listaScada.Count() > 0;

            return lista;
        }

        /// <summary>
        /// Carga Informacion de Despacho diarios en el model para visualizacion de la pagina web
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lista96"></param>
        /// <param name="listaCambios"></param>
        /// <param name="fecha"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public static string[][] ObtenerListaExcelDataM1(FormatoModel model, List<MeMedicion1DTO> lista1, List<MeCambioenvioDTO> listaCambios, string fecha, int idEnvio)
        {
            DateTime fechaIni = model.Formato.FechaInicio;
            string celdaFechaHora = string.Empty;
            int nFil = model.FilasCabecera + model.Formato.Formathorizonte * model.Formato.RowPorDia;
            int kFilDatos = model.FilasCabecera - 1; //fila de inicio de datos de las matriz
            var listaPto = model.ListaHojaPto;
            int nCol = listaPto.Count + 1;

            //inicializar matriz excel
            string[][] lista = new string[nFil][];
            for (int i = 0; i < nFil; i++)
                lista[i] = new string[nCol];

            for (int i = 0; i < nFil; i++)
                for (int j = 0; j < nCol; j++)
                    lista[i][j] = "";


            int idPto = 0;
            int ptoInfoCodi = 0;
            int tipoPtomedicodi = 0;

            //recorrer los días
            //Medifecha

            model.ListaCambios = new List<CeldaCambios>();
            var contador = 0;
            var kfilasIniDia = kFilDatos;
            DateTime mediFechaTmp = DateTime.Now;
            var listaFecha = GetAllDatesByFormatoAndFecha(model.Formato);
            foreach (var f in listaFecha)
            {
                mediFechaTmp = (DateTime)f;

                /// establecer correctamente los puntos de los cambios
                if (idEnvio > 0)
                {
                    for (var z = 1; z < nCol; z++)
                    {
                        idPto = listaPto[z - 1].Ptomedicodi;
                        ptoInfoCodi = listaPto[z - 1].Tipoinfocodi;
                        tipoPtomedicodi = listaPto[z - 1].Tptomedicodi;

                        var regCambio = listaCambios.Where(x => x.Ptomedicodi == idPto && x.Cambenvfecha.Date == mediFechaTmp && x.Tipoinfocodi == ptoInfoCodi && x.Tipoptomedicodi == tipoPtomedicodi && x.Cambenvcolvar != null).FirstOrDefault();
                        if (regCambio != null)
                        {
                            var cambios = regCambio.Cambenvcolvar.Split(',');
                            for (var i = 0; i < cambios.Count(); i++)
                            {
                                var col = z;
                                var row = contador * 1 + model.FilasCabecera + int.Parse(cambios[i]) - 1;
                                model.ListaCambios.Add(new CeldaCambios()
                                {
                                    Row = row,
                                    Col = col
                                });
                            }
                        }
                    }
                }

                ///
                for (var z = 0; z < nCol; z++)
                {
                    MeMedicion1DTO find = null;
                    if (z > 0)
                    {
                        idPto = listaPto[z - 1].Ptomedicodi;
                        ptoInfoCodi = listaPto[z - 1].Tipoinfocodi;
                        tipoPtomedicodi = listaPto[z - 1].Tptomedicodi;
                        find = lista1.Find(x => x.Ptomedicodi == idPto && x.Tipoinfocodi == ptoInfoCodi && x.Tipoptomedicodi == tipoPtomedicodi && x.Medifecha == mediFechaTmp);
                    }
                    for (int k = 1; k <= 1; k++)
                    {
                        if (z == 0) // imprimimos 1ra columna
                        {
                            celdaFechaHora = FormatoHelper.ObtenerCeldaFecha((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, 1, 0, k, model.Formato.TipoAgregarTiempoAdicional, mediFechaTmp);
                            lista[kfilasIniDia + k][0] = celdaFechaHora;
                        }
                        else // imprimimos datos 
                        {
                            if (find != null)
                            {
                                decimal? valor = (decimal?)find.GetType().GetProperty("H" + k).GetValue(find, null);
                                if (valor != null)
                                {
                                    lista[kfilasIniDia + k][z] = valor.ToString();
                                }
                            }
                        }
                    }
                }

                contador++;
                kfilasIniDia = kFilDatos + contador * model.Formato.RowPorDia;
            }


            return lista;
        }

        /// <summary>
        /// Ubica la posicion de una columna en el grid
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        public static int PosListaPunto(List<MeHojaptomedDTO> lista, int orden)
        {
            int pos = -1;
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].Hojaptoorden == orden)
                    pos = i;
            }
            return pos;
        }

        /// <summary>
        /// Borra archivo fisico
        /// </summary>
        /// <param name="archivo"></param>
        public static void BorrarArchivo(String archivo)
        {
            if (System.IO.File.Exists(@archivo))
            {
                try
                {
                    System.IO.File.Delete(@archivo);
                }
                catch (System.IO.IOException e)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Lee archivo excel cargado y llena matriz de datos para visualizacion web
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public static string[][] LeerExcelFile(string file, List<MeHojaptomedDTO> listaPto, MeFormatoDTO formato, string nombreHoja)
        {
            string sheet = nombreHoja != null ? nombreHoja : Constantes.HojaFormatoExcel;
            int rowcabecera = formato.Formatrows;
            int rowsDatos = formato.RowPorDia * formato.Formathorizonte;
            DateTime fechaIni = formato.FechaInicio;

            string celdaFechaHora = string.Empty;
            FileInfo fileInfo = new FileInfo(file);
            double numero = 0;
            int nFil = rowcabecera + rowsDatos;
            int nCol = listaPto.Count + 1;

            string[][] lista = new string[nFil][];
            for (int i = 0; i < nFil; i++)
                lista[i] = new string[nCol];

            for (int i = 0; i < nFil; i++)
                for (int j = 0; j < nCol; j++)
                    lista[i][j] = "";
            lista[0][0] = "CENTRAL";
            lista[1][0] = "GRUPO";
            lista[2][0] = "FECHA/UNIDAD";
            for (int i = 1; i < nCol; i++)
            {
                lista[0][i] = listaPto[i - 1].Equipopadre;
                lista[1][i] = listaPto[i - 1].Equinomb;
                lista[2][i] = "(" + listaPto[i - 1].Tipoinfoabrev + ")";
            }

            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[sheet];

                if (ws == null)
                {
                    throw new Exception("No existe la pestaña [" + sheet + "]");
                }

                /// Verificar Formato
                int filaIniData = ConstantesFormato.FilaExcelData;
                int colIniData = ConstantesFormato.ColExcelData; 
                if (ConstantesFormatoMedicion.IdFormatoGeneracionRERDiario == formato.Formatcodi || ConstantesFormatoMedicion.IdFormatoGeneracionRERSemanal == formato.Formatcodi)
                {
                    filaIniData = ConstantesFormato.FilaExcelDataNuevo;
                    colIniData = ConstantesFormato.ColExcelDataNuevo;
                }

                for (int i = 0; i < nFil - rowcabecera; i++)
                {
                    for (int j = 0; j < nCol - 1; j++)
                    {
                        string valor = (ws.Cells[i + (rowcabecera) + filaIniData, j + 1 + colIniData].Value != null) ?
                           ws.Cells[i + (rowcabecera) + filaIniData, j + 1 + colIniData].Value.ToString() : string.Empty;

                        if (EsNumero(valor))
                        {
                            double.TryParse(valor, out numero);
                            valor = numero.ToString("0.###########################################");
                        }
                        else
                        {
                            if (valor == "E") valor = ConstantesFormato.TieneConCheckExtranet;
                            if (valor == "P") valor = ConstantesFormato.TieneSinCheckExtranet;
                        }
                        lista[i + rowcabecera][j + 1] = valor;

                    }
                }
            }

            for (var i = 1; i <= rowsDatos; i++)
            {
                celdaFechaHora = FormatoHelper.ObtenerCeldaFecha(formato.Formatperiodo.Value, formato.Formatresolucion.Value, 1, 0, i, formato.TipoAgregarTiempoAdicional, fechaIni);
                lista[i + rowcabecera - 1][0] = celdaFechaHora;

            }
            return lista;
        }

        /// <summary>
        /// Funcion que verifica si una cadena es numero(byte,entero o decimal)
        /// </summary>
        /// <param name="numString"></param>
        /// <returns></returns>
        private static Boolean EsNumero(string numString)
        {
            Boolean isNumber = false;
            long number1 = 0;
            bool canConvert = long.TryParse(numString, out number1);
            if (canConvert == true)
                isNumber = true;
            else
            {
                byte number2 = 0;
                canConvert = byte.TryParse(numString, out number2);
                if (canConvert == true)
                    isNumber = true;
                else
                {
                    double number3 = 0;

                    canConvert = double.TryParse(numString, out number3);
                    if (canConvert == true)
                        isNumber = true;

                }
            }
            return isNumber;
        }

        public static short[][] IncializaMatrizEstado(List<MeHojaptomedDTO> listaPtos, int nFilCabecera, int nFilasDatos, short valorInicio, int horizonte)
        {
            int nFil = nFilCabecera + horizonte * nFilasDatos; // numero de filas de las matriz incluido cabecera
            int nCol = listaPtos.Count + 1;
            short[][] lista = new short[nFil][];

            for (int i = 0; i < nFil; i++)
                lista[i] = new short[nCol];

            for (int i = 0; i < nFil; i++)
                for (int j = 0; j < nCol; j++)
                    lista[i][j] = valorInicio;

            return lista;
        }

        /// <summary>
        /// Deshabilita el ingreso de datos para las horas no permitidas (centrales solares)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="matrizTipoEstado"></param>
        /// <returns></returns>
        public static short[][] InicializaMatrizEstadoSolares(FormatoModel model, short[][] matrizTipoEstado)
        {
            string celdaFechaHora = string.Empty;
            int nFil = model.FilasCabecera + model.Formato.Formathorizonte * model.Formato.RowPorDia; // numero de filas de las matriz incluido cabecera
            int kFilDatos = model.FilasCabecera - 1; //fila de inicio de datos de las matriz
            var listaPto = model.ListaHojaPto;
            int nCol = listaPto.Count + 1;

            //Rango Solar
            int hSolarIni = model.ParamSolar.HInicio;
            int hSolarFin = model.ParamSolar.HFin;

            MeHojaptomedDTO pto = null;
            var contador = 0;
            var kfilasIniDia = kFilDatos;
            DateTime mediFechaTmp = DateTime.Now;
            var listaFecha = GetAllDatesByFormatoAndFecha(model.Formato);
            foreach (var f in listaFecha)
            {
                mediFechaTmp = (DateTime)f;

                for (var z = 0; z < nCol; z++)
                {
                    if (z > 0)
                    {
                        pto = listaPto[z - 1];
                    }
                    for (int k = 1; k <= model.Formato.RowPorDia; k++)
                    {
                        if (z == 0) // primera columna (fecha hora)
                        {
                        }
                        else
                        {
                            // coordenadas de celda: [kFilDatos + k][z]
                            if (pto.Famcodi == ConstantesHorasOperacion.IdTipoSolar || pto.Famcodi == ConstantesHorasOperacion.IdGeneradorSolar)
                            {
                                if (!(hSolarIni <= k && k <= hSolarFin))
                                {
                                    //si el punto no tiene check extranet entonces restringir las celdas para que no pueda registrar datos
                                    if(!pto.TieneCheckExtranet) 
                                        matrizTipoEstado[kfilasIniDia + k][z] = -1;
                                }
                            }
                        }
                    }
                }

                contador++;
                kfilasIniDia = kFilDatos + contador * model.Formato.RowPorDia;
            }

            return matrizTipoEstado;
        }

        /// <summary>
        /// devuelve una matriz con los valores de las celdas si es de lectura = true, o escritura = false luego de hacer un cruce con informacion de horas de operación
        /// </summary>
        /// <param name="nFil"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public static void CruceMatrizConHOP(short[][] lista, List<MeHojaptomedDTO> listaPtos, int nFilCabecera, int nFilasDatos, List<EveHoraoperacionDTO> listaHOP, DateTime fechaIni, DateTime fechaFin, int formatresolucion)
        {
            int nFil = nFilCabecera + nFilasDatos; // numero de filas de las matriz incluido cabecera
            int nCol = listaPtos.Count + 1;
            int rowPorDia = 1;
            int rowPorHora = 1;

            //determinar cuantas filas tiene por día
            switch (formatresolucion)
            {
                case ParametrosFormato.ResolucionCuartoHora:
                case ParametrosFormato.ResolucionMediaHora:
                case ParametrosFormato.ResolucionHora:
                    rowPorDia = ParametrosFormato.ResolucionDia / formatresolucion;
                    rowPorHora = ParametrosFormato.ResolucionHora / formatresolucion;
                    break;
                case ParametrosFormato.ResolucionDia:
                case ParametrosFormato.ResolucionSemana:
                default:
                    rowPorDia = 1;
                    break;
            }

            //todos los días del filtro de fecha
            List<DateTime> listaFecha = new List<DateTime>();
            for (var day = fechaIni.Date; day.Date < fechaFin.Date; day = day.AddDays(1))
            {
                listaFecha.Add(day);
            }

            //recorrer cada punto de medición
            int posFil = 0;
            int posCol = 0;
            for (int k = 0; k < nCol - 1; k++)
            {
                for (int i = 0; i < listaFecha.Count; i++)
                {
                    DateTime fecha = listaFecha[i];

                    List<EveHoraoperacionDTO> resultado = listaHOP.Where(x => x.Equicodi == listaPtos[k].Equicodi && x.Hophorini.Value.Date == fecha).ToList(); // buscamos si existe hora de operación
                    if (listaPtos[k].Famcodi == ConstantesHorasOperacion.IdGeneradorSolar || listaPtos[k].Famcodi == ConstantesHorasOperacion.IdGeneradorEolica)
                    {
                        resultado = listaHOP.Where(x => x.Equicodi == listaPtos[k].Equipadre && x.Hophorini.Value.Date == fecha).ToList();
                    }

                    foreach (var reg in resultado)
                    {
                        var horaIni = ((DateTime)reg.Hophorini).Hour;
                        var minIni = ((DateTime)reg.Hophorini).Minute;
                        var horaFin = ((DateTime)reg.Hophorfin).Hour;
                        var minFin = ((DateTime)reg.Hophorfin).Minute;

                        int addInd = (minIni < formatresolucion) ? 0 : 1;
                        int indiceIni = horaIni * rowPorHora + addInd;

                        addInd = (minFin < formatresolucion) ? -1 : 0;
                        int indiceFin = horaFin * rowPorHora + addInd;

                        if ((horaFin + minFin) == 0) //las 00:00 del día siguientes
                        {
                            indiceFin = rowPorDia + addInd;
                        }

                        //si existe hora
                        if (indiceFin >= indiceIni)
                        {
                            for (var z = indiceIni; z <= indiceFin; z++)
                            {
                                posFil = i * rowPorDia + z + nFilCabecera;
                                posCol = k + 1;
                                lista[posFil][posCol] = 1;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cruce de Matriz Excel Web con Restricciones Operativas
        /// </summary>
        /// <param name="matrizTipoEstado"></param>
        /// <param name="listaRestricciones"></param>
        /// <param name="listaPtos"></param>
        /// <param name="nFilCabecera"></param>
        public static void CruceMatrizConRestricOper48(short[][] matrizTipoEstado, List<EveIeodcuadroDTO> listaRestricciones, List<MeHojaptomedDTO> listaPtos, int nFilCabecera, int formatresolucion)
        {
            int rowPorDia = 1;
            int rowPorHora = 1;

            //determinar cuantas filas tiene por día
            switch (formatresolucion)
            {
                case ParametrosFormato.ResolucionCuartoHora:
                case ParametrosFormato.ResolucionMediaHora:
                case ParametrosFormato.ResolucionHora:
                    rowPorDia = ParametrosFormato.ResolucionDia / formatresolucion;
                    rowPorHora = ParametrosFormato.ResolucionHora / formatresolucion;
                    break;
                case ParametrosFormato.ResolucionDia:
                case ParametrosFormato.ResolucionSemana:
                default:
                    rowPorDia = 1;
                    break;
            }

            for (int k = 0; k < listaPtos.Count; k++)
            {
                var find = listaRestricciones.Find(x => x.Equicodi == listaPtos[k].Equicodi); // buscamos si existe hora de operación
                if (find != null)
                {
                    var horaIni = ((DateTime)find.Ichorini).Hour;
                    var minIni = ((DateTime)find.Ichorini).Minute;
                    var horaFin = ((DateTime)find.Ichorfin).Hour;
                    var minFin = ((DateTime)find.Ichorfin).Minute;

                    int addInd = (minIni == 30) ? 1 : 0;
                    if (minIni > 0 && minIni < 30)
                    {
                        addInd = 1;
                    }
                    if (minIni > 30 && minIni < 60)
                    {
                        addInd = 2;
                    }

                    int indiceIni = nFilCabecera + horaIni * 2 + addInd;
                    addInd = (minFin < 30) ? 0 : 1;

                    int indiceFin = nFilCabecera + horaFin * 2 + addInd;
                    if ((horaFin + minFin) == 0) //las 00:00 del día siguientes
                    {
                        indiceFin = rowPorDia + addInd;
                    }

                    if (indiceFin >= indiceIni)
                    {
                        for (var z = indiceIni; z <= indiceFin; z++)
                        {
                            matrizTipoEstado[z][k + 1] = 3; // Modo restriccion
                        }
                    }

                }

            }
        }

        /// <summary>
        /// Cruce de Matriz Excel Web con Mantenimientos
        /// </summary>
        /// <param name="matrizTipoEstado"></param>
        /// <param name="listaResultado"></param>
        /// <param name="listaPtos"></param>
        /// <param name="nFilCabecera"></param>
        public static void CruceMatrizConMantenimiento(short[][] matrizTipoEstado, List<MeMedicion48DTO> listaResultado, List<MeHojaptomedDTO> listaPtos, int nFilCabecera, int nFilaDatos)
        {
            for (int k = 0; k < listaPtos.Count; k++)
            {
                foreach (var obj in listaResultado)
                {
                    if (listaPtos[k].Equicodi == obj.Equicodi)
                    {
                        for (int j = 1; j <= nFilaDatos; j++) // analizamos las celdas de cada intervalo
                        {
                            decimal? valor = (decimal?)obj.GetType().GetProperty("H" + (j).ToString()).GetValue(obj, null);
                            if (valor != null)
                            { // si la celda esta encendida
                                matrizTipoEstado[nFilCabecera + j - 1][k + 1] = 2; // Modo Mantenimiento
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cruce de Matriz Excel Web con Eventos
        /// </summary>
        /// <param name="matrizTipoEstado"></param>
        /// <param name="listaResultado"></param>
        /// <param name="listaPtos"></param>
        /// <param name="nFilCabecera"></param>
        public static void CruceMatrizConEvento(short[][] matrizTipoEstado, List<MeMedicion48DTO> listaResultado, List<MeHojaptomedDTO> listaPtos, int nFilCabecera, int nFilaDatos)
        {
            for (int k = 0; k < listaPtos.Count; k++)
            {
                foreach (var obj in listaResultado)
                {
                    if (listaPtos[k].Equicodi == obj.Equicodi)
                    {
                        for (int j = 1; j <= nFilaDatos; j++) // analizamos las celdas de cada intervalo
                        {
                            decimal? valor = (decimal?)obj.GetType().GetProperty("H" + (j).ToString()).GetValue(obj, null);
                            if (valor != null)
                            { // si la celda esta encendida
                                matrizTipoEstado[nFilCabecera + j - 1][k + 1] = 3; // Modo Evento
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deshabilitar el ingreso de datos para celdas despues de la hora 
        /// </summary>
        /// <param name="matrizTipoEstado"></param>
        /// <param name="listaPtos"></param>
        /// <param name="nFilCabecera"></param>
        /// <param name="nFilasDatos"></param>
        public static void CruceMatrizTiempoReal(short[][] matrizTipoEstado, List<MeHojaptomedDTO> listaPtos, int nFilCabecera, int nFilasDatos, DateTime fechaProceso, ref int maxValorEstado)
        {
            DateTime fechaActual = DateTime.Now.Date;

            if (fechaProceso >= fechaActual)
            {
                DateTime dt2 = DateTime.Now;
                if (dt2 > fechaActual.AddDays(1)) dt2 = fechaActual.AddDays(1);

                //media hora actual
                int hf = (dt2.TimeOfDay.TotalMinutes == 0) ? 48 : (int)dt2.TimeOfDay.TotalMinutes / 30;
                hf = hf < 48 ? hf + 1 : 48;
                //max valor del estado en tiempo real 
                maxValorEstado = hf;

                for (int k = 0; k < listaPtos.Count; k++)
                {
                    for (int j = 1; j <= nFilasDatos; j++) // analizamos las celdas de cada intervalo
                    {
                        if (j >= hf)
                        {
                            if (!listaPtos[k].TieneCheckExtranet)//si no tiene check entonces deshabilita hasta la media hora actual. Caso contrario habilita la edición todo el día
                                matrizTipoEstado[nFilCabecera + j - 1][k + 1] = -1; // deshabilitar
                        }
                    }
                }
            }

            maxValorEstado = 48;
        }

        /// <summary>
        /// Carga Informacion de Scada en el Formato para visualizacion de la pagina web
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lista24"></param>
        /// <param name="listaCambios"></param>
        /// <param name="fecha"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public static string[][] GetListaFormatoScada(FormatoModel model, List<MeScadaSp7DTO> listaScada, string fecha)
        {
            DateTime fechaIni = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            string celdaFechaHora = string.Empty;
            int nFil = model.FilasCabecera + model.Formato.RowPorDia; // numero de filas de las matriz incluido cabecera
            int kFilDatos = model.FilasCabecera - 1; //fila de inicio de datos de las matriz
            var listaPto = model.ListaHojaPto;
            int nCol = listaPto.Count + 1;

            //media hora actual
            DateTime dt2 = DateTime.Now;
            int hActual = (dt2.TimeOfDay.TotalMinutes == 0) ? 48 : (int)dt2.TimeOfDay.TotalMinutes / 30;
            hActual = hActual < 48 ? hActual + 1 : 48;

            string[][] lista = new string[nFil][];
            for (int i = 0; i < nFil; i++)
                lista[i] = new string[nCol];

            for (int i = 0; i < nFil; i++)
                for (int j = 0; j < nCol; j++)
                    lista[i][j] = "";


            int idPto = 0;
            int ptoInfoCodi = 0;

            for (var z = 0; z < nCol; z++)
            {
                if (z > 0)
                {
                    idPto = listaPto[z - 1].Ptomedicodi;
                    ptoInfoCodi = listaPto[z - 1].Tipoinfocodi;
                }
                for (int k = 1; k <= 48; k++)
                {
                    if (z == 0) // imprimimos 1ra columna
                    {
                        celdaFechaHora = FormatoHelper.ObtenerCeldaFecha((int)model.Formato.Formatperiodo, (int)model.Formato.Formatresolucion, 1, 0, k, model.Formato.TipoAgregarTiempoAdicional, fechaIni);
                        lista[kFilDatos + k][0] = celdaFechaHora;
                    }
                    else // imprimimos datos 
                    {
                        var regPto = listaPto[z - 1];

                        // coordenadas de celda: [kFilDatos + k][z]
                        if (regPto.Hptoindcheck != "S")
                        {
                            if (model.Handson.MatrizTipoEstado[kFilDatos + k][z] == 1) // si la celda esta activada 
                            {
                                //valor = encontrar valor de la listaScada
                                var find = listaScada.Find(x => x.Ptomedicodi == idPto && x.Tipoinfocodi == ptoInfoCodi && x.Medifecha == fechaIni);
                                if (find != null)
                                {
                                    var Hk = 2 * k;
                                    decimal? valor = (decimal?)find.GetType().GetProperty("H" + Hk).GetValue(find, null);
                                    if (valor != null)
                                        lista[kFilDatos + k][z] = valor.ToString();

                                }
                            }
                        }

                        #region Check para horario seleccionado

                        if (regPto.Hptoindcheck == "S")
                        {
                            string sCheck = ConstantesFormato.TieneSinCheckExtranet; //default unchecked
                            if (regPto.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW) //caudal
                            {
                                sCheck = ConstantesFormato.TieneConCheckExtranet; //default checked para caudal
                                if (hActual <= k) sCheck = ConstantesFormato.TieneSinCheckExtranet; //uncheck
                            }

                            //valor por defecto sino se usa el importado en excel
                            if (string.IsNullOrEmpty(lista[kFilDatos + k][z]))
                                lista[kFilDatos + k][z] = sCheck;

                            if (regPto.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW && hActual <= k)
                                lista[kFilDatos + k][z] = ConstantesFormato.TieneSinCheckExtranet;
                        }

                        #endregion
                    }
                }
            }

            //Indicar que tiene data scada para tal empresa y día
            model.TieneDataScada = listaScada.Count() > 0;

            return lista;
        }

        private static void GenerarFileExcelHojaFormato(FormatoModel model, ExcelPackage xlPackage, string nombre)
        {
            if (ConstantesFormatoMedicion.IdFormatoGeneracionRERDiario == model.Formato.Formatcodi || ConstantesFormatoMedicion.IdFormatoGeneracionRERSemanal == model.Formato.Formatcodi) 
            {
                GenerarFileExcelHojaFormatoGeneracionRER(model, xlPackage, nombre);
                return;
            }

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombre);
            ws.View.ShowGridLines = false;

            //Imprimimos Codigo Empresa y Codigo Formato (ocultos)
            ws.Column(ConstantesFormato.ColEmpresaExtranet).Hidden = true;
            ws.Column(ConstantesFormato.ColFormatoExtranet).Hidden = true;
            ws.Cells[1, ConstantesFormato.ColEmpresaExtranet].Value = model.IdEmpresa.ToString();
            ws.Cells[1, ConstantesFormato.ColFormatoExtranet].Value = model.Formato.Formatcodi.ToString();

            int row = 5;
            int column = ConstantesFormato.ColExcelData;

            int rowIniFormato = row;
            int colIniFormato = column;
            ws.Cells[rowIniFormato, colIniFormato].Value = model.Formato.Formatnombre;
            ws.Cells[rowIniFormato, colIniFormato].Style.Font.SetFromFont(new Font("Calibri", 14));
            ws.Cells[rowIniFormato, colIniFormato].Style.Font.Bold = true;

            row = rowIniFormato + 3;
            int rowIniEmpresa = row;
            int colIniEmpresa = column;
            int rowIniAnio = rowIniEmpresa + 1;

            switch (model.Formato.Formatperiodo)
            {
                case ParametrosFormato.PeriodoSemanal:

                    ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "Empresa:";
                    ws.Cells[rowIniEmpresa, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa, colIniEmpresa + 1].Value = model.Empresa;

                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa].Value = "Año:";
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa + 1].Style.Numberformat.Format = "@";
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa + 1].Value = " " + model.Anho.ToString();

                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa].Value = "Semana:";
                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa + 1].Style.Numberformat.Format = "@";
                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa + 1].Value = " " + model.Semana;

                    using (var range = ws.Cells[rowIniEmpresa, colIniEmpresa, rowIniEmpresa + 2, colIniEmpresa])
                    {
                        range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    break;
                case ParametrosFormato.PeriodoMensual:

                    ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "Empresa:";
                    ws.Cells[rowIniEmpresa, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa, colIniEmpresa + 1].Value = model.Empresa;

                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa].Value = "Año:";
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa + 1].Style.Numberformat.Format = "@";
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa + 1].Value = model.Anho.ToString();

                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa].Value = "Mes:";
                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa + 1].Value = model.Mes;

                    using (var range = ws.Cells[rowIniEmpresa, colIniEmpresa, rowIniEmpresa + 2, colIniEmpresa])
                    {
                        range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    break;
                case ParametrosFormato.PeriodoDiario:
                default:

                    ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "Empresa:";
                    ws.Cells[rowIniEmpresa, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa, colIniEmpresa + 1].Value = model.Empresa;

                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa].Value = "Año:";
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa + 1].Style.Numberformat.Format = "@";
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa + 1].Value = model.Anho.ToString();

                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa].Value = "Mes:";
                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa + 1].Value = model.Mes;

                    ws.Cells[rowIniEmpresa + 3, colIniEmpresa].Value = "Día:";
                    ws.Cells[rowIniEmpresa + 3, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa + 3, colIniEmpresa + 1].Style.Numberformat.Format = "@";
                    ws.Cells[rowIniEmpresa + 3, colIniEmpresa + 1].Value = model.Dia.ToString();

                    using (var range = ws.Cells[rowIniEmpresa, colIniEmpresa, rowIniEmpresa + 3, colIniEmpresa])
                    {
                        range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        range.Style.Font.Color.SetColor(Color.White);
                    }
                    break;
            }

            //
            ws.Column(1).Width = 3;
            ws.Column(colIniFormato).Width = 19;

            ///Imprimimos cabecera de puntos de medicion
            row = ConstantesFormato.FilaExcelData;
            int rowIniPto = row;
            int colIniPto = column;
            int totColumnas = model.ListaHojaPto.Count;

            for (var i = 0; i <= totColumnas; i++)
            {
                if (i > 0)
                    ws.Column(colIniFormato + i).Width = 15;

                for (var j = 0; j < model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows; j++)
                {
                    decimal valor = 0;
                    model.Handson.ListaExcelData[j][i] = model.Handson.ListaExcelData[j][i] != null ? model.Handson.ListaExcelData[j][i] : string.Empty;
                    bool canConvert = decimal.TryParse(model.Handson.ListaExcelData[j][i], out valor);
                    if (canConvert)
                        ws.Cells[row + j, i + colIniPto].Value = valor;
                    else
                        ws.Cells[row + j, i + colIniPto].Value = model.Handson.ListaExcelData[j][i] != null ? model.Handson.ListaExcelData[j][i].Trim() : string.Empty;

                    //Columna adicional de check
                    if (i >= model.ColumnasCabecera)
                    {
                        var regPto = model.ListaHojaPto[i - model.ColumnasCabecera];
                        if (regPto.Hptoindcheck == "S")
                        {
                            if (j == 1) ws.Cells[row + j, i + colIniPto].Value = "ESTADO";
                            if (j == 2) ws.Cells[row + j, i + colIniPto].Value = "E/P";
                            if (j > 2)
                            {
                                string desc = ((model.Handson.ListaExcelData[j][i])??"").ToUpper() == "TRUE" ? "E" : "P";
                                ws.Cells[row + j, i + colIniPto].Value = desc;
                            }
                        }
                    }

                    ws.Cells[row + j, i + colIniPto].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    if (j < model.Formato.Formatrows && i >= model.Formato.Formatcols)
                    {
                        ws.Cells[row + j, i + colIniPto].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells[row + j, i + colIniPto].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[row + j, i + colIniPto].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[row + j, i + colIniPto].Style.WrapText = true;
                    }
                    if (i == 0 && j >= model.Formato.Formatrows)
                    {
                        ws.Cells[row + j, i + colIniPto].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[row + j, i + colIniPto].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                }
            }
            /////////////////Formato a Celdas Head ///////////////////

            using (var range = ws.Cells[rowIniPto, colIniPto, rowIniPto + model.Formato.Formatrows - 1, colIniPto])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#87CEEB"));
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.WrapText = true;
            }

            if (model.ListaHojaPto.Count > 0)
            {
                using (var range = ws.Cells[rowIniPto, colIniPto + 1, rowIniPto + model.Formato.Formatrows - 1, colIniPto + model.ListaHojaPto.Count])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                ////////////// Formato de Celdas Valores
                using (var range = ws.Cells[rowIniPto + model.Formato.Formatrows, colIniPto + 1
                    , rowIniPto + model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows - 1, colIniPto + model.ListaHojaPto.Count])
                {
                    range.Style.Numberformat.Format = @"0.000";
                }
            }

            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
            //System.Drawing.Image img = null;
            if (img != null)
            {
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 1;
                picture.From.Row = 1;
            }
        }
        
        private static void GenerarFileExcelHojaFormatoGeneracionRER(FormatoModel model, ExcelPackage xlPackage, string nombre)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombre);

            //Imprimimos Codigo Empresa y Codigo Formato (ocultos)
            ws.Row(1).Height = 0;
            ws.Cells[1, ConstantesFormato.ColEmpresaExtranetNuevo].Value = model.IdEmpresa.ToString();
            ws.Cells[1, ConstantesFormato.ColFormatoExtranetNuevo].Value = model.Formato.Formatcodi.ToString();

            int rowIni = 2;
            int rowFin;
            int column = ConstantesFormato.ColExcelDataNuevo;

            #region Filtros

            int colIniProp = column;
            int colIniValor= colIniProp+1;
            int rowIniEmpresa= rowIni;
            int rowIniFormato = rowIniEmpresa + 1;
            int rowIniFecha = rowIniFormato + 1;
            int rowIniSemana = rowIniFormato+1;
            int rowIniFechaDesde = rowIniSemana+1;
            int rowIniFechaHasta = rowIniFechaDesde+1;

            ws.Cells[rowIniEmpresa, colIniProp].Value = "EMPRESA";
            ws.Cells[rowIniFormato, colIniProp].Value = "FORMATO";

            ws.Cells[rowIniEmpresa, colIniValor].Value = model.Empresa;
            ws.Cells[rowIniFormato, colIniValor].Value = model.Formato.Formatnombre;
            
            switch (model.Formato.Formatperiodo)
            {
                case ParametrosFormato.PeriodoSemanal:

                    ws.Cells[rowIniSemana, colIniProp].Value = "SEMANA";
                    ws.Cells[rowIniFechaDesde, colIniProp].Value = "FECHA DESDE";
                    ws.Cells[rowIniFechaHasta, colIniProp].Value = "FECHA HASTA";

                    var tupla = EPDate.f_numerosemana_y_anho(model.Formato.FechaInicio);

                    ws.Cells[rowIniSemana, colIniValor].Value = tupla.Item1;
                    ws.Cells[rowIniFechaDesde, colIniValor].Value = model.Formato.FechaInicio.ToString(ConstantesAppServicio.FormatoFecha);
                    ws.Cells[rowIniFechaHasta, colIniValor].Value = model.Formato.FechaFin.ToString(ConstantesAppServicio.FormatoFecha);
                    rowFin = rowIniFechaHasta;
                    break;
                case ParametrosFormato.PeriodoDiario:
                default:
                    ws.Cells[rowIniFecha, colIniProp].Value = "FECHA";
                    ws.Cells[rowIniFecha, colIniValor].Value = model.Formato.FechaProceso.ToString(ConstantesAppServicio.FormatoFecha);
                    rowFin = rowIniFecha;
                    break;
            }

            //primera columna
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIni, colIniProp, rowFin, colIniProp, "Centro");
            UtilExcel.CeldasExcelColorTexto(ws, rowIni, colIniProp, rowFin, colIniProp, "#333399");
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIni, colIniProp, rowFin, colIniProp, ColorTranslator.FromHtml(ConstantesFormatoMedicion.ColorColumnaHora), Color.Black);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIni, colIniProp, rowFin, colIniProp, "Arial", 10);

            //segunda columna
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIni, colIniValor, rowFin, colIniValor, "Izquierda");
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIni, colIniValor, rowFin, colIniValor, "Arial", 8);
            UtilExcel.CeldasExcelEnNegrita(ws, rowIni, colIniValor, rowFin, colIniValor);

            #endregion

            ///Imprimimos cabecera de puntos de medicion
            int rowIniData = ConstantesFormato.FilaExcelDataNuevo;
            int colIniData = column;
            int totColumnas = model.ListaHojaPto.Count;

            //nota
            ws.Cells[ rowIniData - 1, colIniData].Value = "No modificar la estructura del documento";
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniData - 1, colIniData, rowIniData - 1, colIniData, "Arial", 8);
            ws.Cells[rowIniData - 1, colIniData].Style.Font.Italic = true;

            //ocultar filas
            if (model.ListaFilasOcultas != null) {
                foreach (var posFila in model.ListaFilasOcultas) {
                    ws.Row(rowIniData + posFila).Height = 0;
                }
            }

            //generar datos
            for (var i = 0; i <= totColumnas; i++)
            {
                ws.Column(i + colIniData).Width = i == 0 ? 20 : 15;
                for (var j = 0; j < model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows; j++)
                {
                    model.Handson.ListaExcelData[j][i] = model.Handson.ListaExcelData[j][i] != null ? model.Handson.ListaExcelData[j][i] : string.Empty;

                    bool canConvert = decimal.TryParse(model.Handson.ListaExcelData[j][i], out decimal valor);
                    if (canConvert)
                        ws.Cells[rowIniData + j, i + colIniData].Value = valor;
                    else
                        ws.Cells[rowIniData + j, i + colIniData].Value = model.Handson.ListaExcelData[j][i] != null ? model.Handson.ListaExcelData[j][i].Trim() : string.Empty;

                }
            }
            /////////////////Formato a Celdas Head ///////////////////
            int rowIniPto = rowIniData; 
            int rowFinPto = rowIniPto + model.Formato.Formatrows - 1;
            int colHora = colIniData;

            int colIniPto = colHora + 1;
            int colFinPto = colIniPto + model.ListaHojaPto.Count - 1;
            int rowIniNumero = rowIniPto + model.Formato.Formatrows;
            int rowFinNumero = rowIniPto + model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows - 1;

            //Formato para toda la tabla (cabecera y cuerpo)
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniPto, colHora, rowFinNumero, colFinPto, "Centro");
            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniPto, colHora, rowFinNumero, colFinPto, "Centro");

            //formato para las filas de cabecera (columna hora)
            UtilExcel.CeldasExcelColorTexto(ws, rowIniPto, colHora, rowFinPto, colHora, "#333399");
            UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniPto, colHora, rowFinPto, colHora, ColorTranslator.FromHtml(ConstantesFormatoMedicion.ColorColumnaHora), Color.Black);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniPto, colHora, rowFinPto, colHora, "Arial", 10);

            if (model.ListaHojaPto.Count > 0)
            {
                //Formato para las filas de cabecera (columnas de puntos de medición)
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniPto, colIniPto, rowFinPto, colFinPto, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniPto, colIniPto, rowFinPto, colFinPto, "Arial", 10);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniPto, colIniPto, rowFinPto, colFinPto, "Centro");

                string centralOld = model.ListaHojaPto[0].Equipopadre;
                string color = ConstantesFormatoMedicion.ColorColumnaCentralPar;
                for (var i = 1; i <= totColumnas; i++)
                {
                    string centralActual = model.ListaHojaPto[i - 1].Equipopadre;
                    if (centralActual != centralOld)
                    {
                        centralOld = centralActual;
                        color = color == ConstantesFormatoMedicion.ColorColumnaCentralPar ? ConstantesFormatoMedicion.ColorColumnaCentralImpar : ConstantesFormatoMedicion.ColorColumnaCentralPar;
                    }
                    UtilExcel.CeldasExcelColorFondoYBorder(ws, rowIniPto, i+ colIniData, rowFinPto, i+ colIniData, ColorTranslator.FromHtml(color), Color.Black);
                }
                                
                UtilExcel.CeldasExcelEnNegrita(ws, rowIniPto, colIniPto, rowFinPto, colFinPto);
                UtilExcel.CeldasExcelWrapText(ws, rowIniPto, colIniPto, rowFinPto, colFinPto);

                //Formato para las celdas numericas
                ws.Cells[rowIniNumero, colIniPto, rowFinNumero, colFinPto].Style.Numberformat.Format = @"0.00";               
            }

            //border punteado 
            UtilExcel.BorderCeldasPunteado(ws, rowIniNumero, colHora, rowFinNumero, colFinPto);
            UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniNumero, colHora, rowFinNumero, colFinPto, "Arial", 8);

            //mostrar lineas horas
            for (int c = colHora; c <= colFinPto; c++)
            {
                int totalXRango = 48;
                for (int f = rowIniNumero; f < rowFinNumero; f += totalXRango)
                {
                    ws.Cells[f, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;

                    ws.Cells[f + totalXRango - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f + totalXRango - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f + totalXRango - 1, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    ws.Cells[f, c, f + totalXRango - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f, c, f + totalXRango - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f, c, f + totalXRango - 1, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c, f + totalXRango - 1, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }
            }

            ws.View.FreezePanes(rowFinPto + 1, colHora+ 1);
            ws.View.ZoomScale = 100;
            //ws.View.ShowGridLines = false;
        }

        public static List<DateTime> GetAllDatesByFormatoAndFecha(MeFormatoDTO formato)
        {
            List<DateTime> l = new List<DateTime>();
            DateTime dayIni = formato.FechaInicio;
            DateTime dayFin = formato.FechaFin;
            for (var day = dayIni.Date; day.Date <= dayFin.Date; day = day.AddDays(1))
                l.Add(day);

            return l;
        }

        public static string getDateFormatDDMMYYYY(DateTime a_datetime)
        {
            return a_datetime.ToString("dd/MM/yyyy");
        }

        public static void GetFechaActualEnvio(int periodo, int tipo, ref string mes, ref string fecha, ref int semana, ref int anho)
        {
            DateTime fechaActual = DateTime.Now;
            fecha = fechaActual.ToString(Constantes.FormatoFecha);
            switch (periodo)
            {
                case ParametrosFormato.PeriodoDiario:
                    if (tipo == ParametrosFormato.Ejecutado)
                    {
                        fecha = fechaActual.AddDays(-1).ToString(Constantes.FormatoFecha);
                    }
                    if (tipo == ParametrosFormato.Programado)
                    {
                        fecha = fechaActual.ToString(Constantes.FormatoFecha);
                    }
                    if (tipo == ParametrosFormato.TiempoReal)
                    {
                        fecha = fechaActual.ToString(Constantes.FormatoFecha);
                    }
                    break;
                case ParametrosFormato.PeriodoSemanal:
                    var totalSemanasAnho = EPDate.TotalSemanasEnAnho(fechaActual.Year, 6);
                    semana = EPDate.f_numerosemana(fechaActual);
                    anho = fechaActual.Year;
                    if (tipo == ParametrosFormato.Ejecutado)
                    {

                        if (semana == 1)
                        {
                            semana = totalSemanasAnho;
                            anho = anho - 1;
                        }
                        else
                            semana--;
                    }
                    else
                    {

                        if (semana == totalSemanasAnho)
                        {
                            semana = 1;
                            anho++;
                        }
                        else
                        {
                            semana++;
                        }
                    }
                    break;
                case ParametrosFormato.PeriodoMensual:
                    if (tipo == ParametrosFormato.Ejecutado)
                    {
                        mes = EPDate.GetFechaMes(fechaActual);
                    }
                    else
                    {
                        mes = EPDate.GetFechaMes(fechaActual);
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana:
                    if (tipo == ParametrosFormato.Ejecutado)
                    {
                        mes = EPDate.GetFechaMes(fechaActual);
                    }
                    else
                    {
                        mes = EPDate.GetFechaMes(fechaActual);
                    }
                    break;
                case ParametrosFormato.PeriodoAnual:
                    break;
            }
        }

        /// <summary>
        /// Genera Archivo excel del formato y devuelve la ruta mas el nombre del archivo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ruta"></param>
        public static string GenerarFileExcelFormato(FormatoModel modelPrincipal)
        {
            string fileExcel = string.Empty;
            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                if (!modelPrincipal.UtilizaHoja)
                {
                    GenerarFileExcelHojaFormato(modelPrincipal, xlPackage, Constantes.HojaFormatoExcel);
                }
                else
                {
                    foreach (var model in modelPrincipal.ListaFormatoModel)
                    {
                        GenerarFileExcelHojaFormato(model, xlPackage, model.Hoja.Hojanombre);
                    }
                }

                if (modelPrincipal.IdFormato == ConstantesHard.IdFormatoDespacho)
                    modelPrincipal.NombreArchivoExcel = modelPrincipal.NombreArchivoExcel + modelPrincipal.Emprabrev + "_" + DateTime.Now.ToString("dd-MM-yyyy") + "-" + modelPrincipal.IdEnvio.ToString();
                else
                    modelPrincipal.NombreArchivoExcel = "Archivo" + "_" + modelPrincipal.Formato.FechaProceso.ToString("dd-MM-yyyy") + "_" + modelPrincipal.Empresa + (modelPrincipal.IdEnvio > 0 ? "-" + modelPrincipal.IdEnvio.ToString() : "")+(modelPrincipal.IdFormato == 80?"-MW":modelPrincipal.IdFormato ==81?"-MVAR":"-SSAA");

                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));
            }

            fileExcel += "," + modelPrincipal.NombreArchivoExcel;

            return fileExcel;
        }

        #region Mejoras RDO
        public static string GenerarFileExcelDespachoGeneracion(FormatoModel modelPrincipal, string horario)
        {
            string fileExcel = string.Empty;
            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                int id_fam_codi = modelPrincipal.ListaHojaPto[0].Famcodi;
                if (!modelPrincipal.UtilizaHoja)
                {
                    if (id_fam_codi == 37 || id_fam_codi == 39)
                        GenerarFileExcelHojaDespachoGeneracion(modelPrincipal, xlPackage, Constantes.HojaFormatoExcel, horario);
                    else
                        GenerarFileExcelHojaFormato(modelPrincipal, xlPackage, Constantes.HojaFormatoExcel);
                }
                else
                {
                    foreach (var model in modelPrincipal.ListaFormatoModel)
                    {
                        if (id_fam_codi == 37 || id_fam_codi == 39)
                            GenerarFileExcelHojaDespachoGeneracion(model, xlPackage, model.Hoja.Hojanombre, horario);
                        else
                            GenerarFileExcelHojaFormato(model, xlPackage, model.Hoja.Hojanombre);
                    }
                }



                if (modelPrincipal.IdFormato == ConstantesHard.IdFormatoDespacho)
                    modelPrincipal.NombreArchivoExcel = modelPrincipal.NombreArchivoExcel + modelPrincipal.Emprabrev + "_" + DateTime.Now.ToString("dd-MM-yyyy") + "-" + modelPrincipal.IdEnvio.ToString();
                else
                    modelPrincipal.NombreArchivoExcel = "Archivo" + "_" + modelPrincipal.Formato.FechaProceso.ToString("dd-MM-yyyy") + "_" + modelPrincipal.Empresa + (modelPrincipal.IdEnvio > 0 ? "-" + modelPrincipal.IdEnvio.ToString() : "");

                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));
            }

            fileExcel += "," + modelPrincipal.NombreArchivoExcel;

            return fileExcel;
        }

        private static void GenerarFileExcelHojaDespachoGeneracion(FormatoModel model, ExcelPackage xlPackage, string nombre, string horario)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombre);
            ws.View.ShowGridLines = false;

            //Imprimimos Codigo Empresa y Codigo Formato (ocultos)
            ws.Column(ConstantesFormato.ColEmpresaExtranet).Hidden = true;
            ws.Column(ConstantesFormato.ColFormatoExtranet).Hidden = true;
            ws.Cells[1, ConstantesFormato.ColEmpresaExtranet].Value = model.IdEmpresa.ToString();
            ws.Cells[1, ConstantesFormato.ColFormatoExtranet].Value = model.Formato.Formatcodi.ToString();

            int row = 5;
            int column = ConstantesFormato.ColExcelData;

            int rowIniFormato = row;
            int colIniFormato = column;
            ws.Cells[rowIniFormato, colIniFormato].Value = model.Formato.Formatnombre;
            ws.Cells[rowIniFormato, colIniFormato].Style.Font.SetFromFont(new Font("Calibri", 14));
            ws.Cells[rowIniFormato, colIniFormato].Style.Font.Bold = true;

            row = rowIniFormato + 3;
            int rowIniEmpresa = row;
            int colIniEmpresa = column;
            int rowIniAnio = rowIniEmpresa + 1;

            switch (model.Formato.Formatperiodo)
            {
                case ParametrosFormato.PeriodoSemanal:

                    ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "Empresa:";
                    ws.Cells[rowIniEmpresa, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa, colIniEmpresa + 1].Value = model.Empresa;

                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa].Value = "Año:";
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa + 1].Style.Numberformat.Format = "@";
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa + 1].Value = " " + model.Anho.ToString();

                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa].Value = "Semana:";
                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa + 1].Style.Numberformat.Format = "@";
                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa + 1].Value = " " + model.Semana;

                    using (var range = ws.Cells[rowIniEmpresa, colIniEmpresa, rowIniEmpresa + 2, colIniEmpresa])
                    {
                        range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    break;
                case ParametrosFormato.PeriodoMensual:

                    ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "Empresa:";
                    ws.Cells[rowIniEmpresa, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa, colIniEmpresa + 1].Value = model.Empresa;

                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa].Value = "Año:";
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa + 1].Style.Numberformat.Format = "@";
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa + 1].Value = model.Anho.ToString();

                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa].Value = "Mes:";
                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa + 1].Value = model.Mes;

                    using (var range = ws.Cells[rowIniEmpresa, colIniEmpresa, rowIniEmpresa + 2, colIniEmpresa])
                    {
                        range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    break;
                case ParametrosFormato.PeriodoDiario:
                default:

                    ws.Cells[rowIniEmpresa, colIniEmpresa].Value = "Empresa:";
                    ws.Cells[rowIniEmpresa, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa, colIniEmpresa + 1].Value = model.Empresa;

                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa].Value = "Año:";
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa + 1].Style.Numberformat.Format = "@";
                    ws.Cells[rowIniEmpresa + 1, colIniEmpresa + 1].Value = model.Anho.ToString();

                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa].Value = "Mes:";
                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa + 2, colIniEmpresa + 1].Value = model.Mes;

                    ws.Cells[rowIniEmpresa + 3, colIniEmpresa].Value = "Día:";
                    ws.Cells[rowIniEmpresa + 3, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[rowIniEmpresa + 3, colIniEmpresa + 1].Style.Numberformat.Format = "@";
                    ws.Cells[rowIniEmpresa + 3, colIniEmpresa + 1].Value = model.Dia.ToString();

                    using (var range = ws.Cells[rowIniEmpresa, colIniEmpresa, rowIniEmpresa + 3, colIniEmpresa])
                    {
                        range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        range.Style.Font.Color.SetColor(Color.White);
                    }
                    break;
            }

            //
            ws.Column(1).Width = 3;
            ws.Column(colIniFormato).Width = 19;

            ///Imprimimos cabecera de puntos de medicion
            row = ConstantesFormato.FilaExcelData;
            int rowIniPto = row;
            int colIniPto = column;
            int totColumnas = model.ListaHojaPto.Count;

            for (var i = 0; i <= totColumnas + 1; i++)
            {
                if (i > 0)
                    ws.Column(colIniFormato + i).Width = 15;


                for (var j = 0; j < model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows; j++)
                {
                    decimal valor = 0;
                    bool canConvert = true;

                    if (i <= model.ListaHojaPto.Count)
                        canConvert = decimal.TryParse(model.Handson.ListaExcelData[j][i], out valor);

                    if (canConvert)
                    {
                        if (j == 0)
                            ws.Cells[row + j, i + colIniPto].Value = model.Handson.ListaExcelData[j][i - 1];
                        else if (j == 1)
                            ws.Cells[row + j, i + colIniPto].Value = "ESTADO";
                        else if (j == 2)
                            ws.Cells[row + j, i + colIniPto].Value = "E/P";
                        else if (j > 3)
                        {
                            if (i == model.ListaHojaPto.Count + 1)
                            {
                                //string hora = model.Handson.ListaExcelData[j][0];
                                string hora2 = model.Handson.ListaExcelData[j][0].Substring(11, 5);
                                string hora3 = horario + ":30";
                                if (Convert.ToInt32(model.Handson.ListaExcelData[j][0].Substring(11, 2)) <= Convert.ToInt32(horario) && model.Handson.ListaExcelData[j][0].Substring(11, 5) != horario + ":30" && j < model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows - 1)
                                    ws.Cells[row + j, i + colIniPto].Value = "E";
                                else
                                    ws.Cells[row + j, i + colIniPto].Value = "P";
                            }
                            else
                                ws.Cells[row + j, i + colIniPto].Value = model.Handson.ListaExcelData[j][i];
                        }

                    }

                    else
                        ws.Cells[row + j, i + colIniPto].Value = model.Handson.ListaExcelData[j][i];


                    ws.Cells[row + j, i + colIniPto].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    if (j < model.Formato.Formatrows && i >= model.Formato.Formatcols)
                    {
                        ws.Cells[row + j, i + colIniPto].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells[row + j, i + colIniPto].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                        ws.Cells[row + j, i + colIniPto].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        ws.Cells[row + j, i + colIniPto].Style.WrapText = true;
                    }
                }
            }
            /////////////////Formato a Celdas Head ///////////////////

            using (var range = ws.Cells[rowIniPto, colIniPto, rowIniPto + model.Formato.Formatrows - 1, colIniPto])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#87CEEB"));
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.WrapText = true;
            }

            if (model.ListaHojaPto.Count > 0)
            {
                using (var range = ws.Cells[rowIniPto, colIniPto + 1, rowIniPto + model.Formato.Formatrows - 1, colIniPto + model.ListaHojaPto.Count + 1])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                ////////////// Formato de Celdas Valores
                using (var range = ws.Cells[rowIniPto + model.Formato.Formatrows, colIniPto + 1
                    , rowIniPto + model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows - 1, colIniPto + model.ListaHojaPto.Count])
                {
                    range.Style.Numberformat.Format = @"0.000";
                }
            }

            //HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
            System.Drawing.Image img = null;
            if (img != null)
            {
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 1;
                picture.From.Row = 1;
            }
        }

        public static int VerificarIdsFormatoDespachoGeneracion(string file, int idEmpresa, int idFormato)
        {
            int retorno = 1;
            int idEmpresaArchivo;
            int idFormatoEmpresa;
            FileInfo fileInfo = new FileInfo(file);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                int posIdEmpresa = ConstantesFormato.ColEmpresaExtranet;
                int posIdFormato = ConstantesFormato.ColFormatoExtranet;
                if (ConstantesFormatoMedicion.IdFormatoGeneracionDespachoDiario == idFormato)
                {
                    posIdEmpresa = ConstantesFormato.ColEmpresaExtranetNuevo;
                    posIdFormato = ConstantesFormato.ColFormatoExtranetNuevo;
                }

                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                string valorEmp = string.Empty;
                if (ws.Cells[1, posIdEmpresa].Value != null)
                    valorEmp = ws.Cells[1, posIdEmpresa].Value.ToString();
                if (!int.TryParse(valorEmp, NumberStyles.Any, CultureInfo.InvariantCulture, out idEmpresaArchivo))
                    idEmpresa = 0;
                string valorFormato = string.Empty;
                if (ws.Cells[1, posIdFormato].Value != null)
                    valorFormato = ws.Cells[1, posIdFormato].Value.ToString();
                if (!int.TryParse(valorFormato, NumberStyles.Any, CultureInfo.InvariantCulture, out idFormatoEmpresa))
                    idFormatoEmpresa = 0;
            }

            if (idEmpresaArchivo != idEmpresa)
            {
                retorno = -1;
            }
            if (idFormatoEmpresa != idFormato)
            {
                retorno = -2;
            }

            return retorno;
        }

        /// <summary>
        /// Lee archivo excel cargado y llena matriz de datos para visualizacion web
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="colsHead"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public static string[][] LeerExcelFileDespacho(string file, List<MeHojaptomedDTO> listaPto, MeFormatoDTO formato, string nombreHoja, string horario)
        {
            string sheet = nombreHoja != null ? nombreHoja : Constantes.HojaFormatoExcel;
            int rowcabecera = formato.Formatrows;
            int rowsDatos = formato.RowPorDia * formato.Formathorizonte;
            DateTime fechaIni = formato.FechaInicio;

            string celdaFechaHora = string.Empty;
            FileInfo fileInfo = new FileInfo(file);
            double numero = 0;
            int nFil = rowcabecera + rowsDatos;
            int nCol = listaPto.Count + 1;

            string[][] lista = new string[nFil][];
            for (int i = 0; i < nFil; i++)
                lista[i] = new string[nCol];

            for (int i = 0; i < nFil; i++)
                for (int j = 0; j < nCol; j++)
                    lista[i][j] = "";
            lista[0][0] = "CENTRAL";
            lista[1][0] = "GRUPO";
            lista[2][0] = "FECHA/UNIDAD";
            for (int i = 1; i < nCol; i++)
            {
                lista[0][i] = listaPto[i - 1].Equipopadre;
                lista[1][i] = listaPto[i - 1].Equinomb;
                lista[2][i] = "(" + listaPto[i - 1].Tipoinfoabrev + ")";
            }

            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[sheet];

                if (ws == null)
                {
                    throw new Exception("No existe la pestaña [" + sheet + "]");
                }

                /// Verificar Formato
                int filaIniData = ConstantesFormato.FilaExcelData;
                int colIniData = ConstantesFormato.ColExcelData;
                if (ConstantesFormatoMedicion.IdFormatoGeneracionRERDiario == formato.Formatcodi || ConstantesFormatoMedicion.IdFormatoGeneracionRERSemanal == formato.Formatcodi)
                {
                    filaIniData = ConstantesFormato.FilaExcelDataNuevo;
                    colIniData = ConstantesFormato.ColExcelDataNuevo;
                }

                for (int i = 0; i < nFil - rowcabecera; i++)
                {
                    for (int j = 0; j < nCol - 1; j++)
                    {
                        string _estado = (ws.Cells[i + (rowcabecera) + filaIniData, nCol + colIniData].Value != null) ?
                           ws.Cells[i + (rowcabecera) + filaIniData, nCol + colIniData].Value.ToString() : string.Empty;

                        string _hora = (ws.Cells[i + (rowcabecera) + filaIniData, colIniData].Value != null) ?
                           ws.Cells[i + (rowcabecera) + filaIniData, colIniData].Value.ToString().Substring(11, 2) : string.Empty;

                        string _horario = (ws.Cells[i + (rowcabecera) + filaIniData, colIniData].Value != null) ?
                           ws.Cells[i + (rowcabecera) + filaIniData, colIniData].Value.ToString().Substring(11, 5) : string.Empty;

                        if (_estado == "E")
                        {
                            if (Convert.ToInt32(horario) < Convert.ToInt32(_hora) || _horario == horario + ":30")
                            {
                                throw new Exception("La hora registrada está fuera de rango de horario.");
                            }

                        }

                        string valor = (ws.Cells[i + (rowcabecera) + filaIniData, j + 1 + colIniData].Value != null) ?
                           ws.Cells[i + (rowcabecera) + filaIniData, j + 1 + colIniData].Value.ToString() : string.Empty;

                        if (EsNumero(valor))
                        {
                            double.TryParse(valor, out numero);
                            valor = numero.ToString("0.###########################################");
                        }
                        lista[i + rowcabecera][j + 1] = valor;

                    }
                }
            }

            for (var i = 1; i <= rowsDatos; i++)
            {
                celdaFechaHora = FormatoHelper.ObtenerCeldaFecha(formato.Formatperiodo.Value, formato.Formatresolucion.Value, 1, 0, i, formato.TipoAgregarTiempoAdicional, fechaIni);
                lista[i + rowcabecera - 1][0] = celdaFechaHora;

            }
            return lista;
        }

        #endregion
    }
}