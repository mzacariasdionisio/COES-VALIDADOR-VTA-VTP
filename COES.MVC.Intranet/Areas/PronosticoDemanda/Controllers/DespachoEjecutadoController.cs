using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.PronosticoDemanda.Helper;
using COES.MVC.Intranet.Areas.PronosticoDemanda.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using COES.Servicios.Aplicacion.ServicioRPF;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Controllers
{
    public class DespachoEjecutadoController : BaseController
    {
        public DespachoEjecutadoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Instancia de la clase de servicio
        /// </summary>
        ComparativoAppServicio servicio = new ComparativoAppServicio();
        PronosticoDemandaAppServicio servicioPronostico = new PronosticoDemandaAppServicio();

        /// <summary>
        /// Pagina inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            DespachoEjecutadoModel model = new DespachoEjecutadoModel();
            //model.ListaEmpresas = this.servicio.ObtenerEmpresas();
            //model.ListaCentrales = this.servicio.ObtenerCentrales(-1);
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.ListaAreaOperativa = this.servicioPronostico.ListPrnArea(1);
            return View(model);
        }

        [HttpPost]
        public ActionResult Lista(int Areacodi, string sFecha)
        {
            DespachoEjecutadoModel model = new DespachoEjecutadoModel();
            DateTime fechaConsulta = DateTime.ParseExact(sFecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            Log.Info("ListaDespachos - ListPrnMedicionEqs");
            model.ListaDespachos = this.servicioPronostico.ListPrnMedicionEqs(Areacodi, fechaConsulta);
            //model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, User.Identity.Name);
            return PartialView(model);
        }

        public ActionResult View(int equicodi, string sFecha)
        {
            DespachoEjecutadoModel model = new DespachoEjecutadoModel();
            model.Equicodi = equicodi;
            model.Fecha = sFecha;
            //Log.Info("EntidadProvisionbase - GetByIdVcrProvisionbaseView");
            //model.EntidadProvisionbase = this.servicioCompensacionRsf.GetByIdVcrProvisionbaseView(vcrpbcodi);
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
                PrnMedicioneqDTO dtoMedicionEq = this.servicioPronostico.GetByIdPrnMedicionEq(equicodi, iPrnmeqtipo, fechaConsulta);
                string[] ListaH = listaH.Split(',');
                for (int i = 1; i <= 48; i++)
                {
                    //decimal dAntiguoValor = Convert.ToDecimal(dtoMedicionEq.GetType().GetProperty("H" + i).GetValue(dtoMedicionEq, null));
                    decimal dNuevoValor = Convert.ToDecimal(ListaH[i - 1]);
                    dtoMedicionEq.GetType().GetProperty("H" + i).SetValue(dtoMedicionEq, dNuevoValor);
                }
                dtoMedicionEq.Prnmeqdepurar = Math.Abs(dtoMedicionEq.Prnmeqdepurar) * -1;
                dtoMedicionEq.Prnmequsumodificacion = user;
                this.servicioPronostico.UpdatePrnMedicionEq(dtoMedicionEq);
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
                this.servicio.ObtenerPuntosMedicion(null, equicodi, out puntosRpf, out puntosDespacho, fechaConsulta);
                string rpf = string.Join<int>(Constantes.CaracterComa.ToString(), puntosRpf);
                List<ServicioCloud.Medicion> datosRpf = (new ServicioCloud.ServicioCloudClient()).ObtenerDatosComparacionRango(fechaConsulta, rpf).ToList();
                if (datosRpf.Count == 48)
                {
                    string[] ListaH = listaH.Split(',');
                    //Traemos el Pronostico ya grabado
                    PrnMedicioneqDTO dtoMedicionEq = this.servicioPronostico.GetByIdPrnMedicionEq(equicodi, iPrnmeqtipo, fechaConsulta);
                    for (int i = 1; i < ListaH.Length; i++)
                    {
                        string sH = ListaH[i]; //Nos indica que intervalo hay que actualizar H1,...H48. ojo el primer elemnto es un 0
                        int iIndice = Convert.ToInt32(sH.Substring(1));
                        decimal ValorRPF = datosRpf[iIndice - 1].H0;
                        dtoMedicionEq.GetType().GetProperty(sH).SetValue(dtoMedicionEq, ValorRPF);
                    }
                    dtoMedicionEq.Prnmeqdepurar = Math.Abs(dtoMedicionEq.Prnmeqdepurar) * -1;
                    dtoMedicionEq.Prnmequsumodificacion = user;
                    this.servicioPronostico.UpdatePrnMedicionEq(dtoMedicionEq);
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
            this.servicio.ObtenerPuntosMedicion(idEmpresa, idCentral, out puntosRpf, out puntosDespacho, fechaConsulta);

            model.Mensaje = "Si tiene Puntos relacionados al PRF";
            model.TipoMensaje = ConstantesProdem.MsgInfo;
            if (puntosDespacho.Count == 0)//Validacion si no encuentra puntos relacionados
            {
                this.servicioPronostico.ObtenerPuntosFaltantes(idEmpresa, idCentral, out puntosDespacho);
                model.Mensaje = "No tiene Puntos relacionados al RPF";
            }

            string rpf = string.Join<int>(Constantes.CaracterComa.ToString(), puntosRpf);
            string despacho = string.Join<int>(Constantes.CaracterComa.ToString(), puntosDespacho);

            List<ServicioCloud.Medicion> datosRpf = new List<ServicioCloud.Medicion>();
            if (!rpf.Equals(""))
            {
                datosRpf = (new ServicioCloud.ServicioCloudClient()).ObtenerDatosComparacionRango(fechaConsulta, rpf).ToList();
            }
            MeMedicion48DTO datosDespacho = this.servicio.ObtenerDatosDespacho(fechaConsulta, despacho);
            PrnMedicioneqDTO datosPronostico = this.servicioPronostico.GetByIdPrnMedicionEq((int)idCentral, Prnmeqtipo, fechaConsulta);

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
                this.servicioPronostico.DeletePrnMedicionEq(ConstantesProdem.PrnmtipoDesEjecEquipo, fechaConsulta);
                ListaCentrales = this.servicio.ObtenerCentrales(-1, fechaConsulta);
                foreach (EqEquipoDTO Central in ListaCentrales)
                {
                    //Para cada central
                    //RPF
                    List<int> puntosRpf = new List<int>();
                    //Despacho
                    List<int> puntosDespacho = new List<int>();
                    //Traemos las listas de cada uno
                    this.servicio.ObtenerPuntosMedicion(idEmpresa, Central.Equicodi, out puntosRpf, out puntosDespacho, fechaConsulta);

                    //Validacion si no encuentra puntos relacionados busca los que pertencen a la central
                    if (puntosDespacho.Count == 0)
                    {
                        this.servicioPronostico.ObtenerPuntosFaltantes(idEmpresa, Central.Equicodi, out puntosDespacho);
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
                    MeMedicion48DTO datosDespacho = this.servicio.ObtenerDatosDespacho(fechaConsulta, despacho);

                    //Asignamos el Despacho Ejecutado que se almacenara en nuestra tabla para que quede listo para el PRONOSTICO DE LA DEMANDA
                    PrnMedicioneqDTO entity = new PrnMedicioneqDTO();
                    entity.Equicodi = Central.Equicodi;
                    EqAreaDTO dtoArea = this.servicioPronostico.GetAreaOperativaByEquipo(Central.Equicodi); //Asignamos el AreaOperativa
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
                        this.servicioPronostico.SavePrnMedicionEq(entity);
                    }
                    else if (datosRpf.Count == 48)
                    {
                        //NO hay nada que depurar, RPF pasa de frente a PRN para el pronostico
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal dValorRPF = datosRpf[i - 1].H0;
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dValorRPF);
                        }
                        this.servicioPronostico.SavePrnMedicionEq(entity);
                    }
                    else if (datosDespacho != null && datosDespacho.H1 != null)
                    {
                        //NO hay nada que depurar, Despacho pasa de frente a PRN para el pronostico
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal dValorDespacho = Convert.ToDecimal(datosDespacho.GetType().GetProperty(Constantes.CaracterH + i).GetValue(datosDespacho, null));
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dValorDespacho);
                        }
                        this.servicioPronostico.SavePrnMedicionEq(entity);
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
        /// <param name="fecha">Fecha en que se realiza la consulta. Formato dd/MM/yyyy</param>
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
                List<PrnMedicioneqDTO> listaMedicion = this.servicioPronostico.ListPrnMedicionEqs(Areacodi, fechaConsulta);
                List<PrnMedicioneqDTO> listaRpf = new List<PrnMedicioneqDTO>();
                List<PrnMedicioneqDTO> listaDespacho = new List<PrnMedicioneqDTO>();

                foreach (PrnMedicioneqDTO dtoMedicion in listaMedicion)
                {
                    List<int> puntosRpf = new List<int>();
                    List<int> puntosDespacho = new List<int>();

                    this.servicio.ObtenerPuntosMedicion(idEmpresa, dtoMedicion.Equicodi, out puntosRpf, out puntosDespacho, fechaConsulta);

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
                    MeMedicion48DTO datosDespacho = this.servicio.ObtenerDatosDespacho(fechaConsulta, despacho);
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
                string file = this.servicioPronostico.GenerarFormatoPrnMedicionEq(pathFile, pathLogo, fechaConsulta, listaMedicion, listaRpf, listaDespacho);
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

    }
}