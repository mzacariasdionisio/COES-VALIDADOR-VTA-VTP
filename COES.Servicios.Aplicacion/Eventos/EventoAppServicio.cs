using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Intervenciones;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace COES.Servicios.Aplicacion.Eventos
{
    public class EventoAppServicio : AppServicioBase
    {
        ///// <summary>
        ///// Permite obtener las empresa por tipo
        ///// </summary>
        ///// <returns></returns>
        //public List<SiEmpresaDTO> ObtenerEmpresasPorTipo(string tiposEmpresa)
        //{
        //    if (string.IsNullOrEmpty(tiposEmpresa)) tiposEmpresa = ConstantesAppServicio.ParametroDefecto;
        //    return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasPorTipo(tiposEmpresa);
        //}

        /// <summary>
        /// Permite listar los tipos de eventos
        /// </summary>
        /// <returns></returns>
        public List<TipoEventoDTO> ListarTipoEvento()
        {
            return FactorySic.ObtenerTipoEventoDao().ListarTipoEvento();
        }

        /// <summary>
        /// Permite listar los tipos de eventos
        /// </summary>
        /// <returns></returns>
        public List<EveTipoeventoDTO> ListarTipoEventoMantto()
        {
            List<int> lTipoevencodiMantto = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 9, 10, 12 };
            return FactorySic.GetEveTipoeventoRepository().GetByCriteria().Where(x => lTipoevencodiMantto.Contains(x.Tipoevencodi)).OrderBy(x => x.Tipoevencodi).ToList();
        }

        /// <summary>
        /// Permite listar las causas de ocurrencia de los eventos
        /// </summary>
        /// <param name="idTipoEvento"></param>
        /// <returns></returns>
        public List<SubCausaEventoDTO> ObtenerCausaEvento(int? idTipoEvento)
        {
            return FactorySic.ObtenerSubCausaEventoDao().ObtenerCausaEvento(idTipoEvento);
        }

        /// <summary>
        /// Permite buscar los eventos segun criterios especificados
        /// </summary>
        /// <param name="idTipoEvento"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="version"></param>
        /// <param name="turno"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoEquipo"></param>
        /// <returns></returns>
        public List<EventoDTO> BuscarEventos(int? idTipoEvento, DateTime fechaInicio, DateTime fechaFin,
            string version, string turno, int? idTipoEmpresa, int? idEmpresa, int? idTipoEquipo, string indInterrupcion,
            int nroPage, int nroFilas, string campo, string orden, string areaoperativa, int todosaseg)
        {
            string filtro = string.Empty;

            if (!string.IsNullOrEmpty(turno))
            {
                if (turno == ConstantesEvento.Turno1)
                {
                    filtro = String.Format(ConstantesEvento.FiltroFechaEvento, fechaInicio.ToString(ConstantesEvento.FormatoFechaExtendido),
                        fechaInicio.AddHours(7).ToString(ConstantesEvento.FormatoFechaExtendido));
                }
                if (turno == ConstantesEvento.Turno2)
                {
                    filtro = String.Format(ConstantesEvento.FiltroFechaEvento, fechaInicio.AddHours(7).ToString(ConstantesEvento.FormatoFechaExtendido),
                            fechaInicio.AddHours(15).ToString(ConstantesEvento.FormatoFechaExtendido));
                }
                if (turno == ConstantesEvento.Turno3)
                {
                    filtro = String.Format(ConstantesEvento.FiltroFechaEvento, fechaInicio.AddHours(15).ToString(ConstantesEvento.FormatoFechaExtendido),
                                fechaInicio.AddDays(1).ToString(ConstantesEvento.FormatoFechaExtendido));
                }
            }

            return FactorySic.ObtenerEventoDao().BuscarEventos(idTipoEvento, fechaInicio, fechaFin, version, filtro, idTipoEmpresa,
                idEmpresa, idTipoEquipo, indInterrupcion, nroPage, nroFilas, campo, orden, areaoperativa, todosaseg);
        }

        /// <summary>
        /// Permite obtener el número de filas de la consulta a ejecutar
        /// </summary>
        /// <param name="idTipoEvento"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="version"></param>
        /// <param name="turno"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoEquipo"></param>
        /// <returns></returns>
        public int ObtenerNroFilasEvento(int? idTipoEvento, DateTime fechaInicio, DateTime fechaFin,
            string version, string turno, int? idTipoEmpresa, int? idEmpresa, int? idTipoEquipo, string indInterrupcion, 
            string areaOperativa, int todosaseg)
        {
            string filtro = string.Empty;

            if (!string.IsNullOrEmpty(turno))
            {
                if (turno == ConstantesEvento.Turno1)
                {
                    filtro = String.Format(ConstantesEvento.FiltroFechaEvento, fechaInicio.ToString(ConstantesEvento.FormatoFechaExtendido),
                        fechaInicio.AddHours(7).ToString(ConstantesEvento.FormatoFechaExtendido));
                }
                if (turno == ConstantesEvento.Turno2)
                {
                    filtro = String.Format(ConstantesEvento.FiltroFechaEvento, fechaInicio.AddHours(7).ToString(ConstantesEvento.FormatoFechaExtendido),
                            fechaInicio.AddHours(15).ToString(ConstantesEvento.FormatoFechaExtendido));
                }
                if (turno == ConstantesEvento.Turno3)
                {
                    filtro = String.Format(ConstantesEvento.FiltroFechaEvento, fechaInicio.AddHours(15).ToString(ConstantesEvento.FormatoFechaExtendido),
                                fechaInicio.AddDays(1).ToString(ConstantesEvento.FormatoFechaExtendido));
                }
            }

            return FactorySic.ObtenerEventoDao().ObtenerNroRegistros(idTipoEvento, fechaInicio,
                fechaFin, version, filtro, idTipoEmpresa, idEmpresa, idTipoEquipo, indInterrupcion, areaOperativa, todosaseg);
        }

        /// <summary>
        /// Permite obtener los eventos para generar el archivo de exportación
        /// </summary>
        /// <param name="idTipoEvento"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="version"></param>
        /// <param name="turno"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoEquipo"></param>
        /// <param name="indInterrupcion"></param>       
        /// <returns></returns>
        public List<EventoDTO> ExportarEventos(int? idTipoEvento, DateTime fechaInicio, DateTime fechaFin,
            string version, string turno, int? idTipoEmpresa, int? idEmpresa, int? idTipoEquipo, string indInterrupcion, int indicador, string areaOperativa)
        {
            string filtro = string.Empty;

            if (!string.IsNullOrEmpty(turno))
            {
                if (turno == ConstantesEvento.Turno1)
                {
                    filtro = String.Format(ConstantesEvento.FiltroFechaEvento, fechaInicio.ToString(ConstantesEvento.FormatoFechaExtendido),
                        fechaInicio.AddHours(7).ToString(ConstantesEvento.FormatoFechaExtendido));
                }
                if (turno == ConstantesEvento.Turno2)
                {
                    filtro = String.Format(ConstantesEvento.FiltroFechaEvento, fechaInicio.AddHours(7).ToString(ConstantesEvento.FormatoFechaExtendido),
                            fechaInicio.AddHours(15).ToString(ConstantesEvento.FormatoFechaExtendido));
                }
                if (turno == ConstantesEvento.Turno3)
                {
                    filtro = String.Format(ConstantesEvento.FiltroFechaEvento, fechaInicio.AddHours(15).ToString(ConstantesEvento.FormatoFechaExtendido),
                                fechaInicio.AddDays(1).ToString(ConstantesEvento.FormatoFechaExtendido));
                }
            }

            List<EventoDTO> list = new List<EventoDTO>();

            if (indicador == 0)
                list = FactorySic.ObtenerEventoDao().ExportarEventos((idTipoEvento ?? 0).ToString(), fechaInicio, fechaFin, version, filtro, idTipoEmpresa,
                    (idEmpresa ?? 0).ToString(), (idTipoEquipo ?? 0).ToString(), indInterrupcion, areaOperativa);
            else
                list = FactorySic.ObtenerEventoDao().ExportarEventosDetallado(idTipoEvento, fechaInicio, fechaFin, version, filtro, idTipoEmpresa,
                idEmpresa, idTipoEquipo, indInterrupcion, areaOperativa);

            return list;
        }

        /// <summary>
        /// Permite obtener algunos datos principales del evento
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public EventoDTO ObtenerResumenEvento(int idEvento)
        {
            return FactorySic.ObtenerEventoDao().ObtenerResumenEvento(idEvento);
        }

        /// <summary>
        /// Permite obtener los datos de un evento en particular
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public EventoDTO ObtenerEvento(int idEvento)
        {
            return FactorySic.ObtenerEventoDao().ObtenerEvento(idEvento);
        }

        /// <summary>
        /// Permite actualizar un evento que tiene relacionado un reporte de perturbaciones
        /// </summary>
        /// <param name="idEvento"></param>
        public void ActualizarIndicadorReporteEvento(int idEvento)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Permite obtener los items del reporte de perturbacion de un evento
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public List<InformePerturbacionDTO> ObtenerInformePorEvento(int idEvento)
        {
            return FactorySic.ObtenerInformePerturbacionDao().ObtenerInformePorEvento(idEvento);
        }

        /// <summary>
        /// Permite obtener un item del reporte de perturbaciones de un evento
        /// </summary>
        /// <param name="idInformePerturbacion"></param>
        /// <returns></returns>
        public InformePerturbacionDTO ObtenerItemReporteEvento(int idInformePerturbacion)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Permite grabar o editar los items del reporte de perturbacion
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GrabarInformePerturbacion(List<InformePerturbacionDTO> entitys, int idEvento, string usuario)
        {
            try
            {
                FactorySic.ObtenerInformePerturbacionDao().EliminarInforme(idEvento);

                foreach (InformePerturbacionDTO item in entitys)
                {
                    item.EVENCODI = idEvento;
                    item.LASTUSER = usuario;
                    item.LASTDATE = DateTime.Now;
                }

                FactorySic.ObtenerInformePerturbacionDao().GrabarInforme(entitys);
                FactorySic.ObtenerEventoDao().ActualizarInformePerturbacion(ConstantesEvento.SI, idEvento);

                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Permite quitar un item del reporte de perturbacion
        /// </summary>
        /// <param name="idInformePerturbacion"></param>
        public void EliminarInformePerturbacion(int idEvento)
        {
            try
            {
                FactorySic.ObtenerInformePerturbacionDao().EliminarInforme(idEvento);
                FactorySic.ObtenerEventoDao().ActualizarInformePerturbacion(ConstantesEvento.NO, idEvento);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar las areas operativas por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<AreaDTO> ObtenerAreaPorEmpresa(int? idEmpresa, string idFamilia)
        {
            return FactorySic.ObtenerEventoDao().ObtenerAreaPorEmpresa(idEmpresa, idFamilia);
        }

        /// <summary>
        /// Permite buscar equipos segun los criterios especificados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idArea"></param>
        /// <param name="idFamilia"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public List<EquipoDTO> BuscarEquipoEventoIntervenciones(int? idEmpresa, int? idArea, string idFamilia, string filtro, int nroPagina, int nroFilas)
        {
            return FactorySic.ObtenerEventoDao().BuscarEquipoEventoIntervenciones(idEmpresa.GetValueOrDefault(0).ToString(), idArea, idFamilia, filtro, nroPagina, nroFilas)
                            .OrderBy(x => x.AREANOMB).ThenBy(x => x.EMPRENOMB).ThenBy(x => x.FAMABREV).ThenBy(x => x.EQUIABREV).ToList();
        }

        /// <summary>
        /// Listar equipos actualizados para intervenciones
        /// </summary>
        /// <returns></returns>
        public List<EquipoDTO> ObtenerEquiposIntervencionesActualizados()
        {
            return FactorySic.ObtenerEventoDao().BuscarEquiposIntervencionesActualizados().OrderBy(x => x.EQUIABREV).ToList();
        }

        /// <summary>
        /// Permite buscar equipos segun los criterios especificados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idArea"></param>
        /// <param name="idFamilia"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public List<EquipoDTO> BuscarEquipoEvento(int? idEmpresa, int? idArea, string idFamilia, string filtro, int nroPagina, int nroFilas)
        {
            return FactorySic.ObtenerEventoDao().BuscarEquipoEvento(idEmpresa.GetValueOrDefault(0).ToString(), idArea, idFamilia, filtro, nroPagina, nroFilas)
                            .OrderBy(x => x.AREANOMB).ThenBy(x => x.EMPRENOMB).ThenBy(x => x.FAMABREV).ThenBy(x => x.EQUIABREV).ToList();
        }

        /// <summary>
        /// Permite obtener el nro de items del resultado de la busqueda de equipos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idArea"></param>
        /// <param name="idFamilia"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public int ObtenerNroFilasBusquedaEquipo(int? idEmpresa, int? idArea, string idFamilia, string filtro)
        {
            return FactorySic.ObtenerEventoDao().ObtenerNroFilasBusquedaEquipo(idEmpresa.GetValueOrDefault(0).ToString(), idArea, idFamilia, filtro);
        }

        /// <summary>
        /// Permite buscar equipos segun los criterios especificados para INTERVENCIONES EXTRANET
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idArea"></param>
        /// <param name="idFamilia"></param>
        /// <param name="filtro"></param>
        /// <param name="nroPagina"></param>
        /// <param name="nroFilas"></param>
        /// <param name="tipoEmpr"></param>
        /// <returns></returns>
        public List<EquipoDTO> BuscarEquipoEventoExtranet(string idEmpresa, int? idArea, string idFamilia, string filtro, int nroPagina, int nroFilas, int tipoEmpr)
        {
            return FactorySic.ObtenerEventoDao().BuscarEquipoEventoExtranet(idEmpresa, idArea, idFamilia, filtro, nroPagina, nroFilas, tipoEmpr);
        }

        /// <summary>
        /// Permite obtener el nro de items del resultado de la busqueda de equipos para INTERVENCIONES EXTRANET
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idArea"></param>
        /// <param name="idFamilia"></param>
        /// <param name="filtro"></param>
        /// <param name="tipoEmpr"></param>
        /// <returns></returns>
        public int ObtenerNroFilasBusquedaEquipoExtranet(string idEmpresa, int? idArea, string idFamilia, string filtro, int tipoEmpr)
        {
            return FactorySic.ObtenerEventoDao().ObtenerNroFilasBusquedaEquipoExtranet(idEmpresa, idArea, idFamilia, filtro, tipoEmpr);
        }


        /// <summary>
        /// Permite listar las empresas
        /// </summary>
        /// <returns></returns>
        public List<EmpresaDTO> ListarEmpresas()
        {
            return FactorySic.ObtenerEventoDao().ListarEmpresas();
        }

        /// <summary>
        /// Permite listar las empresas por tipo
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        public List<EmpresaDTO> ListarEmpresasPorTipo(int idTipoEmpresa)
        {
            return FactorySic.ObtenerEventoDao().ListarEmpresasPorTipo(idTipoEmpresa);
        }

        /// <summary>
        /// Permite listar las familias
        /// </summary>
        /// <returns></returns>
        public List<FamiliaDTO> ListarFamilias()
        {
            return FactorySic.ObtenerEventoDao().ListarFamilias();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_TIPOEMPRESA
        /// </summary>
        public List<SiTipoempresaDTO> ListarTipoEmpresas()
        {
            return FactorySic.GetSiTipoempresaRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_CAUSAEVENTO
        /// </summary>
        public List<EveCausaeventoDTO> ListarCausasEventos()
        {
            return FactorySic.GetEveCausaeventoRepository().List();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTipoEvento"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="version"></param>
        /// <param name="turno"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoEquipo"></param>
        /// <param name="informe"></param>
        /// <param name="indInterrupcion"></param>
        /// <param name="nroPage"></param>
        /// <param name="nroFilas"></param>
        /// <returns></returns>
        public List<EveManttoDTO> BuscarMantenimientos(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
            string tiposEmpresa, string empresas, string idsTipoEquipo, string indInterrupcion, string idstipoMantto, int nroPagina, int nroFilas)
        {
            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = (new ConsultaMedidoresAppServicio()).ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                //empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }
            if (string.IsNullOrEmpty(idsTipoMantenimiento)) idsTipoMantenimiento = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(tiposEmpresa)) tiposEmpresa = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(empresas)) empresas = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(idsTipoEquipo)) idsTipoEquipo = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(idstipoMantto)) idstipoMantto = ConstantesAppServicio.ParametroNulo;

            return FactorySic.GetEveManttoRepository().BuscarMantenimientos(idsTipoMantenimiento, fechaInicio, fechaFin,
                indispo, tiposEmpresa, empresas, idsTipoEquipo, indInterrupcion, idstipoMantto, nroPagina, nroFilas);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsTipoMantenimiento"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="indispo"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="idsTipoEquipo"></param>
        /// <param name="indInterrupcion"></param>
        /// <param name="idstipoMantto"></param>
        /// <returns></returns>
        public int ObtenerNroFilasMantenimiento(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
             string tiposEmpresa, string empresas, string idsTipoEquipo, string indInterrupcion, string idstipoMantto)
        {
            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = (new ConsultaMedidoresAppServicio()).ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                //empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }
            if (string.IsNullOrEmpty(idsTipoMantenimiento)) idsTipoMantenimiento = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(tiposEmpresa)) tiposEmpresa = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(empresas)) empresas = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(idsTipoEquipo)) idsTipoEquipo = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(idstipoMantto)) idstipoMantto = ConstantesAppServicio.ParametroNulo;

            return FactorySic.GetEveManttoRepository().ObtenerNroRegistros(idsTipoMantenimiento, fechaInicio,
                fechaFin, indispo, tiposEmpresa, empresas, idsTipoEquipo, indInterrupcion, idstipoMantto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsTipoMantenimiento"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="indispo"></param>
        /// <param name="idsTipoEmpresa"></param>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsTipoEquipo"></param>
        /// <param name="indInterrupcion"></param>
        /// <param name="idsTipoMantto"></param>
        /// <returns></returns>
        public List<EveManttoDTO> GenerarReportesGrafico(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
          string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idsTipoMantto)
        {
            List<EveManttoDTO> entitys = new List<EveManttoDTO>();
            if (idsTipoEmpresa != ConstantesAppServicio.ParametroDefecto && idsEmpresa == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = (new ConsultaMedidoresAppServicio()).ObteneEmpresasPorTipo(idsTipoEmpresa).Select(x => x.Emprcodi).ToList();
                //idsEmpresa = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }
            if (string.IsNullOrEmpty(idsTipoMantenimiento)) idsTipoMantenimiento = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(idsTipoEmpresa)) idsTipoEmpresa = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(idsTipoEquipo)) idsTipoEquipo = ConstantesAppServicio.ParametroNulo;
            entitys = FactorySic.GetEveManttoRepository().ObtenerReporteMantenimientos(idsTipoMantenimiento, fechaInicio, fechaFin, indispo,
             idsTipoEmpresa, idsEmpresa, idsTipoEquipo, indInterrupcion, idsTipoMantto);
            return entitys;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EveEvenclaseDTO> ListarClaseEventos()
        {
            return FactorySic.GetEveEvenclaseRepository().List();
        }

        /// <summary>
        /// Permite obtener los mantenimiento por equipo, clase y fecha
        /// </summary>
        /// <param name="idsEquipo">Códigos de equipo (separados por coma)</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="evenClase">Código de Evenclase</param>
        /// <returns></returns>
        public List<EveManttoDTO> ObtenerManttoEquipoClaseFecha(string idsEquipo, string fechaInicio, string fechaFin, int evenClase)
        {
            return FactorySic.GetEveManttoRepository().ObtenerManttoEquipoClaseFecha(idsEquipo, fechaInicio, fechaFin, evenClase);
        }

        /// <summary>
        /// Permite obtener los mantenimientos de acuerdo a equipos, subcausa, clase y fecha
        /// </summary>
        /// <param name="idsEquipo">Códigos de equipo (separados por coma)</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="evenClase">Código de Evenclase</param>
        /// <param name="subcausaCodi">Código de Subcausa</param>
        /// <returns></returns>
        public List<EveManttoDTO> ObtenerManttoEquipoSubcausaClaseFecha(string idsEquipo, string fechaInicio, string fechaFin, int evenClase, int subcausaCodi)
        {
            return FactorySic.GetEveManttoRepository().ObtenerManttoEquipoSubcausaClaseFecha(idsEquipo, fechaInicio, fechaFin, evenClase, subcausaCodi);
        }

        /// <summary>
        /// Permite obtener los mantenimiento de los equipos Padre por clase y fecha
        /// </summary>
        /// <param name="idsEquipo">Códigos de equipo (separados por coma)</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="evenClase">Código de Evenclase</param>
        /// <returns></returns>
        public List<EveManttoDTO> ObtenerManttoEquipoPadreClaseFecha(string idsEquipo, string fechaInicio, string fechaFin, int evenClase)
        {
            return FactorySic.GetEveManttoRepository().ObtenerManttoEquipoPadreClaseFecha(idsEquipo, fechaInicio, fechaFin, evenClase);
        }

        /// <summary>
        /// Permite obtener los mantenimientos para el portal Web
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<EveManttoDTO> ObtenerMantenimientosProgramados(DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetEveManttoRepository().ObtenerMantenimientosProgramados(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Permite obtener el listado de eventos para el movil
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<EveManttoDTO> ObtenerMantenimientosProgramadosMovil(DateTime fechaInicio, DateTime fechaFin, int tipo)
        {
            return FactorySic.GetEveManttoRepository().ObtenerMantenimientosProgramadosMovil(fechaInicio, fechaFin, tipo);
        }

        /// <summary>
        /// Sobreescritura el metodo dispose
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Permite obtener el listado de fallas por fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<AnalisisFallaDTO> ObtenerAnalisisFallaCompleto(DateTime fecha)
        {
            return FactorySic.ObtenerEventoDao().ObtenerAnalisisFallaCompleto(fecha);
        }
        /// <summary>
        /// Permite obtener el listado de eventos de horas de operación por fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> GetByDetalleHO(DateTime fecha)
        {
            return FactorySic.GetEveHoraoperacionRepository().GetByDetalleHO(fecha);
        }

        #region Migraciones 2024

        /// <summary>
        /// Genera el reporte de formato cruzado de mantenimientos
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="pathLogo"></param>
        /// <param name="nameFile"></param>
        /// <param name="fecInicial"></param>
        /// <param name="fecFinal"></param>
        /// <param name="tiposMantenimiento"></param>
        /// <param name="indisponibilidad"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposEquipo"></param>
        /// <param name="conInterrupcion"></param>
        /// <param name="tiposMantto"></param>
        public void GenerarExportacionRFmtCruzado(string ruta, string pathLogo, string nameFile, DateTime fecInicial, DateTime fecFinal, string tiposMantenimiento, string indisponibilidad, string tiposEmpresa, string empresas, string tiposEquipo, string conInterrupcion, string tiposMantto)
        {
            IntervencionesAppServicio servIntervenciones = new IntervencionesAppServicio();

            //Obtenemos los mantenimientos
            List<EveManttoDTO> listaManttos = BuscarMantenimientos(tiposMantenimiento, fecInicial, fecFinal.AddDays(1), indisponibilidad, tiposEmpresa, empresas, tiposEquipo, conInterrupcion, tiposMantto, 1, 1000000);

            //obtenemos las intervenciones (desde mantenimientos)
            List<InIntervencionDTO> intervencionesCruzadas = new List<InIntervencionDTO>();
            foreach (EveManttoDTO objMantto in listaManttos)
            {
                InIntervencionDTO regInt = servIntervenciones.ConvertirAInIntervencion(objMantto);
                intervencionesCruzadas.Add(regInt);
            }

            //Obtenemos data por cada tipo de mantenimiento
            List<HojaReporteFormatoCruzado> lstDatosReporte = ObtenerNombresHojasReporte(tiposMantenimiento, intervencionesCruzadas);

            List<EqPropequiDTO> listaProp = servIntervenciones.ListarPropiedadPotenciaIndisponible();

            //lista de dias feriados
            List<DocDiaEspDTO> listaFeriados = FactorySic.GetDocDiaEspRepository().List();

            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                foreach (HojaReporteFormatoCruzado hojaReporte in lstDatosReporte)
                {
                    int horaIndisp = 0;
                    servIntervenciones.GenerarReporteTablaCruzada(fecInicial, fecFinal, horaIndisp, true, hojaReporte.ListadoIntervenciones, listaProp, listaFeriados,
                                            out List<IntervencionColumnaDia> listaFecha, out List<IntervencionCeldaEq> listaEq);

                    ExcelWorksheet ws = null;
                    servIntervenciones.GenerarHojaRptCruzada(xlPackage, ref ws, hojaReporte.NombreHoja, "Intervenciones Cruzadas", pathLogo, listaFecha, listaEq);

                }

                xlPackage.Save();
            }

        }

        /// <summary>
        /// Devuelve las hojas que seran parte del reporte de formato cruzado
        /// </summary>
        /// <param name="tiposMantenimiento"></param>
        /// <param name="intervencionesCruzadas"></param>
        /// <returns></returns>
        public List<HojaReporteFormatoCruzado> ObtenerNombresHojasReporte(string tiposMantenimiento, List<InIntervencionDTO> intervencionesCruzadas)
        {
            //Dictionary<int, string> salida  = new Dictionary<int, string>();
            List<HojaReporteFormatoCruzado> lstSalida = new List<HojaReporteFormatoCruzado>();

            List<int> listaTiposMantenimientos = (tiposMantenimiento.Split(ConstantesAppServicio.CaracterComa)).Select(n => int.Parse(n)).ToList();

            foreach (int evenclasecodi in listaTiposMantenimientos)
            {
                string nombHoja = "";
                List<InIntervencionDTO> lstTemp = intervencionesCruzadas.Where(x => x.Evenclasecodi == evenclasecodi).ToList();
                switch (evenclasecodi)
                {
                    case ConstantesEvento.EvenclasecodiEjecutados: nombHoja = "EJECUTADOS"; break;
                    case ConstantesEvento.EvenclasecodiProgramadoDiario: nombHoja = "PROG. DIARIO"; break;
                    case ConstantesEvento.EvenclasecodiProgramadoSemanal: nombHoja = "PROG. SEMANAL"; break;
                    case ConstantesEvento.EvenclasecodiProgramadoMensual: nombHoja = "PROG. MENSUAL"; break;
                    case ConstantesEvento.EvenclasecodiProgramadoAnual: nombHoja = "PROG. ANUAL"; break;
                    case ConstantesEvento.EvenclasecodiProgramadoAjusteDiario: nombHoja = "PROG. AJUSTE DIARIO"; break;
                    case ConstantesEvento.EvenclasecodiEjecutadoDiario: nombHoja = "EJECUTADO DIARIO"; break;
                    case ConstantesEvento.EvenclasecodiEjecutadoMensual: nombHoja = "EJECUTADO MENSUAL"; break;
                    case ConstantesEvento.EvenclasecodiProgramadoAnualMensual: nombHoja = "PROG. ANUAL MENSUAL"; break;
                    case ConstantesEvento.EvenclasecodiProgramadoAnualMensualSemanal: nombHoja = "PROG. ANUAL MEN SEM"; break;
                    case ConstantesEvento.EvenclasecodiProgramadoMensualSemanalDiario: nombHoja = "PROG. MEN SEM DIARIO"; break;
                    case ConstantesEvento.EvenclasecodiProgramadoSemanalDiario: nombHoja = "PROG. SEMANAL DIARIO"; break;
                }

                HojaReporteFormatoCruzado objR = new HojaReporteFormatoCruzado();
                objR.Evenclasecodi = evenclasecodi;
                objR.NombreHoja = nombHoja;
                objR.ListadoIntervenciones = lstTemp;

                lstSalida.Add(objR);
            }

            lstSalida = lstSalida.OrderBy(x => x.NombreHoja).ToList();

            return lstSalida;
        }

        #endregion

    }

    public class HojaReporteFormatoCruzado
    {
        public int Evenclasecodi { get; set; }
        public string NombreHoja { get; set; }
        public List<InIntervencionDTO> ListadoIntervenciones { get; set; }
    }

}
