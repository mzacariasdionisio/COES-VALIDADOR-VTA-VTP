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
    public class RelacionAreasController : BaseController
    {

        PronosticoDemandaAppServicio servicio = new PronosticoDemandaAppServicio();
        RelacionAreasModel model = new RelacionAreasModel();
        //
        // GET: /PronosticoDemanda/RelacionAreas/

        public ActionResult Index()
        {
            RelacionAreasModel model = new RelacionAreasModel();

            model.ListaNiveles = servicio.ListEqAreanivel().Where(x => x.ANivelCodi != -1).ToList();
            model.ListaAreas = servicio.ListPrnArea(ConstantesProdem.NvlAreaOperativa);

            return View(model);
        }

        /// <summary>
        /// Muestra el detalle del nivel seleccionado
        /// </summary>
        /// <param name="idNivel"></param>
        /// <returns></returns>
       [HttpPost]
        public JsonResult DetalleList(int idNivel)//Detalle//ListAgrupacion
        {  
            List<object> tabla = new List<object>();
            tabla = this.servicio.GetAreaOperativaByNivel(idNivel);

            return Json(tabla);
        }

        [HttpPost]
        public PartialViewResult DetalleUpdate(int areapadre, int relpadre, int relnivel) {

            int areanivel, arearelnivel;

            areanivel = arearelnivel = ConstantesProdem.NvlSubestacion;

            model.ListaSubestacionesSeleccionadas = this.servicio.GetSubEstacionSeleccionadas(areapadre, areanivel);
            model.ListaSubestacionesDisponibles = this.servicio.GetSubEstacionDisponibles(areanivel,areapadre,relpadre,relnivel,arearelnivel);//areanivel, areapadre,relpadre,relnivel,arearelnivel

            model.JSeleccionados = JsonConvert.SerializeObject(model.ListaSubestacionesSeleccionadas);
            model.JDisponibles = JsonConvert.SerializeObject(model.ListaSubestacionesDisponibles);

            return PartialView(model);
        }

        /// <summary>
        /// Realiza el registro de una nueva agrupación
        /// </summary>
        /// <param name="idArea"></param>
        /// <param name="listPuntos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(int idArea, List<EqAreaDTO> listPuntos)
        {
            //base.ValidarSesionUsuario();
            EqAreaRelDTO entityAreaRel = new EqAreaRelDTO();

            //Borrando las Subestaciones del AO.
            this.servicio.DeleteEqAreaRelByPadre(idArea);

            //Borrando las subestaciones que pertenecian a otra AO
            //this.servicio.DeleteEqAreaRel(IdArea, ConstantesProdem.NvlSubestacion);
            foreach (var item in listPuntos)
            {
                if (item.Areapadre != idArea && item.Areapadre != 0)
                {
                    this.servicio.DeleteEqAreaRel(item.Areacodi, item.Areapadre);
                }
            }


            foreach (var item in listPuntos)
            {
                entityAreaRel = new EqAreaRelDTO();
                entityAreaRel.AreaPadre = idArea;
                entityAreaRel.AreaCodi = item.Areacodi;
                entityAreaRel.FechaDat = DateTime.Now;
                entityAreaRel.LastCodi = -1;
                entityAreaRel.Arearlusumodificacion = "root";
               // entityAreaRel.Arearlfecmodificacion = DateTime.Now;
               //Log.Info("Registra los puntos de medición de usuarios libres correspondientes a la agrupación - SavePrnAgrupacion");
                this.servicio.SaveEqAreaRel(entityAreaRel);
            }

            return Json(model);
        }

        //SubestacionAO
        /// <summary>
        /// Realiza el registro de una nueva agrupación
        /// </summary>
        /// <param name="areacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SubestacionAO(string areacodi)
        {
            //base.ValidarSesionUsuario();
            model.Area = this.servicio.GetAreaOperativaBySubestacion(Convert.ToInt32(areacodi));

            return Json(model);
        }

        //RelacionSubestacionUpdate
        /// <summary>
        /// Realiza el registro de una nueva agrupación
        /// </summary>
        /// <param name="idArea"></param>
        /// <param name="listPuntos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RelacionSubestacionUpdate(int idArea, int idSubestacion)
        {
            EqAreaRelDTO entityAreaRel = new EqAreaRelDTO();

            entityAreaRel = this.servicio.GetSubestacionRel(idSubestacion);

            if (idArea != 0)
            {
                if (entityAreaRel.AreaCodi != 0)
                {
                    this.servicio.DeleteEqAreaRel(entityAreaRel.AreaCodi, Int32.Parse(entityAreaRel.AreaPadre.ToString()));
                }

                entityAreaRel = new EqAreaRelDTO();

                entityAreaRel.AreaPadre = idArea;
                entityAreaRel.AreaCodi = idSubestacion;
                entityAreaRel.FechaDat = DateTime.Now;
                entityAreaRel.LastCodi = -1;
                entityAreaRel.Arearlusumodificacion = "root";
                this.servicio.SaveEqAreaRel(entityAreaRel);

                model.Mensaje = "La relacion se registro con exito";
            }
            else {
                model.Mensaje = "Debe seleccionar una Area Operativa para poder registrar la relacion...";
            }
            return Json(model);
        }


        [HttpPost]
        public PartialViewResult RelacionSubestacionBarra(string areacodi)
        {
            model.ListaBarrasSeleccionadas = this.servicio.GetBarrasSeleccionadas(Convert.ToInt32(areacodi));
            model.ListaBarrasDisponibles = this.servicio.GetBarrasDisponibles(Convert.ToInt32(areacodi));

            model.BarrasSeleccionados = JsonConvert.SerializeObject(model.ListaBarrasSeleccionadas);
            model.BarrasDisponibles = JsonConvert.SerializeObject(model.ListaBarrasDisponibles);

            return PartialView(model);
        }

        /// <summary>
        /// Realiza el registro de una nueva relacion para Barras
        /// </summary>
        /// <param name="subestacion"></param>
        /// <param name="listBarrasA"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarBarraSeleccionados(int subestacion, List<Object> listBarrasA)//List<PrGrupoDTO> listBarrasA, 
        {
            //base.ValidarSesionUsuario();
            PrGrupoDTO entity = new PrGrupoDTO();

            for (int i = 0; i < listBarrasA.Count; i++)
            {
                entity.Grupocodi = Int32.Parse(listBarrasA[i].ToString());
                entity.Areacodi = subestacion;

                this.servicio.UpdatePrGrupo(entity.Grupocodi, entity.Areacodi);
            }
            model.Mensaje = "Relacion registrada";

            return Json(model);
        }

        /// <summary>
        /// Realiza el registro de una nueva relacion para Barras
        /// </summary>
        /// <param name="listBarrasB"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarBarraDisponibles(List<Object> listBarrasB)
        {
            //base.ValidarSesionUsuario();
            EqAreaRelDTO entityAreaRel = new EqAreaRelDTO();

            PrGrupoDTO entity = new PrGrupoDTO();
            if (listBarrasB != null)
            {
                for (int i = 0; i < listBarrasB.Count; i++)
                {
                    entity.Grupocodi = Int32.Parse(listBarrasB[i].ToString());
                    entity.Areacodi = -1;

                    this.servicio.UpdatePrGrupo(entity.Grupocodi, entity.Areacodi);
                }

            }
            return Json(model);
        }
    }
}
