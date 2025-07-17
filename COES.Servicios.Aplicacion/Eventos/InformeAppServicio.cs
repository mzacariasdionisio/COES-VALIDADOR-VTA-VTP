using System;
using System.Collections.Generic;
using System.Linq;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.Servicios.Aplicacion.Eventos.Helper;

namespace COES.Servicios.Aplicacion.Informe
{
    /// <summary>
    /// Clases con métodos del módulo Informe
    /// </summary>
    public class InformeAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(InformeAppServicio));

        #region Métodos Tabla EVE_INFORME

        /// <summary>
        /// Inserta un registro de la tabla EVE_INFORME
        /// </summary>
        public void SaveEveInforme(EveInformeDTO entity)
        {
            try
            {
                FactorySic.GetEveInformeRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_INFORME
        /// </summary>
        public void UpdateEveInforme(EveInformeDTO entity)
        {
            try
            {
                FactorySic.GetEveInformeRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_INFORME
        /// </summary>
        public void DeleteEveInforme(int eveninfcodi)
        {
            try
            {
                FactorySic.GetEveInformeRepository().Delete(eveninfcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_INFORME
        /// </summary>
        public EveInformeDTO GetByIdEveInforme(int eveninfcodi)
        {
            return FactorySic.GetEveInformeRepository().GetById(eveninfcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_INFORME
        /// </summary>
        public List<EveInformeDTO> ListEveInformes()
        {
            return FactorySic.GetEveInformeRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveInforme
        /// </summary>
        public List<EveInformeDTO> GetByCriteriaEveInformes()
        {
            return FactorySic.GetEveInformeRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener los datos de un informe
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<EveInformeDTO> ObtenerInformesEmpresa(int idEvento, int idEmpresa)
        {
            return FactorySic.GetEveInformeRepository().ObtenerInformesEmpresa(idEvento, idEmpresa);
        }

        /// <summary>
        /// Permite verificar la existencia de informes del evento para la empresa seleccionada
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="idEmpresa"></param>
        public void VerificarExistenciaInforme(int idEvento, int idEmpresa, EveEventoDTO evento, string usuario, bool flag)
        {
            try
            {
                List<EveInformeDTO> list = this.ObtenerInformesEmpresa(idEvento, idEmpresa);
                EveInformeDTO entity = new EveInformeDTO();
                entity.Emprcodi = idEmpresa;
                entity.Evencodi = idEvento;
                entity.Lastuser = usuario;

                int idPreliminar = 0;
                int idFinal = 0;
                int idComplementario = 0;
                int idPreliminarInicial = 0;

                if (idEmpresa == -1)
                {
                    if (list.Where(x => x.Infversion == ConstantesEvento.InformePreliminarInicial).Count() == 0)
                    {
                        entity.Infversion = ConstantesEvento.InformePreliminarInicial;
                        int id = FactorySic.GetEveInformeRepository().Save(entity);

                        if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 4))
                        {
                            EveInformeItemDTO item = new EveInformeItemDTO();
                            item.Descomentario = evento.Evendesc;
                            item.Itemnumber = 4;
                            item.Eveninfcodi = id;
                            FactorySic.GetEveInformeItemRepository().Save(item);
                        }

                        if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 9))
                        {
                            EveInformeItemDTO itemAnalisis = new EveInformeItemDTO();
                            itemAnalisis.Descomentario = string.Empty;
                            itemAnalisis.Eveninfcodi = id;
                            itemAnalisis.Itemnumber = 9;
                            FactorySic.GetEveInformeItemRepository().Save(itemAnalisis);
                        }

                        idPreliminarInicial = id;
                    }
                    else
                    {
                        int id = list.Where(x => x.Infversion == ConstantesEvento.InformePreliminarInicial).First().Eveninfcodi;
                        if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 4))
                        {
                            EveInformeItemDTO item = new EveInformeItemDTO();
                            item.Descomentario = evento.Evendesc;
                            item.Itemnumber = 4;
                            item.Eveninfcodi = id;
                            FactorySic.GetEveInformeItemRepository().Save(item);
                        }

                        if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 9))
                        {
                            EveInformeItemDTO itemAnalisis = new EveInformeItemDTO();
                            itemAnalisis.Descomentario = string.Empty;
                            itemAnalisis.Eveninfcodi = id;
                            itemAnalisis.Itemnumber = 9;
                            FactorySic.GetEveInformeItemRepository().Save(itemAnalisis);
                        }

                        idPreliminarInicial = id;
                    }

                }



                if (list.Where(x => x.Infversion == ConstantesEvento.InformePreliminar).Count() == 0)
                {
                    entity.Infversion = ConstantesEvento.InformePreliminar;
                    int id = FactorySic.GetEveInformeRepository().Save(entity);

                    if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 4))
                    {
                        EveInformeItemDTO item = new EveInformeItemDTO();
                        item.Descomentario = evento.Evendesc;
                        item.Itemnumber = 4;
                        item.Eveninfcodi = id;
                        FactorySic.GetEveInformeItemRepository().Save(item);
                    }

                    if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 9))
                    {
                        EveInformeItemDTO itemAnalisis = new EveInformeItemDTO();
                        itemAnalisis.Descomentario = string.Empty;
                        itemAnalisis.Eveninfcodi = id;
                        itemAnalisis.Itemnumber = 9;
                        FactorySic.GetEveInformeItemRepository().Save(itemAnalisis);
                    }

                    idPreliminar = id;
                }
                else
                {
                    int id = list.Where(x => x.Infversion == ConstantesEvento.InformePreliminar).First().Eveninfcodi;
                    if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 4))
                    {
                        EveInformeItemDTO item = new EveInformeItemDTO();
                        item.Descomentario = evento.Evendesc;
                        item.Itemnumber = 4;
                        item.Eveninfcodi = id;
                        FactorySic.GetEveInformeItemRepository().Save(item);
                    }

                    if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 9))
                    {
                        EveInformeItemDTO itemAnalisis = new EveInformeItemDTO();
                        itemAnalisis.Descomentario = string.Empty;
                        itemAnalisis.Eveninfcodi = id;
                        itemAnalisis.Itemnumber = 9;
                        FactorySic.GetEveInformeItemRepository().Save(itemAnalisis);
                    }

                    idPreliminar = id;
                }

                if (list.Where(x => x.Infversion == ConstantesEvento.InformeFinal).Count() == 0)
                {
                    entity.Infversion = ConstantesEvento.InformeFinal;
                    int id = FactorySic.GetEveInformeRepository().Save(entity);

                    if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 4))
                    {
                        EveInformeItemDTO item = new EveInformeItemDTO();
                        item.Descomentario = evento.Evendesc;
                        item.Itemnumber = 4;
                        item.Eveninfcodi = id;
                        FactorySic.GetEveInformeItemRepository().Save(item);
                    }

                    if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 9))
                    {
                        EveInformeItemDTO itemAnalisis = new EveInformeItemDTO();
                        itemAnalisis.Descomentario = string.Empty;
                        itemAnalisis.Eveninfcodi = id;
                        itemAnalisis.Itemnumber = 9;
                        FactorySic.GetEveInformeItemRepository().Save(itemAnalisis);
                    }

                    idFinal = id;
                }
                else
                {
                    int id = list.Where(x => x.Infversion == ConstantesEvento.InformeFinal).First().Eveninfcodi;
                    if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 4))
                    {
                        EveInformeItemDTO item = new EveInformeItemDTO();
                        item.Descomentario = evento.Evendesc;
                        item.Itemnumber = 4;
                        item.Eveninfcodi = id;
                        FactorySic.GetEveInformeItemRepository().Save(item);
                    }

                    if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 9))
                    {
                        EveInformeItemDTO itemAnalisis = new EveInformeItemDTO();
                        itemAnalisis.Descomentario = string.Empty;
                        itemAnalisis.Eveninfcodi = id;
                        itemAnalisis.Itemnumber = 9;
                        FactorySic.GetEveInformeItemRepository().Save(itemAnalisis);
                    }

                    idFinal = id;
                }

                if (list.Where(x => x.Infversion == ConstantesEvento.InformeComplementario).Count() == 0)
                {
                    entity.Infversion = ConstantesEvento.InformeComplementario;
                    int id = FactorySic.GetEveInformeRepository().Save(entity);

                    if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 4))
                    {
                        EveInformeItemDTO item = new EveInformeItemDTO();
                        item.Descomentario = evento.Evendesc;
                        item.Itemnumber = 4;
                        item.Eveninfcodi = id;
                        FactorySic.GetEveInformeItemRepository().Save(item);
                    }

                    if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 9))
                    {
                        EveInformeItemDTO itemAnalisis = new EveInformeItemDTO();
                        itemAnalisis.Descomentario = string.Empty;
                        itemAnalisis.Eveninfcodi = id;
                        itemAnalisis.Itemnumber = 9;
                        FactorySic.GetEveInformeItemRepository().Save(itemAnalisis);
                    }

                    idComplementario = id;
                }
                else
                {
                    int id = list.Where(x => x.Infversion == ConstantesEvento.InformeComplementario).First().Eveninfcodi;
                    if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 4))
                    {
                        EveInformeItemDTO item = new EveInformeItemDTO();
                        item.Descomentario = evento.Evendesc;
                        item.Itemnumber = 4;
                        item.Eveninfcodi = id;
                        FactorySic.GetEveInformeItemRepository().Save(item);
                    }

                    if (!FactorySic.GetEveInformeItemRepository().VerificarExistencia(id, 9))
                    {
                        EveInformeItemDTO itemAnalisis = new EveInformeItemDTO();
                        itemAnalisis.Descomentario = string.Empty;
                        itemAnalisis.Eveninfcodi = id;
                        itemAnalisis.Itemnumber = 9;
                        FactorySic.GetEveInformeItemRepository().Save(itemAnalisis);
                    }

                    idComplementario = id;
                }

                if (list.Where(x => x.Infversion == ConstantesEvento.InformeArchivos).Count() == 0)
                {
                    entity.Infversion = ConstantesEvento.InformeArchivos;
                    int id = FactorySic.GetEveInformeRepository().Save(entity);
                }

                if (idEmpresa == 0 && flag)
                {
                    this.GenerarReporteConsolidado(idEvento, idPreliminar, idFinal, idComplementario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite generar el informe consolidado de las empresas
        /// </summary>
        /// <param name="idEvento"></param>
        protected void GenerarReporteConsolidado(int idEvento, int idPreliminar, int idFinal, int idComplementario)
        {
            try
            {
                FactorySic.GetEveInformeItemRepository().DeleteConsolidado(idEvento);

                FactorySic.GetEveInformeItemRepository().SaveConsolidado(idEvento, idPreliminar,
                    ConstantesEvento.InformePreliminar);

                FactorySic.GetEveInformeItemRepository().SaveConsolidado(idEvento, idFinal,
                    ConstantesEvento.InformeFinal);

                FactorySic.GetEveInformeItemRepository().SaveConsolidado(idEvento, idComplementario,
                    ConstantesEvento.InformeComplementario);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite obtener el reporte de carga de informes
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public List<EventoInformeReporte> ObtenerInformeResumenIntranet(int idEvento)
        {
            List<EventoInformeReporte> reporte = new List<EventoInformeReporte>();
            List<EveInformeDTO> informes = new List<EveInformeDTO>();

            informes = FactorySic.GetEveInformeRepository().ObtenerReporteEmpresaGeneral(idEvento);
            var listEmpresa = informes.Where(x => x.Emprcodi != -1).Select(x => new { x.Emprcodi, x.Emprnomb }).Distinct().ToList();

            foreach (var item in listEmpresa)
            {
                EventoInformeReporte itemReporte = new EventoInformeReporte();
                itemReporte.Emprcodi = item.Emprcodi;
                itemReporte.Emprnomb = item.Emprnomb;
                itemReporte.Evencodi = idEvento;
                List<EveInformeDTO> listInforme = informes.Where(x => x.Emprcodi == item.Emprcodi).ToList();

                foreach (EveInformeDTO itemInforme in listInforme)
                {
                    if (itemInforme.Infversion == ConstantesEvento.InformePreliminar) itemReporte.EstadoPreliminar = ConstantesEvento.TextoCargo;
                    if (itemInforme.Infversion == ConstantesEvento.InformeFinal) itemReporte.EstadoFinal = ConstantesEvento.TextoCargo;
                    if (itemInforme.Infversion == ConstantesEvento.InformeComplementario) itemReporte.EstadoComplementario = ConstantesEvento.TextoCargo;
                }

                reporte.Add(itemReporte);
            }

            //sacamos el campo para la sco
            var listEmpresaSCO = informes.Where(x => x.Emprcodi == -1).Select(x => new { x.Emprcodi, x.Emprnomb }).Distinct().ToList();

            if (listEmpresaSCO.Count > 0)
            {
                foreach (var item in listEmpresaSCO)
                {
                    EventoInformeReporte itemReporte = new EventoInformeReporte();
                    itemReporte.Emprcodi = item.Emprcodi;
                    itemReporte.Emprnomb = item.Emprnomb;
                    itemReporte.Evencodi = idEvento;
                    List<EveInformeDTO> listInforme = informes.Where(x => x.Emprcodi == item.Emprcodi).ToList();

                    itemReporte.EstadoPreliminar = ConstantesEvento.TextoInformePendiente;
                    itemReporte.EstadoFinal = ConstantesEvento.TextoInformePendiente;
                    itemReporte.EstadoComplementario = ConstantesEvento.TextoInformePendiente;
                    itemReporte.EstadoPreliminarInicial = ConstantesEvento.TextoInformePendiente;

                    foreach (EveInformeDTO itemInforme in listInforme)
                    {
                        if (itemInforme.Infversion == ConstantesEvento.InformePreliminar)
                        {
                            if (itemInforme.Infestado == ConstantesEvento.EstadoFinalizado)
                            {
                                itemReporte.EstadoPreliminar = ConstantesEvento.TextoInformeFinalizado;
                            }
                            else if (itemInforme.Infestado == ConstantesEvento.EstadoRevisado)
                            {
                                itemReporte.EstadoPreliminar = ConstantesEvento.TextoInformeRevisado;
                            }
                            else
                            {
                                itemReporte.EstadoPreliminar = ConstantesEvento.TextoCargo;
                            }
                        }
                        if (itemInforme.Infversion == ConstantesEvento.InformeFinal)
                        {
                            if (itemInforme.Infestado == ConstantesEvento.EstadoFinalizado)
                            {
                                itemReporte.EstadoFinal = ConstantesEvento.TextoInformeFinalizado;
                            }
                            else if (itemInforme.Infestado == ConstantesEvento.EstadoRevisado)
                            {
                                itemReporte.EstadoFinal = ConstantesEvento.TextoInformeRevisado;
                            }
                            else
                            {
                                itemReporte.EstadoFinal = ConstantesEvento.TextoCargo;
                            }
                        }

                        if (itemInforme.Infversion == ConstantesEvento.InformeComplementario)
                        {
                            if (itemInforme.Infestado == ConstantesEvento.EstadoFinalizado)
                            {
                                itemReporte.EstadoComplementario = ConstantesEvento.TextoInformeFinalizado;
                            }
                            else if (itemInforme.Infestado == ConstantesEvento.EstadoRevisado)
                            {
                                itemReporte.EstadoComplementario = ConstantesEvento.TextoInformeRevisado;
                            }
                            else
                            {
                                itemReporte.EstadoComplementario = ConstantesEvento.TextoCargo;
                            }
                        }

                        if (itemInforme.Infversion == ConstantesEvento.InformePreliminarInicial)
                        {
                            if (itemInforme.Infestado == ConstantesEvento.EstadoFinalizado)
                            {
                                itemReporte.EstadoPreliminarInicial = ConstantesEvento.TextoInformeFinalizado;
                            }
                            else if (itemInforme.Infestado == ConstantesEvento.EstadoRevisado)
                            {
                                itemReporte.EstadoPreliminarInicial = ConstantesEvento.TextoInformeRevisado;
                            }
                            else
                            {
                                itemReporte.EstadoPreliminarInicial = ConstantesEvento.TextoCargo;
                            }
                        }
                    }

                    reporte.Add(itemReporte);
                }
            }
            else
            {
                EventoInformeReporte itemReporte = new EventoInformeReporte();
                itemReporte.Emprcodi = -1;
                itemReporte.Emprnomb = ConstantesEvento.AreaSCO;
                itemReporte.Evencodi = idEvento;
                itemReporte.EstadoPreliminar = ConstantesEvento.TextoInformePendiente;
                itemReporte.EstadoFinal = ConstantesEvento.TextoInformePendiente;
                itemReporte.EstadoComplementario = ConstantesEvento.TextoInformePendiente;
                itemReporte.EstadoPreliminarInicial = ConstantesEvento.TextoInformePendiente;

                reporte.Add(itemReporte);
            }

            return reporte;
        }


        /// <summary>
        /// Permite obtener un resumen de los reportes cargados
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="empresas"></param>
        /// <param name="indicador"></param>
        /// <returns></returns>
        public List<EventoInformeReporte> ObtenerInformeResumen(int idEvento, List<int> idEmpresas, string indicador)
        {
            List<EventoInformeReporte> reporte = new List<EventoInformeReporte>();

            string empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idEmpresas);
            List<EveInformeDTO> informes = new List<EveInformeDTO>();

            if (indicador == ConstantesAppServicio.SI)
            {
                informes = FactorySic.GetEveInformeRepository().ObtenerReporteEmpresa(idEvento, empresas);
                var listEmpresa = informes.Where(x => x.Emprcodi != -1).Select(x => new { x.Emprcodi, x.Emprnomb }).Distinct().ToList();

                foreach (var item in listEmpresa)
                {
                    EventoInformeReporte itemReporte = new EventoInformeReporte();
                    itemReporte.Emprcodi = item.Emprcodi;
                    itemReporte.Emprnomb = item.Emprnomb;
                    itemReporte.Evencodi = idEvento;
                    List<EveInformeDTO> listInforme = informes.Where(x => x.Emprcodi == item.Emprcodi).ToList();

                    foreach (EveInformeDTO itemInforme in listInforme)
                    {
                        if (itemInforme.Infversion == ConstantesEvento.InformePreliminar) itemReporte.EstadoPreliminar = ConstantesEvento.TextoCargo;
                        if (itemInforme.Infversion == ConstantesEvento.InformeFinal) itemReporte.EstadoFinal = ConstantesEvento.TextoCargo;
                        if (itemInforme.Infversion == ConstantesEvento.InformeComplementario) itemReporte.EstadoComplementario = ConstantesEvento.TextoCargo;
                    }

                    reporte.Add(itemReporte);
                }

                //sacamos el campo para la sco
                var listEmpresaSCO = informes.Where(x => x.Emprcodi == -1).Select(x => new { x.Emprcodi, x.Emprnomb }).Distinct().ToList();

                if (listEmpresaSCO.Count > 0)
                {
                    foreach (var item in listEmpresaSCO)
                    {
                        EventoInformeReporte itemReporte = new EventoInformeReporte();
                        itemReporte.Emprcodi = item.Emprcodi;
                        itemReporte.Emprnomb = item.Emprnomb;
                        itemReporte.Evencodi = idEvento;
                        List<EveInformeDTO> listInforme = informes.Where(x => x.Emprcodi == item.Emprcodi).ToList();

                        itemReporte.EstadoPreliminar = ConstantesEvento.TextoInformePendiente;
                        itemReporte.EstadoFinal = ConstantesEvento.TextoInformePendiente;
                        itemReporte.EstadoComplementario = ConstantesEvento.TextoInformePendiente;
                        itemReporte.EstadoPreliminarInicial = ConstantesEvento.TextoInformePendiente;

                        foreach (EveInformeDTO itemInforme in listInforme)
                        {
                            if (itemInforme.Infversion == ConstantesEvento.InformePreliminar)
                            {
                                if (itemInforme.Infestado == ConstantesEvento.EstadoFinalizado)
                                {
                                    itemReporte.EstadoPreliminar = ConstantesEvento.TextoInformeFinalizado;
                                }
                                else if (itemInforme.Infestado == ConstantesEvento.EstadoRevisado)
                                {
                                    itemReporte.EstadoPreliminar = ConstantesEvento.TextoInformeRevisado;
                                }
                                else
                                {
                                    itemReporte.EstadoPreliminar = ConstantesEvento.TextoCargo;
                                }
                            }
                            if (itemInforme.Infversion == ConstantesEvento.InformeFinal)
                            {
                                if (itemInforme.Infestado == ConstantesEvento.EstadoFinalizado)
                                {
                                    itemReporte.EstadoFinal = ConstantesEvento.TextoInformeFinalizado;
                                }
                                else if (itemInforme.Infestado == ConstantesEvento.EstadoRevisado)
                                {
                                    itemReporte.EstadoFinal = ConstantesEvento.TextoInformeRevisado;
                                }
                                else
                                {
                                    itemReporte.EstadoFinal = ConstantesEvento.TextoCargo;
                                }
                            }

                            if (itemInforme.Infversion == ConstantesEvento.InformeComplementario)
                            {
                                if (itemInforme.Infestado == ConstantesEvento.EstadoFinalizado)
                                {
                                    itemReporte.EstadoComplementario = ConstantesEvento.TextoInformeFinalizado;
                                }
                                else if (itemInforme.Infestado == ConstantesEvento.EstadoRevisado)
                                {
                                    itemReporte.EstadoComplementario = ConstantesEvento.TextoInformeRevisado;
                                }
                                else
                                {
                                    itemReporte.EstadoComplementario = ConstantesEvento.TextoCargo;
                                }
                            }

                            if (itemInforme.Infversion == ConstantesEvento.InformePreliminarInicial)
                            {
                                if (itemInforme.Infestado == ConstantesEvento.EstadoFinalizado)
                                {
                                    itemReporte.EstadoPreliminarInicial = ConstantesEvento.TextoInformeFinalizado;
                                }
                                else if (itemInforme.Infestado == ConstantesEvento.EstadoRevisado)
                                {
                                    itemReporte.EstadoPreliminarInicial = ConstantesEvento.TextoInformeRevisado;
                                }
                                else
                                {
                                    itemReporte.EstadoPreliminarInicial = ConstantesEvento.TextoCargo;
                                }
                            }
                        }

                        reporte.Add(itemReporte);
                    }
                }
                else
                {
                    EventoInformeReporte itemReporte = new EventoInformeReporte();
                    itemReporte.Emprcodi = -1;
                    itemReporte.Emprnomb = ConstantesEvento.AreaSCO;
                    itemReporte.Evencodi = idEvento;
                    itemReporte.EstadoPreliminar = ConstantesEvento.TextoInformePendiente;
                    itemReporte.EstadoFinal = ConstantesEvento.TextoInformePendiente;
                    itemReporte.EstadoComplementario = ConstantesEvento.TextoInformePendiente;
                    itemReporte.EstadoPreliminarInicial = ConstantesEvento.TextoInformePendiente;

                    reporte.Add(itemReporte);
                }

            }
            else
            {
                informes = FactorySic.GetEveInformeRepository().ObtenerEstadoReporte(idEvento, empresas);
                List<EveInformeDTO> listEmpresas = FactorySic.GetEveInformeRepository().ObtenerEmpresaInforme(empresas);

                foreach (EveInformeDTO empresa in listEmpresas)
                {
                    EventoInformeReporte itemReporte = new EventoInformeReporte();
                    itemReporte.Emprcodi = empresa.Emprcodi;
                    itemReporte.Emprnomb = empresa.Emprnomb;
                    itemReporte.Evencodi = idEvento;
                    List<EveInformeDTO> listInforme = informes.Where(x => x.Emprcodi == empresa.Emprcodi).ToList();

                    if (listInforme.Count > 0)
                    {
                        foreach (EveInformeDTO itemInforme in listInforme)
                        {
                            if (itemInforme.Infversion == ConstantesEvento.InformePreliminar)
                            {
                                itemReporte.EstadoPreliminar = ConstantesEvento.TextoInformeElaboracion;
                                if (itemInforme.Infestado == ConstantesEvento.EstadoFinalizado)
                                {
                                    itemReporte.EstadoPreliminar = ConstantesEvento.TextoInformeFinalizado;
                                }
                            }
                            if (itemInforme.Infversion == ConstantesEvento.InformeFinal)
                            {
                                itemReporte.EstadoFinal = ConstantesEvento.TextoInformeElaboracion;
                                if (itemInforme.Infestado == ConstantesEvento.EstadoFinalizado)
                                {
                                    itemReporte.EstadoFinal = ConstantesEvento.TextoInformeFinalizado;
                                }
                            }
                            if (itemInforme.Infversion == ConstantesEvento.InformeComplementario)
                            {
                                itemReporte.EstadoComplementario = ConstantesEvento.TextoInformeElaboracion;
                                if (itemInforme.Infestado == ConstantesEvento.EstadoFinalizado)
                                {
                                    itemReporte.EstadoComplementario = ConstantesEvento.TextoInformeFinalizado;
                                }
                            }
                        }
                    }
                    else
                    {
                        itemReporte.EstadoPreliminar = ConstantesEvento.TextoInformePendiente;
                        itemReporte.EstadoFinal = ConstantesEvento.TextoInformePendiente;
                        itemReporte.EstadoComplementario = ConstantesEvento.TextoInformePendiente;
                    }

                    reporte.Add(itemReporte);
                }
            }


            return reporte;
        }

        #endregion

        #region Métodos Tabla EVE_INFORME_FILE

        /// <summary>
        /// Inserta un registro de la tabla EVE_INFORME_FILE
        /// </summary>
        public int SaveEveInformeFile(EveInformeFileDTO entity)
        {
            try
            {
                int id = FactorySic.GetEveInformeFileRepository().Save(entity);
                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_INFORME_FILE
        /// </summary>
        public void UpdateEveInformeFile(int idFile, string descripcion)
        {
            try
            {
                FactorySic.GetEveInformeFileRepository().Update(idFile, descripcion);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_INFORME_FILE
        /// </summary>
        public void DeleteEveInformeFile(int eveninffilecodi)
        {
            try
            {
                FactorySic.GetEveInformeFileRepository().Delete(eveninffilecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_INFORME_FILE
        /// </summary>
        public EveInformeFileDTO GetByIdEveInformeFile(int eveninffilecodi)
        {
            return FactorySic.GetEveInformeFileRepository().GetById(eveninffilecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_INFORME_FILE
        /// </summary>
        public List<EveInformeFileDTO> ListEveInformeFiles()
        {
            return FactorySic.GetEveInformeFileRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveInformeFile
        /// </summary>
        public List<EveInformeFileDTO> GetByCriteriaEveInformeFiles()
        {
            return FactorySic.GetEveInformeFileRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener 
        /// </summary>
        /// <param name="idInforme"></param>
        /// <returns></returns>
        public List<EveInformeFileDTO> ObtenerFilesInformeEvento(int idInforme)
        {
            List<EveInformeFileDTO> list = FactorySic.GetEveInformeFileRepository().ObtenerFilesInformeEvento(idInforme);

            foreach (EveInformeFileDTO item in list)
            {
                item.FileName = String.Format(ConstantesEvento.InformeFileName, item.Eveninffilecodi, item.Extfile);
            }

            return list;
        }


        #endregion

        #region Métodos Tabla EVE_INFORME_ITEM

        /// <summary>
        /// Inserta un registro de la tabla EVE_INFORME_ITEM
        /// </summary>
        public EveInformeItemDTO SaveEveInformeItem(EveInformeItemDTO entity)
        {
            try
            {
                int id = 0;
                if (entity.Infitemcodi == 0)
                {
                    id = FactorySic.GetEveInformeItemRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetEveInformeItemRepository().Update(entity);
                    id = entity.Infitemcodi;
                }

                EveInformeItemDTO resultado = FactorySic.GetEveInformeItemRepository().
                    ObtenerItemInformePorId(id);

                return resultado;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar las interrupciones desde el formato excel
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarInterrupciones(List<EveInformeItemDTO> entitys)
        {
            try
            {
                foreach (EveInformeItemDTO entity in entitys)
                {
                    FactorySic.GetEveInformeItemRepository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_INFORME_ITEM
        /// </summary>
        public void UpdateEveInformeItem(EveInformeItemDTO entity)
        {
            try
            {
                FactorySic.GetEveInformeItemRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_INFORME_ITEM
        /// </summary>
        public void DeleteEveInformeItem(int infitemcodi)
        {
            try
            {
                FactorySic.GetEveInformeItemRepository().Delete(infitemcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos registros de un informe de la tabla EVE_INFORME_ITEM
        /// </summary>
        public void DeleteEveInformeItemTotal(int eveninfcodi)
        {
            try
            {
                FactorySic.GetEveInformeItemRepository().DeletePorInforme(eveninfcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite obtener un registro de la tabla EVE_INFORME_ITEM
        /// </summary>
        public EveInformeItemDTO GetByIdEveInformeItem(int infitemcodi)
        {
            return FactorySic.GetEveInformeItemRepository().GetById(infitemcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_INFORME_ITEM
        /// </summary>
        public List<EveInformeItemDTO> ListEveInformeItems()
        {
            return FactorySic.GetEveInformeItemRepository().List();
        }

        /// <summary>
        /// Permite obtener los items de un informe
        /// </summary>
        /// <param name="idInforme"></param>
        /// <returns></returns>
        public List<EveInformeItemDTO> ObtenerItemInformeEvento(int idInforme)
        {
            return FactorySic.GetEveInformeItemRepository().ObtenerItemInformeEvento(idInforme);
        }

        /// <summary>
        /// Permite obtener todos los elementos de un informe de una empresa
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<EveInformeItemDTO> ObtenerItemInformeEvento(int idEvento, int idEmpresa)
        {
            return FactorySic.GetEveInformeItemRepository().ObtenerItemInformeEvento(idEvento, idEmpresa);
        }

        /// <summary>
        /// Permite actualizar el comentario de un item
        /// </summary>
        /// <param name="idItemInforme"></param>
        /// <param name="comentario"></param>
        public void ActualizarTextoInforme(int idItemInforme, string comentario)
        {
            FactorySic.GetEveInformeItemRepository().ActualizarTextoInforme(idItemInforme, comentario);
        }


        /// <summary>
        /// Permite copiar o reemplazar los datos del informe
        /// </summary>
        /// <param name="idOrigen"></param>
        /// <param name="idDestino"></param>
        /// <param name="indicador"></param>
        public void CopiarInforme(int idOrigen, int idDestino, string indicador)
        {
            try
            {
                List<EveInformeItemDTO> list = FactorySic.GetEveInformeItemRepository().GetByCriteria(idOrigen);

                if (indicador == ConstantesAppServicio.SI)
                {
                    FactorySic.GetEveInformeItemRepository().DeletePorInforme(idDestino);

                    foreach (EveInformeItemDTO item in list)
                    {
                        item.Eveninfcodi = idDestino;
                        FactorySic.GetEveInformeItemRepository().Save(item);
                    }
                }
                else
                {
                    List<EveInformeItemDTO> subList = list.Where(x => x.Itemnumber != 4 && x.Itemnumber != 9).ToList();
                    foreach (EveInformeItemDTO item in subList)
                    {
                        item.Eveninfcodi = idDestino;
                        FactorySic.GetEveInformeItemRepository().Save(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite copiar los datos en el informe final
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idDestino"></param>
        /// <param name="tipo"></param>
        /// <param name="indicador"></param>
        public void CopiarInformeEmpresa(int idEvento, int idEmpresa, int idDestino, string tipo, string indicador)
        {
            try
            {
                EveInformeDTO entity = FactorySic.GetEveInformeRepository().ObtenerInformePorTipo(idEvento, idEmpresa, tipo);

                if (entity != null)
                {
                    this.CopiarInforme(entity.Eveninfcodi, idDestino, indicador);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite dar por finalizado la redacción del informe
        /// </summary>
        /// <param name="idInforme"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int FinalizarInforme(int idEvento, string tipo, int idInforme, string username, int idEmpresa)
        {
            try
            {
                int indicador = 1;
                EveEventoDTO evento = FactorySic.GetEveEventoRepository().GetDetalleEvento(idEvento);
                DateTime fecha = (DateTime)evento.Evenini;
                string indPlazo = ConstantesAppServicio.SI;
                string estado = ConstantesEvento.EstadoFinalizado;

                if (idEmpresa == -1)
                {
                    if (tipo == ConstantesEvento.InformePreliminarInicial)
                    {
                        TimeSpan ts = DateTime.Now.Subtract(fecha.AddHours((double)4));
                        if (ts.TotalSeconds > 0) indPlazo = ConstantesAppServicio.NO;
                    }
                }

                if (tipo == ConstantesEvento.InformePreliminar)
                {
                    decimal plazo = 2.5M;
                    if (idEmpresa == -1) plazo = 6;

                    TimeSpan ts = DateTime.Now.Subtract(fecha.AddHours((double)plazo));
                    if (ts.TotalSeconds > 0) indPlazo = ConstantesAppServicio.NO;
                }

                if (tipo == ConstantesEvento.InformeFinal)
                {
                    decimal plazo = 60;
                    if (idEmpresa == -1) plazo = 72;

                    TimeSpan ts = DateTime.Now.Subtract(fecha.AddHours((double)plazo));
                    if (ts.TotalSeconds > 0) indPlazo = ConstantesAppServicio.NO;
                }

                List<EveInformeItemDTO> listItem = FactorySic.GetEveInformeItemRepository().GetByCriteria(idInforme);
                List<EveInformeFileDTO> listFile = FactorySic.GetEveInformeFileRepository().ObtenerFilesInformeEvento(idInforme);

                if (listItem.Count > 0 || listFile.Count > 0)
                {
                    if (listFile.Count == 0)
                    {
                        List<EveInformeItemDTO> subList = listItem.Where(x => x.Itemnumber == 4 || x.Itemnumber == 9).ToList();

                        if (subList.Count == listItem.Count)
                        {
                            EveInformeItemDTO item = subList.Where(x => x.Itemnumber == 9).FirstOrDefault();

                            if (item != null)
                            {
                                if (!string.IsNullOrEmpty(item.Descomentario))
                                {
                                    indicador = 1;
                                }
                                else
                                {
                                    indicador = 2;
                                }
                            }
                            else
                            {
                                indicador = 2;
                            }
                        }
                        else
                        {
                            indicador = 1;
                        }
                    }
                    else
                    {
                        indicador = 1;
                    }
                }
                else
                {
                    indicador = 2;
                }

                if (indicador == 1)
                {
                    FactorySic.GetEveInformeRepository().FinalizarInforme(idInforme, indPlazo, estado, username);
                }

                return indicador;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite dar por finalizado el informe correspondiente
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="tipo"></param>
        /// <param name="idInforme"></param>
        /// <param name="username"></param>
        public void RevisarInforme(int idEvento, string tipo, int idInforme, string username)
        {
            try
            {
                FactorySic.GetEveInformeRepository().RevisarInforme(idInforme, ConstantesEvento.EstadoRevisado, username);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener los elementos que serán utilizados en la interrupcion
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>        
        public List<EveInformeItemDTO> ObtenerInformeInterrupcion(int idEvento)
        {
            return FactorySic.GetEveInformeItemRepository().ObtenerInformeInterrupcion(idEvento);
        }

        #endregion


        /// <summary>
        /// Permite obtener los equipos para la seleccion en el informe
        /// </summary>
        /// <param name="idEmpresa">Identificador de la empresa</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ObtenerEquiposSeleccion(int idEmpresa)
        {
            return FactorySic.GetEveInformeRepository().ObtenerEquiposSeleccion(idEmpresa);
        }

        /// <summary>
        /// Obtiene el dato de la empresa
        /// </summary>
        /// <param name="idEmpresa">Identificador de la empresa</param>
        /// <returns></returns>
        public SiEmpresaDTO ObtenerEmpresa(int idEmpresa)
        {
            return FactorySic.GetSiEmpresaRepository().GetById(idEmpresa);
        }
    }
}
