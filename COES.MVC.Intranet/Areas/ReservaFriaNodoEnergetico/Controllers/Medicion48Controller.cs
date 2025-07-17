using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models;
using COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico.Helper;
using COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Helper;
using COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico;

namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Controllers
{
    public class Medicion48Controller : BaseController
    {
        ReservaFriaNodoEnergeticoAppServicio servicio = new ReservaFriaNodoEnergeticoAppServicio();

        /// <summary>
        /// Fecha de Inicio de consulta
        /// </summary>
        public DateTime? FechaInicio
        {
            get
            {
                return (Session[DatosSesion.FechaConsultaInicio] != null) ?
                    (DateTime?)(Session[DatosSesion.FechaConsultaInicio]) : null;
            }
            set
            {
                Session[DatosSesion.FechaConsultaInicio] = value;
            }
        }


        /// <summary>
        /// Fecha de Fin de Consulta
        /// </summary>
        public DateTime? FechaFinal
        {
            get
            {
                return (Session[DatosSesion.FechaConsultaFin] != null) ?
                  (DateTime?)(Session[DatosSesion.FechaConsultaFin]) : null;
            }
            set
            {
                Session[DatosSesion.FechaConsultaFin] = value;
            }
        }


        /// <summary>
        /// Código de Lectura
        /// </summary>
        public int? Lectura
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.FiltroLectura] != null && (int?)Session[ConstanteReservaFriaNodoEnergetico.FiltroLectura] != 0)
                    ? (int?)(Session[ConstanteReservaFriaNodoEnergetico.FiltroLectura])
                    : ConstantesReservaFriaNodoEnergetico.Lectcodi48PdoSimulado;
            }

            set { Session[ConstanteReservaFriaNodoEnergetico.FiltroLectura] = value; }
        }


        /// <summary>
        /// Código de GrupoCentral
        /// </summary>
        public int? GrupoCentral
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.FiltroGrupoCentral] != null)
                    ? (int?)(Session[ConstanteReservaFriaNodoEnergetico.FiltroGrupoCentral])
                    : 0;
            }

            set { Session[ConstanteReservaFriaNodoEnergetico.FiltroGrupoCentral] = value; }
        }



        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            if (!base.IsValidSesion) return base.RedirectToLogin();

            BusquedaMeMedicion48Model model = new BusquedaMeMedicion48Model();

            string filtroLectura = "," + ConstantesReservaFriaNodoEnergetico.Lectcodi48PdoSimulado + "," +
                                   ConstantesReservaFriaNodoEnergetico.Lectcodi48RdoSimulado + ",";

            List<MeLecturaDTO> listaMeLectura =
               this.servicio.ListMeLecturas()
                   .Where(x => (filtroLectura.IndexOf("," + x.Lectcodi + ",") >= 0)).ToList();

            model.ListaMeLectura = listaMeLectura;
            model.ListaSiTipoinformacion = this.servicio.ListSiTipoinformacions();
            model.ListaMePtomedicion = (new COES.Servicios.Aplicacion.FormatoMedicion.FormatoMedicionAppServicio()).
                ListarPtoMedicion("-1", ConstantesReservaFriaNodoEnergetico.OrigenLecturaNodoEnergetico.ToString(), "-1");

            model.FechaIni = (this.FechaInicio != null)
                ? ((DateTime)this.FechaInicio).ToString(Constantes.FormatoFecha)
                : DateTime.Now.AddDays(-120).ToString(Constantes.FormatoFecha);

            model.FechaFin = (this.FechaFinal != null)
                ? ((DateTime)this.FechaFinal).ToString(Constantes.FormatoFecha)
                : DateTime.Now.ToString(Constantes.FormatoFecha);

            model.Lectura = this.Lectura;
            model.GrupoCentral = this.GrupoCentral;

            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return View(model);
        }


        /// <summary>
        /// Permite la edición de los datos de 30 minutos para el módulo de reserva fría y del nodo energético
        /// </summary>
        /// <param name="idlect">Identificador de lectura</param>
        /// <param name="fecha">Fecha</param>
        /// <param name="idtipoinfo">Código de tipo de información</param>
        /// <param name="id">Código de punto de medición</param>
        /// <param name="accion">Acción</param>
        /// <returns></returns>
        public ActionResult Editar(int idlect, string fecha, int idtipoinfo, int id, int accion)
        {
            //yyyymmdd a yyyy-mm-dd
            fecha = fecha.Substring(0, 4) + "-" + fecha.Substring(4, 2) + "-" + fecha.Substring(6, 2);

            DateTime medifecha = DateTime.ParseExact(fecha, ConstanteReservaFriaNodoEnergetico.FormatoFechaYMD, CultureInfo.InvariantCulture);

            MeMedicion48Model model = new MeMedicion48Model();
            string filtroLectura = "," + ConstantesReservaFriaNodoEnergetico.Lectcodi48PdoSimulado + "," +
                                  ConstantesReservaFriaNodoEnergetico.Lectcodi48RdoSimulado + ",";

            List<MeLecturaDTO> listaMeLectura =
               this.servicio.ListMeLecturas()
                   .Where(x => (filtroLectura.IndexOf("," + x.Lectcodi + ",") >= 0)).ToList();

            model.ListaMeLectura = listaMeLectura;
            model.ListaSiTipoinformacion = this.servicio.ListSiTipoinformacions();
            model.ListaMePtomedicion = (new COES.Servicios.Aplicacion.FormatoMedicion.FormatoMedicionAppServicio()).
                ListarPtoMedicion("-1", ConstantesReservaFriaNodoEnergetico.OrigenLecturaNodoEnergetico.ToString(), "-1");

            MeMedicion48DTO meMedicion48 = this.servicio.GetByIdMeMedicion48(idlect, medifecha, idtipoinfo, id);

            if (meMedicion48 != null)
            {
                model.MeMedicion48 = meMedicion48;
            }
            else
            {
                meMedicion48 = new MeMedicion48DTO();

                meMedicion48.Lectcodi = Convert.ToInt32(Constantes.ParametroDefecto);
                meMedicion48.Tipoinfocodi = ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva;//Convert.ToInt32(Constantes.ParametroDefecto);
                meMedicion48.Ptomedicodi = Convert.ToInt32(Constantes.ParametroDefecto);
                meMedicion48.Medifecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                meMedicion48.Lastdate = DateTime.Now;

                model.MeMedicion48 = meMedicion48;

            }

            model.Accion = accion;
            return View(model);
        }


        /// <summary>
        /// Permite obtener la configuracion de la grilla
        /// </summary>
        /// <param name="idlect">Identificador de lectura</param>
        /// <param name="fecha">Fecha</param>
        /// <param name="idtipoinfo">Código de tipo de información</param>
        /// <param name="id">Código de punto de medición</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerPotenciaDetalle(int idlect, string fecha, int idtipoinfo, int id)
        {
            DateTime medifecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            MeMedicion48Model model = new MeMedicion48Model();

            List<int> titulos = new List<int>();
            List<int> subTitulos = new List<int>();
            List<int> agrupaciones = new List<int>();
            List<int> comentarios = new List<int>();

            string[][] data = new string[48][];
            data = this.PintarCeldas(data, 2);

            //---
            int indice = 0;
            MeMedicion48DTO meMedicion48 = this.servicio.GetByIdMeMedicion48(idlect, medifecha, idtipoinfo, id);
            DateTime fecharec = new DateTime(2017, 1, 1);
            fecharec = fecharec.AddMinutes(30);
            if (meMedicion48 != null)
            {

                for (int i = 1; i <= 48; i++)
                {
                    string f1 = fecharec.ToString(Constantes.FormatoOnlyHora);
                    data[indice][0] = f1;

                    decimal valor = (decimal)meMedicion48.GetType().GetProperty("H" + i.ToString()).GetValue(meMedicion48, null);

                    if (valor != null)
                    {
                        data[indice][1] = valor.ToString();
                    }

                    fecharec = fecharec.AddMinutes(30);
                    indice++;
                }

            }
            else
            {
                for (int i = 1; i <= 48; i++)
                {
                    string f1 = fecharec.ToString(Constantes.FormatoOnlyHora);
                    data[indice][0] = f1;
                    data[indice][1] = "0";
                    fecharec = fecharec.AddMinutes(30);
                    indice++;
                }
            }
            //---


            model.Datos = data;


            List<MergeModel> merge = new List<MergeModel>();
            List<ValidacionListaCelda> validaciones = new List<ValidacionListaCelda>();

            model.Validaciones = validaciones;
            model.IndicesSubtitulo = subTitulos.ToArray();
            model.IndicesTitulo = titulos.ToArray();

            model.Indicador = 48;//list.Count;
            model.IndicesAgrupacion = agrupaciones.ToArray();
            model.IndicesComentario = comentarios.ToArray();

            return Json(model);
        }


        /// <summary>
        /// Permite pintar la data por defecto
        /// </summary>
        /// <param name="data">matriz de datos</param>
        /// <param name="longitud">número de columnas</param>
        /// <returns></returns>
        private string[][] PintarCeldas(string[][] data, int longitud)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new string[longitud];

                for (int j = 0; j < longitud; j++)
                {
                    data[i][j] = string.Empty;
                }
            }

            return data;
        }


        /// <summary>
        /// Permite eliminar datos de reserva fría y nodo energético de 30"
        /// </summary>
        /// <param name="idlect"></param>
        /// <param name="fecha"></param>
        /// <param name="idtipoinfo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int idlect, DateTime fecha, int idtipoinfo, int id)
        {
            try
            {
                this.servicio.DeleteMeMedicion48(idlect, fecha, idtipoinfo, id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite grabar los datos de hojas tipo excel
        /// </summary>
        /// <param name="idlect">Código de lectura</param>
        /// <param name="fecha">Fecha</param>
        /// <param name="idtipoinfo">Código de tipo de inforamción</param>
        /// <param name="id">Código de punto de medición</param>
        /// <param name="dataExcel">Datos Excel</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(int idlect, DateTime fecha, int idtipoinfo, int id, string dataExcel)
        {
            try
            {
                MeMedicion48DTO entity = new MeMedicion48DTO();

                entity.Lectcodi = idlect;
                entity.Medifecha = fecha;
                entity.Tipoinfocodi = idtipoinfo;
                entity.Ptomedicodi = id;

                int nroColumnas = 2;

                List<string> celdas = new List<string>();
                celdas = dataExcel.Split(',').ToList();

                string[][] matriz = GetMatrizExcel(celdas, 0, nroColumnas, 0, celdas.Count / nroColumnas);

                int filaFinal = 0;
                int filas = (celdas.Count / nroColumnas);
                for (int i = 0; i < filas; i++)
                {

                    //todas son blanco?
                    int conteoBlanco = 0;
                    for (int j = 0; j < nroColumnas; j++)
                    {
                        conteoBlanco += (matriz[i][j] == "" ? 1 : 0);

                    }

                    if (conteoBlanco == nroColumnas)
                    {
                        return Json(id);
                    }
                    else
                    {
                        entity.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(entity, Convert.ToDecimal(matriz[i][1]));
                    }

                    filaFinal++;
                }

                entity.Lastuser = base.UserName;
                entity.Lastdate = DateTime.Now;

                int id2 = this.servicio.SaveMeMedicion48Id(entity);

                return Json(id2);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Convierte una lista de datos en una Matriz Excel Web
        /// </summary>
        /// <param name="data">Matriz de datos</param>
        /// <param name="colHead">Columna</param>
        /// <param name="nCol">Número de columnas</param>
        /// <param name="filHead">Fila</param>
        /// <param name="nFil">Número de filas</param>
        /// <returns></returns>
        private static string[][] GetMatrizExcel(List<string> data, int colHead, int nCol, int filHead, int nFil)
        {
            var colTot = nCol + colHead;
            var inicio = (colTot) * filHead;
            int col = 0;
            int fila = 0;
            string[][] arreglo = new string[nFil][];
            arreglo[0] = new string[colTot];

            for (int i = inicio; i < data.Count(); i++)
            {
                string valor = data[i];
                if (col == colTot)
                {
                    fila++;
                    col = 0;
                    arreglo[fila] = new string[colTot];
                }
                arreglo[fila][col] = valor;
                col++;
            }

            return arreglo;
        }


        /// <summary>
        /// Permite obtener un listado de los datos
        /// </summary>
        /// <param name="lectcodi">Código de lectura</param>
        /// <param name="tipoinfocodi">Código de tipo de información</param>
        /// <param name="ptomedicodi">Código de punto de medición</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="nroPage">Número de página</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(int lectcodi, int tipoinfocodi, int ptomedicodi, string fechaIni, string fechaFin, int nroPage)
        {
            BusquedaMeMedicion48Model model = new BusquedaMeMedicion48Model();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (fechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            fechaFinal = fechaFinal.AddDays(1);
            this.FechaInicio = fechaInicio;
            this.FechaFinal = fechaFinal;
            this.Lectura = lectcodi;
            this.GrupoCentral = ptomedicodi;

            model.ListaMeMedicion48 = this.servicio.BuscarOperaciones(lectcodi, tipoinfocodi, ptomedicodi, fechaInicio, fechaFinal, nroPage, Constantes.PageSizeEvento).ToList();

            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return PartialView(model);
        }


        /// <summary>
        /// Permite realizar el paginado
        /// </summary>
        /// <param name="lectcodi">Código de lectura</param>
        /// <param name="tipoinfocodi">Código de tipo de información</param>
        /// <param name="ptomedicodi">Código de punto de medición</param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(int lectcodi, int tipoinfocodi, int ptomedicodi, string fechaIni, string fechaFin)
        {
            Paginacion model = new Paginacion();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (fechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            fechaFinal = fechaFinal.AddDays(1);

            int nroRegistros = this.servicio.ObtenerNroFilas(lectcodi, tipoinfocodi, ptomedicodi, fechaInicio, fechaFinal);

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
