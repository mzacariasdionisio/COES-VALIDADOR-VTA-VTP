using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.TransfPotencia.Helper;
using COES.MVC.Extranet.Areas.TransfPotencia.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.TransfPotencia.Controllers
{
    public class PeajeEgresoMinfoController : BaseController
    {
        // GET: /TransfPotencia/PeajeEgresoMinfo/

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
        ConstantesTransfPotencia libFuncion = new ConstantesTransfPotencia();

        public ActionResult Index(int pericodi = 0, int recpotcodi = 0, int emprcodi = 0)
        {
            base.ValidarSesionUsuario();
            PeajeEgresoMinfoModel model = new PeajeEgresoMinfoModel();
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            { pericodi = model.ListaPeriodos[0].PeriCodi; }
            if (pericodi > 0)
            {
                model.ListaRecalculoPotencia = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(pericodi); //Ordenado en descendente
                if (model.ListaRecalculoPotencia.Count > 0 && recpotcodi == 0)
                { recpotcodi = (int)model.ListaRecalculoPotencia[0].Recpotcodi; }
            }

            if (pericodi > 0 && recpotcodi > 0)
            {
                model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            }
            else
            {
                model.EntidadRecalculoPotencia = new VtpRecalculoPotenciaDTO();
            }
            model.ListaEmpresas = this.servicioEmpresa.ListaEmpresasCombo();
            model.ListaBarras = this.servicioBarra.ListBarras();
            model.Pericodi = pericodi;
            model.Recpotcodi = recpotcodi;
            model.Emprcodi = emprcodi;
            //model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name); 
            return View(model);
        }

        #region Grilla Excel

        /// <summary>
        /// Muestra la grilla excel con los registros de Egresos y Peajes - vista VW_
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelConsulta(int pericodi = 0, int recpotcodi = 0, int emprcodi = 0, int cliemprcodi = 0, int barrcodi = 0, int barrcodifco = 0, string pegrmitipousuario = "*", string pegrmilicitacion = "*", string pegrmicalidad = "*")
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            PeajeEgresoMinfoModel modelpe = new PeajeEgresoMinfoModel();
            modelpe.Pericodi = pericodi;
            modelpe.Recpotcodi = recpotcodi;

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] Cabecera1 = { "Empresa", "Cliente", "Barra", "Tipo Usuario", "Licitación", "PARA EGRESO DE POTENCIA", "", "PARA PEAJE POR CONEXIÓN", "", "", "PARA FLUJO DE CARGA OPTIMO", "", "", "Calidad" };
            string[] Cabecera2 = { "", "", "", "", "", "Precio Potencia S/ /kW-mes", "Potencia Egreso kW", "Potencia Calculada kW", "Potencia Declarada kW", "Peaje Unitario S//kW-mes", "Barra", "Potencia Activa kW", "Potencia Reactiva KW", "" };

            //Anvho de cada columna
            int[] widths = { 150, 150, 120, 60, 60, 80, 60, 80, 80, 80, 120, 80, 80, 110 };
            object[] columnas = new object[14];
            //Lista de PeajesEgreso por EmprCodi
            string pegrmicalidad2 = "*";
            if (pegrmitipousuario.Equals("")) pegrmitipousuario = "*";
            if (pegrmilicitacion.Equals("")) pegrmilicitacion = "*";
            if (pegrmicalidad.Equals(""))
            { pegrmicalidad = "*"; }
            else if (pegrmicalidad.IndexOf("/") > 0)
            {
                string[] aCalidad = pegrmicalidad.Split('/');
                pegrmicalidad = aCalidad[0];
                pegrmicalidad2 = aCalidad[1];
            }
            modelpe.ListaPeajeEgresoMinfo = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEgresoMinfoVista(pericodi, recpotcodi, emprcodi, cliemprcodi, barrcodi, barrcodifco, pegrmitipousuario, pegrmilicitacion, pegrmicalidad, pegrmicalidad2);
            bool bEstado = true;
            //Se arma la matriz de datos
            string[][] data;

            if (modelpe.ListaPeajeEgresoMinfo.Count() != 0)
            {
                data = new string[modelpe.ListaPeajeEgresoMinfo.Count() + 2][];
                data[0] = Cabecera1;
                data[1] = Cabecera2;
                int index = 2;
                foreach (VtpPeajeEgresoMinfoDTO item in modelpe.ListaPeajeEgresoMinfo)
                {
                    string[] itemDato = { item.Genemprnombre, item.Cliemprnombre, item.Barrnombre, item.Pegrmitipousuario, item.Pegrmilicitacion.ToString(), item.Pegrmipreciopote.ToString(), item.Pegrmipoteegreso.ToString(), item.Pegrmipotecalculada.ToString(), item.Pegrmipotedeclarada.ToString(), item.Pegrmipeajeunitario.ToString(), item.Barrnombrefco, item.Pegrmipoteactiva.ToString(), item.Pegrmipotereactiva.ToString(), item.Pegrmicalidad };
                    data[index] = itemDato;
                    index++;
                }
            }
            else
            {
                data = new string[3][];
                data[0] = Cabecera1;
                data[1] = Cabecera2;
                int index = 2;
                string[] itemDato = { "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                data[index] = itemDato;
            }
            ///////////          
            columnas[0] = new
            {   //Emprcodi - Emprnomb
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = bEstado,
            };
            columnas[1] = new
            {   //Emprcodi - Emprnomb
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = bEstado,
            };
            columnas[2] = new
            {   //Barrcodi - Barrnombre
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = bEstado,
            };
            columnas[3] = new
            {   //Rpsctipousuario
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = bEstado,
            };
            columnas[4] = new
            {   //Licitación
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = bEstado,
            };
            columnas[5] = new
            {   //Precio Potencia
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000",
                readOnly = bEstado,
            };
            columnas[6] = new
            {   //Potencia Egreso
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000000000",
                readOnly = bEstado,
            };
            columnas[7] = new
            {   //Potencia Calculada
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000000000",
                readOnly = bEstado,
            };
            columnas[8] = new
            {   //Potencia Declarada
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000000000",
                readOnly = bEstado,
            };
            columnas[9] = new
            {   //Peaje Unitario
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000",
                readOnly = bEstado,
            };
            columnas[10] = new
            {   //Barrcodifco - Barrnombrefco
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = bEstado,
            };
            columnas[11] = new
            {   //Rpscpoteactiva
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000000000",
                readOnly = bEstado,
            };
            columnas[12] = new
            {   //Rpscpotereactiva
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000000000",
                readOnly = bEstado,
            };
            columnas[13] = new
            {   //Rpsccalidad
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = bEstado,
            };

            #endregion
            model.Data = data;
            model.Widths = widths;
            model.Columnas = columnas;
            model.FixedRowsTop = 2;
            model.FixedColumnsLeft = 3;

            return Json(model);
        }


        #endregion

        /// <summary>
        /// Permite exportar a un archivo excel todos los registros en pantalla de consulta
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int pericodi = 0, int recpotcodi = 0, int emprcodi = 0, int formato = 1, int cliemprcodi = 0, int barrcodi = 0, int barrcodifco = 0, string pegrmitipousuario = "", string pegrmilicitacion = "", string pegrmicalidad = "")
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
                ExcelWorksheet ws = null;
                string file = this.servicioTransfPotencia.GenerarFormatoPeajeEgresoMinfo(pericodi, recpotcodi, emprcodi, formato, pathFile, pathLogo, cliemprcodi, barrcodi, barrcodifco, pegrmitipousuario, pegrmilicitacion, pegrmicalidad, out ws);
                return Json(file);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Descarga el archivo excel
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;

            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, app, sFecha + "_" + file);
        }

    }
}
