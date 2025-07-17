using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.TiempoReal.Models;
using COES.Servicios.Aplicacion.Medidores;
using COES.Servicios.Aplicacion.Equipamiento;
using System.Collections;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Mantto;
using COES.MVC.Intranet.Areas.TiempoReal.Helper;
using COES.Servicios.Aplicacion.Eventos.Helper;
using System.IO;
using System.Text;
using COES.Servicios.Aplicacion.TiempoReal.Helper;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.TiempoReal;


namespace COES.MVC.Intranet.Areas.TiempoReal.Controllers
{
    public class ControlController : BaseController
    {
        ControlAppServicio servControl = new ControlAppServicio();
        EquipamientoAppServicio servEquipamiento = new EquipamientoAppServicio();
        MedidoresAppServicio servMedidores = new MedidoresAppServicio();
        EventoAppServicio servEvento = new EventoAppServicio();
        DespachoAppServicio servDespacho = new DespachoAppServicio();

        public ActionResult Index()
        {
            BusquedaAgcControlModel model = new BusquedaAgcControlModel();
            model.FechaGeneral = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaCVIni = DateTime.Now.AddDays(-7).ToString(Constantes.FormatoFecha);
            model.FechaCVFin = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Permite editar el control
        /// </summary>
        /// <param name="id">Código de control</param>
        /// <param name="accion">Acción: 0: ver, 1:editar</param>
        /// <returns></returns>
        public ActionResult EditarControl(int id, int accion)
        {
            AgcControlModel model = new AgcControlModel();
            AgcControlDTO agcControl = null;

            model.ListaMePtomedicion = servControl.ListarControlCentralizado();
            model.ListaEqEquipo = servEquipamiento.ListarEquiposAGC();

            if (id != 0)
                agcControl = servControl.GetByIdAgcControl(id);

            if (agcControl != null)
            {
                model.AgcControl = agcControl;
            }
            else
            {
                agcControl = new AgcControlDTO();
                agcControl.Ptomedicodi = Convert.ToInt32(Constantes.ParametroDefecto);
                model.AgcControl = agcControl;
            }

            //Punto de control
            if (id != 0)
            {
                model.ListaControlPunto = servControl.GetByIdAgcControlPunto(id);
            }
            else
            {
                model.ListaControlPunto = new List<AgcControlPuntoDTO>();
            }

            model.Accion = accion;
            return View(model);
        }

        /// <summary>
        /// Permite editar el punto de medición
        /// </summary>
        /// <param name="id">Código de Punto de medición para el AGC</param>
        /// <param name="accion">Acción: 0: ver, 1:editar</param>
        /// <returns></returns>
        public PartialViewResult EditarPMedicion(int id, int accion)
        {
            MePtomedicionModel model = new MePtomedicionModel();
            MePtomedicionDTO ptoMedicionAGC = null;

            ptoMedicionAGC = servControl.ListarPotenciaEquipo(id);

            if (ptoMedicionAGC != null)
            {
                model.MePtomedicion = ptoMedicionAGC;
            }

            model.Accion = accion;
            return PartialView(model);
        }

        /// <summary>
        /// Permite editar los costos variables para el AGC
        /// </summary>
        /// <param name="id">Código de costo variable</param>
        /// <param name="accion">Acción: 0: ver, 1:editar</param>
        /// <returns></returns>
        public PartialViewResult EditarCostoVariableAGC(int id, int accion)
        {
            MePtomedicionModel model = new MePtomedicionModel();
            model.ListaPrGrupo = servDespacho.ListarModoOperacionCategoriaTermico();
            model.MePtomedicion = servControl.GetByIdAgc(id);
            model.Accion = accion;
            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar los puntos de control relacionados a un control
        /// </summary>
        /// <param name="idAgccCodi">Código de control</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarControlPunto(int idAgccCodi)
        {
            try
            {
                servControl.DeleteAgcControlPunto(idAgccCodi);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar los puntos de control a nivel de detalle
        /// </summary>
        /// <param name="tipoControl">Tipo de Control: Centralizado, Proporcional</param>
        /// <param name="idControl">Código de control</param>
        /// <param name="ptomediCodi">Punto medición</param>
        /// <param name="equiCodi">Código de equipo</param>
        /// <param name="b2">B2 (Siemens)</param>
        /// <param name="b3">B3 (Siemens)</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarControlPunto(string data, int idControl)
        {
            try
            {
                //eliminar codigo
                servControl.DeleteAgcControlPunto(idControl);

                //int[] valores = Array.ConvertAll(data.Split(','), string. int.Parse);

                string[] dataTotal = data.Split(',');

                int rec = 0;

                while (rec < dataTotal.Length)
                {
                    AgcControlPuntoDTO entity = new AgcControlPuntoDTO();
                    entity.Agccpcodi = 0;
                    entity.Agcccodi = idControl;

                    if (dataTotal[rec + 1] == "C")
                    {
                        entity.Ptomedicodi = Convert.ToInt32(dataTotal[rec + 2]);
                    }
                    else
                    {
                        entity.Equicodi = Convert.ToInt32(dataTotal[rec + 3]);
                        entity.Agccpb2 = dataTotal[rec + 4];
                        entity.Agccpb3 = dataTotal[rec + 5];
                    }

                    entity.Agccpusucreacion = User.Identity.Name;
                    entity.Agccpfeccreacion = DateTime.Now;
                    int id = servControl.SaveAgcControlPuntoId(entity);

                    rec += 5;
                }




                //AgcControlPuntoDTO entity = new AgcControlPuntoDTO();
                //entity.Agccpcodi=0;
                //entity.Agcccodi = idControl;

                //if (tipoControl == "C")
                //{
                //    entity.Ptomedicodi = ptomediCodi;
                //}
                //else
                //{
                //    entity.Equicodi = equiCodi;
                //    entity.Agccpb2 = b2;
                //    entity.Agccpb3 = b3;
                //}

                //entity.Agccpusucreacion = User.Identity.Name;
                //entity.Agccpfeccreacion = DateTime.Now;

                //int id = servControl.SaveAgcControlPuntoId(entity);
                //return Json(1);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite grabar los puntos de control a nivel de detalle
        /// </summary>
        /// <param name="tipoControl">Tipo de Control: Centralizado, Proporcional</param>
        /// <param name="idControl">Código de control</param>
        /// <param name="ptomediCodi">Punto medición</param>
        /// <param name="equiCodi">Código de equipo</param>
        /// <param name="b2">B2 (Siemens)</param>
        /// <param name="b3">B3 (Siemens)</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarControlPunto1(string tipoControl, int idControl, int ptomediCodi, int equiCodi, string b2, string b3)
        {
            try
            {
                AgcControlPuntoDTO entity = new AgcControlPuntoDTO();
                entity.Agccpcodi = 0;
                entity.Agcccodi = idControl;

                if (tipoControl == "C")
                {
                    entity.Ptomedicodi = ptomediCodi;
                }
                else
                {
                    entity.Equicodi = equiCodi;
                    entity.Agccpb2 = b2;
                    entity.Agccpb3 = b3;
                }

                entity.Agccpusucreacion = User.Identity.Name;
                entity.Agccpfeccreacion = DateTime.Now;

                int id = servControl.SaveAgcControlPuntoId(entity);
                return Json(id);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite deshabilitar un control configurado del AGC
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DesactivarControl(int id)
        {
            try
            {
                AgcControlDTO entity = null;

                if (id != 0)
                {
                    entity = servControl.GetByIdAgcControl(id);
                    entity.Agccusumodificacion = User.Identity.Name;
                    entity.Agccfecmodificacion = DateTime.Now;
                    entity.Agccvalido = "N";
                    servControl.UpdateAgcControl(entity);
                    return Json(1);
                }
                return Json(-1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar un control para el AGC
        /// </summary>
        /// <param name="model">modelo del tipo AgcControlModel</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarControl(AgcControlModel model)
        {
            try
            {
                AgcControlDTO entity = new AgcControlDTO();

                entity.Agcccodi = model.AgccCodi;
                entity.Agcctipo = model.AgccTipo;

                if (entity.Agcctipo == "C")
                {
                    entity.Agccb2 = model.AgccB2;
                    entity.Agccb3 = model.AgccB3;
                }
                else
                {
                    entity.Ptomedicodi = model.PtomediCodi;
                }

                entity.Agccdescrip = model.AgccDescrip;
                entity.Agccvalido = model.AgccValido;
                entity.Agccusucreacion = model.AgccUsuCreacion;

                if (entity.Agcccodi == 0)
                {
                    entity.Agccusucreacion = User.Identity.Name;
                    entity.Agccfeccreacion = DateTime.Now;
                }
                else
                {
                    if (model.AgccUsuCreacion != null)
                    {
                        entity.Agccusucreacion = model.AgccUsuCreacion;
                    }

                    if (model.AgccFecCreacion != null)
                    {
                        entity.Agccfeccreacion = DateTime.ParseExact(model.AgccFecCreacion, Constantes.FormatoFechaFull,
                            CultureInfo.InvariantCulture);
                    }

                    entity.Agccusumodificacion = User.Identity.Name;
                    entity.Agccfecmodificacion = DateTime.Now;
                }

                int id = this.servControl.SaveAgcControlId(entity);
                return Json(id);

            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar un punto de medición para el AGC
        /// </summary>
        /// <param name="model">Modelo del tipo MePtomedicionModel</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarPMedicion(MePtomedicionModel model)
        {
            try
            {
                MePtomedicionDTO entity = new MePtomedicionDTO();
                entity.Ptomedicodi = model.PtomediCodi;
                entity.Ptomedielenomb = model.PtomediEleNomb; //NCP
                entity.Ptomedibarranomb = model.PtomediBarraNomb; //B2
                entity.Ptomedidesc = model.PtomediDesc; //B3
                entity.Lastuser = User.Identity.Name;
                entity.Lastdate = DateTime.Now;

                servMedidores.UpdateMePtoMedicion(entity);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar los costos variables para el AGC
        /// </summary>
        /// <param name="model">modelo del tipo MePtomedicionModel</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarCVariable(MePtomedicionModel model)
        {
            try
            {
                MePtomedicionDTO entity = new MePtomedicionDTO();
                entity.Ptomedicodi = model.PtomediCodi;
                entity.Grupocodi = model.GrupoCodi;
                entity.Ptomedibarranomb = model.PtomediBarraNomb; //B2
                entity.Ptomedidesc = model.PtomediDesc; //B3
                entity.Lastuser = User.Identity.Name;
                entity.Lastdate = DateTime.Now;

                servMedidores.UpdateMePtoMedicionCVariable(entity);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite obtener la configuración de despacho AGC
        /// </summary>
        /// <param name="estado">Estado de datos</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaDespachoConfiguracion(string estado, int nroPage)
        {
            BusquedaAgcControlModel model = new BusquedaAgcControlModel();
            model.ListaAgcControl = servControl.ListAgcControls(estado, nroPage, Constantes.PageSizeEvento);
            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener la lista de potencia configuración de despacho AGC
        /// </summary>
        /// <param name="familia">Familia</param>
        /// <param name="idOriglectura">Origen lectura</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaPotenciaConfiguracion(string familia, int idOriglectura, string control)
        {
            BusquedaAgcControlModel model = new BusquedaAgcControlModel();
            model.ListaMePtomedicion = servControl.ListarPotencia(familia, idOriglectura, control);
            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener la lista de configuración relacionada a los costos variables para el AGC
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaCostoVariableAGC()
        {
            BusquedaAgcControlModel model = new BusquedaAgcControlModel();
            model.ListaMePtomedicion = servControl.ListarCostoVariableAGC();
            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener la lista de equipos con propiedades configuradas
        /// </summary>
        /// <param name="fecha">Fecha de consulta</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaEquipoPropiedad(string fecha)
        {
            BusquedaAgcControlModel model = new BusquedaAgcControlModel();
            model.ListaEqEquipoPropiedad = servEquipamiento.ListarEquiposPropiedadesAGC(fecha);
            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener los reportes de costo variable disponibles
        /// </summary>
        /// <param name="fechaIni">Fecha inicial de consulta</param>
        /// <param name="fechaFin">Fecha final de consulta</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaCostoVariable(string fechaIni, string fechaFin)
        {
            BusquedaAgcControlModel model = new BusquedaAgcControlModel();
            model.ListaCostoVariable = this.servControl.ObtenerReporte(fechaIni, fechaFin);
            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener el XML de despacho (Hidro y Termoeléctrico)
        /// </summary>
        /// <param name="lectCodi">Código de lectura</param>
        /// <param name="bloqueIni">Bloque inicial</param>
        /// <param name="fecha">Fecha de consulta</param>
        /// <param name="evenClase">Clase de evento</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CrearXmlDespacho(int lectCodi, int bloqueIni, string fecha, int evenClase)
        {
            DateTime fechaInicio = DateTime.Now;
            string result = "";
            string log = "";
            int numlogAlerta = 0;

            string xmlTermo = "";
            string xmlHidro = "";

            try
            {
                if (fecha != null)
                {
                    fechaInicio = DateTime.ParseExact(fecha, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                }

                List<AgcControlDTO> listaAgcControl = servControl.ListAgcControls("S", -1, -1);

                //--- INICIO

                // for de N agrupamientos
                foreach (AgcControlDTO item in listaAgcControl)
                {

                    int agcControlCodi = item.Agcccodi;
                    string agcDescripcion = item.Agccdescrip;
                    string tipoControl = item.Agcctipo;
                    string b2 = item.Agccb2;
                    string b3 = item.Agccb3;

                    //control centralizado
                    if (tipoControl == ConstantesTiempoReal.ControlCentralizado)
                    {

                        List<AgcControlPuntoDTO> ListaControlPunto = servControl.ObtenerPorControl(agcControlCodi);

                        string ptomedicodi = "";

                        //cabecera
                        foreach (AgcControlPuntoDTO itemPunto in ListaControlPunto)
                        {
                            ptomedicodi += itemPunto.Ptomedicodi + ",";
                        }

                        if (ptomedicodi != "")
                        {
                            ptomedicodi = ptomedicodi.Substring(0, ptomedicodi.Length - 1);
                            //genera tramaXML
                            bool retorno = ObtenerXmlPorTipo(lectCodi, bloqueIni, ref xmlHidro, ref xmlTermo,
                                ptomedicodi, b2, b3, log, fechaInicio);
                        }
                    }
                    else
                    {
                        //control proporcional
                        if (tipoControl == ConstantesTiempoReal.ControlProporcional)
                        {
                            //para centrales hidroelectricas
                            //obtener los equicodi de la configuración                            
                            List<AgcControlPuntoDTO> listaControlPunto =
                                servControl.ObtenerPorControl(agcControlCodi);

                            int equicodi = -1; //string Equicodi;
                            Hashtable htEquipoHora = new Hashtable();
                            string equicodiRef = "";
                            string equipadre = "";

                            //equicodi configurado
                            foreach (AgcControlPuntoDTO itemPunto in listaControlPunto)
                            {
                                equicodi = Convert.ToInt32(itemPunto.Equicodi);
                                equicodiRef += equicodi + ",";

                                if (!htEquipoHora.ContainsKey(equicodi))
                                {
                                    PotenciaMW objPM = new PotenciaMW();
                                    objPM.B2 = itemPunto.Agccpb2;
                                    objPM.B3 = itemPunto.Agccpb3;

                                    htEquipoHora.Add(equicodi, objPM);
                                }

                                EqEquipoDTO equipoPadre = servEquipamiento.GetByIdEqEquipo(equicodi);
                                string equipadreRef = (equipoPadre.Equipadre == null
                                    ? ""
                                    : equipoPadre.Equipadre.ToString());

                                if (equipadre != "")
                                {
                                    if (equipadre != equipadreRef)
                                    {
                                        log +=
                                            "Central no es la misma para todas las configuraciones. Configuración: " +
                                            agcDescripcion + "\r\nNose puede continuar.\r\n";
                                        continue;
                                    }

                                }
                                else
                                {
                                    equipadre = equipadreRef;
                                }

                            }

                            //si todos los equipos tienen la misma central hisdroelectrica se continua.
                            if (equipadre == "")
                            {
                                log += "Central no configurada. Configuración: " + agcDescripcion +
                                       "\r\nNose puede continuar.\r\n";
                                continue;
                            }
                            else
                            {
                                if (equipadre != "0" && equipadre != "-1")
                                {

                                    EqEquipoDTO equipo = servEquipamiento.GetByIdEqEquipo(Convert.ToInt32(equipadre));

                                    if (equipo.Famcodi != 4 && equipo.Famcodi != 5) //if (temp != "4" && temp != "5")
                                    {
                                        log += "Central debe ser Hidráulica o Térmica. Configuración: " + agcDescripcion +
                                               "\r\n";

                                        continue;
                                    }
                                    else
                                    {
                                        //HIDRO
                                        if (equipo.Famcodi == 4)
                                        {

                                            //DATOS DE CENTRAL
                                            //CENTRAL
                                            //mantenimiento de equipo E/S: MW indisponible. MW= P. Efectiva (equipamiento) - MW indisponible.
                                            //mantenimiento de equipo F/S:                  MW=0
                                            //potencia efectiva
                                            //potencia mínima
                                            PotenciaMW objCentral = new PotenciaMW();
                                            AsignaMWyAnalizaManttoyOperacionesVarias(ref objCentral, equipadre,
                                                evenClase, 46, 941, true, lectCodi, ref numlogAlerta, agcDescripcion,
                                                ref log, fecha);



                                            //DATOS DE EQUIPOS
                                            foreach (int equicodi2 in htEquipoHora.Keys)
                                            {
                                                PotenciaMW objDatos = (PotenciaMW)htEquipoHora[equicodi2];
                                                AsignaMWyAnalizaManttoyOperacionesVarias(ref objDatos,
                                                    equicodi2.ToString(), evenClase, 164, 299, false, lectCodi,
                                                    ref numlogAlerta, agcDescripcion, ref log, fecha);

                                            }

                                            // de 1 a 48

                                            for (int i = 0; i < 48; i++)
                                            {
                                                //si MW Central ==0
                                                if (objCentral.MW[i] == 0)
                                                {
                                                    //todos los MW son 0
                                                    //generar tramas 0 para cada equipo
                                                    foreach (int Equicodi2 in htEquipoHora.Keys)
                                                    {
                                                        PotenciaMW objDatos = (PotenciaMW)htEquipoHora[Equicodi2];

                                                        objDatos.PAsignada[i] = 0;

                                                    }

                                                }
                                                else
                                                {
                                                    //si MW Central !=0
                                                    //P.Max Equipo = min(P.Efectiva,MW disponible) --SE ASIGNO ANTES Y ESTA EN MW
                                                    //P.Min Equipo = P.Minima

                                                    //DATOS DE EQUIPOS
                                                    double pAsignadaTotal = 0;
                                                    double pTotalxAsignar = objCentral.MW[i]; //potencia totalxasignar
                                                    foreach (int Equicodi2 in htEquipoHora.Keys)
                                                    {
                                                        PotenciaMW objDatos = (PotenciaMW)htEquipoHora[Equicodi2];

                                                        if (objDatos.MW[i] == 0)
                                                        {
                                                            objDatos.PAsignada[i] = -1;
                                                        }
                                                        else
                                                        {
                                                            if (pTotalxAsignar - objDatos.PotenciaMinima > 0)
                                                            {
                                                                //P.Min Equipo Asignada = P.Min Equipo
                                                                objDatos.PAsignada[i] = objDatos.PotenciaMinima;
                                                                pAsignadaTotal += objDatos.PotenciaMinima;
                                                                pTotalxAsignar -= objDatos.PotenciaMinima;
                                                            }
                                                            else
                                                            {
                                                                objDatos.PAsignada[i] = -1;
                                                            }
                                                        }

                                                    }

                                                    //PxAsig = MW Central - P.Min Equipo Asignada (para cada equipo)
                                                    double pxAsig = objCentral.MW[i] - pAsignadaTotal;

                                                    //Delta Equipo=P.Max – P.Min. si Delta>0, Delta=0;
                                                    double sumaDelta = 0;
                                                    foreach (int Equicodi2 in htEquipoHora.Keys)
                                                    {
                                                        PotenciaMW objDatos = (PotenciaMW)htEquipoHora[Equicodi2];

                                                        if (objDatos.PAsignada[i] > 0)
                                                        {

                                                            double delta = objDatos.MW[i] - objDatos.PAsignada[i];


                                                            if (delta >= 0)
                                                                objDatos.Delta[i] = delta;
                                                            else
                                                                objDatos.Delta[i] = 0;

                                                            //Suma Delta equipo
                                                            sumaDelta += objDatos.Delta[i];
                                                        }

                                                    }

                                                    //Proporción: Delta Equipo/Suma Delta equipo
                                                    //MW asignado adicional equipo: MW +PxAsig*Proporción
                                                    if (sumaDelta == 0)
                                                    {
                                                        //todos los MW de los equipos son 0
                                                        foreach (int equicodi2 in htEquipoHora.Keys)
                                                        {
                                                            PotenciaMW objDatos = (PotenciaMW)htEquipoHora[equicodi2];
                                                            objDatos.PAsignada[i] = 0;

                                                        }
                                                    }
                                                    else
                                                    {
                                                        foreach (int equicodi2 in htEquipoHora.Keys)
                                                        {
                                                            PotenciaMW objDatos = (PotenciaMW)htEquipoHora[equicodi2];
                                                            if (objDatos.PAsignada[i] > 0)
                                                            {
                                                                objDatos.PAsignada[i] += pxAsig * objDatos.Delta[i] /
                                                                                         (sumaDelta * 1.0);
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            //imprimiendo los valores de Pasignada
                                            xmlHidro += ObtenerXmlPorCentralEquipo(htEquipoHora, bloqueIni, fecha);

                                        }
                                        else
                                        {
                                            log +=
                                                "Caso de central termoeléctrica no cubierto en el alcance. Configuración: " +
                                                agcDescripcion + "\r\n";
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    log += "Central no configurada. Configuración: " + agcDescripcion + "\r\n";
                                    continue;
                                }
                            }
                        }
                    }
                }

                //fin de n agrupamientos
                xmlHidro = ObtenerXmlCabecera("UnitScheduledLoad") + xmlHidro + ObtenerXmlPie();
                xmlTermo = ObtenerXmlCabecera("UnitScheduledLoad") + xmlTermo + ObtenerXmlPie();

                XMLDocument.GenerarArchivoXML(NombreArchivo.ReporteXMLDespachoHidro, xmlHidro);
                XMLDocument.GenerarArchivoXML(NombreArchivo.ReporteXMLDespachoTermo, xmlTermo);

                //--- FIN

                result = log;
            }
            catch (Exception ex)
            {
                result = "-1";
            }

            return Json(result);
        }

        /// <summary>
        /// Permite obtener la cabecera de archivo XML
        /// </summary>
        /// <param name="scheduledType">Tipo de Programa</param>
        /// <returns></returns>
        private string ObtenerXmlCabecera(string scheduledType)
        {
            DateTime fecha = System.DateTime.Now;
            fecha = fecha.ToUniversalTime();

            string cad = "<ns0:Data xmlns:ns0=\"http://www.siemens.com/cop/SegmentTypeSchedule.xsd\">\r\n";
            cad += "<ns0:DataAnalog>\r\n";
            cad += "<ns0:ScheduleType>" + scheduledType + "</ns0:ScheduleType>\r\n";
            cad += "<ns0:Msg_Time_Stamp>" + fecha.ToString("yyyy-MM-dd") + "T" +
                   fecha.ToString(Constantes.FormatoHora) + "Z</ns0:Msg_Time_Stamp>\r\n";
            cad += "<ns0:Msg_Identifier>" + scheduledType + "</ns0:Msg_Identifier>\r\n";
            cad += "<ns0:Msg_Version>1</ns0:Msg_Version>\r\n";

            return cad;
        }

        /// <summary>
        /// Permite obtener el pie de archivo XML
        /// </summary>
        /// <returns></returns>
        private string ObtenerXmlPie()
        {
            string cad = "</ns0:DataAnalog>\r\n";
            cad += "</ns0:Data>\r\n";
            return cad;
        }


        /// <summary>
        /// Permite asignar y analizar los mantenimientos y operaciones varias
        /// </summary>
        /// <param name="objDatos">objeto datos</param>
        /// <param name="equicodi2">código de equipo</param>
        /// <param name="evenClase">Clase</param>
        /// <param name="propcodiPEfectiva">Código de potencia Efectiva</param>
        /// <param name="propcodiPMinima">Código de Potencia mínima</param>
        /// <param name="esCentral">indicador si es central</param>
        /// <param name="lectcodi">Código de lectura</param>
        /// <param name="numLogAlerta">Contador de alertas</param>
        /// <param name="descripcionAGC">Descripción AGC</param>
        /// <param name="log">Log</param>
        /// <param name="fecha">Fecha de consulta</param>
        private void AsignaMWyAnalizaManttoyOperacionesVarias(ref PotenciaMW objDatos, string equicodi2, int evenClase, int propcodiPEfectiva, int propcodiPMinima, bool esCentral, int lectcodi, ref int numLogAlerta, string descripcionAGC, ref string log, string fecha)
        {
            //164: P. Efectiva
            //299: P. Minima

            //string ls_sql = "";
            double valor = 0;
            string valorPropiedad = "";

            fecha += " ";

            string fechaDMY = fecha.Substring(0, fecha.IndexOf(" "));

            if (!esCentral)
            {   //Es equipo

                //164: Potencia efectiva

                valorPropiedad = servEquipamiento.ObtenerValorPropiedadEquipoFecha(Convert.ToInt32(propcodiPEfectiva), Convert.ToInt32(equicodi2), fechaDMY);

                valor = (valorPropiedad == "" ? 0 : Convert.ToDouble(valorPropiedad));

                if (valor <= 0)
                {
                    log += "Error de Potencia Efectiva en Equipo (valor 0). Configuración: " + descripcionAGC + "\r\n";
                    objDatos.PotenciaEfectiva = 0;
                }
                else
                {
                    objDatos.PotenciaEfectiva = valor;
                }

                //ASIGNACIÓN DE POTENCIA efectiva
                for (int i = 0; i < 48; i++)
                {
                    objDatos.MW[i] = objDatos.PotenciaEfectiva;
                }
            }
            else
            {
                //Es central
                //164: Potencia efectiva

                valorPropiedad = servEquipamiento.ObtenerValorPropiedadEquipoFecha(Convert.ToInt32(propcodiPEfectiva), Convert.ToInt32(equicodi2), fechaDMY);

                valor = (valorPropiedad == "" ? 0 : Convert.ToDouble(valorPropiedad));


                if (valor <= 0)
                {
                    log += "Error de Potencia Efectiva en Central (valor 0). Configuración: " + descripcionAGC + "\r\n";
                    objDatos.PotenciaEfectiva = 0;
                }
                else
                {
                    objDatos.PotenciaEfectiva = valor;
                }

                //despacho
                List<MeMedicion48DTO> meMedicion48 = servMedidores.ObtenerDatosEquipoLectura(equicodi2, lectcodi, fechaDMY);

                if (meMedicion48.Count > 0)
                {
                    foreach (MeMedicion48DTO itemMeMed48 in meMedicion48)
                    {

                        for (int i = 0; i < 48; i++)
                        {
                            //itemMeMed48.Hi 
                            objDatos.MW[i] = Convert.ToDouble(itemMeMed48.GetType().GetProperty("H" + (i + 1)).GetValue(itemMeMed48, null));
                        }
                    }
                }
                else
                {
                    log += "Error de Potencia 30 minutos de Central (valor 0). Se debe grabar datos por central. Configuración: " + descripcionAGC + "\r\n";

                    for (int i = 0; i < 48; i++)
                    {
                        objDatos.MW[i] = 0;
                    }
                }
            }

            //potencia mínima
            valorPropiedad = servEquipamiento.ObtenerValorPropiedadEquipoFecha(Convert.ToInt32(propcodiPMinima), Convert.ToInt32(equicodi2), fechaDMY);
            valor = (valorPropiedad == "" ? 0 : Convert.ToDouble(valorPropiedad));

            objDatos.PotenciaMinima = valor;
            string fechaini = fechaDMY;
            string fechafin = DateTime.ParseExact(fechaDMY, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddDays(1).ToString(Constantes.FormatoFecha);

            //mantenimiento
            ArrayList parametro = new ArrayList();
            parametro.Add(fechaini);
            parametro.Add(fechafin);
            parametro.Add(evenClase);
            parametro.Add(equicodi2);

            AsignaMW(ref objDatos, ref numLogAlerta, descripcionAGC, "Mantenimiento", ref log, "MANTO", parametro);

            //restricciones operativas: Parcial:            icvalor1 con valor<>0
            //                          Indisp. Total:      iccheck2='S'
            parametro = new ArrayList();
            parametro.Add(fechaini);
            parametro.Add(fechafin);
            parametro.Add(ConstantesTiempoReal.RestriccionesOperativas);//205
            parametro.Add(evenClase);
            parametro.Add(equicodi2);

            AsignaMW(ref objDatos, ref  numLogAlerta, descripcionAGC, "Restricciones Operativas", ref log, "RESTRIX", parametro);
        }


        /// <summary>
        /// Permite asignar MW de acuerdo a mantenimiento y restricciones operativas
        /// </summary>
        /// <param name="objDatos">Objeto Datos</param>
        /// <param name="numLogAlerta">Número de alerta</param>
        /// <param name="descripcionAGC">Descripción AGC</param>
        /// <param name="operacion">Operación a realizar (mantenimiento / restricciones operativas)</param>
        /// <param name="log">Log</param>
        /// <param name="opcionRegistro">Parámetro de Opcion a usar</param>
        /// <param name="parametro">Parámetros de consulta a realizar</param>
        private void AsignaMW(ref PotenciaMW objDatos, ref int numLogAlerta, string descripcionAGC, string operacion, ref string log, string opcionRegistro, ArrayList parametro)
        {
            List<EveManttoDTO> listaManto;

            if (opcionRegistro == "MANTO")
            {
                listaManto = servEvento.ObtenerManttoEquipoClaseFecha(parametro[3].ToString(),
                    parametro[0].ToString(),
                    parametro[1].ToString(),
                    Convert.ToInt32(parametro[2]));

            }
            else
            {
                listaManto = servEvento.ObtenerManttoEquipoSubcausaClaseFecha(
                    parametro[4].ToString(),
                    parametro[0].ToString(),
                    parametro[1].ToString(),
                    Convert.ToInt32(parametro[3]),
                    Convert.ToInt32(parametro[2]));
            }

            //EQUIPO
            //equicodi configurado
            foreach (EveManttoDTO itemManto in listaManto)
            {
                string evenindispo = itemManto.Evenindispo;
                DateTime evenFechaini = (DateTime)itemManto.Evenini;
                DateTime evenFechafin = (DateTime)itemManto.Evenfin;
                double mwIndispo = (double)itemManto.Evenmwindisp;

                //mantenimiento de equipo E/S: MW indisponible. MW= P. Efectiva (equipamiento) - MW indisponible.
                if (evenindispo == ConstantesTiempoReal.MantenimientoEnServicio)//"E"
                {
                    int indiceIni = 0; int indiceFin = 0;
                    ConfiguraHoraInicialFinal(ref indiceIni, ref  indiceFin, evenFechaini, evenFechafin);

                    double valorEnservicio = objDatos.PotenciaEfectiva - mwIndispo;
                    if (valorEnservicio < 0)
                    {
                        log += "Error en " + operacion + " Potencia Efectiva < MW disponible Mínima(Asignado 0). Configuración: " + descripcionAGC + "\r\n";
                        valorEnservicio = 0;
                    }

                    for (int j = indiceIni; j <= indiceFin; j++)
                    {
                        objDatos.MW[j] = Math.Min(objDatos.MW[j], valorEnservicio);

                        if (objDatos.MW[j] < 0)
                            objDatos.MW[j] = 0;
                    }

                }
                else
                {
                    //mantenimiento de equipo F/S:                  MW=0
                    if (evenindispo == ConstantesTiempoReal.MantenimientoFueraServicio)// "F"
                    {
                        int indiceIni = 0; int indiceFin = 0;
                        ConfiguraHoraInicialFinal(ref indiceIni, ref  indiceFin, evenFechaini, evenFechafin);

                        for (int j = indiceIni; j <= indiceFin; j++)
                        {
                            objDatos.MW[j] = 0;
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Permite configurar hora inicial y final de acuerdo a indices de referencia
        /// </summary>
        /// <param name="indiceIni">Ïndice inicial</param>
        /// <param name="indiceFin">Ïndice final</param>
        /// <param name="hOpHorIni">Hora de operación inicial</param>
        /// <param name="hOpHorFin">Hora de operación final</param>
        private void ConfiguraHoraInicialFinal(ref int indiceIni, ref int indiceFin, DateTime hOpHorIni, DateTime hOpHorFin)
        {

            indiceIni = 0;
            indiceFin = 0;

            indiceIni = 2 * hOpHorIni.Hour + hOpHorIni.Minute / 30;

            if (hOpHorIni.Day != hOpHorFin.Day)
                indiceFin = 47;
            else
            {
                indiceFin = 2 * hOpHorFin.Hour + hOpHorFin.Minute / 30 - 1;
            }


            if (indiceFin < indiceIni)
                indiceFin = indiceIni;

            if (indiceIni > 0)
                indiceIni--;

            if ((indiceFin > 0) && indiceFin != 47)
                indiceFin--;
        }

        /// <summary>
        /// Permite obtener el XML por central y equipo
        /// </summary>
        /// <param name="htEquipoHora"></param>
        /// <param name="bloqueIni"></param>
        /// <param name="fechaBase"></param>
        /// <returns></returns>
        private string ObtenerXmlPorCentralEquipo(Hashtable htEquipoHora, int bloqueIni, string fechaBase)
        {
            string xml = "";
            DateTime fecha;
            string datos = "";

            foreach (int equicodi2 in htEquipoHora.Keys)
            {
                PotenciaMW objDatos = (PotenciaMW)htEquipoHora[equicodi2];

                //imprimir valores de central as EQUIPO 1..48
                xml += "<ns0:Object>\r\n";
                xml += "<ns0:B1>Network/.Generation</ns0:B1>\r\n";
                xml += "<ns0:B2>" + objDatos.B2 + "</ns0:B2>\r\n";
                xml += "<ns0:B3>" + objDatos.B3 + "</ns0:B3>\r\n";
                xml += "<ns0:Element/>\r\n";

                datos += objDatos.B3 + ",";
                fecha = DateTime.ParseExact(fechaBase, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                fecha = fecha.AddMinutes(-30).ToUniversalTime();

                for (int i = bloqueIni; i <= 48; i++)
                {
                    fecha = fecha.AddMinutes(30);
                    xml += "<ns0:DataInfo>\r\n";
                    xml += "<ns0:Start_Time>" + fecha.ToString("yyyy-MM-dd") + "T" +
                           fecha.ToString(Constantes.FormatoHora) + "Z</ns0:Start_Time>\r\n";

                    if (objDatos.PAsignada[i - 1] > 0)
                    {
                        xml += "<ns0:Value>" + objDatos.PAsignada[i - 1].ToString() + "</ns0:Value>\r\n";
                        datos += objDatos.PAsignada[i - 1].ToString() + ",";
                    }
                    else
                    {
                        xml += "<ns0:Value>" + 0 + "</ns0:Value>\r\n";
                        datos += "0,";
                    }

                    xml += "</ns0:DataInfo>\r\n";

                }

                xml += "</ns0:Object>\r\n";
                datos += "\r\n";
            }

            return xml;
        }

        /// <summary>
        /// Permite obtener el XML por tipo de equipo Hidro / Termoeléctrico
        /// </summary>
        /// <param name="lectCodi">Código de lectura</param>
        /// <param name="bloqueIni">bloque inicial</param>
        /// <param name="xmlHidro">XML hidro</param>
        /// <param name="xmlTermo">XML termo</param>
        /// <param name="codigo">Código de equipo</param>
        /// <param name="b2">Elemento B2 Siemens</param>
        /// <param name="b3">Elemento B3 Siemens</param>
        /// <param name="log">Log</param>
        /// <param name="fecha">Fecha de consulta</param>
        /// <returns></returns>
        private bool ObtenerXmlPorTipo(int lectCodi, int bloqueIni, ref string xmlHidro, ref string xmlTermo,
            string codigo, string b2, string b3, string log, DateTime fecha)
        {
            string fechaDMY = fecha.ToString(Constantes.FormatoFecha);
            string xml;
            string tipoGrupo = servControl.ListarTipoGrupo(codigo);

            ArrayList arrValores = new ArrayList();
            string tipo = "H";

            if (tipoGrupo != "")
                tipo = tipoGrupo;

            if (tipo != "H" && tipo != "T")
            {
                log += "Tipo de Equipo(H,T) no identificado\r\n";
                return false;
            }

            List<MeMedicion48DTO> meMedicion48 = servMedidores.ObtenerDatosPtoMedicionLectura(codigo, lectCodi, fechaDMY);

            foreach (MeMedicion48DTO itemMedicion in meMedicion48)
            {
                for (int i = bloqueIni; i <= 48; i++)
                {
                    arrValores.Add(
                        Convert.ToString(
                            Convert.ToDouble(itemMedicion.GetType().GetProperty("H" + i).GetValue(itemMedicion, null))));
                }
            }

            if (meMedicion48.Count == 0)
            {
                for (int i = bloqueIni; i <= 48; i++)
                {
                    arrValores.Add(0);
                }
            }

            xml = "<ns0:Object>\r\n";
            xml += "<ns0:B1>Network/.Generation</ns0:B1>\r\n";
            xml += "<ns0:B2>" + b2 + "</ns0:B2>\r\n";
            xml += "<ns0:B3>" + b3 + "</ns0:B3>\r\n";
            xml += "<ns0:Element/>\r\n";

            int idx = 0;

            fecha = fecha.AddMinutes(-30).ToUniversalTime();

            for (int i = bloqueIni; i <= 48; i++)
            {
                fecha = fecha.AddMinutes(30);
                xml += "<ns0:DataInfo>\r\n";
                xml += "<ns0:Start_Time>" + fecha.ToString("yyyy-MM-dd") + "T" +
                       fecha.ToString(Constantes.FormatoHora) + "Z</ns0:Start_Time>\r\n";
                xml += "<ns0:Value>" + arrValores[idx].ToString() + "</ns0:Value>\r\n";
                xml += "</ns0:DataInfo>\r\n";
                idx++;
            }

            xml += "</ns0:Object>\r\n";

            if (tipo == "H")
                xmlHidro += xml;
            else
                xmlTermo += xml;

            return true;
        }

        /// <summary>
        /// Permite exportar el xml generado del Despacho Hidro
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarXmlDespachoHidro()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeTiempoReal +
                          NombreArchivo.ReporteXMLDespachoHidro;
            return File(file, Constantes.AppXML, NombreArchivo.ReporteXMLDespachoHidro);
        }

        /// <summary>
        /// Permite exportar el xml generado del despacho termo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarXmlDespachoTermo()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeTiempoReal +
                          NombreArchivo.ReporteXMLDespachoTermo;
            return File(file, Constantes.AppXML, NombreArchivo.ReporteXMLDespachoTermo);
        }

        /// <summary>
        /// Permite exportar el xml generado hidro del Valor Agua
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarXmlValorAgua()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeTiempoReal +
                          NombreArchivo.ReporteXMLValorAgua;
            return File(file, Constantes.AppXML, NombreArchivo.ReporteXMLValorAgua);
        }

        /// <summary>
        /// Permite exportar el xml generado de la Potencia Activa Hidro
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarXmlPActivaHidro()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeTiempoReal +
                          NombreArchivo.ReporteXMLPActivaHidro;
            return File(file, Constantes.AppXML, NombreArchivo.ReporteXMLPActivaHidro);
        }

        /// <summary>
        /// Permite exportar el xml generado de la Potencia Activa Hidro
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarXmlPActivaTermo()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeTiempoReal +
                          NombreArchivo.ReporteXMLPActivaTermo;
            return File(file, Constantes.AppXML, NombreArchivo.ReporteXMLPActivaTermo);
        }

        /// <summary>
        /// Permite exportar el xml generado de los costos variables (CCOMB)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarXmlCVCComb()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeTiempoReal +
                          NombreArchivo.ReporteXMLCCOMB;
            return File(file, Constantes.AppXML, NombreArchivo.ReporteXMLCCOMB);
        }

        /// <summary>
        /// Permite exportar el xml generado de los costos variables (CVC)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarXmlCVCVC()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeTiempoReal +
                          NombreArchivo.ReporteXMLCVC;
            return File(file, Constantes.AppXML, NombreArchivo.ReporteXMLCVC);
        }

        /// <summary>
        /// Permite exportar el xml generado de los costos variables (CVNC)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarXmlCVCVNC()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeTiempoReal +
                          NombreArchivo.ReporteXMLCVNC;
            return File(file, Constantes.AppXML, NombreArchivo.ReporteXMLCVNC);
        }

        /// <summary>
        /// Permite exportar el xml generado de los mantenimientos de hidro
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarXmlMantoHydro()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeTiempoReal +
                          NombreArchivo.ReporteXMLMantoHydro;
            return File(file, Constantes.AppXML, NombreArchivo.ReporteXMLMantoHydro);
        }

        /// <summary>
        /// Permite exportar el xml generado de los mantenimientos de Thermo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarXmlMantoThermo()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeTiempoReal +
                          NombreArchivo.ReporteXMLMantoThermo;
            return File(file, Constantes.AppXML, NombreArchivo.ReporteXMLMantoThermo);
        }

        /// <summary>
        /// Permite exportar el xml generado de los mantenimientos de Banco
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarXmlMantoBanco()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeTiempoReal +
                          NombreArchivo.ReporteXMLMantoBanco;
            return File(file, Constantes.AppXML, NombreArchivo.ReporteXMLMantoBanco);
        }

        /// <summary>
        /// Permite exportar el xml generado de los mantenimientos de Reactor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarXmlMantoReactor()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeTiempoReal +
                          NombreArchivo.ReporteXMLMantoReactor;
            return File(file, Constantes.AppXML, NombreArchivo.ReporteXMLMantoReactor);
        }

        /// <summary>
        /// Permite exportar el xml generado de los mantenimientos de Compensador síncrono
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarXmlMantoCS()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeTiempoReal +
                          NombreArchivo.ReporteXMLMantoCS;
            return File(file, Constantes.AppXML, NombreArchivo.ReporteXMLMantoCS);
        }

        /// <summary>
        /// Permite exportar el xml generado de los mantenimientos de SVC
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarXmlMantoSVC()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeTiempoReal +
                          NombreArchivo.ReporteXMLMantoSVC;
            return File(file, Constantes.AppXML, NombreArchivo.ReporteXMLMantoSVC);
        }

        [HttpPost]
        public ActionResult Upload(string nombreArchivo)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + nombreArchivo;
                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult CargarMatriz(string file, string idTabla, string idOpcionDatos, string fecha, bool esHidro)
        {
            string result = "";
            try
            {
                //abrir el archivo

                List<String> files = new List<string>();
                string tabla = "<table id=\"" + idTabla +
                               "\" border=\"0\" class=\"pretty tabla-adicional\" cellspacing=\"0\">";

                if (file != "" && file != null)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;
                    files = new List<string> { path + file };

                    foreach (string fileMovim in files)
                    {
                        string archivo = fileMovim;


                        if (new FileInfo(archivo).Exists)
                        {

                            ArrayList values = new ArrayList();
                            int lineaInicio = 3;
                            int lineaEjecucion = -1;
                            string[] cabecera = new string[1];
                            string[] contenido1;
                            int lineaContenido = -1;


                            bool esPrimerRegistroCabecera = true;
                            bool esPrimerRegistroDetalle = true;

                            using (StreamReader csvReader = new StreamReader(archivo, Encoding.Default))
                            {

                                string contenidoLinea = "";

                                while ((contenidoLinea = csvReader.ReadLine()) != null)
                                {
                                    lineaEjecucion++;
                                    if (lineaEjecucion < lineaInicio) continue;

                                    if (lineaEjecucion == 3)
                                    {
                                        if (esPrimerRegistroCabecera)
                                        {
                                            tabla += "<thead>";
                                            esPrimerRegistroCabecera = false;
                                        }

                                        cabecera = contenidoLinea.Split(new string[] { "," }, StringSplitOptions.None);
                                        for (int j = 0; j < cabecera.Length; j++)
                                        {
                                            cabecera[j] = cabecera[j].Trim();
                                        }

                                        //creación de cabeceras
                                        tabla += CargaCabeceraArchivo(cabecera, "th", false);

                                    }
                                    else
                                    {
                                        if (esPrimerRegistroDetalle)
                                        {
                                            tabla += "</thead>";
                                            tabla += "<tbody>";
                                            esPrimerRegistroDetalle = false;
                                        }

                                        contenido1 = contenidoLinea.Split(new string[] { "," }, StringSplitOptions.None);

                                        if (cabecera.Length == contenido1.Length)
                                        {
                                            tabla += CargaCabeceraArchivo(contenido1, "td", true);
                                            values.Add(contenido1);
                                        }

                                        lineaContenido++;
                                    }
                                }

                                tabla += "</tbody>";
                                tabla += "</table>";

                                csvReader.Close();
                            }

                            //pasando los datos a matriz

                            ArrayList listaTemporal = new ArrayList();
                            listaTemporal.AddRange(cabecera);

                            listaTemporal.RemoveAt(0); //"Estg"
                            listaTemporal.RemoveAt(0); //"Ser."
                            listaTemporal.RemoveAt(0); //"Pat."

                            cabecera = (string[])listaTemporal.ToArray(typeof(string));

                            double[,] matrizDatos = new double[values.Count, cabecera.Length];

                            int fila = 0;
                            foreach (string[] contenidoM in values)
                            {
                                for (int j = 0; j < cabecera.Length; j++)
                                {
                                    matrizDatos[fila, j] = Convert.ToDouble(contenidoM[j + 3]);
                                }
                                fila++;
                            }

                            //generando el XML a exportar
                            switch (idOpcionDatos)
                            {
                                case "Vagua":
                                    List<MePtomedicionDTO> listaVAgua = servControl.ListarPotencia("2,4", 20,
                                        "#ValoraguaConfiguracion");
                                    string xml = CrearContenidoXML("Network/.Generation", 0.001, listaVAgua, cabecera,
                                        matrizDatos, fecha);

                                    //crea el archivo xml de Valor Agua
                                    XMLDocument.GenerarArchivoXML(NombreArchivo.ReporteXMLValorAgua, xml);

                                    break;
                                case "PActiva":
                                    if (esHidro)
                                    {
                                        List<MePtomedicionDTO> listaPActivaHidro = servControl.ListarPotencia("2,4", 20,
                                            "#PotenciaactivabaseConfiguracionTabHidro");
                                        string xmlPAH = CrearContenidoXML("Network/.System Fuels", 1, listaPActivaHidro,
                                            cabecera, matrizDatos, fecha);

                                        //crea el archivo xml de Potencia Activa Hidro
                                        XMLDocument.GenerarArchivoXML(NombreArchivo.ReporteXMLPActivaHidro, xmlPAH);

                                    }
                                    else
                                    {
                                        List<MePtomedicionDTO> listaPActivaTermo = servControl.ListarPotencia("3,5", 20,
                                            "#PotenciaactivabaseConfiguracionTabTermo");
                                        string xmlPAT = CrearContenidoXML("Network/.System Fuels", 1, listaPActivaTermo,
                                            cabecera, matrizDatos, fecha);

                                        //crea el archivo xml de Potencia Activa Termo
                                        XMLDocument.GenerarArchivoXML(NombreArchivo.ReporteXMLPActivaTermo, xmlPAT);

                                    }

                                    break;
                            }

                            return Json(tabla);
                        }
                        else
                        {
                            return Json("-1");
                        }
                    }
                }

                //preparado el html
                return Json(result);
            }
            catch
            {
                return Json("-1");
            }
        }


        /// <summary>
        /// Permite crear el contenido XML
        /// </summary>
        /// <param name="xmlB1">Tag B1 (Siemens)</param>
        /// <param name="factor">Factor a multiplicar el resultado</param>
        /// <param name="listaPuntos">Lista de puntos de medición</param>
        /// <param name="cabecera">Cabecera</param>
        /// <param name="datos">Matriz de datos</param>
        /// <param name="fecha1">Fecha de consulta</param>
        /// <returns></returns>
        private string CrearContenidoXML(
            string xmlB1, double factor, List<MePtomedicionDTO> listaPuntos, string[] cabecera, double[,] datos,
            string fecha1)
        {
            string contenido = "";

            try
            {

                string horaIni = "00:30";

                if (datos.GetLength(0) < 48)
                {
                    int filaHora = 48 - datos.GetLength(0) + 1;
                    horaIni = (filaHora / 2).ToString("00") + ":" +
                              (30 * (filaHora - 2 * (filaHora / 2))).ToString("00");
                }

                DateTime fecha = DateTime.ParseExact(fecha1 + " " + horaIni, Constantes.FormatoFechaHora,
                    CultureInfo.InvariantCulture);


                //leyendo datos
                for (int j = 0; j < cabecera.Length; j++)
                {
                    string nombreColumna = cabecera[j];

                    MePtomedicionDTO datoVAgua;
                    try
                    {
                        datoVAgua = listaPuntos.Where(x => x.Ptomedielenomb == nombreColumna).First();
                    }
                    catch
                    {
                        datoVAgua = null;
                    }

                    if (datoVAgua == null)
                    {
                        if (nombreColumna == "CañonPato")
                        {
                            datoVAgua = listaPuntos.Where(x => x.Ptomedielenomb == "Ca¿onPato").First();
                        }
                    }

                    if (datoVAgua != null)
                    {
                        contenido += "<ns0:Object>\r\n";
                        contenido += "<ns0:B1>" + xmlB1 + "</ns0:B1>\r\n";
                        contenido += "<ns0:B2>" + datoVAgua.Ptomedibarranomb + "</ns0:B2>\r\n";
                        contenido += "<ns0:B3>" + datoVAgua.Ptomedidesc + "</ns0:B3>\r\n";
                        contenido += "<ns0:Element/>\r\n";
                    }
                    else
                    {
                        contenido += "<ns0:Object>\r\n";
                        contenido += "<ns0:B1>" + xmlB1 + "</ns0:B1>\r\n";
                        contenido += "<ns0:B2>" + "NOENCONTRADO" + "</ns0:B2>\r\n";
                        contenido += "<ns0:B3>" + "NOENCONTRADO" + "</ns0:B3>\r\n";
                        contenido += "<ns0:Element/>\r\n";
                    }

                    DateTime fechaEquipo = fecha.AddMinutes(-30);
                    fechaEquipo = fechaEquipo.ToUniversalTime();

                    for (int filaEquipo = 0; filaEquipo < datos.GetLength(0); filaEquipo++)
                    {
                        contenido += "<ns0:DataInfo>";
                        fechaEquipo = fechaEquipo.AddMinutes(30);
                        contenido += "<ns0:Start_Time>" + fechaEquipo.ToString("yyyy-MM-dd") + "T" +
                                     fechaEquipo.ToString(Constantes.FormatoHora) + "Z</ns0:Start_Time>\r\n";
                        contenido += "<ns0:Value>" + datos[filaEquipo, j] * factor + "</ns0:Value>\r\n";
                        contenido += "</ns0:DataInfo>\r\n";
                    }

                    contenido += "</ns0:Object>\r\n";
                }

                contenido = ObtenerXmlCabecera("WaterWothValue") + contenido + ObtenerXmlPie();
                return contenido;

            }
            catch
            {

                return contenido; // "";
            }
        }

        


        /// <summary>
        /// Permite cargar la cabecera en formato xml
        /// </summary>
        /// <param name="cabecera">arreglo de cabecera</param>
        /// <param name="tag">etiqueta a considerar</param>
        /// <param name="esNumerico">el dato es numérico</param>
        /// <returns></returns>
        private string CargaCabeceraArchivo(string[] cabecera, string tag, bool esNumerico)
        {
            string cadRegistro = "";
            int columna = -1;

            cadRegistro += "<tr>";
            try
            {
                foreach (string cabeceraDetalle in cabecera)
                {

                    columna++;
                    if (columna < 3) continue;

                    cadRegistro += "<" + tag + ">";
                    cadRegistro += (esNumerico
                                       ? Convert.ToDouble(cabeceraDetalle.Trim()).ToString()
                                       : cabeceraDetalle.Trim()) + "</" + tag + ">";
                }
            }
            catch
            {
                return "";
            }

            cadRegistro += "</tr>";
            return cadRegistro;
        }


        /// <summary>
        /// Permite crear un xml de costos variables
        /// </summary>
        /// <param name="repCodi">Código de reporte de costos variables</param>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CrearXmlCostoVariable(int repCodi, string fecha)
        {
            try
            {

                string log = "";

                List<PrCvariablesDTO> listaCVar = servDespacho.ListPrCvariabless(repCodi);
                List<MePtomedicionDTO> listaPMedicion = servControl.ListarCostoVariableAGC();

                ArrayList arrArchivoCampo = new ArrayList();
                arrArchivoCampo.Add(ConstantesTiempoReal.DespachoCComb);
                arrArchivoCampo.Add(ConstantesTiempoReal.DespachoCVariableCombustible);
                arrArchivoCampo.Add(ConstantesTiempoReal.DespachoCVariableNoCombustible);

                //CCOMB CVC CVNC
                string[] nombreReporte = new string[3];

                nombreReporte[0] = NombreArchivo.ReporteXMLCCOMB;
                nombreReporte[1] = NombreArchivo.ReporteXMLCVC;
                nombreReporte[2] = NombreArchivo.ReporteXMLCVNC;


                string[] nombrePrograma = new string[3];

                nombrePrograma[0] = NombreArchivo.ProgramaCCOMB;
                nombrePrograma[1] = NombreArchivo.ProgramaCVC;
                nombrePrograma[2] = NombreArchivo.ProgramaCVNC;

                int indice = 0;
                ArrayList arrLog = new ArrayList();
                foreach (string campo in arrArchivoCampo)
                {
                    string contenido = CrearContenidoCostoVariableXML(indice, campo,
                        ConstantesTiempoReal.TagB1CostoVariable, fecha, listaCVar, listaPMedicion, arrLog);
                    contenido = ObtenerXmlCabecera(nombrePrograma[indice]) + contenido + ObtenerXmlPie();

                    //crea el archivo xml
                    XMLDocument.GenerarArchivoXML(nombreReporte[indice], contenido);
                    indice++;

                }


                if (arrLog.Count == 0)
                {
                    return Json("");
                }
                else
                {
                    log = "Códigos de grupo NO configurados: " + string.Join(",", arrLog.ToArray());
                    return Json(log);
                }
            }
            catch
            {
                return Json("-1");
            }

        }

        /// <summary>
        /// Permite crear el contenido xml de costos variables
        /// </summary>
        /// <param name="indice">Índice de columna a usar</param>
        /// <param name="campo">Campo</param>
        /// <param name="xmlB1">etiqeta B1 (Siemens)</param>
        /// <param name="fechaBase">Fecha base a considerar</param>
        /// <param name="listaCVar">Lista de datos de costos variables</param>
        /// <param name="listaPMedicion">Lista de Puntos de medición</param>
        /// <param name="arrLog">Log</param>
        /// <returns></returns>
        private string CrearContenidoCostoVariableXML(int indice, string campo, string xmlB1, string fechaBase,
            List<PrCvariablesDTO> listaCVar, List<MePtomedicionDTO> listaPMedicion, ArrayList arrLog)
        {
            StringBuilder strContenido = new StringBuilder();

            try
            {
                DateTime fecha = DateTime.ParseExact(fechaBase, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                fecha = fecha.AddMinutes(30);

                fecha = fecha.ToUniversalTime();

                foreach (PrCvariablesDTO itemCVar in listaCVar)
                {
                    //--- ini

                    MePtomedicionDTO datoPtomedicion;
                    try
                    {
                        datoPtomedicion = listaPMedicion.Where(x => x.Grupocodi == itemCVar.Grupocodi).First();
                    }
                    catch
                    {
                        datoPtomedicion = null;
                    }

                    if (datoPtomedicion != null)
                    {
                        strContenido.Append("<ns0:Object>\r\n");
                        strContenido.Append("<ns0:B1>" + xmlB1 + "</ns0:B1>\r\n");
                        strContenido.Append("<ns0:B2>" + datoPtomedicion.Ptomedibarranomb + "</ns0:B2>\r\n");
                        //contenido += "<ns0:B2>" + dgv_ValorAguaConfig[B2, fila].Value + "</ns0:B2>\r\n";
                        strContenido.Append("<ns0:B3>" + datoPtomedicion.Ptomedidesc + "</ns0:B3>\r\n");
                        //contenido += "<ns0:B3>" + dgv_ValorAguaConfig[B3, fila].Value + "</ns0:B3>\r\n";
                        strContenido.Append("<ns0:Element/>\r\n");
                    }
                    else
                    {
                        if (arrLog.IndexOf(itemCVar.Grupocodi) < 0)
                            arrLog.Add(itemCVar.Grupocodi);
                        continue;
                    }


                    DateTime fechaEquipo = fecha.AddMinutes(-30);

                    double valor = Convert.ToDouble(itemCVar.GetType().GetProperty(campo).GetValue(itemCVar, null));

                    for (int i = 0; i < 48; i++)
                    {
                        strContenido.Append("<ns0:DataInfo>");

                        fechaEquipo = fechaEquipo.AddMinutes(30);
                        strContenido.Append("<ns0:Start_Time>" + fechaEquipo.ToString("yyyy-MM-dd") +
                                            "T" + fechaEquipo.ToString(Constantes.FormatoHora) +
                                            "Z</ns0:Start_Time>\r\n");

                        strContenido.Append("<ns0:Value>" + valor + "</ns0:Value>\r\n");
                        strContenido.Append("</ns0:DataInfo>\r\n");
                    }

                    strContenido.Append("</ns0:Object>\r\n");
                }

                return strContenido.ToString();
            }
            catch (Exception e1)
            {
                return strContenido.ToString();
            }
        }

        /// <summary>
        /// Permite crear el xml de mantenimiento
        /// </summary>
        /// <param name="evenClaseCodi">Código de clase</param>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CrearXmlMantenimiento(int evenClaseCodi, string fecha)
        {
            try
            {

                string log = "";
                List<EqEquipoDTO> listaEquipo = servEquipamiento.ListarEquiposPropiedadesAGC(fecha);
                List<MePtomedicionDTO> listaPMedicion = servControl.ListarCostoVariableAGC();

                Hashtable htEquipo = new Hashtable();
                Hashtable htArchivo = new Hashtable();

                DateTime fechaBase = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                htEquipo.Add(ConstantesTiempoReal.FamiliaHydro, "Hydro");
                htEquipo.Add(ConstantesTiempoReal.FamiliaThermo, "Thermo");
                htEquipo.Add(ConstantesTiempoReal.FamiliaBanco, "Banco");
                htEquipo.Add(ConstantesTiempoReal.FamiliaReactor, "Reactor");
                htEquipo.Add(ConstantesTiempoReal.FamiliaCS, "CS");
                htEquipo.Add(ConstantesTiempoReal.FamiliaSVC, "SVC");

                htArchivo.Add(ConstantesTiempoReal.FamiliaHydro, NombreArchivo.ReporteXMLMantoHydro);
                htArchivo.Add(ConstantesTiempoReal.FamiliaThermo, NombreArchivo.ReporteXMLMantoThermo);
                htArchivo.Add(ConstantesTiempoReal.FamiliaBanco, NombreArchivo.ReporteXMLMantoBanco);
                htArchivo.Add(ConstantesTiempoReal.FamiliaReactor, NombreArchivo.ReporteXMLMantoReactor);
                htArchivo.Add(ConstantesTiempoReal.FamiliaCS, NombreArchivo.ReporteXMLMantoCS);
                htArchivo.Add(ConstantesTiempoReal.FamiliaSVC, NombreArchivo.ReporteXMLMantoSVC);

                ArrayList arrLog = new ArrayList();
                foreach (int famcodi in htEquipo.Keys)
                {
                    string contenido = MantenimientosPorFamilia(famcodi.ToString(),
                        ConstantesTiempoReal.TagB1Mantenimiento,
                        listaEquipo.Where(x => (int)(x.Famcodi) == famcodi).ToList(), fechaBase, evenClaseCodi);

                    contenido = ObtenerXmlCabecera("EquipmentOutageStatus") + contenido + ObtenerXmlPie();

                    //crea el archivo xml de mantenimiento
                    XMLDocument.GenerarArchivoXML(htArchivo[famcodi].ToString(), contenido);
                }

                if (arrLog.Count == 0)
                {
                    return Json("");
                }
                else
                {
                    log = "Códigos de grupo NO configurados: " + string.Join(",", arrLog.ToArray());
                    return Json(log);
                }
            }
            catch
            {
                return Json("-1");
            }

        }
        

        /// <summary>
        /// Permite registrar los mantenimientos por familia de equipo considerando sus relaciones entre equipos (ej: central - grupo)
        /// </summary>
        /// <param name="famcodi">Código de familia</param>
        /// <param name="xmlB1">etiqueta B1 (Siemens)</param>
        /// <param name="listaEquipo">Lista de equipo</param>
        /// <param name="fechaBase">Fecha Base</param>
        /// <param name="evenClaseCodi">Código de clase</param>
        /// <returns></returns>
        private string MantenimientosPorFamilia(string famcodi, string xmlB1, List<EqEquipoDTO> listaEquipo,
            DateTime fechaBase, int evenClaseCodi)
        {
            ArrayList arrEquicodi = new ArrayList();
            string strEquicodi = "";
            string contenido = "";
            string strEquicodiPadre = "";

            Hashtable htEquipoHora = new Hashtable();

            foreach (EqEquipoDTO itemEquipo in listaEquipo)
            {
                arrEquicodi.Add(itemEquipo.Equicodi.ToString());
                strEquicodi += itemEquipo.Equicodi.ToString() + ",";
                int equipadre = Convert.ToInt32(itemEquipo.Equipadre.ToString());

                if (equipadre != -1)
                {
                    strEquicodiPadre += equipadre + ",";
                }
            }

            if (arrEquicodi.Count <= 0) return "";

            strEquicodi = strEquicodi.Substring(0, strEquicodi.Length - 1);

            string fechaini = fechaBase.ToString(Constantes.FormatoFecha);
            string fechafin = fechaBase.AddDays(1).ToString(Constantes.FormatoFecha);

            //capturando equipos de la familia
            List<EveManttoDTO> listaMantto = servEvento.ObtenerManttoEquipoPadreClaseFecha(strEquicodi, fechaini,
                fechafin, evenClaseCodi);

            //mantenimientos
            foreach (EveManttoDTO itemMantto in listaMantto)
            {
                string equicodi = itemMantto.Equicodi.ToString();
                DateTime ldtHopHorIni = (DateTime)itemMantto.Evenini;
                DateTime ldtHopHorFin = (DateTime)itemMantto.Evenfin;

                if (!htEquipoHora.ContainsKey(equicodi))
                {
                    htEquipoHora.Add(equicodi, new int[48]);
                }

                int[] arrEquipoHora = (int[])htEquipoHora[equicodi];

                int indiceIni = 0;
                int indiceFin = 0;
                ConfiguraHoraInicialFinal(ref indiceIni, ref indiceFin, ldtHopHorIni, ldtHopHorFin);

                for (int liI = indiceIni; liI <= indiceFin; liI++)
                {
                    arrEquipoHora[liI] = 1;
                }
            }

            //mantenimiento de EQUIPADRE
            if (strEquicodiPadre != "")
            {
                strEquicodiPadre = strEquicodiPadre.Substring(0, strEquicodiPadre.Length - 1);
                List<EveManttoDTO> listaManttoPadre = servEvento.ObtenerManttoEquipoPadreClaseFecha(strEquicodiPadre,
                    fechaini, fechafin, evenClaseCodi);

                foreach (EveManttoDTO itemMantoPadre in listaManttoPadre)
                {
                    string equipadre = itemMantoPadre.Equicodi.ToString();
                    DateTime hopHorIni = (DateTime)itemMantoPadre.Evenini;
                    DateTime hopHorFin = (DateTime)itemMantoPadre.Evenfin;

                    int indiceIni = 0;
                    int indiceFin = 0;
                    ConfiguraHoraInicialFinal(ref indiceIni, ref indiceFin, hopHorIni, hopHorFin);

                    //AFECTA A TODOS LOS HIJOS CONFIGURADOS
                    foreach (EqEquipoDTO itemEquipo in listaEquipo)
                    {
                        string equipadre2 = itemEquipo.Equipadre.ToString();

                        if ((equipadre2 != "-1") && (itemEquipo.Equipadre.ToString() == equipadre))
                        {
                            string equicodi = itemEquipo.Equicodi.ToString();

                            if (!htEquipoHora.ContainsKey(equicodi))
                            {
                                htEquipoHora.Add(equicodi, new int[48]);
                            }

                            int[] arrEquipoHora = (int[])htEquipoHora[equicodi];

                            indiceIni = 0;
                            indiceFin = 0;
                            ConfiguraHoraInicialFinal(ref indiceIni, ref indiceFin, hopHorIni, hopHorFin);

                            for (int liI = indiceIni; liI <= indiceFin; liI++)
                            {
                                arrEquipoHora[liI] = 1;
                            }
                        }
                    }
                }
            }

            DateTime fecha = fechaBase;
            fecha = fecha.AddMinutes(30);
            fecha = fecha.ToUniversalTime();

            //mantenimientos XML
            foreach (EqEquipoDTO itemEquipo in listaEquipo)
            {
                string equicodi = itemEquipo.Equicodi.ToString();
                string b2 = itemEquipo.B2;
                string b3 = itemEquipo.B3;

                int[] arrEquipoHora = (int[])htEquipoHora[equicodi];

                contenido += "<ns0:Object>\r\n";
                contenido += "<ns0:B1>" + xmlB1 + "</ns0:B1>\r\n";
                contenido += "<ns0:B2>" + b2 + "</ns0:B2>\r\n";
                contenido += "<ns0:B3>" + b3 + "</ns0:B3>\r\n";
                contenido += "<ns0:Element/>\r\n";

                DateTime fechaEquipo = fecha.AddMinutes(-30);
                string valor = "";

                if (arrEquipoHora != null)
                {
                    for (int i = 0; i < 48; i++)
                    {
                        contenido += "<ns0:DataInfo>";
                        fechaEquipo = fechaEquipo.AddMinutes(30);
                        contenido += "<ns0:Start_Time>" + fechaEquipo.ToString("yyyy-MM-dd") + "T" +
                                     fechaEquipo.ToString(Constantes.FormatoHora) + "Z</ns0:Start_Time>\r\n";

                        if (arrEquipoHora[i] == 1)
                            valor = ConstantesTiempoReal.MantenimientoIndisponible; // "Unavailable";
                        else
                            valor = ConstantesTiempoReal.MantenimientoDisponible; // "Available";

                        contenido += "<ns0:Value>" + valor + "</ns0:Value>\r\n";
                        contenido += "</ns0:DataInfo>\r\n";
                    }
                }
                else
                {
                    for (int i = 0; i < 48; i++)
                    {
                        contenido += "<ns0:DataInfo>";

                        fechaEquipo = fechaEquipo.AddMinutes(30);
                        contenido += "<ns0:Start_Time>" + fechaEquipo.ToString("yyyy-MM-dd") + "T" +
                                     fechaEquipo.ToString(Constantes.FormatoHora) + "Z</ns0:Start_Time>\r\n";

                        valor = "Available";

                        contenido += "<ns0:Value>" + valor + "</ns0:Value>\r\n";
                        contenido += "</ns0:DataInfo>\r\n";
                    }
                }

                contenido += "</ns0:Object>\r\n";
            }

            return contenido;
        }

        [HttpPost]
        public PartialViewResult Paginado(string estado)
        {
            Paginacion model = new Paginacion();
            int nroRegistros = servControl.ObtenerNroFilas(estado);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return base.Paginado(model);
        }
    }
}