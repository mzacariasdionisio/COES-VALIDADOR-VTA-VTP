using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using COES.MVC.Intranet.Areas.TransfPotencia.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using System.Globalization;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Controllers;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class RepaRecaPeajeDetalleController : BaseController
    {
        // GET: /Transfpotencia/RepaRecaPeajeDetalle/
        //[CustomAuthorize]

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        ConstantesTransfPotencia libFuncion = new ConstantesTransfPotencia();
       
        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int pericodi = 0, int recpotcodi=0)
        {
            base.ValidarSesionUsuario();
            RepaRecaPeajeDetalleModel model = new RepaRecaPeajeDetalleModel();
            model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi); 
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name); 
            model.Cantidad = this.servicioTransfPotencia.GetMaxNumEmpresasVtpRepaRecaPeajeDetalles(pericodi, recpotcodi);
            return View(model);
        }

        #region Grilla Excel

        /// <summary>
        /// Muestra la grilla excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcel(int pericodi = 0, int recpotcodi = 0, int cantidad=0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            RepaRecaPeajeModel modelRrp = new RepaRecaPeajeModel();
            modelRrp.Pericodi = pericodi;
            modelRrp.Recpotcodi = recpotcodi;
            RepaRecaPeajeDetalleModel modelRrpd = new RepaRecaPeajeDetalleModel();

            #region Armando de contenido
            
            int cantidadIngresada = cantidad;

            //Cantidad de columnas a agregar // 1 CantidadEmpresas es igual a 2 columnas(Empresa, Porcentaje)         
            int CantidadEmpresas = 2; //Si no existen datos por defecto se crea la tabla con 2 empresas
            //Variables

            if (cantidadIngresada != 0)
                CantidadEmpresas = cantidadIngresada;
            else
            {
                //Obtiene cantidad maxima de empresas si existen datos
                var cantidadResult = this.servicioTransfPotencia.GetMaxNumEmpresasVtpRepaRecaPeajeDetalles(modelRrp.Pericodi, modelRrp.Recpotcodi);
                if (cantidadResult != 0)
                {
                    CantidadEmpresas = cantidadResult;
                }
            }
          
            //Nombre del header de la columna empresa
            string colEmpr = "Empresa";
            //Nombre del header de la columna porcentaje
            string colPorcentaje = "[%]";
            //Tamaño de la columna empresa
            int widthEmpr = 200;
            //Tamaño de la columna porcentaje
            int widthPorcentaje = 40;

            //headers y tamaños de las columnas
            List<string> header = new List<string> (){ "Reparto", "Total %" };
            List<int> width  = new List<int> (){200, 60};       

            //Agregar header de empresas porcentajes y tamaño de columna
            for (int i = 1; i <= CantidadEmpresas; i++)
            {
                header.Add(colEmpr + " " + i.ToString());
                header.Add(colPorcentaje + " " + i.ToString());
                width.Add(widthEmpr);
                width.Add(widthPorcentaje);
            }

            //Headers final a enviar
            string[] headers = header.ToArray();
            //widths final a enviar
            int[] widths = width.ToArray();
            //Obtener cantidad de columnas
            int total = header.Count();
            //Declarando cantidad de columnas a enviar
            object[] columnas = new object[total];   

                 
            //Obtener empresas para el dropdown
            var ListaEmpresas = this.servicioEmpresa.ListEmpresasSTR().Select(x => x.EmprNombre).ToList();
            // Obtener reparto para mostrar el txt cuando se muestra el codigo
            var repartos = this.servicioTransfPotencia.GetByCriteriaVtpRepaRecaPeajes(modelRrp.Pericodi, modelRrp.Recpotcodi).Select(x => x.Rrpenombre).ToList();
       
            
            //Obtener lista de reparecapeajedealle datos            
            modelRrpd.ListaRepaRecaPeajeDetalle = this.servicioTransfPotencia.GetByCriteriaVtpRepaRecaPeajeDetalles(modelRrp.Pericodi, modelRrp.Recpotcodi);      
            //Obtener lista de repartos para obtener datos
            modelRrp.ListaRepaRecaPeaje = this.servicioTransfPotencia.GetByCriteriaVtpRepaRecaPeajes(modelRrp.Pericodi, modelRrp.Recpotcodi);         
         
            //Se arma la matriz de datos
            string[][] data = new string[modelRrp.ListaRepaRecaPeaje.Count()][];
            int index = 0;

            foreach (VtpRepaRecaPeajeDTO item in modelRrp.ListaRepaRecaPeaje)
            {
                List<string> fila = new List<string> ();
                fila.Add(item.Rrpenombre.ToString());
                fila.Add("");
                foreach (VtpRepaRecaPeajeDetalleDTO itemD in modelRrpd.ListaRepaRecaPeajeDetalle)
                {
                    if (item.Rrpecodi == itemD.Rrpecodi)
                    {
                        EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByIdEmpresa(itemD.Emprcodi);
                        fila.Add(dtoEmpresa.EmprNombre);
                        fila.Add(itemD.Rrpdporcentaje.ToString());
                    }

                }
                while (fila.Count < total)
                {   //total: cantidad de columnas, para completar los espacios en blanco
                    fila.Add("");                   
                }

                data[index] = fila.ToArray();
                index++;
            }   

            //Se arman las columnas
            //Columna Reparto
            columnas[0] = new
            {
                type = GridExcelModel.TipoTexto,                
                source = (new List<String>()).ToArray(),
                strict = true,
                dateFormat = string.Empty,
                correctFormat = true,
                defaultDate = string.Empty,
                format = string.Empty,
                readOnly = true
            };
            //Columna Total
            columnas[1] = new
            {              
                type = GridExcelModel.TipoNumerico,               
                source = (new List<Double>()).ToArray(),
                strict = false,
                dateFormat = string.Empty,
                correctFormat = true,
                defaultDate = string.Empty,
                format = string.Empty,
                readOnly = false
            };
            //Se completa las columnas: Empresa y Porcenaje según la cantidad de columnas asignada (total)
            for (int i = 2; i < total; i++)
            {
                if (i % 2 == 0)
                {
                    columnas[i] = new
                    {
                        type = GridExcelModel.TipoLista,
                        source = ListaEmpresas.ToArray(),
                        strict = false,
                        correctFormat = true,
                        readOnly = false,
                    };
                }
                if (i % 2 != 0)
                {
                    columnas[i] = new
                    {
                        type = GridExcelModel.TipoNumerico,
                        source = (new List<Int32>()).ToArray(),
                        strict = true,
                        dateFormat = string.Empty,
                        correctFormat = true,
                        defaultDate = string.Empty,
                        format = "0.00",
                        readOnly = false
                    };
                }
            }
       
            #endregion
            model.CantidadEmpresas = CantidadEmpresas;
            model.ListaEmpresas = ListaEmpresas.ToArray();
            model.Data = data;
            model.Headers = headers;
            model.Widths = widths;
            model.Columnas = columnas;
            model.FixedRowsTop = 0;
            model.FixedColumnsLeft = 2;
            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcel(int pericodi, int recpotcodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";
            if (pericodi == 0 || recpotcodi == 0)
            {
                sResultado = "Lo sentimos, debe seleccionar un mes de valorización y una versión de recalculo";
                return Json(sResultado);
            }
            try
            {
                //Eliminamos la información procesada en el periodo / versión
                string sBorrar = this.servicioTransfPotencia.EliminarProceso(pericodi, recpotcodi);
                if (!sBorrar.Equals("1"))
                {
                    return Json(sBorrar);
                }

                //Elimnina datos antes de grabar
                this.servicioTransfPotencia.DeleteByCriteriaVtpRepaRecaPeajeDetalle(pericodi, recpotcodi);
          
                //Obtener cantidad de filas y columnas de la matriz
                int col = datos[0].Length;
                int row = datos.Length;             
               
                //Loop para recorrer matriz y grabar datos
                for (int i = 0; i < row ;i++ )
                {
                    //Obtiene Codigo de Reparto
                    VtpRepaRecaPeajeDTO dtoRepaRecaPeaje = this.servicioTransfPotencia.GetByNombreVtpRepaRecaPeaje(pericodi, recpotcodi, Convert.ToString(datos[i][0]));
                    for (int j = 2; j < col; j++)
                    {        
                        //Obtiene Codigo de Empresa de la columna par de izquierda a derecha
                        if (j%2 == 0 )
                        {
                            if (String.IsNullOrEmpty((datos[i][j]).ToString()))
                            {   //Si esta en blanco salir del for  (se puede Capturar mensaje )
                                break;
                            }
                            EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(Convert.ToString(datos[i][j]));
                            j++;
                            //Obtiene valor del porcentaje de la columna de los porcentajes inmediato
                            if (String.IsNullOrEmpty((datos[i][j]).ToString()))
                            {   //Si valor esta en blanco salir del for (Se puede capturar mensaje )
                                break;
                            }
                            decimal Porcentaje = UtilTransfPotencia.ValidarNumero(datos[i][j].ToString());  //Convert.ToDecimal(datos[i][j]);
                            //Crear registro                                          
                            VtpRepaRecaPeajeDetalleDTO dtoEntidad = new VtpRepaRecaPeajeDetalleDTO();
                            dtoEntidad.Rrpecodi = dtoRepaRecaPeaje.Rrpecodi;
                            dtoEntidad.Pericodi = pericodi;
                            dtoEntidad.Recpotcodi = recpotcodi;
                            dtoEntidad.Emprcodi = dtoEmpresa.EmprCodi;
                            dtoEntidad.Rrpdporcentaje = Porcentaje;
                            dtoEntidad.Rrpdusucreacion = User.Identity.Name;
                            dtoEntidad.Rrpdusumodificacion = User.Identity.Name;
                            this.servicioTransfPotencia.SaveVtpRepaRecaPeajeDetalle(dtoEntidad);
                        }
                    }
                }
                return Json(sResultado);
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1"
                return Json(sResultado);
            }
        }

        #endregion

        /// <summary>
        /// Permite eliminar todos los registros del periodo y versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarDatos(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";
            try
            {
                this.servicioTransfPotencia.DeleteByCriteriaVtpRepaRecaPeajeDetalle(pericodi, recpotcodi);
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

        }

        /// <summary>
        /// Permite exportar a un archivo todos los registros en pantalla
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int pericodi = 0, int recpotcodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
                string file = this.servicioTransfPotencia.GenerarFormatoRepaRecaPeajeDetalle(pericodi, recpotcodi, formato, pathFile, pathLogo);
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

            return File(path, app, sFecha + "_" + file);
        }
    }
}
