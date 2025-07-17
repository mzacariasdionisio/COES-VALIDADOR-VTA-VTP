using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Helper;


namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico.Controllers
{
    public class PotenciaconsignaController : BaseController
    {
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
        /// Submodulo
        /// </summary>
        public int? SubModulo
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.FiltroSubModulo] != null)
                    ? (int?)(Session[ConstanteReservaFriaNodoEnergetico.FiltroSubModulo])
                    : 0;
            }

            set { Session[ConstanteReservaFriaNodoEnergetico.FiltroSubModulo] = value; }
        }

        /// <summary>
        /// GrupoPotencia
        /// </summary>
        public int? GrupoPotencia
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.FiltroGrupo] != null)
                    ? (int?)(Session[ConstanteReservaFriaNodoEnergetico.FiltroGrupo])
                    : 0;
            }

            set { Session[ConstanteReservaFriaNodoEnergetico.FiltroGrupo] = value; }
        }

        /// <summary>
        /// Estado
        /// </summary>
        public string Estado
        {
            get
            {
                return (Session[ConstanteReservaFriaNodoEnergetico.FiltroEstado] != null)
                    ? (string)(Session[ConstanteReservaFriaNodoEnergetico.FiltroEstado])
                    : "N";

            }

            set { Session[ConstanteReservaFriaNodoEnergetico.FiltroEstado] = value; }
        }


        ReservaFriaNodoEnergeticoAppServicio servReservaNodo = new ReservaFriaNodoEnergeticoAppServicio();        
        
        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            BusquedaNrPotenciaconsignaModel model = new BusquedaNrPotenciaconsignaModel();
            model.ListaNrSubmodulo = servReservaNodo.ListNrSubmodulos();
            model.ListaPrGrupo = servReservaNodo.ListarModoOperacionSubModulo(Convert.ToInt32(ConstanteReservaFriaNodoEnergetico.ParametroTodos));
            
            model.FechaIni = (this.FechaInicio != null)
                ? ((DateTime)this.FechaInicio).ToString(Constantes.FormatoFecha)
                : DateTime.Now.AddDays(-120).ToString(Constantes.FormatoFecha);

            model.FechaFin = (this.FechaFinal != null)
                ? ((DateTime)this.FechaFinal).ToString(Constantes.FormatoFecha)
                : DateTime.Now.ToString(Constantes.FormatoFecha);

            model.SubModulo = this.SubModulo;
            model.GrupoPotencia = this.GrupoPotencia;
            model.Estado = this.Estado;

            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return View(model);
        }

        /// <summary>
        /// Permite cargar los modos de operación para los submódulos de reserva fría y nodo energético
        /// </summary>
        /// <param name="nrsmodCodi">Códio de submódulo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarModoOperacion(int nrsmodCodi)
        {
            NrPotenciaconsignaModel model = new NrPotenciaconsignaModel();

            model.ListaPrGrupo = servReservaNodo.ListarModoOperacionSubModulo(nrsmodCodi);
            return Json(model.ListaPrGrupo);
        }

        /// <summary>
        /// Permite editar la potencia consigna
        /// </summary>
        /// <param name="id">Código de potencia consigna</param>
        /// <param name="accion">Acción</param>
        /// <returns></returns>
        public ActionResult Editar(int id, int accion)
        {

            NrPotenciaconsignaModel model = new NrPotenciaconsignaModel();
            NrPotenciaconsignaDTO nrPotenciaconsigna =null;

            model.ListaNrSubmodulo = servReservaNodo.ListNrSubmodulos();
            model.ListaPrGrupo = servReservaNodo.ListarModoOperacionSubModulo(0);

            if (id != 0)
                nrPotenciaconsigna = servReservaNodo.GetByIdNrPotenciaconsigna(id);

            if (nrPotenciaconsigna != null)
            {
                model.NrPotenciaconsigna = nrPotenciaconsigna;
            }
            else
            {
                nrPotenciaconsigna = new NrPotenciaconsignaDTO();

                nrPotenciaconsigna.Nrsmodcodi = Convert.ToInt32(Constantes.ParametroDefecto);
                nrPotenciaconsigna.Grupocodi = Convert.ToInt32(Constantes.ParametroDefecto);
                nrPotenciaconsigna.Nrpcfecha = DateTime.ParseExact(DateTime.Now.ToString(Constantes.FormatoFecha), 
                    Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                model.NrPotenciaconsigna = nrPotenciaconsigna;
            }

            model.Accion = accion;
            return View(model);            
        }


        /// <summary>
        /// Permite elmiminar una potencia consigna
        /// </summary>
        /// <param name="id">Código de potencia consigna</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                servReservaNodo.DeleteNrPotenciaconsigna(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite deshabilitar una potencia configurada
        /// </summary>
        /// <param name="id">Código de potencia consigna</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Desactivar(int id)
        {
            try
            {
                NrPotenciaconsignaDTO entity = null;

                if (id != 0)
                {
                    entity = servReservaNodo.GetByIdNrPotenciaconsigna(id);

                    entity.Nrpcusumodificacion = base.UserName;
                    entity.Nrpcfecmodificacion = DateTime.Now;
                    entity.Nrpceliminado = "S";

                    servReservaNodo.UpdateNrPotenciaconsigna(entity);
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
        /// Permite grabar la potencia consigna
        /// </summary>
        /// <param name="model">Modelo del tipo potencia consigna</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(NrPotenciaconsignaModel model)
        {
            try
            {
                NrPotenciaconsignaDTO entity = new NrPotenciaconsignaDTO();

                entity.Nrpccodi = model.NrpcCodi;
                entity.Nrsmodcodi = model.NrsmodCodi;
                entity.Grupocodi = model.GrupoCodi;
                entity.Nrpcfecha = DateTime.ParseExact(model.NrpcFecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Nrpceliminado = model.NrpcEliminado;                               

                if (entity.Nrpccodi == 0)
                {
                    entity.Nrpcusucreacion = base.UserName;
                    entity.Nrpcfeccreacion = DateTime.Now;
                }
                else
                {
                    if (model.NrpcUsuCreacion != null)
                    {
                        entity.Nrpcusucreacion = model.NrpcUsuCreacion;
                    }

                    if (model.NrpcFecCreacion != null)
                    {
                        entity.Nrpcfeccreacion = DateTime.ParseExact(model.NrpcFecCreacion, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }

                    entity.Nrpcusumodificacion = base.UserName;
                    entity.Nrpcfecmodificacion = DateTime.Now;
                }

                int id = this.servReservaNodo.SaveNrPotenciaconsignaId(entity);                
                return Json(id);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Convierte una lista de potencia consigna en una Matriz Excel Web
        /// </summary>
        /// <param name="data">datos en una lista</param>
        /// <param name="colHead">Cabecera</param>
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
        /// Permite grabar las potencias consigna
        /// </summary>
        /// <param name="id">Código de potencia consigna</param>
        /// <param name="dataExcel">Daros "excel"</param>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarConsigna(int id, string dataExcel, string fecha)
        {
            try
            {
                int nroColumnas = 3;

                List<string> celdas = new List<string>();
                celdas = dataExcel.Split(',').ToList();

                string[][] matriz = GetMatrizExcel(celdas, 0, nroColumnas, 0, celdas.Count / nroColumnas);
                
                //eliminar consignas
                servReservaNodo.DeleteNrPotenciaconsignaDetalleTotal(id);
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
                        //insertar fila
                        NrPotenciaconsignaDetalleDTO entity = new NrPotenciaconsignaDetalleDTO();

                        entity.Nrpccodi = id;
                        entity.Nrpcdfecha = DateTime.ParseExact(fecha + " " + matriz[i][0] + "",
                            Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);

                        if (filaFinal > 0 && matriz[i][0] == "00:00")
                        {
                            entity.Nrpcdfecha = ((DateTime) entity.Nrpcdfecha).AddDays(1);
                        }

                        entity.Nrpcdmw = Convert.ToDecimal(matriz[i][1]);
                        entity.Nrpcdmaximageneracion = matriz[i][2];
                        entity.Nrpcdusucreacion = base.UserName;
                        entity.Nrpcdfeccreacion = DateTime.Now;

                        servReservaNodo.SaveNrPotenciaconsignaDetalleId(entity);
                    }

                    filaFinal++;
                }

                return Json(1);                
            }
            catch
            {
                return Json(-1);
            }
        }



        /// <summary>
        /// Permite listar las potencias consigna
        /// </summary>
        /// <param name="nrsmodCodi">Código de submódulo</param>
        /// <param name="grupoCodi">Código de grupo</param>
        /// <param name="nrpcFechaIni">Fecha inicial</param>
        /// <param name="nrpcFechaFin">Fecha final</param>
        /// <param name="estado">Estado</param>
        /// <param name="nroPage">Número de página</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(int nrsmodCodi, int grupoCodi, string nrpcFechaIni, string nrpcFechaFin, string estado, int nroPage)
        {
            BusquedaNrPotenciaconsignaModel model = new BusquedaNrPotenciaconsignaModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (nrpcFechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(nrpcFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (nrpcFechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(nrpcFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            this.FechaInicio = fechaInicio;
            this.FechaFinal = fechaFinal;
            this.SubModulo = nrsmodCodi;
            this.GrupoPotencia = grupoCodi;
            this.Estado = estado;

            model.ListaNrPotenciaconsigna = servReservaNodo.BuscarOperaciones(nrsmodCodi, grupoCodi, fechaInicio, fechaFinal, estado, nroPage, Constantes.PageSizeEvento).ToList();

            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return PartialView(model);
        }


        /// <summary>
        /// Permite realizar el paginado de las potencias consigna
        /// </summary>
        /// <param name="nrsmodCodi">Código de submódulo</param>
        /// <param name="grupoCodi">Código de grupo</param>
        /// <param name="nrpcFechaIni">Fecha inicial</param>
        /// <param name="nrpcFechaFin">Fecha final</param>
        /// <param name="estado">Estado</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(int nrsmodCodi, int grupoCodi, string nrpcFechaIni, string nrpcFechaFin, string estado)
        {
            Paginacion model = new Paginacion();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (nrpcFechaIni != null)
            {
                fechaInicio = DateTime.ParseExact(nrpcFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (nrpcFechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(nrpcFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            int nroRegistros = servReservaNodo.ObtenerNroFilas(nrsmodCodi, grupoCodi, fechaInicio, fechaFinal, estado);

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

        /// <summary>
        /// Permite pintar la data por defecto
        /// </summary>
        /// <param name="data">Datos contenidos en una matriz</param>
        /// <returns></returns>
        private string[][] PintarCeldas(string[][] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new string[3];

                for (int j = 0; j < 3; j++)
                {
                    data[i][j] = string.Empty;
                }
            }

            return data;
        }


        /// <summary>
        /// Permite obtener la configuracion de la grilla
        /// </summary>
        /// <param name="id">Código de potencia consigna</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerPotenciaDetalle(int id)
        {

            NrPotenciaconsignaDetalleModel model = new NrPotenciaconsignaDetalleModel();

            List<NrPotenciaconsignaDetalleDTO> list = this.servReservaNodo.ListNrPotenciaconsignaDetalles().Where(x => x.Nrpccodi == id).ToList();
            List<int> titulos = new List<int>();
            List<int> subTitulos = new List<int>();
            List<int> agrupaciones = new List<int>();
            List<int> comentarios = new List<int>();

            string[][] data = new string[list.Count + 5][];
            data = this.PintarCeldas(data);

            int indice = 0;
            //data[indice][0] = "Potencia Consigna";

            //data[indice + 1][0] = "Hora";
            //data[indice + 1][1] = "MW";
            //data[indice + 1][2] = "Máxima Gen";
            /*
            agrupaciones.Add(indice);
            titulos.Add(indice);
            subTitulos.Add(indice + 1);
            indice+=2;
            */
            foreach (NrPotenciaconsignaDetalleDTO item in list)
            {
                data[indice][0] = ((DateTime)item.Nrpcdfecha).ToString(Constantes.FormatoHoraMinuto);
                data[indice][1] = ((Decimal)item.Nrpcdmw).ToString();                
                data[indice][2] = ((item.Nrpcdmaximageneracion != null) ? item.Nrpcdmaximageneracion : "N");//(item.Nrpcdmaximageneracion == "S").ToString();

                indice++;
            }

            model.Datos = data;
                        
            List<MergeModel> merge = new List<MergeModel>();
            List<ValidacionListaCelda> validaciones = new List<ValidacionListaCelda>();

            model.Validaciones = validaciones;            
            model.IndicesSubtitulo = subTitulos.ToArray();
            model.IndicesTitulo = titulos.ToArray();

            model.Indicador = list.Count;
            model.IndicesAgrupacion = agrupaciones.ToArray();
            model.IndicesComentario = comentarios.ToArray();

            return Json(model);
        }
    }
}
