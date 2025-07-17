using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Evento;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using Novacode;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using COES.Servicios.Aplicacion.ReportesFrecuencia;
using COES.Dominio.DTO.ReportesFrecuencia;

namespace COES.Servicios.Aplicacion.Eventos
{

    public class AnalisisFallasAppServicio : AppServicioBase
    {
        CorreoAppServicio servCorreo = new CorreoAppServicio();
        GeneralAppServicio servGeneral = new GeneralAppServicio();
        WordDocument wordDocument = new WordDocument();
        InformacionFrecuenciaAppServicio servRepFrec = new InformacionFrecuenciaAppServicio();

        /// <summary>
        /// Obtener empresa propietaria
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresaPropietaria()
        {
            return FactorySic.ObtenerEventoDao().ObtenerEmpresaPropietaria();
        }
        /// <summary>
        /// Obtener empresa involucrada
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasInvolucrada()
        {
            return FactorySic.ObtenerEventoDao().ObtenerEmpresasInvolucrada();
        }
        /// <summary>
        /// Obtener empresa recomendacion
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasRecomendacion()
        {
            return FactorySic.ObtenerEventoDao().ObtenerEmpresasRecomendacion();
        }
        /// <summary>
        /// Obtener empresa observacora
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasObservacion()
        {
            return FactorySic.ObtenerEventoDao().ObtenerEmpresasObservacion();
        }
        /// <summary>
        /// Obtener tipo de equipo
        /// </summary>
        /// <returns></returns>
        public List<EventoDTO> ObtenerTipoEquipo()
        {
            return FactorySic.ObtenerEventoDao().ObtenerTipoEquipo();
        }
        /// <summary>
        /// Consultar analisis de falla
        /// </summary>
        /// <param name="oEventoDTO"></param>
        /// <returns></returns>
        public List<EventoDTO> ConsultarAnalisisFallas(EventoDTO oEventoDTO)
        {
            return FactorySic.ObtenerEventoDao().ConsultarAnalisisFallas(oEventoDTO);
        }

        /// <summary>
        /// Generar Alertas de Citacion
        /// </summary>
        /// <returns></returns>
        public void GenerarAlertasCitacion(DateTime fecha, int codigoCorreo)
        {
            var codigos = FactorySic.ObtenerEventoDao().ObtenerCodigosEventosPorFechaNominal(fecha.ToString("yyyy-MM-dd"));
            codigos.ForEach(codigo => servCorreo.EnviarCorreoAnalisisFallaAlertaCitacion(codigo, fecha.ToString("dd/MM/yyyy"), codigoCorreo));
        }
        /// <summary>
        /// Generar Alertas Elaboracion Informe Ctaf
        /// </summary>
        /// <returns></returns>
        public void GenerarAlertasElaboracionInformeCtaf(DateTime fecha, int codigoCorreo)
        {
            var codigos = FactorySic.ObtenerEventoDao().ObtenerCodigosEventosPorFechaReunion(fecha.ToString("yyyy-MM-dd"));
            codigos.ForEach(codigo => servCorreo.EnviarCorreoAlertaElaboracionInformeCtaf(codigo, fecha.ToString("dd/MM/yyyy"), codigoCorreo));
        }
        /// <summary>
        /// Generar Alertas Elaboracion Informe Ctaf mas dos dias habiles
        /// </summary>
        /// <returns></returns>
        public void GenerarAlertasElaboracionInformeCtafMasDosDiasHabiles(DateTime fecha, int codigoCorreo)
        {
            var nuevaFecha = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fecha, 2);
            var codigos = FactorySic.ObtenerEventoDao().ObtenerCodigosEventosPorFechaReunion(nuevaFecha.ToString("yyyy-MM-dd"));
            codigos.ForEach(codigo => servCorreo.EnviarCorreoAlertaElaboracionInformeCtafMasDosDiasHabiles(codigo, nuevaFecha.ToString("dd/MM/yyyy"), codigoCorreo));
        }
        /// <summary>
        /// Generar Alertas Elaboracion Informe Ctaf mas dos dias habiles
        /// </summary>
        /// <returns></returns>
        public void GenerarAlertasElaboracionInformeTecnico(DateTime fecha, int codigoCorreo)
        {
            var codigos = FactorySic.ObtenerEventoDao().ObtenerCodigosEventosPorFechaElaboracionInformeTecnico(fecha.ToString("yyyy-MM-dd"));
            codigos.ForEach(codigo => servCorreo.EnviarCorreoAlertaElaboracionInformeTecnico(codigo, fecha.ToString("dd/MM/yyyy"), codigoCorreo));
        }
        /// <summary>
        /// Generar Alertas Datos Frecuencia
        /// </summary>
        /// <returns></returns>
        public void GenerarAlertasDatosFrecuencia(DateTime fecha, int codigoCorreo)
        {
            List<InformacionFrecuenciaDTO> entities = servRepFrec.GetReporteFrecuenciaDesviacion();
            servCorreo.EnviarCorreoAlertaDesviacionFrecuencias(codigoCorreo, entities);
        }

        /// <summary>
        /// Generar Alertas Eventos Frecuencia
        /// </summary>
        /// <returns></returns>
        public void GenerarAlertasEventosFrecuencia(DateTime fecha, int codigoCorreo)
        {
            List<InformacionFrecuenciaDTO> entities = servRepFrec.GetReporteEventosFrecuencia();
            servCorreo.EnviarCorreoAlertaEventosFrecuencias(codigoCorreo, entities);
        }

        /// <summary>
        /// Generar Alertas Eventos Frecuencia
        /// </summary>
        /// <returns></returns>
        public void GenerarAlertasRepSegFaltantes(DateTime fecha, int codigoCorreo)
        {
            List<EquipoGPSDTO> listaGPS = new List<EquipoGPSDTO>();
            listaGPS = new EquipoGPSAppServicio().GetListaEquipoGPSPorFiltro(0, string.Empty);

            ReporteSegundosFaltantesParam param = new ReporteSegundosFaltantesParam();

            DateTime date = DateTime.Now;
            //model.FechaInicial = date;
            int intAnio = 0;
            int intMes = 0;
            int intDia = 0;
            intAnio = date.Year;
            intMes = date.Month;
            intDia = date.Day;
            string strAnio = intAnio.ToString();
            string strMes = intMes.ToString();
            string strDia = intDia.ToString();

            if (intMes < 10)
            {
                strMes = "0" + strMes;
            }
            if (intDia < 10)
            {
                strDia = "0" + strDia;
            }

            string strFechaActual = strDia + "/" + strMes + "/" + strAnio;
            DateTime dtfechaActual = DateTime.ParseExact(strFechaActual, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime fechaActualAnt = dtfechaActual.AddDays(-1);
            int intAnioAnt = fechaActualAnt.Year;
            int intMesAnt = fechaActualAnt.Month;
            int intDiaAnt = fechaActualAnt.Day;
            string strAnioAnt = intAnioAnt.ToString();
            string strMesAnt = intMesAnt.ToString();
            string strDiaAnt = intDiaAnt.ToString();
            if (intMesAnt < 10)
            {
                strMesAnt = "0" + strMesAnt;
            }
            if (intDiaAnt < 10)
            {
                strDiaAnt = "0" + strDiaAnt;

            }
            string strFechaAnt = strDiaAnt + "/" + strMesAnt + "/" + strAnioAnt;

            //model.FechaInicial = new DateTime(intAnio, intMes, 1, 0, 0, 0);

            param.FechaInicial = strFechaAnt;
            param.FechaFinal = strFechaAnt;
            param.IdGPS = 0;
            //param.IndOficial = model.IndOficial;
            //listaReporteSegundosFaltantes = new ReporteSegundosFaltantesAppServicio().GetReporteSegundosFaltantes(param);
            List<ReporteSegundosFaltantesDTO> listaReporteSegundosFaltantes = new List<ReporteSegundosFaltantesDTO>();
            listaReporteSegundosFaltantes = new ReporteSegundosFaltantesAppServicio().GetReporteTotalSegundosFaltantes(param);


            servCorreo.EnviarCorreoAlertaReporteSegFaltantes(codigoCorreo, listaGPS, listaReporteSegundosFaltantes);
        }
        /// <summary>
        /// Generar Alertas Elaboracion Informe Ctaf mas dos dias habiles
        /// </summary>
        /// <returns></returns>
        public void GenerarAlertasElaboracionInformeTecnicoMasDiasHabiles(DateTime fecha, int dias, int codigoCorreo)
        {
            var nuevaFecha = FactorySic.GetDocDiaEspRepository().ObtenerFechaDiasHabiles(fecha, dias);
            var codigos = FactorySic.ObtenerEventoDao().ObtenerCodigosEventosPorFechaElaboracionInformeTecnico(nuevaFecha.ToString("yyyy-MM-dd"));
            codigos.ForEach(codigo => servCorreo.EnviarCorreoAlertaElaboracionInformeTecnicoMasDiasHabiles(codigo, nuevaFecha.ToString("dd/MM/yyyy"), codigoCorreo));
        }
        /// <summary>
        /// Generar Alertas Elaboracion Informe Ctaf mas dos dias habiles
        /// </summary>
        /// <returns></returns>
        public void GenerarAlertasElaboracionInformeTecnicoSemanal(string fechaInicio, string fechaFin, int codigoCorreo)
        {
            var codigosCitacionSemanal = FactorySic.ObtenerEventoDao().ObtenerCodigosEventosPorFechaNominalSemanal(fechaInicio, fechaFin).DistinctBy(x => new { x.CODIGO, x.FECHA }).ToList();
            var codigosCtafSemanal = FactorySic.ObtenerEventoDao().ObtenerCodigosEventosPorFechaReunionSemanal(fechaInicio, fechaFin).DistinctBy(x => new { x.CODIGO, x.FECHA }).ToList();
            var codigosElaboracionInformeTecnicoSemanal = FactorySic.ObtenerEventoDao().ObtenerCodigosEventosPorFechaElaboracionInformeTecnicoSemanal(fechaInicio, fechaFin).DistinctBy(x => new { x.CODIGO, x.FECHA }).ToList();
            servCorreo.EnviarCorreoAlertaElaboracionInformeTecnicoSemanal(codigosCitacionSemanal, codigosCtafSemanal, codigosElaboracionInformeTecnicoSemanal, codigoCorreo);
        }
        /// <summary>
        /// Buscar empresa
        /// </summary>
        /// <param name="nombreempresa"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> BuscarEmpresa(string nombreempresa)
        {
            return FactorySic.ObtenerEventoDao().BuscarEmpresa(nombreempresa);
        }
        /// <summary>
        /// Obtener empresa
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresa()
        {
            return FactorySic.ObtenerEventoDao().ObtenerEmpresa();
        }
        /// <summary>
        /// Buscar empresa propietaria
        /// </summary>
        /// <param name="nombreempresa"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> BuscarEmpresaPropietaria(string nombreempresa)
        {
            return FactorySic.ObtenerEventoDao().BuscarEmpresaPropietaria(nombreempresa);
        }
        /// <summary>
        /// Obtener responsable del evento
        /// </summary>
        /// <returns></returns>
        public List<string> ObtenerResponsableEvento()
        {
            return FactorySic.ObtenerEventoDao().ObtenerResponsableEvento();
        }

        /// <summary>
        /// Obtener responsable de reunion
        /// </summary>
        /// <returns></returns>
        public List<ReunionResponsableDTO> ObtenerReunionResponsable()
        {
            return FactorySic.ObtenerEventoDao().ObtenerReunionResponsable();
        }
        /// <summary>
        /// Obtener evento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventoDTO ObtenerEvento(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerEvento(id);
        }
        /// <summary>
        /// Obtener análisis de falla
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AnalisisFallaDTO ObtenerAnalisisFalla(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerAnalisisFalla(id);
        }
        /// <summary>
        /// Obtener análisis de falla N2
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AnalisisFallaDTO ObtenerAnalisisFalla2(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerAnalisisFalla2(id);
        }
        /// <summary>
        /// Obtener análisis de falla completo
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<AnalisisFallaDTO> ObtenerAnalisisFallaCompleto(DateTime fecha)
        {
            return FactorySic.ObtenerEventoDao().ObtenerAnalisisFallaCompleto(fecha);
        }
        /// <summary>
        /// Obtener análisis de falla completo
        /// </summary>
        /// <returns></returns>
        public List<AnalisisFallaDTO> ObtenerAnalisisFallaCompleto()
        {
            return FactorySic.ObtenerEventoDao().ObtenerAnalisisFallaCompleto();
        }
        /// <summary>
        /// Obtener equipo por evento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EquipoDTO ObtenerEquipoPorEvento(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerEquipoPorEvento(id);
        }
        /// <summary>
        /// Obtener empresa involucrada por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<EmpresaInvolucradaDTO> ObtenerEmpresasInvolucrada(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerEmpresasInvolucrada(id);
        }
        /// <summary>
        /// Obtener empresa involucrada reunión
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<EmpresaInvolucradaDTO> ObtenerEmpresasInvolucradaReunion(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerEmpresasInvolucradaReunion(id);
        }

        /// <summary>
        /// Obtener empresa involucrada reunión
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ReunionResponsableDTO> ObtenerListaReunionResponsable(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerListaReunionResponsable(id);
        }

        /// <summary>
        /// Obtener secuencia eventos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SecuenciaEventoDTO> ObtenerListaSecuenciaEventos(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerListaSecuenciaEventos(id);
        }
        /// <summary>
        /// Obtener empresa para informe técnico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<EmpresaRecomendacionDTO> ObtenerEmpresaRecomendacionInformeTecnico(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerEmpresaRecomendacionInformeTecnico(id);
        }
        /// <summary>
        /// Obtener empresa para observación de informe técnico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<EmpresaObservacionDTO> ObtenerEmpresaObservacionInformeTecnico(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerEmpresaObservacionInformeTecnico(id);
        }
        /// <summary>
        /// Obtener empresa para informe técnico fuerza mayor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<EmpresaFuerzaMayorDTO> ObtenerEmpresaFuerzaMayorInformeTecnico(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerEmpresaFuerzaMayorInformeTecnico(id);
        }
        /// <summary>
        /// Obtener empresa responsable
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<EmpresaResponsableDTO> ObtenerEmpresaResponsableCompensacion(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerEmpresaResponsableCompensacion(id);
        }
        /// <summary>
        /// Obtener empresa compensada
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<EmpresaResponsableDTO> ObtenerEmpresaCompensadaCompensacion(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerEmpresaCompensadaCompensacion(id);
        }
        /// <summary>
        /// Obtener reclamo de reconsideración
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ReclamoDTO> ObtenerReclamoReconsideracionReconsideracion(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerReclamoReconsideracionReconsideracion(id);
        }
        /// <summary>
        /// Obtener reclamo apelación
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ReclamoDTO> ObtenerReclamoApelacionReconsideracion(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerReclamoApelacionReconsideracion(id);
        }
        /// <summary>
        /// Obtener reclamo arbitraje reconsideración
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ReclamoDTO> ObtenerReclamoArbitrajeReconsideracion(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerReclamoArbitrajeReconsideracion(id);
        }
        /// <summary>
        /// Insertar empresa involucrada
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool InsertarEmpresaInvolucrada(EmpresaInvolucradaDTO obj)
        {
            return FactorySic.ObtenerEventoDao().InsertarEmpresaInvolucrada(obj);
        }

        /// <summary>
        /// Insertar secuencia de evento 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool InsertarSecuenciaEvento(SecuenciaEventoDTO obj)
        {
            return FactorySic.ObtenerEventoDao().InsertarSecuenciaEvento(obj);
        }

        /// <summary>
        /// Insertar empresa involucrada
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool InsertarReunionResponsable(ReunionResponsableDTO obj)
        {
            return FactorySic.ObtenerEventoDao().InsertarReunionResponsable(obj);
        }
        /// <summary>
        /// Obtener empresa para informe técnico
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<EmpresaRecomendacionDTO> ConsultarRecomendacion(EmpresaRecomendacionDTO obj)
        {
            return FactorySic.ObtenerEventoDao().ConsultarRecomendacion(obj);
        }
        /// <summary>
        /// Consultar recomendación
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<EmpresaObservacionDTO> ConsultarObservacion(EmpresaObservacionDTO obj)
        {
            return FactorySic.ObtenerEventoDao().ConsultarObservacion(obj);
        }
        /// <summary>
        /// Ekunubar enoresa involucrada
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool EliminarEmpresaInvolucrada(EmpresaInvolucradaDTO obj)
        {
            return FactorySic.ObtenerEventoDao().EliminarEmpresaInvolucrada(obj);
        }
        /// <summary>
        /// Ekunubar secuencia evento
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool EliminarSecuenciaEvento(int id)
        {
            return FactorySic.ObtenerEventoDao().EliminarSecuenciaEvento(id);
        }
        /// <summary>
        /// Eliminar empresa involucrada reunión
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool EliminarEmpresaInvolucradaReunion(EmpresaInvolucradaDTO obj)
        {
            return FactorySic.ObtenerEventoDao().EliminarEmpresaInvolucradaReunion(obj);
        }
        /// <summary>
        /// Eliminar Responsable de reunion
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool EliminarReunionResponsable(ReunionResponsableDTO obj)
        {
            return FactorySic.ObtenerEventoDao().EliminarReunionResponsable(obj);
        }

        /// <summary>
        /// Eliminar Asistente
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool EliminarAsistenteResponsable(ReunionResponsableDTO obj)
        {
            return FactorySic.ObtenerEventoDao().EliminarAsistenteResponsable(obj);
        }

        /// <summary>
        /// Actualizar fecha convocatoria 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool ActualizarFechaConvocatoriaCitacionReunion(int afecodi, string valor)
        {
            return FactorySic.ObtenerEventoDao().ActualizarFechaConvocatoriaCitacionReunion(afecodi, valor);
        }
        /// <summary>
        /// Actualizar fecha acta de reunión
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="valor"></param>
        /// <returns></returns>

        public bool ActualizarFechaActaReunion(int afecodi, string valor)
        {
            return FactorySic.ObtenerEventoDao().ActualizarFechaActaReunion(afecodi, valor);
        }
        /// <summary>
        /// Actualizar fecha informe
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public bool ActualizarFechaInformeCTAFReunion(int afecodi, string valor)
        {
            return FactorySic.ObtenerEventoDao().ActualizarFechaInformeCTAFReunion(afecodi, valor);
        }
        /// <summary>
        /// Existe recomendación
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExisteRecomendacionInformeTecnico(EmpresaRecomendacionDTO entity)
        {
            return FactorySic.ObtenerEventoDao().ExisteRecomendacionInformeTecnico(entity);
        }
        /// <summary>
        /// Verifica existe empresa 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExisteEmpresaResponsableCompensacion(EmpresaResponsableDTO entity)
        {
            return FactorySic.ObtenerEventoDao().ExisteEmpresaResponsableCompensacion(entity);
        }
        /// <summary>
        /// Verifica existe empresa compensada
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExisteEmpresaCompensadaCompensacion(EmpresaResponsableDTO entity)
        {
            return FactorySic.ObtenerEventoDao().ExisteEmpresaCompensadaCompensacion(entity);
        }

        /// <summary>
        /// Verifica existe responsable de reunion
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExisteResponsableReunion(ReunionResponsableDTO entity)
        {
            return FactorySic.ObtenerEventoDao().ExisteResponsableReunion(entity);
        }

        /// <summary>
        /// Verifica existe responsable de reunion
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExisteAsistenteResponsable(ReunionResponsableDTO entity)
        {
            return FactorySic.ObtenerEventoDao().ExisteAsistenteResponsable(entity);
        }

        /// <summary>
        /// Inserta recomendación
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertarRecomendacionInformeTecnico(EmpresaRecomendacionDTO entity)
        {
            return FactorySic.ObtenerEventoDao().InsertarRecomendacionInformeTecnico(entity);
        }
        /// <summary>
        /// Elimina recomendación
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EliminarRecomendacionInformeTecnico(EmpresaRecomendacionDTO entity)
        {
            return FactorySic.ObtenerEventoDao().EliminarRecomendacionInformeTecnico(entity);
        }
        /// <summary>
        /// Inserta observación
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertarObservacionInformeTecnico(EmpresaObservacionDTO entity)
        {
            return FactorySic.ObtenerEventoDao().InsertarObservacionInformeTecnico(entity);
        }
        /// <summary>
        /// Elimina observación
        /// </summary>
        /// <param name="AFOOBS"></param>
        /// <returns></returns>
        public bool EliminarObservacionInformeTecnico(int AFOOBS)
        {
            return FactorySic.ObtenerEventoDao().EliminarObservacionInformeTecnico(AFOOBS);
        }
        /// <summary>
        /// Actualizar publicación
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="AFEITPITFFECHA"></param>
        /// <param name="AFEITPITFFECHASIST"></param>
        /// <returns></returns>
        public bool ActualizarPublicacionInformeTecnicoAnualInformeTecnico(int afecodi, string AFEITPITFFECHA, string AFEITPITFFECHASIST)
        {
            return FactorySic.ObtenerEventoDao().ActualizarPublicacionInformeTecnicoAnualInformeTecnico(afecodi, AFEITPITFFECHA, AFEITPITFFECHASIST);
        }
        /// <summary>
        /// Actualizar publicación evento informe técnico
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="AFEITPDECISFFECHASIST"></param>
        /// <returns></returns>
        public bool ActualizarPublicacionDesicionEventoInformeTecnico(int afecodi, string AFEITPDECISFFECHASIST)
        {
            return FactorySic.ObtenerEventoDao().ActualizarPublicacionDesicionEventoInformeTecnico(afecodi, AFEITPDECISFFECHASIST);
        }
        /// <summary>
        /// Inserta fuerza mayor
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertarFuerzaMayorInformeTecnico(EmpresaFuerzaMayorDTO entity)
        {
            return FactorySic.ObtenerEventoDao().InsertarFuerzaMayorInformeTecnico(entity);
        }
        /// <summary>
        /// Actualiza fuerza mayor
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ActualizarFuerzaMayorInformeTecnico(EmpresaFuerzaMayorDTO entity)
        {
            return FactorySic.ObtenerEventoDao().ActualizarFuerzaMayorInformeTecnico(entity);
        }
        /// <summary>
        /// Eliminar fuerza mayot
        /// </summary>
        /// <param name="RECLAMOCODI"></param>
        /// <returns></returns>
        public bool EliminarFuerzaMayorInformeTecnico(int RECLAMOCODI)
        {
            return FactorySic.ObtenerEventoDao().EliminarFuerzaMayorInformeTecnico(RECLAMOCODI);
        }
        /// <summary>
        /// Insertar empresa 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertarEmpresaResponsableCompensacion(EmpresaResponsableDTO entity)
        {
            return FactorySic.ObtenerEventoDao().InsertarEmpresaResponsableCompensacion(entity);
        }
        /// <summary>
        /// Eliminar empresa responsable compensación
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EliminarEmpresaResponsableCompensacion(EmpresaResponsableDTO entity)
        {
            return FactorySic.ObtenerEventoDao().EliminarEmpresaResponsableCompensacion(entity);
        }
        /// <summary>
        /// Actualizar informe compensaciones
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="AFECOMPFECHASIST"></param>
        /// <param name="AFECOMPFECHA"></param>
        /// <returns></returns>
        public bool ActualizarInformeCompensaciones(int afecodi, string AFECOMPFECHASIST, string AFECOMPFECHA)
        {
            return FactorySic.ObtenerEventoDao().ActualizarInformeCompensaciones(afecodi, AFECOMPFECHASIST, AFECOMPFECHA);
        }
        /// <summary>
        /// Inserta reclamo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertarReclamoRecApe(ReclamoDTO entity)
        {
            return FactorySic.ObtenerEventoDao().InsertarReclamoRecApe(entity);
        }
        /// <summary>
        /// Eliminar reclamo rec
        /// </summary>
        /// <param name="reclamocodi"></param>
        /// <returns></returns>
        public bool EliminarReclamoRecApe(int reclamocodi)
        {
            return FactorySic.ObtenerEventoDao().EliminarReclamoRecApe(reclamocodi);
        }
        /// <summary>
        /// Actualizar reclamo rec
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ActualizarReclamoRecApe(ReclamoDTO entity)
        {
            return FactorySic.ObtenerEventoDao().ActualizarReclamoRecApe(entity);
        }
        /// <summary>
        /// Actualizar evento
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ActualizarEvento(AnalisisFallaDTO entity)
        {
            return FactorySic.ObtenerEventoDao().ActualizarEvento(entity);
        }
        /// <summary>
        /// Obtener medidas adoptadas por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public AnalisisFallaDTO ObtenerMedidasAdoptadas(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerMedidasAdoptadas(id);
        }
        /// <summary>
        /// Actualizar recomedación MA
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ActualizarRecomendacionMA(AnalisisFallaDTO entity)
        {
            return FactorySic.ObtenerEventoDao().ActualizarRecomendacionMA(entity);
        }
        /// <summary>
        /// Obtener medidas adoptadas por id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ActualizarRecomendacionMAG(AnalisisFallaDTO entity)
        {
            return FactorySic.ObtenerEventoDao().ActualizarRecomendacionMAG(entity);
        }
        /// <summary>
        /// Actualizar Carta RecomendacionCOES
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ActualizarCartaRecomendacionCOES(AnalisisFallaDTO entity)
        {
            return FactorySic.ObtenerEventoDao().ActualizarCartaRecomendacionCOES(entity);
        }
        /// <summary>
        /// Actualizar Carta Respuesta
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ActualizarCartaRespuesta(AnalisisFallaDTO entity)
        {
            return FactorySic.ObtenerEventoDao().ActualizarCartaRespuesta(entity);
        }
        /// <summary>
        /// Obtener CTAF INFORME REPORTE
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<InformeCTAFDTO> ObtenerCTAFINFORMEREPORTE(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerCTAFINFORMEREPORTE(id);
        }

        /// <summary>
        /// Obtener Secuencia Evento REPORTE
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SecuenciaCTAFDTO> ObtenerSecuenciaEventoREPORTE(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerSecuenciaEventoREPORTE(id);
        }
        /// <summary>
        /// Obtener Secuencia Evento REPORTE
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SecuenciaCTAFDTO> ObtenerSecuenciaEventoREPORTEv2(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerSecuenciaEventoREPORTEv2(id);
        }
        /// <summary>
        /// Obtener Secuencia Evento REPORTE V3
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SecuenciaCTAFDTO> ObtenerSecuenciaEventoREPORTEv3(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerSecuenciaEventoREPORTEv3(id);
        }
        /// <summary>
        /// Obtener Senalizacion REPORTE
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SenalizacionCTAFDTO> ObtenerSenalizacionREPORTE(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerSenalizacionREPORTE(id);
        }
        /// <summary>
        /// Obtener Suministro REPORTE
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SuministroCTAFDTO> ObtenerSuministroREPORTE(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerSuministroREPORTE(id);
        }
        /// <summary>
        /// Obtener Evento Citacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<EventoDTO> ObtenerEventoCitacion(int id)
        {
            return FactorySic.ObtenerEventoDao().ObtenerEventoCitacion(id);
        }
        /// <summary>
        ///  Existe IEI
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExisteIEI(EmpresaInvolucradaDTO entity)
        {
            return FactorySic.ObtenerEventoDao().ExisteIEI(entity);
        }

        #region FIT SGOCOES func A - Soporte
        /// <summary>
        /// Valida Correlativo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ValidaCorrelativo(AnalisisFallaDTO entity)
        {
            return FactorySic.ObtenerEventoDao().ValidaCorrelativo(entity);
        }
        /// <summary>
        /// Valida Respuesta COES
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="tipcodi"></param>
        /// <param name="rptacodi"></param>
        /// <returns></returns>
        public int ValidaRespuestaCOES(int afecodi, int tipcodi, int rptacodi)
        {
            return FactorySic.ObtenerEventoDao().ValidaRespuestaCOES(afecodi, tipcodi, rptacodi);
        }
        /// <summary>
        /// Valida Respuesta COES1
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="tipcodi"></param>
        /// <param name="rptacodi"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public int ValidaRespuestaCOES1(int afecodi, int tipcodi, int rptacodi, int emprcodi)
        {
            return FactorySic.ObtenerEventoDao().ValidaRespuestaCOES1(afecodi, tipcodi, rptacodi, emprcodi);
        }
        /// <summary>
        /// Eliminar Empresa Compensada Compensacion
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EliminarEmpresaCompensadaCompensacion(EmpresaResponsableDTO entity)
        {
            return FactorySic.ObtenerEventoDao().EliminarEmpresaCompensadaCompensacion(entity);
        }

        #endregion

        #region Aplicativo Extranet CTAF

        #region Métodos Tabla AF_CONDICIONES

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AnalisisFallasAppServicio));

        /// <summary>
        /// Inserta un registro de la tabla AF_CONDICIONES
        /// </summary>
        public void SaveAfCondiciones(AfCondicionesDTO entity)
        {
            try
            {
                FactorySic.GetAfCondicionesRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AF_CONDICIONES
        /// </summary>
        public void UpdateAfCondiciones(AfCondicionesDTO entity)
        {
            try
            {
                FactorySic.GetAfCondicionesRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AF_CONDICIONES
        /// </summary>
        public void DeleteAfCondiciones(int afcondcodi)
        {
            try
            {
                FactorySic.GetAfCondicionesRepository().Delete(afcondcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AF_CONDICIONES
        /// </summary>
        public AfCondicionesDTO GetByIdAfCondiciones(int afcondcodi)
        {
            return FactorySic.GetAfCondicionesRepository().GetById(afcondcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AF_CONDICIONES
        /// </summary>
        public List<AfCondicionesDTO> ListAfCondicioness(int afecodi, int evencodi = 0)
        {
            var lista = FactorySic.GetAfCondicionesRepository().List().Where(x => x.Afecodi == afecodi).ToList();

            lista = lista.GroupBy(x => new { x.Afcondzona, x.Afcondnumetapa, x.Afcondfuncion })
                    .Select(x => new AfCondicionesDTO()
                    {
                        Afecodi = x.First().Afecodi,
                        Afcondcodi = x.First().Afcondcodi,
                        Afcondzona = x.Key.Afcondzona,
                        Afcondnumetapa = x.Key.Afcondnumetapa,
                        Afcondfuncion = x.Key.Afcondfuncion
                    }).ToList();

            foreach (var reg in lista)
            {
                reg.AfcondzonaDesc = "Zona " + reg.Afcondzona;
                reg.Afcondnumetapadescrip = "Etapa " + reg.Afcondnumetapa;
                reg.Evencodi = evencodi.ToString();
            }

            return lista;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AfCondiciones
        /// </summary>
        public List<AfCondicionesDTO> GetByCriteriaAfCondicioness()
        {
            return FactorySic.GetAfCondicionesRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla AF_EMPRESA

        /// <summary>
        /// Inserta un registro de la tabla AF_EMPRESA
        /// </summary>
        public void SaveAfEmpresa(AfEmpresaDTO entity)
        {
            try
            {
                FactorySic.GetAfEmpresaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AF_EMPRESA
        /// </summary>
        public void UpdateAfEmpresa(AfEmpresaDTO entity)
        {
            try
            {
                FactorySic.GetAfEmpresaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AF_EMPRESA
        /// </summary>
        public void DeleteAfEmpresa(int afemprcodi)
        {
            try
            {
                FactorySic.GetAfEmpresaRepository().Delete(afemprcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AF_EMPRESA
        /// </summary>
        public AfEmpresaDTO GetByIdAfEmpresa(int afemprcodi)
        {
            return FactorySic.GetAfEmpresaRepository().GetById(afemprcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AF_EMPRESA
        /// </summary>
        public List<AfEmpresaDTO> ListAfEmpresas()
        {
            var lista = FactorySic.GetAfEmpresaRepository().List();
            foreach (var item in lista)
            {
                item.Afemprnomb = item.Afemprnomb != null ? item.Afemprnomb.Trim() : string.Empty;
                item.Afemprosinergmin = item.Afemprosinergmin != null ? item.Afemprosinergmin.Trim() : string.Empty;
            }

            return lista;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AfEmpresa
        /// </summary>
        public List<AfEmpresaDTO> GetByCriteriaAfEmpresas()
        {
            return FactorySic.GetAfEmpresaRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla AF_ERACMF_EVENTO

        /// <summary>
        /// Inserta un registro de la tabla AF_ERACMF_EVENTO
        /// </summary>
        public void SaveAfEracmfEvento(AfEracmfEventoDTO entity)
        {
            try
            {
                FactorySic.GetAfEracmfEventoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AF_ERACMF_EVENTO
        /// </summary>
        public void UpdateAfEracmfEvento(AfEracmfEventoDTO entity)
        {
            try
            {
                FactorySic.GetAfEracmfEventoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AF_ERACMF_EVENTO
        /// </summary>
        public void DeleteAfEracmfEvento(int eracmfcodi)
        {
            try
            {
                FactorySic.GetAfEracmfEventoRepository().Delete(eracmfcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AF_ERACMF_EVENTO
        /// </summary>
        public AfEracmfEventoDTO GetByIdAfEracmfEvento(int eracmfcodi)
        {
            return FactorySic.GetAfEracmfEventoRepository().GetById(eracmfcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AF_ERACMF_EVENTO
        /// </summary>
        public List<AfEracmfEventoDTO> ListAfEracmfEventos()
        {
            return FactorySic.GetAfEracmfEventoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AfEracmfEvento
        /// </summary>
        public List<AfEracmfEventoDTO> GetByCriteriaAfEracmfEventos()
        {
            return FactorySic.GetAfEracmfEventoRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AF_ERACMF_EVENTO de acuerdo con su EVENCODI
        /// </summary>
        public List<AfEracmfEventoDTO> GetByEvencodiAfEracmfEventos(int evencodi)
        {
            var lista = FactorySic.GetAfEracmfEventoRepository().GetByEvencodi(evencodi);

            foreach (var item in lista)
            {
                item.Eracmfemprnomb = item.Eracmfemprnomb != null ? item.Eracmfemprnomb.Trim() : string.Empty;
            }

            return lista;
        }

        #endregion

        #region Métodos Tabla AF_ERACMF_ZONA

        /// <summary>
        /// Inserta un registro de la tabla AF_ERACMF_ZONA
        /// </summary>
        public void SaveAfEracmfZona(AfEracmfZonaDTO entity)
        {
            try
            {
                FactorySic.GetAfEracmfZonaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AF_ERACMF_ZONA
        /// </summary>
        public void UpdateAfEracmfZona(AfEracmfZonaDTO entity)
        {
            try
            {
                FactorySic.GetAfEracmfZonaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AF_ERACMF_ZONA
        /// </summary>
        public void DeleteAfEracmfZona(int aferaccodi)
        {
            try
            {
                FactorySic.GetAfEracmfZonaRepository().Delete(aferaccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AF_ERACMF_ZONA
        /// </summary>
        public AfEracmfZonaDTO GetByIdAfEracmfZona(int aferaccodi)
        {
            return FactorySic.GetAfEracmfZonaRepository().GetById(aferaccodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AF_ERACMF_ZONA
        /// </summary>
        public List<AfEracmfZonaDTO> ListAfEracmfZonas()
        {
            return FactorySic.GetAfEracmfZonaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AfEracmfZona
        /// </summary>
        public List<AfEracmfZonaDTO> GetByCriteriaAfEracmfZonas()
        {
            return FactorySic.GetAfEracmfZonaRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla AF_EVENTO

        /// <summary>
        /// Inserta un registro de la tabla AF_EVENTO
        /// </summary>
        public void SaveAfEvento(AfEventoDTO entity)
        {
            try
            {
                FactorySic.GetAfEventoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AF_EVENTO
        /// </summary>
        public void UpdateAfEvento(AfEventoDTO entity)
        {
            try
            {
                FactorySic.GetAfEventoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AF_EVENTO
        /// </summary>
        public void DeleteAfEvento(int afecodi)
        {
            try
            {
                FactorySic.GetAfEventoRepository().Delete(afecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AF_EVENTO
        /// </summary>
        public AfEventoDTO GetByIdAfEvento(int afecodi)
        {
            return FactorySic.GetAfEventoRepository().GetById(afecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AF_EVENTO
        /// </summary>
        public List<AfEventoDTO> ListAfEventos()
        {
            return FactorySic.GetAfEventoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AfEvento
        /// </summary>
        public List<AfEventoDTO> GetByCriteriaAfEventos()
        {
            return FactorySic.GetAfEventoRepository().GetByCriteria();
        }

        /// <summary>
        /// Obtener Evento Por Id
        /// </summary>
        /// <param name="evencodi"></param>
        /// <returns></returns>
        public EveEventoDTO ObtenerEventoPorId(int evencodi)
        {
            return FactorySic.GetEveEventoRepository().GetDetalleEvento(evencodi);
        }

        #endregion

        #region Métodos Tabla AF_HORA_COORD

        /// <summary>
        /// Inserta un registro de la tabla AF_HORA_COORD
        /// </summary>
        public void SaveAfHoraCoord(AfHoraCoordDTO entity)
        {
            try
            {
                FactorySic.GetAfHoraCoordRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AF_HORA_COORD
        /// </summary>
        public void UpdateAfHoraCoord(AfHoraCoordDTO entity)
        {
            try
            {
                FactorySic.GetAfHoraCoordRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AF_HORA_COORD
        /// </summary>
        public void DeleteAfHoraCoord(int afhocodi)
        {
            try
            {
                FactorySic.GetAfHoraCoordRepository().Delete(afhocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AF_HORA_COORD
        /// </summary>
        public AfHoraCoordDTO GetByIdAfHoraCoord(int afhocodi)
        {
            return FactorySic.GetAfHoraCoordRepository().GetById(afhocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AF_HORA_COORD
        /// </summary>
        public List<AfHoraCoordDTO> ListAfHoraCoords()
        {
            return FactorySic.GetAfHoraCoordRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AfHoraCoord
        /// </summary>
        public List<AfHoraCoordDTO> GetByCriteriaAfHoraCoords()
        {
            return FactorySic.GetAfHoraCoordRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AF_HORA_COORD por afecodi
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="fdatcodi"></param>
        /// <returns></returns>
        public List<AfHoraCoordDTO> ListAfHoraCoords(int afecodi, int fdatcodi)
        {
            return FactorySic.GetAfHoraCoordRepository().ListHoraCoord(afecodi, fdatcodi);
        }
        /// <summary>
        /// Permite listar todos los registros de la tabla AF_HORA_COORD por afecodi
        /// </summary>
        /// <param name="afecodi"></param>
        /// <returns></returns>
        public List<AfHoraCoordDTO> ListAfHoraCoordsCtaf(string anio, string correlativo, int fdatcodi)
        {
            return FactorySic.GetAfHoraCoordRepository().ListHoraCoordCtaf(anio, correlativo, fdatcodi);
        }
        #endregion

        #region Métodos Tabla AF_INTERRUP_SUMINISTRO

        /// <summary>
        /// Inserta un registro de la tabla AF_INTERRUP_SUMINISTRO
        /// </summary>
        public void SaveAfInterrupSuministro(AfInterrupSuministroDTO entity)
        {
            try
            {
                FactorySic.GetAfInterrupSuministroRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AF_INTERRUP_SUMINISTRO
        /// </summary>
        public void UpdateAfInterrupSuministro(AfInterrupSuministroDTO entity)
        {
            try
            {
                FactorySic.GetAfInterrupSuministroRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AF_INTERRUP_SUMINISTRO
        /// </summary>
        public void DeleteAfInterrupSuministro(int intsumcodi)
        {
            try
            {
                FactorySic.GetAfInterrupSuministroRepository().Delete(intsumcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AF_INTERRUP_SUMINISTRO
        /// </summary>
        public AfInterrupSuministroDTO GetByIdAfInterrupSuministro(int intsumcodi)
        {
            return FactorySic.GetAfInterrupSuministroRepository().GetById(intsumcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AF_INTERRUP_SUMINISTRO
        /// </summary>
        public List<AfInterrupSuministroDTO> ListAfInterrupSuministros()
        {
            return FactorySic.GetAfInterrupSuministroRepository().List();
        }

        #endregion

        #region Métodos Tabla AF_SOLICITUD_RESP

        /// <summary>
        /// Inserta un registro de la tabla AF_SOLICITUD_RESP
        /// </summary>
        public int SaveAfSolicitudResp(AfSolicitudRespDTO entity)
        {
            try
            {
                var id = FactorySic.GetAfSolicitudRespRepository().Save(entity);
                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AF_SOLICITUD_RESP
        /// </summary>
        public int UpdateAfSolicitudResp(AfSolicitudRespDTO entity)
        {
            try
            {
                var id = FactorySic.GetAfSolicitudRespRepository().Update(entity);
                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AF_SOLICITUD_RESP
        /// </summary>
        public void DeleteAfSolicitudResp(int sorespcodi)
        {
            try
            {
                FactorySic.GetAfSolicitudRespRepository().Delete(sorespcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AF_SOLICITUD_RESP
        /// </summary>
        public AfSolicitudRespDTO GetByIdAfSolicitudResp(int sorespcodi)
        {
            var reg = FactorySic.GetAfSolicitudRespRepository().GetById(sorespcodi);
            this.FormatearObjAfSolicitudResp(reg);
            return reg;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AfSolicitudResp
        /// </summary>
        public List<AfSolicitudRespDTO> GetByCriteriaAfSolicitudResps()
        {
            return FactorySic.GetAfSolicitudRespRepository().GetByCriteria();
        }

        #endregion

        #region Carga de Datos Interrupciones de Suministros Extranet

        /// <summary>
        /// Obtener Nombre Archivo Formato InterruSumini
        /// </summary>
        /// <param name="eveEventoDTO"></param>
        /// <param name="rutaBase"></param>
        /// <param name="idtipoinformacion"></param>
        /// <param name="emprcodi"></param>
        /// <param name="afecodi"></param>
        /// <param name="listaInterrupcionSuministro"></param>
        /// <returns></returns>
        public string ObtenerNombreArchivoFormatoInterruSumini(EventoDTO eveEventoDTO, string rutaBase, int idtipoinformacion, int emprcodi, int afecodi, List<AfInterrupSuministroDTO> listaInterrupcionSuministro)
        {
            string nombPlantilla = string.Empty;
            string nombArchivo = string.Empty;
            switch (idtipoinformacion)
            {
                case (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF:
                    nombPlantilla = ConstantesExtranetCTAF.FormatoInterrupciónPorActivaciónERACMF;
                    nombArchivo = "FORMATO INTERRUPCIÓN POR ACTIVACIÓN ERACMF";
                    break;
                case (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion:
                    nombPlantilla = ConstantesExtranetCTAF.FormatoInterrupciones;
                    nombArchivo = "FORMATO INTERRUPCIONES";
                    break;
                case (int)ConstantesExtranetCTAF.Fuentedato.ReduccionSuministros:
                    nombPlantilla = ConstantesExtranetCTAF.FormatoReducciónDeSuministros;
                    nombArchivo = "FORMATO REDUCCIÓN DE SUMINISTROS";
                    break;
            }

            return ObtenerExcelInterrupcionSuministro(rutaBase, nombPlantilla, nombArchivo, eveEventoDTO, idtipoinformacion, emprcodi, afecodi, listaInterrupcionSuministro);
        }

        /// <summary>
        /// Obtener Excel Interrupcion Suministro
        /// </summary>
        /// <param name="rutaBase"></param>
        /// <param name="nombPlantilla"></param>
        /// <param name="nombArchivoFormato"></param>
        /// <param name="eveEventoDTO"></param>
        /// <param name="fdatcodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="afecodi"></param>
        /// <param name="lstInterrSuministro"></param>
        /// <returns></returns>
        private string ObtenerExcelInterrupcionSuministro(string rutaBase, string nombPlantilla, string nombArchivoFormato, EventoDTO eveEventoDTO
            , int fdatcodi, int emprcodi, int afecodi, List<AfInterrupSuministroDTO> lstInterrSuministro)
        {
            var nombCompletPlantilla = $"{rutaBase}{nombPlantilla}";
            var nombFormato = $"{nombArchivoFormato}{ConstantesAppServicio.ExtensionExcel}";
            var nombCompletFormato = $"{rutaBase}{nombFormato}";

            var archivoPlantilla = new FileInfo(nombCompletPlantilla);

            var nuevoArchivo = new FileInfo(nombCompletFormato);
            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombCompletFormato);
            }

            using (var xlPackage = new ExcelPackage(archivoPlantilla))
            {
                using (var ws = xlPackage.Workbook.Worksheets[1])
                {
                    //Filtros
                    SiEmpresaDTO objEmpresa = FactorySic.GetSiEmpresaRepository().GetById(emprcodi);
                    SiFuentedatosDTO objFuenteDatos = FactorySic.GetSiFuentedatosRepository().GetById(fdatcodi);
                    int rowIniFiltro = 2;
                    int colIniFiltro = 5;
                    ws.Cells[rowIniFiltro, colIniFiltro].Value = objFuenteDatos.Fdatnombre;
                    ws.Cells[rowIniFiltro + 1, colIniFiltro].Value = objEmpresa.Emprnomb != null ? objEmpresa.Emprnomb.Trim() : string.Empty;

                    //Datos del evento
                    int eveRowIni = 7;
                    int eveColumnValue = 4;

                    ws.Cells[eveRowIni++, eveColumnValue].Value = eveEventoDTO.CODIGO;
                    ws.Cells[eveRowIni++, eveColumnValue].Value = eveEventoDTO.EVENINI?.ToString(ConstantesBase.FormatoFechaFullBase);
                    ws.Cells[eveRowIni++, eveColumnValue].Value = eveEventoDTO.EVENFIN?.ToString(ConstantesBase.FormatoFechaFullBase);
                    ws.Cells[eveRowIni++, eveColumnValue].Value = eveEventoDTO.EVENASUNTO;
                    ws.Cells[eveRowIni++, eveColumnValue].Value = eveEventoDTO.EVENDESC;

                    //Lista de datos de interrupciones
                    int rowIni = 14, colIni = 3;
                    int colIniDynamic = colIni, rowIniDynamic = rowIni;

                    this.FormaterDataExtranet(ref lstInterrSuministro);
                    if (fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF)
                    {
                        ws.Protection.IsProtected = true;

                        List<AfInterrupSuministroDTO> lstDBEracmfEventos = ObtenerEracmfEvento(emprcodi, eveEventoDTO.EVENCODI.Value);
                        List<AfInterrupSuministroDTO> lstUnionEracmfInterr = ObtenerDataUnionEracmfConInterrupSumin(lstDBEracmfEventos, lstInterrSuministro);

                        foreach (var interr in lstUnionEracmfInterr)
                        {
                            colIniDynamic = colIni;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.Intsumzona;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.Intsumempresa;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.Intsumsuministro;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.Intsumsubestacion;

                            var cellIntsummw = ws.Cells[rowIniDynamic, colIniDynamic++];
                            cellIntsummw.Style.Locked = false; cellIntsummw.Value = interr.Intsummw;

                            var cellFechaini = ws.Cells[rowIniDynamic, colIniDynamic++];
                            cellFechaini.Style.Locked = false; cellFechaini.Value = interr.Intsumfechainterrini2;

                            var cellFechafin = ws.Cells[rowIniDynamic, colIniDynamic++];
                            cellFechafin.Style.Locked = false; cellFechafin.Value = interr.Intsumfechainterrfin2;

                            var cellDuracion = ws.Cells[rowIniDynamic, colIniDynamic++];
                            cellDuracion.Value = interr.Intsumduracion;

                            var cellFuncion = ws.Cells[rowIniDynamic, colIniDynamic++];
                            cellFuncion.Style.Locked = false; cellFuncion.Value = interr.Intsumfuncion;

                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.Intsumnumetapa;

                            var cellObser = ws.Cells[rowIniDynamic, colIniDynamic++];
                            cellObser.Style.Locked = false; cellObser.Value = interr.Intsumobs;

                            rowIniDynamic++;
                        }

                        if (lstUnionEracmfInterr.Any())
                        {
                            var modelTable = ws.Cells[rowIni, colIni, rowIniDynamic - 1, colIniDynamic - 1];
                            UtilExcel.AllBorders(modelTable);

                            var columnSoloLectura = ws.Cells[rowIni, colIni, rowIniDynamic - 1, colIni + 3];
                            UtilExcel.BackgroundColor(columnSoloLectura, ColorTranslator.FromHtml("#D2EFF7"));

                            UtilExcel.BackgroundColor(ws.Cells[rowIni, 10, rowIniDynamic - 1, 10], ColorTranslator.FromHtml("#D2EFF7"));
                            UtilExcel.BackgroundColor(ws.Cells[rowIni, 12, rowIniDynamic - 1, 12], ColorTranslator.FromHtml("#D2EFF7"));
                        }
                    }
                    else if (fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion || fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.ReduccionSuministros)
                    {
                        foreach (var interr in lstInterrSuministro)
                        {
                            colIniDynamic = colIni;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.Intsumsuministro;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.Intsumsubestacion;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.Intsummw;

                            if (fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.ReduccionSuministros)
                            {
                                ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.Intsummwfin;
                                ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.Intsummwred;
                            }

                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.Intsumfechainterrini2;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.Intsumfechainterrfin2;
                            ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.Intsumduracion;

                            if (fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion)
                            {
                                ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.Intsumobs;
                            }
                            rowIniDynamic++;
                        }

                        if (lstInterrSuministro.Any())
                        {
                            var modelTable = ws.Cells[rowIni, colIni, rowIniDynamic - 1, colIniDynamic - 1];
                            UtilExcel.AllBorders(modelTable);
                        }

                    }

                    xlPackage.SaveAs(nuevoArchivo);
                }
            }
            return nombFormato;
        }

        /// <summary>
        /// Obtener Data Eracmf Evento
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="enviocodi"></param>
        /// <returns></returns>
        public List<AfInterrupSuministroDTO> ObtenerDataEracmfEvento(int afecodi, int emprcodi, int enviocodi)
        {
            List<AfInterrupSuministroDTO> lstInterrSuministro = ObtenerUltimoEnvioInterrupcionSuministro(afecodi, emprcodi, (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF, enviocodi);

            List<AfInterrupSuministroDTO> lstEracmfEventos = this.ObtenerInterrupcionSuministroUnionDb(afecodi, emprcodi, lstInterrSuministro);

            return lstEracmfEventos;
        }

        /// <summary>
        /// Obtener Interrupcion Suministro Union Db Excel
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="lstInterrSuministro"></param>
        /// <returns></returns>
        public List<AfInterrupSuministroDTO> ObtenerInterrupcionSuministroUnionDb(int afecodi, int emprcodi, List<AfInterrupSuministroDTO> lstInterrSuministro)
        {
            AfEventoDTO afEvento = GetByIdAfEvento(afecodi);
            EveEventoDTO eveEventoDTO = ObtenerEventoPorId(afEvento.Evencodi.Value);

            List<AfInterrupSuministroDTO> lstDBEracmfEventos = ObtenerEracmfEvento(emprcodi, eveEventoDTO.Evencodi);

            List<AfInterrupSuministroDTO> lstEracmfEventos = ObtenerDataUnionEracmfConInterrupSumin(lstDBEracmfEventos, lstInterrSuministro);

            return lstEracmfEventos;
        }

        /// <summary>
        /// Obtener Data Union Eracmf Con InterrupSumin
        /// </summary>
        /// <param name="lstDBEracmfEventos"></param>
        /// <param name="lstInterrSuministro"></param>
        /// <returns></returns>
        private List<AfInterrupSuministroDTO> ObtenerDataUnionEracmfConInterrupSumin(List<AfInterrupSuministroDTO> lstDBEracmfEventos, List<AfInterrupSuministroDTO> lstInterrSuministro)
        {
            foreach (var it in lstInterrSuministro)
            {
                var eracmfEventos = lstDBEracmfEventos
                    .Find(x => $"{x.Intsumzona}{x.Intsumempresa}{x.Intsumsuministro}{x.Intsumsubestacion}{x.Intsumnumetapa}" == $"Zona {it.Intsumzona}{it.Intsumempresa}{it.Intsumsuministro}{it.Intsumsubestacion}{it.Intsumnumetapa}");

                if (eracmfEventos != null)
                {
                    if (!string.IsNullOrEmpty(it.Intsumfechainterrini2)) it.Intsumfechainterrini = DateTime.ParseExact(it.Intsumfechainterrini2, ConstantesBase.FormatoFechaFullBase, CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(it.Intsumfechainterrfin2)) it.Intsumfechainterrfin = DateTime.ParseExact(it.Intsumfechainterrfin2, ConstantesBase.FormatoFechaFullBase, CultureInfo.InvariantCulture);

                    eracmfEventos.Intsummw = it.Intsummw;
                    eracmfEventos.Intsumfechainterrini = it.Intsumfechainterrini;
                    eracmfEventos.Intsumfechainterrfin = it.Intsumfechainterrfin;
                    eracmfEventos.Intsumfechainterrini2 = it.Intsumfechainterrini?.ToString(ConstantesBase.FormatoFechaFullBase);
                    eracmfEventos.Intsumfechainterrfin2 = it.Intsumfechainterrfin?.ToString(ConstantesBase.FormatoFechaFullBase);
                    eracmfEventos.Intsumduracion = it.Intsumduracion;
                    eracmfEventos.Intsumfuncion = it.Intsumfuncion;
                    eracmfEventos.Intsumobs = it.Intsumobs;
                }
            }

            lstDBEracmfEventos = lstDBEracmfEventos.OrderBy(x => x.Intsumzona).ThenBy(x => x.Intsumsubestacion).ThenBy(x => x.Intsumsuministro).ThenBy(x => x.Intsumnumetapa).ToList();

            return lstDBEracmfEventos;
        }

        /// <summary>
        /// Obtener Ultimo Envio Interrupcion Suministro
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="fdatcodi"></param>
        /// <param name="enviocodi"></param>
        /// <returns></returns>
        public List<AfInterrupSuministroDTO> ObtenerUltimoEnvioInterrupcionSuministro(int afecodi, int emprcodi, int fdatcodi, int enviocodi)
        {
            List<AfInterrupSuministroDTO> lstInterrSuministro = new List<AfInterrupSuministroDTO>();
            if (enviocodi > 0)
                lstInterrSuministro = FactorySic.GetAfInterrupSuministroRepository().GetByCriteria(afecodi, emprcodi, fdatcodi, enviocodi);
            else
                lstInterrSuministro = FactorySic.GetAfInterrupSuministroRepository().ObtenerUltimoEnvio(afecodi, emprcodi, fdatcodi);

            //formatear data
            foreach (var item in lstInterrSuministro)
            {
                item.Intsummw2 = item.Intsummw?.ToString();
                item.Intsummwfin2 = item.Intsummwfin?.ToString();
                item.Intsumfechainterrini2 = item.Intsumfechainterrini?.ToString(ConstantesBase.FormatoFechaFullBase);
                item.Intsumfechainterrfin2 = item.Intsumfechainterrfin?.ToString(ConstantesBase.FormatoFechaFullBase);

                if (item.Intsumfechainterrfin != null && item.Intsumfechainterrini != null)
                {
                    item.Intsumduracion = Convert.ToDecimal(item.Intsumfechainterrfin.Value.Subtract(item.Intsumfechainterrini.Value).TotalMinutes);
                    //redondeo solo en la extranet
                    item.Intsumduracion = MathHelper.Round(item.Intsumduracion.Value, ConstantesExtranetCTAF.DigitosParteDecimalDuracion);
                }
            }

            return lstInterrSuministro.OrderBy(X => X.Intsumsubestacion).ThenBy(x => x.Intsumsuministro).ToList();
        }

        /// <summary>
        /// Obtener Interrupcion Suministro De Data Excel
        /// </summary>
        /// <param name="stremExcel"></param>
        /// <param name="fdatcodi"></param>
        /// <returns></returns>
        public List<AfInterrupSuministroDTO> ObtenerInterrupcionSuministroDeDataExcel(Stream stremExcel, int fdatcodi)
        {
            List<AfInterrupSuministroDTO> lstInterrupcionSuministro = new List<AfInterrupSuministroDTO>();
            using (var xlPackage = new ExcelPackage(stremExcel))
            {
                var ws = xlPackage.Workbook.Worksheets[1];

                int rowIni = 13, colIni = fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF ? 2 : 3;

                var dim = ws.Dimension;
                ExcelRange excelRange = ws.Cells[rowIni, colIni, dim.End.Row, dim.End.Column];
                var dataExcel = (object[,])excelRange.Value;

                var rowLast = dim.End.Row - rowIni;
                if (fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF)
                {
                    for (int i = 1; i <= rowLast; i++)
                    {
                        var exIntsumzona = dataExcel[i, 1]?.ToString();
                        var exIntsumempresa = dataExcel[i, 2]?.ToString();
                        var exIntsumsuministro = dataExcel[i, 3]?.ToString();
                        var exIntsumsubestacion = dataExcel[i, 4]?.ToString();
                        var exIntsummw = (double?)dataExcel[i, 5];
                        var exIntsumfechainterrini = dataExcel[i, 6]?.ToString();
                        var exIntsumfechainterrfin = dataExcel[i, 7]?.ToString();
                        var exIntsumduracion = (double?)dataExcel[i, 8];
                        var exIntsumfuncion = dataExcel[i, 9]?.ToString();
                        var exIntsumetapa = dataExcel[i, 10]?.ToString();
                        var exIIntsumobs = dataExcel[i, 11]?.ToString();

                        decimal? intsummw = null, intsumduracion = null;
                        int numEtapa = 0;

                        if (exIntsummw.HasValue) intsummw = Convert.ToDecimal(exIntsummw.Value);
                        if (exIntsumduracion.HasValue) intsumduracion = Convert.ToDecimal(exIntsumduracion.Value);
                        if (exIntsumetapa != null) numEtapa = Convert.ToInt32(exIntsumetapa);

                        if (!string.IsNullOrEmpty(exIntsumzona))
                        {
                            exIntsumzona = exIntsumzona.Trim().Last().ToString();
                        }

                        lstInterrupcionSuministro.Add(new AfInterrupSuministroDTO()
                        {
                            Intsumzona = exIntsumzona,
                            Intsumempresa = exIntsumempresa,
                            Intsumsuministro = exIntsumsuministro,
                            Intsumsubestacion = exIntsumsubestacion,
                            Intsummw = intsummw,
                            Intsumfechainterrini2 = exIntsumfechainterrini,
                            Intsumfechainterrfin2 = exIntsumfechainterrfin,
                            Intsumduracion = intsumduracion,
                            Intsumfuncion = exIntsumfuncion,
                            Intsumnumetapa = numEtapa,
                            Intsumobs = exIIntsumobs
                        });
                    }
                }
                else if (fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion || fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.ReduccionSuministros)
                {
                    for (int i = 1; i <= rowLast; i++)
                    {
                        int col = 0;

                        var exIntsumsuministro = dataExcel[i, col++]?.ToString();
                        var exIntsumsubestacion = dataExcel[i, col++]?.ToString();
                        var exIntsummw = dataExcel[i, col++]?.ToString();

                        string exIntsummwfin = null, exIIntsumobs = null;

                        if (fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.ReduccionSuministros)
                        {
                            exIntsummwfin = dataExcel[i, col++]?.ToString();
                            col++;
                        }

                        var exIntsumfechainterrini = dataExcel[i, col++]?.ToString();
                        var exIntsumfechainterrfin = dataExcel[i, col++]?.ToString();

                        if (fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion)
                        {
                            col++;
                            exIIntsumobs = dataExcel[i, col++]?.ToString();
                        }

                        lstInterrupcionSuministro.Add(new AfInterrupSuministroDTO()
                        {
                            Intsumsuministro = exIntsumsuministro,
                            Intsumsubestacion = exIntsumsubestacion,
                            Intsummw2 = exIntsummw,
                            Intsummwfin2 = exIntsummwfin,
                            Intsumfechainterrini2 = exIntsumfechainterrini,
                            Intsumfechainterrfin2 = exIntsumfechainterrfin,
                            Intsumobs = exIIntsumobs
                        });
                    }
                }
            }

            return lstInterrupcionSuministro;
        }

        /// <summary>
        /// Es válido las filas del excel web
        /// </summary>
        /// <param name="fdatcodi"></param>
        /// <param name="lista"></param>
        /// <returns></returns>
        private bool EsValidoDataExtranetCTAF(int fdatcodi, List<AfInterrupSuministroDTO> lista)
        {
            switch (fdatcodi)
            {
                case (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF:
                    var listaData = lista.GroupBy(x => new { x.Intsumzona, x.Intsumempresa, x.Intsumsubestacion, x.Intsumsuministro, x.Intsumnumetapa })
                           .Where(grp => grp.Count() > 1);
                    return !listaData.Any();
                default:
                    var listaData2 = lista.GroupBy(x => new { x.Intsumsubestacion, x.Intsumsuministro })
                           .Where(grp => grp.Count() > 1);
                    return !listaData2.Any();
            }
        }

        /// <summary>
        /// Enviar Datos Interrupcion Suministro
        /// </summary>
        /// <param name="username"></param>
        /// <param name="emprcodi"></param>
        /// <param name="fdatcodi"></param>
        /// <param name="afecodi"></param>
        /// <param name="listaInterrupcionSuministro"></param>
        /// <returns></returns>
        public int EnviarDatosInterrupcionSuministro(string username, int emprcodi, int fdatcodi, int afecodi, List<AfInterrupSuministroDTO> listaInterrupcionSuministro)
        {
            int enviocodi = 0;
            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        //Validación de data
                        if (!this.EsValidoDataExtranetCTAF(fdatcodi, listaInterrupcionSuministro))
                            throw new Exception("Existe filas duplicadas en el excel web.");

                        //
                        EventoDTO afEvento = ObtenerInterrupcionByAfecodi(afecodi);
                        DateTime fechaRegistro = DateTime.Now;

                        //Registrar en tabla ME_ENVIO
                        var envio = new MeEnvioDTO()
                        {
                            Emprcodi = emprcodi,
                            Fdatcodi = fdatcodi,
                            Lastdate = fechaRegistro,
                            Lastuser = username,
                            Enviofecha = fechaRegistro,
                            Enviofechaini = afEvento.EVENINI.Value,
                            Enviofechafin = afEvento.EVENFIN.Value,
                            Enviofechaplazoini = afEvento.EVENINI.Value,
                            Enviofechaplazofin = afEvento.Afeplazofecha,
                            Enviofechaperiodo = afEvento.EVENINI.Value.Date,
                            Estenvcodi = 1,
                            Formatcodi = 0,
                            Envioplazo = (afEvento.Afeplazofechaampl != null && fechaRegistro > afEvento.Afeplazofecha) ? "F" : "P"
                        };

                        enviocodi = FactorySic.GetMeEnvioRepository().Save(envio, connection, transaction);

                        //Dar de baja los envíos anteriores.
                        FactorySic.GetAfInterrupSuministroRepository().UpdateAEstadoBajaXEmpresa(afecodi, emprcodi, fdatcodi, connection, transaction);

                        //Registrar el Excel web
                        this.FormaterDataExtranet(ref listaInterrupcionSuministro);
                        foreach (var entity in listaInterrupcionSuministro)
                        {
                            entity.Enviocodi = enviocodi;
                            entity.Afecodi = afecodi;
                            entity.Intsumestado = 1;
                            entity.Intsumfeccreacion = fechaRegistro;
                            entity.Intsumusucreacion = username;
                            var intsumcodi = FactorySic.GetAfInterrupSuministroRepository().Save(entity, connection, transaction);
                        }

                        transaction.Commit();

                        //Enviar notificacion al CTAF
                        EnviarCorreoNotificacionCargaInformacionAgentes(afEvento, username, fechaRegistro, emprcodi, enviocodi, 1);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return enviocodi;
        }

        /// <summary>
        /// Obtiene en enviocodi al registrar una solicitud
        /// </summary>
        /// <param name="username"></param>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public int EnviarDatosSolicitud(string username, AfSolicitudRespDTO solicitud)
        {
            int enviocodi = 0;
            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        DateTime fechaRegistro = DateTime.Now;

                        //Registrar en tabla ME_ENVIO 
                        var envio = new MeEnvioDTO()
                        {
                            Emprcodi = solicitud.Emprcodi,
                            Fdatcodi = ConstantesExtranetCTAF.FdatcodiCTAFSolicitudes,
                            Lastdate = DateTime.Now,
                            Lastuser = username,
                            Enviofecha = DateTime.Now,
                            Enviofechaperiodo = DateTime.Now.Date,
                            Estenvcodi = 1,
                            Formatcodi = 0,
                            Envioplazo = "P"
                        };

                        enviocodi = FactorySic.GetMeEnvioRepository().Save(envio, connection, transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return enviocodi;
        }

        /// <summary>
        /// Formater Data Extranet
        /// </summary>
        /// <param name="listaInterrupcionSuministro"></param>
        private void FormaterDataExtranet(ref List<AfInterrupSuministroDTO> listaInterrupcionSuministro)
        {
            //Registrar el Excel web
            listaInterrupcionSuministro = listaInterrupcionSuministro != null ? listaInterrupcionSuministro : new List<AfInterrupSuministroDTO>();
            foreach (var entity in listaInterrupcionSuministro)
            {
                if (!string.IsNullOrEmpty(entity.Intsumzona))
                {
                    entity.Intsumzona = entity.Intsumzona.Trim().Last().ToString();
                }

                if (!string.IsNullOrEmpty(entity.Intsumfechainterrini2)) entity.Intsumfechainterrini = DateTime.ParseExact(entity.Intsumfechainterrini2, ConstantesBase.FormatoFechaFullBase, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(entity.Intsumfechainterrfin2)) entity.Intsumfechainterrfin = DateTime.ParseExact(entity.Intsumfechainterrfin2, ConstantesBase.FormatoFechaFullBase, CultureInfo.InvariantCulture);

                if (entity.Intsumfechainterrfin.HasValue && entity.Intsumfechainterrini.HasValue)
                {
                    entity.Intsumduracion = Convert.ToDecimal(entity.Intsumfechainterrfin.Value.Subtract(entity.Intsumfechainterrini.Value).TotalMinutes);
                    //redondeo solo en la extranet
                    entity.Intsumduracion = MathHelper.Round(entity.Intsumduracion.Value, ConstantesExtranetCTAF.DigitosParteDecimalDuracion);
                }

                if (entity.Intsummw.HasValue && entity.Intsummwfin.HasValue)
                {
                    entity.Intsummwred = entity.Intsummw.Value - entity.Intsummwfin.Value;
                }
            }
        }

        /// <summary>
        /// Obtener Envios Interrup Suministro
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="afecodi"></param>
        /// <param name="fdatcodi"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> ObtenerEnviosInterrupSuministro(int emprcodi, int afecodi, int fdatcodi)
        {
            return FactorySic.GetMeEnvioRepository().ObtenerEnvioInterrupSuministro(emprcodi, afecodi, fdatcodi);
        }

        /// <summary>
        /// Preparar Lista Envios Interrup Suministro
        /// </summary>
        /// <param name="lstEnvios"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> PrepararListaEnviosInterrupSuministro(List<MeEnvioDTO> lstEnvios)
        {
            foreach (var envio in lstEnvios)
            {
                if (envio.Envioplazo == "F")
                    envio.Envioplazo = "Fuera de plazo";
                else if (envio.Envioplazo == "P")
                    envio.Envioplazo = "En plazo";

                envio.Enviofecha2 = envio.Enviofecha?.ToString(ConstantesBase.FormatFechaFull);
            }
            return lstEnvios;
        }

        /// <summary>
        /// Método que valida los plazos y muestra el color del plazo envío
        /// </summary>
        /// <param name="lista"></param>
        public void CalcularPlazoInterrupcion(List<EventoDTO> lista)
        {
            foreach (var val in lista)
            {
                DeterminarPlazoInterrupcion(val);
            }
        }

        /// <summary>
        /// Permite deterner plazo de envío
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public void DeterminarPlazoInterrupcion(EventoDTO val)
        {
            val.ColorPlazo = "";

            DateTime fechaFinEnPlazo = this.GetFechaFinEnPlazo(val.FechaEvento);

            if (val.FechaEvento != null && val.Afeplazofecha != null)
            {
                DateTime fechaActual = DateTime.Now;
                DateTime fechaVencimiento = val.Afeplazofechaampl != null ? val.Afeplazofechaampl.Value : val.Afeplazofecha.Value;

                if (val.FechaEvento < fechaActual && fechaActual < fechaFinEnPlazo)
                {
                    var horas = (fechaVencimiento - fechaActual).ToString(@"dd\d\ hh\h\ mm\m\ ss\s");
                    val.PlazoEnvio = "Faltan " + horas;
                    val.ColorPlazo = "#39C251";
                    val.EnPlazo = true;
                }
                else if (fechaActual >= fechaFinEnPlazo && fechaActual <= fechaVencimiento)
                {
                    var horas = (fechaVencimiento - fechaActual).ToString(@"dd\d\ hh\h\ mm\m\ ss\s");
                    val.PlazoEnvio = "Faltan " + horas;
                    val.ColorPlazo = "#FFD800";
                    val.EnPlazo = true;
                }
                else
                {
                    val.PlazoEnvio = "Plazo Vencido";
                    val.ColorPlazo = "#FF0000";
                    val.Deshabilidado = true;
                }

                if (val.EnPlazo)
                {
                    //si existe ampliacion y la fecha actual esta entre la fecha de vencimiento inicial y la fecha de ampliacion entonces esta fuera de plazo
                    if (val.Afeplazofechaampl != null && fechaActual > val.Afeplazofecha)
                        val.EnPlazo = false;
                }
            }
        }

        /// <summary>
        /// Notifica via email cada vez que un agente realiza una carga de informacion 
        /// </summary>
        /// <param name="afEventoDTO"></param>
        /// <param name="usuario"></param>
        /// <param name="fechaModificacion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="tipo"></param>
        public void EnviarCorreoNotificacionCargaInformacionAgentes(EventoDTO afEventoDTO, string usuario, DateTime fechaModificacion, int idEmpresa, int idEnvio, int tipo)
        {

            //Enviar correo
            if (afEventoDTO != null)
            {
                try
                {
                    DateTime fechaPeriodo = afEventoDTO.EVENINI.Value.Date;

                    SiPlantillacorreoDTO plantilla = this.servCorreo.GetByIdSiPlantillacorreo(ConstantesExtranetCTAF.PlantcodiNotificacionCarga);

                    if (plantilla != null)
                    {
                        string empresa = FactorySic.GetSiEmpresaRepository().GetById(idEmpresa).Emprnomb;
                        string codigoEvento = string.Format("{0}", afEventoDTO.CODIGO);
                        string fechaEvento = afEventoDTO.EVENINI.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);

                        string asunto = string.Format(plantilla.Plantasunto, empresa, codigoEvento, fechaEvento);
                        string contenido = this.GetContenidoCorreoCargaInformacion(empresa, codigoEvento, fechaEvento, tipo);

                        List<string> listaTo = plantilla.Planticorreos.Split(';').Select(x => x).ToList();

                        List<string> listaCC = new List<string>();
                        string from = TipoPlantillaCorreo.MailFrom;
                        string to = string.Join(" ", listaTo);

                        if (!string.IsNullOrEmpty(contenido))
                        {
                            if (!ConstantesExtranetCTAF.FlagEnviarNotificacionCargaEvento)
                            {
                                listaTo = ConstantesExtranetCTAF.ListaEmailAdminEventosTo.Split(';').ToList();
                                listaCC = ConstantesExtranetCTAF.ListaEmailAdminEventosCC.Split(';').ToList();
                            }

                            COES.Base.Tools.Util.SendEmail(listaTo, listaCC, asunto, contenido);

                            SiCorreoDTO correo = new SiCorreoDTO();
                            correo.Corrasunto = asunto;
                            correo.Corrcontenido = contenido;
                            correo.Corrfechaenvio = fechaModificacion;
                            correo.Corrfechaperiodo = fechaPeriodo;
                            correo.Corrfrom = from;
                            correo.Corrto = to;
                            correo.Emprcodi = idEmpresa <= 1 ? 1 : idEmpresa;
                            correo.Enviocodi = idEnvio;
                            correo.Plantcodi = plantilla.Plantcodi;

                            this.servCorreo.SaveSiCorreo(correo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                }
            }
        }

        /// <summary>
        /// Crea el contenido del email a notificar carga de informacion de eventos
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="codigoEvento"></param>
        /// <param name="fechaEvento"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        private string GetContenidoCorreoCargaInformacion(string empresa, string codigoEvento, string fechaEvento, int tipo)
        {
            string html = string.Empty;
            if (tipo == 1)
            {
                html = @"
                <html>

                    <head>
	                    <STYLE TYPE='text/css'>
	                    body {{font-size: .80em;font-family: 'Helvetica Neue', 'Lucida Grande', 'Segoe UI', Arial, Helvetica, Verdana, sans-serif;}}
	                    .mail {{width:500px;border-spacing:0;border-collapse:collapse;}}
	                    .mail thead th {{text-align: center;background: #417AA7;color:#ffffff}}
	                    .mail tr td {{border:1px solid #dddddd;}}
	                    table.tabla_hop thead th {text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;}
	                    </style>
                    </head>

                    <body>
                        La empresa <b><Empresa></b> cargó sus datos de interrupción de suministros, respecto al evento <b><CodigoEvento></b> ocurrido el <b><FechaEvento></b>.
	                    
                        <br><br><br>
                        {footer}
                    </body>
                </html>
            ";
            }
            else if (tipo == 2)
            {
                html = @"
                <html>

                    <head>
	                    <STYLE TYPE='text/css'>
	                    body {{font-size: .80em;font-family: 'Helvetica Neue', 'Lucida Grande', 'Segoe UI', Arial, Helvetica, Verdana, sans-serif;}}
	                    .mail {{width:500px;border-spacing:0;border-collapse:collapse;}}
	                    .mail thead th {{text-align: center;background: #417AA7;color:#ffffff}}
	                    .mail tr td {{border:1px solid #dddddd;}}
	                    table.tabla_hop thead th {text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;}
	                    </style>
                    </head>

                    <body>
                        Se ha eliminado los datos de interrupción de suministros de la empresa <b><Empresa></b> , respecto al evento <b><CodigoEvento></b> ocurrido el <b><FechaEvento></b>.
	                    
                        <br><br><br>
                        {footer}
                    </body>
                </html>
            ";
            }

            html = html.Replace("<Empresa>", empresa);
            html = html.Replace("<CodigoEvento>", codigoEvento);
            html = html.Replace("<FechaEvento>", fechaEvento);

            html = html.Replace("{footer}", CorreoAppServicio.GetFooterCorreo());

            return html;
        }

        /// <summary>
        /// Crea el contenido del email a notificar CTAF
        /// </summary>
        /// <param name="tipoReunion"></param>
        /// <param name="fechaLimite"></param>
        /// <param name="evento"></param>
        /// <param name="descripcion"></param>
        /// <param name="empresasInvolucradas"></param>
        /// <param name="contenido"></param>
        /// <returns></returns>
        private string GetContenidoCorreoCitacionCTAF(string tipoReunion, string fechaLimite, string evento, string descripcion, string empresasInvolucradas, string contenido)
        {
            string html = string.Empty;
            html = @contenido;

            html = html.Replace("<TipoReunion>", tipoReunion);
            html = html.Replace("<FechaLimite>", fechaLimite);
            html = html.Replace("<Evento>", evento);
            html = html.Replace("<Descripcion>", descripcion);
            html = html.Replace("<EmpresasInvolucradas>", empresasInvolucradas);

            html = html.Replace("{footer}", CorreoAppServicio.GetFooterCorreoCTAF());

            return html;
        }

        private string GetContenidoCorreoActaCTAF(string eventoCompleto, string diaFechaLimite, List<EveRecomobservDTO> lstObservaciones, List<EveRecomobservDTO> ListaRecomendaciones)
        {
            string html = string.Empty;
            html = @"                                          
                <html>                                
                 <head>
                <style TYPE=''text/css''>
                    body { font-size:11.0pt; font-family: 'Calibri', Calibri; }
                    .EnNegrita { font-weight: bold; }
                    .evento { font-weight: bold; text-decoration: underline; }
                    .indentar { margin-left: 20px; }
                </style>
                </head>
                <body>
                    <p id='SaludoInicial'>Estimados ingenieros integrantes del CTAF:</p>
                    <p>Se adjunta el informe CT-AF respecto al evento: <b><Evento></b>. Agradeceremos remitir sus comentarios para que sean considerados en el informe final hasta el día: <span style='color:#0000FF; font-weight: bold;'><FechaLimite></span>.</p>

                    <CuadroObservaciones>
                    <br/>
                    <p class='EnNegrita' style='margin-top: -0.4%; margin-bottom: 2%;'>Saludos,</P>
                    {footer}
                </body>
                </html>
            ";

            string cuadroObservaciones = "";


            cuadroObservaciones = cuadroObservaciones + "<table><tr><td colspan = '3' style='text-decoration: underline;font-weight: bold;'>OBSERVACIONES</td></tr>";

            //cuadroObservaciones = cuadroObservaciones + "<p class='EnNegrita' style='text-decoration: underline;'>OBSERVACIONES:</p>";
            int contador = 0;
            //foreach (var item in lstObservaciones)
            //{
                var idsEmpresas = lstObservaciones.Select(y => new { y.EMPRCODI }).Distinct().ToList();
                foreach (var item2 in idsEmpresas)
                {
                    bool band = false;
                    int num_ = contador + 1;
                    for (int x = 0; x < lstObservaciones.Count; x++)
                    {
                        if (item2.EMPRCODI == lstObservaciones[x].EMPRCODI)
                        {                          
                            if (!band)
                            {
                                cuadroObservaciones = cuadroObservaciones + "<tr><td colspan='3'><b>" + num_.ToString() + ". " + lstObservaciones[x].EMPRNOMB + "</b></td></tr>";
                                cuadroObservaciones = cuadroObservaciones + "<tr><td width='10'></td><td width='10'>•</td><td>" + lstObservaciones[x].EVERECOMOBSERVDESC + "</td></tr>";
                                contador++;
                                band = true;
                            }
                            else
                            {
                                cuadroObservaciones = cuadroObservaciones + "<tr><td width='25'></td><td width='25'>•</td><td>" + lstObservaciones[x].EVERECOMOBSERVDESC + "</td></tr>";
                            }  
                        }
                    }                   
                }
            //}
            cuadroObservaciones = cuadroObservaciones + "<tr><td colspan = '3' style='text-decoration: underline;font-weight: bold; padding-top:10px;'>RECOMENDACIONES</td></tr> ";

            int contador2 = 0;

            //foreach (var item in ListaRecomendaciones)
            //{
                var idsEmpresasRe = ListaRecomendaciones.Select(y => new { y.EMPRCODI }).Distinct().ToList();
                foreach (var item2 in idsEmpresasRe)
                {
                    bool band = false;

                    for (int x = 0; x < ListaRecomendaciones.Count; x++)
                    {
                        if (item2.EMPRCODI == ListaRecomendaciones[x].EMPRCODI)
                        {
                            if (!band)
                            {
                                int num_ = contador2 + 1;
                                cuadroObservaciones = cuadroObservaciones + "<tr><td colspan='3'><b>" + num_.ToString() + ". " + ListaRecomendaciones[x].EMPRNOMB + "</b></td></tr>";
                                cuadroObservaciones = cuadroObservaciones + "<tr><td width='10'></td><td width='10'>•</td><td>" + ListaRecomendaciones[x].EVERECOMOBSERVDESC + "</td></tr>";
                                contador2++;
                                band = true;
                            }
                            else
                            {
                                cuadroObservaciones = cuadroObservaciones + "<tr><td width='25'></td><td width='25'>•</td><td>" + ListaRecomendaciones[x].EVERECOMOBSERVDESC + "</td></tr>";
                            }
                        }
                    }         
                }
            cuadroObservaciones = cuadroObservaciones + "</table>";
            //}

            html = html.Replace("<Evento>", eventoCompleto);
            html = html.Replace("<FechaLimite>", diaFechaLimite);
            html = html.Replace("<CuadroObservaciones>", cuadroObservaciones);
            html = html.Replace("{footer}", CorreoAppServicio.GetFooterCorreoCTAF());

            return html;
        }


        #endregion

        #region Listado eventos interrupción

        /// <summary>
        /// Listado de eventos
        /// </summary>
        /// <param name="oEventoDTO"></param>
        /// <returns></returns>
        public List<EventoDTO> ConsultarInterrupcionesSuministros(EventoDTO oEventoDTO)
        {
            return FactorySic.ObtenerEventoDao().ConsultarInterrupcionesSuministros(oEventoDTO);
        }

        /// <summary>
        /// Deveulve la lista  de registros que se mostraran en la tabla de interrupciones en HTML
        /// </summary>
        /// <param name="url"></param>
        /// <param name="permisoGrabar"></param>
        /// <param name="listaInterrupciones"></param>
        /// <returns></returns>
        public string ObtenerListadoInterrupcionesHtml(string url, bool permisoGrabar, List<EventoDTO> listaInterrupciones)
        {
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table class='pretty tabla-adicional' id='reporte' style='table-layout: fixed;'>");

            #region cabecera

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 60px'>Acciones</th>");
            strHtml.Append("<th style='width: 80px'>CÓDIGO</th>");
            strHtml.Append("<th style=''>NOMBRE DE EVENTO</th>");
            strHtml.Append("<th style='width: 80px'>MW INTERR</th>");
            strHtml.Append("<th style='width: 80px'>ERACMF</th>");
            strHtml.Append("<th style='width: 130px'>HORA EVENTO</th>");
            strHtml.Append("<th style='width: 130px'>HORA INTERRUPCIÓN</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            foreach (var reg in listaInterrupciones)
            {
                strHtml.AppendFormat("<tr style='height: 25px;' class='{0}'>", reg.Afefechainterr != null && reg.Reportado != ConstantesAppServicio.SI ? "fila_no_reportado" : string.Empty);
                strHtml.Append("<td>");
                if (permisoGrabar)
                    strHtml.AppendFormat("<a href = '{0}Eventos/AnalisisFallas/Edicion/{1}' id='btnEditar' target=''><img src ='{0}Content/Images/btn-edit.png' alt='Editar evento' title='Editar evento' style='width='24px;'></a> ", url, reg.AFECODI);
                else
                    strHtml.AppendFormat("<a href = '{0}Eventos/AnalisisFallas/Edicion/{1}' id='btnEditar' target=''><img src ='{0}Content/Images/btn-open.png' alt='Visualizar evento' title='Visualizar evento' style='width='24px;'></a> ", url, reg.AFECODI);
                if (reg.Afefechainterr != null)
                    strHtml.AppendFormat("<a href = '{0}Eventos/AnalisisFallas/ReporteExtranetCTAF/{1},{2},{3}' id='btnReporte' target=''><img src ='{0}Content/Images/btn-properties.png' alt='Reporte' title='Reporte' style='width='24px;'></a> ", url, reg.AFECODI, reg.Afeanio, reg.Afecorr);
                if (reg.ERACMF == "S")
                {
                    strHtml.AppendFormat("<a href = '{0}Eventos/AnalisisFallas/CargarERACMF/{1}' id='btnCargar' target=''><img src ='{0}Content/Images/Import2.png' alt='Cargar archivo ERACMF' title='Cargar archivo ERACMF' style='width='24px;'></a> ", url, reg.AFECODI);
                }
                strHtml.Append("</td>");
                strHtml.AppendFormat("<td style='text-align: center;'>{0}</td>", reg.CODIGO);
                strHtml.AppendFormat("<td style='text-align: left;'>{0}</td>", reg.NOMBRE_EVENTO);
                strHtml.AppendFormat("<td style='text-align: right; padding-right: 22px;'>{0}</td>", reg.INTERRUMPIDO);
                var Eracmf = reg.ERACMF == "N" ? "NO" : "SÍ";
                strHtml.AppendFormat("<td style='text-align: center;'>{0}</td>", Eracmf);
                strHtml.AppendFormat("<td style='text-align: center;'>{0}</td>", reg.FechaEvento.ToString(ConstantesBase.FormatoFechaFullBase));
                var fechaInterr = reg.Afefechainterr.HasValue ? reg.Afefechainterr.Value.ToString(ConstantesBase.FormatoFechaFullBase) : "";
                strHtml.AppendFormat("<td style='text-align: center;'>{0}</td>", fechaInterr);
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");

            #endregion

            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Validar Evento
        /// </summary>
        /// <param name="oEventoDTO"></param>
        /// <param name="oAnalisisFallaDTO"></param>
        /// <returns></returns>
        public EventoDTO ValidarEvento(EventoDTO oEventoDTO, AnalisisFallaDTO oAnalisisFallaDTO)
        {
            if (oEventoDTO != null)
            {
                string strD = oEventoDTO.EVENINI.Value.ToString(ConstantesBase.FormatoFechaFullBase);
                oEventoDTO.FECHA_EVENTO = strD;
            }
            if (oAnalisisFallaDTO != null)
            {
                oEventoDTO.ERACMF = oAnalisisFallaDTO.AFEERACMF == "N" ? "NO" : "SÍ";
            }
            return oEventoDTO;
        }

        /// <summary>
        /// Permite editar El evento
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int EditarEvento(EventoDTO entity)
        {
            EventoDTO regBD = this.ObtenerInterrupcionByAfecodi(entity.AFECODI);
            if (regBD.Afeplazofecha != null)
            {
                //Si se cambia la fecha de vencimiento, entonces se ha ampliado el plazo
                if (entity.Afeplazofecha != regBD.Afeplazofecha)
                {
                    entity.Afeplazofechaampl = entity.Afeplazofecha;
                    entity.Afeplazofecha = regBD.Afeplazofecha;
                }
            }

            return FactorySic.ObtenerEventoDao().EditarAfEvento(entity);
        }

        /// <summary>
        /// Obtener una interrupción según su id
        /// </summary>
        /// <param name="afecodi"></param>
        /// <returns></returns>
        public EventoDTO ObtenerInterrupcionByAfecodi(int afecodi)
        {
            var reg = FactorySic.ObtenerEventoDao().ObtenerInterrupcion(afecodi);
            reg.FechaEventoDesc = reg.FechaEvento.ToString(ConstantesAppServicio.FormatoFechaFull2);
            reg.FechaInterrupcion = reg.Afefechainterr != null ? reg.Afefechainterr.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
            reg.Eveninidesc = reg.EVENINI.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
            reg.Evenfindesc = reg.EVENFIN.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
            reg.EVENDESC = !string.IsNullOrEmpty(reg.EVENDESC) ? reg.EVENDESC.Trim() : string.Empty;
            return reg;
        }

        /// <summary>
        /// Obtener una interrupción según su id
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public List<EventoDTO> ObtenerInterrupcionCTAF(string anio, string correlativo)
        {
            var listReg = FactorySic.ObtenerEventoDao().ObtenerInterrupcionCTAF(anio, correlativo);


            foreach (var reg in listReg)
            {
                reg.FechaEventoDesc = reg.FechaEvento.ToString(ConstantesAppServicio.FormatoFechaFull2);
                reg.FechaInterrupcion = reg.Afefechainterr != null ? reg.Afefechainterr.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                reg.Eveninidesc = reg.EVENINI.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                reg.Evenfindesc = reg.EVENFIN.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                reg.EVENDESC = !string.IsNullOrEmpty(reg.EVENDESC) ? reg.EVENDESC.Trim() : string.Empty;
            }

            return listReg;
        }

        /// <summary>
        /// Obtener una interrupción según su codigo de evento
        /// </summary>
        /// <param name="evencodi"></param>
        /// <returns></returns>
        public EventoDTO ObtenerInterrupcionByEvencodi(int evencodi)
        {
            var reg = FactorySic.ObtenerEventoDao().ObtenerInterrupcionByEvencodi(evencodi);
            reg.FechaEventoDesc = reg.FechaEvento.ToString(ConstantesAppServicio.FormatoFechaFull2);
            return reg;
        }

        /// <summary>
        /// Método para completar las fechas de interrupción y de plazo
        /// </summary>
        /// <param name="oEventoDTO"></param>
        /// <returns></returns>
        public EventoDTO CompletaFechasInterrupcion(EventoDTO oEventoDTO)
        {
            if (oEventoDTO != null)
            {
                oEventoDTO.FechaInterrupcion = oEventoDTO.EVENINI.Value.AddSeconds(1).ToString(ConstantesBase.FormatoFechaFullBase);
                oEventoDTO.ValidFecha = 1;
            }
            return oEventoDTO;
        }


        /// <summary>
        /// Método para completar las fechas de interrupción y de plazo
        /// </summary>
        /// <param name="oEventoDTO"></param>
        /// <returns></returns>
        public EventoDTO CompletaFechasPlazo(EventoDTO oEventoDTO)
        {
            if (oEventoDTO != null)
            {
                var fechaFinPlazo = GetFechaFinPlazo(oEventoDTO.EVENINI.Value);
                oEventoDTO.FechaPlazoEnvio = fechaFinPlazo.ToString(ConstantesBase.FormatoFechaFullBase);
                oEventoDTO.ValidFecha = 1;
            }
            return oEventoDTO;
        }
        #endregion

        #region CARGAR ERACMF VIGENTE

        /// <summary>
        /// Obtener datos del archivo excel a objeto Eracmf
        /// </summary>
        /// <param name="stremExcel"></param>
        /// <param name="evencodi"></param>
        /// <param name="user"></param>
        /// <param name="listaCorrecta"></param>
        /// <param name="listaErrores"></param>
        /// <returns></returns>
        public void ObtenerDataExcelERACMF(Stream stremExcel, string evencodi, string user, out List<AfEracmfEventoDTO> listaCorrecta, out List<AfEracmfEventoDTO> listaErrores)
        {
            DateTime fechaActualizacion = DateTime.Now;

            List<AfEracmfEventoDTO> listaEracmf = new List<AfEracmfEventoDTO>();
            List<AfEracmfEventoDTO> listaEracmfErrores = new List<AfEracmfEventoDTO>();

            using (var xlPackage = new ExcelPackage(stremExcel))
            {
                var ws = xlPackage.Workbook.Worksheets[1];
                var dim = ws.Dimension;

                //Dimensiones
                ExcelRange excelRangeCabecera = ws.Cells[dim.Start.Row, dim.Start.Column, dim.Start.Row, dim.End.Column];
                ExcelRange excelRangeData = ws.Cells[dim.Start.Row + 1, dim.Start.Column, dim.End.Row, dim.End.Column];

                var CabExcel = (object[,])excelRangeCabecera.Value;
                var dataExcel = (object[,])excelRangeData.Value;

                var ultimaFilatBloque = dim.End.Row - dim.Start.Row - 1;

                var coluIniBloque = dim.Start.Column;
                var coluFinBloque = dim.End.Column;

                int filaCabecera = ConstantesEracmf.FilaInicialExcelEracmf;

                //Obtenemos las cabeceras y su posicion
                var dictionary = new Dictionary<string, int>();

                for (int col = coluIniBloque - 1; col < coluFinBloque; col++)
                {
                    var nomCab = CabExcel[filaCabecera, col].ToString().Trim();
                    if (nomCab != "")
                        dictionary.Add(nomCab, col);
                }

                for (int fila = filaCabecera; fila <= ultimaFilatBloque; fila++)
                {

                    var empresa = dataExcel[fila, dictionary[ConstantesEracmf.Empresa]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var zona = dataExcel[fila, dictionary[ConstantesEracmf.Zona]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var codRele = dataExcel[fila, dictionary[ConstantesEracmf.CodRele]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var marca = dataExcel[fila, dictionary[ConstantesEracmf.Marca]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var modelo = dataExcel[fila, dictionary[ConstantesEracmf.Modelo]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var serie = dataExcel[fila, dictionary[ConstantesEracmf.Serie]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var subestacion = dataExcel[fila, dictionary[ConstantesEracmf.Subestacion]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var nivelTension = dataExcel[fila, dictionary[ConstantesEracmf.NivelTension]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var circAlimentador = dataExcel[fila, dictionary[ConstantesEracmf.CircAlimentador]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var codInterruptor = dataExcel[fila, dictionary[ConstantesEracmf.CodInterruptor]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var nEtapa = dataExcel[fila, dictionary[ConstantesEracmf.NEtapa]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var arranqueU = dataExcel[fila, dictionary[ConstantesEracmf.ArranqueU]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var tiempoU = dataExcel[fila, dictionary[ConstantesEracmf.TiempoU]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var arranqueD = dataExcel[fila, dictionary[ConstantesEracmf.ArranqueD]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var dfdt = dataExcel[fila, dictionary[ConstantesEracmf.Dfdt]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var tiempoD = dataExcel[fila, dictionary[ConstantesEracmf.TiempoD]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var maxRegistrada = dataExcel[fila, dictionary[ConstantesEracmf.MaxRegistrada]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var medRegistrada = dataExcel[fila, dictionary[ConstantesEracmf.MedRegistrada]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var minRegistrada = dataExcel[fila, dictionary[ConstantesEracmf.MinRegistrada]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var referencia = dataExcel[fila, dictionary[ConstantesEracmf.Referencia]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var suministrador = dataExcel[fila, dictionary[ConstantesEracmf.Suministrador]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var observaciones = dataExcel[fila, dictionary[ConstantesEracmf.Observaciones]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var fecImplem = dataExcel[fila, dictionary[ConstantesEracmf.FecImplem]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var fecIngreso = dataExcel[fila, dictionary[ConstantesEracmf.FecIngreso]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var fecRetiro = dataExcel[fila, dictionary[ConstantesEracmf.FecRetiro]].ToString().Replace("?", "").Replace("#", "").Trim();
                    var tipoRegistro = dataExcel[fila, dictionary[ConstantesEracmf.TipoRegistro]].ToString().Replace("?", "").Replace("#", "").Trim();

                    decimal valorarranqueU;
                    decimal valortiempoU;
                    decimal valorarranqueD;
                    decimal valordfdt;
                    decimal valortiempoD;
                    decimal valormaxRegistrada;
                    decimal valormedRegistrada;
                    decimal valorminRegistrada;
                    decimal valorreferencia;

                    #region Validación de campos

                    this.AgregarErrorArchivoEracmf(fila, ConstantesEracmf.ArranqueU, arranqueU, out valorarranqueU, ref listaEracmfErrores);
                    this.AgregarErrorArchivoEracmf(fila, ConstantesEracmf.TiempoU, tiempoU, out valortiempoU, ref listaEracmfErrores);
                    this.AgregarErrorArchivoEracmf(fila, ConstantesEracmf.ArranqueD, arranqueD, out valorarranqueD, ref listaEracmfErrores);
                    this.AgregarErrorArchivoEracmf(fila, ConstantesEracmf.Dfdt, dfdt, out valordfdt, ref listaEracmfErrores);
                    this.AgregarErrorArchivoEracmf(fila, ConstantesEracmf.TiempoD, tiempoD, out valortiempoD, ref listaEracmfErrores);
                    this.AgregarErrorArchivoEracmf(fila, ConstantesEracmf.MaxRegistrada, maxRegistrada, out valormaxRegistrada, ref listaEracmfErrores);
                    this.AgregarErrorArchivoEracmf(fila, ConstantesEracmf.MedRegistrada, medRegistrada, out valormedRegistrada, ref listaEracmfErrores);
                    this.AgregarErrorArchivoEracmf(fila, ConstantesEracmf.MinRegistrada, minRegistrada, out valorminRegistrada, ref listaEracmfErrores);
                    this.AgregarErrorArchivoEracmf(fila, ConstantesEracmf.Referencia, referencia, out valorreferencia, ref listaEracmfErrores);

                    #endregion

                    listaEracmf.Add(new AfEracmfEventoDTO()
                    {
                        Evencodi = int.Parse(evencodi),
                        Eracmfemprnomb = empresa,
                        Eracmfzona = zona,
                        Eracmfcodrele = codRele,
                        Eracmfmarca = marca,
                        Eracmfmodelo = modelo,
                        Eracmfnroserie = serie,
                        Eracmfsubestacion = subestacion,
                        Eracmfnivtension = nivelTension,
                        Eracmfciralimentador = circAlimentador,
                        Eracmfcodinterruptor = codInterruptor,
                        Eracmfnumetapa = nEtapa,

                        Eracmfarranqrumbral = valorarranqueU,
                        Eracmftiemporumbral = valortiempoU,
                        Eracmfarranqrderivada = valorarranqueD,
                        Eracmfdfdtrderivada = valordfdt,
                        Eracmftiemporderivada = valortiempoD,
                        Eracmfmaxdregistrada = valormaxRegistrada,
                        Eracmfmediadregistrada = valormedRegistrada,
                        Eracmfmindregistrada = valorminRegistrada,
                        Eracmfdreferencia = valorreferencia,

                        Eracmfsuministrador = suministrador,
                        Eracmfobservaciones = observaciones,
                        Eracmffechimplementacion = fecImplem,
                        Eracmffechingreso = fecIngreso,
                        Eracmffechretiro = fecRetiro,
                        Eracmftiporegistro = tipoRegistro,
                        Eracmfusucreacion = user,
                        Eracmffeccreacion = fechaActualizacion,
                        Eracmfusumodificacion = user,
                        Eracmffecmodificacion = fechaActualizacion,
                    });
                }
            }

            listaCorrecta = listaEracmf;
            listaErrores = listaEracmfErrores;
        }

        /// <summary>
        /// Reporte html de lista del Eracmf
        /// </summary>
        /// <param name="listaEracmf"></param>
        /// <returns></returns>
        public string ObtenerTablaERACMFhtml(List<AfEracmfEventoDTO> listaEracmf)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table id='tabla' class='pretty tabla-adicional'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style=''>EMPRESA</th>");
            strHtml.Append("<th style=''>ZONA</th>");
            strHtml.Append("<th style=''>COD. RELE</th>");
            strHtml.Append("<th style=''>MARCA</th>");
            strHtml.Append("<th style=''>MODELO</th>");
            strHtml.Append("<th style=''>NRO SERIE</th>");
            strHtml.Append("<th style=''>SUBESTACION</th>");
            strHtml.Append("<th style=''> NIVEL TENSION <br/> KV(KV) </th> ");

            strHtml.Append("<th style=''>  CIRCUITO ALIMENTADOR</th>   ");
            strHtml.Append("<th style=''> CODIGO INTERRUPTOR</th>      ");
            strHtml.Append("<th style=''>NUM.ETAPA</th>                 ");
            strHtml.Append("<th style=''> ARRANQUE(Hz) <br/> R.UMBRAL</th>   ");
            strHtml.Append("<th style=''> TIEMPO(seg) <br/> R.UMBRAL</th>    ");
            strHtml.Append("<th style=''> ARRANQUE(Hz) <br/> R.DERIVADA</th> ");
            strHtml.Append("<th style=''> DF/DT(Hz/s) <br/> R.DERIVADA</th>  ");
            strHtml.Append("<th style=''> TIEMPO(seg) <br/> R.DERIVADA</th>  ");
            strHtml.Append("<th style=''>  MAXIMA(MW) <br/> D.REGISTRADA</th>");
            strHtml.Append("<th style=''> MEDIA(MW) <br/> D.REGISTRADA</th> ");
            strHtml.Append("<th style=''> MINIMA(MW) <br/> D.REGISTRADA</th>");
            strHtml.Append("<th style=''> D.REFERENCIA(MW) </th>      ");

            strHtml.Append("<th  style=''>SUMINISTRADOR</th>                   ");
            strHtml.Append("<th  style=''>OBSERVACIONES</th>                   ");
            strHtml.Append("<th  style=''> FECHA DE <br/> IMPLEMENTACION </th>       ");
            strHtml.Append("<th  style=''> FECHA DE <br/> INGRESO </th>   ");
            strHtml.Append("<th  style=''>FECHA DE <br/> RETIRO</th>                  ");
            strHtml.Append("<th  style=''>TIPO <br/> REGISTRO</th>                   ");

            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");
            foreach (var fila in listaEracmf)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td>" + fila.Eracmfemprnomb + " </td>");
                strHtml.Append("<td>" + fila.Eracmfzona + " </td>");
                strHtml.Append("<td>" + fila.Eracmfcodrele + " </td>");
                strHtml.Append("<td>" + fila.Eracmfmarca + " </td>");
                strHtml.Append("<td>" + fila.Eracmfmodelo + " </td>");
                strHtml.Append("<td>" + fila.Eracmfnroserie + " </td>");
                strHtml.Append("<td>" + fila.Eracmfsubestacion + " </td>");
                strHtml.Append("<td style='text-align: center;'>" + fila.Eracmfnivtension + " </td>");
                strHtml.Append("<td>" + fila.Eracmfciralimentador + " </td>");
                strHtml.Append("<td>" + fila.Eracmfcodinterruptor + " </td>");
                strHtml.Append("<td style='text-align: center;'>" + fila.Eracmfnumetapa + " </td>");
                strHtml.Append("<td style='text-align: center;'>" + fila.Eracmfarranqrumbral + " </td>");
                strHtml.Append("<td style='text-align: center;'>" + fila.Eracmftiemporumbral + " </td>");
                strHtml.Append("<td style='text-align: center;'>" + fila.Eracmfarranqrderivada + " </td>");
                strHtml.Append("<td style='text-align: center;'>" + fila.Eracmfdfdtrderivada + " </td>");
                strHtml.Append("<td style='text-align: center;'>" + fila.Eracmftiemporderivada + " </td>");
                strHtml.Append("<td style='text-align: center;'>" + fila.Eracmfmaxdregistrada + " </td>");
                strHtml.Append("<td style='text-align: center;'>" + fila.Eracmfmediadregistrada + " </td>");
                strHtml.Append("<td style='text-align: center;'>" + fila.Eracmfmindregistrada + " </td>");
                strHtml.Append("<td style='text-align: center;'>" + fila.Eracmfdreferencia + " </td>");
                strHtml.Append("<td>" + fila.Eracmfsuministrador + " </td>");
                strHtml.Append("<td>" + fila.Eracmfobservaciones + " </td>");
                strHtml.Append("<td style='text-align: center;'>" + fila.Eracmffechimplementacion + " </td>");
                strHtml.Append("<td style='text-align: center;'>" + fila.Eracmffechingreso + " </td>");
                strHtml.Append("<td style='text-align: center;'>" + fila.Eracmffechretiro + " </td>");
                strHtml.Append("<td>" + fila.Eracmftiporegistro + " </td>");


                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");


            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// Reporte html de lista del Eracmf
        /// </summary>
        /// <param name="listaEracmf"></param>
        /// <returns></returns>
        public string ObtenerTablaERACMFErroresHtml(List<AfEracmfEventoDTO> listaEracmf)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table id='tablaErroresEracmf' class='pretty tabla-adicional'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th style=''>Número de fila</th>");
            strHtml.Append("<th style=''>Campo</th>");
            strHtml.Append("<th style=''>Valor</th>");
            strHtml.Append("<th style=''>Mensaje de Validación</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");
            foreach (var fila in listaEracmf)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td style='text-align: center;'>" + fila.NumeroFilaExcel + " </td>");
                strHtml.Append("<td>" + fila.CampoExcel + " </td>");
                strHtml.Append("<td>" + fila.ValorCeldaExcel + " </td>");
                strHtml.Append("<td>" + fila.MensajeValidacion + " </td>");
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");


            #endregion

            return strHtml.ToString();
        }

        private void AgregarErrorArchivoEracmf(int fila, string campo, string valorCelda, out decimal valorNumerico, ref List<AfEracmfEventoDTO> listaErrores)
        {
            if (!decimal.TryParse(valorCelda, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out valorNumerico))
            {
                AfEracmfEventoDTO error = new AfEracmfEventoDTO();
                error.NumeroFilaExcel = fila + 1;
                error.CampoExcel = campo;
                error.ValorCeldaExcel = valorCelda;
                error.MensajeValidacion = ConstantesEracmf.MensajeValidacionNumero;

                listaErrores.Add(error);
            }
        }

        /// <summary>
        /// Buscar AFEracmf Evento By Evencodi
        /// </summary>
        /// <param name="evencodiEracmfCargado"></param>
        /// <returns></returns>
        public List<AfEracmfEventoDTO> BuscarAFEracmfEventoByEvencodi(int evencodiEracmfCargado)
        {
            List<AfEracmfEventoDTO> lstEranmf = new List<AfEracmfEventoDTO>();

            lstEranmf = GetByEvencodiAfEracmfEventos(evencodiEracmfCargado);

            return lstEranmf;
        }

        /// <summary>
        /// Eliminar AFEracmf Evento By Evencodi
        /// </summary>
        /// <param name="evencodiEracmfCargado"></param>
        public void EliminarAFEracmfEventoByEvencodi(int evencodiEracmfCargado)
        {
            List<AfEracmfEventoDTO> lstEranmfBorrar = BuscarAFEracmfEventoByEvencodi(evencodiEracmfCargado);

            if (lstEranmfBorrar.Any())
            {
                foreach (var item in lstEranmfBorrar)
                {
                    DeleteAfEracmfEvento(item.Eracmfcodi);
                }
            }

        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_FUENTEDATOS
        /// </summary>
        public List<SiFuentedatosDTO> ListSiFuentedatosByFdatpadre(int fdatpadre)
        {
            return FactorySic.GetSiFuentedatosRepository().List().Where(x => x.Fdatpadre == fdatpadre).OrderBy(x => x.Fdatcodi).ToList();
        }

        /// <summary>
        /// Mostrar el Eracmf Vigente en reporte web By codigo
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="reporteHtml"></param>
        /// <param name="usuarioActualizacion"></param>
        /// <param name="fechaActualizacion"></param>
        public void ListarERACMFVigente(int idEvento, out string reporteHtml, out string usuarioActualizacion, out string fechaActualizacion)
        {
            List<AfEracmfEventoDTO> listaCircuito = this.BuscarAFEracmfEventoByEvencodi(idEvento);

            this.ListarERACMFVigenteByData(listaCircuito, out reporteHtml, out usuarioActualizacion, out fechaActualizacion);
        }

        /// <summary>
        /// Mostrar el Eracmf Vigente en reporte web segun data
        /// </summary>
        /// <param name="listaCircuito"></param>
        /// <param name="reporteHtml"></param>
        /// <param name="usuarioActualizacion"></param>
        /// <param name="fechaActualizacion"></param>
        public void ListarERACMFVigenteByData(List<AfEracmfEventoDTO> listaCircuito, out string reporteHtml, out string usuarioActualizacion, out string fechaActualizacion)
        {
            usuarioActualizacion = string.Empty;
            fechaActualizacion = string.Empty;

            //campos de auditoria
            var reg = listaCircuito.FirstOrDefault();
            if (reg != null)
            {
                string feccreacionDesc = reg.Eracmffeccreacion != null ? reg.Eracmffeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : string.Empty;
                string fecmodificacionDesc = reg.Eracmffecmodificacion != null ? reg.Eracmffecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : string.Empty;
                usuarioActualizacion = reg.Eracmffecmodificacion != null ? reg.Eracmfusumodificacion : reg.Eracmfusucreacion;
                fechaActualizacion = reg.Eracmffecmodificacion != null ? fecmodificacionDesc : feccreacionDesc;
            }

            //reporte
            reporteHtml = this.ObtenerTablaERACMFhtml(listaCircuito);
        }

        #endregion

        #region CONFIGURACION EMPRESAS

        /// <summary>
        /// Es Empresa Activo
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public bool EsEmpresaActivo(int emprcodi)
        {
            var empresa = FactorySic.GetSiEmpresaRepository().GetById(emprcodi);
            return empresa.EmpresaEstado == ConstantesAppServicio.Activo;
        }

        /// <summary>
        /// Obtener Lista Empresas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerListaEmpresas()
        {
            return FactorySic.GetSiEmpresaRepository().ListarEmpresasEventoCTAF();
        }

        /// <summary>
        /// Devuelve lista de emrpesas segun el tipoEmpresa
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasPorTipoEmpresa(int idTipoEmpresa)
        {
            List<SiEmpresaDTO> lstEmpresasXTipo = new List<SiEmpresaDTO>();

            List<SiEmpresaDTO> listaEmpresasTotal = ObtenerListaEmpresas();
            lstEmpresasXTipo = listaEmpresasTotal.Where(x => x.Tipoemprcodi == idTipoEmpresa).ToList();

            return lstEmpresasXTipo;
        }

        /// <summary>
        /// Devuelve la tabla con el listado (en html) de las configuraciones de empresas
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        public string ListarConfiguracionEmpresasHtml(string idTipoEmpresa, string url)
        {
            StringBuilder strHtml = new StringBuilder();

            List<EmpresaReporte> empresasTabla = ObtenerListadoConfiguracionEmpresa(idTipoEmpresa);

            strHtml.Append("<table border='0' class='pretty tabla-icono' cellspacing='0' width='100 %' id='tabla'>");
            #region cabecera

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th>Acciones</th>");
            strHtml.Append("<th>Tipo de Empresa</th>");
            strHtml.Append("<th>Empresa SICOES</th>");
            strHtml.Append("<th>Empresa archivo ERACMF</th>");
            strHtml.Append("<th>Código OSINERGMIN</th>");
            strHtml.Append("<th>Usuario actualización</th>");
            strHtml.Append("<th>Fecha actualización</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            if (empresasTabla.Any())
            {
                //Ordeno la lista de emrpesas segun TipoEmpres, EmpresaCoes, EmpresaEracmf                
                empresasTabla = empresasTabla.OrderBy(x => x.TipoEmpresa).ThenBy(x => x.EmpresaSICCOES).ThenBy(x => x.EmpresaERACMF).ToList();

                #region cuerpo
                strHtml.Append("<tbody>");
                foreach (var item in empresasTabla)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append("<td style=''>");
                    strHtml.AppendFormat("<a href = 'JavaScript:editarEquipo({0});' title = 'Editar Configuración' class=''><img src ='{1}Content/Images/btn-edit.png' alt='' height='24px' width='24px'></a> ", item.Afemprcodi, url);
                    strHtml.AppendFormat("<a href = 'JavaScript:eliminarConfiguracion({0});' title='Eliminar Configuración'><img src = '{1}Content/Images/btn-cancel.png' alt='' height='24px' width='24px'></a>", item.Afemprcodi, url);
                    strHtml.Append("</td>");
                    strHtml.AppendFormat("<td >{0}</td>", item.TipoEmpresa);
                    strHtml.AppendFormat("<td >{0}</td>", item.EmpresaSICCOES);
                    strHtml.AppendFormat("<td style='text-align: left' >{0}</td>", item.EmpresaERACMF);
                    strHtml.AppendFormat("<td >{0}</td>", item.CodigoOsinergmin);
                    strHtml.AppendFormat("<td >{0}</td>", item.Usuario);
                    strHtml.AppendFormat("<td >{0}</td>", item.Fecha);
                    strHtml.Append("</tr>");
                }
                strHtml.Append("</tbody>");

                #endregion
            }

            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Obtiene los valores de cada registro de las configuraciones de empresas
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        private List<EmpresaReporte> ObtenerListadoConfiguracionEmpresa(string idTipoEmpresa)
        {
            EventoAppServicio servEve = new EventoAppServicio();

            List<EmpresaReporte> empresasTabla = new List<EmpresaReporte>();

            List<SiEmpresaDTO> lstEmpresasEventos = new List<SiEmpresaDTO>();
            List<SiEmpresaDTO> lstEmpresaXTipo = new List<SiEmpresaDTO>();

            List<AfEmpresaDTO> lstAfEmpresas = ListAfEmpresas();

            //Si hay registros en la base de datos para la tabla AfEmpresa
            if (lstAfEmpresas.Any())
            {
                //Obtengo todos los tipos emrpesas
                var tipoEmpresas = servEve.ListarTipoEmpresas();

                //Obtengo la lista de empresas de todos los eventos
                lstEmpresasEventos = ObtenerListaEmpresas();

                if (idTipoEmpresa == ConstantesAppServicio.ParametroDefecto)
                    lstEmpresaXTipo = lstEmpresasEventos; //para TODOS los tipos
                else
                    lstEmpresaXTipo = lstEmpresasEventos.Where(x => x.Tipoemprcodi == Int32.Parse(idTipoEmpresa)).ToList(); //por cada tipoEmpresa


                if (lstEmpresaXTipo.Any())
                {
                    List<int> lstEmprcodis = lstEmpresaXTipo.Select(x => x.Emprcodi).Distinct().ToList(); //Lista emprcodis de las empresas por tipo

                    var lstEmpresaTipo = lstEmpresaXTipo.Select(x => new SiEmpresaDTO() { Emprcodi = x.Emprcodi, Emprnomb = x.Emprnomb, Tipoemprcodi = x.Tipoemprcodi }).ToList();

                    var lstE = lstAfEmpresas.Where(x => lstEmprcodis.Contains(x.Emprcodi)).ToList(); // Lista empresas eventos por tipo

                    foreach (var item in lstE)
                    {
                        var tipoEmprcodi = lstEmpresaTipo.Where(x => x.Emprcodi == item.Emprcodi).First().Tipoemprcodi;
                        var nombreEmpresa = lstEmpresaTipo.Where(x => x.Emprcodi == item.Emprcodi).First().Emprnomb;


                        EmpresaReporte objEmpresa = new EmpresaReporte();
                        var tipoEmpresa = tipoEmpresas.Where(x => x.Tipoemprcodi == tipoEmprcodi).First().Tipoemprdesc;
                        var empresaSICCOES = nombreEmpresa;
                        var empresaERACMF = item.Afemprnomb;
                        var codigoOsinergmin = item.Afemprosinergmin;
                        var emprcodi = item.Emprcodi;
                        var afemprcodi = item.Afemprcodi;

                        objEmpresa.TipoEmpresa = tipoEmpresa != null ? tipoEmpresa.Trim() : "";
                        objEmpresa.EmpresaSICCOES = empresaSICCOES != null ? empresaSICCOES.Trim() : "";
                        objEmpresa.EmpresaERACMF = empresaERACMF != null ? empresaERACMF.Trim() : "";
                        objEmpresa.CodigoOsinergmin = codigoOsinergmin != null ? codigoOsinergmin.Trim() : "";
                        objEmpresa.Usuario = item.Afemprfecmodificacion != null ? item.Afemprusumodificacion : item.Afemprusucreacion;
                        objEmpresa.Fecha = item.Afemprfecmodificacion != null ? item.Afemprfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : item.Afemprfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
                        objEmpresa.Emprcodi = emprcodi;
                        objEmpresa.Afemprcodi = afemprcodi;

                        empresasTabla.Add(objEmpresa);
                    }
                }
            }

            return empresasTabla;
        }

        /// <summary>
        /// Obtiene los valores de cada registro de las configuraciones de empresas
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        public List<EmpresaReporte> ObtenerListadoConfiguracionEmpresaComboBox()
        {
            string idTipoEmpresa = "-1";
            EventoAppServicio servEve = new EventoAppServicio();

            List<EmpresaReporte> empresasTabla = new List<EmpresaReporte>();

            List<SiEmpresaDTO> lstEmpresasEventos = new List<SiEmpresaDTO>();
            List<SiEmpresaDTO> lstEmpresaXTipo = new List<SiEmpresaDTO>();

            List<AfEmpresaDTO> lstAfEmpresas = ListAfEmpresas();

            //Si hay registros en la base de datos para la tabla AfEmpresa
            if (lstAfEmpresas.Any())
            {
                //Obtengo todos los tipos emrpesas
                var tipoEmpresas = servEve.ListarTipoEmpresas();

                //Obtengo la lista de empresas de todos los eventos
                lstEmpresasEventos = ObtenerListaEmpresas();

                if (idTipoEmpresa == ConstantesAppServicio.ParametroDefecto)
                    lstEmpresaXTipo = lstEmpresasEventos; //para TODOS los tipos
                else
                    lstEmpresaXTipo = lstEmpresasEventos.Where(x => x.Tipoemprcodi == Int32.Parse(idTipoEmpresa)).ToList(); //por cada tipoEmpresa


                if (lstEmpresaXTipo.Any())
                {
                    List<int> lstEmprcodis = lstEmpresaXTipo.Select(x => x.Emprcodi).Distinct().ToList(); //Lista emprcodis de las empresas por tipo

                    var lstEmpresaTipo = lstEmpresaXTipo.Select(x => new SiEmpresaDTO() { Emprcodi = x.Emprcodi, Emprnomb = x.Emprnomb, Tipoemprcodi = x.Tipoemprcodi }).ToList();

                    var lstE = lstAfEmpresas.Where(x => lstEmprcodis.Contains(x.Emprcodi)).ToList(); // Lista empresas eventos por tipo

                    foreach (var item in lstE)
                    {
                        var tipoEmprcodi = lstEmpresaTipo.Where(x => x.Emprcodi == item.Emprcodi).First().Tipoemprcodi;
                        var nombreEmpresa = lstEmpresaTipo.Where(x => x.Emprcodi == item.Emprcodi).First().Emprnomb;


                        EmpresaReporte objEmpresa = new EmpresaReporte();
                        var tipoEmpresa = tipoEmpresas.Where(x => x.Tipoemprcodi == tipoEmprcodi).First().Tipoemprdesc;
                        var empresaSICCOES = nombreEmpresa;
                        var empresaERACMF = item.Afemprnomb;
                        var codigoOsinergmin = item.Afemprosinergmin;
                        var emprcodi = item.Emprcodi;
                        var afemprcodi = item.Afemprcodi;

                        objEmpresa.TipoEmpresa = tipoEmpresa != null ? tipoEmpresa.Trim() : "";
                        objEmpresa.EmpresaSICCOES = empresaSICCOES != null ? empresaSICCOES.Trim() : "";
                        objEmpresa.EmpresaERACMF = empresaERACMF != null ? empresaERACMF.Trim() : "";
                        objEmpresa.CodigoOsinergmin = codigoOsinergmin != null ? ("CC-" + codigoOsinergmin.Trim()) : "";
                        objEmpresa.Usuario = item.Afemprfecmodificacion != null ? item.Afemprusumodificacion : item.Afemprusucreacion;
                        objEmpresa.Fecha = item.Afemprfecmodificacion != null ? item.Afemprfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : item.Afemprfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
                        objEmpresa.Emprcodi = emprcodi;
                        objEmpresa.Afemprcodi = afemprcodi;

                        empresasTabla.Add(objEmpresa);
                    }
                }
            }

            return empresasTabla;
        }

        /// <summary>
        /// Devuelve la lista de tipos de empresas y la relacion de empresas por tipo
        /// </summary>
        /// <param name="lstTiposAUsar"></param>
        /// <param name="lstEmpresasAUsar"></param>
        public void ObtenerListadoEmpresasYTipos(out List<SiTipoempresaDTO> lstTiposAUsar, out List<SiEmpresaDTO> lstEmpresasAUsar)
        {
            GeneralAppServicio appGeneral = new GeneralAppServicio();

            var lstTipos = appGeneral.ListarTiposEmpresa().Where(t => t.Tipoemprcodi > 0).ToList();
            var lstEmpresas = ObtenerListaEmpresas();

            List<int> lstTiposEmpresa = lstEmpresas.Select(x => x.Tipoemprcodi).Distinct().ToList();

            var listaTipos = lstTipos.Where(x => lstTiposEmpresa.Contains(x.Tipoemprcodi)).OrderBy(x => x.Tipoemprdesc).ToList();
            var listaEmpresas = lstEmpresas.Where(x => x.Tipoemprcodi == listaTipos.First().Tipoemprcodi).OrderBy(x => x.Emprnomb).ToList();

            lstTiposAUsar = listaTipos;
            lstEmpresasAUsar = listaEmpresas;
        }

        /// <summary>
        /// Guarda una nueva configuracion de empresas
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="codEracmf"></param>
        /// <param name="codOsinergmin"></param>
        /// <param name="usuario"></param>
        /// <param name="afalerta"></param>
        public void GuardarNuevaConfiguracion(string emprcodi, string codEracmf, string codOsinergmin, string usuario, string afalerta)
        {
            AfEmpresaDTO objEmpresa = new AfEmpresaDTO();

            objEmpresa.Emprcodi = Int32.Parse(emprcodi);
            objEmpresa.Afemprnomb = codEracmf.Trim();
            objEmpresa.Afemprosinergmin = codOsinergmin.Trim();
            objEmpresa.Afemprestado = 1;
            objEmpresa.Afemprusucreacion = usuario;
            objEmpresa.Afemprfeccreacion = DateTime.Now;
            objEmpresa.Afemprusumodificacion = usuario;
            objEmpresa.Afemprfecmodificacion = DateTime.Now;
            objEmpresa.Afalerta = afalerta;

            SaveAfEmpresa(objEmpresa);
        }

        /// <summary>
        /// Verifica si existen registros con los datos ingresados
        /// </summary>
        /// <param name="codEracmf"></param>
        /// <param name="codOsinergmin"></param>
        /// <param name="existeConfig"></param>
        /// <param name="existeCodOsinergmin"></param>
        /// <returns></returns>
        public void VerificarExistenciaConfiguracion(string codEracmf, string codOsinergmin, out bool existeConfig, out bool existeCodOsinergmin)
        {
            existeConfig = false;
            existeCodOsinergmin = false;
            List<AfEmpresaDTO> lstAfEmpresas = ListAfEmpresas();

            //Verifico que no existan configuraciones similares

            AfEmpresaDTO empresaEncontrada = lstAfEmpresas.Find(x => x.Afemprnomb.ToUpper() == codEracmf.ToUpper() && (x.Afemprosinergmin != null ? x.Afemprosinergmin.ToUpper() : "") == (codOsinergmin != "" ? codOsinergmin.ToUpper() : codOsinergmin));
            if (empresaEncontrada != null)
                existeConfig = true;

            //Verifico si ya existe un condigo Osinergmin igual al ingresado
            if (codOsinergmin != "")
            {
                var lstCodOsinergmin = lstAfEmpresas.Select(x => x.Afemprosinergmin).Distinct().ToList();
                if (lstCodOsinergmin.Any())
                {
                    var temp = lstCodOsinergmin.Find(x => (x != null ? x.ToUpper() : "") == codOsinergmin.ToUpper());
                    if (temp != null)
                        existeCodOsinergmin = true;
                }
            }
        }

        /// <summary>
        /// Verifica si existen registros con los datos ingresados (tomo en cuenta tods menos el que estoy editando)
        /// </summary>
        /// <param name="afemprcodi"></param>
        /// <param name="codEracmf"></param>
        /// <param name="codOsinergmin"></param>
        /// <param name="existeConfig"></param>
        /// <param name="existeCodOsinergmin"></param>
        /// <returns></returns>
        public void VerificarExistenciaEdicionConfiguracion(int afemprcodi, string codEracmf, string codOsinergmin, out bool existeConfig, out bool existeCodOsinergmin)
        {
            existeConfig = false;
            existeCodOsinergmin = false;
            List<AfEmpresaDTO> lstAfEmpresas = ListAfEmpresas().Where(x => x.Afemprcodi != afemprcodi).ToList(); //todos menos el editado


            //Verifico que no existan configuraciones similares

            AfEmpresaDTO empresaEncontrada = lstAfEmpresas.Find(x => x.Afemprnomb.ToUpper() == codEracmf.ToUpper() && (x.Afemprosinergmin != null ? x.Afemprosinergmin.ToUpper() : "") == (codOsinergmin != "" ? codOsinergmin.ToUpper() : codOsinergmin));
            if (empresaEncontrada != null)
                existeConfig = true;

            //Verifico si ya existe un condigo Osinergmin igual al ingresado
            if (codOsinergmin != "")
            {
                var lstCodOsinergmin = lstAfEmpresas.Select(x => x.Afemprosinergmin).Distinct().ToList();
                if (lstCodOsinergmin.Any())
                {
                    var temp = lstCodOsinergmin.Find(x => (x != null ? x.ToUpper() : "") == codOsinergmin.ToUpper());
                    if (temp != null)
                        existeCodOsinergmin = true;
                }
            }
        }

        /// <summary>
        /// Guarda  la edicion de la configuracion
        /// </summary>
        /// <param name="afemprcodi"></param>
        /// <param name="codEracmf"></param>
        /// <param name="codOsinergmin"></param>
        /// <param name="usuario"></param>
        /// <param name="afalerta"></param>
        public void GuardarEdicionConfiguracion(int afemprcodi, string codEracmf, string codOsinergmin, string usuario, string afalerta)
        {
            AfEmpresaDTO objEmp = GetByIdAfEmpresa(afemprcodi);

            objEmp.Afemprnomb = codEracmf;
            objEmp.Afemprosinergmin = codOsinergmin;
            objEmp.Afemprfecmodificacion = DateTime.Now;
            objEmp.Afemprusumodificacion = usuario;
            objEmp.Afalerta = afalerta;

            UpdateAfEmpresa(objEmp);
        }

        #endregion

        #region Proceso de Asignación de Responsabilidad

        /// <summary>
        /// Obtener Dias Habiles
        /// </summary>
        /// <param name="fechEvento"></param>
        /// <param name="fechSolicitud"></param>
        /// <returns></returns>
        public int ObtenerDiasHabiles(DateTime fechEvento, DateTime fechSolicitud)
        {
            int cantidadDias = 0;
            var listaFeriados = FactorySic.GetDocDiaEspRepository().List();

            for (DateTime day = fechEvento; day <= fechSolicitud; day = day.AddDays(1))
            {
                //calculos feriados
                if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday || servGeneral.EsFeriadoByFecha(day.Date, listaFeriados))
                {
                    continue;
                }
                cantidadDias++;
            }

            return cantidadDias;
        }

        /// <summary>
        /// Consultar Solicitudes Asignacion
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public List<AfSolicitudRespDTO> ConsultarSolicitudesAsignacion(AfSolicitudRespDTO solicitud)
        {
            var lista = FactorySic.GetAfSolicitudRespRepository().ListarSolicitudesxFiltro(solicitud).OrderByDescending(x => x.Sorespfeccreacion).ToList();

            foreach (var reg in lista)
            {
                this.FormatearObjAfSolicitudResp(reg);
            }

            return lista;
        }

        /// <summary>
        /// Formatear objeto AfSolicitudResp
        /// </summary>
        /// <param name="reg"></param>
        private void FormatearObjAfSolicitudResp(AfSolicitudRespDTO reg)
        {
            reg.SorespfechaeventoDesc = reg.Sorespfechaevento.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
            reg.SorespfeccreacionDesc = reg.Sorespfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
            reg.SorespfecmodificacionDesc = reg.Sorespfecmodificacion != null ? reg.Sorespfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
            reg.Estado = reg.Sorespestadosol == "P" ? "Pendiente" : "Atendido";
            reg.Emprnomb = reg.Emprnomb != null ? reg.Emprnomb.Trim() : string.Empty;
        }

        /// <summary>
        /// Generar Excel Reporte de Reducción de suministros
        /// </summary>
        /// <param name="solicitudDTO"></param>
        /// <param name="esExportacionIntranet"></param>
        /// <returns></returns>
        public string GenerarReporteSolicitudesExcel(AfSolicitudRespDTO solicitudDTO, bool esExportacionIntranet)
        {
            List<AfSolicitudRespDTO> listSolicitudes = this.ConsultarSolicitudesAsignacion(solicitudDTO);


            string rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesExtranetCTAF.RutaReportes;
            string nombPlantilla = ConstantesExtranetCTAF.PlantillaReporteSolicitud;
            string nombCompletPlantilla = $"{rutaBase}{nombPlantilla}";
            FileInfo archivoPlantilla = new FileInfo(nombCompletPlantilla);

            string rutaOutput = AppDomain.CurrentDomain.BaseDirectory + ConstantesExtranetCTAF.RutaReportes;
            string nombFormato = $"ReporteSolicitudesProcesoAsignaciónResponsabilidad.xlsx";
            string nombCompletFormato = $"{rutaOutput}{nombFormato}";
            FileInfo nuevoArchivo = new FileInfo(nombCompletFormato);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombCompletFormato);
            }

            using (var xlPackage = new ExcelPackage(archivoPlantilla))
            {
                ExcelWorksheet ws = null;

                //TOTAL DE DATOS
                ws = xlPackage.Workbook.Worksheets[1];
                this.GenerarRptSolicitudHojaExcel(xlPackage, ref ws, listSolicitudes, 6, 2, esExportacionIntranet);

                if (ws != null)
                {
                    xlPackage.Workbook.View.ActiveTab = 0;
                    xlPackage.SaveAs(nuevoArchivo);
                }
            }

            return nombFormato;
        }

        /// <summary>
        /// Hoja del reporte de solicitudes
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="ws"></param>
        /// <param name="listSolicitudes"></param>
        /// <param name="filaIniCab"></param>
        /// <param name="coluIniCab"></param>
        /// <param name="esExportacionIntranet"></param>
        public void GenerarRptSolicitudHojaExcel(ExcelPackage xlPackage, ref ExcelWorksheet ws, List<AfSolicitudRespDTO> listSolicitudes, int filaIniCab, int coluIniCab, bool esExportacionIntranet)
        {
            int filaIniData = filaIniCab + 1;
            int coluIniData = coluIniCab;
            int coluFinData = esExportacionIntranet ? coluIniData + 7 : coluIniData + 5;

            if (listSolicitudes.Count > 0)
            {
                int filaX = 0;
                //data
                foreach (var reg in listSolicitudes)
                {
                    int colX = coluIniData;

                    int filActual = filaIniData + filaX;

                    ws.Cells[filActual, colX++].Value = reg.Emprnomb;
                    ws.Cells[filActual, colX++].Value = reg.SorespfechaeventoDesc;
                    ws.Cells[filActual, colX++].Value = reg.Sorespdesc;
                    ws.Cells[filActual, colX++].Value = reg.SorespfeccreacionDesc;
                    ws.Cells[filActual, colX++].Value = reg.Estado;
                    ws.Cells[filActual, colX++].Value = reg.Sorespusucreacion;

                    if (esExportacionIntranet)
                    {
                        ws.Cells[filActual, colX++].Value = reg.SorespfecmodificacionDesc;
                        ws.Cells[filActual, colX++].Value = reg.Sorespusumodificacion;
                    }

                    filaX++;
                }

                UtilExcel.BorderCeldas3(ws, filaIniData, coluIniData, filaIniData + filaX - 1, coluFinData);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniData, coluIniData, filaIniData + filaX - 1, coluFinData, "Centro");
            }

            ws.View.ZoomScale = 90;
        }

        #endregion

        #region Generación de Reportes

        #region REPORTES DE INTERRUPCION POR ACTIVACION DE ERACMF

        /// <summary>
        /// Generar Excel INTERRUPCION POR ACTIVACION DE ERACMF
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="anio"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public string GenerarReporte1ByInterrupcionERACMFExcel(int tipoReporte, int afecodi, int emprcodi, string anio, string correlativo)
        {
            bool esPorEracmf = true;

            //Data del reporte y datos del evento
            int fdatcodi = (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF;
            SiEmpresaDTO objEmpresa = emprcodi > 0 ? FactorySic.GetSiEmpresaRepository().GetById(emprcodi) : null;
            SiFuentedatosDTO objFuenteDatos = FactorySic.GetSiFuentedatosRepository().GetById(fdatcodi);
            EventoDTO regEvento = this.ObtenerInterrupcionByAfecodi(afecodi);

            //Data del reporte
            this.ListarInterrupcionSuministrosGral(afecodi, emprcodi, fdatcodi, out List<AfInterrupSuministroDTO> listaData, out List<AfInterrupSuministroDTO> listDataReportCero, out List<string> listaMsjValidacion, out List<int> listaEmprcodiReportaron, anio, correlativo);

            //Lista horas de coordinacion de normalización
            List<AfHoraCoordDTO> lstHandsonHorasCoordinacion = this.ObtenerListaCruceHoracoordInterrsuministro(afecodi, fdatcodi, listaData, anio, correlativo);

            //TOTAL DE DATOS
            this.ListarRptTotalDatos(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte1, out bool formatFechaCab1);
            TablaReporte tablaReporte1 = this.ObtenerDataExcelDatosTotales(listaReporte1, esPorEracmf, formatFechaCab1);

            //RESUMEN
            this.ListarRptResumen(afecodi, emprcodi, listaData, out List<ReporteInterrupcion> listaReporte2);
            TablaReporte tablaReporte2 = this.ObtenerDataExcelResumen(listaReporte2);

            //MALAS ACTUACIONES
            List<AfCondicionesDTO> listaFuncionesYEtapas = new List<AfCondicionesDTO>();
            var afecodis = listaData.Select(x => new { x.Afecodi, x.EVENCODI }).Distinct();
            foreach (var item in afecodis)
            {
                listaFuncionesYEtapas.AddRange(ListAfCondicioness(item.Afecodi, item.EVENCODI));
            }
            this.ListarRptMalasActuaciones(afecodi, emprcodi, listaData, out List<ReporteInterrupcion> listaReporte3);

            TablaReporte tablaReporte3 = this.ObtenerDataExcelMalasActuaciones(listaReporte3);

            //MENORES A 3 MINUTOS
            this.ListarRptMenores3Minutos(afecodi, emprcodi, listaData, null, out List<ReporteInterrupcion> listaReporte4, out bool formatFechaCab4);
            TablaReporte tablaReporte4 = this.ObtenerDataExcelMenores3minutos(listaReporte4, esPorEracmf, formatFechaCab4);

            //NO REPORTARON INTERRUPCIÓN
            this.ListarRptNoReportaronInterrupcion(afecodi, emprcodi, listaData, listaEmprcodiReportaron, out List<ReporteInterrupcion> listaReporte5, out List<string> listaMsjValidacionEracmf);
            TablaReporte tablaReporte5 = this.ObtenerDataExcelNoReportaronInterrupcion(listaReporte5);

            //PARA LA DECISION
            bool esRptActivacionEracmf = true;
            this.ListarRptDecision(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte6, out bool formatFechaCab6, esRptActivacionEracmf);
            TablaReporte tablaReporte6 = this.ObtenerDataExcelDecision(listaReporte6, formatFechaCab6);

            //REPORTARON 0
            esRptActivacionEracmf = true;
            this.ListarRptReportaron0(afecodi, emprcodi, listDataReportCero, out List<ReporteInterrupcion> listaReporte7);
            TablaReporte tablaReporte7 = this.ObtenerDataExcelReportaron0(listaReporte7);

            //AGENTE CON DEMORAS
            this.ListarRptTotalDatos(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte8, out bool formatFechaCab8);
            TablaReporte tablaReporte8 = this.ObtenerDataExcelAgenteDemoras(listaReporte8, lstHandsonHorasCoordinacion);

            //PARA EL RESARCIMIENTO
            this.ListarRptResarcimiento(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte9, out bool formatFechaCab, esRptActivacionEracmf);
            TablaReporte tablaReporte9 = this.ObtenerDataExcelResarcimiento(listaReporte9, formatFechaCab);


            //Plantilla y archivo salida
            string rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesExtranetCTAF.RutaReportes;
            string nombPlantilla = ConstantesExtranetCTAF.PlantillaReporteInterrupcionPorActivacionERACMF;
            string nombCompletPlantilla = $"{rutaBase}{nombPlantilla}";
            FileInfo archivoPlantilla = new FileInfo(nombCompletPlantilla);

            string rutaOutput = AppDomain.CurrentDomain.BaseDirectory + ConstantesExtranetCTAF.RutaReportes;
            string nombFormato = $"{regEvento.CODIGO}_ReporteInterrupciónPorActivaciónERACMF.xlsx";
            string nombCompletFormato = $"{rutaOutput}{nombFormato}";
            FileInfo nuevoArchivo = new FileInfo(nombCompletFormato);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombCompletFormato);
            }

            //Guardar excel
            using (var xlPackage = new ExcelPackage(archivoPlantilla))
            {
                ExcelWorksheet ws = null;
                int ultimaFila = 0;
                int coluIniCab = 3;

                //01. TOTAL DE DATOS
                this.AgregarHojaExcelGenerico(1, xlPackage, ref ws, tablaReporte1, regEvento, objEmpresa, objFuenteDatos, "Total de Datos", out ultimaFila);
                this.GenerarDetalleHoraNormalizacion(ref ws, lstHandsonHorasCoordinacion, ultimaFila, coluIniCab);

                //02. RESUMEN
                this.AgregarHojaExcelGenerico(2, xlPackage, ref ws, tablaReporte2, regEvento, objEmpresa, objFuenteDatos, "Reporte Resumen", out ultimaFila, 2);

                //03. MALAS ACTUACIONES
                this.AgregarHojaExcelGenerico(3, xlPackage, ref ws, tablaReporte3, regEvento, objEmpresa, objFuenteDatos, "Reporte Malas Actuaciones", out ultimaFila, 3, listaFuncionesYEtapas);
                //this.GenerarDetalleFuncionesYEtapas(ref ws, listaFuncionesYEtapas, ultimaFila, coluIniCab);

                //04. MENORES A 3 MINUTOS
                this.AgregarHojaExcelGenerico(4, xlPackage, ref ws, tablaReporte4, regEvento, objEmpresa, objFuenteDatos, "Interrupciones menores a 3 minutos", out ultimaFila);

                //05. REPORTARON 0
                this.AgregarHojaExcelGenerico(5, xlPackage, ref ws, tablaReporte7, regEvento, objEmpresa, objFuenteDatos, "Agentes que Reportaron 0", out ultimaFila);

                //06. NO REPORTARON INTERRUPCIÓN
                this.AgregarHojaExcelGenerico(6, xlPackage, ref ws, tablaReporte5, regEvento, objEmpresa, objFuenteDatos, "Agentes que no reportaron interrupciones por activación del ERACMF", out ultimaFila, 6);

                //07. AGENTE CON DEMORAS
                this.AgregarHojaExcelGenerico(7, xlPackage, ref ws, tablaReporte8, regEvento, objEmpresa, objFuenteDatos, "Agentes con Demoras", out ultimaFila);

                //08. PARA LA DECISION
                this.AgregarHojaExcelGenerico(8, xlPackage, ref ws, tablaReporte6, regEvento, objEmpresa, objFuenteDatos, "Interrupciones de suministros para la Decisión", out ultimaFila);

                //09. PARA EL RESARCIMIENTO
                this.AgregarHojaExcelGenerico(9, xlPackage, ref ws, tablaReporte9, regEvento, objEmpresa, objFuenteDatos, "Resarcimiento", out ultimaFila, 9);

                if (ws != null)
                {
                    xlPackage.Workbook.View.ActiveTab = 0;
                    xlPackage.SaveAs(nuevoArchivo);
                }
            }

            return nombFormato;
        }

        /// <summary>
        /// Generar Reporte web de INTERRUPCION POR ACTIVACION DE ERACMF
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listaReporteHtml"></param>
        /// <param name="lstHandsonHorasCoordinacion"></param>
        /// <param name="lstHandsonAgenteDemoras"></param>
        /// <param name="anio"></param>
        /// <param name="correlativo"></param>
        public void GenerarReporte1ByInterrupcionERACMF(int afecodi, int emprcodi, out List<string> listaReporteHtml, out List<AfHoraCoordDTO> lstHandsonHorasCoordinacion, out List<AfHoraCoordDTO> lstHandsonAgenteDemoras, string anio, string correlativo)
        {
            bool esPorEracmf = true;

            this.ListarInterrupcionSuministrosGral(afecodi, emprcodi, (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF, out List<AfInterrupSuministroDTO> listaData, out List<AfInterrupSuministroDTO> listDataReportCero, out List<string> listaMsjValidacion, out List<int> listaEmprcodiReportaron, anio, correlativo);

            //Lista horas de coordinacion de normalización
            lstHandsonHorasCoordinacion = ObtenerListaCruceHoracoordInterrsuministro(afecodi, (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF, listaData, anio, correlativo);

            //Lista de Agente con demoras
            var listaHorasCoordinacion = lstHandsonHorasCoordinacion;
            lstHandsonAgenteDemoras = ObtenerListaAgenteConDemoras(listaHorasCoordinacion, listaData);

            //TOTAL DE DATOS
            this.ListarRptTotalDatos(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte1, out bool formatFechaCab1);
            TablaReporte tablaReporte1 = this.ObtenerDataTablaRptTotalDatos(listaReporte1, lstHandsonHorasCoordinacion, esPorEracmf);
            string reporte1 = this.GenerarHtmlValidacion(listaMsjValidacion) + this.GenerarRptHtml(tablaReporte1, 1, 1);

            //RESUMEN
            this.ListarRptResumen(afecodi, emprcodi, listaData, out List<ReporteInterrupcion> listaReporte2);
            TablaReporte tablaReporte2 = this.ObtenerDataTablaRptResumen(listaReporte2);
            string reporte2 = this.GenerarHtmlValidacion(listaMsjValidacion) + this.GenerarRptHtmlResumen(tablaReporte2, 2);

            //MALAS ACTUACIONES
            this.ListarRptMalasActuaciones(afecodi, emprcodi, listaData, out List<ReporteInterrupcion> listaReporte3);
            TablaReporte tablaReporte3 = this.ObtenerDataTablaRptMalasActuaciones(listaReporte3, esPorEracmf);
            string reporte3 = this.GenerarHtmlValidacion(listaMsjValidacion) + this.GenerarRptHtml(tablaReporte3, 3, 1);

            //MENORES A 3 MINUTOS
            this.ListarRptMenores3Minutos(afecodi, emprcodi, listaData, null, out List<ReporteInterrupcion> listaReporte4, out bool formatFechaCab4);
            TablaReporte tablaReporte4 = this.ObtenerDataTablaRptMenores3Minutos(listaReporte4, esPorEracmf);
            string reporte4 = this.GenerarHtmlValidacion(listaMsjValidacion) + this.GenerarRptHtml(tablaReporte4, 4, 1);

            //NO REPORTARON INTERRUPCIÓN
            this.ListarRptNoReportaronInterrupcion(afecodi, emprcodi, listaData, listaEmprcodiReportaron, out List<ReporteInterrupcion> listaReporte5, out List<string> listaMsjValidacionEracmf);
            TablaReporte tablaReporte5 = this.ObtenerDataTablaRptNoReportaronInterrupcion(listaReporte5);
            listaMsjValidacionEracmf.AddRange(listaMsjValidacion);
            string reporte5 = this.GenerarHtmlValidacion(listaMsjValidacionEracmf) + this.GenerarRptHtml(tablaReporte5, 5, 1);

            //PARA LA DECISION
            bool esRptActivacionEracmf = true;
            this.ListarRptDecision(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte6, out bool formatFechaCab, esRptActivacionEracmf);
            TablaReporte tablaReporte6 = this.ObtenerDataTablaRptDecision(listaReporte6);
            string reporte6 = this.GenerarHtmlValidacion(listaMsjValidacion) + this.GenerarRptHtml(tablaReporte6, 6, 1);

            //REPORTARON 0
            this.ListarRptReportaron0(afecodi, emprcodi, listDataReportCero, out List<ReporteInterrupcion> listaReporte7);
            TablaReporte tablaReporte7 = this.ObtenerDataTablaRptReportaron0(listaReporte7);
            string reporte7 = this.GenerarHtmlValidacion(listaMsjValidacion) + this.GenerarRptHtml(tablaReporte7, 7, 1);

            //PARA EL RESARCIMIENTO
            this.ListarRptResarcimiento(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte8, out bool formatFechaCab2, esRptActivacionEracmf);
            TablaReporte tablaReporte8 = this.ObtenerDataTablaRptResarcimiento(listaReporte8);
            string reporte8 = this.GenerarHtmlValidacion(listaMsjValidacion) + this.GenerarRptHtml(tablaReporte8, 8, 1);

            listaReporteHtml = new List<string>
            {
                reporte1,
                reporte2,
                reporte3,
                reporte4,
                reporte5,
                reporte6,
                reporte7,
                reporte8
            };
        }

        /// <summary>
        /// Permite obtener lista cruce hora coordinación con empresas interrupción suministro
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="fdatcodi"></param>
        /// <param name="listaData"></param>
        /// <returns></returns>
        public List<AfHoraCoordDTO> ObtenerListaCruceHoracoordInterrupcion(int afecodi, int fdatcodi, List<AfInterrupSuministroDTO> listaData, string anio, string correlativo)
        {
            List<AfHoraCoordDTO> lstHoraCoordinacion = new List<AfHoraCoordDTO>();

            if (!string.IsNullOrWhiteSpace(anio) && !string.IsNullOrWhiteSpace(correlativo))
                lstHoraCoordinacion = ListAfHoraCoordsCtaf(anio, correlativo, fdatcodi);
            else
                lstHoraCoordinacion = ListAfHoraCoords(afecodi, fdatcodi);


            List<AfHoraCoordDTO> lstHandsonHorasCoordinacion = new List<AfHoraCoordDTO>();

            foreach (var emprsaIntSum in listaData)
            {
                var horasCoordinacion = lstHoraCoordinacion.Find(x => x.Emprcodi == emprsaIntSum.Emprcodi && x.Intsumcodi == emprsaIntSum.Intsumcodi);
                var Afhofechadescripcion = "";
                var Afhocodi = 0;
                DateTime? Afhofecmodificacion = null;
                string Afhousumodificacion = "";
                DateTime? Afhofeccreacion = null;
                string Afhousucreacion = "";
                DateTime? Afhofecha = null;

                if (horasCoordinacion != null)
                {

                    Afhofechadescripcion = horasCoordinacion.Afhofecha?.ToString(ConstantesBase.FormatoFechaFullBase);
                    Afhocodi = horasCoordinacion.Afhocodi;
                    Afhofecmodificacion = horasCoordinacion.Afhofecmodificacion;
                    Afhousumodificacion = horasCoordinacion.Afhousumodificacion;
                    Afhofeccreacion = horasCoordinacion.Afhofeccreacion;
                    Afhousucreacion = horasCoordinacion.Afhousucreacion;
                    Afhofecha = horasCoordinacion.Afhofecha;

                }

                lstHandsonHorasCoordinacion.Add(new AfHoraCoordDTO()
                {
                    Afhocodi = Afhocodi,
                    Afecodi = emprsaIntSum.Afecodi,
                    Fdatcodi = fdatcodi,
                    Emprcodi = emprsaIntSum.Emprcodi,
                    Codigoosinergmin = emprsaIntSum.CodigoOsinergmin,
                    Emprnombr = emprsaIntSum.Emprnomb,
                    Intsumsubestacion = emprsaIntSum.Intsumsubestacion,
                    Afhofechadescripcion = Afhofechadescripcion,
                    Intsumcodi = emprsaIntSum.Intsumcodi,
                    Afhofecmodificacion = Afhofecmodificacion,
                    Afhousumodificacion = Afhousumodificacion,
                    Afhofeccreacion = Afhofeccreacion,
                    Afhousucreacion = Afhousucreacion,
                    Afhofecha = Afhofecha
                });

            }

            return lstHandsonHorasCoordinacion.OrderBy(x => x.Codigoosinergmin).ToList();
        }

        /// <summary>
        /// Permite obtener lista cruce hora coordinación con empresas interrupción suministro
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="fdatcodi"></param>
        /// <param name="listaData"></param>
        /// <returns></returns>
        private List<AfHoraCoordDTO> ObtenerListaCruceHoracoordInterrsuministro(int afecodi, int fdatcodi, List<AfInterrupSuministroDTO> listaData, string anio, string correlativo)
        {
            List<AfHoraCoordDTO> lstHoraCoordinacion = new List<AfHoraCoordDTO>();

            if (!string.IsNullOrWhiteSpace(anio) && !string.IsNullOrWhiteSpace(correlativo))
                lstHoraCoordinacion = ListAfHoraCoordsCtaf(anio, correlativo, fdatcodi);
            else
                lstHoraCoordinacion = ListAfHoraCoords(afecodi, fdatcodi);


            List<AfHoraCoordDTO> lstHandsonHorasCoordinacion = new List<AfHoraCoordDTO>();

            foreach (var emprsaIntSum in listaData.GroupBy(x => new { x.Emprcodi, x.CodigoOsinergmin, x.Afecodi, x.Emprnomb, x.EmpresaSuministradora, x.Afemprnomb }))
            {
                var horasCoordinacion = lstHoraCoordinacion.Find(x => x.Emprcodi == emprsaIntSum.Key.Emprcodi && x.Afecodi == emprsaIntSum.Key.Afecodi);
                if (horasCoordinacion != null)
                {
                    horasCoordinacion.Codigoosinergmin = emprsaIntSum.Key.CodigoOsinergmin;
                    horasCoordinacion.Afhofechadescripcion = horasCoordinacion.Afhofecha?.ToString(ConstantesBase.FormatoFechaFullBase);
                    horasCoordinacion.EmpresaSuministradora = emprsaIntSum.Key.EmpresaSuministradora;
                    horasCoordinacion.AfEmprenomb = emprsaIntSum.Key.Afemprnomb;
                    lstHandsonHorasCoordinacion.Add(horasCoordinacion);
                }
                else
                {
                    lstHandsonHorasCoordinacion.Add(new AfHoraCoordDTO()
                    {
                        Afecodi = emprsaIntSum.Key.Afecodi,
                        Fdatcodi = fdatcodi,
                        Emprcodi = emprsaIntSum.Key.Emprcodi,
                        Codigoosinergmin = emprsaIntSum.Key.CodigoOsinergmin,
                        EmpresaSuministradora = emprsaIntSum.Key.EmpresaSuministradora,
                        AfEmprenomb = emprsaIntSum.Key.Afemprnomb
                    });

                }
            }


            return lstHandsonHorasCoordinacion.OrderBy(x => x.Codigoosinergmin).ToList();
        }

        /// <summary>
        /// Permite obtener lista de agentes con demoras
        /// </summary>
        /// <param name="listaHoraCoordinacion"></param>
        /// <param name="listaData"></param>
        /// <returns></returns>
        private List<AfHoraCoordDTO> ObtenerListaAgenteConDemoras(List<AfHoraCoordDTO> listaHoraCoordinacion, List<AfInterrupSuministroDTO> listaData)
        {
            List<AfHoraCoordDTO> lstHandsonAgentesDemoras = new List<AfHoraCoordDTO>();
            List<AfHoraCoordDTO> lstTotalAgentesDemoras = new List<AfHoraCoordDTO>();
            var lstEmpresasConfiguradas = ListAfEmpresas();

            foreach (var reg in listaData)
            {
                //Obtenemos Tabla Normalizacion Coordinacion
                if (listaHoraCoordinacion != null) // solo reporte TotalDatos
                {
                    if (listaHoraCoordinacion.Any())
                    {
                        var idEmpresaInterrupcion = reg.Emprcodi;

                        var regHoraCord = listaHoraCoordinacion.Find(x => x.Emprcodi == idEmpresaInterrupcion);

                        if (regHoraCord.Afhofecha != null)
                        {
                            var horaCord = regHoraCord.Afhofecha.Value;
                            var horaAdicionada = horaCord.AddMinutes(15);

                            int val = reg.Intsumfechainterrfin.Value.CompareTo(horaAdicionada);

                            if (val > 0)
                            {
                                lstTotalAgentesDemoras.Add(regHoraCord);
                            }
                        }
                    }
                }
            }

            foreach (var emprsaIntSum in lstTotalAgentesDemoras.GroupBy(x => x.Emprcodi))
            {
                var agenteDemoras = lstTotalAgentesDemoras.Find(x => x.Emprcodi == emprsaIntSum.Key);

                agenteDemoras.AfEmprenomb = lstEmpresasConfiguradas.Find(x => x.Emprcodi == agenteDemoras.Emprcodi).Afemprnomb;
                agenteDemoras.Afhmotivo = agenteDemoras.Afhmotivo != null ? agenteDemoras.Afhmotivo : string.Empty;

                lstHandsonAgentesDemoras.Add(agenteDemoras);
            }

            return lstHandsonAgentesDemoras.OrderBy(x => x.AfEmprenomb).ToList();
        }

        #endregion

        #region REPORTES DE INTERRUPCION

        /// <summary>
        /// Generar Excel Reporte
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="anio"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public string GenerarReporte2ByInterrupcionExcel(int tipoReporte, int afecodi, int emprcodi, string anio = "", string correlativo = "")

        {
            bool esPorEracmf = false;

            //Data del reporte y datos del evento
            int fdatcodi = (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion;
            SiEmpresaDTO objEmpresa = emprcodi > 0 ? FactorySic.GetSiEmpresaRepository().GetById(emprcodi) : null;
            SiFuentedatosDTO objFuenteDatos = FactorySic.GetSiFuentedatosRepository().GetById(fdatcodi);
            EventoDTO regEvento = this.ObtenerInterrupcionByAfecodi(afecodi);

            //Data del reporte
            this.ListarInterrupcionSuministrosGral(afecodi, emprcodi, fdatcodi, out List<AfInterrupSuministroDTO> listaData, out List<AfInterrupSuministroDTO> listDataReportCero, out List<string> listaMsjValidacion, out List<int> listaEmprcodiReportaron, anio, correlativo);

            //Lista horas de coordinacion de normalización
            List<AfHoraCoordDTO> lstHandsonHorasCoordinacion = this.ObtenerListaCruceHoracoordInterrupcion(afecodi, fdatcodi, listaData, anio, correlativo);

            //TOTAL DE DATOS
            this.ListarRptTotalDatos(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte1, out bool formatFechaCab1);
            TablaReporte tablaReporte1 = this.ObtenerDataExcelDatosTotales(listaReporte1, esPorEracmf, formatFechaCab1);

            //MENORES A 3 MINUTOS
            this.ListarRptMenores3Minutos(afecodi, emprcodi, listaData, null, out List<ReporteInterrupcion> listaReporte2, out bool formatFechaCab4);
            TablaReporte tablaReporte2 = this.ObtenerDataExcelMenores3minutos(listaReporte2, esPorEracmf, formatFechaCab4);

            //PARA LA DECISION            
            this.ListarRptDecision(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte3, out bool formatFechaCab3, esPorEracmf);
            TablaReporte tablaReporte3 = this.ObtenerDataExcelDecision(listaReporte3, formatFechaCab3);

            //PARA EL RESTABLECIMIENTO            
            this.ListarRptRestablecimiento(listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte4, out bool formatFechaCab);
            TablaReporte tablaReporte4 = this.ObtenerDataTablaRptRestablecimiento(listaReporte4, formatFechaCab);

            //PARA EL RESARCIMIENTO
            this.ListarRptResarcimientoNoERACMF(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte5, out bool formatFechaCab5);
            TablaReporte tablaReporte8 = this.ObtenerDataExcelResarcimiento(listaReporte5, formatFechaCab5);



            ////////////////////////////////////////////////////////////////////////////////////////////////////
            //Plantilla y archivo salida
            string rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesExtranetCTAF.RutaReportes;
            string nombPlantilla = ConstantesExtranetCTAF.PlantillaReporteInterrupciones;
            string nombCompletPlantilla = $"{rutaBase}{nombPlantilla}";
            FileInfo archivoPlantilla = new FileInfo(nombCompletPlantilla);

            string rutaOutput = AppDomain.CurrentDomain.BaseDirectory + ConstantesExtranetCTAF.RutaReportes;
            string nombFormato = $"{regEvento.CODIGO}_ReporteInterrupciones.xlsx";
            string nombCompletFormato = $"{rutaOutput}{nombFormato}";
            FileInfo nuevoArchivo = new FileInfo(nombCompletFormato);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombCompletFormato);
            }

            //Guardar excel
            using (var xlPackage = new ExcelPackage(archivoPlantilla))
            {
                ExcelWorksheet ws = null;
                int ultimaFila = 0;
                int coluIniCab = 3;

                //TOTAL DE DATOS
                this.AgregarHojaExcelGenerico(1, xlPackage, ref ws, tablaReporte1, regEvento, objEmpresa, objFuenteDatos, "Total de Datos", out ultimaFila);
                this.GenerarDetalleHoraNormalizacionNoERACMF(ref ws, lstHandsonHorasCoordinacion, ultimaFila, coluIniCab - 1);

                //PARA EL RESTABLECIMIENTO
                this.AgregarHojaExcelGenerico(2, xlPackage, ref ws, tablaReporte4, regEvento, objEmpresa, objFuenteDatos, "Demoras en el Restablecimiento", out ultimaFila, 13);

                //MENORES A 3 MINUTOS
                this.AgregarHojaExcelGenerico(3, xlPackage, ref ws, tablaReporte2, regEvento, objEmpresa, objFuenteDatos, "Interrupciones menores a 3 minutos", out ultimaFila);

                //PARA LA DECISION
                this.AgregarHojaExcelGenerico(4, xlPackage, ref ws, tablaReporte3, regEvento, objEmpresa, objFuenteDatos, "Interrupciones de suministros para la Decisión", out ultimaFila);

                //PARA EL RESARCIMIENTO
                this.AgregarHojaExcelGenerico(5, xlPackage, ref ws, tablaReporte8, regEvento, objEmpresa, objFuenteDatos, "Interrupciones para el Resarcimiento", out ultimaFila, 9);

                if (ws != null)
                {
                    xlPackage.Workbook.View.ActiveTab = 0;
                    xlPackage.SaveAs(nuevoArchivo);
                }
            }

            return nombFormato;
        }

        /// <summary>
        /// Generar Reporte 2 By Interrupcion
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listaReporteHtml"></param>
        /// <param name="lstHandsonHorasCoordinacion"></param>
        /// <param name="anio"></param>
        /// <param name="correlativo"></param>
        public void GenerarReporte2ByInterrupcion(int afecodi, int emprcodi, out List<string> listaReporteHtml /*otros salidas para el handson*/, out List<AfHoraCoordDTO> lstHandsonHorasCoordinacion, string anio, string correlativo)
        {
            bool esPorEracmf = false;

            this.ListarInterrupcionSuministrosGral(afecodi, emprcodi, (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion, out List<AfInterrupSuministroDTO> listaData, out List<AfInterrupSuministroDTO> listDataReportCero, out List<string> listaMsjValidacion, out List<int> listaEmprcodiReportaron, anio, correlativo);

            //Lista horas de coordinacion de normalización
            lstHandsonHorasCoordinacion = ObtenerListaCruceHoracoordInterrupcion(afecodi, (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion, listaData, anio, correlativo);

            //TOTAL DE DATOS
            this.ListarRptTotalDatos(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte1, out bool formatFechaCab1);
            TablaReporte tablaReporte1 = this.ObtenerDataTablaRptTotalDatos(listaReporte1, lstHandsonHorasCoordinacion, esPorEracmf);
            string reporte1 = this.GenerarHtmlValidacion(listaMsjValidacion) + this.GenerarRptHtml(tablaReporte1, 7, 2);

            //MENORES A 3 MINUTOS
            this.ListarRptMenores3Minutos(afecodi, emprcodi, listaData, null, out List<ReporteInterrupcion> listaReporte2, out bool formatFechaCab4);
            TablaReporte tablaReporte2 = this.ObtenerDataTablaRptMenores3Minutos(listaReporte2, esPorEracmf);
            string reporte2 = this.GenerarHtmlValidacion(listaMsjValidacion) + this.GenerarRptHtml(tablaReporte2, 8, 2);

            //PARA LA DECISION            
            this.ListarRptDecision(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte3, out bool formatFechaCab, esPorEracmf);
            TablaReporte tablaReporte3 = this.ObtenerDataTablaRptDecision(listaReporte3);
            string reporte3 = this.GenerarHtmlValidacion(listaMsjValidacion) + this.GenerarRptHtml(tablaReporte3, 9, 2);

            //Demoras Restablecimiento
            this.ListarRptRestablecimiento(listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte4, out bool formatFechaCab2);
            TablaReporte tablaReporte4 = this.ObtenerDataTablaRptRestablecimiento(listaReporte4, formatFechaCab2);
            string reporte4 = this.GenerarHtmlValidacion(listaMsjValidacion) + this.GenerarRptHtmlRestablecimiento(tablaReporte4, 6);

            //PARA EL RESARCIMIENTO
            this.ListarRptResarcimientoNoERACMF(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte5, out bool formatFechaCab5);
            TablaReporte tablaReporte8 = this.ObtenerDataTablaRptResarcimiento(listaReporte5);
            string reporte5 = this.GenerarHtmlValidacion(listaMsjValidacion) + this.GenerarRptHtml(tablaReporte8, 8, 2);

            listaReporteHtml = new List<string>
            {
                reporte1,
                reporte2,
                reporte3,
                reporte4,
                reporte5
            };
        }

        #endregion

        #region REPORTES DE REDUCCION DE SUMINISTROS

        /// <summary>
        /// Generar Excel Reporte de Reducción de suministros
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="anio"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public string GenerarReporte3ByReduccionExcel(int tipoReporte, int afecodi, int emprcodi, string anio, string correlativo)
        {
            //Data del reporte y datos del evento
            int fdatcodi = (int)ConstantesExtranetCTAF.Fuentedato.ReduccionSuministros;
            SiEmpresaDTO objEmpresa = emprcodi > 0 ? FactorySic.GetSiEmpresaRepository().GetById(emprcodi) : null;
            SiFuentedatosDTO objFuenteDatos = FactorySic.GetSiFuentedatosRepository().GetById(fdatcodi);
            EventoDTO regEvento = this.ObtenerInterrupcionByAfecodi(afecodi);

            this.ListarInterrupcionSuministrosGral(afecodi, emprcodi, fdatcodi, out List<AfInterrupSuministroDTO> listaData, out List<AfInterrupSuministroDTO> listDataReportCero, out List<string> listaMsjValidacion, out List<int> listaEmprcodiReportaron, anio, correlativo);

            this.ListarRpt7ReduccionDeSuministro(afecodi, emprcodi, listaData, out List<ReporteInterrupcion> listaReporte, out bool formatFechaCab1);

            TablaReporte tablaReporte7 = this.ObtenerDataExcelReduccionSuministros(listaReporte, formatFechaCab1);

            //Plantilla y archivo salida
            string rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesExtranetCTAF.RutaReportes;
            string nombPlantilla = ConstantesExtranetCTAF.PlantillaReporteReduccionDeSuministros;
            string nombCompletPlantilla = $"{rutaBase}{nombPlantilla}";
            FileInfo archivoPlantilla = new FileInfo(nombCompletPlantilla);

            string rutaOutput = AppDomain.CurrentDomain.BaseDirectory + ConstantesExtranetCTAF.RutaReportes;
            string nombFormato = $"{regEvento.CODIGO}_ReporteReducciónDeSuministros.xlsx";
            string nombCompletFormato = $"{rutaOutput}{nombFormato}";
            FileInfo nuevoArchivo = new FileInfo(nombCompletFormato);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombCompletFormato);
            }

            //Guardar excel
            using (var xlPackage = new ExcelPackage(archivoPlantilla))
            {
                ExcelWorksheet ws = null;
                int ultimaFila = 0;

                this.AgregarHojaExcelGenerico(1, xlPackage, ref ws, tablaReporte7, regEvento, objEmpresa, objFuenteDatos, "", out ultimaFila);

                if (ws != null)
                {
                    xlPackage.Workbook.View.ActiveTab = 0;
                    xlPackage.SaveAs(nuevoArchivo);
                }
            }

            return nombFormato;
        }

        /// <summary>
        /// Generar Reporte 3 By Reduccion
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listaReporteHtml"></param>
        /// <param name="anio"></param>
        /// <param name="correlativo"></param>
        public void GenerarReporte3ByReduccion(int afecodi, int emprcodi, out List<string> listaReporteHtml, string anio, string correlativo)
        {
            this.ListarInterrupcionSuministrosGral(afecodi, emprcodi, (int)ConstantesExtranetCTAF.Fuentedato.ReduccionSuministros, out List<AfInterrupSuministroDTO> listaData, out List<AfInterrupSuministroDTO> listDataReportCero, out List<string> listaMsjValidacion, out List<int> listaEmprcodiReportaron, anio, correlativo);

            this.ListarRpt7ReduccionDeSuministro(afecodi, emprcodi, listaData, out List<ReporteInterrupcion> listaReporte, out bool formatFechaCab1);
            TablaReporte tablaReporte7 = this.ObtenerDataTablaRpt7ReduccionDeSuministro(listaReporte);

            string reporte7 = this.GenerarHtmlValidacion(listaMsjValidacion) + this.GenerarRptHtml(tablaReporte7, 0, 3);

            listaReporteHtml = new List<string>
            {
                reporte7
            };
        }

        #endregion

        #region Obtencion Listas para cada Reporte

        /// <summary>
        /// Obtiene las listas que se usará en el reporte: Total de Datos
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listaAllData"></param>
        /// <param name="tabHoraCoord"></param>
        /// <param name="listaReporte"></param>
        /// <param name="formatFechaCab"></param>
        public void ListarRptTotalDatos(int afecodi, int emprcodi, List<AfInterrupSuministroDTO> listaAllData, List<AfHoraCoordDTO> tabHoraCoord, out List<ReporteInterrupcion> listaReporte, out bool formatFechaCab)
        {
            List<AfInterrupSuministroDTO> listaData = new List<AfInterrupSuministroDTO>();
            List<AfHoraCoordDTO> registrosHorasCoordinacion = new List<AfHoraCoordDTO>();
            listaData = listaAllData;

            listaReporte = lstDatosTotales_MalasActuaciones_Menores3minutos(listaData);
            listaReporte = listaReporte.OrderBy(x => x.Zona).ThenBy(x => x.Suministro).ThenBy(x => x.Subestacion).ThenBy(x => x.FechaInicio).ThenBy(x => x.FechaFin).ToList();

            formatFechaCab = false;
            foreach (var reg in listaReporte)
            {
                if (tabHoraCoord != null)
                {
                    foreach (var hrcoord in tabHoraCoord)
                    {
                        if (hrcoord.Intsumcodi > 0)
                        {
                            registrosHorasCoordinacion = tabHoraCoord.Where(x => x.Afecodi == reg.Afecodi && x.Intsumcodi == reg.Intsumcodi).ToList();
                            if (registrosHorasCoordinacion.Count > 0)
                            {
                                reg.TieneDemoraCeldaPintada = PresentaDemoraSegunTablaNormalizacion(registrosHorasCoordinacion, reg.IdEmpresa, reg.FechaFin, out DateTime hc);
                            }
                        }
                        else
                            reg.TieneDemoraCeldaPintada = PresentaDemoraSegunTablaNormalizacion(tabHoraCoord, reg.IdEmpresa, reg.FechaFin, out DateTime hc);
                    }
                }

                var resultado = TieneFormatoFechaEspecialWord(reg.FechaInicio, reg.FechaFin);
                formatFechaCab = resultado == true ? resultado : formatFechaCab;
            }
        }

        /// <summary>
        /// Obtiene las listas que se usará en el reporte: Rersumen
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listaData"></param>
        /// <param name="listaReporte"></param>
        private void ListarRptResumen(int afecodi, int emprcodi, List<AfInterrupSuministroDTO> listaAllData, out List<ReporteInterrupcion> listaReporte)
        {


            listaReporte = new List<ReporteInterrupcion>();


            var afecodis = listaAllData.Select(x => x.Afecodi).Distinct();

            foreach (var item in afecodis)
            {
                afecodi = item;
                List<AfCondicionesDTO> listaFuncionesYEtapas = ListAfCondicioness(afecodi);
                var registros = listaAllData.Where(x => x.Afecodi == afecodi).OrderByDescending(c => c.EVENCODI).ToList();
                if (listaFuncionesYEtapas.Any())
                {
                    //Listado de Malas actuaciones
                    bool contieneSoloMalasActuaciones1 = true;
                    List<AfInterrupSuministroDTO> listaMalasActuaciones = ObtenerListadoConSinMalasActuaciones(afecodi, registros, contieneSoloMalasActuaciones1);

                    //Listado de Buenas actuaciones
                    bool contieneSoloMalasActuaciones2 = false;
                    List<AfInterrupSuministroDTO> listaBuenasActuaciones = ObtenerListadoConSinMalasActuaciones(afecodi, registros, contieneSoloMalasActuaciones2);

                    ReporteInterrupcion regTotal = ObtenerReporteInterrupcionRptResumen(registros, ref listaReporte, listaMalasActuaciones, listaBuenasActuaciones);

                    listaReporte.Add(regTotal);
                }
                else  //TODAS Serán considerados Buenas Actuaciones
                {


                    //Listado de Malas actuaciones, se envia vacía
                    List<AfInterrupSuministroDTO> listaMalasActuaciones = new List<AfInterrupSuministroDTO>();

                    //Todas serán Buenas actuaciones
                    List<AfInterrupSuministroDTO> listaBuenasActuaciones = registros;

                    ReporteInterrupcion regTotal = ObtenerReporteInterrupcionRptResumen(registros, ref listaReporte, listaMalasActuaciones, listaBuenasActuaciones);

                    listaReporte.Add(regTotal);
                }


            }
            listaReporte = listaReporte.OrderBy(x => x.Zona).ThenBy(x => x.NombEmpresa).ToList();



            //List<AfCondicionesDTO> listaFuncionesYEtapas = ListAfCondicioness(afecodi);





        }

        /// <summary>
        /// Devuelve los datos que seran usados para formar el Reporte Resumen
        /// </summary>
        /// <param name="listaAllData"></param>
        /// <param name="listaReporte"></param>
        /// <param name="listaMalasActuaciones"></param>
        /// <param name="listaBuenasActuaciones"></param>
        /// <returns></returns>
        private ReporteInterrupcion ObtenerReporteInterrupcionRptResumen(List<AfInterrupSuministroDTO> listaAllData, ref List<ReporteInterrupcion> listaReporte, List<AfInterrupSuministroDTO> listaMalasActuaciones, List<AfInterrupSuministroDTO> listaBuenasActuaciones)
        {
            var lstTempZonaEmpresa = listaAllData.GroupBy(x => new { x.Intsumzona, x.CodigoOsinergmin, x.Emprnomb, x.Emprcodi, x.EVENCODI, x.EVENINI }).ToList();
            string evencodi = "";
            //ListasSuma
            List<decimal?> lstf1 = new List<decimal?>();
            List<decimal?> lstf2 = new List<decimal?>();
            List<decimal?> lstf3 = new List<decimal?>();
            List<decimal?> lstf4 = new List<decimal?>();
            List<decimal?> lstf5 = new List<decimal?>();
            List<decimal?> lstf6 = new List<decimal?>();
            List<decimal?> lstf7 = new List<decimal?>();
            List<decimal?> lstDf1 = new List<decimal?>();
            List<decimal?> lstDf2 = new List<decimal?>();
            List<decimal?> lstDf3 = new List<decimal?>();
            List<decimal?> lstTotal = new List<decimal?>();

            foreach (var regZonaEmpr in lstTempZonaEmpresa)
            {
                var reg = new ReporteInterrupcion();

                var zona = regZonaEmpr.Key.Intsumzona;
                var empresa = regZonaEmpr.Key.CodigoOsinergmin;
                var codigoNombreEmpresa = regZonaEmpr.Key.Emprnomb;
                var idEmpresa = regZonaEmpr.Key.Emprcodi;
                evencodi = regZonaEmpr.Key.EVENCODI.ToString();
                var evvenini = regZonaEmpr.Key.EVENINI.ToString("dd/MM/yyyy HH:mm:ss");
                reg.Zona = zona;
                reg.NombEmpresa = empresa;
                reg.IdEmpresa = idEmpresa;
                reg.Evencodi = evencodi;
                reg.Evenini = evvenini;
                reg.Etapaf1 = ObtenerValorCeldaPotencia(zona, empresa, ConstantesExtranetCTAF.f, 1, listaMalasActuaciones, listaBuenasActuaciones, out bool esMalaActf1);
                reg.Etapaf2 = ObtenerValorCeldaPotencia(zona, empresa, ConstantesExtranetCTAF.f, 2, listaMalasActuaciones, listaBuenasActuaciones, out bool esMalaActf2);
                reg.Etapaf3 = ObtenerValorCeldaPotencia(zona, empresa, ConstantesExtranetCTAF.f, 3, listaMalasActuaciones, listaBuenasActuaciones, out bool esMalaActf3);
                reg.Etapaf4 = ObtenerValorCeldaPotencia(zona, empresa, ConstantesExtranetCTAF.f, 4, listaMalasActuaciones, listaBuenasActuaciones, out bool esMalaActf4);
                reg.Etapaf5 = ObtenerValorCeldaPotencia(zona, empresa, ConstantesExtranetCTAF.f, 5, listaMalasActuaciones, listaBuenasActuaciones, out bool esMalaActf5);
                reg.Etapaf6 = ObtenerValorCeldaPotencia(zona, empresa, ConstantesExtranetCTAF.f, 6, listaMalasActuaciones, listaBuenasActuaciones, out bool esMalaActf6);
                reg.Etapaf7 = ObtenerValorCeldaPotencia(zona, empresa, ConstantesExtranetCTAF.f, 7, listaMalasActuaciones, listaBuenasActuaciones, out bool esMalaActf7);
                reg.EtapaDf1 = ObtenerValorCeldaPotencia(zona, empresa, ConstantesExtranetCTAF.Df, 1, listaMalasActuaciones, listaBuenasActuaciones, out bool esMalaActDf1);
                reg.EtapaDf2 = ObtenerValorCeldaPotencia(zona, empresa, ConstantesExtranetCTAF.Df, 2, listaMalasActuaciones, listaBuenasActuaciones, out bool esMalaActDf2);
                reg.EtapaDf3 = ObtenerValorCeldaPotencia(zona, empresa, ConstantesExtranetCTAF.Df, 3, listaMalasActuaciones, listaBuenasActuaciones, out bool esMalaActDf3);

                reg.ColorEtapaf1 = esMalaActf1;
                reg.ColorEtapaf2 = esMalaActf2;
                reg.ColorEtapaf3 = esMalaActf3;
                reg.ColorEtapaf4 = esMalaActf4;
                reg.ColorEtapaf5 = esMalaActf5;
                reg.ColorEtapaf6 = esMalaActf6;
                reg.ColorEtapaf7 = esMalaActf7;
                reg.ColorEtapaDf1 = esMalaActDf1;
                reg.ColorEtapaDf2 = esMalaActDf2;
                reg.ColorEtapaDf3 = esMalaActDf3;

                var potf1 = reg.Etapaf1;
                var potf2 = reg.Etapaf2;
                var potf3 = reg.Etapaf3;
                var potf4 = reg.Etapaf4;
                var potf5 = reg.Etapaf5;
                var potf6 = reg.Etapaf6;
                var potf7 = reg.Etapaf7;
                var potDf1 = reg.EtapaDf1;
                var potDf2 = reg.EtapaDf2;
                var potDf3 = reg.EtapaDf3;

                var totalFila =
                    (potf1 != null ? potf1 : 0) +
                    (potf2 != null ? potf2 : 0) +
                    (potf3 != null ? potf3 : 0) +
                    (potf4 != null ? potf4 : 0) +
                    (potf5 != null ? potf5 : 0) +
                    (potf6 != null ? potf6 : 0) +
                    (potf7 != null ? potf7 : 0) +
                    (potDf1 != null ? potDf1 : 0) +
                    (potDf2 != null ? potDf2 : 0) +
                    (potDf3 != null ? potDf3 : 0);

                reg.TotalZona = totalFila;
                reg.CodigoNombreEmpresa = "- " + empresa + ": " + codigoNombreEmpresa;
                lstf1.Add(potf1);
                lstf2.Add(potf2);
                lstf3.Add(potf3);
                lstf4.Add(potf4);
                lstf5.Add(potf5);
                lstf6.Add(potf6);
                lstf7.Add(potf7);
                lstDf1.Add(potDf1);
                lstDf2.Add(potDf2);
                lstDf3.Add(potDf3);
                lstTotal.Add(totalFila);

                listaReporte.Add(reg);
            }

            listaReporte = listaReporte.OrderBy(x => x.Zona).ThenBy(x => x.NombEmpresa).ToList();

            //Totales
            ReporteInterrupcion regTotal = new ReporteInterrupcion();
            regTotal.TipoReporte = ConstantesExtranetCTAF.TipoReporteTotal;
            regTotal.Evencodi = evencodi;
            regTotal.SumaEtapaf1 = lstf1.Sum(x => x ?? 0);
            regTotal.SumaEtapaf2 = lstf2.Sum(x => x ?? 0);
            regTotal.SumaEtapaf3 = lstf3.Sum(x => x ?? 0);
            regTotal.SumaEtapaf4 = lstf4.Sum(x => x ?? 0);
            regTotal.SumaEtapaf5 = lstf5.Sum(x => x ?? 0);
            regTotal.SumaEtapaf6 = lstf6.Sum(x => x ?? 0);
            regTotal.SumaEtapaf7 = lstf7.Sum(x => x ?? 0);
            regTotal.SumaEtapaDf1 = lstDf1.Sum(x => x ?? 0);
            regTotal.SumaEtapaDf2 = lstDf2.Sum(x => x ?? 0);
            regTotal.SumaEtapaDf3 = lstDf3.Sum(x => x ?? 0);

            regTotal.SumaTotal = lstTotal.Sum(x => x ?? 0);


            return regTotal;
        }

        /// <summary>
        /// Devuelve la potencia total de la celda cuya zona, etapa, funcion y empresa son sus parametros
        /// </summary>
        /// <param name="zona"></param>
        /// <param name="empresa"></param>
        /// <param name="funcion"></param>
        /// <param name="etapa"></param>
        /// <param name="listaMalasActuaciones"></param>
        /// <param name="listaBuenasActuaciones"></param>
        /// <param name="esMalaActf1"></param>
        /// <returns></returns>
        private decimal? ObtenerValorCeldaPotencia(string zona, string empresa, string funcion, int etapa, List<AfInterrupSuministroDTO> listaMalasActuaciones, List<AfInterrupSuministroDTO> listaBuenasActuaciones, out bool esMalaActf1)
        {
            esMalaActf1 = false;
            decimal? potenciaTotal = 0;

            var potenciaMalaActuacion = listaMalasActuaciones.Where(x => (x.Intsumzona != null ? x.Intsumzona.ToUpper() : "") == zona.ToUpper() && (x.CodigoOsinergmin != null ? x.CodigoOsinergmin.ToUpper() : "") == empresa.ToUpper() && (x.Intsumfuncion != null ? x.Intsumfuncion.ToUpper() : "") == funcion.ToUpper() && x.Intsumnumetapa == etapa).Sum(n => n.Intsummw);
            var potenciaBuenaActuacion = listaBuenasActuaciones.Where(x => (x.Intsumzona != null ? x.Intsumzona.ToUpper() : "") == zona.ToUpper() && (x.CodigoOsinergmin != null ? x.CodigoOsinergmin.ToUpper() : "") == empresa.ToUpper() && (x.Intsumfuncion != null ? x.Intsumfuncion.ToUpper() : "") == funcion.ToUpper() && x.Intsumnumetapa == etapa).Sum(n => n.Intsummw);

            if (potenciaMalaActuacion > 0)
            {
                esMalaActf1 = true;

                if (potenciaBuenaActuacion > 0)
                {
                    potenciaTotal = potenciaMalaActuacion + potenciaBuenaActuacion;
                }
                else
                {
                    potenciaTotal = potenciaMalaActuacion;
                }
            }
            else
            {
                if (potenciaBuenaActuacion > 0)
                {
                    potenciaTotal = potenciaBuenaActuacion;
                }
                else
                {
                    //El registro no existe
                    potenciaTotal = null;
                }
            }

            return potenciaTotal;
        }

        /// <summary>
        /// Obtiene las listas que se usará en el reporte: Malas Actuaciones
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listaAllData"></param>
        /// <param name="listaReporte"></param>
        private void ListarRptMalasActuaciones(int afecodi, int emprcodi, List<AfInterrupSuministroDTO> listaAllData, out List<ReporteInterrupcion> listaReporte)
        {
            var afecodis = listaAllData.Select(x => x.Afecodi).Distinct();
            List<AfInterrupSuministroDTO> listaDataFiltro;
            listaReporte = new List<ReporteInterrupcion>();
            foreach (var item in afecodis)
            {
                listaDataFiltro = listaAllData.Where(x => x.Afecodi == item).ToList();
                //Considerar malas actuaciones
                bool contieneSoloMalasActuaciones = true;
                List<AfInterrupSuministroDTO> listaMalasActuaciones = ObtenerListadoConSinMalasActuaciones(item, listaDataFiltro, contieneSoloMalasActuaciones);

                List<AfInterrupSuministroDTO> listaData = listaMalasActuaciones;

                listaReporte.AddRange(lstDatosTotales_MalasActuaciones_Menores3minutos(listaData));
            }
        }

        /// <summary>
        /// Obtiene las listas que se usará en el reporte: Menores a 3 Minutos
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listaAllData"></param>
        /// <param name="tabHoraCoord"></param>
        /// <param name="listaReporte"></param>
        /// <param name="formatFechaCab"></param>
        private void ListarRptMenores3Minutos(int afecodi, int emprcodi, List<AfInterrupSuministroDTO> listaAllData, List<AfHoraCoordDTO> tabHoraCoord, out List<ReporteInterrupcion> listaReporte, out bool formatFechaCab)
        {
            //Lista de interrupciones menroes a 3 minutos
            List<AfInterrupSuministroDTO> listaMenores3minutos = listaAllData.Where(x => x.Intsumduracion < 3).ToList();

            List<AfInterrupSuministroDTO> listaData = listaMenores3minutos;

            listaReporte = lstDatosTotales_MalasActuaciones_Menores3minutos(listaData);

            //Formateo por fila
            formatFechaCab = false;
            foreach (var reg in listaReporte)
            {
                reg.TieneDemoraCeldaPintada = PresentaDemoraSegunTablaNormalizacion(tabHoraCoord, reg.IdEmpresa, reg.FechaFin, out DateTime hc);

                var resultado = TieneFormatoFechaEspecialWord(reg.FechaInicio, reg.FechaFin);
                formatFechaCab = resultado == true ? resultado : formatFechaCab;
            }
        }

        /// <summary>
        /// Obtiene las listas que se usará en el reporte: No reportaron interrupcion
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listaInterrupcionesReportados"></param>
        /// <param name="listaEmprcodiReportaron"></param>
        /// <param name="listaReporte"></param>
        /// <param name="listaMsjValidacionEracmf"></param>
        public void ListarRptNoReportaronInterrupcion(int afecodi, int emprcodi, List<AfInterrupSuministroDTO> listaInterrupcionesReportados, List<int> listaEmprcodiReportaron, out List<ReporteInterrupcion> listaReporte, out List<string> listaMsjValidacionEracmf)
        {
            listaMsjValidacionEracmf = new List<string>();
            List<ReporteInterrupcion> listaPreReporte = new List<ReporteInterrupcion>();
            listaReporte = new List<ReporteInterrupcion>();

            List<AfInterrupSuministroDTO> listaInterrupcionesReportadosPre = new List<AfInterrupSuministroDTO>();

            var afcodis = listaInterrupcionesReportados.Select(y => new { y.Afecodi, y.EVENCODI, y.EVENINI }).Distinct().ToList();
            List<AfInterrupSuministroDTO> listaBuenasActuaciones = new List<AfInterrupSuministroDTO>();
            //foreach (var item in afcodis)
            //{
            //    listaBuenasActuaciones.AddRange(ObtenerListadoConSinMalasActuaciones(item.Afecodi, listaAllData.Where(x => x.Afecodi == item.Afecodi).ToList(), contieneSoloMalasActuaciones));
            //}
            List<AfEracmfEventoDTO> lstEracmfInterrupcionesQueDebieronReportar = new List<AfEracmfEventoDTO>();
            List<AfEracmfEventoDTO> lstEracmfInterrupcionesQueNoReportaron = new List<AfEracmfEventoDTO>();
            List<AfInterrupSuministroDTO> lstSiReportaronCorrectamente = new List<AfInterrupSuministroDTO>();

            foreach (var item in afcodis)
            {
                listaPreReporte = new List<ReporteInterrupcion>();
                lstEracmfInterrupcionesQueNoReportaron = new List<AfEracmfEventoDTO>();
                afecodi = item.Afecodi;
                AfEventoDTO objAfEvento = GetByIdAfEvento(afecodi);
                listaInterrupcionesReportadosPre = listaInterrupcionesReportados.Where(x => x.Afecodi == afecodi).ToList();

                if (objAfEvento != null)
                {

                    List<AfEracmfEventoDTO> lstEracmfAgentesQueNoReportaron = new List<AfEracmfEventoDTO>();

                    List<AfEracmfEventoDTO> lstEracmfTotal = GetByEvencodiAfEracmfEventos(objAfEvento.Evencodi.Value);

                    lstEracmfTotal = lstEracmfTotal.Where(x => x.Eracmfarranqrumbral > 0 || x.Eracmfarranqrderivada > 0).ToList();

                    //Filtro las que debieron reportar, basado en las funciones y etapas guardadas
                    List<AfCondicionesDTO> listaFuncionesYEtapas = ListAfCondicioness(afecodi);
                    var lstZonaEtapa = listaFuncionesYEtapas.GroupBy(x => new { x.Afcondnumetapa, x.Afcondzona }).ToList();

                    foreach (var objFunEta in lstZonaEtapa)
                    {
                        //var funcion = objFunEta.Afcondfuncion;
                        var etapa = "Etapa " + objFunEta.Key.Afcondnumetapa;
                        var zona = "Zona " + objFunEta.Key.Afcondzona;


                        List<AfEracmfEventoDTO> eracmfCoincidentes = lstEracmfTotal.Where(x => ((x.Eracmfzona != null ? x.Eracmfzona.ToUpper() : "") == zona.ToUpper()) && ((x.Eracmfnumetapa != null ? x.Eracmfnumetapa.ToUpper() : "") == etapa.ToString().ToUpper())).ToList();

                        lstEracmfInterrupcionesQueDebieronReportar.AddRange(eracmfCoincidentes);
                        lstEracmfInterrupcionesQueNoReportaron.AddRange(eracmfCoincidentes);
                    }

                    if (lstEracmfInterrupcionesQueDebieronReportar.Any())
                    {
                        foreach (var interrReportado in listaInterrupcionesReportadosPre)
                        {
                            var zona = "Zona " + interrReportado.Intsumzona;
                            var empresa = interrReportado.Emprnomb;
                            var circuito = interrReportado.Intsumsuministro;
                            var subestacion = interrReportado.Intsumsubestacion;
                            var etapa = "Etapa " + interrReportado.Intsumnumetapa;

                            var interrEncontrado = lstEracmfInterrupcionesQueNoReportaron.Find(x => ((x.Eracmfzona != null ? x.Eracmfzona.ToUpper() : "") == zona.ToUpper()) &&
                                                                                      x.Eracmfemprnomb == empresa &&
                                                                                      x.Eracmfciralimentador == circuito &&
                                                                                      x.Eracmfsubestacion == subestacion &&
                                                                                    ((x.Eracmfnumetapa != null ? x.Eracmfnumetapa.ToUpper() : "") == etapa.ToString().ToUpper())
                                                                              );

                            if (interrEncontrado != null)
                            {
                                lstSiReportaronCorrectamente.Add(interrReportado);
                                lstEracmfInterrupcionesQueNoReportaron.Remove(interrEncontrado);
                            }
                        }
                    }

                    //TODAS LAS EMRPESAS CONFIGURADAS
                    var lstEmpresasConfiguradas = ListAfEmpresas();


                    List<int> lstEmprcodiAgentesReportaron = listaEmprcodiReportaron.Distinct().ToList();
                    List<string> lstEmprnombAgentesReportaron = lstEmpresasConfiguradas.Where(x => lstEmprcodiAgentesReportaron.Contains(x.Emprcodi)).Select(x => x.Afemprnomb).Distinct().ToList();

                    //Los Agentes que reportaron almenos una interrupcion, NO DEBEN aparecer en la lista de "NO REPORTADOS"
                    lstEracmfAgentesQueNoReportaron = lstEracmfInterrupcionesQueNoReportaron.Where((x) => !(lstEmprnombAgentesReportaron.Contains(x.Eracmfemprnomb))).ToList();


                    List<string> lstNombreEmpresasNoReportaron = new List<string>();

                    //Formateo por fila
                    foreach (var regData in lstEracmfAgentesQueNoReportaron)
                    {
                        //Obtengo codigoOsinergmin (si existe : codOsinergmin, No existe: nombreEmpresa)
                        var codOSI = "";
                        var emprConfigurada = lstEmpresasConfiguradas.Find(x => x.Afemprnomb.ToUpper() == regData.Eracmfemprnomb.ToUpper());

                        if (emprConfigurada != null)
                        {
                            codOSI = emprConfigurada.Afemprosinergmin != null ? emprConfigurada.Afemprosinergmin : regData.Eracmfemprnomb;


                            //Obtengo Etapa
                            var funcion = "";
                            var esF = regData.Eracmfarranqrumbral != 0 ? "f" : "";
                            var esDf = regData.Eracmfarranqrderivada != 0 ? "Df" : "";
                            funcion = esDf != "" ? (esF != "" ? (esF + ", " + esDf) : esDf) : esF;

                            //Armado del reporte
                            var reg = new ReporteInterrupcion();
                            reg.Zona = regData.Eracmfzona;
                            reg.NombEmpresa = regData.Eracmfemprnomb;
                            reg.Suministro = codOSI + "-" + regData.Eracmfsubestacion + "(" + regData.Eracmfciralimentador + ")";
                            reg.Subestacion = regData.Eracmfsubestacion;
                            reg.Funcion = funcion;
                            reg.Etapa = regData.Eracmfnumetapa;
                            reg.IdEmpresa = emprConfigurada.Emprcodi;
                            reg.Evencodi = item.EVENCODI.ToString();
                            reg.Evenini = item.EVENINI.ToString("dd/MM/yyyy HH:mm:ss");
                            reg.Afecodi = item.Afecodi;
                            listaPreReporte.Add(reg);
                        }
                        else
                        {
                            lstNombreEmpresasNoReportaron.Add(regData.Eracmfemprnomb);
                        }
                    }

                    lstNombreEmpresasNoReportaron = lstNombreEmpresasNoReportaron.Distinct().OrderBy(x => x).ToList();
                    foreach (var nombEmpr in lstNombreEmpresasNoReportaron)
                    {
                        listaMsjValidacionEracmf.Add("La empresa " + nombEmpr + " que no reporto información, NO tiene Configuración de equivalencia.");
                    }

                    listaPreReporte = (listaPreReporte.Where(x => x.IdEmpresa == emprcodi || emprcodi == -1).ToList());
                    listaReporte.AddRange(listaPreReporte.OrderBy(x => x.Zona).ThenBy(x => x.NombEmpresa).ThenBy(x => x.Suministro).ThenBy(x => x.Subestacion).ToList());
                }

                //  listaReporte.AddRange(listaPreReporte);
            }

        }

        /// <summary>
        /// Obtiene las listas que se usará en el reporte: para la decisión
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listaAllData"></param>
        /// <param name="lstHorasCoordinacion"></param>
        /// <param name="listaReporte"></param>
        /// <param name="formatFechaCab"></param>
        /// <param name="esPorEracmf"></param>
        private void ListarRptDecision(int afecodi, int emprcodi, List<AfInterrupSuministroDTO> listaAllData, List<AfHoraCoordDTO> lstHorasCoordinacion, out List<ReporteInterrupcion> listaReporte, out bool formatFechaCab, bool esPorEracmf)
        {
            List<AfInterrupSuministroDTO> listaData = new List<AfInterrupSuministroDTO>();
            List<AfInterrupSuministroDTO> listaDataDistintos = new List<AfInterrupSuministroDTO>();

            //Lista de interrupciones mayores iguales a 3 minutos
            List<AfInterrupSuministroDTO> listaMayores3minutos = listaAllData.Where(x => x.Intsumduracion >= 3).ToList();

            if (esPorEracmf)
            {
                //Lista de interrupciones con malas actuaciones
                bool contieneSoloMalasActuaciones = false; // Solo buenas actuaciones
                var afcodis = listaAllData.Select(y => new { y.Afecodi }).Distinct().ToList();
                List<AfInterrupSuministroDTO> listaBuenasActuaciones = new List<AfInterrupSuministroDTO>();
                foreach (var item in afcodis)
                {
                    listaBuenasActuaciones.AddRange(ObtenerListadoConSinMalasActuaciones(item.Afecodi, listaAllData.Where(x => x.Afecodi == item.Afecodi).ToList(), contieneSoloMalasActuaciones));
                }

                //Considerar buenas actuaciones Y mayores iguales a 3 minutos
                listaData = (List<AfInterrupSuministroDTO>)listaMayores3minutos.Intersect(listaBuenasActuaciones).ToList();
            }
            else
            {
                //Considerar mayores iguales a 3 minutos
                listaData = listaMayores3minutos;
            }

            listaDataDistintos = listaData.Distinct().ToList(); //Lista interrupciones sin menores 3 min y sin malas actuacioens (Si es por activacion ERACMF)  

            listaReporte = new List<ReporteInterrupcion>();
            formatFechaCab = false;
            foreach (var regData in listaDataDistintos)
            {
                var reg = new ReporteInterrupcion();
                var registrosHorasCoordinacion = lstHorasCoordinacion.Where(x => x.Afecodi == regData.Afecodi).ToList();

                reg.Suministro = regData.CodigoOsinergmin + "-" + regData.Intsumsubestacion + "(" + regData.Intsumsuministro + ")";
                reg.Potencia = regData.Intsummw.Value;
                reg.HoraInicio = regData.Intsumfechainterrini2;
                reg.HoraFinal = regData.Intsumfechainterrfin2;
                reg.Duracion = regData.Intsumduracion.Value;

                reg.FechaInicio = regData.Intsumfechainterrini.Value;
                reg.FechaFin = regData.Intsumfechainterrfin.Value;
                reg.IdEmpresa = regData.Emprcodi;
                reg.Evencodi = regData.EVENCODI.ToString();
                reg.Evenini = regData.EVENINI.ToString("dd/MM/yyyy HH:mm:ss");
                DateTime horaCordinacionAumentada;
                bool celdaConDemora = false;
                if (esPorEracmf)
                {
                    celdaConDemora = PresentaDemoraSegunTablaNormalizacion(registrosHorasCoordinacion, reg.IdEmpresa, regData.Intsumfechainterrfin.Value, out horaCordinacionAumentada);
                }
                else
                {
                    celdaConDemora = PresentaDemoraSegunTablaNormalizacionSubEstacion(registrosHorasCoordinacion, reg.IdEmpresa, regData.Intsumfechainterrfin.Value, out horaCordinacionAumentada, regData.Intsumcodi);
                }


                if (celdaConDemora)
                {
                    reg.FechaFin = horaCordinacionAumentada;
                    reg.HoraFinal = horaCordinacionAumentada.ToString(ConstantesAppServicio.FormatoFechaFull2);
                    reg.Duracion = (decimal)(horaCordinacionAumentada - reg.FechaInicio).TotalMinutes;

                    //redondeo solo en la extranet
                    reg.Duracion = MathHelper.Round(reg.Duracion, ConstantesExtranetCTAF.DigitosParteDecimalDuracion);
                }
                var resultado = TieneFormatoFechaEspecialWord(reg.FechaInicio, reg.FechaFin);
                formatFechaCab = resultado == true ? resultado : formatFechaCab;

                listaReporte.Add(reg);
            }

            listaReporte = listaReporte.OrderBy(x => x.Suministro).ThenBy(x => x.HoraInicio).ToList();
            //Formateo por fila
            int fila = 1;
            foreach (var reg in listaReporte)
            {
                reg.NumFila = fila;
                fila++;
            }
        }

        //nuevos reportes
        /// <summary>
        /// Obtiene las listas que se usará en el reporte: para la decisión
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listaAllData"></param>
        /// <param name="lstHorasCoordinacion"></param>
        /// <param name="listaReporte"></param>
        /// <param name="formatFechaCab"></param>
        private void ListarRptResarcimientoNoERACMF(int afecodi, int emprcodi, List<AfInterrupSuministroDTO> listaAllData, List<AfHoraCoordDTO> lstHorasCoordinacion, out List<ReporteInterrupcion> listaReporte, out bool formatFechaCab)
        {
            List<AfInterrupSuministroDTO> listaData = new List<AfInterrupSuministroDTO>();
            List<AfInterrupSuministroDTO> listaDataDistintos = new List<AfInterrupSuministroDTO>();

            //Lista de interrupciones mayores iguales a 3 minutos
            List<AfInterrupSuministroDTO> listaMayores3minutos = listaAllData.Where(x => x.Intsumduracion >= 3).ToList();


            listaData = listaMayores3minutos;


            listaDataDistintos = listaData.Distinct().ToList(); //Lista interrupciones sin menores 3 min y sin malas actuacioens (Si es por activacion ERACMF)  

            listaReporte = new List<ReporteInterrupcion>();
            formatFechaCab = false;


            var codigosEventos = listaDataDistintos.Select(y => new { y.EVENCODI }).Distinct().ToList();
            var listaDataCompleta = listaDataDistintos;

            listaReporte = new List<ReporteInterrupcion>();
            List<ReporteInterrupcion> listaReporteTemporal;
            formatFechaCab = false;

            List<AfInterrupSuministroDTO> registros = new List<AfInterrupSuministroDTO>();

            foreach (var item in codigosEventos)
            {
                listaReporteTemporal = new List<ReporteInterrupcion>();
                registros = listaDataCompleta.Where(x => x.EVENCODI == item.EVENCODI).OrderByDescending(c => c.EVENCODI).ToList();
                var registrosHorasCoordinacion = lstHorasCoordinacion.Where(x => x.Afecodi == registros[0].Afecodi).ToList();
                foreach (var regData in registros)
                {
                    var reg = new ReporteInterrupcion();

                    reg.Suministro = regData.CodigoOsinergmin + "-" + regData.Intsumsubestacion + "(" + regData.Intsumsuministro + ")";
                    reg.Potencia = regData.Intsummw.Value;
                    reg.HoraInicio = regData.Intsumfechainterrini2;
                    reg.HoraFinal = regData.Intsumfechainterrfin2;
                    reg.Duracion = regData.Intsumduracion.Value;

                    reg.FechaInicio = regData.Intsumfechainterrini.Value;
                    reg.FechaFin = regData.Intsumfechainterrfin.Value;
                    reg.IdEmpresa = regData.Emprcodi;
                    reg.Evencodi = regData.EVENCODI.ToString();
                    reg.Evenini = regData.EVENINI.ToString("dd/MM/yyyy HH:mm:ss");
                    bool celdaConDemora = PresentaDemoraSegunTablaNormalizacionSubEstacion(registrosHorasCoordinacion, reg.IdEmpresa, regData.Intsumfechainterrfin.Value, out DateTime horaCordinacionAumentada, regData.Intsumcodi);

                    if (celdaConDemora)
                    {
                        reg.FechaFin = horaCordinacionAumentada;
                        reg.HoraFinal = horaCordinacionAumentada.ToString(ConstantesAppServicio.FormatoFechaFull2);
                        reg.Duracion = (decimal)(horaCordinacionAumentada - reg.FechaInicio).TotalMinutes;

                        //redondeo solo en la extranet
                        reg.Duracion = MathHelper.Round(reg.Duracion, ConstantesExtranetCTAF.DigitosParteDecimalDuracion);
                    }
                    reg.DuracionHoras = reg.Duracion / 60;
                    reg.EnergiaNoSuministrada = reg.Potencia * reg.DuracionHoras;
                    reg.EnergiaNoSuministrada = reg.EnergiaNoSuministrada;
                    var resultado = TieneFormatoFechaEspecialWord(reg.FechaInicio, reg.FechaFin);
                    formatFechaCab = resultado == true ? resultado : formatFechaCab;

                    listaReporte.Add(reg);
                }

                listaReporte = listaReporte.OrderBy(x => x.Suministro).ThenBy(x => x.HoraInicio).ToList();
                //Formateo por fila
                int fila = 1;
                foreach (var reg in listaReporte)
                {
                    reg.NumFila = fila;
                    fila++;
                }
                //Totales
                ReporteInterrupcion regTotal = new ReporteInterrupcion();
                regTotal.TipoReporte = ConstantesExtranetCTAF.TipoReporteTotal;
                regTotal.Potencia = listaReporte.Sum(x => x.Potencia);
                regTotal.EnergiaNoSuministrada = listaReporte.Sum(x => x.EnergiaNoSuministrada);
                regTotal.Evencodi = item.EVENCODI.ToString();

                listaReporte.Add(regTotal);

            }
        }


        /// <summary>
        /// Obtiene las listas que se usará en el reporte: reportaron 0
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listaAllData"></param>
        /// <param name="listaReporte"></param>
        private void ListarRptReportaron0(int afecodi, int emprcodi, List<AfInterrupSuministroDTO> listaAllData, out List<ReporteInterrupcion> listaReporte)
        {
            List<AfInterrupSuministroDTO> listaData = new List<AfInterrupSuministroDTO>();
            listaReporte = new List<ReporteInterrupcion>();

            List<AfInterrupSuministroDTO> listaDataDistintos = new List<AfInterrupSuministroDTO>();

            listaDataDistintos = listaAllData.Distinct().ToList(); //Lista interrupciones sin menores 3 min y sin malas actuacioens (Si es por activacion ERACMF)  
            var afcodis = listaDataDistintos.Select(y => new { y.Afecodi }).Distinct().ToList();
            List<AfInterrupSuministroDTO> listaBuenasActuaciones = new List<AfInterrupSuministroDTO>();

            foreach (var item in afcodis)
            {

                listaData = listaDataDistintos.Where(x => x.Afecodi == item.Afecodi).ToList();
                foreach (var regData in listaData)
                {
                    var reg = new ReporteInterrupcion();
                    reg.IdEmpresa = regData.Emprcodi;
                    reg.NombEmpresa = regData.Emprnomb.ToUpper();
                    reg.Observaciones = regData.Intsumobs.ToUpper();
                    reg.Evencodi = regData.EVENCODI.ToString();
                    reg.Evenini = regData.EVENINI.ToString("dd/MM/yyyy HH:mm:ss");
                    listaReporte.Add(reg);
                }
            }


            listaReporte = listaReporte.OrderBy(x => x.Suministro).ThenBy(x => x.HoraInicio).ToList();
        }

        /// <summary>
        /// Obtiene las listas que se usará en el reporte: para la decisión
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listaAllData"></param>
        /// <param name="lstHorasCoordinacion"></param>
        /// <param name="listaReporte"></param>
        /// <param name="formatFechaCab"></param>
        /// <param name="esPorEracmf"></param>
        private void ListarRptResarcimiento(int afecodi, int emprcodi, List<AfInterrupSuministroDTO> listaAllData, List<AfHoraCoordDTO> lstHorasCoordinacion, out List<ReporteInterrupcion> listaReporte, out bool formatFechaCab, bool esPorEracmf)
        {
            List<AfInterrupSuministroDTO> listaData = new List<AfInterrupSuministroDTO>();
            List<AfInterrupSuministroDTO> listaDataDistintos = new List<AfInterrupSuministroDTO>();

            //Lista de interrupciones mayores iguales a 3 minutos
            List<AfInterrupSuministroDTO> listaMayores3minutos = listaAllData.Where(x => x.Intsumduracion >= 3).ToList();




            if (esPorEracmf)
            {
                //Lista de interrupciones con malas actuaciones
                bool contieneSoloMalasActuaciones = false; // Solo buenas actuaciones
                var afcodis = listaAllData.Select(y => new { y.Afecodi }).Distinct().ToList();
                List<AfInterrupSuministroDTO> listaBuenasActuaciones = new List<AfInterrupSuministroDTO>();
                foreach (var item in afcodis)
                {
                    listaBuenasActuaciones.AddRange(ObtenerListadoConSinMalasActuaciones(item.Afecodi, listaAllData.Where(x => x.Afecodi == item.Afecodi).ToList(), contieneSoloMalasActuaciones));
                }
                listaData = (List<AfInterrupSuministroDTO>)listaMayores3minutos.Intersect(listaBuenasActuaciones).ToList();
            }
            else
            {
                //Considerar mayores iguales a 3 minutos
                listaData = listaMayores3minutos;
            }





            listaDataDistintos = listaData.Distinct().ToList(); //Lista interrupciones sin menores 3 min y sin malas actuacioens (Si es por activacion ERACMF)  

            var codigosEventos = listaData.Select(y => new { y.EVENCODI }).Distinct().ToList();
            var listaDataCompleta = listaData;

            listaReporte = new List<ReporteInterrupcion>();
            List<ReporteInterrupcion> listaReporteTemporal;
            formatFechaCab = false;

            List<AfInterrupSuministroDTO> registros = new List<AfInterrupSuministroDTO>();
            foreach (var item in codigosEventos)
            {
                listaReporteTemporal = new List<ReporteInterrupcion>();
                registros = listaDataCompleta.Where(x => x.EVENCODI == item.EVENCODI).OrderByDescending(c => c.EVENCODI).ToList();
                var registrosHorasCoordinacion = lstHorasCoordinacion.Where(x => x.Afecodi == registros[0].Afecodi).ToList();
                foreach (var regData in registros)
                {
                    var reg = new ReporteInterrupcion();

                    reg.Suministro = regData.CodigoOsinergmin + "-" + regData.Intsumsubestacion + "(" + regData.Intsumsuministro + ")";
                    reg.Potencia = regData.Intsummw.Value;
                    reg.HoraInicio = regData.Intsumfechainterrini2;
                    reg.HoraFinal = regData.Intsumfechainterrfin2;
                    reg.Duracion = regData.Intsumduracion.Value;

                    reg.FechaInicio = regData.Intsumfechainterrini.Value;
                    reg.FechaFin = regData.Intsumfechainterrfin.Value;
                    reg.IdEmpresa = regData.Emprcodi;
                    reg.Evencodi = regData.EVENCODI.ToString();
                    reg.Evenini = regData.EVENINI.ToString("dd/MM/yyyy HH:mm:ss");


                    DateTime horaCordinacionAumentada;
                    bool celdaConDemora = false;
                    if (esPorEracmf)
                    {
                        celdaConDemora = PresentaDemoraSegunTablaNormalizacion(registrosHorasCoordinacion, reg.IdEmpresa, regData.Intsumfechainterrfin.Value, out horaCordinacionAumentada);
                    }
                    else
                    {
                        celdaConDemora = PresentaDemoraSegunTablaNormalizacionSubEstacion(registrosHorasCoordinacion, reg.IdEmpresa, regData.Intsumfechainterrfin.Value, out horaCordinacionAumentada, regData.Intsumcodi);
                    }
                    if (celdaConDemora)
                    {
                        reg.HoraFinal = horaCordinacionAumentada.ToString(ConstantesAppServicio.FormatoFechaFull2);
                        reg.FechaFin = horaCordinacionAumentada;
                        reg.Duracion = (decimal)(horaCordinacionAumentada - reg.FechaInicio).TotalMinutes;


                        //redondeo solo en la extranet
                        reg.Duracion = MathHelper.Round(reg.Duracion, ConstantesExtranetCTAF.DigitosParteDecimalDuracion);
                    }

                    reg.DuracionHoras = reg.Duracion / 60;
                    reg.EnergiaNoSuministrada = reg.Potencia * reg.DuracionHoras;
                    reg.EnergiaNoSuministrada = reg.EnergiaNoSuministrada;

                    var resultado = TieneFormatoFechaEspecialWord(reg.FechaInicio, reg.FechaFin);
                    formatFechaCab = resultado == true ? resultado : formatFechaCab;

                    listaReporte.Add(reg);
                    listaReporteTemporal.Add(reg);
                }

                listaReporte = listaReporte.OrderBy(x => x.Suministro).ThenBy(x => x.HoraInicio).ToList();

                //Totales
                ReporteInterrupcion regTotal = new ReporteInterrupcion();
                regTotal.TipoReporte = ConstantesExtranetCTAF.TipoReporteTotal;
                regTotal.Potencia = listaReporteTemporal.Sum(x => x.Potencia);
                regTotal.EnergiaNoSuministrada = listaReporteTemporal.Sum(x => x.EnergiaNoSuministrada);
                regTotal.Evencodi = item.EVENCODI.ToString();
                listaReporte.Add(regTotal);
            }
        }

        /// <summary>
        /// Obtiene las listas que se usará en el reporte 7: Reducción de suministros
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="listaAllData"></param>
        /// <param name="listaReporte"></param>
        /// <param name="formatFechaCab"></param>
        public void ListarRpt7ReduccionDeSuministro(int afecodi, int emprcodi, List<AfInterrupSuministroDTO> listaAllData, out List<ReporteInterrupcion> listaReporte, out bool formatFechaCab)
        {
            listaReporte = new List<ReporteInterrupcion>();

            //Solo considerar a los que tuvieron Reducción de MW
            List<AfInterrupSuministroDTO> listaData = listaAllData.Where(x => x.Intsummwred > 0).ToList();
            List<AfInterrupSuministroDTO> listaDataFiltro;
            var codigosEventos = listaData.Select(y => new { y.EVENCODI, y.EVENINI }).Distinct().ToList();

            //Formateo por fila
            formatFechaCab = false;
            foreach (var item in codigosEventos)
            {
                listaDataFiltro = listaData.Where(x => x.EVENCODI == item.EVENCODI).OrderByDescending(c => c.EVENCODI).ToList();

                foreach (var regData in listaDataFiltro)
                {
                    var reg = new ReporteInterrupcion();
                    reg.Suministro = regData.CodigoOsinergmin + "-" + regData.Intsumsubestacion + "(" + regData.Intsumsuministro + ")";
                    reg.Subestacion = regData.Intsumsubestacion;
                    reg.RedSumDE = regData.Intsummw.Value;
                    reg.RedSumA = regData.Intsummwfin.Value;
                    reg.Reduccion = regData.Intsummwred.Value;
                    reg.FechaInicio = regData.Intsumfechainterrini.Value;
                    reg.FechaFin = regData.Intsumfechainterrfin.Value;
                    reg.HoraInicio = regData.Intsumfechainterrini2;
                    reg.HoraFinal = regData.Intsumfechainterrfin2;
                    reg.Duracion = regData.Intsumduracion.Value;
                    reg.Afecodi = regData.Afecodi;
                    reg.Evenini = regData.EVENINI.ToString("dd/MM/yyyy HH:mm:ss");
                    reg.Evencodi = regData.EVENCODI.ToString();
                    reg.Afecodi = regData.Afecodi;
                    var resultado = TieneFormatoFechaEspecialWord(reg.FechaInicio, reg.FechaFin);
                    formatFechaCab = resultado == true ? resultado : formatFechaCab;

                    listaReporte.Add(reg);
                }

                listaReporte = listaReporte.OrderBy(x => x.Suministro).ThenBy(x => x.Subestacion).ThenBy(x => x.FechaInicio).ToList();

                //Totales
                ReporteInterrupcion regTotal = new ReporteInterrupcion();
                regTotal.TipoReporte = ConstantesExtranetCTAF.TipoReporteTotal;
                regTotal.Reduccion = listaDataFiltro.Sum(x => x.Intsummwred.Value);
                regTotal.Evencodi = item.EVENCODI.ToString();
                regTotal.Evenini = item.EVENINI.ToString();

                listaReporte.Add(regTotal);
            }
            if (codigosEventos.Count == 0)
            {

                //Totales
                ReporteInterrupcion regTotal = new ReporteInterrupcion();
                regTotal.TipoReporte = ConstantesExtranetCTAF.TipoReporteTotal;
                regTotal.Potencia = listaReporte.Sum(x => x.Potencia);

                listaReporte.Add(regTotal);
            }
        }

        /// <summary>
        /// Obtiene las listas que se usará en el reporte: para la decisión
        /// </summary>
        /// <param name="listaAllData"></param>
        /// <param name="lstHorasCoordinacion"></param>
        /// <param name="listaReporte"></param>
        /// <param name="formatFechaCab"></param>
        private void ListarRptRestablecimiento(List<AfInterrupSuministroDTO> listaAllData, List<AfHoraCoordDTO> lstHorasCoordinacion, out List<ReporteInterrupcion> listaReporte, out bool formatFechaCab)
        {
            listaAllData = listaAllData.Distinct().ToList();

            //Formateo por fila
            listaReporte = new List<ReporteInterrupcion>();
            formatFechaCab = false;
            foreach (var regData in listaAllData)
            {
                var registrosHorasCoordinacion = lstHorasCoordinacion.Where(x => x.Afecodi == regData.Afecodi && x.Intsumcodi == regData.Intsumcodi).ToList();
                var reg = new ReporteInterrupcion();
                reg.Suministro = regData.CodigoOsinergmin + "-" + regData.Intsumsubestacion + "(" + regData.Intsumsuministro + ")";
                reg.Subestacion = regData.Intsumsubestacion;

                reg.HoraInicio = regData.Intsumfechainterrini2;
                reg.HoraFinal = regData.Intsumfechainterrfin2;

                reg.FechaInicio = regData.Intsumfechainterrini.Value;
                reg.FechaFin = regData.Intsumfechainterrfin.Value;
                reg.IdEmpresa = regData.Emprcodi;
                //
                reg.NombEmpresa = regData.Emprnomb;
                //
                reg.Evencodi = regData.EVENCODI.ToString();
                reg.Evenini = regData.EVENINI.ToString("dd/MM/yyyy HH:mm:ss");

                bool tieneDemora = PresentaDemoraSegunTablaNormalizacionRestablecimiento(registrosHorasCoordinacion, reg.IdEmpresa, reg.FechaFin, out DateTime hc, regData.Afecodi, regData.Intsumcodi);
                if (tieneDemora)
                {
                    var regHoraCord = lstHorasCoordinacion.Find(x => x.Emprcodi == reg.IdEmpresa && x.Afecodi == regData.Afecodi && x.Intsumcodi == regData.Intsumcodi);

                    reg.HoraInicio = regHoraCord.Afhofecha.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                    reg.FechaInicio = regHoraCord.Afhofecha.Value;

                    var duracion = (decimal)(reg.FechaFin - regHoraCord.Afhofecha.Value).TotalMinutes;
                    reg.Duracion = duracion - 15.0m;

                    //redondeo solo en la extranet
                    reg.Duracion = MathHelper.Round(reg.Duracion, ConstantesExtranetCTAF.DigitosParteDecimalDuracion);

                    var resultado = TieneFormatoFechaEspecialWord(reg.FechaInicio, reg.FechaFin);
                    formatFechaCab = resultado == true ? resultado : formatFechaCab;

                    listaReporte.Add(reg);
                }
            }

            listaReporte = listaReporte.OrderBy(x => x.Suministro).ThenBy(x => x.Subestacion).ThenBy(x => x.FechaInicio).ToList();

            listaReporte = listaReporte.OrderBy(x => x.Suministro).ThenBy(x => x.HoraInicio).ToList();
            //Formateo por fila
            int fila = 1;
            foreach (var reg in listaReporte)
            {
                reg.NumFila = fila;
                fila++;
            }
        }

        #endregion

        #region Obtencion TablaReporte por reporte

        /// <summary>
        /// Obtiene listas para armar el reporte Total Datos
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="lstHorasCoordinacion"></param>
        /// <param name="esPorEracmf"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataTablaRptTotalDatos(List<ReporteInterrupcion> listaReporte, List<AfHoraCoordDTO> lstHorasCoordinacion, bool esPorEracmf)
        {
            TablaReporte tabla = ObtenerDayaTablaDatosTotales_MalasActuaciones_Menores3minutos(listaReporte, lstHorasCoordinacion, esPorEracmf);
            return tabla;
        }

        /// <summary>
        /// Obtiene listas para armar el reporte: Resumen
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataTablaRptResumen(List<ReporteInterrupcion> listaReporte)
        {
            TablaReporte tabla = new TablaReporte();

            #region cabecera
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[0, 0];


            cabRepo.CabeceraData = matrizCabecera;

            tabla.Cabecera = cabRepo;

            #endregion

            #region cuerpo

            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();
            ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal);

            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();
            foreach (var reg in listaData)
            {
                RegistroReporte registro = new RegistroReporte();
                List<CeldaReporte> datos = new List<CeldaReporte>();

                datos.Add(this.GetCeldaEspecialZona("Zona " + reg.Zona, false, false));
                datos.Add(this.GetCeldaEspecialEmpresa(reg.NombEmpresa, false, false, true, reg.IdEmpresa));
                datos.Add(this.GetCeldaEspecialMW((reg.Etapaf1), false, false, reg.ColorEtapaf1));
                datos.Add(this.GetCeldaEspecialMW((reg.Etapaf2), false, false, reg.ColorEtapaf2));
                datos.Add(this.GetCeldaEspecialMW((reg.Etapaf3), false, false, reg.ColorEtapaf3));
                datos.Add(this.GetCeldaEspecialMW((reg.Etapaf4), false, false, reg.ColorEtapaf4));
                datos.Add(this.GetCeldaEspecialMW((reg.Etapaf5), false, false, reg.ColorEtapaf5));
                datos.Add(this.GetCeldaEspecialMW((reg.Etapaf6), false, false, reg.ColorEtapaf6));
                datos.Add(this.GetCeldaEspecialMW((reg.Etapaf7), false, false, reg.ColorEtapaf7));
                datos.Add(this.GetCeldaEspecialMW((reg.EtapaDf1), false, false, reg.ColorEtapaDf1));
                datos.Add(this.GetCeldaEspecialMW((reg.EtapaDf2), false, false, reg.ColorEtapaDf2));
                datos.Add(this.GetCeldaEspecialMW((reg.EtapaDf3), false, false, reg.ColorEtapaDf3));

                datos.Add(this.GetCeldaEspecialMW(reg.TotalZona, false, false, false));

                registro.ListaCelda = datos;
                registro.Emprcodi = reg.IdEmpresa;
                registros.Add(registro);
            }

            //Totales
            if (regTotal != null)
            {
                RegistroReporte registro0 = new RegistroReporte();
                registro0.EsFilaResumen = true;
                List<CeldaReporte> datos0 = new List<CeldaReporte>();

                datos0.Add(this.GetCeldaEspecialZona(string.Empty, true, false));
                datos0.Add(this.GetCeldaEspecialTotal("TOTAL", true, false));

                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaf1, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaf2, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaf3, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaf4, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaf5, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaf6, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaf7, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaDf1, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaDf2, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaDf3, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaTotal, true, true, false));

                registro0.ListaCelda = datos0;

                registros.Add(registro0);
            }


            tabla.ListaRegistros = registros;

            #endregion
            return tabla;
        }

        /// <summary>
        /// Obtiene listas para formas el reporte Malas Actuaciones
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="esPorEracmf"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataTablaRptMalasActuaciones(List<ReporteInterrupcion> listaReporte, bool esPorEracmf)
        {
            TablaReporte tabla = ObtenerDayaTablaDatosTotales_MalasActuaciones_Menores3minutos(listaReporte, null, esPorEracmf);
            return tabla;
        }

        /// <summary>
        /// Obtiene listas para formas el reporte Menores a 3 minutos
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="esPorEracmf"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataTablaRptMenores3Minutos(List<ReporteInterrupcion> listaReporte, bool esPorEracmf)
        {
            TablaReporte tabla = ObtenerDayaTablaDatosTotales_MalasActuaciones_Menores3minutos(listaReporte, null, esPorEracmf);
            return tabla;
        }

        /// <summary>
        /// Obtiene listas para formas el reporte No reportaron informacion 
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataTablaRptNoReportaronInterrupcion(List<ReporteInterrupcion> listaReporte)
        {
            TablaReporte tabla = new TablaReporte();

            #region cabecera
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[1, 6];
            matrizCabecera[0, 0] = "Zona";
            matrizCabecera[0, 1] = "Empresa";
            matrizCabecera[0, 2] = "Suministro";
            matrizCabecera[0, 3] = "S.E.";
            matrizCabecera[0, 4] = "Función";
            matrizCabecera[0, 5] = "Etapa";

            cabRepo.CabeceraData = matrizCabecera;

            tabla.Cabecera = cabRepo;

            #endregion

            #region cuerpo
            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();
            ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal);

            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();
            foreach (var reg in listaData)
            {
                RegistroReporte registro = new RegistroReporte();
                List<CeldaReporte> datos = new List<CeldaReporte>();

                datos.Add(this.GetCeldaEspecialZona(reg.Zona, false, false));
                datos.Add(this.GetCeldaEspecialEmpresa(reg.NombEmpresa, false, false, true, reg.IdEmpresa));
                datos.Add(this.GetCeldaEspecialSuministro(reg.Suministro, false, false));
                datos.Add(this.GetCeldaEspecialSE(reg.Subestacion, false, false));
                datos.Add(this.GetCeldaEspecialFuncion((reg.Funcion != null ? (reg.Funcion.ToUpper() == "F" ? "f" : (reg.Funcion.ToUpper() == "DF" ? "Df" : reg.Funcion)) : ""), false, false));
                datos.Add(this.GetCeldaEspecialEtapa(reg.Etapa, false, false));

                registro.ListaCelda = datos;

                registros.Add(registro);
            }

            tabla.ListaRegistros = registros;

            #endregion

            return tabla;
        }

        /// <summary>
        /// Obtiene listas para armar el reporte Total Datos
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataTablaRptDecision(List<ReporteInterrupcion> listaReporte)
        {
            TablaReporte tabla = new TablaReporte();

            #region cabecera
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[1, 6];
            matrizCabecera[0, 0] = "N°";
            matrizCabecera[0, 1] = "Suministro";
            matrizCabecera[0, 2] = "Potencia (MW)";
            matrizCabecera[0, 3] = "Inicio";
            matrizCabecera[0, 4] = "Final";
            matrizCabecera[0, 5] = "Duración (min)";

            cabRepo.CabeceraData = matrizCabecera;

            tabla.Cabecera = cabRepo;

            #endregion

            #region cuerpo
            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();

            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();
            foreach (var reg in listaData)
            {
                RegistroReporte registro = new RegistroReporte();
                List<CeldaReporte> datos = new List<CeldaReporte>();

                datos.Add(this.GetCeldaEspecialFila(reg.NumFila, false, false));
                datos.Add(this.GetCeldaEspecialSuministro(reg.Suministro, false, false, false, true, reg.IdEmpresa));
                datos.Add(this.GetCeldaEspecialMW(reg.Potencia, false, false, false));
                datos.Add(this.GetCeldaEspecialHora(reg.HoraInicio, false, false));
                datos.Add(this.GetCeldaEspecialHora(reg.HoraFinal, false, false));
                datos.Add(this.GetCeldaEspecialDuracion(reg.Duracion, false, false, false));

                registro.ListaCelda = datos;

                registros.Add(registro);
            }

            tabla.ListaRegistros = registros;

            #endregion
            return tabla;
        }

        /// <summary>
        /// Obtiene listas para armar el reporte que reportaron 0
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataTablaRptReportaron0(List<ReporteInterrupcion> listaReporte)
        {
            TablaReporte tabla = new TablaReporte();

            #region cabecera
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[1, 2];
            matrizCabecera[0, 0] = "Empresa";
            matrizCabecera[0, 1] = "Motivo";

            cabRepo.CabeceraData = matrizCabecera;

            tabla.Cabecera = cabRepo;

            #endregion

            #region cuerpo
            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();
            ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal);

            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();

            foreach (var listaAgrupada in listaData.GroupBy(x => x.IdEmpresa))
            {
                List<string> observaciones = new List<string>();

                var listaAgrupNew = listaAgrupada.ToList();
                var reg = listaAgrupNew.First();

                foreach (var val in listaAgrupNew)
                {
                    if (val.Observaciones != string.Empty)
                    {
                        observaciones.Add(val.Observaciones.ToUpper());
                    }
                }
                string listObservacion = observaciones.Count > 0 ? string.Join(",", observaciones) : string.Empty;

                RegistroReporte registro = new RegistroReporte();
                List<CeldaReporte> datos = new List<CeldaReporte>();

                datos.Add(this.GetCeldaEspecialEmpresa(reg.NombEmpresa, false, false, true, reg.IdEmpresa));
                datos.Add(this.GetCeldaEspecialObservacion(listObservacion, false, false));
                registro.ListaCelda = datos;

                registros.Add(registro);
            }

            tabla.ListaRegistros = registros;

            #endregion

            return tabla;
        }

        /// <summary>
        /// Obtiene listas para armar el reporte de Para el Resarcimiento
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataTablaRptResarcimiento(List<ReporteInterrupcion> listaReporte)
        {
            TablaReporte tabla = new TablaReporte();

            #region cabecera
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[1, 7];
            matrizCabecera[0, 0] = "Suministros Afectados";
            matrizCabecera[0, 1] = "Potencia Interrumpida (MW)";
            matrizCabecera[0, 2] = "Hora Inicio";
            matrizCabecera[0, 3] = "Hora Final";
            matrizCabecera[0, 4] = "Duración (min)";
            matrizCabecera[0, 5] = "Duración (Hrs)";
            matrizCabecera[0, 6] = "Energía no Suministrada (MWH)";

            cabRepo.CabeceraData = matrizCabecera;

            tabla.Cabecera = cabRepo;

            #endregion

            #region cuerpo
            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();
            ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal);
            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();
            foreach (var reg in listaData)
            {
                RegistroReporte registro = new RegistroReporte();
                List<CeldaReporte> datos = new List<CeldaReporte>();

                datos.Add(this.GetCeldaEspecialSuministro(reg.Suministro, false, false, false, true, reg.IdEmpresa));
                datos.Add(this.GetCeldaEspecialMW(reg.Potencia, false, false, false));
                datos.Add(this.GetCeldaEspecialHora(reg.HoraInicio, false, false));
                datos.Add(this.GetCeldaEspecialHora(reg.HoraFinal, false, false));
                datos.Add(this.GetCeldaEspecialDuracion(reg.Duracion, false, false, false));
                datos.Add(this.GetCeldaEspecialDuracionHoras(reg.DuracionHoras, false, false, false));
                datos.Add(this.GetCeldaEspecialENSTruncate(reg.EnergiaNoSuministrada, false, false, false));

                registro.ListaCelda = datos;

                registros.Add(registro);
            }


            if (regTotal != null)
            {
                //Totales
                RegistroReporte registro0 = new RegistroReporte();
                registro0.AltoFila = 0.5M;
                registro0.EsFilaResumen = true;
                registro0.EsAgrupado = true;
                List<CeldaReporte> datos0 = new List<CeldaReporte>();

                datos0.Add(this.GetCeldaEspecialENS("TOTAL(MW)--->", true, true));
                datos0.Add(this.GetCeldaEspecialENSTruncate(regTotal.Potencia, false, false, false));

                datos0.Add(this.GetCeldaEspecialFilaVacia());
                datos0.Add(this.GetCeldaEspecialFilaVacia());
                datos0.Add(this.GetCeldaEspecialFilaVacia());
                datos0.Add(this.GetCeldaEspecialENS("ENS<sub>F</sub>(MWH)--->", true, true));
                datos0.Add(this.GetCeldaEspecialENSTruncate(regTotal.EnergiaNoSuministrada, false, false, false));
                //datos0.Add(this.GetCeldaEspecialTotal("MWH", true, false));

                registro0.ListaCelda = datos0;

                registros.Add(registro0);
            }




            tabla.ListaRegistros = registros;

            #endregion
            return tabla;
        }

        private TablaReporte ObtenerDataTablaRptRestablecimiento(List<ReporteInterrupcion> listaReporte, bool formatFechaCab)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = ConstantesExtranetCTAF.InterrupRpt02DemoraRestablecimiento;

            //Caracteristicas generales del reporte
            tabla.TamLetra = 8;
            tabla.TipoFuente = "Calibri";
            tabla.Color = "#bfbfbf";
            tabla.EsMayuscula = true;

            #region cabecera
            CabeceraReporte cabRepo = new CabeceraReporte();
            string[,] matrizCabecera = new string[0, 0];
            cabRepo.CabeceraData = matrizCabecera;

            tabla.Cabecera = cabRepo;

            tabla.AltoFilaCab = 0.7M;

            List<CabeceraReporteColumna> columnasCabecera = new List<CabeceraReporteColumna>();

            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "SUMINISTRO", AnchoColumna = 4M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "SUBESTACIÓN", AnchoColumna = 2M });

            if (formatFechaCab)
            {
                tabla.AltoFilaCab = 1.24M;
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "INICIO\n (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 2.75M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FINAL\n (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 3M });
            }
            else
            {
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "INICIO\n (HH:MM:SS)", AnchoColumna = 2.75M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FINAL\n (HH:MM:SS)", AnchoColumna = 3M });
            }
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "DEMORA EN EL \nRESTABLECIMIENTO \n(MIN)", AnchoColumna = 2.60M });

            tabla.CabeceraColumnas = columnasCabecera;

            #endregion

            #region cuerpo

            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();

            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();
            foreach (var reg in listaData)
            {
                RegistroReporte registro = new RegistroReporte();
                List<CeldaReporte> datos = new List<CeldaReporte>();

                registro.AltoFila = 0.5M;

                datos.Add(this.GetCeldaEspecialSuministro(reg.Suministro, false, false));
                datos.Add(this.GetCeldaEspecialSE(reg.Subestacion, false, false));
                datos.Add(this.GetCeldaEspecialHora(reg.HoraInicio, false, false, false, formatFechaCab));
                datos.Add(this.GetCeldaEspecialHora(reg.HoraFinal, false, false, false, formatFechaCab));
                datos.Add(this.GetCeldaEspecialDuracion(reg.Duracion, false, false, false));

                registro.ListaCelda = datos;
                registro.Nombre = string.Format("Hora Inicio:{0}", reg.Evenini);
                registro.codigo = reg.Evencodi;
                registros.Add(registro);
            }
            tabla.ListaRegistros = registros;

            #endregion
            return tabla;
        }

        /// <summary>
        /// ObtenerDataTablaRpt7ReduccionDeSuministro
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataTablaRpt7ReduccionDeSuministro(List<ReporteInterrupcion> listaReporte)
        {
            TablaReporte tabla = new TablaReporte();

            #region cabecera
            CabeceraReporte cabRepo = new CabeceraReporte();

            string[,] matrizCabecera = new string[1, 8];
            matrizCabecera[0, 0] = "Suministro";
            matrizCabecera[0, 1] = "S.E.";
            matrizCabecera[0, 2] = "De (MW)";
            matrizCabecera[0, 3] = "A (MW)";
            matrizCabecera[0, 4] = "Reducción (MW)";
            matrizCabecera[0, 5] = "Inicio";
            matrizCabecera[0, 6] = "Fin";
            matrizCabecera[0, 7] = "Duración (min)";

            cabRepo.CabeceraData = matrizCabecera;

            tabla.Cabecera = cabRepo;

            #endregion

            #region cuerpo
            ReporteInterrupcion regTotal = new ReporteInterrupcion();
            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();
            regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal);

            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();
            foreach (var reg in listaData)
            {
                RegistroReporte registro = new RegistroReporte();
                List<CeldaReporte> datos = new List<CeldaReporte>();

                datos.Add(this.GetCeldaEspecialSuministro(reg.Suministro, false, false));
                datos.Add(this.GetCeldaEspecialSE(reg.Subestacion, false, false));

                datos.Add(this.GetCeldaEspecialMW(reg.RedSumDE, false, false, false));
                datos.Add(this.GetCeldaEspecialMW(reg.RedSumA, false, false, false));
                datos.Add(this.GetCeldaEspecialMW(reg.Reduccion, false, false, false));

                datos.Add(this.GetCeldaEspecialHora(reg.HoraInicio, false, false));
                datos.Add(this.GetCeldaEspecialHora(reg.HoraFinal, false, false));

                datos.Add(this.GetCeldaEspecialDuracion(reg.Duracion, false, false, false));

                registro.ListaCelda = datos;

                registros.Add(registro);
            }

            //Totales
            RegistroReporte registro0 = new RegistroReporte();
            registro0.EsFilaResumen = true;
            List<CeldaReporte> datos0 = new List<CeldaReporte>();


            datos0.Add(this.GetCeldaEspecialSuministro(string.Empty, true, false));
            datos0.Add(this.GetCeldaEspecialTotal("TOTAL", true, false));

            datos0.Add(this.GetCeldaEspecialMW(null, true, true, false));
            datos0.Add(this.GetCeldaEspecialMW(null, true, true, false));
            datos0.Add(this.GetCeldaEspecialMW(regTotal.Reduccion, true, true, false));

            datos0.Add(this.GetCeldaEspecialHora(string.Empty, true, true));
            datos0.Add(this.GetCeldaEspecialHora(string.Empty, true, true));

            datos0.Add(this.GetCeldaEspecialDuracion(null, true, true, false));

            registro0.ListaCelda = datos0;

            registros.Add(registro0);
            tabla.ListaRegistros = registros;

            #endregion

            return tabla;
        }

        #endregion

        #region Validacion Columnas

        /// <summary>
        /// Get Celda Especial MW
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <param name="tieneTextoColor"></param>
        /// <param name="tieneAgrupacion"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialMW(decimal? valor, bool tieneTextoNegrita, bool tieneBorder, bool tieneTextoColor, bool tieneAgrupacion = false)
        {
            CeldaReporte reg = new CeldaReporte();
            reg.EsNumero = true;
            reg.Valor = valor;
            reg.TieneTextoNegrita = tieneTextoNegrita;
            reg.TieneFormatoNumeroEspecial = ConstantesExtranetCTAF.TieneFormatoNumeroEspecialMW;
            reg.EsNumeroTruncado = ConstantesExtranetCTAF.EsNumeroTruncadoMW;
            reg.EsNumeroRedondeado = ConstantesExtranetCTAF.EsNumeroRedondeadoMW;
            reg.DigitosParteDecimal = ConstantesExtranetCTAF.DigitosParteDecimalMW;
            reg.TieneTextoCentrado = true;
            reg.TieneBorder = tieneBorder;
            reg.TieneTextoColor = tieneTextoColor;
            reg.TieneAgrupacion = tieneAgrupacion;
            return reg;
        }

        /// <summary>
        /// Get Celda Especial Duracion
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <param name="tieneColor"></param>
        /// <param name="tieneAgrupacion"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialDuracion(decimal? valor, bool tieneTextoNegrita, bool tieneBorder, bool tieneColor, bool tieneAgrupacion = false)
        {
            CeldaReporte reg = new CeldaReporte();
            reg.EsNumero = true;
            reg.Valor = valor;
            reg.TieneTextoNegrita = tieneTextoNegrita;
            reg.TieneFormatoNumeroEspecial = ConstantesExtranetCTAF.TieneFormatoNumeroEspecialDuracion;
            reg.EsNumeroTruncado = ConstantesExtranetCTAF.EsNumeroTruncadoDuracion;
            reg.EsNumeroRedondeado = ConstantesExtranetCTAF.EsNumeroRedondeadoDuracion;
            reg.DigitosParteDecimal = ConstantesExtranetCTAF.DigitosParteDecimalDuracion;
            reg.TieneTextoCentrado = true;
            reg.TieneBorder = tieneBorder;
            reg.TieneColor = tieneColor;
            reg.TieneAgrupacion = tieneAgrupacion;

            return reg;
        }

        /// <summary>
        /// Get Celda Especial Duracion Horas
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <param name="tieneColor"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialDuracionHoras(decimal? valor, bool tieneTextoNegrita, bool tieneBorder, bool tieneColor)
        {
            CeldaReporte reg = new CeldaReporte();
            reg.EsNumero = true;
            reg.Valor = valor;
            reg.TieneTextoNegrita = tieneTextoNegrita;
            reg.TieneFormatoNumeroEspecial = ConstantesExtranetCTAF.TieneFormatoNumeroEspecialDuracion;
            reg.EsNumeroTruncado = ConstantesExtranetCTAF.EsNumeroTruncadoDuracion;
            reg.EsNumeroRedondeado = ConstantesExtranetCTAF.EsNumeroRedondeadoDuracion;
            reg.DigitosParteDecimal = ConstantesExtranetCTAF.DigitosParteDecimalDuracion;
            reg.TieneTextoCentrado = true;
            reg.TieneBorder = tieneBorder;
            reg.TieneColor = tieneColor;

            return reg;
        }

        /// <summary>
        /// Get Celda Especial Hora
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <param name="tieneAgrupacion"></param>
        /// <param name="tieneFecha"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialHora(string valor, bool tieneTextoNegrita, bool tieneBorder, bool tieneAgrupacion = false, bool tieneFecha = false)
        {
            var reg = new CeldaReporte(valor, tieneTextoNegrita, true, false);
            reg.TieneBorder = tieneBorder;
            reg.EsTextoFecha = true;
            reg.TieneFormatoFechaExcel = tieneFecha;
            reg.TieneAgrupacion = tieneAgrupacion;
            return reg;
        }

        /// <summary>
        /// Get Celda Especial Suministro
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <param name="tieneAgrupacion"></param>
        /// <param name="tieneERACMF"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialSuministro(string valor, bool tieneTextoNegrita, bool tieneBorder, bool tieneAgrupacion = false, bool tieneERACMF = false, int idEmpresa = 0)
        {
            var reg = new CeldaReporte(valor, tieneTextoNegrita, false, true);
            reg.TieneBorder = tieneBorder;
            reg.TieneAgrupacion = tieneAgrupacion;
            if (tieneERACMF)
            {
                var regAfEmpresa = GetByAfEmpresaxEmprcodi(idEmpresa);
                if (regAfEmpresa != null && regAfEmpresa.Afalerta == "S")
                    reg.TieneColorERACMF = tieneERACMF;
            }

            return reg;
        }

        /// <summary>
        /// Get Celda Especial SE
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialSE(string valor, bool tieneTextoNegrita, bool tieneBorder)
        {
            var reg = new CeldaReporte(valor, tieneTextoNegrita, true, false);
            reg.TieneBorder = tieneBorder;
            return reg;
        }

        /// <summary>
        /// Get Celda Especial Zona
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <param name="tieneAgrupacion"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialZona(string valor, bool tieneTextoNegrita, bool tieneBorder, bool tieneAgrupacion = false)
        {
            var reg = new CeldaReporte(valor, tieneTextoNegrita, true, false);
            reg.TieneBorder = tieneBorder;
            reg.TieneAgrupacion = tieneAgrupacion;
            return reg;
        }

        /// <summary>
        /// Get Celda Especial Empresa
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <param name="tieneERACMF"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialEmpresa(string valor, bool tieneTextoNegrita, bool tieneBorder, bool tieneERACMF = false, int idEmpresa = 0)
        {
            var reg = new CeldaReporte(valor, tieneTextoNegrita, false, true);
            reg.TieneBorder = tieneBorder;
            reg.TieneTextoCentrado = true;
            if (tieneERACMF)
            {
                var regAfEmpresa = GetByAfEmpresaxEmprcodi(idEmpresa);
                if (regAfEmpresa != null && regAfEmpresa.Afalerta == "S")
                    reg.TieneColorERACMF = tieneERACMF;
            }
            return reg;
        }

        /// <summary>
        /// Get Celda Especial Empresa
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialCodigoNombreEmpresa(string valor, bool tieneTextoNegrita, bool tieneBorder)
        {
            var reg = new CeldaReporte(valor, tieneTextoNegrita, false, true);
            reg.TieneBorder = tieneBorder;
            return reg;
        }
        /// <summary>
        /// Get Celda Especial Funcion
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <param name="tieneAgrupacion"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialFuncion(string valor, bool tieneTextoNegrita, bool tieneBorder, bool tieneAgrupacion = false)
        {
            var reg = new CeldaReporte(valor, tieneTextoNegrita, true, false);
            reg.TieneBorder = tieneBorder;
            reg.TieneAgrupacion = tieneAgrupacion;
            return reg;
        }

        /// <summary>
        /// Get Celda Especial Etapa
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <param name="tieneAgrupacion"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialEtapa(string valor, bool tieneTextoNegrita, bool tieneBorder, bool tieneAgrupacion = false)
        {
            var reg = new CeldaReporte(valor, tieneTextoNegrita, true, false);
            reg.TieneBorder = tieneBorder;
            reg.TieneAgrupacion = tieneAgrupacion;
            return reg;
        }

        /// <summary>
        /// Get Celda Especial Fila
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialFila(int valor, bool tieneTextoNegrita, bool tieneBorder)
        {
            var reg = new CeldaReporte(valor, tieneTextoNegrita, true, false);
            reg.EsNumero = true;
            reg.Valor = valor;
            reg.TieneBorder = tieneBorder;
            return reg;
        }

        /// <summary>
        /// Get Celda Especial Total
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <param name="tieneAgrupacion"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialTotal(string valor, bool tieneTextoNegrita, bool tieneBorder, bool tieneAgrupacion = false)
        {
            var reg = new CeldaReporte(valor, tieneTextoNegrita, false, false, true);
            reg.TieneBorder = tieneBorder;
            reg.TieneAgrupacion = tieneAgrupacion;
            return reg;
        }

        /// <summary>
        /// Get Celda Especial Observaciones
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialObservacion(string valor, bool tieneTextoNegrita, bool tieneBorder)
        {
            var reg = new CeldaReporte(valor, tieneTextoNegrita, false, true);
            reg.TieneBorder = tieneBorder;
            reg.TieneTextoCentrado = true;
            return reg;
        }

        /// <summary>
        /// Get Celda Especial Hora coordinacion
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialHoraCoordinacion(string valor, bool tieneTextoNegrita, bool tieneBorder)
        {
            var reg = new CeldaReporte(valor, tieneTextoNegrita, false, true);
            reg.TieneBorder = tieneBorder;
            reg.TieneTextoCentrado = true;
            return reg;
        }

        /// <summary>
        /// Get Celda Especial Fila sin formato
        /// </summary>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialFilaVacia(bool tieneAgrupacion = false)
        {
            var reg = new CeldaReporte();
            reg.TieneAgrupacion = tieneAgrupacion;
            return reg;
        }

        /// <summary>
        /// Get Celda Especial Ens
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialENS(string valor, bool tieneTextoNegrita, bool tieneBorder)
        {
            var reg = new CeldaReporte(valor, tieneTextoNegrita, false, true);
            reg.TieneBorder = tieneBorder;
            reg.TieneTextoCentrado = true;
            return reg;
        }

        /// <summary>
        /// Get Celda Especial Energía no suministrada truncada
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="tieneTextoNegrita"></param>
        /// <param name="tieneBorder"></param>
        /// <param name="tieneTextoColor"></param>
        /// <param name="tieneAgrupacion"></param>
        /// <returns></returns>
        private CeldaReporte GetCeldaEspecialENSTruncate(decimal? valor, bool tieneTextoNegrita, bool tieneBorder, bool tieneTextoColor, bool tieneAgrupacion = false)
        {
            CeldaReporte reg = new CeldaReporte();
            reg.EsNumero = true;
            reg.Valor = valor;
            reg.TieneTextoNegrita = tieneTextoNegrita;
            reg.TieneFormatoNumeroEspecial = false;
            reg.EsNumeroTruncado = ConstantesExtranetCTAF.EsNumeroTruncadoMW;
            reg.EsNumeroRedondeado = ConstantesExtranetCTAF.EsNumeroRedondeadoMW;
            reg.DigitosParteDecimal = ConstantesExtranetCTAF.DigitosParteDecimalMW;
            reg.TieneTextoCentrado = true;
            reg.TieneBorder = tieneBorder;
            reg.TieneTextoColor = tieneTextoColor;
            reg.TieneAgrupacion = tieneAgrupacion;

            return reg;
        }

        #endregion

        #region Excel Web

        /// <summary>
        /// Permite guardar registros de horas de coordinación
        /// </summary>
        /// <param name="username"></param>
        /// <param name="listaHansonHoraCoord"></param>
        /// <param name="listaHansonHoraCoordSumini"></param>
        /// <returns></returns>
        public bool GuardarHorasCoordinacion(string username, List<AfHoraCoordDTO> listaHansonHoraCoord, List<AfHoraCoordDTO> listaHansonHoraCoordSumini, int afecodiSco)
        {
            //si existe más de una empresa eliminar todos los del afecodi sino el seleccionado
            int emprcodi = listaHansonHoraCoord.Count > 1 ? -1 : (listaHansonHoraCoord.Count == 1 ? listaHansonHoraCoord.First().Emprcodi : 0);

            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        var afecodi = listaHansonHoraCoord.First().Afecodi;
                        var fdatcodi = listaHansonHoraCoord.First().Fdatcodi;
                        //FactorySic.GetAfHoraCoordRepository().DeleteHoraCoord(afecodi, fdatcodi, emprcodi, connection, transaction);

                        foreach (var horacoord in listaHansonHoraCoord)
                        {
                            if (horacoord.Afhocodi > 0)
                                FactorySic.GetAfHoraCoordRepository().Delete(horacoord.Afhocodi);

                            horacoord.Afhofecha = DateTime.ParseExact(horacoord.Afhofechadescripcion, ConstantesBase.FormatoFechaFullBase, CultureInfo.InvariantCulture);
                            horacoord.Afhousucreacion = username;
                            horacoord.Afhofeccreacion = DateTime.Now;

                            int id = FactorySic.GetAfHoraCoordRepository().Save(horacoord, connection, transaction);
                        }

                        if (listaHansonHoraCoordSumini != null)
                        {
                            foreach (var horacoordSumin in listaHansonHoraCoordSumini)
                            {
                                horacoordSumin.Afhofecha = DateTime.ParseExact(horacoordSumin.Afhofechadescripcion, ConstantesBase.FormatoFechaFullBase, CultureInfo.InvariantCulture);
                                horacoordSumin.Afecodi = afecodiSco;
                                FactorySic.GetAfHoraCoordRepository().UpdateHoraCoordSuministradora(horacoordSumin, connection, transaction);
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return true;
                }
            }
        }

        /// <summary>
        /// Obtener Eracmf Evento
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="evencodi"></param>
        /// <returns></returns>
        public List<AfInterrupSuministroDTO> ObtenerEracmfEvento(int emprcodi, int evencodi)
        {
            List<AfEracmfEventoDTO> lstEracmfEvento = FactorySic.GetAfEracmfEventoRepository().GetByEvento(emprcodi, evencodi);

            //Caso ENEL DISTRIBUCION PERU, para la SE Zarate tiene dos Etapa 1 con el mismo circuito alimentador. en el excel solo debe reflejarse uno
            lstEracmfEvento = lstEracmfEvento.GroupBy(x => new { x.Eracmfzona, x.Eracmfemprnomb, x.Eracmfciralimentador, x.Eracmfsubestacion, x.Eracmfnumetapa })
                .Select(x => new AfEracmfEventoDTO()
                {
                    Eracmfzona = x.Key.Eracmfzona,
                    Eracmfemprnomb = x.Key.Eracmfemprnomb,
                    Eracmfciralimentador = x.Key.Eracmfciralimentador,
                    Eracmfsubestacion = x.Key.Eracmfsubestacion,
                    Eracmfnumetapa = x.Key.Eracmfnumetapa
                }).ToList();

            List<AfInterrupSuministroDTO> lstInterrSum = new List<AfInterrupSuministroDTO>();

            foreach (var item in lstEracmfEvento)
            {
                string numetapa = item.Eracmfnumetapa.Trim().Last().ToString();
                int numeroetapa = 1;
                if (!string.IsNullOrEmpty(numetapa) && numetapa.Any(x => char.IsDigit(x)))
                {
                    int.TryParse(numetapa, out int intnumetapa);
                    numeroetapa = intnumetapa;
                }

                lstInterrSum.Add(new AfInterrupSuministroDTO()
                {
                    Intsumzona = item.Eracmfzona,
                    Intsumempresa = item.Eracmfemprnomb,
                    Intsumsuministro = item.Eracmfciralimentador,
                    Intsumsubestacion = item.Eracmfsubestacion,
                    Intsumnumetapa = numeroetapa
                });
            }

            return lstInterrSum;
        }

        /// <summary>
        /// Obtener Listado AfCondiciones por afecodi
        /// </summary>
        /// <param name="afecodi"></param>
        /// <returns></returns>
        public List<AfCondicionesDTO> ListHandsonCondiciones(int afecodi)
        {
            List<AfCondicionesDTO> listResult = new List<AfCondicionesDTO>();

            var lstCondiciones = FactorySic.GetAfCondicionesRepository().ListByAfecodi(afecodi);
            var lstCondicionesGrp = lstCondiciones.GroupBy(x => new { x.Afecodi, x.Afcondfuncion, x.Afcondzona });

            foreach (var condicion in lstCondicionesGrp)
            {
                var condi = condicion.First();

                condi.Afcondnumetapadescrip = "Etapa " + condicion.Max(x => x.Afcondnumetapa);
                condi.Afcondzona = "Zona " + condi.Afcondzona;
                listResult.Add(condi);
            }

            if (listResult.Count <= 4)
            {
                for (int i = listResult.Count; i < 4; i++)
                {
                    listResult.Add(new AfCondicionesDTO() { Afecodi = afecodi });
                }
            }

            return listResult;
        }

        /// <summary>
        /// Guardar AfCondiciones
        /// </summary>
        /// <param name="name"></param>
        /// <param name="listaHansonCondiciones"></param>
        /// <returns></returns>
        public bool SaveAfCondiciones(string name, List<AfCondicionesDTO> listaHansonCondiciones)
        {
            List<AfCondicionesDTO> lstCondiones = CompletarEtapasAnteriores(listaHansonCondiciones);

            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        var afecodi = lstCondiones.First().Afecodi;
                        FactorySic.GetAfCondicionesRepository().DeleteByAfecodi(afecodi, connection, transaction);

                        foreach (var condicion in lstCondiones)
                        {
                            condicion.Afcondusucreacion = name;
                            condicion.Afcondfeccreacion = DateTime.Now;
                            condicion.Afcondestado = 1;
                            FactorySic.GetAfCondicionesRepository().Save(condicion, connection, transaction);
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return true;
                }
            }
        }

        /// <summary>
        /// Permite completar etapas anteriores de las condiciones
        /// </summary>
        /// <param name="listInput"></param>
        /// <returns></returns>
        private List<AfCondicionesDTO> CompletarEtapasAnteriores(List<AfCondicionesDTO> listInput)
        {
            List<AfCondicionesDTO> listaHansonCondiciones = new List<AfCondicionesDTO>();
            foreach (var reg in listInput)
            {
                var numEtapa = int.Parse(reg.Afcondnumetapadescrip.Trim().Last().ToString());
                var zona = reg.Afcondzona.Trim().Last().ToString();

                if (zona == "A" || zona == "B")
                {
                    if (reg.Afcondfuncion == "f" && numEtapa >= 1 && numEtapa <= 7)
                    {
                        listaHansonCondiciones.Add(reg);
                    }

                    if (reg.Afcondfuncion == "Df" && numEtapa >= 1 && numEtapa <= 3)
                    {
                        listaHansonCondiciones.Add(reg);
                    }
                }
            }

            var listCondiciones = listaHansonCondiciones.GroupBy(x => new { x.Afecodi, x.Afcondfuncion, x.Afcondnumetapadescrip, x.Afcondzona });

            List<AfCondicionesDTO> lstCondiones = new List<AfCondicionesDTO>();

            foreach (var condici in listCondiciones)
            {
                var cond = condici.First();
                var numEtapa = int.Parse(cond.Afcondnumetapadescrip.Trim().Last().ToString());
                cond.Afcondnumetapa = numEtapa;
                cond.Afcondzona = cond.Afcondzona.Trim().Last().ToString();

                lstCondiones.Add(cond);

                for (int etapa = 1; etapa < numEtapa; etapa++)
                {
                    lstCondiones.Add(new AfCondicionesDTO() { Afecodi = cond.Afecodi, Afcondfuncion = cond.Afcondfuncion, Afcondzona = cond.Afcondzona, Afcondnumetapa = etapa });
                }
            }

            return lstCondiones;
        }



        #endregion

        #region Metodos Generales

        /// <summary>
        /// Determina si un registro de interrucion presenta demora segun la tabla de Coordinacion y Normalizacion
        /// </summary>
        /// <param name="tabHoraCoord"></param>
        /// <param name="emprcodi"></param>
        /// <param name="fechaFin"></param>
        /// <param name="horaCordinacion"></param>
        /// <returns></returns>
        private bool PresentaDemoraSegunTablaNormalizacion(List<AfHoraCoordDTO> tabHoraCoord, int emprcodi, DateTime fechaFin, out DateTime horaCordinacion)
        {
            horaCordinacion = new DateTime();
            bool tieneDemora = false;

            //Obtenemos Tabla Normalizacion Coordinacion
            if (tabHoraCoord != null) // solo reporte TotalDatos
            {
                if (tabHoraCoord.Any())
                {
                    var idEmpresaInterrupcion = emprcodi;

                    var regHoraCord = tabHoraCoord.Find(x => x.Emprcodi == idEmpresaInterrupcion);

                    if (regHoraCord != null && regHoraCord.Afhofecha != null)
                    {
                        var horaCord = regHoraCord.Afhofecha.Value;
                        var horaAdicionada = horaCord.AddMinutes(15);

                        int val = fechaFin.CompareTo(horaAdicionada);

                        if (val > 0)
                        {
                            tieneDemora = true;
                            horaCordinacion = horaAdicionada;
                        }
                    }
                }
            }

            return tieneDemora;
        }

        /// <summary>
        /// Determina si un registro de interrucion presenta demora segun la tabla de Coordinacion y Normalizacion
        /// </summary>
        /// <param name="tabHoraCoord"></param>
        /// <param name="emprcodi"></param>
        /// <param name="fechaFin"></param>
        /// <param name="horaCordinacion"></param>
        /// <param name="intsumcodi"></param>
        /// <returns></returns>
        private bool PresentaDemoraSegunTablaNormalizacionSubEstacion(List<AfHoraCoordDTO> tabHoraCoord, int emprcodi, DateTime fechaFin, out DateTime horaCordinacion, int intsumcodi)
        {
            horaCordinacion = new DateTime();
            bool tieneDemora = false;

            //Obtenemos Tabla Normalizacion Coordinacion
            if (tabHoraCoord != null) // solo reporte TotalDatos
            {
                if (tabHoraCoord.Any())
                {
                    var idEmpresaInterrupcion = emprcodi;

                    var regHoraCord = tabHoraCoord.Find(x => x.Emprcodi == idEmpresaInterrupcion && x.Intsumcodi == intsumcodi);

                    if (regHoraCord != null && regHoraCord.Afhofecha != null)
                    {
                        var horaCord = regHoraCord.Afhofecha.Value;
                        var horaAdicionada = horaCord.AddMinutes(15);

                        int val = fechaFin.CompareTo(horaAdicionada);

                        if (val > 0)
                        {
                            tieneDemora = true;
                            horaCordinacion = horaAdicionada;
                        }
                    }
                }
            }

            return tieneDemora;
        }
        /// <summary>
        /// Determina si un registro de interrucion presenta demora segun la tabla de Coordinacion y Normalizacion
        /// </summary>
        /// <param name="tabHoraCoord"></param>
        /// <param name="reg"></param>
        /// <returns></returns>
        private bool PresentaDemoraSegunTablaNormalizacionRestablecimiento(List<AfHoraCoordDTO> tabHoraCoord, int emprcodi, DateTime fechaFin, out DateTime horaCordinacion, int afecodi, int intsumcodi)
        {
            horaCordinacion = new DateTime();
            bool tieneDemora = false;

            //Obtenemos Tabla Normalizacion Coordinacion
            if (tabHoraCoord != null) // solo reporte TotalDatos
            {
                if (tabHoraCoord.Any())
                {
                    var idEmpresaInterrupcion = emprcodi;

                    var regHoraCord = tabHoraCoord.Find(x => x.Emprcodi == idEmpresaInterrupcion && x.Afecodi == afecodi && x.Intsumcodi == intsumcodi);

                    if (regHoraCord != null && regHoraCord.Afhofecha != null)
                    {
                        var horaCord = regHoraCord.Afhofecha.Value;
                        var horaAdicionada = horaCord.AddMinutes(15);

                        int val = fechaFin.CompareTo(horaAdicionada);

                        if (val > 0)
                        {
                            tieneDemora = true;
                            horaCordinacion = horaAdicionada;
                        }
                    }
                }
            }

            return tieneDemora;
        }
        /// <summary>
        /// Devuelve una listadoReporte que es usado tanto por DatosTotales MalasActuaciones y Menores3minutos
        /// </summary>
        /// <param name="listaData"></param>
        /// <returns></returns>
        private List<ReporteInterrupcion> lstDatosTotales_MalasActuaciones_Menores3minutos(List<AfInterrupSuministroDTO> listaData)
        {
            List<ReporteInterrupcion> listaReporte = new List<ReporteInterrupcion>();

            listaData = listaData.Distinct().ToList();

            var codigosEventos = listaData.Select(y => new { y.EVENCODI, y.EVENINI }).Distinct().ToList();
            var listaDataCompleta = listaData;
            //Lista Data
            List<AfInterrupSuministroDTO> registros = new List<AfInterrupSuministroDTO>();
            foreach (var item in codigosEventos)
            {
                registros = listaDataCompleta.Where(x => x.EVENCODI == item.EVENCODI).OrderByDescending(c => c.EVENCODI).ToList();
                foreach (var regData in registros)
                {
                    var reg = new ReporteInterrupcion();
                    reg.Zona = regData.Intsumzona;
                    reg.Suministro = regData.CodigoOsinergmin + "-" + regData.Intsumsubestacion + "(" + regData.Intsumsuministro + ")";
                    reg.Subestacion = regData.Intsumsubestacion;
                    reg.Potencia = regData.Intsummw.Value;
                    reg.HoraInicio = regData.Intsumfechainterrini2;
                    reg.HoraFinal = regData.Intsumfechainterrfin2;
                    reg.Duracion = regData.Intsumduracion.Value;
                    reg.Funcion = regData.Intsumfuncion;
                    reg.Etapa = regData.Intsumnumetapa.ToString();
                    reg.Evencodi = regData.EVENCODI.ToString();
                    reg.Evenini = regData.EVENINI.ToString("dd/MM/yyyy HH:mm:ss");

                    reg.FechaInicio = regData.Intsumfechainterrini.Value;
                    reg.FechaFin = regData.Intsumfechainterrfin.Value;
                    reg.IdEmpresa = regData.Emprcodi;
                    //
                    reg.NombEmpresa = regData.Emprnomb;
                    reg.Afecodi = regData.Afecodi;
                    reg.Intsumcodi = regData.Intsumcodi;
                    //
                    listaReporte.Add(reg);
                }

                listaReporte = listaReporte.OrderBy(x => x.Suministro).ThenBy(x => x.Subestacion).ThenBy(x => x.FechaInicio).ToList();

                //Totales
                ReporteInterrupcion regTotal = new ReporteInterrupcion();
                regTotal.TipoReporte = ConstantesExtranetCTAF.TipoReporteTotal;
                regTotal.Potencia = registros.Sum(x => x.Intsummw.Value);
                regTotal.Evencodi = item.EVENCODI.ToString();
                regTotal.Evenini = item.EVENINI.ToString();
                listaReporte.Add(regTotal);
            }
            if (codigosEventos.Count == 0)
            {

                //Totales
                ReporteInterrupcion regTotal = new ReporteInterrupcion();
                regTotal.TipoReporte = ConstantesExtranetCTAF.TipoReporteTotal;
                regTotal.Potencia = listaReporte.Sum(x => x.Potencia);
                listaReporte.Add(regTotal);
            }
            return listaReporte;
        }

        /// <summary>
        /// Obtiene listas para armar el reporte DatosTotales MalasActuaciones y Menores3minutos
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="tabHoraCoord"></param>
        /// <param name="esPorEracmf"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDayaTablaDatosTotales_MalasActuaciones_Menores3minutos(List<ReporteInterrupcion> listaReporte, List<AfHoraCoordDTO> tabHoraCoord, bool esPorEracmf)
        {
            TablaReporte tabla = new TablaReporte();
            List<AfHoraCoordDTO> registrosHorasCoordinacion = new List<AfHoraCoordDTO>();

            //Caracteristicas generales del reporte
            tabla.TamLetra = 9;
            tabla.TipoFuente = "Calibri";
            tabla.Color = "#000000";
            //

            #region cabecera
            CabeceraReporte cabRepo = new CabeceraReporte();

            if (esPorEracmf)
            {
                string[,] matrizCabecera = new string[1, 9];
                matrizCabecera[0, 0] = "Zona";
                matrizCabecera[0, 1] = "Suministro";
                matrizCabecera[0, 2] = "S.E.";
                matrizCabecera[0, 3] = "Potencia (MW)";
                matrizCabecera[0, 4] = "Inicio";
                matrizCabecera[0, 5] = "Final";
                matrizCabecera[0, 6] = "Duración (min)";
                matrizCabecera[0, 7] = "Función";
                matrizCabecera[0, 8] = "Etapa";
                cabRepo.CabeceraData = matrizCabecera;
            }
            else
            {
                string[,] matrizCabecera = new string[1, 6];
                matrizCabecera[0, 0] = "Suministro";
                matrizCabecera[0, 1] = "S.E.";
                matrizCabecera[0, 2] = "Potencia (MW)";
                matrizCabecera[0, 3] = "Inicio";
                matrizCabecera[0, 4] = "Final";
                matrizCabecera[0, 5] = "Duración (min)";
                cabRepo.CabeceraData = matrizCabecera;
            }

            tabla.Cabecera = cabRepo;

            #endregion

            #region cuerpo

            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();
            ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal);

            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();
            foreach (var reg in listaData)
            {
                bool tieneDemora = false;
                if (tabHoraCoord != null)
                {
                    foreach (var hrcoord in tabHoraCoord)
                    {
                        if (hrcoord.Intsumcodi > 0)
                        {
                            registrosHorasCoordinacion = tabHoraCoord.Where(x => x.Afecodi == reg.Afecodi && x.Intsumcodi == reg.Intsumcodi).ToList();
                            if (registrosHorasCoordinacion.Count > 0)
                            {
                                tieneDemora = PresentaDemoraSegunTablaNormalizacion(registrosHorasCoordinacion, reg.IdEmpresa, reg.FechaFin, out DateTime hc);
                            }
                        }
                        else
                            tieneDemora = PresentaDemoraSegunTablaNormalizacion(tabHoraCoord, reg.IdEmpresa, reg.FechaFin, out DateTime hc);
                    }
                }

                //bool tieneDemora = PresentaDemoraSegunTablaNormalizacion(tabHoraCoord, reg.IdEmpresa, reg.FechaFin, out DateTime hc);

                RegistroReporte registro = new RegistroReporte();
                List<CeldaReporte> datos = new List<CeldaReporte>();
                registro.AltoFila = 0.5M;

                if (esPorEracmf)
                {
                    datos.Add(this.GetCeldaEspecialZona("Zona " + reg.Zona, false, false));
                    datos.Add(this.GetCeldaEspecialSuministro(reg.Suministro, false, false, false, true, reg.IdEmpresa));
                    datos.Add(this.GetCeldaEspecialSE(reg.Subestacion, false, false));
                    datos.Add(this.GetCeldaEspecialMW(reg.Potencia, false, false, false));
                    datos.Add(this.GetCeldaEspecialHora(reg.HoraInicio, false, false));
                    datos.Add(this.GetCeldaEspecialHora(reg.HoraFinal, false, false));
                    datos.Add(this.GetCeldaEspecialDuracion(reg.Duracion, false, false, tieneDemora));
                    datos.Add(this.GetCeldaEspecialFuncion((reg.Funcion != null ? (reg.Funcion.ToUpper() == "F" ? "f" : (reg.Funcion.ToUpper() == "DF" ? "Df" : reg.Funcion)) : ""), false, false));
                    datos.Add(this.GetCeldaEspecialEtapa("Etapa " + reg.Etapa, false, false));
                }
                else
                {
                    datos.Add(this.GetCeldaEspecialSuministro(reg.Suministro, false, false));
                    datos.Add(this.GetCeldaEspecialSE(reg.Subestacion, false, false));
                    datos.Add(this.GetCeldaEspecialMW(reg.Potencia, false, false, false));
                    datos.Add(this.GetCeldaEspecialHora(reg.HoraInicio, false, false));
                    datos.Add(this.GetCeldaEspecialHora(reg.HoraFinal, false, false));
                    datos.Add(this.GetCeldaEspecialDuracion(reg.Duracion, false, false, tieneDemora));
                }

                registro.ListaCelda = datos;
                registro.Emprcodi = reg.IdEmpresa;
                registros.Add(registro);
            }

            //Totales
            RegistroReporte registro0 = new RegistroReporte();
            registro0.AltoFila = 0.5M;
            registro0.EsFilaResumen = true;
            registro0.EsAgrupado = true;
            List<CeldaReporte> datos0 = new List<CeldaReporte>();

            if (esPorEracmf)
            {
                datos0.Add(this.GetCeldaEspecialZona(string.Empty, true, false, true));
                datos0.Add(this.GetCeldaEspecialSuministro(null, true, true, true, true));
                datos0.Add(this.GetCeldaEspecialTotal("TOTAL", true, false, true));

                datos0.Add(this.GetCeldaEspecialMW(regTotal != null ? regTotal.Potencia : 0, true, true, false));

                datos0.Add(this.GetCeldaEspecialHora(string.Empty, true, true));
                datos0.Add(this.GetCeldaEspecialHora(string.Empty, true, true));

                datos0.Add(this.GetCeldaEspecialDuracion(null, true, true, false));
                datos0.Add(this.GetCeldaEspecialFuncion(null, true, true));
                datos0.Add(this.GetCeldaEspecialEtapa(null, true, true));
            }
            else
            {
                datos0.Add(this.GetCeldaEspecialSuministro(null, true, true));
                datos0.Add(this.GetCeldaEspecialTotal("TOTAL", true, false));

                datos0.Add(this.GetCeldaEspecialMW(regTotal != null ? regTotal.Potencia : 0, true, true, false));

                datos0.Add(this.GetCeldaEspecialHora(string.Empty, true, true));
                datos0.Add(this.GetCeldaEspecialHora(string.Empty, true, true));

                datos0.Add(this.GetCeldaEspecialDuracion(null, true, true, false));
            }

            registro0.ListaCelda = datos0;

            registros.Add(registro0);
            tabla.ListaRegistros = registros;

            #endregion
            return tabla;
        }

        /// <summary>
        /// Devuelve Listado de mala Actuaciones
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="listaAllData"></param>
        /// <param name="esMalaActuacion"></param>
        /// <returns></returns>
        private List<AfInterrupSuministroDTO> ObtenerListadoConSinMalasActuaciones(int afecodi, List<AfInterrupSuministroDTO> listaAllData, bool esMalaActuacion)
        {
            //Lista de interrupciones con malas actuaciones
            List<AfInterrupSuministroDTO> listaConMalasActuaciones = new List<AfInterrupSuministroDTO>();
            List<AfInterrupSuministroDTO> listaSinMalasActuaciones = new List<AfInterrupSuministroDTO>();
            List<AfCondicionesDTO> listaFuncionesYEtapas = ListAfCondicioness(afecodi);

            //Si no hay data en la tabla Etapas Y Funciones, se consideran buenas actuaciones
            if (listaFuncionesYEtapas.Any())
            {
                foreach (var regInterrup in listaAllData)
                {
                    var funcion = regInterrup.Intsumfuncion;
                    var etapa = regInterrup.Intsumnumetapa;
                    var zona = regInterrup.Intsumzona;

                    var regExistente = listaFuncionesYEtapas.Find(x => (x.Afcondfuncion != null ? x.Afcondfuncion.ToUpper() : "") == funcion.ToUpper()
                    && (x.Afcondnumetapa != null ? x.Afcondnumetapa : 0) == etapa && (x.Afcondzona != null ? x.Afcondzona.ToUpper() : "") == zona.ToUpper());

                    //Si el registro EXISTE en la tabla Funciones y Etapas, es una BUENA actuacion
                    if (regExistente != null)
                    {
                        listaSinMalasActuaciones.Add(regInterrup);
                    }
                    else //Si el registro NO EXISTE en la tabla Funciones y Etapas, es una MALA actuacion
                    {
                        listaConMalasActuaciones.Add(regInterrup);
                    }
                }

                if (esMalaActuacion)
                    return listaConMalasActuaciones;
                else
                    return listaSinMalasActuaciones;
            }
            else
            {
                return listaConMalasActuaciones;
            }
        }

        /// <summary>
        /// Obtiene todas las interrupciones del evento y otra lista de validacion con los que no tienen configuracion de equivalencia
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="fdatcodi"></param>
        /// <param name="listaData"></param>
        /// <param name="listDataReportCero"></param>
        /// <param name="listaMsjValidacion"></param>
        /// <param name="listaEmprcodiReportaron"></param>
        public void ListarInterrupcionSuministrosGral(int afecodi, int emprcodi, int fdatcodi, out List<AfInterrupSuministroDTO> listaData, out List<AfInterrupSuministroDTO> listDataReportCero, out List<string> listaMsjValidacion, out List<int> listaEmprcodiReportaron, string anio = "", string correlativo = "")
        {

            this.ListarDataReporteExtranetCTAF(afecodi, emprcodi, fdatcodi, out listaData, out listDataReportCero, out listaMsjValidacion, out listaEmprcodiReportaron, anio, correlativo);
        }

        /// <summary>
        /// Html de reporte plantilla genérica
        /// </summary>
        /// <param name="tablaData"></param>
        /// <param name="numReporte"></param>
        /// <param name="tipoInstruccion"></param>
        /// <returns></returns>
        private string GenerarRptHtml(TablaReporte tablaData, int numReporte, int tipoInstruccion)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            StringBuilder strHtml = new StringBuilder();

            if (dataCab.Length > 0)
            {
                var numColumnas = dataCab.Length;
                string grupoColu = "";

                for (int i = 0; i < numColumnas; i++)
                {
                    grupoColu = grupoColu + "<th style=''>" + dataCab[0, i] + "</th>";
                }

                strHtml.AppendFormat(@"
                    <div class='' id='vista1tabla7' style=''>
                        <table id='tablaRpt{0}' class='pretty tabla-icono'>
                            <thead>
                                <tr>
                                    {1}
                                </tr>
                            </thead>
                    ", numReporte, grupoColu);
            }
            if (registros.Count > 0)
            {
                string strBody = string.Empty;
                string strFoot = string.Empty;
                //data
                foreach (var reg in registros)
                {
                    StringBuilder strHtmlFila = new StringBuilder();
                    strHtmlFila.Append("<tr>");

                    foreach (var celda in reg.ListaCelda)
                    {
                        string strValor = string.Empty;
                        string strStyle = string.Empty;
                        string strClass = string.Empty;

                        if (celda.TieneTextoIzquierdo) strStyle += "text-align: left; padding-left: 5px;";
                        if (celda.TieneTextoDerecho) strStyle += "text-align: right; padding-right: 5px;";
                        if (celda.TieneTextoCentrado) strStyle += "text-align: center;";
                        if (celda.TieneTextoNegrita) strStyle += "font-weight:bold;";
                        if (reg.EsFilaResumen) strStyle += "background: #2980B9; color: #ffffff; height: 20px;";
                        if (celda.TieneBorder) strStyle += "border-left: 1px solid #dddddd;";
                        if (celda.TieneColor) strStyle += "background: #72B7DE; color: #ffffff;";
                        if (celda.TieneTextoColor) strStyle += "color: #DF0000;";
                        if (celda.TieneColorERACMF && tipoInstruccion == 1) strStyle += "background: #FB7A57; color: #000000;";

                        if (celda.EsNumero)
                        {
                            if (celda.Valor != null)
                            {
                                int numDigitos = celda.DigitosParteDecimal;
                                if (celda.TieneFormatoNumeroEspecial)
                                {
                                    numDigitos = MathHelper.GetDecimalPlaces(celda.Valor.Value);
                                    numDigitos = (numDigitos > celda.DigitosParteDecimal) ? (numDigitos <= ConstantesExtranetCTAF.MaxNumDigitos ? numDigitos : ConstantesExtranetCTAF.MaxNumDigitos) : celda.DigitosParteDecimal;
                                }

                                if (celda.EsNumeroTruncado)
                                {
                                    celda.Valor = MathHelper.TruncateDecimal(celda.Valor.Value, numDigitos);
                                }
                                if (celda.EsNumeroRedondeado)
                                {
                                    celda.Valor = MathHelper.Round(celda.Valor.Value, numDigitos);
                                }

                                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                                nfi.NumberGroupSeparator = " ";
                                nfi.NumberDecimalDigits = numDigitos;
                                nfi.NumberDecimalSeparator = ",";

                                strValor = celda.Valor.Value.ToString("N", nfi);
                            }
                        }
                        else
                        {
                            strValor = celda.Texto;
                        }
                        if (!reg.EsFilaResumen)
                            strHtmlFila.AppendFormat("<td style='{1}' class='{2}'>{0}</td>", strValor, strStyle, strClass);
                        else
                            strHtmlFila.AppendFormat("<th style='{1}' class='{2}'>{0}</th>", strValor, strStyle, strClass);
                    }

                    strHtmlFila.Append("</tr>");

                    if (!reg.EsFilaResumen)
                        strBody += strHtmlFila.ToString();
                    else
                        strFoot += strHtmlFila.ToString();
                }
                strHtml.AppendFormat(@"<tbody>{0}</tbody>", strBody);
                strHtml.AppendFormat(@"<tfoot>{0}</tfoot>", strFoot);

                //Fila de totales
                strHtml.AppendFormat(@"
                </table>
            <div/>
            ");
            }
            return strHtml.ToString();
        }

        /// <summary>
        /// Html de reporte 7 de Interrupcion por Eracmf
        /// </summary>
        /// <param name="tablaData"></param>
        /// <param name="numReporte"></param>
        /// <returns></returns>
        private string GenerarRptHtmlResumen(TablaReporte tablaData, int numReporte)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            StringBuilder strHtml = new StringBuilder();

            strHtml.AppendFormat(@"
            <div class='' id='vista1tabla2' style=''>
                <table id='tablaRpt{0}' class='pretty tabla-icono'>
                    <thead>
                        <tr>
                            <th rowspan='3'  style=''>Zona</th>
                            <th rowspan='3' style=''>Empresa</th>
                            <th colspan='10' style=''>Etapa</th>
                            <th rowspan='3' style=''>Total Zona (MW)</th>
                        </tr>
                        <tr>
                            <th colspan='7' style=''>f</th>
                            <th colspan='3' style=''>Df</th>
                        </tr>
                        <tr>
                            <th style=''>1°</th>
                            <th style=''>2°</th>
                            <th style=''>3°</th>
                            <th style=''>4°</th>
                            <th style=''>5°</th>
                            <th style=''>6°</th>
                            <th style=''>7° <br/>(Reposición)</th>
                            <th style=''>1</th>
                            <th style=''>2</th>
                            <th style=''>3</th>
                        </tr>
                    </thead>
            ", numReporte);

            if (registros.Count > 0)
            {
                string strBody = string.Empty;
                string strFoot = string.Empty;
                //data
                foreach (var reg in registros)
                {
                    StringBuilder strHtmlFila = new StringBuilder();
                    strHtmlFila.Append("<tr>");

                    foreach (var celda in reg.ListaCelda)
                    {
                        string strValor = string.Empty;
                        string strStyle = string.Empty;
                        string strClass = string.Empty;

                        if (celda.TieneTextoIzquierdo) strStyle += "text-align: left; padding-left: 5px;";
                        if (celda.TieneTextoDerecho) strStyle += "text-align: right; padding-right: 5px;";
                        if (celda.TieneTextoCentrado) strStyle += "text-align: center;";
                        if (celda.TieneTextoNegrita) strStyle += "font-weight:bold;";
                        if (reg.EsFilaResumen) strStyle += "background: #2980B9; color: #ffffff; height: 20px;";
                        if (celda.TieneBorder) strStyle += "border-left: 1px solid #dddddd;";
                        if (celda.TieneColor) strStyle += "background: #72B7DE; color: #ffffff;";
                        if (celda.TieneTextoColor) strStyle += "color: #DF0000;";
                        if (celda.TieneColorERACMF) strStyle += "background: #FB7A57; color: #000000;";

                        if (celda.EsNumero)
                        {
                            if (celda.Valor != null)
                            {
                                int numDigitos = MathHelper.GetDecimalPlaces(celda.Valor.Value);
                                numDigitos = (numDigitos > celda.DigitosParteDecimal) ? (numDigitos <= 6 ? numDigitos : 6) : celda.DigitosParteDecimal;

                                if (celda.EsNumeroTruncado)
                                {
                                    celda.Valor = MathHelper.TruncateDecimal(celda.Valor.Value, numDigitos);
                                }
                                if (celda.EsNumeroRedondeado)
                                {
                                    celda.Valor = MathHelper.Round(celda.Valor.Value, numDigitos);
                                }

                                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                                nfi.NumberGroupSeparator = " ";
                                nfi.NumberDecimalDigits = numDigitos;
                                nfi.NumberDecimalSeparator = ",";

                                strValor = celda.Valor.Value.ToString("N", nfi);
                            }
                        }
                        else
                        {
                            strValor = celda.Texto;
                        }
                        if (!reg.EsFilaResumen)
                            strHtmlFila.AppendFormat("<td style='{1}' class='{2}'>{0}</td>", strValor, strStyle, strClass);
                        else
                            strHtmlFila.AppendFormat("<th style='{1}' class='{2}'>{0}</th>", strValor, strStyle, strClass);
                    }

                    strHtmlFila.Append("</tr>");

                    if (!reg.EsFilaResumen)
                        strBody += strHtmlFila.ToString();
                    else
                        strFoot += strHtmlFila.ToString();
                }
                strHtml.AppendFormat(@"<tbody>{0}</tbody>", strBody);
                strHtml.AppendFormat(@"<tfoot>{0}</tfoot>", strFoot);

                //Fila de totales
                strHtml.AppendFormat(@"
                </table>
            <div/>
            ");
            }
            return strHtml.ToString();
        }

        /// <summary>
        /// Html de reporte de interrupción para el Restablecimiento
        /// </summary>
        /// <param name="tablaData"></param>
        /// <param name="numReporte"></param>
        /// <returns></returns>
        private string GenerarRptHtmlRestablecimiento(TablaReporte tablaData, int numReporte)
        {
            var dataCab = tablaData.Cabecera.CabeceraData;
            var registros = tablaData.ListaRegistros;

            StringBuilder strHtml = new StringBuilder();


            strHtml.AppendFormat(@"
            <div class='' id='vista2tabla4' style=''>
                <table id='tablaRpt{0}' class='pretty tabla-icono'>
                    <thead>
                        <tr>
                            <th rowspan='2'  style=''>Suministro</th>
                            <th rowspan='2' style=''>Subestación</th>
                            <th style=''>Coordinado</th>
                            <th style=''>Ejecutado</th>
                            <th rowspan='2' style=''>Demora en el Restablecimiento (min)</th>
                        </tr>
                        <tr>
                            <th style=''>Inicio</th>
                            <th style=''>Final</th>
                        </tr>
                    </thead>
            ", numReporte);

            if (registros != null && registros.Count > 0)
            {
                string strBody = string.Empty;
                string strFoot = string.Empty;
                //data
                foreach (var reg in registros)
                {
                    StringBuilder strHtmlFila = new StringBuilder();
                    strHtmlFila.Append("<tr>");

                    foreach (var celda in reg.ListaCelda)
                    {
                        string strValor = string.Empty;
                        string strStyle = string.Empty;
                        string strClass = string.Empty;

                        if (celda.TieneTextoIzquierdo) strStyle += "text-align: left; padding-left: 5px;";
                        if (celda.TieneTextoDerecho) strStyle += "text-align: right; padding-right: 5px;";
                        if (celda.TieneTextoCentrado) strStyle += "text-align: center;";
                        if (celda.TieneTextoNegrita) strStyle += "font-weight:bold;";
                        if (reg.EsFilaResumen) strStyle += "background: #2980B9; color: #ffffff; height: 20px;";
                        if (celda.TieneBorder) strStyle += "border-left: 1px solid #dddddd;";
                        if (celda.TieneColor) strStyle += "background: #72B7DE; color: #ffffff;";
                        if (celda.TieneTextoColor) strStyle += "color: #DF0000;";

                        if (celda.EsNumero)
                        {
                            if (celda.Valor != null)
                            {
                                int numDigitos = celda.DigitosParteDecimal;
                                if (celda.TieneFormatoNumeroEspecial)
                                {
                                    numDigitos = MathHelper.GetDecimalPlaces(celda.Valor.Value);
                                    numDigitos = (numDigitos > celda.DigitosParteDecimal) ? (numDigitos <= 6 ? numDigitos : 6) : celda.DigitosParteDecimal;
                                }

                                if (celda.EsNumeroTruncado)
                                {
                                    celda.Valor = MathHelper.TruncateDecimal(celda.Valor.Value, numDigitos);
                                }
                                if (celda.EsNumeroRedondeado)
                                {
                                    celda.Valor = MathHelper.Round(celda.Valor.Value, numDigitos);
                                }

                                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                                nfi.NumberGroupSeparator = " ";
                                nfi.NumberDecimalDigits = numDigitos;
                                nfi.NumberDecimalSeparator = ",";

                                strValor = celda.Valor.Value.ToString("N", nfi);
                            }
                        }
                        else
                        {
                            strValor = celda.Texto;
                        }
                        if (!reg.EsFilaResumen)
                            strHtmlFila.AppendFormat("<td style='{1}' class='{2}'>{0}</td>", strValor, strStyle, strClass);
                        else
                            strHtmlFila.AppendFormat("<th style='{1}' class='{2}'>{0}</th>", strValor, strStyle, strClass);
                    }

                    strHtmlFila.Append("</tr>");

                    if (!reg.EsFilaResumen)
                        strBody += strHtmlFila.ToString();
                    else
                        strFoot += strHtmlFila.ToString();
                }
                strHtml.AppendFormat(@"<tbody>{0}</tbody>", strBody);
                strHtml.AppendFormat(@"<tfoot>{0}</tfoot>", strFoot);

                //Fila de totales
                strHtml.AppendFormat(@"
                </table>
            <div/>
            ");
            }
            return strHtml.ToString();
        }

        /// <summary>
        /// Generar html de mensaje de advertencia si no tienen abreviatura
        /// </summary>
        /// <param name="listaMsjValidacion"></param>
        /// <returns></returns>
        private string GenerarHtmlValidacion(List<string> listaMsjValidacion)
        {
            if (listaMsjValidacion.Any())
            {
                string htmlLista = string.Empty;
                foreach (var reg in listaMsjValidacion)
                {
                    htmlLista += "<li> " + reg + "</li>";
                }
                string html = string.Format("<div class='action-alert' style='margin: -10px 0px 10px;display: inline-block;'> <ul> {0} </ul> </div>", htmlLista);
                return html;
            }

            return string.Empty;
        }

        /// <summary>
        /// Obtener la data para los reportes intranet
        /// </summary>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="fdatcodi"></param>
        /// <param name="listaData"></param>
        /// <param name="listDataReportCero"></param>
        /// <param name="listaMsjValidacion"></param>
        /// <param name="listaEmprcodiReportaron"></param>
        /// <param name="anio"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public void ListarDataReporteExtranetCTAF(int afecodi, int emprcodi, int fdatcodi, out List<AfInterrupSuministroDTO> listaData, out List<AfInterrupSuministroDTO> listDataReportCero, out List<string> listaMsjValidacion, out List<int> listaEmprcodiReportaron, string anio = "", string correlativo = "")
        {
            List<EventoDTO> objEventos = new List<EventoDTO>();
            EventoDTO objEventoDTO = new EventoDTO();
            if (!string.IsNullOrWhiteSpace(anio) && !string.IsNullOrWhiteSpace(correlativo))
                objEventos = this.ObtenerInterrupcionCTAF(anio, correlativo);
            else
                objEventoDTO = this.ObtenerInterrupcionByAfecodi(afecodi);

            listDataReportCero = new List<AfInterrupSuministroDTO>();
            listaData = new List<AfInterrupSuministroDTO>();
            listaMsjValidacion = null;
            listaEmprcodiReportaron = null;
            listaMsjValidacion = new List<string>();

            if (objEventos.Count > 0)
            {
                foreach (var objEvento in objEventos)
                {
                    if (objEvento.Afefechainterr != null)
                    {
                        DateTime fechaIniValida1 = objEvento.Afefechainterr.Value;
                        DateTime fechaIniValida2 = fechaIniValida1.AddSeconds(ConstantesExtranetCTAF.NumeroMaxSegundosFechaIniExtranet);

                        List<EmpresaReporte> listaConfEmp = this.ObtenerListadoConfiguracionEmpresa(ConstantesAppServicio.ParametroDefecto);
                        List<AfInterrupSuministroDTO> lista;

                        lista = FactorySic.GetAfInterrupSuministroRepository().ListarReporteExtranetCTAF(objEvento.AFECODI, emprcodi, fdatcodi);
                        lista = lista.Where(x => x.Intsumfechainterrini >= fechaIniValida1).ToList();//La data debe ser mayor o igual a la fecha de interrupción definido por el evaluador
                        lista = lista.Where(x => x.Intsumfechainterrini <= fechaIniValida2).ToList();//La data debe ser menor o igual a la fecha de interrupción + n segundos definido por el aplicativo

                        listaEmprcodiReportaron = lista.Select(x => x.Emprcodi).Distinct().ToList();

                        lista = lista.Where(x => x.Intsummw >= 0).ToList(); //Para los reportes solo se considera las potencias mayor que cero

                        foreach (var reg in lista)
                        {
                            //Asignar codigo osinergmin
                            var regConfEmp = listaConfEmp.Find(x => x.Emprcodi == reg.Emprcodi);
                            if (regConfEmp != null)
                            {
                                reg.CodigoOsinergmin = regConfEmp.CodigoOsinergmin;

                                reg.CodigoOsinergmin = !string.IsNullOrEmpty(reg.CodigoOsinergmin) ? regConfEmp.CodigoOsinergmin : regConfEmp.EmpresaERACMF; //asignar el nombre de la empresa en el archivo ERACMF
                            }
                            else
                            {
                                reg.CodigoOsinergmin = reg.Emprnomb; //asignar el nombre de la empresa en BD COES
                                listaMsjValidacion.Add("La empresa " + reg.Emprnomb + " no tiene Configuración de equivalencia.");
                            }

                            //formatear datos
                            reg.Emprnomb = !string.IsNullOrEmpty(reg.Emprnomb) ? reg.Emprnomb.Trim() : string.Empty;
                            reg.CodigoOsinergmin = !string.IsNullOrEmpty(reg.CodigoOsinergmin) ? reg.CodigoOsinergmin.Trim() : string.Empty;
                            reg.Intsumfechainterrini2 = reg.Intsumfechainterrini.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                            reg.Intsumfechainterrfin2 = reg.Intsumfechainterrfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);

                            //redondeo solo en la extranet
                            reg.Intsumduracion = Convert.ToDecimal(reg.Intsumfechainterrfin.Value.Subtract(reg.Intsumfechainterrini.Value).TotalMinutes);
                            reg.Intsumduracion = MathHelper.Round(reg.Intsumduracion.Value, ConstantesExtranetCTAF.DigitosParteDecimalDuracion);

                            //dar formato y validar observaciones
                            reg.Intsumobs = !string.IsNullOrEmpty(reg.Intsumobs) ? reg.Intsumobs.Trim() : string.Empty;
                        }

                        listaMsjValidacion.AddRange(listaMsjValidacion.Distinct().OrderBy(x => x).ToList());


                        //listaData.AddRange(lista.Where(x => x.Intsummw > 0).ToList());
                        listaData.AddRange(lista);
                        List<int> listaEmprcodiMwMayorCero = listaData.Select(x => x.Emprcodi).Distinct().ToList();

                        //reportado cero MW de interrupción (cero en todos sus alimentadores)
                        listDataReportCero.AddRange(lista.Where(x => x.Intsummw == 0).ToList());
                    }

                }
            }
            else
            {
                if (objEventoDTO.Afefechainterr != null)
                {
                    DateTime fechaIniValida1 = objEventoDTO.Afefechainterr.Value;
                    DateTime fechaIniValida2 = fechaIniValida1.AddSeconds(ConstantesExtranetCTAF.NumeroMaxSegundosFechaIniExtranet);

                    List<EmpresaReporte> listaConfEmp = this.ObtenerListadoConfiguracionEmpresa(ConstantesAppServicio.ParametroDefecto);
                    List<AfInterrupSuministroDTO> lista;

                    lista = FactorySic.GetAfInterrupSuministroRepository().ListarReporteExtranetCTAF(afecodi, emprcodi, fdatcodi);
                    lista = lista.Where(x => x.Intsumfechainterrini >= fechaIniValida1).ToList();//La data debe ser mayor o igual a la fecha de interrupción definido por el evaluador
                    lista = lista.Where(x => x.Intsumfechainterrini <= fechaIniValida2).ToList();//La data debe ser menor o igual a la fecha de interrupción + n segundos definido por el aplicativo

                    listaEmprcodiReportaron = lista.Select(x => x.Emprcodi).Distinct().ToList();

                    lista = lista.Where(x => x.Intsummw >= 0).ToList(); //Para los reportes solo se considera las potencias mayor que cero

                    foreach (var reg in lista)
                    {
                        //Asignar codigo osinergmin
                        var regConfEmp = listaConfEmp.Find(x => x.Emprcodi == reg.Emprcodi);
                        if (regConfEmp != null)
                        {
                            reg.CodigoOsinergmin = regConfEmp.CodigoOsinergmin;

                            reg.CodigoOsinergmin = !string.IsNullOrEmpty(reg.CodigoOsinergmin) ? regConfEmp.CodigoOsinergmin : regConfEmp.EmpresaERACMF; //asignar el nombre de la empresa en el archivo ERACMF
                        }
                        else
                        {
                            reg.CodigoOsinergmin = reg.Emprnomb; //asignar el nombre de la empresa en BD COES
                            listaMsjValidacion.Add("La empresa " + reg.Emprnomb + " no tiene Configuración de equivalencia.");
                        }

                        //formatear datos
                        reg.Emprnomb = !string.IsNullOrEmpty(reg.Emprnomb) ? reg.Emprnomb.Trim() : string.Empty;
                        reg.CodigoOsinergmin = !string.IsNullOrEmpty(reg.CodigoOsinergmin) ? reg.CodigoOsinergmin.Trim() : string.Empty;
                        reg.Intsumfechainterrini2 = reg.Intsumfechainterrini.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                        reg.Intsumfechainterrfin2 = reg.Intsumfechainterrfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);

                        //redondeo solo en la extranet
                        reg.Intsumduracion = Convert.ToDecimal(reg.Intsumfechainterrfin.Value.Subtract(reg.Intsumfechainterrini.Value).TotalMinutes);
                        reg.Intsumduracion = MathHelper.Round(reg.Intsumduracion.Value, ConstantesExtranetCTAF.DigitosParteDecimalDuracion);

                        //dar formato y validar observaciones
                        reg.Intsumobs = !string.IsNullOrEmpty(reg.Intsumobs) ? reg.Intsumobs.Trim() : string.Empty;
                    }

                    listaMsjValidacion.AddRange(listaMsjValidacion.Distinct().OrderBy(x => x).ToList());


                    //listaData.AddRange(lista.Where(x => x.Intsummw > 0).ToList());
                    listaData.AddRange(lista);
                    List<int> listaEmprcodiMwMayorCero = listaData.Select(x => x.Emprcodi).Distinct().ToList();

                    //reportado cero MW de interrupción (cero en todos sus alimentadores)
                    listDataReportCero.AddRange(lista.Where(x => x.Intsummw == 0).ToList());
                }

            }
        }

        /// <summary>
        /// Agregar Hoja Excel Generico
        /// </summary>
        /// <param name="posHoja"></param>
        /// <param name="xlPackage"></param>
        /// <param name="ws"></param>
        /// <param name="tablaData"></param>
        /// <param name="regEvento"></param>
        /// <param name="objEmpresa"></param>
        /// <param name="objFuenteDatos"></param>
        /// <param name="nombreReporte"></param>
        /// <param name="ultimaFila"></param>
        /// <param name="reporte"></param>
        private void AgregarHojaExcelGenerico(int posHoja, ExcelPackage xlPackage, ref ExcelWorksheet ws, TablaReporte tablaData
            , EventoDTO regEvento, SiEmpresaDTO objEmpresa, SiFuentedatosDTO objFuenteDatos, string nombreReporte, out int ultimaFila, int reporte = 0, List<AfCondicionesDTO> listaFuncionesYEtapas = null)
        {
            //TOTAL DE DATOS
            ws = xlPackage.Workbook.Worksheets[posHoja];


            int filaIniCab = 3;
            int coluIniCab = 1;
            this.GenerarDetalleEventoHojaExcel(xlPackage, ws, regEvento, objEmpresa, objFuenteDatos, nombreReporte, filaIniCab, coluIniCab, tablaData.Color);

            filaIniCab = 12;
            coluIniCab = 2;
            if (posHoja == 2 && objFuenteDatos.Fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF)
                filaIniCab = 13;
            if (posHoja == 2 && objFuenteDatos.Fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion)
                filaIniCab = 12;

            ultimaFila = 0;

            if (((posHoja == 1 || posHoja == 2 || posHoja == 3 || posHoja == 4 || posHoja == 5) && objFuenteDatos.Fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion) ||
                 ((posHoja == 1 || posHoja == 2 || posHoja == 3 || posHoja == 5 || posHoja == 6 || posHoja == 7 || posHoja == 9 || posHoja == 4 || posHoja == 8) && objFuenteDatos.Fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF) ||
                 ((posHoja == 1) && objFuenteDatos.Fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.ReduccionSuministros))
            {
                List<RegistroReporte> registros = new List<RegistroReporte>();
                List<RegistroReporte> registrosTotales = new List<RegistroReporte>();
                TablaReporte Tabla = new TablaReporte();

                List<MeEnvioDTO> listaUltimosEnvios = new List<MeEnvioDTO>();
                var codigosEventos = tablaData.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                registrosTotales = tablaData.ListaRegistros;
                foreach (var item in codigosEventos)
                {
                    if (item.codigo != null)
                    {
                        registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();
                        Tabla = tablaData;
                        Tabla.ListaRegistros = registros;
                        this.GenerarRptHojaExcel(xlPackage, ws, regEvento, Tabla, filaIniCab, coluIniCab, out ultimaFila, reporte);
                        filaIniCab = ultimaFila;
                    }
                    if (posHoja == 3 && objFuenteDatos.Fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF)
                    {
                        this.GenerarDetalleFuncionesYEtapas(ref ws, listaFuncionesYEtapas.Where(x => x.Evencodi == item.codigo).ToList(), filaIniCab, coluIniCab, out ultimaFila);
                    }
                    filaIniCab = ultimaFila;


                }


            }
            else
            {
                this.GenerarRptHojaExcel(xlPackage, ws, regEvento, tablaData, filaIniCab, coluIniCab, out ultimaFila, reporte);
            }







        }

        /// <summary>
        /// Generar Detalle Evento Hoja Excel
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="ws"></param>
        /// <param name="regEvento"></param>
        /// <param name="objEmpresa"></param>
        /// <param name="objFuenteDatos"></param>
        /// <param name="nombreReporte"></param>
        /// <param name="filaIniCab"></param>
        /// <param name="coluIniCab"></param>
        /// <param name="colorFondo"></param>
        private void GenerarDetalleEventoHojaExcel(ExcelPackage xlPackage, ExcelWorksheet ws, EventoDTO regEvento, SiEmpresaDTO objEmpresa, SiFuentedatosDTO objFuenteDatos, string nombreReporte, int filaIniCab, int coluIniCab, string colorFondo)
        {
            //Filtros
            int rowIniFiltro = filaIniCab;
            int colIniFiltro = coluIniCab;
            ws.Cells[rowIniFiltro, colIniFiltro + 1].Value = objFuenteDatos != null ? objFuenteDatos.Fdatnombre + (nombreReporte.Length > 0 ? " - " + nombreReporte : string.Empty) : string.Empty;
            ws.Cells[rowIniFiltro + 1, colIniFiltro + 1].Value = objEmpresa != null && objEmpresa.Emprnomb != null ? objEmpresa.Emprnomb.Trim() : string.Empty;

            //Datos del evento
            int eveRowIni = rowIniFiltro + 2;
            int eveColumnValue = colIniFiltro + 1;

            ws.Cells[eveRowIni, eveColumnValue].Value = regEvento.CODIGO;
            ws.Cells[eveRowIni + 1, eveColumnValue].Value = regEvento.EVENINI?.ToString(ConstantesBase.FormatoFechaFullBase);
            ws.Cells[eveRowIni + 2, eveColumnValue].Value = regEvento.EVENFIN?.ToString(ConstantesBase.FormatoFechaFullBase);
            ws.Cells[eveRowIni + 3, eveColumnValue].Value = regEvento.EVENASUNTO;
            ws.Cells[eveRowIni + 4, eveColumnValue].Value = regEvento.EVENDESC;

            ws.Column(colIniFiltro).Width = 17;


            ws.Row(eveRowIni + 4).Height = 80;

            UtilExcel.CeldasExcelAlinearVerticalmente(ws, eveRowIni, coluIniCab, filaIniCab + 4, coluIniCab, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, eveRowIni, coluIniCab, filaIniCab + 4, coluIniCab, "Izquierda");
        }

        /// <summary>
        /// Generar Detalle Hora Normalizacion
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lstHandsonHorasCoordinacion"></param>
        /// <param name="filaIniCab"></param>
        /// <param name="coluIniCab"></param>
        private void GenerarDetalleHoraNormalizacion(ref ExcelWorksheet ws, List<AfHoraCoordDTO> lstHandsonHorasCoordinacion, int filaIniCab, int coluIniCab)
        {
            List<AfHoraCoordDTO> listaValida = lstHandsonHorasCoordinacion.Where(x => !string.IsNullOrEmpty(x.Afhofechadescripcion)).ToList();

            if (listaValida.Any())
            {
                //cabecera
                ws.Cells[filaIniCab, coluIniCab].Value = "Empresa";
                ws.Cells[filaIniCab, coluIniCab + 1].Value = "Fecha y hora de Coordinación de Normalización";
                ws.Row(filaIniCab).Height = 40;
                UtilExcel.BorderCeldas3(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 1);
                UtilExcel.CeldasExcelWrapText(ws, filaIniCab, coluIniCab + 1);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 1, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 1, "Centro");
                UtilExcel.CeldasExcelColorTexto(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 1, "#000000");
                UtilExcel.CeldasExcelColorFondo(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 1, "#bfbfbf");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 1, "Calibri", 9);
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 1);

                int filaX = 1;
                foreach (var reg in listaValida)
                {
                    int filaData = filaIniCab + filaX;
                    int colDataIni = coluIniCab;
                    int colDataFin = coluIniCab + 1;
                    ws.Cells[filaData, colDataIni].Value = reg.Codigoosinergmin;
                    ws.Cells[filaData, colDataFin].Value = reg.Afhofechadescripcion;

                    UtilExcel.BorderCeldas3(ws, filaData, colDataIni, filaData, colDataFin);
                    UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaData, colDataIni, filaData, colDataFin, "Centro");
                    UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaData, colDataIni, filaData, colDataFin, "Centro");
                    UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaData, colDataIni, filaData, colDataFin, "Calibri", 9);

                    filaX++;
                }
            }
        }

        /// <summary>
        /// Generar Detalle Hora Normalizacion
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lstHandsonHorasCoordinacion"></param>
        /// <param name="filaIniCab"></param>
        /// <param name="coluIniCab"></param>
        private void GenerarDetalleHoraNormalizacionNoERACMF(ref ExcelWorksheet ws, List<AfHoraCoordDTO> lstHandsonHorasCoordinacion, int filaIniCab, int coluIniCab)
        {
            List<AfHoraCoordDTO> listaValida = lstHandsonHorasCoordinacion.Where(x => !string.IsNullOrEmpty(x.Afhofechadescripcion)).ToList();

            if (listaValida.Any())
            {
                //cabecera
                ws.Cells[filaIniCab, coluIniCab].Value = "Empresa";
                ws.Cells[filaIniCab, coluIniCab + 1].Value = "Nombre de Empresa";
                ws.Cells[filaIniCab, coluIniCab + 2].Value = "S.E";
                ws.Cells[filaIniCab, coluIniCab + 3].Value = "Fecha y hora de Coordinación de Normalización";
                ws.Row(filaIniCab).Height = 40;
                UtilExcel.BorderCeldas3(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 3);
                UtilExcel.CeldasExcelWrapText(ws, filaIniCab, coluIniCab + 3);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 3, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 3, "Centro");
                UtilExcel.CeldasExcelColorTexto(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 3, "#000000");
                UtilExcel.CeldasExcelColorFondo(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 3, "#bfbfbf");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 3, "Calibri", 9);
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 3);

                int filaX = 1;
                foreach (var reg in listaValida)
                {
                    int filaData = filaIniCab + filaX;
                    int colDataIni = coluIniCab;

                    ws.Cells[filaData, colDataIni].Value = reg.Codigoosinergmin;
                    ws.Cells[filaData, colDataIni + 1].Value = reg.Emprnombr;
                    ws.Cells[filaData, colDataIni + 2].Value = reg.Intsumsubestacion;
                    ws.Cells[filaData, colDataIni + 3].Value = reg.Afhofechadescripcion;

                    UtilExcel.BorderCeldas3(ws, filaData, colDataIni, filaData, colDataIni + 3);
                    UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaData, colDataIni, filaData, colDataIni + 3, "Centro");
                    UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaData, colDataIni, filaData, colDataIni + 3, "Centro");
                    UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaData, colDataIni, filaData, colDataIni + 3, "Calibri", 9);

                    filaX++;
                }
            }
        }

        /// <summary>
        /// Generar Detalle Funciones Y Etapas
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="listaFuncionesYEtapas"></param>
        /// <param name="filaIniCab"></param>
        /// <param name="coluIniCab"></param>
        private void GenerarDetalleFuncionesYEtapas(ref ExcelWorksheet ws, List<AfCondicionesDTO> listaFuncionesYEtapas, int filaIniCabReal, int coluIniCab, out int filaUltima)
        {
            int filaIniCab = filaIniCabReal - 2;
            filaUltima = filaIniCab;
            if (listaFuncionesYEtapas.Any())
            {
                //cabecera
                ws.Cells[filaIniCab, coluIniCab].Value = "Función";
                ws.Cells[filaIniCab, coluIniCab + 1].Value = "Etapa";
                ws.Cells[filaIniCab, coluIniCab + 2].Value = "Zona";
                UtilExcel.BorderCeldas3(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 2);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 2, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 2, "Centro");
                UtilExcel.CeldasExcelColorTexto(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 2, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 2, "#bfbfbf");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 2, "Calibri", 11);
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + 2);

                int filaX = 1;
                listaFuncionesYEtapas = listaFuncionesYEtapas
                    .OrderBy(x => x.Afcondfuncion).ThenBy(x => x.Afcondzona).ThenBy(x => x.Afcondnumetapadescrip).ToList();
                foreach (var reg in listaFuncionesYEtapas)
                {
                    int filaData = filaIniCab + filaX;
                    int colDataIni = coluIniCab;
                    int colDataFin = coluIniCab + 2;

                    ws.Cells[filaData, colDataIni].Value = reg.Afcondfuncion;
                    ws.Cells[filaData, colDataIni + 1].Value = reg.Afcondnumetapadescrip;
                    ws.Cells[filaData, colDataFin].Value = reg.Afcondzona;

                    UtilExcel.BorderCeldas3(ws, filaData, colDataIni, filaData, colDataFin);
                    UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaData, colDataIni, filaData, colDataFin, "Centro");
                    UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaData, colDataIni, filaData, colDataFin, "Centro");

                    filaX++;
                }
                filaUltima = filaUltima + filaX + 3;

            }

        }

        /// <summary>
        /// Generar Rpt Hoja Excel
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="ws"></param>
        /// <param name="regEvento"></param>
        /// <param name="tablaData"></param>
        /// <param name="filaIniCab"></param>
        /// <param name="coluIniCab"></param>
        /// <param name="ultimaFila"></param>
        /// <param name="reporte"></param>
        public void GenerarRptHojaExcel(ExcelPackage xlPackage, ExcelWorksheet ws, EventoDTO regEvento, TablaReporte tablaData, int filaIniCab, int coluIniCab, out int ultimaFila, int reporte)
        {
            string titulo = "";


            //var dataCab = tablaData.Cabecera.CabeceraData;
            var dataCab = tablaData.CabeceraColumnas;
            var registros = tablaData.ListaRegistros;


            if (tablaData.ListaRegistros.Count > 0)
            {
                titulo = tablaData.ListaRegistros[0].Nombre;
                ws.Cells[filaIniCab, coluIniCab + 0].Value = titulo;
                UtilExcel.CeldasExcelNoWrapText(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab);
            }
            if (tablaData.ListaRegistros.Count > 0 && reporte == 2)
            {
                titulo = tablaData.ListaRegistros[0].Nombre;
                ws.Cells[filaIniCab - 2, coluIniCab + 0].Value = titulo;
                UtilExcel.CeldasExcelNoWrapText(ws, filaIniCab - 2, coluIniCab, filaIniCab - 2, coluIniCab);
            }
            filaIniCab = filaIniCab + 2;
            int filaIniData = filaIniCab + 1;
            int coluIniData = coluIniCab;



            ultimaFila = filaIniCab + 3;

            //>>>>>>>>>>>>>>>>>>> Agrupación Malas actuaciones
            var empresa = reporte == 3 ? (registros.Count > 0 ? registros.First().ListaCelda[1].Texto : "") : string.Empty;
            var filaXAgrupa = filaIniData;
            //>>>>>>>>>>>>>>>>>>>

            if (dataCab.Count > 0 && reporte != 2 && reporte != 13)
            {
                var numColumnas = tablaData.CabeceraColumnas.Count;

                int numCol = 0;
                foreach (var item in tablaData.CabeceraColumnas)
                {
                    decimal anchoColumna;
                    ws.Cells[filaIniCab, coluIniCab + numCol].Value = item.NombreColumna;

                    if (reporte != 9)
                        UtilExcel.BorderCeldas3(ws, filaIniCab, coluIniCab + numCol, filaIniCab, coluIniCab + numCol);
                    else
                    {
                        if (numCol != 7)
                            UtilExcel.BorderCeldas3(ws, filaIniCab, coluIniCab + numCol, filaIniCab, coluIniCab + numCol);
                    }

                    double ancho = Convert.ToDouble(item.AnchoColumna) * Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumExcel);
                    anchoColumna = Convert.ToDecimal(ancho);

                    UtilExcel.AnchoColumna(ws, coluIniCab + numCol, anchoColumna);
                    numCol++;
                }

                numColumnas = reporte != 9 ? numColumnas : numColumnas - 1;

                if (reporte == 6)
                    UtilExcel.CeldasExcelAgrupar(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + numCol - 1);

                UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + numColumnas - 1, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + numColumnas - 1, "Centro");
                UtilExcel.CeldasExcelColorTexto(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + numColumnas - 1, "#000000");
                UtilExcel.CeldasExcelColorFondo(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + numColumnas - 1, tablaData.Color);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + numColumnas - 1, tablaData.TipoFuente, tablaData.TamLetra);
                UtilExcel.CeldasExcelEnNegrita(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + numColumnas - 1);
                UtilExcel.CeldasExcelWrapText(ws, filaIniCab, coluIniCab, filaIniCab, coluIniCab + numColumnas - 1);

                #region Caso Especial - Resarcimiento

                if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt09Resarcimiento || tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.InterrupcionRpt09Resarcimiento)
                {
                    int numCol2 = 0;
                    foreach (var item in tablaData.CabeceraColumnas)
                    {


                        int filActual = filaIniCab;
                        int colActual = coluIniCab + numCol2;
                        switch (numCol2)
                        {
                            case 1:
                                ws.Cells[filActual, colActual].IsRichText = true;
                                OfficeOpenXml.Style.ExcelRichTextCollection rtfCollection = ws.Cells[filActual, colActual].RichText;
                                OfficeOpenXml.Style.ExcelRichText ert = rtfCollection.Add("POTENCIA INTERRUMPIDA (MW) ");
                                ert.Bold = true;

                                ert = rtfCollection.Add("\n(A)");
                                ert.Color = Color.Red;
                                ert.Bold = true;

                                ws.Cells[filActual, colActual].Style.Font.SetFromFont(new Font(tablaData.TipoFuente, tablaData.TamLetra));

                                break;
                            case 5:
                                ws.Cells[filActual, colActual].IsRichText = true;
                                OfficeOpenXml.Style.ExcelRichTextCollection rtfCollection2 = ws.Cells[filActual, colActual].RichText;
                                OfficeOpenXml.Style.ExcelRichText ert2 = rtfCollection2.Add("TIEMPO DURACIÓN (HORAS) ");
                                ert2.Bold = true;

                                ert2 = rtfCollection2.Add("\n(B)");
                                ert2.Color = Color.Red;
                                ert2.Bold = true;

                                ws.Cells[filActual, colActual].Style.Font.SetFromFont(new Font(tablaData.TipoFuente, tablaData.TamLetra));


                                break;
                            case 6:
                                ws.Cells[filActual, colActual].IsRichText = true;
                                OfficeOpenXml.Style.ExcelRichTextCollection rtfCollection3 = ws.Cells[filActual, colActual].RichText;
                                OfficeOpenXml.Style.ExcelRichText ert3 = rtfCollection3.Add("ENERGÍA NO SUMINISTRADA (MWH) ");
                                ert3.Bold = true;

                                ert3 = rtfCollection3.Add("\n(AXB)");
                                ert3.Color = Color.Red;
                                ert3.Bold = true;

                                ws.Cells[filActual, colActual].Style.Font.SetFromFont(new Font(tablaData.TipoFuente, tablaData.TamLetra));

                                break;
                            default:

                                break;
                        }
                        numCol2++;
                    }
                }

                #endregion

                ws.Row(filaIniCab).Height = Convert.ToDouble(tablaData.AltoFilaCab) * Convert.ToDouble(ConstantesExtranetCTAF.FactorAltoFilaExcel);
            }

            //>>>>>>>>>>>> Cabecera Reporte Resumen >>>>>>>>>>>>>>>>>>>>>>
            if (dataCab.Count > 0 && reporte == 2)
            {
                var numColumnas = tablaData.CabeceraColumnas.Count;

                int numCol = 0;
                foreach (var item in tablaData.CabeceraColumnas)
                {
                    decimal anchoColumna;
                    UtilExcel.BorderCeldas3(ws, filaIniCab, coluIniCab + numCol, filaIniCab, coluIniCab + numCol);
                    double ancho = Convert.ToDouble(item.AnchoColumna) * Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumExcel);
                    anchoColumna = Convert.ToDecimal(ancho);
                    UtilExcel.AnchoColumna(ws, coluIniCab + numCol, anchoColumna);
                    numCol++;
                }
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Primera fila cabecera
                int filActual = filaIniCab - 2;
                var alturaFila = Convert.ToDouble(0.51M) * Convert.ToDouble(ConstantesExtranetCTAF.FactorAltoFilaExcel);
                ws.Row(filActual).Height = alturaFila;

                int rowIniZona = filaIniCab - 2;
                int colIniZona = coluIniCab;
                int colIniEmpresa = colIniZona + 1;
                int colIniEtapa = colIniEmpresa + 1;
                int colIniTotalZona = colIniEtapa + 10;

                ws.Cells[rowIniZona, colIniZona].Value = "ZONA";
                ws.Cells[rowIniZona, colIniEmpresa].Value = "EMPRESA";
                ws.Cells[rowIniZona, colIniEtapa].Value = "ETAPA";
                ws.Cells[rowIniZona, colIniTotalZona].Value = "TOTAL ZONA (MW)";

                UtilExcel.CeldasExcelAgrupar(ws, rowIniZona, colIniZona, rowIniZona + 2, colIniZona);
                UtilExcel.CeldasExcelAgrupar(ws, rowIniZona, colIniEmpresa, rowIniZona + 2, colIniEmpresa);
                UtilExcel.CeldasExcelAgrupar(ws, rowIniZona, colIniEtapa, rowIniZona, colIniEtapa + 9);
                UtilExcel.CeldasExcelAgrupar(ws, rowIniZona, colIniTotalZona, rowIniZona + 2, colIniTotalZona);

                UtilExcel.BorderCeldas3(ws, rowIniZona, colIniZona, rowIniZona + 2, colIniZona);
                UtilExcel.BorderCeldas3(ws, rowIniZona, colIniEmpresa, rowIniZona + 2, colIniEmpresa);
                UtilExcel.BorderCeldas3(ws, rowIniZona, colIniEtapa, rowIniZona, colIniEtapa + 9);
                UtilExcel.BorderCeldas3(ws, rowIniZona, colIniTotalZona, rowIniZona + 2, colIniTotalZona);

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Segunda fila cabecera
                filActual = filActual + 1;
                alturaFila = Convert.ToDouble(0.51M) * Convert.ToDouble(ConstantesExtranetCTAF.FactorAltoFilaExcel);
                ws.Row(filActual).Height = alturaFila;

                int rowInif = rowIniZona + 1;
                int colInif = coluIniCab + 2;
                int colIniDf = colInif + 7;

                ws.Cells[rowInif, colInif].Value = "f";
                ws.Cells[rowInif, colIniDf].Value = "Df";

                UtilExcel.CeldasExcelAgrupar(ws, rowInif, colInif, rowInif, colInif + 6);
                UtilExcel.CeldasExcelAgrupar(ws, rowInif, colIniDf, rowInif, colIniDf + 2);

                UtilExcel.BorderCeldas3(ws, rowInif, colInif, rowInif, colInif + 6);
                UtilExcel.BorderCeldas3(ws, rowInif, colIniDf, rowInif, colIniDf + 2);

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>> tercera fila cabecera
                filActual = filActual + 1;
                alturaFila = Convert.ToDouble(0.85M) * Convert.ToDouble(ConstantesExtranetCTAF.FactorAltoFilaExcel);
                ws.Row(filActual).Height = alturaFila;

                int rowIni1 = rowInif + 1;
                int colIni1 = coluIniCab + 2;
                int colIni2 = colIni1 + 1;
                int colIni3 = colIni2 + 1;
                int colIni4 = colIni3 + 1;
                int colIni5 = colIni4 + 1;
                int colIni6 = colIni5 + 1;
                int colIni7 = colIni6 + 1;
                int colIni8 = colIni7 + 1;
                int colIni9 = colIni8 + 1;
                int colIni10 = colIni9 + 1;

                ws.Cells[rowIni1, colIni1].Value = "1°";
                ws.Cells[rowIni1, colIni2].Value = "2°";
                ws.Cells[rowIni1, colIni3].Value = "3°";
                ws.Cells[rowIni1, colIni4].Value = "4°";
                ws.Cells[rowIni1, colIni5].Value = "5°";
                ws.Cells[rowIni1, colIni6].Value = "6°";
                ws.Cells[rowIni1, colIni7].Value = "7° (Reposición)";
                ws.Cells[rowIni1, colIni8].Value = "1°";
                ws.Cells[rowIni1, colIni9].Value = "2°";
                ws.Cells[rowIni1, colIni10].Value = "3°";

                UtilExcel.BorderCeldas3(ws, rowIni1, colIni1, rowIni1, colIni1);
                UtilExcel.BorderCeldas3(ws, rowIni1, colIni2, rowIni1, colIni2);
                UtilExcel.BorderCeldas3(ws, rowIni1, colIni3, rowIni1, colIni3);
                UtilExcel.BorderCeldas3(ws, rowIni1, colIni4, rowIni1, colIni4);
                UtilExcel.BorderCeldas3(ws, rowIni1, colIni5, rowIni1, colIni5);
                UtilExcel.BorderCeldas3(ws, rowIni1, colIni6, rowIni1, colIni6);
                UtilExcel.BorderCeldas3(ws, rowIni1, colIni7, rowIni1, colIni7);
                UtilExcel.BorderCeldas3(ws, rowIni1, colIni8, rowIni1, colIni8);
                UtilExcel.BorderCeldas3(ws, rowIni1, colIni9, rowIni1, colIni9);
                UtilExcel.BorderCeldas3(ws, rowIni1, colIni10, rowIni1, colIni10);

                //Formato
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniZona, coluIniCab, filActual, colIniTotalZona, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniZona, coluIniCab, filActual, colIniTotalZona, "Centro");
                UtilExcel.CeldasExcelColorTexto(ws, rowIniZona, coluIniCab, filActual, colIniTotalZona, "#000000");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniZona, coluIniCab, filActual, colIniTotalZona, tablaData.Color);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniZona, coluIniCab, filActual, colIniTotalZona, tablaData.TipoFuente, tablaData.TamLetra);
                UtilExcel.CeldasExcelEnNegrita(ws, rowIniZona, coluIniCab, filActual, colIniTotalZona);
                UtilExcel.CeldasExcelWrapText(ws, rowIniZona, coluIniCab, filActual, colIniTotalZona);
            }

            //>>>>>>>>>>>> Cabecera Reporte Restablecimiento >>>>>>>>>>>>>>>>>>>>>>
            if (dataCab.Count > 0 && reporte == 13)
            {
                int numCol = 0;
                foreach (var item in tablaData.CabeceraColumnas)
                {
                    decimal anchoColumna;
                    ws.Cells[filaIniCab, coluIniCab + numCol].Value = item.NombreColumna;
                    UtilExcel.BorderCeldas3(ws, filaIniCab, coluIniCab + numCol, filaIniCab, coluIniCab + numCol);
                    double ancho = Convert.ToDouble(item.AnchoColumna) * Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumExcel);
                    //item.AnchoColumna = Convert.ToDecimal(ancho);
                    anchoColumna = Convert.ToDecimal(ancho);
                    UtilExcel.AnchoColumna(ws, coluIniCab + numCol, anchoColumna);
                    numCol++;
                }
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Primera fila cabecera
                int filActual = filaIniCab - 1;
                var alturaFila = Convert.ToDouble(0.5M) * Convert.ToDouble(ConstantesExtranetCTAF.FactorAltoFilaExcel);
                ws.Row(filActual).Height = alturaFila;

                int rowIniSuministro = filaIniCab - 1;
                int colIniSuministro = coluIniCab;
                int colIniSubestacion = colIniSuministro + 1;
                int colIniCoordinado = colIniSubestacion + 1;
                int colIniEjecutado = colIniCoordinado + 1;
                int colIniDuracion = colIniEjecutado + 1;

                ws.Cells[rowIniSuministro, colIniSuministro].Value = "SUMINISTRO";
                ws.Cells[rowIniSuministro, colIniSubestacion].Value = "SUBESTACIÓN";
                ws.Cells[rowIniSuministro, colIniCoordinado].Value = "COORDINADO";
                ws.Cells[rowIniSuministro, colIniEjecutado].Value = "EJECUTADO";
                ws.Cells[rowIniSuministro, colIniDuracion].Value = "DEMORA EN EL \n RESTABLECIMIENTO \n (MIN)";

                UtilExcel.CeldasExcelAgrupar(ws, rowIniSuministro, colIniSuministro, rowIniSuministro + 1, colIniSuministro);
                UtilExcel.CeldasExcelAgrupar(ws, rowIniSuministro, colIniSubestacion, rowIniSuministro + 1, colIniSubestacion);
                UtilExcel.CeldasExcelAgrupar(ws, rowIniSuministro, colIniDuracion, rowIniSuministro + 1, colIniDuracion);

                UtilExcel.BorderCeldas3(ws, rowIniSuministro, colIniSuministro, rowIniSuministro + 1, colIniSuministro);
                UtilExcel.BorderCeldas3(ws, rowIniSuministro, colIniSubestacion, rowIniSuministro + 1, colIniSubestacion);
                UtilExcel.BorderCeldas3(ws, rowIniSuministro, colIniCoordinado, rowIniSuministro, colIniCoordinado);
                UtilExcel.BorderCeldas3(ws, rowIniSuministro, colIniEjecutado, rowIniSuministro, colIniEjecutado);
                UtilExcel.BorderCeldas3(ws, rowIniSuministro, colIniDuracion, rowIniSuministro + 1, colIniDuracion);

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Segunda fila cabecera
                filActual = filActual + 1;
                alturaFila = Convert.ToDouble(0.7M) * Convert.ToDouble(ConstantesExtranetCTAF.FactorAltoFilaExcel);
                ws.Row(filActual).Height = alturaFila;

                int rowIniInicio = rowIniSuministro + 1;
                int colIniInicio = coluIniCab + 2;
                int colIniFinal = colIniInicio + 1;


                UtilExcel.BorderCeldas3(ws, rowIniInicio, colIniInicio, rowIniInicio, colIniInicio);
                UtilExcel.BorderCeldas3(ws, rowIniInicio, colIniFinal, rowIniInicio, colIniFinal);

                //Formato
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniSuministro, coluIniCab, filActual, colIniDuracion, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniSuministro, coluIniCab, filActual, colIniDuracion, "Centro");
                UtilExcel.CeldasExcelColorTexto(ws, rowIniSuministro, coluIniCab, filActual, colIniDuracion, "#000000");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniSuministro, coluIniCab, filActual, colIniDuracion, tablaData.Color);
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniSuministro, coluIniCab, filActual, colIniDuracion, tablaData.TipoFuente, tablaData.TamLetra);
                UtilExcel.CeldasExcelEnNegrita(ws, rowIniSuministro, coluIniCab, filActual, colIniDuracion);
            }


            if (registros != null && registros.Count > 0)
            {
                int filaX = 0;
                //data
                foreach (var reg in registros)
                {
                    int colAgrup = coluIniData;
                    int colX = 0;
                    int filActual = filaIniData + filaX;

                    // se usará la altura por defecto del excel para la data de cada reporte

                    if (reg.ListaCelda.Count() - 1 < 0)


                        UtilExcel.CeldasExcelWrapText(ws, filActual, coluIniData, filActual, coluIniData + reg.ListaCelda.Count() - 1);
                    if (reg.EsFilaResumen)
                        UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filActual, coluIniData, filActual, coluIniData + reg.ListaCelda.Count() - 1, tablaData.TipoFuente, tablaData.TamLetra);
                    else
                        UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filActual, coluIniData, filActual, coluIniData + reg.ListaCelda.Count() - 1, tablaData.TipoFuente, tablaData.TamLetra);

                    foreach (var celda in reg.ListaCelda)
                    {
                        int colActual = coluIniData + colX;

                        UtilExcel.BorderCeldas3(ws, filActual, colActual, filActual, colActual);
                        UtilExcel.CeldasExcelAlinearVerticalmente(ws, filActual, colActual, filActual, colActual, "Centro");

                        if (celda.TieneTextoIzquierdo)
                            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filActual, colActual, filActual, colActual, "Izquierda");
                        if (celda.TieneTextoDerecho)
                            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filActual, colActual, filActual, colActual, "Derecha");
                        if (celda.TieneTextoCentrado)
                            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filActual, colActual, filActual, colActual, "Centro");
                        if (reg.EsFilaResumen)
                        {
                            //solo se usará la altura para la fila total
                            var alturaFila = Convert.ToDouble(reg.AltoFila) * Convert.ToDouble(ConstantesExtranetCTAF.FactorAltoFilaExcel);
                            ws.Row(filActual).Height = alturaFila;

                            UtilExcel.CeldasExcelColorTexto(ws, filActual, colActual, filActual, colActual, "#000000");
                            UtilExcel.CeldasExcelColorFondo(ws, filActual, colActual, filActual, colActual, "#FFFFFF");
                            ws.Cells[filActual, colActual].Value = celda.Texto;

                            if (celda.TieneAgrupacion)
                            {
                                UtilExcel.CeldasExcelAgrupar(ws, filActual, colAgrup, filActual, colActual);
                                if (reporte == 2)
                                {

                                    UtilExcel.CeldasExcelAlinearVerticalmente(ws, filaIniCab, coluIniCab, filActual, coluIniCab + 1, "Centro");
                                    UtilExcel.CeldasExcelAlinearHorizontalmente(ws, filaIniCab, coluIniCab, filActual, coluIniCab + 1, "Centro");
                                    UtilExcel.CeldasExcelColorTexto(ws, filaIniCab, coluIniCab, filActual, coluIniCab + 1, "#000000");

                                    UtilExcel.CeldasExcelColorTexto(ws, filaIniCab, coluIniCab, filActual, coluIniCab + 1, "#000000");
                                    UtilExcel.CeldasExcelTipoYTamanioLetra(ws, filaIniCab, coluIniCab, filActual, coluIniCab + 1, tablaData.TipoFuente, tablaData.TamLetra);
                                    UtilExcel.CeldasExcelEnNegrita(ws, filaIniCab, coluIniCab, filActual, coluIniCab + 1);
                                }
                            }
                            else
                                colAgrup = colActual;
                        }
                        if (celda.TieneBorder)
                            UtilExcel.CeldasExcelColorFondoYBorder3(ws, filActual, colActual, filActual, colActual, ColorTranslator.FromHtml("#FFFFFF"), "Izquierda");
                        if (celda.TieneTextoNegrita)
                            UtilExcel.CeldasExcelEnNegrita(ws, filActual, colActual, filActual, colActual);
                        if (celda.TieneColor)
                        {
                            UtilExcel.CeldasExcelColorTexto(ws, filActual, colActual, filActual, colActual, "#FFFFFF");
                            UtilExcel.CeldasExcelColorFondo(ws, filActual, colActual, filActual, colActual, "#72B7DE");
                        }
                        if (celda.TieneTextoColor)
                        {
                            UtilExcel.CeldasExcelColorFondo(ws, filActual, colActual, filActual, colActual, "#FFFFFF");
                            UtilExcel.CeldasExcelColorTexto(ws, filActual, colActual, filActual, colActual, "#DF0000");
                        }
                        if (reporte == 2 && colActual <= coluIniCab + 1)
                        {
                            UtilExcel.CeldasExcelColorFondo(ws, filActual, coluIniCab, filActual, colActual, tablaData.Color);
                        }
                        if (celda.TieneColorERACMF)
                            UtilExcel.CeldasExcelColorFondo(ws, filActual, colActual, filActual, colActual, "#FB7A57");

                        if (celda.EsNumero)
                        {
                            if (celda.Valor != null)
                            {
                                if (celda.DigitosParteDecimal > 0)
                                {
                                    int numDigitos = celda.DigitosParteDecimal;
                                    if (celda.TieneFormatoNumeroEspecial)
                                    {
                                        numDigitos = MathHelper.GetDecimalPlaces(celda.Valor.Value);
                                        numDigitos = (numDigitos > celda.DigitosParteDecimal) ? (numDigitos <= ConstantesExtranetCTAF.MaxNumDigitos ? numDigitos : ConstantesExtranetCTAF.MaxNumDigitos) : celda.DigitosParteDecimal;
                                    }

                                    if (celda.EsNumeroTruncado)
                                    {
                                        celda.Valor = MathHelper.TruncateDecimal(celda.Valor.Value, numDigitos);
                                    }
                                    if (celda.EsNumeroRedondeado)
                                    {
                                        celda.Valor = MathHelper.Round(celda.Valor.Value, numDigitos);
                                    }

                                    string strParteDecimal = string.Empty;
                                    for (int i = 1; i <= numDigitos; i++) strParteDecimal += "0";
                                    string strFormat = "#,##0." + strParteDecimal;
                                    ws.Cells[filActual, colActual].Style.Numberformat.Format = strFormat;
                                }
                                else
                                {
                                    string strFormat = "#,##0";
                                    ws.Cells[filActual, colActual].Style.Numberformat.Format = strFormat;
                                }

                                ws.Cells[filActual, colActual].Value = celda.Valor;
                            }
                        }
                        else
                        {
                            //border transparente
                            int tieneBorderEspecial = reg.EsFilaResumen && string.IsNullOrEmpty(celda.Texto) ? 1 : 0;
                            if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt01TotalDatos) tieneBorderEspecial = 0;
                            if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt02Resumen) tieneBorderEspecial = 0;
                            if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt04Menores3Min) tieneBorderEspecial = (colX >= 3 && string.IsNullOrEmpty(celda.Texto)) ? 1 : 0;
                            if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt09Resarcimiento) tieneBorderEspecial = (colX == 7 && string.IsNullOrEmpty(celda.Texto)) ? 2 : 0;
                            if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.InterrupcionRpt09Resarcimiento) tieneBorderEspecial = (colX == 7 && string.IsNullOrEmpty(celda.Texto)) ? 2 : 0;
                            if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.ReduccionRpt01) tieneBorderEspecial = (colX >= 5 && string.IsNullOrEmpty(celda.Texto)) ? 1 : 0;

                            #region border transparente

                            if (tieneBorderEspecial == 1) //menores a tres minutos
                            {
                                var borderTabla = ws.Cells[filActual, colActual].Style.Border;
                                ws.Cells[filActual, colActual].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                                ws.Cells[filActual, colActual].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                                ws.Cells[filActual, colActual].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                            }
                            if (tieneBorderEspecial == 2)  //resarcimiento
                            {
                                var borderTabla = ws.Cells[filActual, colActual].Style.Border;
                                ws.Cells[filActual, colActual].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                                borderTabla.Left.Style = borderTabla.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                                ws.Cells[filActual, colActual].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                            }

                            #endregion


                            celda.Texto = (celda.Texto != null && tablaData.EsMayuscula) ? celda.Texto.ToUpper() : celda.Texto;

                            if (!string.IsNullOrEmpty(celda.Texto) && celda.Texto.ToUpper() == "ENSf--->".ToUpper())
                            {
                                ws.Cells[filActual, colActual].IsRichText = true;
                                OfficeOpenXml.Style.ExcelRichTextCollection rtfCollection = ws.Cells[filActual, colActual].RichText;
                                OfficeOpenXml.Style.ExcelRichText ert = rtfCollection.Add("ENS");
                                ert.Bold = true;

                                ert = rtfCollection.Add("F");
                                ert.VerticalAlign = OfficeOpenXml.Style.ExcelVerticalAlignmentFont.Subscript;

                                ert = rtfCollection.Add("--->");
                                ws.Cells[filActual, colActual].Style.Font.SetFromFont(new Font(tablaData.TipoFuente, tablaData.TamLetra));
                            }
                            else
                            {
                                ws.Cells[filActual, colActual].Value = celda.Texto;
                                if (celda.EsTextoFecha)
                                {
                                    string hhmmss = celda.Texto != null && celda.Texto.Length > 8 ? celda.Texto.Substring(celda.Texto.Length - 8, 8) : string.Empty;
                                    string ddmmyyy = celda.Texto != null && celda.Texto.Length > 8 ? celda.Texto.Substring(0, 10).Replace("/", ".") : string.Empty;
                                    ws.Cells[filActual, colActual].Value = hhmmss + (celda.TieneFormatoFechaExcel ? "\n" + ddmmyyy : string.Empty);
                                }
                            }
                        }
                        colX++;

                    }

                    //>>>>>>>>>>>>>>Caso malas actuaciones
                    if (reporte == 3)
                    {
                        if (reg.EsAgrupado && reg.ListaCelda[1].Texto == empresa)
                            UtilExcel.CeldasExcelAgrupar(ws, filaXAgrupa, coluIniData + 1, filActual, coluIniData + 1);
                        else
                            filaXAgrupa = filActual;
                        empresa = reg.ListaCelda[1].Texto;
                    }
                    //>>>>>>>>>>>>>>
                    filaX++;
                }

                ultimaFila += registros.Count + 2;
            }
            ws.View.ZoomScale = 100;




        }

        /// <summary>
        /// Determina el formato HH:MM:SS DD.MM.YYYY de la fecha para el reporte Excel
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        private bool TieneFormatoFechaEspecialWord(DateTime fechaInicio, DateTime fechaFinal)
        {
            var resultado = fechaFinal.Date - fechaInicio.Date;
            int dias = resultado.Days;
            return dias > 0;
        }
        #endregion

        #region EXPORTACION WORD
        /// <summary>
        /// Generar Reporte1ByInterrupcion ERACMF Word
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="anio"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public string GenerarReporte1ByInterrupcionERACMFWord(int tipoReporte, int afecodi, int emprcodi, string anio, string correlativo)
        {
            bool esPorEracmf = true;
            ///Data del reporte y datos del evento
            int fdatcodi = (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF;
            SiEmpresaDTO objEmpresa = emprcodi > 0 ? FactorySic.GetSiEmpresaRepository().GetById(emprcodi) : null;
            SiFuentedatosDTO objFuenteDatos = FactorySic.GetSiFuentedatosRepository().GetById(fdatcodi);
            EventoDTO regEvento = this.ObtenerInterrupcionByAfecodi(afecodi);

            //Data del reporte
            this.ListarInterrupcionSuministrosGral(afecodi, emprcodi, fdatcodi, out List<AfInterrupSuministroDTO> listaData, out List<AfInterrupSuministroDTO> listDataReportCero, out List<string> listaMsjValidacion, out List<int> listaEmprcodiReportaron, anio, correlativo);

            //Lista horas de coordinacion de normalización
            List<AfHoraCoordDTO> lstHandsonHorasCoordinacion = this.ObtenerListaCruceHoracoordInterrsuministro(afecodi, fdatcodi, listaData, anio, correlativo);

            //TOTAL DE DATOS
            this.ListarRptTotalDatos(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte1, out bool formatFechaCab1);
            TablaReporte tablaReporte1 = this.ObtenerDataExcelDatosTotales(listaReporte1, esPorEracmf, formatFechaCab1);

            //RESUMEN
            this.ListarRptResumen(afecodi, emprcodi, listaData, out List<ReporteInterrupcion> listaReporte2);
            TablaReporte tablaReporte2 = this.ObtenerDataExcelResumen(listaReporte2);
            TablaReporte tablaReporte2Leyenda = this.ObtenerDataExcelResumenLeyenda(listaReporte2);

            //MALAS ACTUACIONES

            List<AfCondicionesDTO> listaFuncionesYEtapas = new List<AfCondicionesDTO>();
            var afecodis = listaData.Select(x => new { x.Afecodi, x.EVENCODI }).Distinct();
            foreach (var item in afecodis)
            {
                listaFuncionesYEtapas.AddRange(ListAfCondicioness(item.Afecodi, item.EVENCODI));
            }

            this.ListarRptMalasActuaciones(afecodi, emprcodi, listaData, out List<ReporteInterrupcion> listaReporte3);
            TablaReporte tablaReporte3 = this.ObtenerDataExcelMalasActuaciones(listaReporte3);

            //MENORES A 3 MINUTOS
            this.ListarRptMenores3Minutos(afecodi, emprcodi, listaData, null, out List<ReporteInterrupcion> listaReporte4, out bool formatFechaCab4);
            TablaReporte tablaReporte4 = this.ObtenerDataExcelMenores3minutos(listaReporte4, esPorEracmf, formatFechaCab4);

            //NO REPORTARON INTERRUPCIÓN
            this.ListarRptNoReportaronInterrupcion(afecodi, emprcodi, listaData, listaEmprcodiReportaron, out List<ReporteInterrupcion> listaReporte5, out List<string> listaMsjValidacionEracmf);
            TablaReporte tablaReporte5 = this.ObtenerDataExcelNoReportaronInterrupcion(listaReporte5);

            //PARA LA DECISION
            bool esRptActivacionEracmf = true;
            this.ListarRptDecision(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte6, out bool formatFechaCab6, esRptActivacionEracmf);
            TablaReporte tablaReporte6 = this.ObtenerDataExcelDecision(listaReporte6, formatFechaCab6);

            //REPORTARON 0
            esRptActivacionEracmf = true;
            this.ListarRptReportaron0(afecodi, emprcodi, listDataReportCero, out List<ReporteInterrupcion> listaReporte7);
            TablaReporte tablaReporte7 = this.ObtenerDataExcelReportaron0(listaReporte7);

            //AGENTE CON DEMORAS
            this.ListarRptTotalDatos(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte8, out bool formatFechaCab8);
            TablaReporte tablaReporte8 = this.ObtenerDataExcelAgenteDemoras(listaReporte8, lstHandsonHorasCoordinacion);

            //PARA EL RESARCIMIENTO
            this.ListarRptResarcimiento(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte9, out bool formatFechaCab2, esRptActivacionEracmf);
            TablaReporte tablaReporte9 = this.ObtenerDataExcelResarcimiento(listaReporte9, formatFechaCab2);

            FontFamily fontDoc = new FontFamily(ConstantesExtranetCTAF.RptTipoFuente);

            //Plantilla y archivo salida
            string rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesExtranetCTAF.RutaReportes;
            string nombPlantilla = ConstantesExtranetCTAF.PlantillaReporteWordInterrupcionPorActivacionERACMF;
            string nombCompletPlantilla = $"{rutaBase}{nombPlantilla}";
            FileInfo archivoPlantilla = new FileInfo(nombCompletPlantilla);

            string rutaOutput = AppDomain.CurrentDomain.BaseDirectory + ConstantesExtranetCTAF.RutaReportes;
            string nombFormato = $"{regEvento.CODIGO}_ReporteInterrupciónPorActivaciónERACMF.docx";
            string nombCompletFormato = $"{rutaOutput}{nombFormato}";
            FileInfo nuevoArchivo = new FileInfo(nombCompletFormato);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombCompletFormato);
            }

            using (DocX document = DocX.Create(nombCompletFormato))
            {
                #region Header y Footer
                document.MarginLeft = 114.0f;
                document.MarginRight = 114.0f;

                //Cabecera
                document.AddHeaders();


                Novacode.Image logo = document.AddImage(AppDomain.CurrentDomain.BaseDirectory + "Content/Images/" + "Coes.png");
                document.DifferentFirstPage = false;
                document.DifferentOddAndEvenPages = false;

                Header header_first = document.Headers.odd;

                Table header_first_table = header_first.InsertTable(2, 2);
                header_first_table.Design = TableDesign.None;

                //primera fila
                Paragraph upperRightParagraph = header_first.Tables[0].Rows[0].Cells[0].Paragraphs[0];
                upperRightParagraph.AppendPicture(logo.CreatePicture(48, 92));
                upperRightParagraph.Alignment = Alignment.left;
                header_first_table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Top;

                header_first.InsertParagraph().AppendLine().Append(" ").Font(fontDoc);

                //Pie de página
                document.AddFooters();

                Footer footer_main = document.Footers.odd;

                Paragraph pFooter = footer_main.Paragraphs.First();
                pFooter.Alignment = Alignment.left;
                pFooter.Append("Página ");
                pFooter.AppendPageNumber(PageNumberFormat.normal);
                pFooter.Append(" de ");
                pFooter.AppendPageCount(PageNumberFormat.normal);
                #endregion

                #region Título y Evento

                this.GenerarTituloWord(document, ConstantesExtranetCTAF.RptTipoFuente, objEmpresa, objFuenteDatos);

                this.GenerarTablaEventoWord(document, regEvento, ConstantesExtranetCTAF.RptTipoFuente, ConstantesExtranetCTAF.RptColor, ConstantesExtranetCTAF.RptTamanio);

                #endregion
                #region REPORTE DATOS TOTALES
                Paragraph p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "01.Total de Datos", fontDoc);

                List<CabeceraReporteColumna> dataCab = new List<CabeceraReporteColumna>();
                List<RegistroReporte> registros = new List<RegistroReporte>();
                List<RegistroReporte> registrosTotales, registrosTotalesLeyenda = new List<RegistroReporte>();
                TablaReporte Tabla = new TablaReporte();

                registrosTotales = tablaReporte1.ListaRegistros;
                int numFilas;
                int numColumnas;
                //Table secuencia_0; 

                //var codigosEventos = tablaReporte1.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                //foreach (var item in codigosEventos)
                //{
                //    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                ////numFilas = registros.Count;
                ////numColumnas = tablaReporte1.CabeceraColumnas.Count;
                ////Table secuencia_0 = document.InsertTable(numFilas + 1, numColumnas);

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //this.GenerarRptWord(ref secuencia_0, tablaReporte1, 1, fdatcodi);
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                Table secuencia_0;
                registrosTotales = tablaReporte1.ListaRegistros;
                var codigosEventos = tablaReporte1.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                foreach (var item in codigosEventos)
                {
                    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtituloReporte(ref p, registros[0].Nombre, fontDoc);
                    Tabla = tablaReporte1;
                    Tabla.ListaRegistros = registros;
                    numFilas = registros.Count;
                    numColumnas = tablaReporte1.CabeceraColumnas.Count;
                    secuencia_0 = document.InsertTable(numFilas + 1, numColumnas);

                    this.GenerarRptWord(ref secuencia_0, tablaReporte1, 1, fdatcodi);

                }



                ////>>>>>>>>>>>>>>>>>>>>>Horas de Coordinación >>>>>>>>>>>>>>>>>>>>>>>>>>>
                #region


                #endregion

                #endregion

                #region REPORTE RESUMEN
                p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "02. Resumen", fontDoc);

                registrosTotales = tablaReporte2.ListaRegistros;
                registrosTotalesLeyenda = tablaReporte2Leyenda.ListaRegistros;
                var codigosEventosResumen = tablaReporte2.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                foreach (var item in codigosEventosResumen)
                {
                    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtituloReporte(ref p, registros[0].Nombre, fontDoc);
                    Tabla = tablaReporte2;
                    Tabla.ListaRegistros = registros;
                    numFilas = registros.Count;
                    numColumnas = tablaReporte2.CabeceraColumnas.Count;
                    Table secuencia_2 = document.InsertTable(numFilas + 3, numColumnas);

                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    this.GenerarRptWord(ref secuencia_2, tablaReporte2, 2, fdatcodi);
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>



                    registros = registrosTotalesLeyenda.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    //Leyenda
                    p = document.InsertParagraph().Font(fontDoc);
                    Tabla = tablaReporte2Leyenda;
                    Tabla.ListaRegistros = registros;


                    numFilas = registros.Count;
                    numColumnas = tablaReporte2Leyenda.CabeceraColumnas.Count;
                    if (numFilas > 0)
                    {
                        Table secuencia_2Leyenda = document.InsertTable(numFilas, numColumnas);

                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        this.GenerarRptWord(ref secuencia_2Leyenda, tablaReporte2Leyenda, 2, fdatcodi);
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    }


                }






                #endregion

                #region REPORTE MALAS ACTUACIONES
                p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "03. Malas Actuaciones", fontDoc);



                Table secuencia_3;
                registrosTotales = tablaReporte3.ListaRegistros;
                var codigosEventosMalasActuaciones = tablaReporte3.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                foreach (var item in codigosEventosMalasActuaciones)
                {
                    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtituloReporte(ref p, registros[0].Nombre, fontDoc);
                    Tabla = tablaReporte3;
                    Tabla.ListaRegistros = registros;
                    numFilas = registros.Count;
                    numColumnas = tablaReporte3.CabeceraColumnas.Count;
                    secuencia_3 = document.InsertTable(numFilas + 1, numColumnas);

                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    GenerarRptWord(ref secuencia_3, tablaReporte3, 3, fdatcodi);
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                }









                #region 

                #endregion

                #endregion

                #region REPORTE MENORES A 3 MINUTOS
                p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "04. Menores a 3 minutos", fontDoc);

                //dataCab = tablaReporte4.CabeceraColumnas;
                //registros = tablaReporte4.ListaRegistros;

                ////var reg1 = registros.FirstOrDefault();
                ////var reg2 = registros.Find(x=>x.EsFilaResumen);
                ////registros = new List<RegistroReporte>();
                ////registros.Add(reg1);
                ////registros.Add(reg2);
                //////registros = registros.Where(x=>!x.EsFilaResumen).ToList();
                ////tablaReporte4.ListaRegistros = registros;

                //numFilas = registros.Count;
                //numColumnas = tablaReporte4.CabeceraColumnas.Count;
                //Table secuencia_5 = document.InsertTable(numFilas + 1, numColumnas);

                ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //GenerarRptWord(ref secuencia_5, tablaReporte4, 4, fdatcodi);
                ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                Table secuencia_5;
                registrosTotales = tablaReporte4.ListaRegistros;
                var codigosEventosDemora = tablaReporte4.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                foreach (var item in codigosEventosDemora)
                {
                    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtituloReporte(ref p, registros[0].Nombre, fontDoc);
                    Tabla = tablaReporte4;
                    Tabla.ListaRegistros = registros;
                    numFilas = registros.Count;
                    numColumnas = tablaReporte4.CabeceraColumnas.Count;
                    secuencia_5 = document.InsertTable(numFilas + 1, numColumnas);

                    GenerarRptWord(ref secuencia_5, tablaReporte4, 4, fdatcodi);

                }

                #endregion

                #region REPORTE AGENTES QUE REPORTARON 0
                p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "05. Agentes que reportaron 0", fontDoc);

                //dataCab = tablaReporte7.CabeceraColumnas;
                //registros = tablaReporte7.ListaRegistros;

                //numFilas = registros.Count;
                //numColumnas = tablaReporte7.CabeceraColumnas.Count;


                registrosTotales = tablaReporte7.ListaRegistros;
                var codigosEventosReportaronCero = tablaReporte7.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                foreach (var item in codigosEventosReportaronCero)
                {
                    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtituloReporte(ref p, registros[0].Nombre, fontDoc);
                    Tabla = tablaReporte7;
                    Tabla.ListaRegistros = registros;
                    numFilas = registros.Count;
                    numColumnas = tablaReporte7.CabeceraColumnas.Count;
                    Table secuencia_6 = document.InsertTable(numFilas + 1, numColumnas);

                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    GenerarRptWord(ref secuencia_6, tablaReporte7, 4, fdatcodi);
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                }




                #endregion

                #region REPORTE AGENTES QUE NO REPORTARON
                p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "06. Agentes que no reportaron", fontDoc);

                //dataCab = tablaReporte5.CabeceraColumnas;
                //registros = tablaReporte5.ListaRegistros;

                //numFilas = registros.Count;
                //numColumnas = tablaReporte5.CabeceraColumnas.Count;

                registrosTotales = tablaReporte5.ListaRegistros;
                var codigosEventosNoReportaron = tablaReporte5.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                foreach (var item in codigosEventosNoReportaron)
                {
                    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtituloReporte(ref p, registros[0].Nombre, fontDoc);
                    Tabla = tablaReporte5;
                    Tabla.ListaRegistros = registros;
                    numFilas = registros.Count;
                    numColumnas = tablaReporte5.CabeceraColumnas.Count;
                    Table secuencia_7 = document.InsertTable(numFilas + 1, numColumnas);

                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    GenerarRptWord(ref secuencia_7, tablaReporte5, 4, fdatcodi);
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                }



                #endregion

                #region REPORTE AGENTE CON DEMORAS
                p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "07. Agente con demoras", fontDoc);

                //dataCab = tablaReporte8.CabeceraColumnas;
                //registros = tablaReporte8.ListaRegistros;

                //numFilas = registros != null ? registros.Count : 0;
                //numColumnas = tablaReporte8.CabeceraColumnas.Count;
                //Table secuencia_8 = document.InsertTable(numFilas + 1, numColumnas);

                registrosTotales = tablaReporte8.ListaRegistros;
                var codigosEventosConDemora = tablaReporte8.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                foreach (var item in codigosEventosConDemora)
                {
                    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtituloReporte(ref p, registros[0].Nombre, fontDoc);
                    Tabla = tablaReporte8;
                    Tabla.ListaRegistros = registros;
                    numFilas = registros.Count;
                    numColumnas = tablaReporte8.CabeceraColumnas.Count;
                    Table secuencia_8 = document.InsertTable(numFilas + 1, numColumnas);

                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    GenerarRptWord(ref secuencia_8, tablaReporte8, 4, fdatcodi);
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                }






                #endregion

                #region REPORTE DECISIÓN
                p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "08. Decisión", fontDoc);

                //dataCab = tablaReporte6.CabeceraColumnas;
                //registros = tablaReporte6.ListaRegistros;

                //numFilas = registros.Count;
                //numColumnas = tablaReporte6.CabeceraColumnas.Count;
                //Table secuencia_9 = document.InsertTable(numFilas + 1, numColumnas);

                ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //GenerarRptWord(ref secuencia_9, tablaReporte6, 4, fdatcodi);
                ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                Table secuencia_9;
                registrosTotales = tablaReporte6.ListaRegistros;
                var codigosEventosDecision = tablaReporte6.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                foreach (var item in codigosEventosDecision)
                {
                    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    int nroFila = 1;
                    foreach (var item2 in registros)
                    {
                        item2.ListaCelda[0].Fila = nroFila;
                        item2.ListaCelda[0].Valor = nroFila;
                        nroFila++;
                    }

                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtituloReporte(ref p, registros[0].Nombre, fontDoc);
                    Tabla = tablaReporte6;
                    Tabla.ListaRegistros = registros;
                    numFilas = registros.Count;
                    numColumnas = tablaReporte6.CabeceraColumnas.Count;
                    secuencia_9 = document.InsertTable(numFilas + 1, numColumnas);

                    GenerarRptWord(ref secuencia_9, tablaReporte6, 4, fdatcodi);

                }
                #endregion

                #region REPORTE RESARCIMIENTO
                p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "09. Resarcimiento", fontDoc);

                //dataCab = tablaReporte9.CabeceraColumnas;
                //registros = tablaReporte9.ListaRegistros;

                ////var reg1 = registros.FirstOrDefault();
                ////var reg2 = registros.Find(x => x.EsFilaResumen);
                ////registros = new List<RegistroReporte>();
                ////registros.Add(reg1);
                ////registros.Add(reg2);
                //////////registros = registros.Where(x=>!x.EsFilaResumen).ToList();
                ////tablaReporte9.ListaRegistros = registros;

                //numFilas = registros.Count;
                //numColumnas = tablaReporte9.CabeceraColumnas.Count;
                //Table secuencia_10 = document.InsertTable(numFilas + 1, numColumnas);

                ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                ////GenerarRptWord(ref secuencia_10, tablaReporte9, 4, fdatcodi);
                ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                ///
                Table secuencia_10;
                registrosTotales = tablaReporte9.ListaRegistros;
                var codigosEventosResarcimiento = tablaReporte9.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                foreach (var item in codigosEventosResarcimiento)
                {
                    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtituloReporte(ref p, registros[0].Nombre, fontDoc);
                    Tabla = tablaReporte9;
                    Tabla.ListaRegistros = registros;
                    numFilas = registros.Count;
                    numColumnas = tablaReporte9.CabeceraColumnas.Count;
                    secuencia_10 = document.InsertTable(numFilas + 1, numColumnas);

                    GenerarRptWord(ref secuencia_10, tablaReporte9, 4, fdatcodi);

                }

                #endregion

                document.SaveAs(nombCompletFormato);
            }
            return nombFormato;

        }

        private void HeadTablaWordConfig(TablaReporte tabla, ref Table secuencia, int numFil = 0)
        {
            FontFamily fontDoc1 = new FontFamily(tabla.TipoFuente);

            int numCol = 0;
            foreach (var item in tabla.CabeceraColumnas)
            {
                double ancho = Convert.ToDouble(item.AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);

                string valorTexto = !string.IsNullOrEmpty(item.NombreColumna) ? item.NombreColumna : " ";

                secuencia.Rows[numFil].Cells[numCol].Width = ancho;
                if (tabla.ReptiCodiTabla != ConstantesExtranetCTAF.EracmfRpt09Resarcimiento)
                    secuencia.Rows[numFil].Cells[numCol].Paragraphs[0].Append(valorTexto);
                else
                {
                    Novacode.Formatting textRed = new Novacode.Formatting();
                    textRed.FontColor = Color.Red;
                    switch (numCol)
                    {
                        case 1:
                            secuencia.Rows[numFil].Cells[numCol].Paragraphs[0].InsertText("POTENCIA INTERRUMPIDA (MW) ");
                            secuencia.Rows[numFil].Cells[numCol].Paragraphs[0].InsertText("\n(A)", false, textRed);
                            break;
                        case 5:
                            secuencia.Rows[numFil].Cells[numCol].Paragraphs[0].InsertText("TIEMPO DURACIÓN (HORAS) ");
                            secuencia.Rows[numFil].Cells[numCol].Paragraphs[0].InsertText("\n(B)", false, textRed);
                            break;
                        case 6:
                            secuencia.Rows[numFil].Cells[numCol].Paragraphs[0].InsertText("ENERGÍA NO SUMINISTRADA (MWH) ");
                            secuencia.Rows[numFil].Cells[numCol].Paragraphs[0].InsertText("\n(AXB)", false, textRed);
                            break;
                        default:
                            secuencia.Rows[numFil].Cells[numCol].Paragraphs[0].Append(valorTexto);
                            break;
                    }
                }

                this.CentrarCellTableWord(secuencia.Rows[0].Cells[numCol]);
                secuencia.Rows[numFil].Cells[numCol].FillColor = System.Drawing.ColorTranslator.FromHtml(tabla.Color);
                secuencia.Rows[numFil].Cells[numCol].Paragraphs[0].FontSize(tabla.TamLetra);
                secuencia.Rows[numFil].Cells[numCol].Paragraphs[0].Font(fontDoc1);
                secuencia.Rows[numFil].Cells[numCol].Paragraphs[0].Bold();

                secuencia.Rows[numFil].Cells[numCol].MarginLeft = 0;
                secuencia.Rows[numFil].Cells[numCol].MarginRight = 0;

                numCol++;
            }
        }

        private void HeadTablaWordConfigResumen(TablaReporte tablaReporte2, ref Table secuencia_2)
        {

            //fila 1
            string strHead = "ZONA,EMPRESA,ETAPA,,,,,,,,,,TOTAL ZONA (MW)";

            foreach (var item in tablaReporte2.CabeceraColumnas)
                item.NombreColumna = string.Empty;

            this.HeadTablaWordConfig(tablaReporte2, ref secuencia_2, 1);
            this.HeadTablaWordConfig(tablaReporte2, ref secuencia_2, 2);

            tablaReporte2.CabeceraColumnas[0].NombreColumna = "ZONA";
            tablaReporte2.CabeceraColumnas[1].NombreColumna = "EMPRESA";
            tablaReporte2.CabeceraColumnas[2].NombreColumna = "ETAPA";
            tablaReporte2.CabeceraColumnas[12].NombreColumna = "TOTAL ZONA (MW)";

            this.HeadTablaWordConfig(tablaReporte2, ref secuencia_2, 0);

            //Fila2
            IngresarCeldaCAB(secuencia_2, 1, 2, "f");
            IngresarCeldaCAB(secuencia_2, 1, 9, "Df");

            //fila3
            IngresarCeldaCAB(secuencia_2, 2, 2, "1°");
            IngresarCeldaCAB(secuencia_2, 2, 3, "2°");
            IngresarCeldaCAB(secuencia_2, 2, 4, "3°");
            IngresarCeldaCAB(secuencia_2, 2, 5, "4°");
            IngresarCeldaCAB(secuencia_2, 2, 6, "5°");
            IngresarCeldaCAB(secuencia_2, 2, 7, "6°");
            IngresarCeldaCAB(secuencia_2, 2, 8, "7° (Reposición)");

            IngresarCeldaCAB(secuencia_2, 2, 9, "1°");
            IngresarCeldaCAB(secuencia_2, 2, 10, "2°");
            IngresarCeldaCAB(secuencia_2, 2, 11, "3°");

            //Agrupar 
            CeldasWordAgruparColumna(ref secuencia_2, 0, 0, 2);//zona
            CeldasWordAgruparColumna(ref secuencia_2, 1, 0, 2);//empresa
            CeldasWordAgruparColumna(ref secuencia_2, 12, 0, 2);//total

            CeldasWordAgruparFila(ref secuencia_2, 1, 2, 8); //f
            CeldasWordAgruparFila(ref secuencia_2, 1, 3, 5); //df
            CeldasWordAgruparFila(ref secuencia_2, 0, 2, 11); //etapa


            //quitar parrafos unidos
            for (int i = 0; i < 9; i++) secuencia_2.Rows[0].Cells[2].Paragraphs.Last().Remove(false);//etapa
            for (int i = 0; i < 6; i++) secuencia_2.Rows[1].Cells[2].Paragraphs.Last().Remove(false);//df
            for (int i = 0; i < 2; i++) secuencia_2.Rows[1].Cells[3].Paragraphs.Last().Remove(false);//f


            //Alto a las filas de la cabecera
            var alturaFila1 = Convert.ToDouble(0.54M) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAltoFilaWord);
            var alturaFila2 = Convert.ToDouble(0.54M) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAltoFilaWord);
            var alturaFila3 = Convert.ToDouble(0.85M) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAltoFilaWord);
            secuencia_2.Rows[0].Height = alturaFila1;
            secuencia_2.Rows[1].Height = alturaFila2;
            secuencia_2.Rows[2].Height = alturaFila3;
        }

        private void HeadTablaWordConfigResumenLeyenda(TablaReporte tablaReporte2, ref Table secuencia_2)
        {

            //fila 1
            string strHead = "ZONA,EMPRESA,ETAPA";



            tablaReporte2.CabeceraColumnas[0].NombreColumna = "ZONA";
            tablaReporte2.CabeceraColumnas[1].NombreColumna = "EMPRESA";
            tablaReporte2.CabeceraColumnas[2].NombreColumna = "ETAPA";

            this.HeadTablaWordConfig(tablaReporte2, ref secuencia_2, 0);



        }

        private void HeadTablaWordConfigDemoraRestablecimiento(TablaReporte tablaReporte2, ref Table secuencia_2)
        {

            //fila 1
            var string1 = tablaReporte2.CabeceraColumnas[2].NombreColumna;
            var string2 = tablaReporte2.CabeceraColumnas[3].NombreColumna;
            var string3 = tablaReporte2.CabeceraColumnas[0].NombreColumna;
            var string4 = tablaReporte2.CabeceraColumnas[1].NombreColumna;
            var string5 = tablaReporte2.CabeceraColumnas[4].NombreColumna;

            tablaReporte2.CabeceraColumnas[2].NombreColumna = "COORDINADO";
            tablaReporte2.CabeceraColumnas[3].NombreColumna = "EJECUTADO";
            this.HeadTablaWordConfig(tablaReporte2, ref secuencia_2, 0);

            foreach (var item in tablaReporte2.CabeceraColumnas)
                item.NombreColumna = string.Empty;
            tablaReporte2.CabeceraColumnas[2].NombreColumna = string1;
            tablaReporte2.CabeceraColumnas[3].NombreColumna = string2;

            this.HeadTablaWordConfig(tablaReporte2, ref secuencia_2, 1);

            tablaReporte2.CabeceraColumnas[0].NombreColumna = string3;
            tablaReporte2.CabeceraColumnas[1].NombreColumna = string4;
            tablaReporte2.CabeceraColumnas[4].NombreColumna = string5;



            //Agrupar 
            CeldasWordAgruparColumna(ref secuencia_2, 0, 0, 1);//suministro
            CeldasWordAgruparColumna(ref secuencia_2, 1, 0, 1);//subestacion
            CeldasWordAgruparColumna(ref secuencia_2, 4, 0, 1);//duracion

            this.CentrarCellTableWord(secuencia_2.Rows[1].Cells[2]);
            this.CentrarCellTableWord(secuencia_2.Rows[1].Cells[3]);




            //Alto a las filas de la cabecera
            var alturaFila1 = Convert.ToDouble(0.5M) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAltoFilaWord);
            var alturaFila2 = Convert.ToDouble(tablaReporte2.AltoFilaCab) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAltoFilaWord);
            secuencia_2.Rows[0].Height = alturaFila1;
            secuencia_2.Rows[1].Height = alturaFila2;
        }

        private void AddFilaTablaWordX(ref Table secuencia_, int row, int col, string name, decimal anchoColum)
        {
            FontFamily fontDoc1 = new FontFamily("Calibri");
            int fontTamanio = 8;

            secuencia_.Rows[row].Cells[col].Paragraphs[0].Append(name);
            this.CentrarCellTableWord(secuencia_.Rows[row].Cells[col]);

            secuencia_.Rows[row].Cells[col].Paragraphs[0].Font(fontDoc1);
            secuencia_.Rows[row].Cells[col].Paragraphs[0].FontSize(fontTamanio);

            secuencia_.Rows[row].Cells[col].MarginLeft = 0;
            secuencia_.Rows[row].Cells[col].MarginRight = 0;



            double ancho = Convert.ToDouble(anchoColum) * 10;
            secuencia_.Rows[row].Cells[col].Width = ancho;
        }

        private void CeldasWordAgruparFila(ref Table secuencia_, int fila, int coluIni, int coluFin)
        {
            FontFamily fontDoc1 = new FontFamily("Calibri");
            int fontTamanio = 8;

            this.CentrarCellTableWord(secuencia_.Rows[fila].Cells[coluIni]);

            secuencia_.Rows[fila].Cells[coluIni].Paragraphs[0].Font(fontDoc1);
            secuencia_.Rows[fila].Cells[coluIni].Paragraphs[0].FontSize(fontTamanio);

            secuencia_.Rows[fila].Cells[coluIni].MarginLeft = 0;
            secuencia_.Rows[fila].Cells[coluIni].MarginRight = 0;

            secuencia_.Rows[fila].MergeCells(coluIni, coluFin);
        }

        private void CeldasWordAgruparColumna(ref Table secuencia_, int columna, int filIni, int filFin)
        {
            FontFamily fontDoc1 = new FontFamily("Calibri");
            int fontTamanio = 8;

            this.CentrarCellTableWord(secuencia_.Rows[filIni].Cells[columna]);

            secuencia_.Rows[filIni].Cells[columna].Paragraphs[0].Font(fontDoc1);
            secuencia_.Rows[filIni].Cells[columna].Paragraphs[0].FontSize(fontTamanio);

            secuencia_.Rows[filIni].Cells[columna].MarginLeft = 0;
            secuencia_.Rows[filIni].Cells[columna].MarginRight = 0;

            secuencia_.MergeCellsInColumn(columna, filIni, filFin);
        }

        private void IngresarCeldaCAB(Table secuencia_, int fila, int col, string valor)
        {
            FontFamily fontDoc1 = new FontFamily("Calibri");
            int fontTamanio = 8;

            secuencia_.Rows[fila].Cells[col].Paragraphs[0].Append(valor);
            secuencia_.Rows[fila].Cells[col].FillColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            this.CentrarCellTableWord(secuencia_.Rows[fila].Cells[col]);
            secuencia_.Rows[fila].Cells[col].Paragraphs[0].Bold();
            secuencia_.Rows[fila].Cells[col].Paragraphs[0].Font(fontDoc1);
            secuencia_.Rows[fila].Cells[col].Paragraphs[0].FontSize(fontTamanio);
        }
        /// <summary>
        /// Generar Titulo Word
        /// </summary>
        /// <param name="document"></param>
        /// <param name="tipoFuente"></param>
        /// <param name="objEmpresa"></param>
        /// <param name="objFuenteDatos"></param>
        public void GenerarTituloWord(DocX document, string tipoFuente, SiEmpresaDTO objEmpresa, SiFuentedatosDTO objFuenteDatos)
        {
            FontFamily fontDoc = new FontFamily(tipoFuente);
            string titulo = objFuenteDatos != null ? objFuenteDatos.Fdatnombre : string.Empty;
            string nombreEmpresa = objEmpresa != null && objEmpresa.Emprnomb != null ? objEmpresa.Emprnomb.Trim() : string.Empty;

            Paragraph p = document.InsertParagraph().Append(titulo).FontSize(16).Font(fontDoc).Bold();


            p.Alignment = Alignment.center;

            if (!string.IsNullOrEmpty(nombreEmpresa))
            {
                p.AppendLine().Font(fontDoc).Append(nombreEmpresa).FontSize(13).Font(fontDoc).Bold();
                p.Alignment = Alignment.center;
            }

            p = document.InsertParagraph().Font(fontDoc);
        }
        /// <summary>
        /// Generar Tabla Event oWord
        /// </summary>
        /// <param name="document"></param>
        /// <param name="regEvento"></param>
        /// <param name="tipoFuente"></param>
        /// <param name="color"></param>
        /// <param name="tamLetra"></param>
        public void GenerarTablaEventoWord(DocX document, EventoDTO regEvento, string tipoFuente, string color, int tamLetra)
        {
            Table secuencia = document.InsertTable(5, 2);
            FontFamily fontDoc1 = new FontFamily(tipoFuente);

            List<string> listaCol1 = new List<string>();
            listaCol1.Add("Nro. Evento:");
            listaCol1.Add("Hora Inicio:");
            listaCol1.Add("Hora Fin:");
            listaCol1.Add("Resumen del Evento:");
            listaCol1.Add("Descripción:");

            List<string> listaCol2 = new List<string>();
            listaCol2.Add(regEvento.CODIGO);
            listaCol2.Add(regEvento.EVENINI?.ToString(ConstantesBase.FormatoFechaFullBase));
            listaCol2.Add(regEvento.EVENFIN?.ToString(ConstantesBase.FormatoFechaFullBase));
            listaCol2.Add(regEvento.EVENASUNTO);
            listaCol2.Add(regEvento.EVENDESC);

            double margin = Convert.ToDouble(0.07) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);

            for (int numCol = 0; numCol < 2; numCol++)
            {
                decimal tamanioCol = numCol == 0 ? 3.0m : 12.0m;
                string colorCol = numCol == 0 ? color : null;
                List<string> listaCol = numCol == 0 ? listaCol1 : listaCol2;

                for (int numFil = 0; numFil < 5; numFil++)
                {
                    double ancho = Convert.ToDouble(tamanioCol) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);

                    secuencia.Rows[numFil].Cells[numCol].Width = ancho;
                    secuencia.Rows[numFil].Cells[numCol].Paragraphs[0].Append(listaCol[numFil]);


                    if (colorCol != null)
                        secuencia.Rows[numFil].Cells[numCol].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                    secuencia.Rows[numFil].Cells[numCol].Paragraphs[0].FontSize(tamLetra);
                    secuencia.Rows[numFil].Cells[numCol].Paragraphs[0].Font(fontDoc1);
                    if (colorCol != null)
                        secuencia.Rows[numFil].Cells[numCol].Paragraphs[0].Bold();

                    secuencia.Rows[numFil].Cells[numCol].MarginLeft = margin;
                    secuencia.Rows[numFil].Cells[numCol].MarginRight = margin;
                    secuencia.Rows[numFil].Cells[numCol].MarginTop = margin;
                    secuencia.Rows[numFil].Cells[numCol].MarginBottom = margin;
                }
            }
        }
        /// <summary>
        /// Generar Rpt Word
        /// </summary>
        /// <param name="secuencia_"></param>
        /// <param name="tablaData"></param>
        /// <param name="reporte"></param>
        /// <param name="fdatcodi"></param>
        public void GenerarRptWord(ref Table secuencia_, TablaReporte tablaData, int reporte, int fdatcodi)
        {
            var dataCab = tablaData.CabeceraColumnas;
            List<RegistroReporte> registros = tablaData.ListaRegistros;

            var numFilas = registros != null ? registros.Count : 0;
            var numColumnas = tablaData.CabeceraColumnas.Count;

            //secuencia_.AutoFit = AutoFit.Contents;
            secuencia_.AutoFit = AutoFit.ColumnWidth;
            secuencia_.Rows[0].TableHeader = true;

            //secuencia_.AutoFit = AutoFit.ColumnWidth;
            secuencia_.Design = TableDesign.TableGrid;
            if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt02Resumen && tablaData.CabeceraColumnas.Count == 3)
            {
                secuencia_.Design = TableDesign.None;
                secuencia_.Rows[0].TableHeader = false;

            }
            if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt02Resumen && tablaData.CabeceraColumnas.Count > 3)
            {

                secuencia_.Rows[0].TableHeader = true;
                secuencia_.Rows[1].TableHeader = true;
                secuencia_.Rows[2].TableHeader = true;

            }
            if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.InterrupRpt02DemoraRestablecimiento)
            {

                secuencia_.Rows[0].TableHeader = true;
                secuencia_.Rows[1].TableHeader = true;

            }

            secuencia_.Alignment = Alignment.center;

            if (!((fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF && tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt02Resumen)
                || (fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF && tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt06NoReportaron)
                || (fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF && tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt09Resarcimiento)
                 || (fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion && tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.InterrupcionRpt09Resarcimiento)
                || (fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion && tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.InterrupRpt02DemoraRestablecimiento)))
            {
                this.HeadTablaWordConfig(tablaData, ref secuencia_);
                var alturaCabecera = Convert.ToDouble(tablaData.AltoFilaCab) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAltoFilaWord);
                secuencia_.Rows[0].Height = alturaCabecera;
            }
            else
            {
                if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt02Resumen && tablaData.CabeceraColumnas.Count > 3)
                {
                    this.HeadTablaWordConfigResumen(tablaData, ref secuencia_);
                }

                if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt06NoReportaron)
                {
                    this.HeadTablaWordConfig(tablaData, ref secuencia_);
                    var alturaCabecera = Convert.ToDouble(tablaData.AltoFilaCab) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAltoFilaWord);
                    secuencia_.Rows[0].Height = alturaCabecera;

                    secuencia_.Rows[0].MergeCells(0, 3);
                    //quitar parrafos unidos
                    for (int i = 0; i < 3; i++) secuencia_.Rows[0].Cells[0].Paragraphs.Last().Remove(false);
                }

                if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt09Resarcimiento)
                {
                    this.HeadTablaWordConfig(tablaData, ref secuencia_);

                    //ocultar la ultima celda de la cabecera
                    secuencia_.Rows[0].Cells[7].FillColor = Color.White;
                    secuencia_.Rows[0].Cells[7].SetBorder(TableCellBorderType.Right, new Border(BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Transparent));
                    secuencia_.Rows[0].Cells[7].SetBorder(TableCellBorderType.Bottom, new Border(BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Transparent));
                    secuencia_.Rows[0].Cells[7].SetBorder(TableCellBorderType.Left, new Border(BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Transparent));
                    secuencia_.Rows[0].Cells[7].SetBorder(TableCellBorderType.Top, new Border(BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Transparent));
                }

                if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.InterrupRpt02DemoraRestablecimiento)
                    this.HeadTablaWordConfigDemoraRestablecimiento(tablaData, ref secuencia_);
            }

            //>>>>>>>>>>>>>>>>>>> Agrupación Malas actuaciones
            var empresa = string.Empty;
            var filaXAgrupa = 1;
            //>>>>>>>>>>>>>>>>>>>


            if (registros != null && registros.Count > 0)
            {
                int filaX = reporte == 2 ? 2 : 0;
                filaX = (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.InterrupRpt02DemoraRestablecimiento) ? 1 : filaX;

                if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt02Resumen && tablaData.CabeceraColumnas.Count == 3)
                {
                    filaX = -1;
                }

                //data
                foreach (var reg in registros)
                {
                    int colX = 0; //posicion columna en novacode
                    int colTablaReporte = 0; //posicion columna de la tablareporte

                    //si la fila tiene una celda con valor NULL o vacío, la celda no debe tener margin top o bottom, por defecto se pone Calibri 11, que aumenta el alto por defecto
                    bool existeFilaCompleta = true;
                    foreach (var celda in reg.ListaCelda)
                    {
                        if (!celda.EsNumero && !celda.EsTexto)
                        {
                            existeFilaCompleta = false;
                        }
                        else
                        {
                            if (celda.EsNumero)
                                existeFilaCompleta = celda.Valor != null && existeFilaCompleta;
                            else
                                existeFilaCompleta = !string.IsNullOrEmpty(celda.Texto) && existeFilaCompleta;
                        }
                    }

                    foreach (var celda in reg.ListaCelda)
                    {
                        if (!celda.TieneAgrupacion)
                        {
                            double ancho = Convert.ToDouble(tablaData.CabeceraColumnas[colTablaReporte].AnchoColumna) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                            double marginVertical = Convert.ToDouble(0.07) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);
                            double marginHorizontal = Convert.ToDouble(0.06) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAnchoColumWord);

                            secuencia_.Rows[filaX + 1].Cells[colX].MarginLeft = marginHorizontal;
                            secuencia_.Rows[filaX + 1].Cells[colX].MarginRight = marginHorizontal;

                            if (existeFilaCompleta)
                            {
                                secuencia_.Rows[filaX + 1].Cells[colX].MarginTop = marginVertical;
                                secuencia_.Rows[filaX + 1].Cells[colX].MarginBottom = marginVertical;
                            }
                            secuencia_.Rows[filaX + 1].Cells[colX].Width = ancho;

                            //la orientacion debe ser luego de generar texto
                            int alignHorizontal = (int)Alignment.center;
                            if (celda.TieneTextoIzquierdo)
                                alignHorizontal = (int)Alignment.left;
                            if (celda.TieneTextoDerecho)
                                alignHorizontal = (int)Alignment.right;
                            if (celda.TieneTextoCentrado)
                                alignHorizontal = (int)Alignment.center;

                            bool tieneNegrita = celda.TieneTextoNegrita;

                            string colorFondo = null;
                            string colorTexto = null;
                            if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt02Resumen && (colTablaReporte == 0 || colTablaReporte == 1) && tablaData.CabeceraColumnas.Count > 3)
                            {
                                colorFondo = tablaData.Color;
                                tieneNegrita = true;
                            }
                            if (celda.TieneTextoColor)
                            {
                                colorTexto = "#FFFFFF";
                            }
                            if (celda.TieneColorERACMF)
                                colorFondo = tablaData.ColorERACMF;

                            if (reg.EsFilaResumen)
                            {
                                tieneNegrita = true;
                                alignHorizontal = (int)Alignment.center;

                                var alturaFila = Convert.ToDouble(reg.AltoFila) / Convert.ToDouble(ConstantesExtranetCTAF.FactorAltoFilaWord);
                                secuencia_.Rows[filaX + 1].Height = alturaFila;

                                if (reg.EsAgrupado)
                                {
                                    if (celda.EsTexto) //
                                    {
                                        int numColumnasAgrupadas = 0;
                                        bool terminoBucle = false;
                                        for (int i = colX + 1; i < reg.ListaCelda.Count() && !terminoBucle; i++)
                                        {
                                            if (reg.ListaCelda[i].TieneAgrupacion)
                                                numColumnasAgrupadas++;
                                            else
                                                terminoBucle = true;
                                        }

                                        if (numColumnasAgrupadas > 0)
                                        {
                                            //unir tres columnas y eliminar los dos ultimos parrafos
                                            secuencia_.Rows[filaX + 1].MergeCells(colX, numColumnasAgrupadas);
                                            for (int i = 0; i < numColumnasAgrupadas; i++)
                                            {
                                                secuencia_.Rows[filaX + 1].Cells[colX].Paragraphs.Last().Remove(false);
                                            }
                                        }
                                    }
                                }
                            }

                            if (celda.EsNumero)
                            {
                                if (celda.Valor != null)
                                {
                                    NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                                    nfi.NumberGroupSeparator = " ";
                                    nfi.NumberDecimalDigits = 3;
                                    nfi.NumberDecimalSeparator = ",";

                                    if (celda.DigitosParteDecimal > 0)
                                    {
                                        int numDigitos = celda.DigitosParteDecimal;
                                        if (celda.TieneFormatoNumeroEspecial)
                                        {
                                            numDigitos = MathHelper.GetDecimalPlaces(celda.Valor.Value);
                                            numDigitos = (numDigitos > celda.DigitosParteDecimal) ? (numDigitos <= ConstantesExtranetCTAF.MaxNumDigitos ? numDigitos : ConstantesExtranetCTAF.MaxNumDigitos) : celda.DigitosParteDecimal;
                                        }

                                        if (celda.EsNumeroTruncado)
                                        {
                                            celda.Valor = MathHelper.TruncateDecimal(celda.Valor.Value, numDigitos);
                                        }
                                        if (celda.EsNumeroRedondeado)
                                        {
                                            celda.Valor = MathHelper.Round(celda.Valor.Value, numDigitos);
                                        }

                                        string strParteDecimal = string.Empty;
                                        for (int i = 1; i <= numDigitos; i++) strParteDecimal += "0";
                                        string strFormat = "#,##0." + strParteDecimal;

                                        nfi.NumberDecimalDigits = numDigitos;
                                    }
                                    else
                                    {
                                        string strFormat = "#,##0";

                                        nfi.NumberDecimalDigits = 0;
                                    }

                                    this.AddRowsTablaWordX(ref secuencia_, filaX + 1, colX, celda.Valor.Value.ToString("N", nfi), alignHorizontal, tieneNegrita, colorFondo, 0, 8, colorTexto);
                                }
                                else
                                {
                                    this.AddRowsTablaWordX(ref secuencia_, filaX + 1, colX, " ", alignHorizontal, tieneNegrita, colorFondo, 0, 8, colorTexto);
                                }
                            }
                            else
                            {
                                //border transparente
                                int tieneBorderEspecial = reg.EsFilaResumen && string.IsNullOrEmpty(celda.Texto) ? 1 : 0;
                                if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt01TotalDatos && fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF) tieneBorderEspecial = 0;
                                if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt09Resarcimiento) tieneBorderEspecial = (colTablaReporte == 7 && string.IsNullOrEmpty(celda.Texto)) ? 2 : 0;

                                celda.Texto = (celda.Texto != null && tablaData.EsMayuscula) ? celda.Texto.ToUpper() : celda.Texto;

                                if (celda.EsTextoFecha)
                                {
                                    string hhmmss = celda.Texto != null && celda.Texto.Length > 8 ? celda.Texto.Substring(celda.Texto.Length - 8, 8) : string.Empty;
                                    string ddmmyyy = celda.Texto != null && celda.Texto.Length > 8 ? celda.Texto.Substring(0, 10).Replace("/", ".") : string.Empty;
                                    this.AddRowsTablaWordX(ref secuencia_, filaX + 1, colX, hhmmss + (celda.TieneFormatoFechaExcel ? "\n" + ddmmyyy : string.Empty), alignHorizontal, tieneNegrita, colorFondo, 0, 8, colorTexto);
                                }
                                else
                                {
                                    int tamanioLetra = 8;
                                    //if (tablaData.ReptiCodiTabla == ConstantesExtranetCTAF.EracmfRpt02Resumen && tablaData.CabeceraColumnas.Count == 3)
                                    //{
                                    //    tamanioLetra = 6;
                                    //}
                                    this.AddRowsTablaWordX(ref secuencia_, filaX + 1, colX, celda.Texto, alignHorizontal, tieneNegrita, colorFondo, tieneBorderEspecial, tamanioLetra, colorTexto);
                                }
                            }
                            colX++;
                            colTablaReporte++;
                        }
                        else
                        {
                            colTablaReporte++;
                        }
                    }

                    //>>>>>>>>>>>>>>Caso malas actuaciones
                    if (reporte == 3)
                    {
                        if (reg.EsAgrupado && reg.ListaCelda[1].Texto == empresa)
                            secuencia_.MergeCellsInColumn(1, filaXAgrupa, filaX + 1);
                        else
                            filaXAgrupa = filaX + 1;
                        empresa = reg.ListaCelda[1].Texto;
                    }
                    //>>>>>>>>>>>>>>

                    filaX++;
                }

            }
        }

        private void AddRowsTablaWordX(ref Table secuencia_, int row, int col, string name, int alignHorizontal, bool tieneNegrita, string color, int tipoBorderEspecial, int tamanioLetra, string colortexto)
        {
            FontFamily fontDoc1 = new FontFamily("Calibri");
            int fontTamanio = tamanioLetra;

            string valorTexto = !string.IsNullOrEmpty(name) ? name : " ";

            secuencia_.Rows[row].Cells[col].VerticalAlignment = VerticalAlignment.Center;

            if (valorTexto.ToUpper() != "ENSf--->".ToUpper())
                secuencia_.Rows[row].Cells[col].Paragraphs[0].Append(valorTexto);
            else
            {
                Novacode.Formatting fotext = new Novacode.Formatting();
                fotext.Script = Script.subscript;

                secuencia_.Rows[row].Cells[col].Paragraphs[0].InsertText("ENS");
                secuencia_.Rows[row].Cells[col].Paragraphs[0].InsertText("F", false, fotext);
                secuencia_.Rows[row].Cells[col].Paragraphs[0].InsertText("--->");
            }

            secuencia_.Rows[row].Cells[col].Paragraphs[0].Alignment = (Alignment)alignHorizontal;
            secuencia_.Rows[row].Cells[col].Paragraphs[0].Font(fontDoc1);
            secuencia_.Rows[row].Cells[col].Paragraphs[0].FontSize(fontTamanio);


            if (tipoBorderEspecial == 1)
            {
                secuencia_.Rows[row].Cells[col].SetBorder(TableCellBorderType.Right, new Border(BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Transparent));
                secuencia_.Rows[row].Cells[col].SetBorder(TableCellBorderType.Bottom, new Border(BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Transparent));
                secuencia_.Rows[row].Cells[col].SetBorder(TableCellBorderType.Left, new Border(BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Transparent));

            }
            if (tipoBorderEspecial == 2)
            {
                secuencia_.Rows[row].Cells[col].SetBorder(TableCellBorderType.Right, new Border(BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Transparent));
                secuencia_.Rows[row].Cells[col].SetBorder(TableCellBorderType.Bottom, new Border(BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Transparent));
                secuencia_.Rows[row].Cells[col].SetBorder(TableCellBorderType.Left, new Border(BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Transparent));
                secuencia_.Rows[row].Cells[col].SetBorder(TableCellBorderType.Top, new Border(BorderStyle.Tcbs_none, BorderSize.one, 1, Color.Transparent));
            }

            if (!string.IsNullOrEmpty(color))
                secuencia_.Rows[row].Cells[col].FillColor = System.Drawing.ColorTranslator.FromHtml(color);

            if (tieneNegrita)
                secuencia_.Rows[row].Cells[col].Paragraphs[0].Bold();

            if (!string.IsNullOrEmpty(colortexto))
                secuencia_.Rows[row].Cells[col].Paragraphs[0].Color(Color.Red);
        }

        private void HeadTablaResumen(ref Table secuencia, List<string> namesColumn, List<double> widthColumn, int nroColumn)
        {
            FontFamily fontDoc1 = new FontFamily("Calibri");
            int fontTamanio = 8;
            string color = "#bfbfbf";
            for (int x = 0; x < nroColumn; x++)
            {
                secuencia.Rows[0].Cells[x].Paragraphs[0].Append(namesColumn[x]);
                this.CentrarCellTableWord(secuencia.Rows[0].Cells[x]);
                secuencia.Rows[0].Cells[x].FillColor = System.Drawing.ColorTranslator.FromHtml(color);
                secuencia.Rows[0].Cells[x].Paragraphs[0].FontSize(fontTamanio);
                secuencia.Rows[0].Cells[x].Paragraphs[0].Font(fontDoc1);
                secuencia.Rows[0].Cells[x].Paragraphs[0].Bold();
            }
        }

        /// <summary>
        /// Agregar sub titulo
        /// </summary>
        /// <param name="p"></param>
        /// <param name="descripcion"></param>
        /// <param name="fontDoc"></param>
        private void AddSubtitulo(ref Paragraph p, string descripcion, FontFamily fontDoc)
        {
            p.AppendLine().Font(fontDoc).AppendLine().Font(fontDoc).Append(descripcion).FontSize(11).Font(fontDoc).Bold().AppendLine().Append(" ").Font(fontDoc);
        }

        /// <summary>
        /// Agregar sub titulo reporte
        /// </summary>
        /// <param name="p"></param>
        /// <param name="descripcion"></param>
        /// <param name="fontDoc"></param>
        public void AddSubtituloReporte(ref Paragraph p, string descripcion, FontFamily fontDoc)
        {
            p.AppendLine().Font(fontDoc).AppendLine().Font(fontDoc).Append(descripcion).FontSize(11).Font(fontDoc).AppendLine().Append(" ").Font(fontDoc).Alignment = Alignment.center;
        }

        /// <summary>
        /// Agregar sub titulo reporte SIN TANTOS ESPACIOS!!!
        /// </summary>
        /// <param name="p"></param>
        /// <param name="descripcion"></param>
        /// <param name="fontDoc"></param>
        public void AddSubtituloReporte2(Paragraph p, string descripcion, FontFamily fontDoc)
        {
            p.Append($"{descripcion}\n").Font(fontDoc).FontSize(11).Alignment = Alignment.center;
        }
        /// <summary>
        /// Centrar celda table Word
        /// </summary>
        /// <param name="cell"></param>
        private void CentrarCellTableWord(Cell cell)
        {
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.Paragraphs[0].Alignment = Alignment.center;
        }

        #endregion

        #region Word Interrrupciones
        /// <summary>
        /// Generar Reporte 2ByInterrupcionWord
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="anio"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public string GenerarReporte2ByInterrupcionWord(int tipoReporte, int afecodi, int emprcodi, string anio, string correlativo)
        {
            bool esPorEracmf = false;
            ///Data del reporte y datos del evento
            int fdatcodi = (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion;
            SiEmpresaDTO objEmpresa = emprcodi > 0 ? FactorySic.GetSiEmpresaRepository().GetById(emprcodi) : null;
            SiFuentedatosDTO objFuenteDatos = FactorySic.GetSiFuentedatosRepository().GetById(fdatcodi);
            EventoDTO regEvento = this.ObtenerInterrupcionByAfecodi(afecodi);

            //Data del reporte
            this.ListarInterrupcionSuministrosGral(afecodi, emprcodi, fdatcodi, out List<AfInterrupSuministroDTO> listaData, out List<AfInterrupSuministroDTO> listDataReportCero, out List<string> listaMsjValidacion, out List<int> listaEmprcodiReportaron, anio, correlativo);

            //Lista horas de coordinacion de normalización
            List<AfHoraCoordDTO> lstHandsonHorasCoordinacion = this.ObtenerListaCruceHoracoordInterrupcion(afecodi, fdatcodi, listaData, anio, correlativo);

            //TOTAL DE DATOS
            this.ListarRptTotalDatos(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte1, out bool formatFechaCab1);
            TablaReporte tablaReporte1 = this.ObtenerDataExcelDatosTotales(listaReporte1, esPorEracmf, formatFechaCab1);

            //MENORES A 3 MINUTOS
            this.ListarRptMenores3Minutos(afecodi, emprcodi, listaData, null, out List<ReporteInterrupcion> listaReporte2, out bool formatFechaCab4);
            TablaReporte tablaReporte2 = this.ObtenerDataExcelMenores3minutos(listaReporte2, esPorEracmf, formatFechaCab4);

            //PARA LA DECISION            
            this.ListarRptDecision(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte3, out bool formatFechaCab3, esPorEracmf);
            TablaReporte tablaReporte3 = this.ObtenerDataExcelDecision(listaReporte3, formatFechaCab3);

            //DEMORAS PARA EL RESTABLECIMIENTO            
            this.ListarRptRestablecimiento(listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte4, out bool formatFechaCab2);
            TablaReporte tablaReporte4 = this.ObtenerDataTablaRptRestablecimiento(listaReporte4, formatFechaCab2);

            //PARA EL RESARCIMIENTO
            this.ListarRptResarcimientoNoERACMF(afecodi, emprcodi, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte5, out bool formatFechaCab5);
            TablaReporte tablaReporte5 = this.ObtenerDataExcelResarcimiento(listaReporte5, formatFechaCab5);

            FontFamily fontDoc = new FontFamily(tablaReporte1.TipoFuente);

            //Plantilla y archivo salida
            string rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesExtranetCTAF.RutaReportes;
            string nombPlantilla = ConstantesExtranetCTAF.PlantillaReporteWordInterrupciones;
            string nombCompletPlantilla = $"{rutaBase}{nombPlantilla}";
            FileInfo archivoPlantilla = new FileInfo(nombCompletPlantilla);

            string rutaOutput = AppDomain.CurrentDomain.BaseDirectory + ConstantesExtranetCTAF.RutaReportes;
            string nombFormato = $"{regEvento.CODIGO}_ReporteInterrupciones.docx";
            string nombCompletFormato = $"{rutaOutput}{nombFormato}";
            FileInfo nuevoArchivo = new FileInfo(nombCompletFormato);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombCompletFormato);
            }

            using (DocX document = DocX.Create(nombCompletFormato))
            {
                #region Header y Footer

                document.MarginLeft = 114.0f;
                document.MarginRight = 114.0f;

                //Cabecera
                document.AddHeaders();


                Novacode.Image logo = document.AddImage(AppDomain.CurrentDomain.BaseDirectory + "Content/Images/" + "Coes.png");
                document.DifferentFirstPage = false;
                document.DifferentOddAndEvenPages = false;

                Header header_first = document.Headers.odd;

                Table header_first_table = header_first.InsertTable(2, 2);
                header_first_table.Design = TableDesign.None;

                //primera fila
                Paragraph upperRightParagraph = header_first.Tables[0].Rows[0].Cells[0].Paragraphs[0];
                upperRightParagraph.AppendPicture(logo.CreatePicture(48, 92));
                upperRightParagraph.Alignment = Alignment.left;
                header_first_table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Top;

                header_first.InsertParagraph().AppendLine().Append(" ").Font(fontDoc);

                //Pie de página
                document.AddFooters();

                Footer footer_main = document.Footers.odd;

                Paragraph pFooter = footer_main.Paragraphs.First();
                pFooter.Alignment = Alignment.left;
                pFooter.Append("Página ");
                pFooter.AppendPageNumber(PageNumberFormat.normal);
                pFooter.Append(" de ");
                pFooter.AppendPageCount(PageNumberFormat.normal);
                #endregion

                #region Título y Evento

                this.GenerarTituloWord(document, ConstantesExtranetCTAF.RptTipoFuente, objEmpresa, objFuenteDatos);

                this.GenerarTablaEventoWord(document, regEvento, ConstantesExtranetCTAF.RptTipoFuente, ConstantesExtranetCTAF.RptColor, ConstantesExtranetCTAF.RptTamanio);

                #endregion

                #region REPORTE DATOS TOTALES
                Paragraph p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "01.Total de Datos", fontDoc);

                List<CabeceraReporteColumna> dataCab = new List<CabeceraReporteColumna>();

                List<RegistroReporte> registros = new List<RegistroReporte>();
                List<RegistroReporte> registrosTotales = new List<RegistroReporte>();
                TablaReporte Tabla = new TablaReporte();


                registrosTotales = tablaReporte1.ListaRegistros;
                int numFilas;
                int numColumnas;
                Table secuencia_0;

                var codigosEventos = tablaReporte1.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                foreach (var item in codigosEventos)
                {
                    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtituloReporte(ref p, registros[0].Nombre, fontDoc);
                    Tabla = tablaReporte1;
                    Tabla.ListaRegistros = registros;
                    numFilas = registros.Count;
                    numColumnas = tablaReporte1.CabeceraColumnas.Count;
                    secuencia_0 = document.InsertTable(numFilas + 1, numColumnas);

                    GenerarRptWord(ref secuencia_0, tablaReporte1, 12, fdatcodi);

                }



                #endregion

                #region REPORTE DEMORAS EN EL RESTABLECIMIENTO
                p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "02. Demoras en el restablecimiento", fontDoc);

                dataCab = tablaReporte4.CabeceraColumnas;
                registros = tablaReporte4.ListaRegistros;



                Table secuencia_2;
                registrosTotales = tablaReporte4.ListaRegistros;
                var codigosEventosRestablecimiento = tablaReporte4.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                foreach (var item in codigosEventosRestablecimiento)
                {
                    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtituloReporte(ref p, registros[0].Nombre, fontDoc);
                    Tabla = tablaReporte4;
                    Tabla.ListaRegistros = registros;
                    numFilas = registros.Count;
                    numColumnas = tablaReporte4.CabeceraColumnas.Count;
                    secuencia_2 = document.InsertTable(numFilas + 2, numColumnas);

                    this.GenerarRptWord(ref secuencia_2, tablaReporte4, 13, fdatcodi);

                }

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                // this.GenerarRptWord(ref secuencia_2, tablaReporte4, 13, fdatcodi);
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                #endregion

                #region REPORTE MENORES A 3 MINUTOS
                p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "03. Menores a 3 minutos", fontDoc);

                //dataCab = tablaReporte2.CabeceraColumnas;
                //registros = tablaReporte2.ListaRegistros;

                ////numFilas = registros.Count;
                ////numColumnas = tablaReporte2.CabeceraColumnas.Count;
                ////Table secuencia_3 = document.InsertTable(numFilas + 1, numColumnas);

                //dataCab = tablaReporte4.CabeceraColumnas;
                //registros = tablaReporte4.ListaRegistros;

                Table secuencia_3;
                registrosTotales = tablaReporte2.ListaRegistros;
                var codigosEventosMenores = tablaReporte2.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                foreach (var item in codigosEventosMenores)
                {
                    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtituloReporte(ref p, registros[0].Nombre, fontDoc);
                    Tabla = tablaReporte2;
                    Tabla.ListaRegistros = registros;
                    numFilas = registros.Count;
                    numColumnas = tablaReporte4.CabeceraColumnas.Count;
                    secuencia_3 = document.InsertTable(numFilas + 1, numColumnas);

                    this.GenerarRptWord(ref secuencia_3, tablaReporte2, 14, fdatcodi);

                }

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //this.GenerarRptWord(ref secuencia_3, tablaReporte2, 14, fdatcodi);
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                #endregion

                #region REPORTE DECISIÓN
                p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "04. Decisión", fontDoc);

                Table secuencia_4;
                registrosTotales = tablaReporte3.ListaRegistros;
                var codigosEventosDecision = tablaReporte3.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                foreach (var item in codigosEventosDecision)
                {
                    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    p = document.InsertParagraph().Font(fontDoc);
                    int nroFila = 1;
                    foreach(var item2 in registros)
                    {
                        item2.ListaCelda[0].Fila = nroFila;
                        item2.ListaCelda[0].Valor = nroFila;
                        nroFila++;
                    }


                    this.AddSubtituloReporte(ref p, registros[0].Nombre, fontDoc);
                    Tabla = tablaReporte3;
                    Tabla.ListaRegistros = registros;
                    numFilas = registros.Count;
                    numColumnas = tablaReporte3.CabeceraColumnas.Count;
                    secuencia_4 = document.InsertTable(numFilas + 1, numColumnas);

                    this.GenerarRptWord(ref secuencia_4, tablaReporte3, 15, fdatcodi);

                }


                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //this.GenerarRptWord(ref secuencia_4, tablaReporte3, 15, fdatcodi);
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                #endregion

                #region REPORTE RESARCIMIENTO
                p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "05. Resarcimiento", fontDoc);

                //dataCab = tablaReporte5.CabeceraColumnas;
                //registros = tablaReporte5.ListaRegistros;

                //numFilas = registros.Count;
                //numColumnas = tablaReporte5.CabeceraColumnas.Count;
                Table secuencia_5; //= document.InsertTable(numFilas + 1, numColumnas);
                registrosTotales = tablaReporte5.ListaRegistros;

                var codigosEventosRearcimiento = tablaReporte5.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                foreach (var item in codigosEventosRearcimiento)
                {
                    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    p = document.InsertParagraph().Font(fontDoc);

                    decimal _valorTotal_mw = 0, _valorTotal_mwh = 0;
                    for (int x = 0; x < registros.Count; x++)
                    {
                        if(x < registros.Count - 1)
                        {
                            _valorTotal_mw += (decimal)registros[x].ListaCelda[1].Valor;
                            _valorTotal_mwh += (decimal)registros[x].ListaCelda[6].Valor;
                        }
                            
                        if (x == registros.Count - 1)
                        {
                            registros[x].ListaCelda[1].Valor = _valorTotal_mw;
                            registros[x].ListaCelda[6].Valor = _valorTotal_mwh;
                        }
                            
                    }

                    this.AddSubtituloReporte(ref p, registros[0].Nombre, fontDoc);
                    Tabla = tablaReporte5;
                    Tabla.ListaRegistros = registros;
                    numFilas = registros.Count;
                    numColumnas = tablaReporte5.CabeceraColumnas.Count;
                    secuencia_5 = document.InsertTable(numFilas + 1, numColumnas);
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    this.GenerarRptWord(ref secuencia_5, tablaReporte5, 16, fdatcodi);
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                }


                #endregion

                document.SaveAs(nombCompletFormato);
            }
            return nombFormato;

        }

        #endregion

        #region Exportación Word - Reducción de Suministros
        /// <summary>
        /// Generar Reporte 3ByReduccionWord
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="anio"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public string GenerarReporte3ByReduccionWord(int tipoReporte, int afecodi, int emprcodi, string anio, string correlativo)
        {
            //Data del reporte y datos del evento
            int fdatcodi = (int)ConstantesExtranetCTAF.Fuentedato.ReduccionSuministros;
            SiEmpresaDTO objEmpresa = emprcodi > 0 ? FactorySic.GetSiEmpresaRepository().GetById(emprcodi) : null;
            SiFuentedatosDTO objFuenteDatos = FactorySic.GetSiFuentedatosRepository().GetById(fdatcodi);
            EventoDTO regEvento = this.ObtenerInterrupcionByAfecodi(afecodi);

            //Data del reporte
            this.ListarInterrupcionSuministrosGral(afecodi, emprcodi, fdatcodi, out List<AfInterrupSuministroDTO> listaData, out List<AfInterrupSuministroDTO> listDataReportCero, out List<string> listaMsjValidacion, out List<int> listaEmprcodiReportaron, anio, correlativo);

            this.ListarRpt7ReduccionDeSuministro(afecodi, emprcodi, listaData, out List<ReporteInterrupcion> listaReporte, out bool formatFechaCab1);
            TablaReporte tablaReporte7 = this.ObtenerDataExcelReduccionSuministros(listaReporte, formatFechaCab1);

            FontFamily fontDoc = new FontFamily(tablaReporte7.TipoFuente);

            //Plantilla y archivo salida
            string rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesExtranetCTAF.RutaReportes;
            string nombPlantilla = ConstantesExtranetCTAF.PlantillaReporteWordReduccionDeSuministros;
            string nombCompletPlantilla = $"{rutaBase}{nombPlantilla}";
            FileInfo archivoPlantilla = new FileInfo(nombCompletPlantilla);

            string rutaOutput = AppDomain.CurrentDomain.BaseDirectory + ConstantesExtranetCTAF.RutaReportes;
            string nombFormato = $"{regEvento.CODIGO}_ReporteReducciónDeSuministros.docx";
            string nombCompletFormato = $"{rutaOutput}{nombFormato}";
            FileInfo nuevoArchivo = new FileInfo(nombCompletFormato);

            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombCompletFormato);
            }

            using (DocX document = DocX.Create(nombCompletFormato))
            {
                #region Header y Footer

                document.MarginLeft = 114.0f;
                document.MarginRight = 114.0f;

                //Cabecera
                document.AddHeaders();

                Novacode.Image logo = document.AddImage(AppDomain.CurrentDomain.BaseDirectory + "Content/Images/" + "Coes.png");
                document.DifferentFirstPage = false;
                document.DifferentOddAndEvenPages = false;

                Header header_first = document.Headers.odd;

                Table header_first_table = header_first.InsertTable(2, 2);
                header_first_table.Design = TableDesign.None;

                //primera fila
                Paragraph upperRightParagraph = header_first.Tables[0].Rows[0].Cells[0].Paragraphs[0];
                upperRightParagraph.AppendPicture(logo.CreatePicture(48, 92));
                upperRightParagraph.Alignment = Alignment.left;
                header_first_table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Top;

                header_first.InsertParagraph().AppendLine().Append(" ").Font(fontDoc);

                //Pie de página
                document.AddFooters();

                Footer footer_main = document.Footers.odd;

                Paragraph pFooter = footer_main.Paragraphs.First();
                pFooter.Alignment = Alignment.left;
                pFooter.Append("Página ");
                pFooter.AppendPageNumber(PageNumberFormat.normal);
                pFooter.Append(" de ");
                pFooter.AppendPageCount(PageNumberFormat.normal);
                #endregion

                #region Título y Evento

                this.GenerarTituloWord(document, tablaReporte7.TipoFuente, objEmpresa, objFuenteDatos);

                this.GenerarTablaEventoWord(document, regEvento, ConstantesExtranetCTAF.RptTipoFuente, ConstantesExtranetCTAF.RptColor, ConstantesExtranetCTAF.RptTamanio);

                #endregion

                #region REPORTE DATOS TOTALES
                Paragraph p = document.InsertParagraph().Font(fontDoc);
                this.AddSubtitulo(ref p, "01. Reducción de suministros", fontDoc);
                TablaReporte Tabla = new TablaReporte();
                //var dataCab = tablaReporte7.CabeceraColumnas;
                //var registros = tablaReporte7.ListaRegistros;

                int numFilas;
                int numColumnas;

                List<RegistroReporte> registros = new List<RegistroReporte>();
                List<RegistroReporte> registrosTotales = new List<RegistroReporte>();
                var codigosEventos = tablaReporte7.ListaRegistros.Select(y => new { y.codigo }).Distinct().ToList();
                registrosTotales = tablaReporte7.ListaRegistros;
                foreach (var item in codigosEventos)
                {
                    registros = registrosTotales.Where(x => x.codigo == item.codigo || x.codigo == null).OrderByDescending(c => c.codigo).ToList();

                    p = document.InsertParagraph().Font(fontDoc);
                    this.AddSubtituloReporte(ref p, registros[0].Nombre, fontDoc);
                    Tabla = tablaReporte7;
                    Tabla.ListaRegistros = registros;
                    numFilas = registros.Count;
                    numColumnas = tablaReporte7.CabeceraColumnas.Count;
                    Table secuencia_0 = document.InsertTable(numFilas + 1, numColumnas);

                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    GenerarRptWord(ref secuencia_0, tablaReporte7, 16, fdatcodi);
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                }



                #endregion

                document.SaveAs(nombCompletFormato);
            }
            return nombFormato;

        }

        #endregion

        #region  Obtencion TablaReporte para Excel

        /// <summary>
        /// Obtiene listas para armar el Excel DatosTotales
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="esPorEracmf"></param>
        /// <param name="formatFechaCab"></param>
        /// <returns></returns>
        public TablaReporte ObtenerDataExcelDatosTotales(List<ReporteInterrupcion> listaReporte, bool esPorEracmf, bool formatFechaCab)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = ConstantesExtranetCTAF.EracmfRpt01TotalDatos;

            //Caracteristicas generales del reporte
            tabla.TamLetra = 8;
            tabla.TipoFuente = "Calibri";
            tabla.Color = "#bfbfbf";
            tabla.EsMayuscula = true;
            tabla.ColorERACMF = "#FB7A57";
            //

            #region cabecera
            CabeceraReporte cabRepo = new CabeceraReporte();

            if (esPorEracmf)
            {
                tabla.AltoFilaCab = 0.7M;
                List<CabeceraReporteColumna> columnasCabecera = new List<CabeceraReporteColumna>();

                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "ZONA", AnchoColumna = 1.23M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "SUMINISTRO", AnchoColumna = 5.13M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "SUBESTACIÓN", AnchoColumna = 2.93M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "POTENCIA (MW)", AnchoColumna = 1.57M });

                if (formatFechaCab)
                {
                    tabla.AltoFilaCab = 1.24M;
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "INICIO (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 1.85M });
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FINAL (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 1.85M });
                }
                else
                {
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "INICIO (HH:MM:SS)", AnchoColumna = 1.64M });
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FINAL (HH:MM:SS)", AnchoColumna = 1.64M });
                }
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "DURACIÓN (MIN)", AnchoColumna = 1.50M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FUNCIÓN", AnchoColumna = 1.29M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "ETAPA", AnchoColumna = 1.37M });

                tabla.CabeceraColumnas = columnasCabecera;
            }
            else
            {
                tabla.AltoFilaCab = 0.7M;
                List<CabeceraReporteColumna> columnasCabecera = new List<CabeceraReporteColumna>();
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "SUMINISTRO", AnchoColumna = 5.13M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "SUBESTACIÓN", AnchoColumna = 2.93M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "POTENCIA (MW)", AnchoColumna = 1.57M });

                if (formatFechaCab)
                {
                    tabla.AltoFilaCab = 1.24M;
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "INICIO (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 1.85M });
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FINAL (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 1.85M });
                }
                else
                {
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "INICIO (HH:MM:SS)", AnchoColumna = 1.64M });
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FINAL (HH:MM:SS)", AnchoColumna = 1.64M });
                }
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "DURACIÓN (MIN)", AnchoColumna = 1.76M });

                tabla.CabeceraColumnas = columnasCabecera;
            }

            tabla.Cabecera = cabRepo;

            #endregion

            #region cuerpo

            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();
            List<ReporteInterrupcion> lista = new List<ReporteInterrupcion>();


            var codigosEventos = listaData.Select(y => new { y.Evencodi }).Distinct().ToList();
            var listaDataCompleta = listaData;
            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();
            foreach (var item in codigosEventos)
            {
                lista = listaDataCompleta.Where(x => x.Evencodi == item.Evencodi).OrderByDescending(c => c.Evencodi).ToList();



                foreach (var reg in lista)
                {
                    RegistroReporte registro = new RegistroReporte();
                    List<CeldaReporte> datos = new List<CeldaReporte>();
                    registro.AltoFila = 0.5M;

                    var tieneFormatFecha = formatFechaCab;

                    if (esPorEracmf)
                    {
                        datos.Add(this.GetCeldaEspecialZona("ZONA " + reg.Zona, false, false));
                        datos.Add(this.GetCeldaEspecialSuministro(reg.Suministro, false, false, false, true, reg.IdEmpresa));
                        datos.Add(this.GetCeldaEspecialSE(reg.Subestacion, false, false));
                        datos.Add(this.GetCeldaEspecialMW(reg.Potencia, false, false, false));
                        datos.Add(this.GetCeldaEspecialHora(reg.HoraInicio, false, false, false, tieneFormatFecha));
                        datos.Add(this.GetCeldaEspecialHora(reg.HoraFinal, false, false, false, tieneFormatFecha));
                        datos.Add(this.GetCeldaEspecialDuracion(reg.Duracion, false, false, false));
                        datos.Add(this.GetCeldaEspecialFuncion((reg.Funcion != null ? reg.Funcion.ToUpper() : string.Empty), false, false));
                        datos.Add(this.GetCeldaEspecialEtapa("ETAPA " + reg.Etapa, false, false));
                    }
                    else
                    {
                        datos.Add(this.GetCeldaEspecialSuministro(reg.Suministro, false, false));
                        datos.Add(this.GetCeldaEspecialSE(reg.Subestacion, false, false));
                        datos.Add(this.GetCeldaEspecialMW(reg.Potencia, false, false, false));
                        datos.Add(this.GetCeldaEspecialHora(reg.HoraInicio, false, false, false, tieneFormatFecha));
                        datos.Add(this.GetCeldaEspecialHora(reg.HoraFinal, false, false, false, tieneFormatFecha));
                        datos.Add(this.GetCeldaEspecialDuracion(reg.Duracion, false, false, false));
                    }

                    registro.ListaCelda = datos;
                    registro.Nombre = string.Format("Hora Inicio:{0}", reg.Evenini);
                    registro.codigo = reg.Evencodi;
                    registro.Emprcodi = reg.IdEmpresa;
                    registros.Add(registro);
                }

                //Totales
                RegistroReporte registro0 = new RegistroReporte();
                registro0.AltoFila = 0.5M;
                registro0.EsFilaResumen = true;
                registro0.EsAgrupado = true;
                List<CeldaReporte> datos0 = new List<CeldaReporte>();
                ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal && x.Evencodi == item.Evencodi);

                if (esPorEracmf)
                {
                    datos0.Add(this.GetCeldaEspecialZona("TOTAL", true, false, false));
                    datos0.Add(this.GetCeldaEspecialSuministro(null, true, false, true, true));
                    datos0.Add(this.GetCeldaEspecialTotal(null, true, false, true));

                    datos0.Add(this.GetCeldaEspecialMW(regTotal.Potencia, true, false, false));

                    datos0.Add(this.GetCeldaEspecialHora(string.Empty, true, false));
                    datos0.Add(this.GetCeldaEspecialHora(string.Empty, true, false));

                    datos0.Add(this.GetCeldaEspecialDuracion(null, true, false, false));
                    datos0.Add(this.GetCeldaEspecialFuncion(null, true, false));
                    datos0.Add(this.GetCeldaEspecialEtapa(null, true, false));
                }
                else
                {
                    datos0.Add(this.GetCeldaEspecialZona("TOTAL", true, false, false));
                    datos0.Add(this.GetCeldaEspecialTotal(null, true, false, true));

                    datos0.Add(this.GetCeldaEspecialMW(regTotal.Potencia, true, false, false));
                    datos0.Add(this.GetCeldaEspecialFilaVacia());
                    datos0.Add(this.GetCeldaEspecialFilaVacia());
                    datos0.Add(this.GetCeldaEspecialFilaVacia());
                }

                registro0.ListaCelda = datos0;
                registro0.codigo = regTotal.Evencodi;
                registros.Add(registro0);
            }
            tabla.ListaRegistros = registros;

            #endregion
            return tabla;
        }

        /// <summary>
        /// Obtiene listas para armar el Excel Resumen
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataExcelResumen(List<ReporteInterrupcion> listaReporte)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = ConstantesExtranetCTAF.EracmfRpt02Resumen;

            //Caracteristicas generales del reporte
            tabla.TamLetra = 8;
            tabla.TipoFuente = "Calibri";
            tabla.Color = "#bfbfbf";
            tabla.EsMayuscula = true;
            tabla.ColorERACMF = "#FB7A57";

            #region cabecera
            tabla.AltoFilaCab = 0.5M;
            List<CabeceraReporteColumna> columnasCabecera = new List<CabeceraReporteColumna>();

            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "ZONA", AnchoColumna = 1.24M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "EMPRESA", AnchoColumna = 1.50M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "1°", AnchoColumna = 1.32M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "2°", AnchoColumna = 1.32M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "3°", AnchoColumna = 1.24M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "4°", AnchoColumna = 1.24M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "5°", AnchoColumna = 1.24M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "6°", AnchoColumna = 1M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "7° (Reposición)", AnchoColumna = 1.69M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "1°", AnchoColumna = 1.11M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "2°", AnchoColumna = 1M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "3°", AnchoColumna = 1M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "TOTAL ZONA (MW)", AnchoColumna = 2.06M });

            tabla.CabeceraColumnas = columnasCabecera;

            #endregion

            #region cuerpo

            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();
            //ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal);
            List<ReporteInterrupcion> lista;

            var codigosEventos = listaData.Select(y => new { y.Evencodi }).Distinct().ToList();
            var listaDataCompleta = listaData;
            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();
            foreach (var item in codigosEventos)
            {
                lista = listaDataCompleta.Where(x => x.Evencodi == item.Evencodi).OrderByDescending(c => c.Evencodi).ToList();

                foreach (var reg in lista)
                {
                    RegistroReporte registro = new RegistroReporte();
                    List<CeldaReporte> datos = new List<CeldaReporte>();
                    registro.AltoFila = 0.51M;

                    datos.Add(this.GetCeldaEspecialZona("Zona " + reg.Zona, false, false));
                    datos.Add(this.GetCeldaEspecialEmpresa(reg.NombEmpresa, false, false, true, reg.IdEmpresa));
                    datos.Add(this.GetCeldaEspecialMW((reg.Etapaf1), false, false, reg.ColorEtapaf1));
                    datos.Add(this.GetCeldaEspecialMW((reg.Etapaf2), false, false, reg.ColorEtapaf2));
                    datos.Add(this.GetCeldaEspecialMW((reg.Etapaf3), false, false, reg.ColorEtapaf3));
                    datos.Add(this.GetCeldaEspecialMW((reg.Etapaf4), false, false, reg.ColorEtapaf4));
                    datos.Add(this.GetCeldaEspecialMW((reg.Etapaf5), false, false, reg.ColorEtapaf5));
                    datos.Add(this.GetCeldaEspecialMW((reg.Etapaf6), false, false, reg.ColorEtapaf6));
                    datos.Add(this.GetCeldaEspecialMW((reg.Etapaf7), false, false, reg.ColorEtapaf7));
                    datos.Add(this.GetCeldaEspecialMW((reg.EtapaDf1), false, false, reg.ColorEtapaDf1));
                    datos.Add(this.GetCeldaEspecialMW((reg.EtapaDf2), false, false, reg.ColorEtapaDf2));
                    datos.Add(this.GetCeldaEspecialMW((reg.EtapaDf3), false, false, reg.ColorEtapaDf3));

                    datos.Add(this.GetCeldaEspecialMW(reg.TotalZona, false, false, false));
                    registro.codigo = reg.Evencodi;
                    registro.ListaCelda = datos;
                    registro.Nombre = string.Format("Hora Inicio:{0}", reg.Evenini);
                    registros.Add(registro);
                }

                //Totales
                RegistroReporte registro0 = new RegistroReporte();
                registro0.EsFilaResumen = true;
                registro0.AltoFila = 0.95M;
                registro0.EsAgrupado = true;
                List<CeldaReporte> datos0 = new List<CeldaReporte>();
                ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal && x.Evencodi == item.Evencodi);
                datos0.Add(this.GetCeldaEspecialZona("TOTAL ETAPA", true, false, false));
                datos0.Add(this.GetCeldaEspecialFilaVacia(true));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaf1, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaf2, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaf3, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaf4, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaf5, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaf6, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaf7, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaDf1, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaDf2, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaEtapaDf3, true, true, false));
                datos0.Add(this.GetCeldaEspecialMW(regTotal.SumaTotal, true, true, false));

                registro0.ListaCelda = datos0;
                registro0.codigo = regTotal.Evencodi;
                registro0.codigo = regTotal.Evencodi;
                registros.Add(registro0);

                tabla.ListaRegistros = registros;
            }
            #endregion
            return tabla;

        }


        /// <summary>
        /// Obtiene listas para armar el Excel Resumen
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataExcelResumenLeyenda(List<ReporteInterrupcion> listaReporte)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = ConstantesExtranetCTAF.EracmfRpt02Resumen;

            //Caracteristicas generales del reporte
            tabla.TamLetra = 8;
            tabla.TipoFuente = "Calibri";
            tabla.Color = "#bfbfbf";
            tabla.EsMayuscula = true;

            #region cabecera
            tabla.AltoFilaCab = 0.5M;
            List<CabeceraReporteColumna> columnasCabecera = new List<CabeceraReporteColumna>();

            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "", AnchoColumna = 6M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "", AnchoColumna = 6M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "", AnchoColumna = 6M });

            tabla.CabeceraColumnas = columnasCabecera;

            #endregion

            #region cuerpo

            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();
            List<ReporteInterrupcion> lista;
            ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal);

            var codigosEventos = listaData.Select(y => new { y.Evencodi }).Distinct().ToList();
            var listaDataCompleta = listaData;

            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();
            foreach (var item in codigosEventos)
            {
                lista = listaDataCompleta.Where(x => x.Evencodi == item.Evencodi).OrderByDescending(c => c.Evencodi).ToList();



                for (int i = 0; i < lista.Count; i++)
                {
                    RegistroReporte registro = new RegistroReporte();
                    List<CeldaReporte> datos = new List<CeldaReporte>();
                    ReporteInterrupcion reg;
                    registro.AltoFila = 0.51M;
                    reg = lista[i];
                    //datos.Add(this.GetCeldaEspecialZona("Zona " + reg.Zona, false, false));
                    //datos.Add(this.GetCeldaEspecialEmpresa(reg.NombEmpresa, false, false));
                    datos.Add(this.GetCeldaEspecialCodigoNombreEmpresa((reg.CodigoNombreEmpresa), false, false));
                    i++;
                    if (i >= lista.Count)
                    {
                        registro.ListaCelda = datos;
                        registro.codigo = reg.Evencodi;
                        registros.Add(registro);
                        break;
                    }

                    reg = lista[i];

                    datos.Add(this.GetCeldaEspecialCodigoNombreEmpresa((reg.CodigoNombreEmpresa), false, false));
                    i++;
                    if (i >= lista.Count)
                    {
                        registro.ListaCelda = datos;
                        registro.codigo = reg.Evencodi;
                        registros.Add(registro);
                        break;
                    }
                    reg = lista[i];
                    datos.Add(this.GetCeldaEspecialCodigoNombreEmpresa((reg.CodigoNombreEmpresa), false, false));

                    //datos.Add(this.GetCeldaEspecialMW((reg.Etapaf4), false, false, reg.ColorEtapaf4));
                    //datos.Add(this.GetCeldaEspecialMW((reg.Etapaf5), false, false, reg.ColorEtapaf5));
                    //datos.Add(this.GetCeldaEspecialMW((reg.Etapaf6), false, false, reg.ColorEtapaf6));
                    //datos.Add(this.GetCeldaEspecialMW((reg.Etapaf7), false, false, reg.ColorEtapaf7));
                    //datos.Add(this.GetCeldaEspecialMW((reg.EtapaDf1), false, false, reg.ColorEtapaDf1));
                    //datos.Add(this.GetCeldaEspecialMW((reg.EtapaDf2), false, false, reg.ColorEtapaDf2));
                    //datos.Add(this.GetCeldaEspecialMW((reg.EtapaDf3), false, false, reg.ColorEtapaDf3));

                    //datos.Add(this.GetCeldaEspecialMW(reg.TotalZona, false, false, false));

                    registro.codigo = reg.Evencodi;
                    registro.ListaCelda = datos;

                    registros.Add(registro);
                }
            }

            //foreach (var reg in listaData)
            //{

            //}

            tabla.ListaRegistros = registros;

            #endregion
            return tabla;
        }

        /// <summary>
        /// Obtiene listas para armar el Excel DatosTotales MalasActuaciones
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="tabHoraCoord"></param>
        /// <param name="esPorEracmf"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataExcelMalasActuaciones(List<ReporteInterrupcion> listaReporte)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = ConstantesExtranetCTAF.EracmfRpt03MalasActuaciones;

            //Caracteristicas generales del reporte
            tabla.TamLetra = 8;
            tabla.TipoFuente = "Calibri";
            tabla.Color = "#bfbfbf";
            tabla.EsMayuscula = true;
            tabla.ColorERACMF = "#FB7A57";

            #region cabecera
            tabla.AltoFilaCab = 0.5M;
            List<CabeceraReporteColumna> columnasCabecera = new List<CabeceraReporteColumna>();

            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "ZONA", AnchoColumna = 1.12M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "EMPRESA", AnchoColumna = 3.42M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "SUMINISTRO", AnchoColumna = 6.44M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "SUBESTACIÓN", AnchoColumna = 2.21M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FUNCIÓN", AnchoColumna = 1.55M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "ETAPA", AnchoColumna = 1.27M });
            //cabRepo.CabeceraData = matrizCabecera;
            tabla.CabeceraColumnas = columnasCabecera;

            #endregion

            #region cuerpo

            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();
            ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal);

            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();

            //ordenar lista
            listaData = listaData.OrderBy(x => x.Zona).ThenBy(x => x.NombEmpresa).ThenBy(x => x.Suministro).ToList();

            var empresa = listaData.Count > 0 ? listaData.First().NombEmpresa : "";


            foreach (var reg in listaData)
            {
                RegistroReporte registro = new RegistroReporte();
                List<CeldaReporte> datos = new List<CeldaReporte>();
                registro.AltoFila = 0.5M;

                registro.EsAgrupado = reg.NombEmpresa == empresa ? true : false;

                datos.Add(this.GetCeldaEspecialZona("Zona " + reg.Zona, false, false));
                datos.Add(this.GetCeldaEspecialEmpresa(reg.NombEmpresa, false, false, true, reg.IdEmpresa));
                datos.Add(this.GetCeldaEspecialSuministro(reg.Suministro, false, false));
                datos.Add(this.GetCeldaEspecialSE(reg.Subestacion, false, false));
                datos.Add(this.GetCeldaEspecialFuncion((reg.Funcion != null ? (reg.Funcion.ToUpper() == "F" ? "f" : (reg.Funcion.ToUpper() == "DF" ? "Df" : reg.Funcion)) : ""), false, false));
                datos.Add(this.GetCeldaEspecialEtapa("Etapa " + reg.Etapa, false, false));

                empresa = reg.NombEmpresa;

                registro.ListaCelda = datos;
                registro.Nombre = string.Format("Hora Inicio:{0}", reg.Evenini);
                registro.codigo = reg.Evencodi;
                registros.Add(registro);
            }

            tabla.ListaRegistros = registros;

            #endregion
            return tabla;
        }

        /// <summary>
        /// Obtiene listas para armar el Excel Menores3minutos
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="tabHoraCoord"></param>
        /// <param name="esPorEracmf"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataExcelMenores3minutos(List<ReporteInterrupcion> listaReporte, bool esPorEracmf, bool formatFechaCab)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = ConstantesExtranetCTAF.EracmfRpt04Menores3Min;

            //Caracteristicas generales del reporte
            tabla.TamLetra = 8;
            tabla.TipoFuente = "Calibri";
            tabla.Color = "#bfbfbf";
            tabla.EsMayuscula = true;
            tabla.ColorERACMF = "#FB7A57";
            //

            #region cabecera
            CabeceraReporte cabRepo = new CabeceraReporte();

            if (esPorEracmf)
            {
                tabla.AltoFilaCab = 0.7M;
                List<CabeceraReporteColumna> columnasCabecera = new List<CabeceraReporteColumna>();

                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "ZONA", AnchoColumna = 1.21M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "SUMINISTRO", AnchoColumna = 8.01M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "POTENCIA (MW)", AnchoColumna = 1.7M });

                if (formatFechaCab)
                {
                    tabla.AltoFilaCab = 1.24M;
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "INICIO (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 1.85M });
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FINAL (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 1.85M });
                }
                else
                {
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "INICIO (HH:MM:SS)", AnchoColumna = 1.69M });
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FINAL (HH:MM:SS)", AnchoColumna = 1.69M });
                }
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "DURACIÓN (MIN)", AnchoColumna = 1.71M });

                tabla.CabeceraColumnas = columnasCabecera;
            }
            else
            {
                tabla.AltoFilaCab = 0.7M;
                List<CabeceraReporteColumna> columnasCabecera = new List<CabeceraReporteColumna>();
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "SUMINISTRO", AnchoColumna = 8.11M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "POTENCIA (MW)", AnchoColumna = 1.7M });

                if (formatFechaCab)
                {
                    tabla.AltoFilaCab = 1.24M;
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "INICIO (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 1.85M });
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FINAL (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 1.85M });
                }
                else
                {
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "INICIO (HH:MM:SS)", AnchoColumna = 1.69M });
                    columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FINAL (HH:MM:SS)", AnchoColumna = 1.69M });
                }
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "DURACIÓN (MIN)", AnchoColumna = 1.71M });
                tabla.CabeceraColumnas = columnasCabecera;
            }

            tabla.Cabecera = cabRepo;

            #endregion

            #region cuerpo

            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();

            List<ReporteInterrupcion> lista = new List<ReporteInterrupcion>();
            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();
            var codigosEventos = listaData.Select(y => new { y.Evencodi }).Distinct().ToList();
            var listaDataCompleta = listaData;

            foreach (var item in codigosEventos)
            {
                lista = listaDataCompleta.Where(x => x.Evencodi == item.Evencodi).OrderByDescending(c => c.Evencodi).ToList();

                //ordenar lista
                lista = lista.OrderBy(x => x.Zona).ThenBy(x => x.Suministro).ToList();

                if (listaData.Any())
                {
                    foreach (var reg in lista)
                    {
                        bool tieneDemora = reg.TieneDemoraCeldaPintada;

                        RegistroReporte registro = new RegistroReporte();
                        List<CeldaReporte> datos = new List<CeldaReporte>();
                        registro.AltoFila = 0.5M;

                        var tieneFormatFecha = formatFechaCab;

                        if (esPorEracmf)
                        {
                            datos.Add(this.GetCeldaEspecialZona("ZONA " + reg.Zona, false, false));
                            datos.Add(this.GetCeldaEspecialSuministro(reg.Suministro, false, false, false, true, reg.IdEmpresa));
                            datos.Add(this.GetCeldaEspecialMW(reg.Potencia, false, false, false));
                            datos.Add(this.GetCeldaEspecialHora(reg.HoraInicio, false, false, false, tieneFormatFecha));
                            datos.Add(this.GetCeldaEspecialHora(reg.HoraFinal, false, false, false, tieneFormatFecha));
                            datos.Add(this.GetCeldaEspecialDuracion(reg.Duracion, false, false, tieneDemora));
                        }
                        else
                        {
                            datos.Add(this.GetCeldaEspecialSuministro(reg.Suministro, false, false));
                            datos.Add(this.GetCeldaEspecialMW(reg.Potencia, false, false, false));
                            datos.Add(this.GetCeldaEspecialHora(reg.HoraInicio, false, false, false, tieneFormatFecha));
                            datos.Add(this.GetCeldaEspecialHora(reg.HoraFinal, false, false, false, tieneFormatFecha));
                            datos.Add(this.GetCeldaEspecialDuracion(reg.Duracion, false, false, tieneDemora));
                        }

                        registro.ListaCelda = datos;
                        registro.Nombre = string.Format("Hora Inicio:{0}", reg.Evenini);
                        registro.codigo = reg.Evencodi;
                        registros.Add(registro);
                    }

                    //Totales
                    RegistroReporte registro0 = new RegistroReporte();
                    registro0.AltoFila = 0.5M;
                    registro0.EsFilaResumen = true;
                    registro0.EsAgrupado = true;
                    List<CeldaReporte> datos0 = new List<CeldaReporte>();
                    ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal && x.Evencodi == item.Evencodi);
                    if (esPorEracmf)
                    {
                        datos0.Add(this.GetCeldaEspecialZona("TOTAL", true, false, false));
                        datos0.Add(this.GetCeldaEspecialTotal(null, true, false, true));
                        datos0.Add(this.GetCeldaEspecialMW(regTotal.Potencia, true, true, false, false));
                        datos0.Add(this.GetCeldaEspecialFilaVacia());
                        datos0.Add(this.GetCeldaEspecialFilaVacia());
                        datos0.Add(this.GetCeldaEspecialFilaVacia());
                    }
                    else
                    {
                        datos0.Add(this.GetCeldaEspecialZona("TOTAL", true, false, false));
                        datos0.Add(this.GetCeldaEspecialMW(regTotal.Potencia, true, true, false, false));
                        datos0.Add(this.GetCeldaEspecialFilaVacia());
                        datos0.Add(this.GetCeldaEspecialFilaVacia());
                        datos0.Add(this.GetCeldaEspecialFilaVacia());
                    }

                    registro0.ListaCelda = datos0;
                    registro0.codigo = regTotal.Evencodi;
                    registros.Add(registro0);
                }

            }
            tabla.ListaRegistros = registros;

            #endregion
            return tabla;
        }

        /// <summary>
        /// Obtiene listas para formas el Excel No reportaron informacion 
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataExcelNoReportaronInterrupcion(List<ReporteInterrupcion> listaReporte)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = ConstantesExtranetCTAF.EracmfRpt06NoReportaron;

            //Caracteristicas generales del reporte
            tabla.TamLetra = 8;
            tabla.TipoFuente = "Calibri";
            tabla.Color = "#bfbfbf";
            tabla.EsMayuscula = true;
            tabla.ColorERACMF = "#FB7A57";

            #region cabecera
            tabla.AltoFilaCab = 0.5M;
            List<CabeceraReporteColumna> columnasCabecera = new List<CabeceraReporteColumna>();

            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "EMPRESAS", AnchoColumna = 4M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = string.Empty, AnchoColumna = 4M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = string.Empty, AnchoColumna = 4M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = string.Empty, AnchoColumna = 4M });

            tabla.CabeceraColumnas = columnasCabecera;
            #endregion

            #region cuerpo
            List<ReporteInterrupcion> listaDataTotal = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();
            List<ReporteInterrupcion> listaData;
            ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal);


            var afecodis = listaDataTotal.Select(x => new { x.Afecodi, x.Evencodi, x.Evenini }).Distinct();
            List<RegistroReporte> registros = new List<RegistroReporte>();
            foreach (var item in afecodis)
            {

                listaData = listaDataTotal.Where(x => x.Afecodi == item.Afecodi).ToList();

                List<string> listaEmpresa = listaData.Select(x => x.NombEmpresa).Distinct().OrderBy(x => x).ToList();


                int numFilas = Convert.ToInt32(Math.Floor(listaEmpresa.Count() / 4.0m));

                for (int i = 0; i <= numFilas; i++)
                {
                    int indiceI = i * 4;
                    int cantidad = listaEmpresa.Count() > indiceI + 4 ? 4 : listaEmpresa.Count() - indiceI;

                    if (cantidad > 0)
                    {
                        var subLista = listaEmpresa.GetRange(indiceI, cantidad);

                        List<CeldaReporte> datos = new List<CeldaReporte>();
                        foreach (var emprnomb in subLista)
                            datos.Add(this.GetCeldaEspecialEmpresa(emprnomb, false, false));

                        //agregar
                        for (int j = 0; j < 4 - cantidad; j++)
                            datos.Add(this.GetCeldaEspecialFilaVacia());

                        RegistroReporte registro = new RegistroReporte();
                        registro.AltoFila = 0.5M;
                        registro.ListaCelda = datos;
                        registro.Nombre = string.Format("Hora Inicio:{0}", item.Evenini);
                        registro.codigo = item.Evencodi.ToString();
                        registros.Add(registro);
                    }
                }
            }

            tabla.ListaRegistros = registros;

            #endregion

            return tabla;
        }

        /// <summary>
        /// Obtiene listas para armar el Excel Decisión
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataExcelDecision(List<ReporteInterrupcion> listaReporte, bool formatFechaCab)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = ConstantesExtranetCTAF.EracmfRpt08Decision;

            //Caracteristicas generales del reporte
            tabla.TamLetra = 8;
            tabla.TipoFuente = "Calibri";
            tabla.Color = "#bfbfbf";
            tabla.EsMayuscula = true;
            tabla.ColorERACMF = "#FB7A57";

            #region cabecera
            tabla.AltoFilaCab = 0.7M;

            List<CabeceraReporteColumna> columnasCabecera = new List<CabeceraReporteColumna>();

            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "N°", AnchoColumna = 1.21M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "SUMINISTRO", AnchoColumna = 8.26M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "POTENCIA (MW)", AnchoColumna = 1.59M });

            if (formatFechaCab)
            {
                tabla.AltoFilaCab = 1.24M;
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "INICIO (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 1.85M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FINAL (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 1.85M });
            }
            else
            {
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "INICIO (HH:MM:SS)", AnchoColumna = 1.64M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FINAL (HH:MM:SS)", AnchoColumna = 1.64M });
            }

            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "DURACIÓN (MIN)", AnchoColumna = 1.79M });

            tabla.CabeceraColumnas = columnasCabecera;

            #endregion

            #region cuerpo
            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();

            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();
            foreach (var reg in listaData)
            {
                RegistroReporte registro = new RegistroReporte();
                List<CeldaReporte> datos = new List<CeldaReporte>();
                registro.AltoFila = 0.5M;

                datos.Add(this.GetCeldaEspecialFila(reg.NumFila, false, false));
                datos.Add(this.GetCeldaEspecialSuministro(reg.Suministro, false, false, false, true, reg.IdEmpresa));
                datos.Add(this.GetCeldaEspecialMW(reg.Potencia, false, false, false));
                datos.Add(this.GetCeldaEspecialHora(reg.HoraInicio, false, false, false, formatFechaCab));
                datos.Add(this.GetCeldaEspecialHora(reg.HoraFinal, false, false, false, formatFechaCab));
                datos.Add(this.GetCeldaEspecialDuracion(reg.Duracion, false, false, false));

                registro.ListaCelda = datos;
                registro.Nombre = string.Format("Hora Inicio:{0}", reg.Evenini);
                registro.codigo = reg.Evencodi;
                registros.Add(registro);
            }

            tabla.ListaRegistros = registros;

            #endregion
            return tabla;
        }

        /// <summary>
        /// Obtiene listas para armar el Excel que reportaron 0
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataExcelReportaron0(List<ReporteInterrupcion> listaReporte)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = ConstantesExtranetCTAF.EracmfRpt05Reportaron0;

            //Caracteristicas generales del reporte
            tabla.TamLetra = 8;
            tabla.TipoFuente = "Calibri";
            tabla.Color = "#bfbfbf";
            tabla.EsMayuscula = true;
            tabla.ColorERACMF = "#FB7A57";

            #region cabecera
            tabla.AltoFilaCab = 0.5M;
            List<CabeceraReporteColumna> columnasCabecera = new List<CabeceraReporteColumna>();

            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "EMPRESA", AnchoColumna = 6.54M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "MOTIVO", AnchoColumna = 9.46M });
            tabla.CabeceraColumnas = columnasCabecera;
            #endregion

            #region cuerpo
            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();
            ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal);

            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();

            //ordenar lista
            listaData = listaData.OrderBy(x => x.NombEmpresa).ToList();

            foreach (var listaAgrupada in listaData.GroupBy(x => x.IdEmpresa))
            {
                List<string> observaciones = new List<string>();

                var listaAgrupNew = listaAgrupada.ToList();
                var reg = listaAgrupNew.First();
                foreach (var val in listaAgrupNew)
                {
                    if (!string.IsNullOrEmpty(val.Observaciones))
                        observaciones.Add(val.Observaciones.ToUpper());
                }

                string listObservacion = observaciones.Count > 0 ? string.Join(", ", observaciones) : "NO REPORTARON";

                RegistroReporte registro = new RegistroReporte();
                List<CeldaReporte> datos = new List<CeldaReporte>();
                registro.AltoFila = 0.5M;

                datos.Add(this.GetCeldaEspecialEmpresa(reg.NombEmpresa, false, false, true, reg.IdEmpresa));
                datos.Add(this.GetCeldaEspecialObservacion(listObservacion, false, false));
                registro.ListaCelda = datos;
                registro.Nombre = string.Format("Hora Inicio:{0}", reg.Evenini);
                registro.codigo = reg.Evencodi;
                registros.Add(registro);
            }

            tabla.ListaRegistros = registros;

            #endregion

            return tabla;
        }

        /// <summary>
        /// Obtiene listas para armar el Excel Agente con Demoras 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lstHandsonHorasCoordinacion"></param>
        /// <param name="filaIniCab"></param>
        /// <param name="coluIniCab"></param>
        private TablaReporte ObtenerDataExcelAgenteDemoras(List<ReporteInterrupcion> listaReporte, List<AfHoraCoordDTO> lstHorasCoordinacion)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = ConstantesExtranetCTAF.EracmfRpt07Demoras;

            //Caracteristicas generales del reporte
            tabla.TamLetra = 8;
            tabla.TipoFuente = "Calibri";
            tabla.Color = "#bfbfbf";
            tabla.EsMayuscula = true;
            tabla.ColorERACMF = "#FB7A57";

            #region cabecera
            tabla.AltoFilaCab = 0.5M;
            List<CabeceraReporteColumna> columnasCabecera = new List<CabeceraReporteColumna>();

            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "EMPRESA", AnchoColumna = 5.54M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "HORA DE COORDINACIÓN", AnchoColumna = 5M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "MOTIVO", AnchoColumna = 5.46M });
            tabla.CabeceraColumnas = columnasCabecera;
            #endregion

            #region cuerpo

            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();
            ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal);

            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();

            List<AfHoraCoordDTO> lstTotalAgentesDemoras = new List<AfHoraCoordDTO>();
            var lstEmpresasConfiguradas = ListAfEmpresas();
            foreach (var reg in listaData)
            {
                if (lstHorasCoordinacion != null) // solo reporte TotalDatos
                {
                    if (lstHorasCoordinacion.Any())
                    {
                        var idEmpresaInterrupcion = reg.IdEmpresa;

                        var regHoraCord = lstHorasCoordinacion.Find(x => x.Emprcodi == idEmpresaInterrupcion && x.Afecodi == reg.Afecodi);

                        if (regHoraCord.Afhofecha != null)
                        {
                            var horaCord = regHoraCord.Afhofecha.Value;
                            var horaAdicionada = horaCord.AddMinutes(15);

                            int val = reg.FechaFin.CompareTo(horaAdicionada);

                            if (val > 0)
                            {
                                regHoraCord.AfEmprenomb = lstEmpresasConfiguradas.Find(x => x.Emprcodi == regHoraCord.Emprcodi).Afemprnomb;
                                regHoraCord.Evenini = reg.Evenini;
                                regHoraCord.Evencodi = reg.Evencodi;
                                lstTotalAgentesDemoras.Add(regHoraCord);
                            }
                        }
                    }
                }
            }

            lstTotalAgentesDemoras = lstTotalAgentesDemoras.OrderBy(x => x.AfEmprenomb).ToList();

            foreach (var emprsaIntSum in lstTotalAgentesDemoras.GroupBy(x => new { x.Emprcodi, x.Afecodi }))
            {
                var agenteDemoras = lstTotalAgentesDemoras.Find(x => x.Emprcodi == emprsaIntSum.Key.Emprcodi && x.Afecodi == emprsaIntSum.Key.Afecodi);

                RegistroReporte registro = new RegistroReporte();
                List<CeldaReporte> datos = new List<CeldaReporte>();
                registro.AltoFila = 0.5M;

                datos.Add(this.GetCeldaEspecialEmpresa(agenteDemoras.AfEmprenomb, false, false, true, agenteDemoras.Emprcodi));
                datos.Add(this.GetCeldaEspecialHoraCoordinacion(agenteDemoras.Afhofecha.Value.ToString(ConstantesAppServicio.FormatoFechaFull2), false, false));
                var observ = agenteDemoras.Afhmotivo != null ? agenteDemoras.Afhmotivo : "NO INFORMADO";
                datos.Add(this.GetCeldaEspecialObservacion(observ, false, false));

                registro.ListaCelda = datos;
                registro.Nombre = string.Format("Hora Inicio:{0}", agenteDemoras.Evenini);
                registro.codigo = agenteDemoras.Evencodi;
                registros.Add(registro);
                tabla.ListaRegistros = registros;
            }

            #endregion
            return tabla;
        }

        /// <summary>
        /// Obtiene listas para armar el Excel Resarcimiento
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="lstHorasCoordinacion"></param>
        /// <returns></returns>
        private TablaReporte ObtenerDataExcelResarcimiento(List<ReporteInterrupcion> listaReporte, bool formatFechaCab)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = ConstantesExtranetCTAF.EracmfRpt09Resarcimiento;

            //Caracteristicas generales del reporte
            tabla.TamLetra = 8;
            tabla.TipoFuente = "Calibri";
            tabla.Color = "#bfbfbf";
            tabla.EsMayuscula = true;
            tabla.ColorERACMF = "#FB7A57";

            #region cabecera
            tabla.AltoFilaCab = 1.5M;

            List<CabeceraReporteColumna> columnasCabecera = new List<CabeceraReporteColumna>();

            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "SUMINISTROS AFECTADOS", AnchoColumna = 5.49M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "POTENCIA INTERRUMPIDA (MW) \n(A)", AnchoColumna = 2.11M });

            if (formatFechaCab)
            {
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "HORA\nINICIO (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 1.85M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "HORA\nFINAL (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 1.85M });
            }
            else
            {
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "HORA\nINICIO (HH:MM:SS)", AnchoColumna = 1.64M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "HORA\nFINAL (HH:MM:SS)", AnchoColumna = 1.64M });
            }

            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "TIEMPO DURACIÓN (MINUTOS)", AnchoColumna = 1.58M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "TIEMPO DURACIÓN (HORAS) \n(B)", AnchoColumna = 1.64M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "ENERGÍA NO SUMINISTRADA (MWH) \n(AXB)", AnchoColumna = 2.1M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = string.Empty, AnchoColumna = 1.15M });
            tabla.CabeceraColumnas = columnasCabecera;
            #endregion

            #region cuerpo
            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();
            //ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal);
            //Lista Data
            List<RegistroReporte> registros = new List<RegistroReporte>();

            List<ReporteInterrupcion> lista = new List<ReporteInterrupcion>();


            var codigosEventos = listaData.Select(y => new { y.Evencodi }).Distinct().ToList();
            var listaDataCompleta = listaData;

            foreach (var item in codigosEventos)
            {
                lista = listaDataCompleta.Where(x => x.Evencodi == item.Evencodi).OrderByDescending(c => c.Evencodi).ToList();
                foreach (var reg in lista)
                {
                    RegistroReporte registro = new RegistroReporte();
                    List<CeldaReporte> datos = new List<CeldaReporte>();
                    registro.AltoFila = 0.5M;

                    datos.Add(this.GetCeldaEspecialSuministro(reg.Suministro, false, false, false, true, reg.IdEmpresa));
                    datos.Add(this.GetCeldaEspecialMW(reg.Potencia, false, false, false));
                    datos.Add(this.GetCeldaEspecialHora(reg.HoraInicio, false, false, false, formatFechaCab));
                    datos.Add(this.GetCeldaEspecialHora(reg.HoraFinal, false, false, false, formatFechaCab));
                    datos.Add(this.GetCeldaEspecialDuracion(reg.Duracion, false, false, false));
                    datos.Add(this.GetCeldaEspecialDuracionHoras(reg.DuracionHoras, false, false, false));
                    datos.Add(this.GetCeldaEspecialENSTruncate(reg.EnergiaNoSuministrada, false, false, false));
                    datos.Add(this.GetCeldaEspecialFilaVacia());

                    registro.ListaCelda = datos;
                    registro.Nombre = string.Format("Hora Inicio:{0}", reg.Evenini);
                    registro.codigo = reg.Evencodi;
                    registros.Add(registro);
                }

                //Totales
                RegistroReporte registro0 = new RegistroReporte();
                registro0.AltoFila = 0.5M;
                registro0.EsFilaResumen = true;
                registro0.EsAgrupado = true;
                List<CeldaReporte> datos0 = new List<CeldaReporte>();
                ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal && x.Evencodi == item.Evencodi);


                datos0.Add(this.GetCeldaEspecialTotal("TOTAL(MW)--->", true, false));
                datos0.Add(this.GetCeldaEspecialENSTruncate(regTotal.Potencia, false, false, false));

                datos0.Add(this.GetCeldaEspecialFilaVacia());
                datos0.Add(this.GetCeldaEspecialFilaVacia());
                datos0.Add(this.GetCeldaEspecialFilaVacia());
                datos0.Add(this.GetCeldaEspecialENS("ENSf--->", true, true));
                datos0.Add(this.GetCeldaEspecialENSTruncate(regTotal.EnergiaNoSuministrada, false, false, false));
                datos0.Add(this.GetCeldaEspecialSuministro("MWH", true, false));

                registro0.ListaCelda = datos0;
                registro0.codigo = regTotal.Evencodi;
                registros.Add(registro0);
            }
            tabla.ListaRegistros = registros;

            #endregion
            return tabla;
        }

        /// <summary>
        /// Obtiene listas para formas el Excel Reducción de Suministros
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        public TablaReporte ObtenerDataExcelReduccionSuministros(List<ReporteInterrupcion> listaReporte, bool formatFechaCab)
        {
            TablaReporte tabla = new TablaReporte();
            tabla.ReptiCodiTabla = ConstantesExtranetCTAF.ReduccionRpt01;

            //Caracteristicas generales del reporte
            tabla.TamLetra = ConstantesExtranetCTAF.RptTamanio;
            tabla.TipoFuente = ConstantesExtranetCTAF.RptTipoFuente;
            tabla.Color = ConstantesExtranetCTAF.RptColor;
            tabla.EsMayuscula = true;

            #region cabecera
            tabla.AltoFilaCab = 0.7M;

            List<CabeceraReporteColumna> columnasCabecera = new List<CabeceraReporteColumna>();

            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "SUMINISTRO", AnchoColumna = 3M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "SUBESTACIÓN", AnchoColumna = 2.75M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "DE\n (MW)", AnchoColumna = 2M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "A\n (MW)", AnchoColumna = 2M });
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "REDUCCIÓN\n (MW)", AnchoColumna = 2M });

            if (formatFechaCab)
            {
                tabla.AltoFilaCab = 1.24M;
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "INICIO (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 1.85M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FINAL (HH:MM:SS) DD.MM.YYYY", AnchoColumna = 1.85M });
            }
            else
            {
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "INICIO (HH:MM:SS)", AnchoColumna = 1.85M });
                columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "FINAL (HH:MM:SS)", AnchoColumna = 1.85M });
            }
            columnasCabecera.Add(new CabeceraReporteColumna() { NombreColumna = "DURACIÓN (MIN)", AnchoColumna = 1.55M });

            tabla.CabeceraColumnas = columnasCabecera;
            #endregion

            #region cuerpo
            List<ReporteInterrupcion> listaData = listaReporte.Where(x => x.TipoReporte != ConstantesExtranetCTAF.TipoReporteTotal).ToList();
            List<ReporteInterrupcion> listaDataFiltro;
            List<RegistroReporte> registros = new List<RegistroReporte>();

            var codigosEventos = listaData.Select(y => new { y.Evencodi }).Distinct().ToList();

            foreach (var item in codigosEventos)
            {
                listaDataFiltro = listaData.Where(x => x.Evencodi == item.Evencodi).OrderByDescending(c => c.Evencodi).ToList();

                if (listaDataFiltro.Any())
                {
                    //Lista Data
                    foreach (var reg in listaDataFiltro)
                    {
                        RegistroReporte registro = new RegistroReporte();
                        List<CeldaReporte> datos = new List<CeldaReporte>();
                        registro.AltoFila = 0.5M;

                        var tieneFormatFecha = formatFechaCab;

                        datos.Add(this.GetCeldaEspecialSuministro(reg.Suministro, false, false));
                        datos.Add(this.GetCeldaEspecialSE(reg.Subestacion, false, false));

                        datos.Add(this.GetCeldaEspecialMW(reg.RedSumDE, false, false, false));
                        datos.Add(this.GetCeldaEspecialMW(reg.RedSumA, false, false, false));
                        datos.Add(this.GetCeldaEspecialMW(reg.Reduccion, false, false, false));
                        datos.Add(this.GetCeldaEspecialHora(reg.HoraInicio, false, false, false, tieneFormatFecha));
                        datos.Add(this.GetCeldaEspecialHora(reg.HoraFinal, false, false, false, tieneFormatFecha));

                        datos.Add(this.GetCeldaEspecialDuracion(reg.Duracion, false, false, false));

                        registro.ListaCelda = datos;
                        registro.codigo = reg.Evencodi;
                        registro.Nombre = string.Format("Hora Inicio:{0}", reg.Evenini);
                        registros.Add(registro);
                    }

                    //Totales
                    RegistroReporte registro0 = new RegistroReporte();
                    registro0.AltoFila = 0.5M;
                    registro0.EsFilaResumen = true;
                    registro0.EsAgrupado = true;
                    List<CeldaReporte> datos0 = new List<CeldaReporte>();
                    ReporteInterrupcion regTotal = listaReporte.Find(x => x.TipoReporte == ConstantesExtranetCTAF.TipoReporteTotal && x.Evencodi == item.Evencodi);

                    datos0.Add(this.GetCeldaEspecialZona("TOTAL", true, false, false));
                    datos0.Add(this.GetCeldaEspecialTotal(null, true, false, true));

                    datos0.Add(this.GetCeldaEspecialMW(null, true, true, false, true));
                    datos0.Add(this.GetCeldaEspecialMW(null, true, true, false, true));
                    datos0.Add(this.GetCeldaEspecialMW(regTotal.Reduccion, true, true, false, false));
                    datos0.Add(this.GetCeldaEspecialFilaVacia());
                    datos0.Add(this.GetCeldaEspecialFilaVacia());
                    datos0.Add(this.GetCeldaEspecialFilaVacia());

                    registro0.ListaCelda = datos0;
                    registro0.codigo = regTotal.Evencodi;
                    registro0.Nombre = string.Format("Hora Inicio:{0}", regTotal.Evenini);
                    registros.Add(registro0);
                }
            }

            tabla.ListaRegistros = registros;

            #endregion

            return tabla;
        }

        #endregion

        #endregion

        #region Configuración de plazos de carga de interrupción

        /// <summary>
        /// Método para obtener en formato HH:mm la cantidad de minutos que se guardaron en BD
        /// </summary>
        /// <param name="valorEnPlazo"></param>
        /// <param name="valorFueraPlazo"></param>
        public void GetConfiguracionPlazo(out string valorEnPlazo, out string valorFinPlazo)
        {
            ParametroAppServicio servParametro = new ParametroAppServicio();
            List<SiParametroValorDTO> listaParam = servParametro.ListSiParametroValorByIdParametro(ConstantesExtranetCTAF.IdParametroConfiguracionPlazo);
            var regEnPlazo = listaParam.Find(x => x.Siparvnota == ConstantesExtranetCTAF.ValorParametroEnPlazo);
            var regFinPlazo = listaParam.Find(x => x.Siparvnota == ConstantesExtranetCTAF.ValorParametroFinPlazo);

            valorEnPlazo = ParametroAppServicio.ConvertirMinutosFormatoCadena(regEnPlazo);
            valorFinPlazo = ParametroAppServicio.ConvertirMinutosFormatoCadena(regFinPlazo);
        }

        /// <summary>
        /// Actualizar parametros
        /// </summary>
        /// <param name="valorEnPlazo"></param>
        /// <param name="valorFinPlazo"></param>
        /// <param name="usuario"></param>
        public void ActualizarConfiguracionPlazo(string valorEnPlazo, string valorFinPlazo, string usuario)
        {
            ParametroAppServicio servParametro = new ParametroAppServicio();

            int minEnPlazo = ParametroAppServicio.ConvertirMinutosFormatoNumero(valorEnPlazo);
            int minFinPlazo = ParametroAppServicio.ConvertirMinutosFormatoNumero(valorFinPlazo);

            if (minFinPlazo <= minEnPlazo)
            {
                throw new Exception("En Plazo debe ser menor a Fin de plazo");
            }

            List<SiParametroValorDTO> listaParam = servParametro.ListSiParametroValorByIdParametro(ConstantesExtranetCTAF.IdParametroConfiguracionPlazo);
            var regEnPlazo = listaParam.Find(x => x.Siparvnota == ConstantesExtranetCTAF.ValorParametroEnPlazo);
            var regFinPlazo = listaParam.Find(x => x.Siparvnota == ConstantesExtranetCTAF.ValorParametroFinPlazo);

            //Update si es que cambiaron los valores
            if (regEnPlazo.Siparvvalor != minEnPlazo)
            {
                regEnPlazo.Siparvvalor = minEnPlazo;
                regEnPlazo.Siparvusumodificacion = usuario;
                regEnPlazo.Siparvfecmodificacion = DateTime.Now;
                servParametro.UpdateSiParametroValor(regEnPlazo);
            }

            if (regFinPlazo.Siparvvalor != minFinPlazo)
            {
                regFinPlazo.Siparvvalor = minFinPlazo;
                regFinPlazo.Siparvusumodificacion = usuario;
                regFinPlazo.Siparvfecmodificacion = DateTime.Now;
                servParametro.UpdateSiParametroValor(regFinPlazo);
            }
        }

        /// <summary>
        /// Obtener la fecha de vencimiento
        /// </summary>
        /// <param name="fechaEvento"></param>
        /// <returns></returns>
        public DateTime GetFechaFinPlazo(DateTime fechaEvento)
        {
            ParametroAppServicio servParametro = new ParametroAppServicio();
            List<SiParametroValorDTO> listaParam = servParametro.ListSiParametroValorByIdParametro(ConstantesExtranetCTAF.IdParametroConfiguracionPlazo);
            var regFinPlazo = listaParam.Find(x => x.Siparvnota == ConstantesExtranetCTAF.ValorParametroFinPlazo);

            return fechaEvento.AddMinutes(Convert.ToDouble(regFinPlazo.Siparvvalor ?? 0));
        }

        /// <summary>
        /// Obtener la fecha de fin en plazo (verde)
        /// </summary>
        /// <param name="fechaEvento"></param>
        /// <returns></returns>
        public DateTime GetFechaFinEnPlazo(DateTime fechaEvento)
        {
            ParametroAppServicio servParametro = new ParametroAppServicio();
            List<SiParametroValorDTO> listaParam = servParametro.ListSiParametroValorByIdParametro(ConstantesExtranetCTAF.IdParametroConfiguracionPlazo);
            var regEnPlazo = listaParam.Find(x => x.Siparvnota == ConstantesExtranetCTAF.ValorParametroEnPlazo);

            return fechaEvento.AddMinutes(Convert.ToDouble(regEnPlazo.Siparvvalor ?? 0));
        }

        #endregion

        #region NOTIFICAR SOLICITUDES PENDIENTES

        /// <summary>
        /// Actualiza la hora de ejecucion
        /// </summary>
        /// <param name="horaejecucion"></param>
        public void ActualizarConfiguracionProceso(string horaejecucion)
        {
            ParametroAppServicio servParametro = new ParametroAppServicio();

            SiProcesoDTO proceso = ObtenerProcesoSolicitudesPendientes();

            string[] separadas;
            separadas = horaejecucion.Split(':');
            int hora = Convert.ToInt32(separadas[0]);
            int minutos = Convert.ToInt32(separadas[1]);

            //Update si es que cambiaron los valores
            if (proceso.Prschorainicio.Value != hora)
                proceso.Prschorainicio = hora;

            if (proceso.Prscminutoinicio != minutos)
                proceso.Prscminutoinicio = minutos;

            FactorySic.GetSiProcesoRepository().Update(proceso);
        }

        /// <summary>
        /// Devuelve elproceso designado para las solicitudes pendientes
        /// </summary>
        /// <returns></returns>
        private SiProcesoDTO ObtenerProcesoSolicitudesPendientes()
        {
            return FactorySic.GetSiProcesoRepository().GetById(ConstantesExtranetCTAF.IdProcesoConfiguracionHoraNotificacion);
        }

        /// <summary>
        /// Obtener Hora Ejecucion
        /// </summary>
        /// <returns></returns>
        public string ObtenerHoraEjecucion()
        {
            string horaEjecucion = "";
            SiProcesoDTO proceso = ObtenerProcesoSolicitudesPendientes();
            var hora = string.Format("{0:D2}", proceso.Prschorainicio.Value);
            var minuto = string.Format("{0:D2}", proceso.Prscminutoinicio.Value);

            horaEjecucion = hora + ":" + minuto;

            return horaEjecucion;
        }

        /// <summary>
        /// Notifica via email sobre las solicitudes pendientes de revision
        /// </summary>
        public void NotificarSolicitudesPendientes()
        {
            try
            {
                List<AfSolicitudRespDTO> lstSolicitudesPendientes = ObtenerSolicitudesPorEstado(ConstantesExtranetCTAF.Pendiente);

                if (lstSolicitudesPendientes.Any())
                {
                    var empresas = FactorySic.GetSiEmpresaRepository().ListGeneral();
                    var listaEmpresasSolicitantes = lstSolicitudesPendientes.GroupBy(x => new { x.Emprcodi }).Select(y => new SolicitudesPendientes() { Emprcodi = y.Key.Emprcodi, NumSolicitudes = y.Count() }).ToList();

                    foreach (var solicitudPendiente in listaEmpresasSolicitantes)
                    {
                        var idEmpresa = solicitudPendiente.Emprcodi;
                        var empresaX = empresas.Find(x => x.Emprcodi == idEmpresa);
                        if (empresaX != null)
                            solicitudPendiente.Emprnomb = empresaX.Emprnomb;
                    }

                    if (listaEmpresasSolicitantes.Any())
                    {
                        try
                        {
                            SiPlantillacorreoDTO plantilla = this.servCorreo.GetByIdSiPlantillacorreo(ConstantesExtranetCTAF.PlantcodiNotificacionSolicitudesPendientes);

                            if (plantilla != null)
                            {
                                string asunto = string.Format(plantilla.Plantasunto);
                                string contenido = this.GetContenidoNotificaionSolicitudesPendientes(listaEmpresasSolicitantes.ToList());
                                List<string> listaCC = new List<string>();
                                List<string> listaTo = plantilla.Planticorreos.Split(';').Select(x => x).ToList();

                                string from = TipoPlantillaCorreo.MailFrom;
                                string to = string.Join(" ", listaTo);

                                if (!string.IsNullOrEmpty(contenido))
                                {
                                    if (!(ConfigurationManager.AppSettings["FlagNotificarSolicitudesPendientes"].ToString() == "S"))
                                    {
                                        listaTo = ConstantesExtranetCTAF.ListaEmailAdminEventosTo.Split(';').ToList();
                                        listaCC = ConstantesExtranetCTAF.ListaEmailAdminEventosCC.Split(';').ToList();
                                    }

                                    COES.Base.Tools.Util.SendEmail(listaTo, listaCC, asunto, contenido);


                                    DateTime fechaEnvio = DateTime.Now;
                                    SiCorreoDTO correo = new SiCorreoDTO();
                                    correo.Corrasunto = asunto;
                                    correo.Corrcontenido = contenido;
                                    correo.Corrfechaenvio = fechaEnvio;
                                    //correo.Corrfechaperiodo = fechaPeriodo;
                                    correo.Corrfrom = from;
                                    correo.Corrto = to;
                                    //correo.Emprcodi = empresaCodi;
                                    //correo.Enviocodi = idEnvio;
                                    correo.Plantcodi = plantilla.Plantcodi;

                                    this.servCorreo.SaveSiCorreo(correo);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ConstantesAppServicio.LogError, ex);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        /// <summary>
        /// Devuelve listado de solicitudes segun su estado
        /// </summary>
        /// <param name="pendiente"></param>
        /// <returns></returns>
        private List<AfSolicitudRespDTO> ObtenerSolicitudesPorEstado(string pendiente)
        {
            List<AfSolicitudRespDTO> lstSolicitudes = new List<AfSolicitudRespDTO>();
            var lstTotal = GetByCriteriaAfSolicitudResps();

            if (lstTotal.Any())
            {
                lstSolicitudes = lstTotal.Where(x => x.Sorespestadosol == pendiente).ToList();
            }

            return lstSolicitudes;
        }

        /// <summary>
        /// Generación del contenido del correo Solicitudes pendientes
        /// </summary>
        /// <param name="listaDespues"></param>
        /// <param name="listaAntes"></param>
        /// <returns></returns>
        private string GetContenidoNotificaionSolicitudesPendientes(List<SolicitudesPendientes> listaEmpresasSolicitantes)
        {
            string html = @"
                <html>
	                <head>
		                <STYLE TYPE='text/css'>
		                body {{font-size: .80em;font-family: 'Helvetica Neue', 'Lucida Grande', 'Segoe UI', Arial, Helvetica, Verdana, sans-serif;}}
		                .mail {{width:500px;border-spacing:0;border-collapse:collapse;}}
		                .mail thead th {{text-align: center;background: #417AA7;color:#ffffff}}
		                .mail tr td {{border:1px solid #dddddd;}}
		                table.tabla_hop thead th {text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;}
		                </style>
	                </head>
	                <body>
	                    Estimados señores del CTAF, existen solicitudes pendientes de revisión para las siguientes empresas: <br>
                        </br>
		                {contenidoEmpresas}

		                <br/><br/>
		                {footer}
	                </body>
                </html>
            ";

            html = html.Replace("{contenidoEmpresas}", this.GetHtmlListadoEmpresasSolicitudesPendientes(listaEmpresasSolicitantes));

            html = html.Replace("{footer}", CorreoAppServicio.GetFooterCorreo());

            return html;
        }

        /// <summary>
        /// Arma el listado de empresas con solicitudes pendientes
        /// </summary>
        /// <param name="listaEmpresasSolicitantes"></param>
        /// <returns></returns>
        private string GetHtmlListadoEmpresasSolicitudesPendientes(List<SolicitudesPendientes> listaEmpresasSolicitantes)
        {
            string cadena = "";
            listaEmpresasSolicitantes = listaEmpresasSolicitantes.OrderBy(x => x.Emprnomb).ToList();

            if (listaEmpresasSolicitantes.Any())
            {
                cadena = cadena + "<ul>";
                foreach (var empresa in listaEmpresasSolicitantes)
                {
                    cadena = cadena + "<li> Empresa: " + empresa.Emprnomb + " (<b> " + empresa.NumSolicitudes + " " + (empresa.NumSolicitudes > 1 ? "solicitudes </b>)" : "solicitud </b>)") + "</li>";
                }
                cadena = cadena + "</ul>";
            }

            return cadena;
        }

        /// <summary>
        /// Obtiene laa lista de los Correos Por Empresa y Modulo
        /// </summary>
        /// <param name="idModulo">Id Modulo</param>
        /// <param name="idEmpresa">Id Empresa</param>  
        /// <returns>Listado de Correos por Empresa y Modulo</returns>
        public List<SiEmpresaCorreoDTO> ObtenerCorreosPorEmpresaModulo(int idModulo, int idEmpresa)
        {
            return FactorySic.GetSiEmpresaCorreoRepository().ObtenerCorreosPorEmpresaModulo(idModulo, idEmpresa);
        }

        #endregion

        #region NOTIFICAR RESULTADO DE SOLICITUDES

        /// <summary>
        /// Enviar Correo Notificacion Resultado Solicitud
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="solicitud"></param>
        /// <param name="archivosAdjuntados"></param>
        /// <param name="pathTemporalEventos"></param>
        public void EnviarCorreoNotificacionResultadoSolicitud(string usuario, AfSolicitudRespDTO solicitud, List<FileData> archivosAdjuntados, string pathTemporalEventos)
        {
            //Enviar correo
            if (solicitud != null)
            {
                try
                {
                    SiPlantillacorreoDTO plantilla = this.servCorreo.GetByIdSiPlantillacorreo(ConstantesExtranetCTAF.PlantcodiNotificacionEstadoSolicitudes);

                    if (plantilla != null)
                    {
                        var idEmpresa = solicitud.Emprcodi;
                        var idEnvio = solicitud.Enviocodi;
                        var fechaModificacion = DateTime.Now;
                        var fechaPeriodo = fechaModificacion.Date;

                        string asunto = string.Format(plantilla.Plantasunto, solicitud.Emprnomb, solicitud.SorespfechaeventoDesc);
                        string contenido = this.GetContenidoCorreoNotificacionEstadoSolicitud(solicitud, archivosAdjuntados);

                        // Correos de destino(To:)
                        List<string> listaTo = new List<string>();
                        // Correos con copia (Cc:)
                        List<string> listaCC = new List<string>();

                        string from = TipoPlantillaCorreo.MailFrom;
                        string to = string.Join(" ", listaTo);

                        /*****************************************************************************************************************************/

                        // Obtener archivos adjuntos
                        List<Tuple<Stream, string>> filesDescargados = new List<Tuple<Stream, string>>();
                        List<FileData> listaDocumentos = FileServer.ListarArhivos(pathTemporalEventos, null);
                        foreach (var item in listaDocumentos)
                        {
                            filesDescargados.Add(new Tuple<Stream, string>(FileServer.DownloadToStream(pathTemporalEventos + item.FileName, string.Empty), item.FileName));
                        }

                        // Correo de agentes por módulo y por empresa.
                        List<SiEmpresaCorreoDTO> auxiliar = this.ObtenerCorreosPorEmpresaModulo(ConstantesExtranetCTAF.ModcodiCTAF, solicitud.Emprcodi);
                        for (int x = 0; x < auxiliar.Count(); x++)
                        {
                            listaTo.Add(auxiliar[x].Useremail);
                        }

                        if (!string.IsNullOrEmpty(contenido))
                        {
                            if (!ConstantesExtranetCTAF.FlagEnviarNotificacionEstadoSolicitudes)
                            {
                                listaTo = ConstantesExtranetCTAF.ListaEmailAdminSolicitudTo.Split(';').ToList();
                                listaCC = ConstantesExtranetCTAF.ListaEmailAdminSolicitudCC.Split(';').ToList();
                            }

                            COES.Base.Tools.Util.SendEmailAttachDispose(listaTo, asunto, contenido, from, listaCC, filesDescargados);

                            SiCorreoDTO correo = new SiCorreoDTO();
                            correo.Corrasunto = asunto;
                            correo.Corrcontenido = contenido;
                            correo.Corrfechaenvio = fechaModificacion;
                            correo.Corrfechaperiodo = fechaPeriodo;
                            correo.Corrfrom = from;
                            correo.Corrto = to;
                            correo.Emprcodi = idEmpresa <= 1 ? 1 : idEmpresa;
                            correo.Enviocodi = idEnvio;
                            correo.Plantcodi = plantilla.Plantcodi;

                            this.servCorreo.SaveSiCorreo(correo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                }
            }
        }

        /// <summary>
        /// Enviar Correo CTAF
        /// </summary>
        /// <param name="oAnalisisFallaDTO"></param>
        /// <param name="oEventoDTO"></param>
        /// <param name="LstEvento"></param>
        /// <param name="filename"></param>
        public bool EnviarCorreoCTAF(AnalisisFallaDTO oAnalisisFallaDTO, EventoDTO oEventoDTO, List<EventoDTO> LstEvento, string filename)
        {
            try
            {
                string asuntoInicial = oAnalisisFallaDTO.AFECORR.ToString().PadLeft(3, '0');
                string asuntoFinal = oAnalisisFallaDTO.AFEANIO.ToString();
                string tipoReunion = string.Empty;

                if (LstEvento[0].EVENTIPOFALLA == "N")
                    tipoReunion = "No Presencial (NP)";
                else
                    tipoReunion = "Presencial (P)";

                SiPlantillacorreoDTO plantilla = this.servCorreo.GetByIdSiPlantillacorreo(ConstantesAppServicio.PlantcodiNotificacionCitacion);

                string plantAsunto = "CITACIÓN CTAF Nº " + asuntoInicial + ". Reunión del Comité Técnico de Análisis de Fallas - " + asuntoFinal;
                string plantiCorreoFrom = plantilla.PlanticorreoFrom;
                string plantiCorreos = plantilla.Planticorreos;
                string plantiCorreosCC = plantilla.PlanticorreosCc;
                string plantiCorreosBCC = plantilla.PlanticorreosBcc;

                string fechaLimite = oAnalisisFallaDTO.AFEREUFECHAPROG.Value.ToString(ConstantesAppServicio.FormatoFechaMail);
                string evento = "EV-" + asuntoInicial;
                string descripcion = oEventoDTO.EVENASUNTO;
                string nuevaLineaWeb = "<br/>";
                string empresasInvolucradas = $"{nuevaLineaWeb}{LstEvento[0].EmpresaInvolucrada}{nuevaLineaWeb}{nuevaLineaWeb}";

                List<string> files = new List<string>();
                files.Add(@filename);

                string asunto = plantAsunto;
                string contenido = this.GetContenidoCorreoCitacionCTAF(tipoReunion, fechaLimite, evento, descripcion, empresasInvolucradas, plantilla.Plantcontenido);

                List<string> listaTo = new List<string>();
                List<string> listaCC = new List<string>();
                List<string> listaBCC = new List<string>();

                if (plantiCorreos != null)
                    listaTo = plantiCorreos.Split(';').Select(x => x).ToList();
                if (plantiCorreosCC != null)
                    listaCC = plantiCorreosCC.Split(';').Select(x => x).ToList();
                if (plantiCorreosBCC != null)
                    listaBCC = plantiCorreosBCC.Split(';').Select(x => x).ToList();

                if (!string.IsNullOrEmpty(contenido))
                {
                    COES.Base.Tools.Util.SendEmail(listaTo, listaCC, listaBCC, asunto, contenido, plantiCorreoFrom, files);

                    SiCorreoDTO correo = new SiCorreoDTO();
                    correo.Corrasunto = asunto;
                    correo.Corrcontenido = contenido;
                    correo.Corrfrom = plantiCorreoFrom;
                    correo.Corrto = plantiCorreos;
                    correo.Plantcodi = plantilla.Plantcodi;

                    this.servCorreo.SaveSiCorreo(correo);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return false;
            }
        }


        /// <summary>
        /// Enviar Correo CTAF
        /// </summary>
        /// <param name="oAnalisisFallaDTO"></param>
        /// <param name="oEventoDTO"></param>
        /// <param name="filename"></param>
        /// <param name="fileInformepdf"></param>
        /// <param name="ListaObservaciones"></param>
        /// <param name="ListaRecomendaciones"></param>
        public bool EnviarCorreoActaCTAF(AnalisisFallaDTO oAnalisisFallaDTO, EventoDTO oEventoDTO, string filename, string fileInformepdf, List<EveRecomobservDTO> ListaObservaciones, List<EveRecomobservDTO> ListaRecomendaciones)
        {
            //Enviar correo
            try
            {
                // TODO: Reemplazar con las variables del correo electronico
                string evento = "EV-" + oAnalisisFallaDTO.AFECORR.ToString().PadLeft(3, '0') + "-" + oAnalisisFallaDTO.AFEANIO;
                string nombreEvento = oEventoDTO.EVENASUNTO;
                string eventoCompleto = evento + ": " + nombreEvento;
                string asunto = "Informe CTAF " + evento;
                string fechaLimite = fechaLimite = oAnalisisFallaDTO.AFELIMATENCOMEN.Value.ToString(ConstantesAppServicio.FormatoFechaMail);
                string nombreFechaLimite = new CultureInfo("ES-ES").DateTimeFormat.GetDayName(oAnalisisFallaDTO.AFELIMATENCOMEN.Value.DayOfWeek);
                string diaFechaLimite = nombreFechaLimite + " " + fechaLimite;

                SiPlantillacorreoDTO plantilla = this.servCorreo.GetByIdSiPlantillacorreo(ConstantesAppServicio.PlantcodiNotificacionInformeCtaf);

                string plantiCorreoFrom = plantilla.PlanticorreoFrom;
                string plantiCorreos = plantilla.Planticorreos;
                string plantiCorreosCC = plantilla.PlanticorreosCc;
                string plantiCorreosBCC = plantilla.PlanticorreosBcc;

                //TODO: Se tiiene que ingresar la ruta completas del archivo
                List<string> files = new List<string>();
                files.Add(@filename);
                files.Add(@fileInformepdf);

                //SiPlantillacorreoDTO plantilla = this.servCorreo.GetByIdSiPlantillacorreo(ConstantesExtranetCTAF.PlantcodiNotificacionEstadoSolicitudes);

                //if (plantilla != null)
                //if (plantilla != null)
                //{
                //List<EveRecomobservDTO> ListaObservaciones = this.ListEveRecomobserv((int)oEventoDTO.EVENCODI, 2);
                //List<EveRecomobservDTO> ListaRecomendaciones = this.ListEveRecomobserv((int)oEventoDTO.EVENCODI, 1);
                string contenido = this.GetContenidoCorreoActaCTAF(eventoCompleto, diaFechaLimite, ListaObservaciones, ListaRecomendaciones);

                List<string> listaTo = new List<string>();
                List<string> listaCC = new List<string>();
                List<string> listaBCC = new List<string>();

                if (plantiCorreos != null)
                    listaTo = plantiCorreos.Split(';').Select(x => x).ToList();
                if (plantiCorreosCC != null)
                    listaCC = plantiCorreosCC.Split(';').Select(x => x).ToList();
                if (plantiCorreosBCC != null)
                    listaBCC = plantiCorreosBCC.Split(';').Select(x => x).ToList();

                if (!string.IsNullOrEmpty(contenido))
                {
                    //if (!ConstantesExtranetCTAF.FlagEnviarNotificacionCargaEvento)
                    //{
                    //    listaTo = ConstantesExtranetCTAF.ListaEmailAdminEventosTo.Split(';').ToList();
                    //    listaCC = ConstantesExtranetCTAF.ListaEmailAdminEventosCC.Split(';').ToList();
                    //}

                    COES.Base.Tools.Util.SendEmail(listaTo, listaCC, listaBCC, asunto, contenido, plantiCorreoFrom, files);

                    //SiCorreoDTO correo = new SiCorreoDTO();
                    //correo.Corrasunto = asunto;
                    //correo.Corrcontenido = contenido;
                    //correo.Corrfrom = from;
                    //correo.Corrto = to;
                    //correo.Plantcodi = plantilla.Plantcodi;

                    //this.servCorreo.SaveSiCorreo(correo);
                }
                //}
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return false;
            }
        }




        /// <summary>
        /// Get Contenido Correo Notificacion EstadoSolicitud
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="archivosAdjuntados"></param>
        /// <returns></returns>
        private string GetContenidoCorreoNotificacionEstadoSolicitud(AfSolicitudRespDTO solicitud, List<FileData> archivosAdjuntados)
        {
            var estadoSolicitud = solicitud.Estado.ToUpper();
            var descripcionEvento = "<br>"
                + "<br> <b>Fecha del Evento:</b>" + solicitud.SorespfechaeventoDesc
                + "<br><b>Descripción del Evento: </b>" + solicitud.Sorespdesc;
            var comentario = "<br><br>" + "<b>Observacion: </b>" + solicitud.Sorespobs + "<br>";
            int numArchivos = archivosAdjuntados.Any() ? archivosAdjuntados.Count : 0;
            string textoArchivoAdjuntado = numArchivos > 0 ? "<br><br><b>NOTA<b>: Se adjunta " + numArchivos + " archivo(s). <br>" : string.Empty;

            string html = @"
                <html>

                    <head>
	                    <STYLE TYPE='text/css'>
	                    body {{font-size: .80em;font-family: 'Helvetica Neue', 'Lucida Grande', 'Segoe UI', Arial, Helvetica, Verdana, sans-serif;}}
	                    .mail {{width:500px;border-spacing:0;border-collapse:collapse;}}
	                    .mail thead th {{text-align: center;background: #417AA7;color:#ffffff}}
	                    .mail tr td {{border:1px solid #dddddd;}}
	                    table.tabla_hop thead th {text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;}
	                    </style>
                    </head>

                    <body>
                        Estimados Señores. <br>
                        Su solicitud de Asignación de Responsabilidad ha sido <b><Estado></b>.
                        
                        <DescripcionEvento>
                        
                        <Comentario>
                        
                        <i><Adjuntado></i>
	                    
                        <br>
                        {footer}
                    </body>
                </html>
            ";
            html = html.Replace("<Estado>", estadoSolicitud);
            html = html.Replace("<DescripcionEvento>", descripcionEvento);
            html = html.Replace("<Comentario>", comentario);
            html = html.Replace("<Adjuntado>", textoArchivoAdjuntado);

            html = html.Replace("{footer}", CorreoAppServicio.GetFooterCorreo());

            return html;
        }

        #endregion

        #endregion

        #region Mejoras CTAF
        /// <summary>
        /// Actualiza el campo Evenctaf en la tabla eve_evento 
        /// </summary>
        public void eliminarEventoCtaf(int evencodi)
        {
            try
            {
                FactorySic.GetEveEventoRepository().UpdateEventoCtaf(evencodi, "N");
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Inserta en tabla media los eventos relacionados entre sí
        /// </summary>
        public List<EveEventoDTO> ListadoEventosAsoCtaf(int evencodi)
        {
            try
            {
                return FactorySic.GetEveEventoRepository().ListadoEventosAsoCtaf(evencodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Inserta en tabla media los eventos relacionados entre sí
        /// </summary>
        public int ObtieneCantFileEnviadosSco(int evencodi)
        {
            try
            {
                return FactorySic.GetEveEventoRepository().ObtieneCantFileEnviadosSco(evencodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Lista los eventos Sco asociados a un evento Ctaf
        /// </summary>
        public List<EventoDTO> LstEventosSco(string anio, string correlativo)
        {
            return FactorySic.ObtenerEventoDao().LstEventosSco(anio, correlativo);
        }


        /// <summary>
        /// Listado de interrupciones por evento SCO
        /// </summary>
        /// <param name="oEventoDTO"></param>
        /// <returns></returns>
        public List<EventoDTO> ListarInterrupcionPorEventoSCO(EventoDTO oEventoDTO)
        {
            return FactorySic.ObtenerEventoDao().ListarInterrupcionPorEventoSCO(oEventoDTO);
        }

        /// <summary>
        /// Listado de Señalizaciones de Protecciones
        /// </summary>
        /// <param name="CodigoEvento"></param>
        /// <returns></returns>
        public List<SiSenializacionDTO> ListarSenializacionesProteccion(int CodigoEvento)
        {
            return FactorySic.ObtenerEventoDao().ListarSenializacionesProteccion(CodigoEvento);
        }

        /// <summary>
        /// Listado de Señalizaciones de Protecciones
        /// </summary>
        /// <param name="CodigoEvento"></param>
        /// <returns></returns>
        public List<SiSenializacionDTO> ListarSenializacionesProteccionAgrupado(int CodigoEvento)
        {
            return FactorySic.ObtenerEventoDao().ListarSenializacionesProteccionAgrupado(CodigoEvento);
        }

        /// <summary>
        /// Eliminar Señalizaciones de Protecciones por CodigoEvento
        /// </summary>
        /// <param name="CodigoEvento"></param>
        /// <returns></returns>
        public int EliminarSenializacionProteccion(int CodigoEvento)
        {
            return FactorySic.ObtenerEventoDao().EliminarSenializacionProteccion(CodigoEvento);
        }

        /// <summary>
        /// Grabar Señalizaciones de Protecciones por CodigoEvento
        /// </summary>
        /// <param name="siSenializacionDTO"></param>
        /// <returns></returns>
        public int GrabarSenializacionProteccion(SiSenializacionDTO siSenializacionDTO)
        {
            return FactorySic.ObtenerEventoDao().GrabarSenializacionProteccion(siSenializacionDTO);
        }

        /// <summary>
        /// ListarInterrupcionPorEventoSCOHtml
        /// </summary>
        /// <param name="oEventoDTO"></param>
        /// <returns></returns>
        public string ListarInterrupcionPorEventoSCOHtml(List<EventoDTO> oEventoDTO)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            strHtml.Append("<table id='table' class='pretty tabla-adicional' style='table-layout: fixed; margin-top: 20px;width:600px'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width: 100px'>Hora de Evento</th>");
            strHtml.AppendFormat("<th style='width: 120px'>Hora de Interrupción</th>");
            strHtml.AppendFormat("<th style='width: 20px'>ERACMF</th>");
            strHtml.AppendFormat("<th style='width: 20px; display:none;'>ERACMF</th>");
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            foreach (var reg in oEventoDTO)
            {
                strHtml.Append("<tr style='height: 25px;'>");
                //strHtml.Append("<td>");
                ////if (permisoGrabar)
                ////    strHtml.AppendFormat("<a href = '{0}Eventos/AnalisisFallas/Edicion/{1}' id='btnEditar' target=''><img src ='{0}Content/Images/btn-edit.png' alt='Editar evento' title='Editar evento' style='width='24px;'></a> ", url, reg.AFECODI);
                ////else
                ////    strHtml.AppendFormat("<a href = '{0}Eventos/AnalisisFallas/Edicion/{1}' id='btnEditar' target=''><img src ='{0}Content/Images/btn-open.png' alt='Visualizar evento' title='Visualizar evento' style='width='24px;'></a> ", url, reg.AFECODI);
                ////if (reg.Afefechainterr != null)
                ////    strHtml.AppendFormat("<a href = '{0}Eventos/AnalisisFallas/ReporteExtranetCTAF/{1}' id='btnReporte' target=''><img src ='{0}Content/Images/btn-properties.png' alt='Reporte' title='Reporte' style='width='24px;'></a> ", url, reg.AFECODI);
                ////if (reg.ERACMF == "S")
                ////{
                ////    strHtml.AppendFormat("<a href = '{0}Eventos/AnalisisFallas/CargarERACMF/{1}' id='btnCargar' target=''><img src ='{0}Content/Images/Import2.png' alt='Cargar archivo ERACMF' title='Cargar archivo ERACMF' style='width='24px;'></a> ", url, reg.AFECODI);
                ////}
                //strHtml.Append("</td>");
                string fechaInterrupcion;
                if (reg.Afefechainterr.HasValue)
                {
                    fechaInterrupcion = reg.Afefechainterr.Value.ToString("dd/MM/yyyy HH:mm:ss");
                }
                else
                {
                    fechaInterrupcion = CompletaFechasInterrupcion(reg).FechaInterrupcion.ToString();
                }
                strHtml.AppendFormat("<td style='text-align: center;'>{0}</td>", reg.EVENINI != null ? reg.EVENINI.Value.ToString("dd/MM/yyyy HH:mm:ss") : null);
                strHtml.AppendFormat("<td style='text-align: center;'> <input type='text'  style='width: 150px' value='{0}' class='dp-row'</td>", fechaInterrupcion);
                strHtml.AppendFormat("<td style='text-align: center; padding-right: 15px;'>{0}</td>", reg.Afeeracmf.Trim() == "N" ? "NO" : "SI");
                strHtml.AppendFormat("<td style='text-align: center; padding-right: 15px; display:none;'>{0}</td>", reg.AFECODI);
                strHtml.AppendFormat("<td style='text-align: center; padding-right: 15px; display:none;'>{0}</td>", reg.EVENCODI);

                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");

            #endregion

            strHtml.Append("</table>");

            return strHtml.ToString();
        }
        /// <summary>
        /// Actualiza AF por cada evento sco asociado
        /// </summary>
        /// <param name="evencodi"></param>
        /// <param name="afecorr"></param>
        /// <returns></returns>
        public void ActualizarCodEvento(int evencodi, int afecorr)
        {
            FactorySic.ObtenerEventoDao().ActualizarCodEvento(evencodi, afecorr);
        }
        /// <summary>
        /// Actualiza datos de AF seleccionado en check de evento sco
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void ActualizarEventoxAfecodi(AnalisisFallaDTO entity)
        {
            FactorySic.ObtenerEventoDao().ActualizarEventoxAfecodi(entity);
        }
        /// <summary>
        /// Actualiza datos de AF padre
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ActualizarEventoAF(AnalisisFallaDTO entity)
        {
            return FactorySic.ObtenerEventoDao().ActualizarEventoAF(entity);
        }
        /// <summary>
        /// Obtiene cantidad de informes
        /// </summary>
        public int ObtieneCantInformes(int afecodi, int emprcodi, string afiversion)
        {
            try
            {
                return FactorySic.ObtenerEventoDao().ObtieneCantInformesCtaf(afecodi, emprcodi, afiversion);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Inserta en tabla media los eventos relacionados entre sí
        /// </summary>
        public EventoDTO EventoDTOAsoCtaf(int evencodi)
        {
            try
            {
                return FactorySic.ObtenerEventoDao().EventoAsociadoCtaf(evencodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Inserta en tabla media los eventos relacionados entre sí
        /// </summary>
        public EventoDTO InterrupcionAsoCtaf(int evencodi)
        {
            try
            {
                return FactorySic.ObtenerEventoDao().InterrupcionAsoCtaf(evencodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Lista archivos enviados histórico por evencodi
        /// </summary>
        /// <param name="evencodi"></param>
        /// <returns></returns>
        public List<EmpresaInvolucradaDTO> ObtenerEmpresasInvolucradaxEvencodi(int evencodi)
        {
            return FactorySic.ObtenerEventoDao().ObtenerEmpresasInvolucradaxEvencodi(evencodi);
        }
        /// <summary>
        /// Eliminar Datos Interrupcion Suministro
        /// </summary>
        /// <param name="username"></param>
        /// <param name="emprcodi"></param>
        /// <param name="fdatcodi"></param>
        /// <param name="afecodi"></param>
        /// <returns></returns>
        public int EliminarInterrupcionSuministro(string username, int emprcodi, int fdatcodi, int afecodi)
        {
            int enviocodi = 0;
            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        EventoDTO afEvento = ObtenerInterrupcionByAfecodi(afecodi);
                        DateTime fechaRegistro = DateTime.Now;
                        //Dar de baja los envíos anteriores.
                        FactorySic.GetAfInterrupSuministroRepository().UpdateAEstadoBajaXEmpresa(afecodi, emprcodi, fdatcodi, connection, transaction);
                        transaction.Commit();
                        //Enviar notificacion al CTAF
                        EnviarCorreoNotificacionCargaInformacionAgentes(afEvento, username, fechaRegistro, emprcodi, enviocodi, 2);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return enviocodi;
        }
        /// <summary>
        /// Permite obtener un registro de la tabla AF_EMPRESA
        /// </summary>
        public AfEmpresaDTO GetByAfEmpresaxEmprcodi(int emprcodi)
        {
            return FactorySic.GetAfEmpresaRepository().GetByIdxEmprcodi(emprcodi);
        }
        /// <summary>
        /// Permite actualizar una recomendación AO
        /// </summary>
        public bool ActualizarRecomendacionAO(int afrrec, string evenrcmctaf)
        {
            return FactorySic.ObtenerEventoDao().ActualizarRecomendacionAO(afrrec, evenrcmctaf);
        }
        /// <summary>
        /// Permite obtener un registro de la tabla AF_EMPRESA
        /// </summary>
        public EmpresaRecomendacionDTO GetByEmpresaRecomendacion(int afrrec)
        {
            return FactorySic.ObtenerEventoDao().ObtenerEmpresaRecomendacion(afrrec);
        }
        /// <summary>
        /// Actualiza en eve_evento si es evento AO
        /// </summary>
        /// <param name="evencodi"></param>
        /// <param name="evenrcmctaf"></param>
        /// <returns></returns>
        public void ActualizarEventoAO(int evencodi, string evenrcmctaf)
        {
            FactorySic.ObtenerEventoDao().ActualizarEventoAO(evencodi, evenrcmctaf);
        }


        /// <summary>
        /// Permite listar todas las empresas suministradora
        /// </summary>
        /// <param name="afecodi"></param>
        /// <returns></returns>
        public List<AfHoraCoordDTO> ListAfHoraCoordSuministradora(int afecodi)
        {
            return FactorySic.GetAfHoraCoordRepository().ListHoraCoordSuministradora(afecodi);
        }

        /// <summary>
        /// Obtener Nombre Archivo Formato InterruSumini
        /// </summary>
        /// <param name="eveEventoDTO"></param>
        /// <param name="rutaBase"></param>
        /// <param name="idtipoinformacion"></param>
        /// <param name="emprcodi"></param>
        /// <param name="afecodi"></param>
        /// <param name="listaInterrupcionSuministro"></param>
        /// <returns></returns>
        public string ObtenerNombreArchivoCoorSuministradora__(EventoDTO eveEventoDTO, string rutaBase, int idtipoinformacion, int emprcodi, int afecodi, List<AfInterrupSuministroDTO> listaInterrupcionSuministro)
        {
            string nombPlantilla = string.Empty;
            string nombArchivo = string.Empty;
            switch (idtipoinformacion)
            {
                case (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF:
                    nombPlantilla = ConstantesExtranetCTAF.FormatoInterrupciónPorActivaciónERACMF;
                    nombArchivo = "FORMATO INTERRUPCIÓN POR ACTIVACIÓN ERACMF";
                    break;
                case (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion:
                    nombPlantilla = ConstantesExtranetCTAF.FormatoInterrupciones;
                    nombArchivo = "FORMATO INTERRUPCIONES";
                    break;
                case (int)ConstantesExtranetCTAF.Fuentedato.ReduccionSuministros:
                    nombPlantilla = ConstantesExtranetCTAF.FormatoReducciónDeSuministros;
                    nombArchivo = "FORMATO REDUCCIÓN DE SUMINISTROS";
                    break;
            }

            return ObtenerExcelInterrupcionSuministro(rutaBase, nombPlantilla, nombArchivo, eveEventoDTO, idtipoinformacion, emprcodi, afecodi, listaInterrupcionSuministro);
        }



        /// <summary>
        /// Obtener Excel Interrupcion Suministro
        /// </summary>
        /// <param name="rutaBase"></param>
        /// <param name="nombPlantilla"></param>
        /// <param name="nombArchivoFormato"></param>
        /// <param name="eveEventoDTO"></param>
        /// <param name="fdatcodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="afecodi"></param>
        /// <param name="lstInterrSuministro"></param>
        /// <returns></returns>
        public string ObtenerNombreArchivoCoorSuministradora(string rutaBase, List<AfHoraCoordDTO> listaHorasCoordSuministradora)
        {

            string nombPlantilla = "PlantillaCTAF_FechaHoraSuministradora.xlsx";
            string nombArchivoFormato = "FORMATO FECHA Y HORAS SUMINISTRADORA";



            var nombCompletPlantilla = $"{rutaBase}{nombPlantilla}";
            var nombFormato = $"{nombArchivoFormato}{ConstantesAppServicio.ExtensionExcel}";
            var nombCompletFormato = $"{rutaBase}{nombFormato}";

            var archivoPlantilla = new FileInfo(nombCompletPlantilla);

            var nuevoArchivo = new FileInfo(nombCompletFormato);
            if (nuevoArchivo.Exists)
            {
                nuevoArchivo.Delete();
                nuevoArchivo = new FileInfo(nombCompletFormato);
            }

            using (var xlPackage = new ExcelPackage(archivoPlantilla))
            {
                using (var ws = xlPackage.Workbook.Worksheets[1])
                {

                    //Lista de datos de interrupciones
                    int rowIni = 5, colIni = 3;
                    int colIniDynamic = colIni, rowIniDynamic = rowIni;

                    //  this.FormaterDataExtranet(ref listaHorasCoordSuministradora);
                    ws.Protection.IsProtected = true;

                    foreach (var interr in listaHorasCoordSuministradora)
                    {
                        colIniDynamic = colIni;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.EmpresaSuministradora;
                        ws.Cells[rowIniDynamic, colIniDynamic++].Value = interr.Afhofechadescripcion;


                        rowIniDynamic++;
                    }
                    var modelTable = ws.Cells[rowIni, colIni, rowIniDynamic - 1, colIniDynamic - 1];
                    UtilExcel.AllBorders(modelTable);


                    xlPackage.SaveAs(nuevoArchivo);
                }
            }
            return nombFormato;
        }


        /// <summary>
        /// Obtener Interrupcion Suministro De Data Excel
        /// </summary>
        /// <param name="stremExcel"></param>
        /// <returns></returns>
        public List<AfHoraCoordDTO> ObtenerDataFechaCoorSuministradora(Stream stremExcel)
        {
            List<AfHoraCoordDTO> lstHoraCoorSuminsitradora = new List<AfHoraCoordDTO>();
            using (var xlPackage = new ExcelPackage(stremExcel))
            {
                var ws = xlPackage.Workbook.Worksheets[1];

                int rowIni = 4, colIni = 3;

                var dim = ws.Dimension;
                ExcelRange excelRange = ws.Cells[rowIni, colIni, dim.End.Row, dim.End.Column];
                var dataExcel = (object[,])excelRange.Value;

                var rowLast = dim.End.Row - rowIni;

                for (int i = 1; i <= rowLast; i++)
                {
                    var exIntsumzona = dataExcel[i, 0]?.ToString();
                    var exIntsumempresa = dataExcel[i, 1]?.ToString().PadLeft(19, '0');

                    lstHoraCoorSuminsitradora.Add(new AfHoraCoordDTO()
                    {
                        EmpresaSuministradora = exIntsumzona,
                        Afhofechadescripcion = exIntsumempresa

                    });
                }

            }

            return lstHoraCoorSuminsitradora;
        }

        /// <summary>
        /// Permite listar todas las empresas clientes por empresa suministradora
        /// </summary>
        /// <param name="eracmfsuministrador"></param>
        /// <returns></returns>
        public List<AfHoraCoordDTO> ListEmpClixSuministradora(string eracmfsuministrador)
        {
            return FactorySic.GetAfHoraCoordRepository().ListEmpClixSuministradora(eracmfsuministrador);
        }
        #endregion

        /// <summary>
        /// Actualiza descripcion de evento AF
        /// </summary>
        /// <param name="evencodi"></param>
        /// <param name="evendescctaf"></param>
        /// <returns></returns>
        public void ActualizarDesEventoAF(int evencodi, string evendescctaf)
        {
            FactorySic.ObtenerEventoDao().ActualizarDesEventoAF(evencodi, evendescctaf);
        }

        /// <summary>
        /// Obtener lista de interruptores y descargadores
        /// </summary>
        /// <param name="evencodi"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<EveintdescargaDTO> ObtenerListaInterruptoresDescargadores(int evencodi, int tipo)
        {
            return FactorySic.GetEveintdescargaRepository().List(evencodi, tipo);
        }

        /// <summary>
        /// Registra interruptores y descargadores en tabla eve_int_descarga
        /// </summary>
        /// <param name="Entidad"></param>
        /// <returns></returns>
        public int InsertarInterruptoresDescargadores(EveintdescargaDTO Entidad)
        {
            int id = 0;
            id = FactorySic.GetEveintdescargaRepository().Save(Entidad);
            return id;
        }

        /// <summary>
        /// Eliminar Interruptores y Descargadores
        /// </summary>
        /// <param name="evencodi"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public void EliminarInterruptoresDescargadores(int evencodi, int tipo)
        {
            FactorySic.GetEveintdescargaRepository().Delete(evencodi, tipo);
        }

        /// <summary>
        /// Registra condiciones previas
        /// </summary>
        /// <param name="Entidad"></param>
        /// <returns></returns>
        public int InsertarCondicionPrevia(EveCondPreviaDTO Entidad)
        {
            int id = 0;
            id = FactorySic.GetEveCondPreviaRepository().Save(Entidad);
            return id;
        }

        /// <summary>
        /// Actualizar condiciones previas
        /// </summary>
        /// <param name="Entidad"></param>
        /// <returns></returns>
        public void ActualizarCondicionPrevia(EveCondPreviaDTO Entidad)
        {
            FactorySic.GetEveCondPreviaRepository().Update(Entidad);

        }

        /// <summary>
        /// Eliminar condición previa
        /// </summary>
        /// <param name="evecondprcodi"></param>
        /// <returns></returns>
        public void EliminarCondicionPrevia(int evecondprcodi)
        {
            FactorySic.GetEveCondPreviaRepository().Delete(evecondprcodi);
        }

        /// <summary>
        /// Obtener Condición Previa
        /// </summary>
        /// <param name="evecondprcodi"></param>
        /// <returns></returns>
        public EveCondPreviaDTO GetByIdCondicionPrevia(int evecondprcodi)
        {
            return FactorySic.GetEveCondPreviaRepository().GetById(evecondprcodi);
        }

        /// <summary>
        /// Obtener Condición Previa
        /// </summary>
        /// <param name="evencodi"></param>
        /// <param name="evecondprtipo"></param>
        /// <param name="zona"></param>
        /// <param name="evecondprcodigounidad"></param>
        /// <returns></returns>
        public EveCondPreviaDTO GetByIdCanalZona(int evencodi, string evecondprtipo, int zona, string evecondprcodigounidad)
        {
            return FactorySic.GetEveCondPreviaRepository().GetByIdCanalZona(evencodi, evecondprtipo, zona, evecondprcodigounidad);
        }

        /// <summary>
        /// Obtener lista condiciones previas
        /// </summary>
        /// <param name="evencodi"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<EveCondPreviaDTO> ObtenerListaCondicionesPrevias(int evencodi, string tipo)
        {
            return FactorySic.GetEveCondPreviaRepository().List(evencodi, tipo);
        }

        /// <summary>
        /// Obtener lista de histórico scada en evento análisis
        /// </summary>
        /// <param name="evencodi"></param>
        /// <returns></returns>
        public List<EveHistoricoScadaDTO> ObtenerListaHistoricoScada(int evencodi)
        {
            return FactorySic.GetEveHistoricoScadaRepository().List(evencodi);
        }

        /// <summary>
        /// Obtener histórico scada en evento análisis
        /// </summary>
        /// <param name="evehistscdacodi"></param>
        /// <returns></returns>
        public EveHistoricoScadaDTO GetByIdHistoricoScada(int evehistscdacodi)
        {
            return FactorySic.GetEveHistoricoScadaRepository().GetById(evehistscdacodi);
        }

        /// <summary>
        /// Registra EveHistoricoScadaDTO
        /// </summary>
        /// <param name="Entidad"></param>
        /// <returns></returns>
        public int InsertarHistoricoScada(EveHistoricoScadaDTO Entidad)
        {
            int id = 0;
            //FactorySic.GetEveHistoricoScadaRepository().Delete(Entidad);
            id = FactorySic.GetEveHistoricoScadaRepository().Save(Entidad);
            return id;
        }

        /// <summary>
        /// Elimina EveHistoricoScadaDTO
        /// </summary>
        /// <returns></returns>
        public void EliminaHistoricoScada(int evencodi)
        {
            FactorySic.GetEveHistoricoScadaRepository().DeleteAll(evencodi);
        }

        /// <summary>
        /// Obtener lista de histórico scada en evento análisis
        /// </summary>
        /// <param name="evencodi"></param>
        /// <returns></returns>
        public List<EveAnalisisEventoDTO> ObtenerListaAnalisisEventos(int evencodi)
        {
            return FactorySic.GetEveAnalisisEventoRepository().List(evencodi);
        }

        /// <summary>
        /// Obtener Análisis evento
        /// </summary>
        /// <param name="eveanaevecodi"></param>
        /// <returns></returns>
        public EveAnalisisEventoDTO GetByIdEveAnalisisEvento(int eveanaevecodi)
        {
            return FactorySic.GetEveAnalisisEventoRepository().GetById(eveanaevecodi);
        }

        /// <summary>
        /// Registra EveAnalisisEventoDTO
        /// </summary>
        /// <param name="Entidad"></param>
        /// <returns></returns>
        public int InsertarAnalisisEvento(EveAnalisisEventoDTO Entidad)
        {
            int id = 0;
            id = FactorySic.GetEveAnalisisEventoRepository().Save(Entidad);
            return id;
        }

        /// <summary>
        /// Actualiza EveAnalisisEventoDTO
        /// </summary>
        /// <param name="Entidad"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public void ActualizarAnalisisEvento(EveAnalisisEventoDTO Entidad, int tipo)
        {
            if (tipo == 1)
                FactorySic.GetEveAnalisisEventoRepository().Update(Entidad);
            else if (tipo == 2)
                FactorySic.GetEveAnalisisEventoRepository().UpdateDescripcion(Entidad);
        }

        /// <summary>
        /// Elimina EveAnalisisEventoDTO
        /// </summary>
        /// <param name="evencodi"></param>
        /// <returns></returns>
        public void EliminaAnalisisEvento(int evencodi)
        {
            FactorySic.GetEveAnalisisEventoRepository().Delete(evencodi);
        }

        /// <summary>
        /// Elimina EveAnalisisEventoDTO
        /// </summary>
        /// <param name="eveanaevecodi"></param>
        /// <returns></returns>
        public void EliminarAnalisisxEvento(int eveanaevecodi)
        {
            FactorySic.GetEveAnalisisEventoRepository().DeleteAnalisis(eveanaevecodi);
        }

        /// <summary>
        /// Validar Numeral x Análisis de evento
        /// </summary>
        /// <param name="evetipnumcodi"></param>
        /// <returns></returns>
        public bool ValidarTipoNumeralxAnalisisEvento(int evetipnumcodi)
        {
            return FactorySic.GetEveAnalisisEventoRepository().ValidarTipoNumeralxAnalisisEvento(evetipnumcodi);
        }

        /// <summary>
        /// Obtener análisis de falla
        /// </summary>
        /// <param name="evencodi"></param>
        /// <returns></returns>
        public List<EveRecomobservDTO> ListEveRecomobserv(int evencodi, int tipo)
        {
            return FactorySic.GetEveRecomobservRepository().List(evencodi, tipo);
        }

        /// <summary>
        /// Registra EveRecomobservDTO
        /// </summary>
        /// <param name="Entidad"></param>
        /// <returns></returns>
        public int InsertarEveRecomobserv(EveRecomobservDTO Entidad)
        {
            int id = 0;
            id = FactorySic.GetEveRecomobservRepository().Save(Entidad);
            return id;
        }

        /// <summary>
        /// Eliminar EveRecomobservDTO
        /// </summary>
        /// <param name="everecomobservcodi"></param>
        /// <returns></returns>
        public void EliminarObservacionRecomendacion(int everecomobservcodi)
        {
            FactorySic.GetEveRecomobservRepository().Delete(everecomobservcodi);
        }

        /// <summary>
        /// Obtener tipos de numerales
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        public List<EveTiposNumeralDTO> ListaTiposNumeral(string estado)
        {
            return FactorySic.GetEveTiposNumeralRepository().List(estado);
        }

        /// <summary>
        /// Registra EveAnalisisEventoDTO
        /// </summary>
        /// <param name="Entidad"></param>
        /// <returns></returns>
        public int InsertarTiposNumeral(EveTiposNumeralDTO Entidad)
        {
            int id = 0;
            id = FactorySic.GetEveTiposNumeralRepository().Save(Entidad);
            return id;
        }
        /// <summary>
        /// Eliminar Tipo de Numeral
        /// </summary>
        /// <param name="evetipnumcodi"></param>
        /// <returns></returns>
        public void EliminarTiposNumeral(int evetipnumcodi)
        {
            FactorySic.GetEveTiposNumeralRepository().Delete(evetipnumcodi);
        }
        /// <summary>
        /// Registra EveAnalisisEventoDTO
        /// Elimina Tipos de numeral
        /// </summary>
        /// <returns></returns>
        public void ActualizarTiposNumeral(EveTiposNumeralDTO Entidad)
        {
            FactorySic.GetEveTiposNumeralRepository().Update(Entidad);
        }

        /// <summary>
        /// Consultar analisis de falla por año
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public List<EventoDTO> ConsultarAnalisisFallasAnio(int anio)
        {
            return FactorySic.ObtenerEventoDao().ConsultarAnalisisFallasAnio(anio);
        }

        /// <summary>
        /// Genera el documento de decision CTAF
        /// </summary>
        /// <param name="analisisFallaId"></param>
        /// <param name="eventoAppServicio"></param>
        /// <param name="logo"></param>
        /// <param name="subcarpeta"></param>
        /// <param name="fileserverSev"></param>
        /// <param name="formatoFecha"></param>
        /// <returns></returns>
        public GenerarDecisionCtafFDTO GenerarDecisionCTAF(int analisisFallaId, EventosAppServicio eventoAppServicio, string logo, string subcarpeta, string fileserverSev, string formatoFecha, string imagenLinea)
        {
            var infoDecisionCtaf = ObtenerInformaciónDecisionCTAF(analisisFallaId, eventoAppServicio);
            var fileName = ObtenerCarpetaSEV(infoDecisionCtaf.AnalisisFalla, eventoAppServicio, infoDecisionCtaf.EventosSCO, subcarpeta, fileserverSev, formatoFecha);
            return wordDocument.DecisionCTAF(infoDecisionCtaf, logo, fileName, imagenLinea);
        }

        public string ObtenerCarpetaSEV(AnalisisFallaDTO oAnalisisFallaDTO, EventosAppServicio eventoAppServicio, List<EventoDTO> ListaEventosSco, string SubCarpeta, string fileserverSev, string formatoFecha)
        {
            string fileSev = string.Empty;

            EventoDTO primer_evento = ListaEventosSco.OrderBy(c => c.EVENINI).FirstOrDefault();
            if (primer_evento != null)
            {
                oAnalisisFallaDTO = eventoAppServicio.ObtenerAnalisisFalla(Convert.ToInt32(primer_evento.EVENCODI));
            }

            string aaaa = primer_evento.EVENINI.Value.ToString("yyyy");
            //string asunto = (RemoveAccentsWithRegEx(Regex.Replace(oAnalisisFallaDTO.EVENASUNTO, "[\t@,\\\"/:*?<>|\\\\]", string.Empty)).TrimEnd()).TrimStart();

            //int maxcaracteres = ConstantesEvento.MaxCaractAF;
            //if (asunto.Length > maxcaracteres)
            //    asunto = asunto.Substring(0, maxcaracteres).Trim();

            //string NombreEvento = oAnalisisFallaDTO.CODIGO + "_" + asunto + "_" + oAnalisisFallaDTO.EVENINI.Value.ToString("dd.MM.yyyy");
            string NombreEvento = oAnalisisFallaDTO.CODIGO + "_" + oAnalisisFallaDTO.EVENINI.Value.ToString("dd.MM.yyyy");
            DateTime FechaFinSem1 = DateTime.ParseExact(ConstantesEvento.FechaFinSem1 + aaaa, formatoFecha, CultureInfo.InvariantCulture);
            DateTime FechaInicioSem2 = DateTime.ParseExact(ConstantesEvento.FechaInicioSem2 + aaaa, formatoFecha, CultureInfo.InvariantCulture);
            DateTime FechaFinSem2 = DateTime.ParseExact(ConstantesEvento.FechaFinSem2 + aaaa, formatoFecha, CultureInfo.InvariantCulture);
            DateTime FechaEvento = DateTime.ParseExact(oAnalisisFallaDTO.EVENINI.Value.ToString("dd/MM/yyyy"), formatoFecha, CultureInfo.InvariantCulture);

            string semestre = string.Empty;

            if (FechaEvento <= FechaFinSem1)
            {
                semestre = "Semestre I";
            }
            else if (FechaEvento >= FechaInicioSem2 && FechaEvento <= FechaFinSem2)
            {
                semestre = "Semestre II";
            }

            fileSev = fileserverSev + aaaa + "\\" + semestre + "\\" + NombreEvento + "\\" + SubCarpeta;
            string file = aaaa + "\\" + semestre + "\\" + NombreEvento + "\\" + SubCarpeta;

            if (!System.IO.File.Exists(fileSev))
            {
                FileServer.CreateFolder(string.Empty, file, fileserverSev);
            }

            return fileSev;
        }

        public InfoDecisionCTAF ObtenerInformaciónDecisionCTAF(int analisisFallaId, EventosAppServicio eventoAppServicio)
        {
            var tempAnalisisFalla = ObtenerAnalisisFalla(analisisFallaId);
            var tempListaEventosSco = LstEventosSco(tempAnalisisFalla.AFEANIO.ToString(), tempAnalisisFalla.AFECORR.ToString());
            var tempEquipoDTO = ObtenerEquipoPorEvento(tempAnalisisFalla.EVENCODI);
            var tempEventoCTAF = ObtenerEvento(tempAnalisisFalla.EVENCODI);
            var tempInformesFinales = new List<EveInformesScoDTO>();

            foreach (var reg in tempListaEventosSco)
            {
                var tempEmpresasInvolucradas = ObtenerEmpresasInvolucradaxEvencodi((int)reg.EVENCODI);
                var lstInformesFinales = eventoAppServicio.ListEveInformesScoxEvento((int)reg.EVENCODI, 2).ToList();

                if (lstInformesFinales.Count == 0)
                {
                    var lstInformesPreliminares = eventoAppServicio.ListEveInformesScoxEvento((int)reg.EVENCODI, 1).ToList();

                    if (lstInformesPreliminares.Count > 0)
                    {
                        tempInformesFinales.AddRange(lstInformesPreliminares);
                    }
                }
                else
                {
                    tempInformesFinales.AddRange(lstInformesFinales);
                }

                if (tempEmpresasInvolucradas.Count > 0)
                {
                    foreach (var x in tempEmpresasInvolucradas)
                    {
                        if (lstInformesFinales.Count > 0)
                        {
                            int countInfo = lstInformesFinales.Where(y => y.Afecodi == x.AFECODI && y.Portalweb == "S" && y.Emprcodi == x.EMPRCODI).Count();
                            if (countInfo == 0)
                            {
                                lstInformesFinales.Add(new EveInformesScoDTO()
                                {
                                    Afecodi = x.AFECODI,
                                    Emprcodi = x.EMPRCODI,
                                    Emprnomb = x.EMPRNOMB,
                                    Version = x.VERSION,
                                    Cumplimiento = x.CUMPLIMIENTO,
                                    Portalweb = x.AFIPUBLICA,
                                    Afiversion = Convert.ToInt32(x.AFIVERSION),
                                    Tipodata = "A"
                                });
                            }
                        }
                        else
                        {
                            lstInformesFinales.Add(new EveInformesScoDTO()
                            {
                                Afecodi = x.AFECODI,
                                Emprcodi = x.EMPRCODI,
                                Emprnomb = x.EMPRNOMB,
                                Version = x.VERSION,
                                Cumplimiento = x.CUMPLIMIENTO,
                                Portalweb = x.AFIPUBLICA,
                                Afiversion = Convert.ToInt32(x.AFIVERSION),
                                Tipodata = "A"
                            });
                        }

                    }
                }
            }

            int empresaCodigo = -1;
            int fuentaDatoCodigo = 0;

            var objEmpresa = empresaCodigo > 0 ? eventoAppServicio.ObtenerEmpresa(empresaCodigo) : null;

            bool esPorEracmf = false;

            if (tempAnalisisFalla.AFEERACMF == "S")
            {
                esPorEracmf = true;
                fuentaDatoCodigo = (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF;
            }
            else
            {
                fuentaDatoCodigo = (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion;
            }

            ListarInterrupcionSuministrosGral(tempAnalisisFalla.AFECODI, empresaCodigo, fuentaDatoCodigo, out List<AfInterrupSuministroDTO> listaData, out List<AfInterrupSuministroDTO> listDataReportCero, out List<string> listaMsjValidacion, out List<int> listaEmprcodiReportaron, tempAnalisisFalla.AFEANIO.ToString(), tempAnalisisFalla.AFECORR.ToString());
            var lstHandsonHorasCoordinacion = ObtenerListaCruceHoracoordInterrupcion(tempAnalisisFalla.AFECODI, fuentaDatoCodigo, listaData, tempAnalisisFalla.ANIO, tempAnalisisFalla.AFECORR.ToString());

            ListarRptTotalDatos(tempAnalisisFalla.AFECODI, empresaCodigo, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte1, out bool formatFechaCab1);
            TablaReporte tempTablaReporteTotales = ObtenerDataExcelDatosTotales(listaReporte1, esPorEracmf, formatFechaCab1);

            ListarRptDecision(tempAnalisisFalla.AFECODI, empresaCodigo, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte3, out bool formatFechaCab3, esPorEracmf);
            TablaReporte tempTablaReporteDecision = ObtenerDataExcelDecision(listaReporte3, formatFechaCab3);

            ListarRptResarcimientoNoERACMF(tempAnalisisFalla.AFECODI, empresaCodigo, listaData, lstHandsonHorasCoordinacion, out List<ReporteInterrupcion> listaReporte9, out bool formatFechaCab);
            TablaReporte tempTablaResarcimiento = ObtenerDataExcelResarcimiento(listaReporte9, formatFechaCab);

            return new InfoDecisionCTAF()
            {
                AnalisisFalla = tempAnalisisFalla,
                EventosSCO = tempListaEventosSco,
                Equipo = tempEquipoDTO,
                EventoCTAF = tempEventoCTAF,
                InformesFinales = tempInformesFinales,
                TablaReporte = tempTablaReporteTotales,
                TablaResarcimiento = tempTablaResarcimiento,
                TablaDecision = tempTablaReporteDecision,
                AnalisisFallasAppServicio = this,
            };
        }
        /// <summary>
        /// Muestra cant de días hábiles
        /// </summary>
        /// <param name="FechaInicio"></param>
        /// <param name="FechaFin"></param>
        /// <returns></returns>
        public int CantDiasHabiles(DateTime FechaInicio, DateTime FechaFin)
        {
            return FactorySic.GetDocDiaEspRepository().NumDiasHabiles(FechaInicio, FechaFin);
        }

        public static string RemoveAccentsWithRegEx(string inputString)
        {
            Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex replace_A_Accents = new Regex("[Á|À|Ä|Â]", RegexOptions.Compiled);
            Regex replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex replace_E_Accents = new Regex("[É|È|Ë|Ê]", RegexOptions.Compiled);
            Regex replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex replace_I_Accents = new Regex("[Í|Ì|Ï|Î]", RegexOptions.Compiled);
            Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex replace_O_Accents = new Regex("[Ó|Ò|Ö|Ô]", RegexOptions.Compiled);
            Regex replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            Regex replace_U_Accents = new Regex("[Ú|Ù|Ü|Û]", RegexOptions.Compiled);

            inputString = replace_a_Accents.Replace(inputString, "a");
            inputString = replace_A_Accents.Replace(inputString, "A");
            inputString = replace_e_Accents.Replace(inputString, "e");
            inputString = replace_E_Accents.Replace(inputString, "E");
            inputString = replace_i_Accents.Replace(inputString, "i");
            inputString = replace_I_Accents.Replace(inputString, "I");
            inputString = replace_o_Accents.Replace(inputString, "o");
            inputString = replace_O_Accents.Replace(inputString, "O");
            inputString = replace_u_Accents.Replace(inputString, "u");
            inputString = replace_U_Accents.Replace(inputString, "U");
            return inputString;
        }
    }

}
