using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.Eventos.Helper;
using COES.MVC.Extranet.Areas.Eventos.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Pruebaunidad;
using COES.Servicios.Aplicacion.StockCombustibles;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Eventos.Controllers
{
    public class PruebaunidadController : FormatoController
    {
        //
        // GET: /Eventos/Envio/
        PruebaunidadAppServicio servPruebaunidad = new PruebaunidadAppServicio();
        FormatoMedicionAppServicio servFormatomed = new FormatoMedicionAppServicio();
        EquipamientoAppServicio servEquipo = new EquipamientoAppServicio();


        /// <summary>
        /// Principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            if (!base.IsValidSesion) return base.RedirectToLogin();

            PruebaUnidadModel model = new PruebaUnidadModel();
            model.ListaEmpresas = new List<SiEmpresaDTO>();

            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }


        /// <summary>
        /// Permite obtener la unidad sorteada de acuerdo a una fecha seleccionada
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UnidadSorteada(string fecha)
        {
            //equipo:la unidad sorteada
            List<EqEquipoDTO> equipos = new List<EqEquipoDTO>();

            try
            {
                DateTime fechaUnidad = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                EqEquipoDTO equipo = servPruebaunidad.ObtenerUnidadSorteada(fechaUnidad);

                if (equipo != null)
                {
                    equipos.Add(equipo);
                }

                //se agregan las unidades habilitadas
                List<EqEquipoDTO> listaUnidadHabilitada = servPruebaunidad.ObtenerUnidadSorteadaHabilitada(fechaUnidad);

                if (equipo != null)
                {
                    List<EqEquipoDTO> listaRepetida = listaUnidadHabilitada.Where(x => x.Equicodi == equipo.Equicodi).ToList();
                    foreach (EqEquipoDTO equipo1 in listaRepetida)
                    {
                        listaUnidadHabilitada.Remove(equipo1);
                    }

                    foreach (EqEquipoDTO equipo1 in listaUnidadHabilitada)
                    {
                        equipos.Add(equipo1);
                    }

                }
                else
                {
                    //agregando equipos de unidades habilitadas
                    equipos = new List<EqEquipoDTO>();

                    foreach (EqEquipoDTO item in listaUnidadHabilitada)
                    {
                        equipos.Add(item);
                    }
                }


     
                if (equipos.Count() > 0)
                {
     
                    return Json(equipos);
                }
                else
                {
                    EqEquipoDTO eq = new EqEquipoDTO();
                    eq.Equicodi = 0;
                    eq.Equinomb = "No se registró unidad";
                    eq.Emprcodi = 0;
                    eq.Emprnomb = "---";
                    equipos.Add(eq);
                 
                    return Json(equipos);
                }
            }
            catch
            {
                EqEquipoDTO eq = new EqEquipoDTO();
                eq.Equicodi = -1;
                eq.Equinomb = "---";
                eq.Emprcodi = 0;
                eq.Emprnomb = "---";
                equipos.Add(eq);
      
                return Json(equipos);
            }
        }


        /// <summary>
        /// Permite obtener datos almacenados para un envío
        /// </summary>
        /// <param name="idEnvio">Código de envío</param>
        /// <param name="fecha">Fecha</param>
        /// <param name="idEmpresa">Código de empresa</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarDatos(int idEnvio, string fecha, int idEmpresa, int equicodi)
        {
            try
            {
                PruebaUnidadModel jsModel = BuildHojaExcel(idEmpresa, idEnvio, fecha, equicodi);
                return Json(jsModel);

            }
            catch
            {
                return Json(-1);
            }

        }


        /// <summary>
        /// Devuelve el model con informacion de Presion de Gas
        /// </summary>sic
        /// <param name="idEmpresa">Código de empresa</param>
        /// <param name="idEnvio">Código de envío</param>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        public PruebaUnidadModel BuildHojaExcel(int idEmpresa, int idEnvio, string fecha, int equicodi)
        {
            DateTime fechaUnidad = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            PruebaUnidadModel model = new PruebaUnidadModel();
            model.Handson = new HandsonModel();


            //--- obteniendo nombre de empresa            

            EqEquipoDTO equipo = servEquipo.GetByIdEqEquipo(equicodi);
            model.Empresa = equipo != null ? equipo.EMPRNOMB : "";
            //---


            ////////// Obtiene el Fotmato ////////////////////////
            model.Formato = servFormato.GetByIdMeFormato(ConstantesEventos.IdFormatoPruebaUnidad);

            /// DEFINICION DEL FORMATO //////
            model.Formato.Formatcols = 1;
            model.Formato.Formatrows = 3;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.FilasCabecera = model.Formato.Formatrows;


            int idCfgFormato = 0;
            List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>();
            if (idEnvio <= 0)
            {
                model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, "", "", fecha, Constantes.FormatoFecha);
                Tools.GetSizeFormato(model.Formato);
                model.EnPlazo = servFormatomed.ValidarPlazo(model.Formato);
                model.Handson.ReadOnly = false;
            }
            else  // Fecha proceso es obtenida del registro envio
            {
                model.Handson.ReadOnly = true;

                var envioAnt = servFormato.GetByIdMeEnvio(idEnvio);
                if (envioAnt != null)
                {
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    if (envioAnt.Cfgenvcodi != null)
                    {
                        idCfgFormato = (int)envioAnt.Cfgenvcodi;
                    }
                }
                else
                    model.Formato.FechaProceso = DateTime.MinValue;
                Tools.GetSizeFormato(model.Formato);


                model.EnPlazo = envioAnt.Envioplazo == "P";

                listaCambios = servFormato.GetAllCambioEnvio(ConstantesEventos.IdFormatoPruebaUnidad, model.Formato.FechaProceso, model.Formato.FechaProceso, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
            }
            model.Formato.FechaInicio = model.Formato.FechaProceso;
            model.Formato.FechaFin = model.Formato.FechaProceso;

            model.Anho = model.Formato.FechaInicio.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaInicio.Month);
            model.Dia = fechaUnidad.ToString(Constantes.FormatoFecha);
            model.ListaEnvios = servFormato.GetByCriteriaMeEnvios(idEmpresa, ConstantesEventos.IdFormatoPruebaUnidad, fechaUnidad);

            int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar
            if (model.ListaEnvios.Count > 0)
            {
                idUltEnvio = model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi;
                var reg = model.ListaEnvios.Find(x => x.Enviocodi == idEnvio);
                if (reg != null)
                    model.FechaEnvio = ((DateTime)reg.Enviofecha).ToString(Constantes.FormatoFechaHora);
            }

            //obteniendo el ptomedicodi de la prueba
  
            int ptomedicodi = servFormatomed.GetByIdEquipoMePtomedicion(equicodi, 1).Where(x => x.Origlectcodi == ConstantesEventos.OriglectcodiDespacho).FirstOrDefault().Ptomedicodi;

            model.ListaHojaPto = new List<MeHojaptomedDTO>();
            MeHojaptomedDTO pto = new MeHojaptomedDTO();
            pto.Ptomedicodi = ptomedicodi;
            pto.Ptomedidesc = servFormato.GetByIdMePtomedicion(ptomedicodi).Ptomedielenomb != null ? servFormato.GetByIdMePtomedicion(ptomedicodi).Ptomedielenomb : "";
            pto.Hojaptoliminf = 0;
            pto.Hojaptolimsup = 9999;
            pto.Tipoinfocodi = 1;

            model.ListaHojaPto.Add(pto);

            model.Handson.ListaColWidth = new List<int>();
            model.Handson.ListaColWidth.Add(150);
            for (var i = 0; i < model.ListaHojaPto.Count; i++)
            {
                model.Handson.ListaColWidth.Add(100);
            }

            List<MeMedicion96DTO> lista96 = new List<MeMedicion96DTO>();

            lista96 = servPruebaunidad.GetByCriteriaMedicion96(ConstantesEventos.TipoinfocodiMw, ptomedicodi, ConstantesEventos.LectcodiPruebaUnidad, fechaUnidad, fechaUnidad);

            foreach (MeMedicion96DTO item in lista96)
            {
                item.Tipoptomedicodi = -1;
            }

            if (idEnvio >= 0) // Es nuevo envio(se consulta el ultimo envio) o solo se consulta envio seleccionado de la BD
            {
              
                var lista = servFormatomed.GetDataFormato96(lista96, idEmpresa, model.Formato, idEnvio, idUltEnvio);
                model.Handson.ListaExcelData = Tools.ObtenerListaExcelData(model, lista, listaCambios, fecha, idEnvio);
            }
            else
            {
                model.Handson.ListaExcelData = this.MatrizExcel;

            }

            model.Formato.RowPorDia = 96;


            return model;
        }


        /// <summary>
        /// Graba los datos enviados por el agente
        /// </summary>
        /// <param name="data">Datos</param>
        /// <param name="idEquipo">Código de equipo</param>
        /// <param name="fecha">fecha</param>
        /// <param name="idEmpresa">Código de empresa</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarExcelWeb(string[][] data, int idEquipo, string fecha, int idEmpresa)
        {

            DateTime fechaUnidad = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            base.ValidarSesionUsuario();

            bool usuarioValido = ValidarUsuarioEmpresa(idEmpresa);

            ///////// Definicion de Variables ////////////////
            FormatoResultado model = new FormatoResultado();
            model.Resultado = 0;
            int idFormato = ConstantesEventos.IdFormatoPruebaUnidad;
            int exito = 0;
            string empresa = string.Empty;

            //////////////////////////////////////////////////

            if (!usuarioValido)
            {
                exito = -2;
                model.Resultado = exito;
                return Json(model);
            }

            MeFormatoDTO formato = servFormato.GetByIdMeFormato(idFormato);
            formato.Formatcols = 1;
            formato.Formatrows = 1;

            int filaHead = formato.Formatrows;
            int colHead = formato.Formatcols;
            var listaPto = servFormato.GetByCriteriaMeHojaptomeds(-1, idFormato, fechaUnidad, fechaUnidad);


            //---
            //obteniendo el ptomedicodi de la prueba

            int ptomedicodi = servFormatomed.GetByIdEquipoMePtomedicion(idEquipo, 1).Where(x => x.Origlectcodi == ConstantesEventos.OriglectcodiDespacho).FirstOrDefault().Ptomedicodi;

            var listaPtoFiltro = listaPto.Where(x => x.Ptomedicodi == ptomedicodi);

            if (listaPtoFiltro.Count() == 0)
            {
                exito = -3;
                model.Resultado = exito;
                return Json(model);
            }


            listaPto = new List<MeHojaptomedDTO>();
            MeHojaptomedDTO pto = new MeHojaptomedDTO();
            pto.Ptomedicodi = ptomedicodi;

            listaPto.Add(pto);
            //---

            int nPtos = listaPto.Count;

            /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////

            formato.FechaProceso = fechaUnidad;
            Tools.GetSizeFormato(formato);
            /////////////// Grabar Config Formato Envio //////////////////
            MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
            config.Formatcodi = idFormato;
            config.Emprcodi = idEmpresa;

            ///////////////Grabar Envio//////////////////////////
       
            Boolean enPlazo = servFormatomed.ValidarPlazo(formato);
            MeEnvioDTO envio = new MeEnvioDTO();
            envio.Archcodi = 0;
            envio.Emprcodi = idEmpresa;
            envio.Enviofecha = DateTime.Now;
            envio.Enviofechaperiodo = formato.FechaProceso;
            envio.Enviofechaini = formato.FechaInicio;
            envio.Enviofechafin = formato.FechaFin;
            envio.Envioplazo = (enPlazo) ? "P" : "F";
            envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
            envio.Lastdate = DateTime.Now;
            envio.Lastuser = User.Identity.Name;
            envio.Userlogin = User.Identity.Name;
            envio.Formatcodi = idFormato;
       
            int idEnvio = servFormato.SaveMeEnvio(envio);
            model.IdEnvio = idEnvio;
            ///////////////////////////////////////////////////////
   
            try
            {
                var lista96 = ObtenerDatos(data, listaPto, formato.Formatcheckblanco);

                try
                {
        
                    servFormatomed.GrabarValoresCargados96(lista96, User.Identity.Name, idEnvio, idEmpresa, formato);
                }
                catch
                {

                }

                envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                envio.Enviocodi = idEnvio;
                envio.Cfgenvcodi = 0;

                servFormato.UpdateMeEnvio(envio);
                exito = 1;
                model.Resultado = 1;
            }
            catch
            {
                exito = -1;
                model.Resultado = -1;
            }

            model.Resultado = exito;
            return Json(model);
        }


        /// <summary>
        /// Permite validar si el usuario puede registrar
        /// </summary>
        /// <param name="emprcodi">Código de empresa</param>
        /// <returns></returns>
        private bool ValidarUsuarioEmpresa(int emprcodi)
        {

            foreach (var item in base.ListaEmpresas)
            {
                if (item.EMPRCODI == emprcodi || item.EMPRCODI == ConstantesEventos.EmprcodiCoes || base.EmpresaId == ConstantesEventos.EmprcodiCoes)
                {
                    return true;
                }

            }

            return false;

        }


        /// <summary>
        /// Lee los datos del formato web y los almacena en una lista DTO
        /// </summary>
        /// <param name="datos">datos del formato</param>
        /// <param name="ptos">Puntos</param>
        /// <param name="checkBlanco">check</param>
        /// <returns></returns>
        private List<MeMedicion96DTO> ObtenerDatos(string[][] datos, List<MeHojaptomedDTO> ptos, int checkBlanco)
        {
            int nFil = 99;
            int nCol = ptos.Count + 1;
            List<MeMedicion96DTO> lista = new List<MeMedicion96DTO>();
            MeMedicion96DTO reg;
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;

            fecha = DateTime.ParseExact(datos[3][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);

            for (var i = 1; i < nCol; i++)
            {
                reg = new MeMedicion96DTO();
                for (var j = 3; j < nFil; j++)
                {
                    int indice = j - 2;
                    reg.Ptomedicodi = ptos[i - 1].Ptomedicodi;
                    reg.Tipoinfocodi = ConstantesEventos.TipoinfocodiMw;
                    reg.Meditotal = 0;
                    reg.Lectcodi = ConstantesEventos.LectcodiPruebaUnidad;

                    reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                    stValor = datos[j][i];
                    if (COES.Base.Tools.Util.EsNumero(stValor))
                    {
                        valor = decimal.Parse(stValor);
                        reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                    }
                    else
                    {
                        if (checkBlanco == 0)
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);
                        else
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, 0);
                    }
                }
                lista.Add(reg);
            }
            return lista;
        }


        /// <summary>
        /// Permite generar el formato en formato excel
        /// </summary>
        /// <param name="idEquipo">Código de equipo</param>
        /// <param name="dia">Día</param>
        /// <param name="idEmpresa">Código de empresa</param>
        /// <returns></returns>
        [HttpPost]
        public string GenerarFormato(int idEquipo, string dia, int idEmpresa, int equicodi)
        {
            int idEnvio = 0;
            string ruta = string.Empty;
            try
            {
                PruebaUnidadModel model = BuildHojaExcel(idEmpresa, idEnvio, dia, equicodi);
                ruta = Tools.GenerarFileExcel(model);
            }
            catch
            {
                ruta = "-1";
            }
            return ruta;
        }


        /// <summary>
        /// Lee datos desde excel
        /// </summary>
        /// <param name="idEquipo">Código de equipo</param>
        /// <param name="dia">Día</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LeerFileUpExcel(int idEquipo, string dia)
        {
            int retorno = Tools.VerificarIdsFormato(this.NombreFile, idEquipo, ConstantesStockCombustibles.IdFormatoPGas);

            if (retorno > 0)
            {
                DateTime fechaUnidad = DateTime.ParseExact(dia, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                MeFormatoDTO formato = servFormato.GetByIdMeFormato(ConstantesStockCombustibles.IdFormatoPGas);
                DateTime fechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, string.Empty, string.Empty, dia, Constantes.FormatoFecha);
                formato.FechaProceso = fechaProceso;
                var cabecera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();

                //obteniendo el ptomedicodi de la prueba
                //int ptomedicodi = servPruebaunidad.ObtenerPtomedicionSorteo(fechaUnidad, ConstantesEventos.OriglectcodiDespacho);
                int ptomedicodi = servFormatomed.GetByIdEquipoMePtomedicion(idEquipo, 1).Where(x => x.Origlectcodi == ConstantesEventos.OriglectcodiDespacho).FirstOrDefault().Ptomedicodi;

                var listaPtos = new List<MeHojaptomedDTO>();
                MeHojaptomedDTO pto = new MeHojaptomedDTO();
                pto.Ptomedicodi = ptomedicodi;
                pto.Ptomedidesc = servFormato.GetByIdMePtomedicion(ptomedicodi).Ptomedielenomb != null ? servFormato.GetByIdMePtomedicion(ptomedicodi).Ptomedielenomb : "";

                listaPtos.Add(pto);

                int nCol = listaPtos.Count;
                int horizonte = formato.Formathorizonte;
                Tools.GetSizeFormato(formato);
                this.MatrizExcel = Tools.LeerExcelFile(this.NombreFile, listaPtos, dia);
            }
            Tools.BorrarArchivo(this.NombreFile);
            return Json(retorno);
        }

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
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                string valorEmp = string.Empty;
                if (ws.Cells[1, ConstantesEventos.ColEmpresa].Value != null)
                    valorEmp = ws.Cells[1, ConstantesEventos.ColEmpresa].Value.ToString();
                if (!int.TryParse(valorEmp, NumberStyles.Any, CultureInfo.InvariantCulture, out idEmpresaArchivo))
                    idEmpresa = 0;
                string valorFormato = string.Empty;
                if (ws.Cells[1, ConstantesEventos.ColFormato].Value != null)
                    valorFormato = ws.Cells[1, ConstantesEventos.ColFormato].Value.ToString();
                if (!int.TryParse(valorFormato, NumberStyles.Any, CultureInfo.InvariantCulture, out idFormatoEmpresa))
                    idFormatoEmpresa = 0;
                if (idEmpresaArchivo != idEmpresa)
                {
                    retorno = -1;
                }
                if (idFormatoEmpresa != idFormato)
                {
                    retorno = -2;
                }
            }
            retorno = 1;
            return retorno;
        }

    }
}
