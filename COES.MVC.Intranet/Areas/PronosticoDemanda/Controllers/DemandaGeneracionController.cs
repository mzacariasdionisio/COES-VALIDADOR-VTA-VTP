using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using COES.MVC.Intranet.Areas.PronosticoDemanda.Models;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using COES.Dominio.DTO.Sic;
using System.Configuration;
using COES.MVC.Intranet.Helper;
using log4net;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.ServicioRPF;
using System.Reflection;
using System.Globalization;
using Newtonsoft.Json;
using System.IO;
using COES.MVC.Intranet.Areas.PronosticoDemanda.Helper;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Controllers
{
    public class DemandaGeneracionController : BaseController
    {
        PronosticoDemandaAppServicio servicio = new PronosticoDemandaAppServicio();
        public ActionResult Index()
        {
            return View();
        }

        #region Módulo de Demanda Histórica por Áreas
        /// <summary>
        /// Inicia la opción de demanda histórica por áreas
        /// </summary>
        /// <returns></returns>
        public ActionResult DemandaHistoricaPorAreas()
        {
            PronosticoModel model = new PronosticoModel();
            model.Fecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.ListArea = UtilProdem.ListAreaOperativa(true);
            model.Mensaje = "Puede consultar la demanda hístorica por áreas.";
            model.TipoMensaje = ConstantesProdem.MsgInfo;

            return PartialView(model);
        }

        /// <summary>
        /// Carga los datos y les da formato para la presentación
        /// </summary>
        /// <param name="idArea">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <returns></returns>
        public JsonResult DemandaHistorica(int idArea, string regFecha)
        {
            object res = this.servicio.DemandaHistoricaDatos(idArea, regFecha);
            return Json(res);
        }

        #endregion

        #region Módulo de Iteración Demanda (Perdidas PR03)
        public ActionResult Perdidas()
        {
            PronosticoModel model = new PronosticoModel();
            model.Mensaje = "Módulo de pérdidas segun el PR-03";
            model.TipoMensaje = ConstantesProdem.MsgInfo;
            model.Fecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            return PartialView(model);
        }

        /// <summary>
        /// Carga los datos y les da formato para la presentación
        /// </summary>
        /// <param name="regFecha">Fecha de consulta</param>
        /// <returns></returns>
        public JsonResult PerdidasPR03Datos(string regFecha)
        {
            object res = this.servicio.PerdidasPR03Datos(regFecha);
            return Json(res);
        }

        /// <summary>
        /// Importa el archivo con la información de la demanda Yupana y calcula el % de recálculo
        /// </summary>
        /// <param name="archivo">Archivo importado que sera subido al servidor temporalmente</param>
        /// <param name="regFecha">Fecha para la obtención de la demanda Sein</param>
        /// <returns></returns>
        public JsonResult PerdidasPR03Importar(HttpPostedFileBase archivo, string regFecha)
        {
            object res = new object();
            string typeMsg = String.Empty;
            string dataMsg = String.Empty;

            //string ruta = @"D:\Oficina\COES\Directorio\";
            string ruta = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString();
            string nombreArchivo = Path.GetFileName(archivo.FileName);
            if (!Directory.Exists(ruta))
            {
                typeMsg = "error";
                dataMsg = "La carpeta requerida para la importación no existe";
                res = new { typeMsg, dataMsg };
                return Json(res);
            }

            try
            {
                //Crea el archivo en el servidor                
                FileInfo nuevoArchivo = new FileInfo(ruta + nombreArchivo);
                if (nuevoArchivo.Exists) nuevoArchivo.Delete();
                archivo.SaveAs(ruta + nombreArchivo);

                //Procesa los datos del archivo
                res = this.servicio.PerdidasPR03Procesar(ruta + nombreArchivo, regFecha);

                //Elimina el archivo del servidor
                nuevoArchivo.Delete();
            }
            catch (Exception ex)
            {
                typeMsg = "error";
                dataMsg = ex.Message;
                res = new { typeMsg, dataMsg };
                return Json(res);
            }

            return Json(res);
        }

        /// <summary>
        /// Refleja el pronóstico por áreas a pronóstico por barras
        /// </summary>
        /// <param name="perdidasNorte">% de perdidas del área norte (ingresado por el usuario)</param>
        /// <param name="perdidasSur">% de perdidas del área sur (ingresado por el usuario)</param>
        /// <param name="perdidasCentro">% de perdidas del área centro (ingresado por el usuario)</param>
        /// <param name="perdidasSCentro">% de perdidas del área sierra centro (ingresado por el usuario)</param>
        /// <param name="recalculo">valor porcentual generado a traves de los datos importados</param>
        /// <param name="regFecha">Fecha para la ejecución</param>
        /// <returns></returns>
        public JsonResult PerdidasPR03Ejecutar(decimal[] perdidasNorte, decimal[] perdidasSur,
            decimal[] perdidasCentro, decimal[] perdidasSCentro, decimal[] recalculo, string regFecha)
        {
            object res = this.servicio.PerdidasPR03Ejecutar(perdidasNorte, perdidasSur,
                perdidasCentro, perdidasSCentro, recalculo, regFecha);
            return Json(res);
        }
        #endregion

        #region Módulo de Pronóstico por Generación
        public ActionResult PronosticoPorGeneracion()
        {
            PronosticoModel model = new PronosticoModel();
            model.Mensaje = "Pronóstico de la demanda a nivel de Generación";
            model.TipoMensaje = ConstantesProdem.MsgInfo;
            model.Fecha = DateTime.Now.ToString(ConstantesProdem.FormatoFecha);
            model.ListArea = UtilProdem.ListAreaOperativa(false);
            return PartialView(model);
        }

        /// <summary>
        /// Carga los datos y les da formato para la presentación
        /// </summary>
        /// <param name="idArea">Identificador del área operativa</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="selHistoricos">Lista de registros históricos seleccionados</param>
        /// <returns></returns>
        public JsonResult PronosticoPorGeneracionDatos(int idArea, string regFecha, List<string> selHistoricos)
        {
            object res = this.servicio.PronosticoPorGeneracionDatos(idArea, regFecha, selHistoricos);
            return Json(res);
        }

        /// <summary>
        /// Obtiene los datos de una medición histórica segun fecha
        /// </summary>
        /// <param name="idArea">Identificador del área operativa</param>
        /// <param name="regFecha">Fecha de la medición histórica</param>
        /// <returns></returns>
        public JsonResult PronosticoPorGeneracionMedicion(int idArea, string regFecha)
        {
            decimal[] res = this.servicio.PronosticoPorGeneracionMedicion(idArea, regFecha);
            return Json(res);
        }

        /// <summary>
        /// Ejecuta el pronóstico de la demanda para el día seleccionado
        /// </summary>
        /// <param name="regFecha">Fecha para la cual se ejecuta el pronóstico</param>
        /// <param name="selHistoricos">Fechas utilizadas para el cálculo del pronóstico</param>
        /// <returns></returns>
        public JsonResult PronosticoPorGeneracionEjecutar(string regFecha, List<string> selHistoricos)
        {
            object res = this.servicio.PronosticoPorGeneracionEjecutar(regFecha, selHistoricos);
            return Json(res);
        }

        /// <summary>
        /// Registra los ajustes realizados a la demanda de un área operativa
        /// </summary>
        /// <param name="idArea">Identificador del área operativa</param>
        /// <param name="regFecha">Fecha del registro</param>
        /// <param name="dataMedicion">Datos del ajuste realizado</param>
        /// <returns></returns>
        public JsonResult PronosticoPorGeneracionSave(int idArea, string regFecha, PrnMedicion48DTO dataMedicion)
        {
            object res = this.servicio.PronosticoPorGeneracionSave(idArea, regFecha, dataMedicion, User.Identity.Name);
            return Json(res);
        }
        #endregion

        #region Módulo de Despacho Ejecutado
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ComparativoAppServicio servicioComparativo = new ComparativoAppServicio();

        public ActionResult DespachoEjecutado()
        {
            DespachoEjecutadoModel model = new DespachoEjecutadoModel();
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.ListaAreaOperativa = this.servicio.ListPrnArea(1);
            return View(model);
        }

        [HttpPost]
        public ActionResult Lista(int Areacodi, string sFecha)
        {
            DespachoEjecutadoModel model = new DespachoEjecutadoModel();
            DateTime fechaConsulta = DateTime.ParseExact(sFecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            Log.Info("ListaDespachos - ListPrnMedicionEqs");
            model.ListaDespachos = this.servicio.ListPrnMedicionEqs(Areacodi, fechaConsulta);
            return PartialView(model);
        }

        public ActionResult View(int equicodi, string sFecha)
        {
            DespachoEjecutadoModel model = new DespachoEjecutadoModel();
            model.Equicodi = equicodi;
            model.Fecha = sFecha;
            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar la data del Despacho Ejecutado para el Pronostico
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="fecha"></param>
        /// <param name="listaH"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarPronostico(int equicodi, string fecha, string listaH)
        {
            int iResultado = 1;
            int iPrnmeqtipo = ConstantesProdem.PrnmtipoDesEjecEquipo;
            string user = User.Identity.Name;
            DateTime fechaConsulta = DateTime.ParseExact(fecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            try
            {
                //Traemos el Pronostico ya grabado
                PrnMedicioneqDTO dtoMedicionEq = this.servicio.GetByIdPrnMedicionEq(equicodi, iPrnmeqtipo, fechaConsulta);
                string[] ListaH = listaH.Split(',');
                for (int i = 1; i <= 48; i++)
                {
                    //decimal dAntiguoValor = Convert.ToDecimal(dtoMedicionEq.GetType().GetProperty("H" + i).GetValue(dtoMedicionEq, null));
                    decimal dNuevoValor = Convert.ToDecimal(ListaH[i - 1]);
                    dtoMedicionEq.GetType().GetProperty("H" + i).SetValue(dtoMedicionEq, dNuevoValor);
                }
                dtoMedicionEq.Prnmeqdepurar = Math.Abs(dtoMedicionEq.Prnmeqdepurar) * -1;
                dtoMedicionEq.Prnmequsumodificacion = user;
                this.servicio.UpdatePrnMedicionEq(dtoMedicionEq);
            }
            catch (Exception e)
            {
                string sError = e.Message;
                iResultado = -1;
            }
            return Json(iResultado);
        }

        /// <summary>
        /// Permite grabar la data del RSF para el Pronostico
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="fecha"></param>
        /// <param name="listaH"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarRSF(int equicodi, string fecha, string listaH)
        {
            int iResultado = 1;
            int iPrnmeqtipo = ConstantesProdem.PrnmtipoDesEjecEquipo;
            string user = User.Identity.Name;
            DateTime fechaConsulta = DateTime.ParseExact(fecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            try
            {
                if (listaH.Length == 1)
                    return Json(0);
                List<int> puntosRpf = new List<int>();
                List<int> puntosDespacho = new List<int>();
                this.servicioComparativo.ObtenerPuntosMedicion(null, equicodi, out puntosRpf, out puntosDespacho, fechaConsulta);
                string rpf = string.Join<int>(Constantes.CaracterComa.ToString(), puntosRpf);
                List<ServicioCloud.Medicion> datosRpf = (new ServicioCloud.ServicioCloudClient()).ObtenerDatosComparacionRango(fechaConsulta, rpf).ToList();
                if (datosRpf.Count == 48)
                {
                    string[] ListaH = listaH.Split(',');
                    //Traemos el Pronostico ya grabado
                    PrnMedicioneqDTO dtoMedicionEq = this.servicio.GetByIdPrnMedicionEq(equicodi, iPrnmeqtipo, fechaConsulta);
                    for (int i = 1; i < ListaH.Length; i++)
                    {
                        string sH = ListaH[i]; //Nos indica que intervalo hay que actualizar H1,...H48. ojo el primer elemnto es un 0
                        int iIndice = Convert.ToInt32(sH.Substring(1));
                        decimal ValorRPF = datosRpf[iIndice - 1].H0;
                        dtoMedicionEq.GetType().GetProperty(sH).SetValue(dtoMedicionEq, ValorRPF);
                    }
                    dtoMedicionEq.Prnmeqdepurar = Math.Abs(dtoMedicionEq.Prnmeqdepurar) * -1;
                    dtoMedicionEq.Prnmequsumodificacion = user;
                    this.servicio.UpdatePrnMedicionEq(dtoMedicionEq);
                }
            }
            catch (Exception e)
            {
                string sError = e.Message;
                iResultado = -1;
            }
            return Json(iResultado);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //VISTA DETALLE
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Permite obtener la data
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Comparativo(int? idEmpresa, int? idCentral, string fecha)
        {
            return Json(this.CalcularComparativo(idEmpresa, idCentral, fecha));
        }

        /// <summary>
        /// Obtener comparativo
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private DespachoEjecutadoModel CalcularComparativo(int? idEmpresa, int? idCentral, string fecha)//List<ComparativoItemModel>
        {
            DespachoEjecutadoModel model = new DespachoEjecutadoModel();

            int Prnmeqtipo = ConstantesProdem.PrnmtipoDesEjecEquipo;
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<int> puntosRpf = new List<int>();
            List<int> puntosDespacho = new List<int>();
            this.servicioComparativo.ObtenerPuntosMedicion(idEmpresa, idCentral, out puntosRpf, out puntosDespacho, fechaConsulta);

            model.Mensaje = "Si tiene Puntos relacionados al PRF";
            model.TipoMensaje = ConstantesProdem.MsgInfo;
            if (puntosDespacho.Count == 0)//Validacion si no encuentra puntos relacionados
            {
                this.servicio.ObtenerPuntosFaltantes(idEmpresa, idCentral, out puntosDespacho);
                model.Mensaje = "No tiene Puntos relacionados al RPF";
            }

            string rpf = string.Join<int>(Constantes.CaracterComa.ToString(), puntosRpf);
            string despacho = string.Join<int>(Constantes.CaracterComa.ToString(), puntosDespacho);

            List<ServicioCloud.Medicion> datosRpf = new List<ServicioCloud.Medicion>();
            if (!rpf.Equals(""))
            {
                datosRpf = (new ServicioCloud.ServicioCloudClient()).ObtenerDatosComparacionRango(fechaConsulta, rpf).ToList();
            }
            MeMedicion48DTO datosDespacho = this.servicioComparativo.ObtenerDatosDespacho(fechaConsulta, despacho);
            PrnMedicioneqDTO datosPronostico = this.servicio.GetByIdPrnMedicionEq((int)idCentral, Prnmeqtipo, fechaConsulta);

            //List<ComparativoItemModel> list = DespachoEjecutadoHelper.ObtenerComparativo(fechaConsulta, datosRpf, datosDespacho, datosPronostico);
            model.ListComparativo = DespachoEjecutadoHelper.ObtenerComparativo(fechaConsulta, datosRpf, datosDespacho, datosPronostico);

            //Nombre de la central
            //if (list != null) list[0].Central = datosPronostico.Equinomb;
            //return list;
            if (model.ListComparativo != null) model.ListComparativo[0].Central = datosPronostico.Equinomb;
            return model;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //CALCULO DEL DESPACHO EJECUTADO PARA UN DIA
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Permite realizar la copia del Despacho Ejecutado a la tabla PrnMedicionEq
        /// </summary>
        /// <param name="fecha">Fecha en que se realiza la copia. Formato dd/MM/yyyy</param>
        [HttpPost]
        public JsonResult ObtenerDespachoEjecutado(string fecha)
        {
            int iResultado = 1;
            int idEmpresa = -1;
            char CaracterComa = ',';
            decimal dPorcantajeDesviacion = 0.05M;
            string user = "assetec";
            if (User != null)//ASSETEC 20181122
            {
                user = User.Identity.Name;
            }
            DateTime fechaConsulta = DateTime.ParseExact(fecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
            List<EqEquipoDTO> ListaCentrales = new List<EqEquipoDTO>();
            //traemos la lista de centrales
            try
            {
                //Eliminamos todo el Despacho Copiado para la fecha
                this.servicio.DeletePrnMedicionEq(ConstantesProdem.PrnmtipoDesEjecEquipo, fechaConsulta);
                ListaCentrales = this.servicioComparativo.ObtenerCentrales(-1, fechaConsulta);
                foreach (EqEquipoDTO Central in ListaCentrales)
                {
                    //Para cada central
                    //RPF
                    List<int> puntosRpf = new List<int>();
                    //Despacho
                    List<int> puntosDespacho = new List<int>();
                    //Traemos las listas de cada uno
                    this.servicioComparativo.ObtenerPuntosMedicion(idEmpresa, Central.Equicodi, out puntosRpf, out puntosDespacho, fechaConsulta);

                    //Validacion si no encuentra puntos relacionados busca los que pertencen a la central
                    if (puntosDespacho.Count == 0)
                    {
                        this.servicio.ObtenerPuntosFaltantes(idEmpresa, Central.Equicodi, out puntosDespacho);
                    }

                    //rpf contiene la lista de Puntos de Medicion con origlectcodi == 1
                    string rpf = string.Join<int>(CaracterComa.ToString(), puntosRpf);

                    //Despacho contiene la lista de Puntos de Medición con origlectcodi == 2
                    string despacho = string.Join<int>(CaracterComa.ToString(), puntosDespacho);

                    if (despacho.Equals(""))
                        continue;
                    //El servicio trae los datos del RPF
                    List<ServicioCloud.Medicion> datosRpf = new List<ServicioCloud.Medicion>();
                    if (!rpf.Equals(""))
                    {
                        datosRpf = (new ServicioCloud.ServicioCloudClient()).ObtenerDatosComparacionRango(fechaConsulta, rpf).ToList();
                    }

                    //Para este despacho traemos la SUMA de todos los puntos de medición de cada intervalo H
                    MeMedicion48DTO datosDespacho = this.servicioComparativo.ObtenerDatosDespacho(fechaConsulta, despacho);

                    //Asignamos el Despacho Ejecutado que se almacenara en nuestra tabla para que quede listo para el PRONOSTICO DE LA DEMANDA
                    PrnMedicioneqDTO entity = new PrnMedicioneqDTO();
                    entity.Equicodi = Central.Equicodi;
                    EqAreaDTO dtoArea = this.servicio.GetAreaOperativaByEquipo(Central.Equicodi); //Asignamos el AreaOperativa
                    if (dtoArea != null)
                        entity.Areacodi = dtoArea.Areacodi;
                    entity.Prnmeqtipo = ConstantesProdem.PrnmtipoDesEjecEquipo;
                    entity.Medifecha = fechaConsulta;
                    entity.Prnmeqdejevsrpf = 0; //Almacena la suma del Valor absoluto de las diferencias de Energia del Despacho Ejecutado menos el RPF
                    entity.Prnmequsucreacion = user;
                    entity.Prnmequsumodificacion = user;
                    entity.Prnmeqdepurar = 0;
                    if (datosRpf.Count == 48 && datosDespacho != null && datosDespacho.H1 != null)
                    {
                        int iDepuracion = 0; //NO hay nada que depurar
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal dValorRPF = datosRpf[i - 1].H0;
                            decimal dValorDespacho = Convert.ToDecimal(datosDespacho.GetType().GetProperty(Constantes.CaracterH + i).GetValue(datosDespacho, null));
                            entity.Prnmeqdejevsrpf += Math.Abs(dValorDespacho - dValorRPF);
                            decimal dDesviacion = (dValorRPF != 0) ? (dValorDespacho - dValorRPF) / dValorRPF : 0;
                            dDesviacion = Math.Abs(dDesviacion);
                            if (dDesviacion >= dPorcantajeDesviacion)
                            {
                                iDepuracion++; //Contabiliza lo que va a depurar
                            }
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dValorDespacho);
                            entity.Prnmeqdejevsrpf += dDesviacion;
                        }
                        entity.Prnmeqdepurar = iDepuracion;
                        this.servicio.SavePrnMedicionEq(entity);
                    }
                    else if (datosRpf.Count == 48)
                    {
                        //NO hay nada que depurar, RPF pasa de frente a PRN para el pronostico
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal dValorRPF = datosRpf[i - 1].H0;
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dValorRPF);
                        }
                        this.servicio.SavePrnMedicionEq(entity);
                    }
                    else if (datosDespacho != null && datosDespacho.H1 != null)
                    {
                        //NO hay nada que depurar, Despacho pasa de frente a PRN para el pronostico
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal dValorDespacho = Convert.ToDecimal(datosDespacho.GetType().GetProperty(Constantes.CaracterH + i).GetValue(datosDespacho, null));
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dValorDespacho);
                        }
                        this.servicio.SavePrnMedicionEq(entity);
                    }
                }
            }
            catch (Exception e)
            {
                string sError = e.Message;
                iResultado = -1;
            }
            return Json(iResultado);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //EXPORTAR DEL DESPACHO EJECUTADO PARA UN DIA
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Permite exportar el Despacho Ejecutado
        /// </summary>
        /// <param name="Areacodi">Identificador del área operativa/param>
        /// <param name="sFecha">Fecha en que se realiza la consulta. Formato dd/MM/yyyy</param>
        [HttpPost]
        public JsonResult ExportarData(int Areacodi, string sFecha)
        {
            base.ValidarSesionUsuario();
            try
            {
                int idEmpresa = -1;
                char CaracterComa = ',';
                DateTime fechaConsulta = DateTime.ParseExact(sFecha, ConstantesProdem.FormatoFecha, CultureInfo.InvariantCulture);
                //Traemos la lista de Despachos del día
                List<PrnMedicioneqDTO> listaMedicion = this.servicio.ListPrnMedicionEqs(Areacodi, fechaConsulta);
                List<PrnMedicioneqDTO> listaRpf = new List<PrnMedicioneqDTO>();
                List<PrnMedicioneqDTO> listaDespacho = new List<PrnMedicioneqDTO>();

                foreach (PrnMedicioneqDTO dtoMedicion in listaMedicion)
                {
                    List<int> puntosRpf = new List<int>();
                    List<int> puntosDespacho = new List<int>();

                    this.servicioComparativo.ObtenerPuntosMedicion(idEmpresa, dtoMedicion.Equicodi, out puntosRpf, out puntosDespacho, fechaConsulta);

                    //rpf contiene la lista de Puntos de Medicion con origlectcodi == 1
                    string rpf = string.Join<int>(CaracterComa.ToString(), puntosRpf);
                    //El servicio trae los datos del RPF
                    List<ServicioCloud.Medicion> datosRpf = (new ServicioCloud.ServicioCloudClient()).ObtenerDatosComparacionRango(fechaConsulta, rpf).ToList();
                    PrnMedicioneqDTO entityRpf = new PrnMedicioneqDTO();
                    entityRpf.Equicodi = dtoMedicion.Equicodi;
                    if (datosRpf.Count == 48)
                    {
                        //NO hay nada que depurar, RPF pasa de frente a PRN para el pronostico
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal dValorRPF = datosRpf[i - 1].H0;
                            entityRpf.GetType().GetProperty("H" + i).SetValue(entityRpf, dValorRPF);
                        }
                    }
                    listaRpf.Add(entityRpf);

                    //Despacho contiene la lista de Puntos de Medición con origlectcodi == 2
                    string despacho = string.Join<int>(CaracterComa.ToString(), puntosDespacho);
                    //Para este despacho traemos la SUMA de todos los puntos de medición de cada intervalo H
                    MeMedicion48DTO datosDespacho = this.servicioComparativo.ObtenerDatosDespacho(fechaConsulta, despacho);
                    PrnMedicioneqDTO entityDespacho = new PrnMedicioneqDTO();
                    entityDespacho.Equicodi = dtoMedicion.Equicodi;
                    if (datosDespacho != null && datosDespacho.H1 != null)
                    {
                        //NO hay nada que depurar, Despacho pasa de frente a PRN para el pronostico
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal dValorDespacho = Convert.ToDecimal(datosDespacho.GetType().GetProperty(Constantes.CaracterH + i).GetValue(datosDespacho, null));
                            entityDespacho.GetType().GetProperty("H" + i).SetValue(entityDespacho, dValorDespacho);
                        }

                    }
                    listaDespacho.Add(entityDespacho);
                }

                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString();
                Log.Info("Exportar información Despacho Ejecutado - GenerarFormatoPrnMedicionEq");
                string file = this.servicio.GenerarFormatoPrnMedicionEq(pathFile, pathLogo, fechaConsulta, listaMedicion, listaRpf, listaDespacho);
                return Json(file);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        /// <summary>
        /// Abrir el archivo
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;
            return File(path, app, sFecha + "_" + file);
        }
        #endregion
    }
}