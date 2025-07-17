using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using COES.MVC.Intranet.Areas.PronosticoDemanda.Models;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using COES.Dominio.DTO.Sic;
using log4net;
using COES.MVC.Intranet.Controllers;
using System.Reflection;
using System.Globalization;
using Newtonsoft.Json;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Controllers
{
    public class RelacionBarrasController : BaseController
    {
        PronosticoDemandaAppServicio servicio = new PronosticoDemandaAppServicio();

        public ActionResult Index()
        {
            RelacionBarrasModel model = new RelacionBarrasModel();
            model.Mensaje = "Puede relacionar un punto de medición o una agrupación a una barra PM";
            model.TipoMensaje = ConstantesProdem.MsgInfo;
            model.ListArea = UtilProdem.ListAreaOperativa(false);
            model.ListBarraPM = this.servicio.ListRelacionBarrasPM(ConstantesProdem.NvlSubestacion,
                ConstantesProdem.NvlAreaOperativa, ConstantesProdem.Prcatecodi, "0", "0");
            model.ListEmpresa = this.servicio.ListEmpresaBarrasRel();
            model.ListAgrupaciones = this.servicio.AgrupacionesList(ConstantesProdem.OriglectcodiAgrupacion, ConstantesProdem.BarraNoDefinido);
            model.ListPuntos = this.servicio.ListPuntosNoAgrupaciones(ConstantesProdem.OriglectcodiAgrupacion, ConstantesProdem.BarraNoDefinido, ConstantesProdem.OriglectcodiPR03);
            return View(model);
        }

        public JsonResult List(int start, int length, RelacionBarrasModel dataFiltros)
        {
            string idArea = (dataFiltros.SelArea.Count != 0) ? string.Join(",", dataFiltros.SelArea) : "0";
            string idBarra = (dataFiltros.SelBarra.Count != 0) ? string.Join(",", dataFiltros.SelBarra) : "0";
            string idEmpresa = (dataFiltros.SelEmpresas.Count != 0) ? string.Join(",", dataFiltros.SelEmpresas) : "0";

            object res = this.servicio.RelacionBarraList(start, length, idArea, idBarra, idEmpresa);

            return Json(res);
        }

        /// <summary>
        /// Carga toda la informacion de listas y tablas para el popup
        /// </summary>
        /// <param name="barra"></param>
        /// <returns></returns>
        public JsonResult ListDataPopup(int barra)
        {
            RelacionBarrasModel model = new RelacionBarrasModel();

            model.ListEmpresas = this.servicio.PR03Empresas();
            model.ListUbicaciones = this.servicio.PR03Ubicaciones();
            
            model.DtAgrupaciones = this.servicio.AgrupacionesDtList();
            model.DtPuntos = this.servicio.PuntosDtList();
            model.DtSeleccionados = this.servicio.AgrupacionesPuntosDtList(barra);

            return Json(model);
        }

        /// <summary>
        /// Grabar relacion barra en tabla MePtoMedicion
        /// </summary>
        /// <param name="entData">Lista de objetos seleccionados</param>
        /// <param name="idBarra">Identificador de la tabla PR_GRUPO</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Save(List<PrnPtomedpropDTO> entData, int idBarra)
        {
            List<MePtomedicionDTO> relPrevias = this.servicio.PuntosBarraList(idBarra);

            //Reinicia los puntos de medición relacionados a la barra
            foreach (var i in relPrevias)
            {
                this.servicio.UpdateMeMedicionBarra(i.Ptomedicodi,
                    ConstantesProdem.BarraNoDefinido, User.Identity.Name);
            }

            //Registra las nuevas relaciones
            foreach (var j in entData)
            {
                //Crea las nuevas relaciones entre barras y puntos de medición
                this.servicio.UpdateMeMedicionBarra(j.Ptomedicodi, idBarra, User.Identity.Name);

                //Registra las nuevas propiedades del punto de medición
                PrnPtomedpropDTO entity = this.servicio.GetByIdPrnPtomedprop(j.Ptomedicodi);
                if (entity.Ptomedicodi != 0)
                {
                    entity.Prnpmpvarexoproceso = j.Prnpmpvarexoproceso;
                    this.servicio.UpdatePrnPtomedprop(entity);
                }
                else
                {
                    entity = new PrnPtomedpropDTO
                    {
                        Ptomedicodi = j.Ptomedicodi,
                        Prnpmpvarexoproceso = j.Prnpmpvarexoproceso,
                        Prnpmpusucreacion = User.Identity.Name,
                        Prnpmpfeccreacion = DateTime.Now,
                        Prnpmpusumodificacion = User.Identity.Name,
                        Prnpmpfecmodificacion = DateTime.Now
                    };
                    this.servicio.SavePrnPtomedprop(entity);
                }
            }

            return Json("Registro Exitoso!");
        }

        /// <summary>
        /// Actualiza la lista de puntos de medición disponibles segun filtros
        /// </summary>
        /// <param name="dataFiltros">Datos de los filtros seleccionados</param>
        /// <returns></returns>
        public JsonResult UpdatePopupDtPuntos(RelacionBarrasModel dataFiltros)
        {
            List<MePtomedicionDTO> dataPuntos = this.servicio.
                PR03PuntosNoAgrupados().Where(x => x.Grupocodibarra == -1).ToList();
            
            if (dataFiltros.SelUbicaciones.Count != 0)
            {
                dataPuntos = dataPuntos.
                    Where(x => dataFiltros.SelUbicaciones.
                    Contains(x.Areacodi)).ToList();
            }
            if (dataFiltros.SelEmpresas.Count != 0)
            {
                dataPuntos = dataPuntos.
                    Where(x => dataFiltros.SelEmpresas.
                    Contains(x.Emprcodi ?? 0)).ToList();
            }

            object entity;
            List<object> res = new List<object>();
            foreach (var e in dataPuntos)
            {
                entity = new
                {
                    id = e.Ptomedicodi,
                    nombre = e.Ptomedidesc,
                    ubicacion = e.Areanomb,
                    empresa = e.Emprnomb,
                    barracodi = e.Grupocodibarra
                };

                res.Add(entity);
            }

            return Json(res);
        }

        /// <summary>
        /// Refresca la lista de Empresas segun las barras seleccionadas
        /// </summary>
        /// <param name="barra"></param>
        /// <returns></returns>
        public JsonResult RefreshListEmpresa(List<int> barrasel)
        {
            RelacionBarrasModel model = new RelacionBarrasModel();
            string idBarra = (barrasel.Count != 0) ? string.Join(",", barrasel) : "0";
            model.ListEmpresa = this.servicio.ListEmpresaByBarra(idBarra);

            return Json(model.ListEmpresa);
        }

        /// <summary>
        /// Lista las empresas
        /// </summary>
        /// <returns></returns>
        public JsonResult ListEmpresa()
        {
            RelacionBarrasModel model = new RelacionBarrasModel();
            model.ListEmpresa = this.servicio.ListEmpresaBarrasRel();

            return Json(model.ListEmpresa);
        }

        /// <summary>
        /// Filtro de barra refrescado mediante el checkbox
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public JsonResult FiltroBarra(int flag)
        {
            RelacionBarrasModel model = new RelacionBarrasModel();

            if (flag == 0)
            {
                model.ListBarraPM = this.servicio.ListRelacionBarrasPM(ConstantesProdem.NvlSubestacion,
                                        ConstantesProdem.NvlAreaOperativa, ConstantesProdem.Prcatecodi, "0", "0");
            }
            else { 
                model.ListBarraPM = this.servicio.ListBarrasInPtoMedicion(ConstantesProdem.NvlSubestacion,
                                        ConstantesProdem.NvlAreaOperativa, ConstantesProdem.Prcatecodi, "0", "0");
            }
            model.ListEmpresa = this.servicio.ListEmpresaBarrasRel();
         
            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;


            return JsonResult;
        }
    }
}