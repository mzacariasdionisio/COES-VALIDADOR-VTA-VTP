using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Evento;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace COES.Servicios.Aplicacion.Eventos
{
    public class CriteriosEventoAppServicio : AppServicioBase
    {
        CorreoAppServicio servCorreo = new CorreoAppServicio();
        GeneralAppServicio servGeneral = new GeneralAppServicio();
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EventosAppServicio));
        /// <summary>
        /// Consultar CR_Eventos
        /// </summary>
        /// <param name="oCrevento"></param>
        /// <returns></returns>
        public List<CrEventoDTO> ConsultarCriterio(CrEventoDTO oCrevento)
        {
            return FactorySic.ObtenerCriterioEventoDao().ConsultarCriterio(oCrevento);
        }
        public List<CrEventoDTO> ConsultarCriterio2(CrEventoDTO oCrevento)
        {
            return FactorySic.ObtenerCriterioEventoDao().ConsultarCriterio2(oCrevento);
        }
        public List<CrEventoDTO> SqlTraerEtapaxEvento(CrEventoDTO oCrevento)
        {
            return FactorySic.GetCrEventoRepository().ListCrEventos(oCrevento);
        }
        public string ObtenerComentarioXEventoyEtapa(int CREVENCODI, int CRETAPA)
        {
            return FactorySic.ObtenerCriterioEventoDao().SqlObtenerComentarioXEventoyEtapa(CREVENCODI, CRETAPA);
        }
        public List<Responsables> SqlObtenerEmpresaResponsable(int CREVENCODI, int CRETAPACODI)
        {
            return FactorySic.ObtenerCriterioEventoDao().SqlObtenerEmpresaResponsable(CREVENCODI,CRETAPACODI);
        }
        
        public List<Solicitantes> SqlObtenerEmpresaSolicitante(int CREVENCODI, int CRETAPACODI)
        {
            return FactorySic.ObtenerCriterioEventoDao().SqlObtenerEmpresaSolicitante(CREVENCODI,CRETAPACODI);
        }
        
        /// <summary>
        /// Consultar  CR_CASOS_ESPECIALES
        /// </summary>
        /// <returns></returns>
        public List<CrCasosEspecialesDTO> ObtenerCasosEspeciales()
        {
            return FactorySic.ObtenerCriterioEventoDao().ObtenerCasosEspeciales();
        }
        /// <summary>
        /// insertar registro en la tabla CR_CASOS_ESPECIALES
        /// </summary>
        /// <param name="oCasosEspecialesDTO"></param>
        /// <returns></returns>
        public void SaveCasosEspeciales(CrCasosEspecialesDTO oCasosEspecialesDTO)
        {
            try
            {
                if (oCasosEspecialesDTO.CRESPECIALCODI == 0)
                {
                    FactorySic.ObtenerCriterioEventoDao().SaveCasosEspeciales(oCasosEspecialesDTO);
                }
                else {
                    FactorySic.ObtenerCriterioEventoDao().UpdateCasosEspeciales(oCasosEspecialesDTO);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            
        }
        /// <summary>
        /// Permite obtener un registro de la tabla CR_CASOS_ESPECIALES
        /// <param name="CRESPECIALCODI"></param>
        /// </summary>
        public CrCasosEspecialesDTO GetByIdCasosEspeciales(int CRESPECIALCODI)
        {
            return FactorySic.ObtenerCriterioEventoDao().GetByIdCasosEspeciales(CRESPECIALCODI);
        }
        /// <summary>
        /// Consultar casos especiales
        /// </summary>
        /// <returns></returns>
        public List<CrCasosEspecialesDTO> ListCasosEspeciales()
        {
            return FactorySic.ObtenerCriterioEventoDao().ListCasosEspeciales();
        }
        /// <summary>
        /// eliminar casos especiales
        /// </summary>
        /// <param name="CRESPECIALCODI"></param>
        /// <returns></returns>
        public void  DeleteCasosEspeciales (int CRESPECIALCODI)
        {
             FactorySic.ObtenerCriterioEventoDao().DeleteCasosEspeciales(CRESPECIALCODI);
        }
        /// <summary>
        /// validar casos especiales
        /// </summary>
        /// <param name="CRESPECIALCODI"></param>
        /// <returns></returns>
        public int ValidarCasosEspeciales(int CRESPECIALCODI)
        {
            return FactorySic.ObtenerCriterioEventoDao().ValidarCasosEspeciales(CRESPECIALCODI);
        }
        /// <summary>
        /// Consultar criterio
        /// </summary>
        /// <returns></returns>
        public List<CrCriteriosDTO> ObtenerCriterios()
        {
            return FactorySic.ObtenerCriterioEventoDao().ObtenerCriterios();
        }
        /// <summary>
        /// agregar registro en la tabla CR_CRITERIOS
        /// </summary>
        /// <param name="oCriteriosDTO"></param>
        /// <returns></returns>
        public void SaveCriterios(CrCriteriosDTO oCriteriosDTO)
        {
            try
            {
               
                if (oCriteriosDTO.CRCRITERIOCODI == 0)
                {
                    FactorySic.ObtenerCriterioEventoDao().SaveCriterios(oCriteriosDTO);
                }
                else
                {
                    FactorySic.ObtenerCriterioEventoDao().UpdateCriterios(oCriteriosDTO);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }
        /// <summary>
        /// validar criterios
        /// </summary>
        /// <param name="crcriteriocodi"></param>
        /// <returns></returns>
        public int ValidarCriterios(int crcriteriocodi)
        {
            return FactorySic.ObtenerCriterioEventoDao().ValidarCriterios(crcriteriocodi);
        }
        /// <summary>
        /// Consultar analisis de falla
        /// </summary>
        /// <returns></returns>
        public List<CrCriteriosDTO> ListCriterios()
        {
            return FactorySic.ObtenerCriterioEventoDao().ListCriterios();
        }
        /// <summary>
        /// Permite obtener un registro de la tabla CR_CRITERIOS
        /// <param name="CRCRITERIOCODI"></param>
        /// </summary>
        public CrCriteriosDTO GetByIdCriterios(int CRCRITERIOCODI)
        {
            return FactorySic.ObtenerCriterioEventoDao().GetByIdCriterios(CRCRITERIOCODI);
        }
        /// <summary>
        /// Eliminar criterio
        /// </summary>
        /// <param name="CRCRITERIOCODI"></param>
        /// <returns></returns>
        public void DeleteCriterios(int CRCRITERIOCODI)
        {
            FactorySic.ObtenerCriterioEventoDao().DeleteCriterios(CRCRITERIOCODI);
        }
        /// <summary>
        /// Permite obtener un registro de la tabla CR_EVENTO
        /// <param name="crevencodi"></param>
        /// </summary>
        public CrEventoDTO GetByIdCrEvento(int crevencodi)
        {
            return FactorySic.GetCrEventoRepository().GetById(crevencodi);
        }
        /// <summary>
        /// Permite obtener un registro de la tabla CR_EVENTO
        /// <param name="crevencodi"></param>
        /// <param name="cretapa"></param>
        /// </summary>
        public CrEtapaEventoDTO ObtenerCrEtapaEvento(int crevencodi, int cretapa)
        {
            return FactorySic.GetCrEtapaEventoRepository().ObtenerCrEtapaEvento(crevencodi, cretapa);
        }
        /// <summary>
        /// Permite registrar en tabla cr_etapa_evento
        /// <param name="entity"></param>
        /// </summary>
        public int SaveCrEtapaEvento(CrEtapaEventoDTO entity)
        {
            int id = 0;
            id = FactorySic.GetCrEtapaEventoRepository().save(entity);
            return id;
        }
        /// <summary>
        /// Permite registrar en tabla cr_etapa_evento
        /// <param name="entity"></param>
        /// </summary>
        public void UpdateCrEtapaEvento(CrEtapaEventoDTO entity)
        {
            FactorySic.GetCrEtapaEventoRepository().Update(entity);

        }
        /// <summary>
        /// Permite eliminar en tabla cr_etapa_evento
        /// <param name="cretapacodi"></param>
        /// </summary>
        public void DeleteCrEtapaEvento(int cretapacodi)
        {
            FactorySic.GetCrEtapaEventoRepository().Delete(cretapacodi);

        }
        /// <summary>
        /// Obtener criterio por etapa evento
        /// </summary>
        /// <returns></returns>
        public List<CrEtapaEventoDTO> ObtenerCriterioxEtapaEvento(int crevencodi)
        {
            return FactorySic.GetCrEtapaEventoRepository().ObtenerCriterioxEtapaEvento(crevencodi);
        }
        /// <summary>
        /// Consultar Empresas Responsables
        /// <param name="cretapacodi"></param>
        /// </summary>
        /// <returns></returns>
        public List<CrEmpresaResponsableDTO> ListEmpresaResponsable(int cretapacodi)
        {
            return FactorySic.GetCrEmpresaResponsableRepository().ListrEmpresaResponsable(cretapacodi);
        }

        /// <summary>
        /// insertar registro en la tabla CR_EMPRESA_RESPONSABLE
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveCrEmpresaResponsable(CrEmpresaResponsableDTO entity)
        {
            FactorySic.GetCrEmpresaResponsableRepository().Save(entity);
        }

        /// <summary>
        /// valida si existe la empresa responsable en la etapa
        /// </summary>
        /// <param name="cretapacodi"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public bool ValidarCrEmpresaResponsable(int cretapacodi, int emprcodi)
        {
            return FactorySic.GetCrEmpresaResponsableRepository().ValidarEmpresaResponsable(cretapacodi, emprcodi);
        }

        /// <summary>
        /// Eliminar CrEmpresaResponsable
        /// </summary>
        /// <param name="crrespemprcodi"></param>
        /// <returns></returns>
        public void EliminarCrEmpresaResponsable(int crrespemprcodi)
        {
            FactorySic.GetCrEmpresaResponsableRepository().Delete(crrespemprcodi);
        }

        /// <summary>
        /// Eliminar CrEmpresaResponsable
        /// </summary>
        /// <param name="cretapacodi"></param>
        /// <returns></returns>
        public void EliminarCrEmpresaResponsableEtapa(int cretapacodi)
        {
            FactorySic.GetCrEmpresaResponsableRepository().DeletexEtapa(cretapacodi);
        }
        /// <summary>
        /// Listar CrEmpresaResponsable
        /// </summary>
        /// <param name="CREVENCODI"></param>
        /// <returns></returns>
        public List<CrEmpresaResponsableDTO> SqlObtenerEmpresaResponsablexEvento(int CREVENCODI)
        {
            return FactorySic.GetCrEmpresaResponsableRepository().SqlObtenerEmpresaResponsablexEvento(CREVENCODI);
        }

        /// <summary>
        /// Consultar Empresas Responsables
        /// <param name="cretapacodi"></param>
        /// </summary>
        /// <returns></returns>
        public List<CrEmpresaSolicitanteDTO> ListEmpresaSolicitante(int cretapacodi)
        {
            return FactorySic.GetCrEmpresaSolicitanteRepository().ListrEmpresaSolicitante(cretapacodi);
        }

        /// <summary>
        /// insertar registro en la tabla CR_EMPRESA_SOLICITANTE
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveCrEmpresaSolicitante(CrEmpresaSolicitanteDTO entity)
        {
            FactorySic.GetCrEmpresaSolicitanteRepository().Save(entity);
        }

        /// <summary>
        /// actualizar registro en la tabla CR_EMPRESA_SOLICITANTE
        /// </summary>
        /// <param name="crsolemprcodi"></param>
        /// <param name="argumentos"></param>
        /// <param name="desicion"></param>
        /// <returns></returns>
        public void ActualizarCrEmpresaSolicitante(int crsolemprcodi, string argumentos, string desicion)
        {
            FactorySic.GetCrEmpresaSolicitanteRepository().Update(crsolemprcodi, argumentos, desicion);
        }

        /// <summary>
        /// valida si existe la empresa solicitante en la etapa
        /// </summary>
        /// <param name="cretapacodi"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public bool ValidarCrEmpresaSolicitante(int cretapacodi, int emprcodi)
        {
            return FactorySic.GetCrEmpresaSolicitanteRepository().ValidarEmpresaSolicitante(cretapacodi, emprcodi);
        }

        /// <summary>
        /// Eliminar CrEmpresaSolicitante
        /// </summary>
        /// <param name="crsolemprcodi"></param>
        /// <returns></returns>
        public void EliminarCrEmpresaSolicitante(int crsolemprcodi)
        {
            FactorySic.GetCrEmpresaSolicitanteRepository().Delete(crsolemprcodi);
        }

        /// <summary>
        /// Eliminar CrEmpresaSolicitante por etapa
        /// </summary>
        /// <param name="cretapacodi"></param>
        /// <returns></returns>
        public void EliminarCrEmpresaSolicitantexEtapa(int cretapacodi)
        {
            FactorySic.GetCrEmpresaSolicitanteRepository().DeleteSolicitantexEtapa(cretapacodi);
        }

        /// <summary>
        /// Obtener Empresa Solicitante
        /// <param name="crsolemprcodi"></param>
        /// </summary>
        /// <returns></returns>
        public CrEmpresaSolicitanteDTO ObtenerEmpresaSolicitante(int crsolemprcodi)
        {
            return FactorySic.GetCrEmpresaSolicitanteRepository().ObtenerEmpresaSolicitante(crsolemprcodi);
        }
        /// <summary>
        /// Obtener Empresa Solicitante x evento
        /// <param name="CREVENCODI"></param>
        /// </summary>
        /// <returns></returns>
        public List<CrEmpresaSolicitanteDTO> SqlObtenerEmpresaSolicitantexEvento(int CREVENCODI)
        {
            return FactorySic.GetCrEmpresaSolicitanteRepository().SqlObtenerEmpresaSolicitantexEvento(CREVENCODI);
        }

        /// <summary>
        /// Permite registrar en tabla cr_etapa_evento
        /// <param name="entity"></param>
        /// </summary>
        public int SaveCrEtapaCriterio(CrEtapaCriterioDTO entity)
        {
            int id = 0;
            id = FactorySic.GetCrEtapaCriterioRepository().Save(entity);
            return id;
        }

        /// <summary>
        /// Permite eliminar en tabla cr_etapa_criterio
        /// <param name="cretapacodi"></param>
        /// </summary>
        public void DeleteCrEtapaCriterio(int cretapacodi)
        {
            FactorySic.GetCrEtapaCriterioRepository().Delete(cretapacodi);
        }

        /// <summary>
        /// Consultar Criterios por etapa
        /// <param name="cretapacricodi"></param>
        /// </summary>
        /// <returns></returns>
        public List<CrEtapaCriterioDTO> ListaCriteriosEtapa(int cretapacricodi)
        {
            return FactorySic.GetCrEtapaCriterioRepository().ListaCriteriosEtapa(cretapacricodi);
        }

        /// <summary>
        /// Consultar Criterios por evento - etapa
        /// <param name="cretapacodi"></param>
        /// </summary>
        /// <returns></returns>
        public List<CrEtapaCriterioDTO> ListaCriteriosEtapaEvento(int cretapacodi)
        {
            return FactorySic.GetCrEtapaCriterioRepository().ListaCriteriosEvento(cretapacodi);
        }

        /// <summary>
        /// Permite actualizar en tabla cr_evento
        /// <param name="entity"></param>
        /// </summary>
        public void UpdateCrEvento(CrEventoDTO entity)
        {
            FactorySic.GetCrEventoRepository().Update(entity);

        }

        /// <summary>
        /// Permite insertar en tabla cr_evento
        /// <param name="entity"></param>
        /// </summary>
        public int SaveCrEvento(CrEventoDTO entity)
        {
            return FactorySic.GetCrEventoRepository().Save(entity);
        }

        /// <summary>
        /// Permite eliminar en tabla cr_evento
        /// <param name="crevencodi"></param>
        /// </summary>
        public void DeleteCrEvento(int crevencodi)
        {
            FactorySic.GetCrEventoRepository().Delete(crevencodi);
        }

        /// <summary>
        /// Obtener CrEvento
        /// <param name="afecodi"></param>
        /// </summary>
        /// <returns></returns>
        public CrEventoDTO ObtenerCrEventoxAF(int afecodi)
        {
            return FactorySic.GetCrEventoRepository().GetByAfecodi(afecodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla CR_EVENTO
        /// <param name="crevencodi"></param>
        /// </summary>
        public List<CrEtapaEventoDTO> ListarCrEtapaEvento(int crevencodi)
        {
            return FactorySic.GetCrEtapaEventoRepository().ListarCrEtapaEvento(crevencodi);
        }

        /// <summary>
        /// Permite insertar en tabla cr_evento
        /// <param name="entity"></param>
        /// </summary>
        public int SaveCrEventoR(CrEventoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            return FactorySic.GetCrEventoRepository().SaveR(entity, conn, tran);
        }

        /// <summary>
        /// Permite registrar en tabla cr_etapa_evento
        /// <param name="entity"></param>
        /// </summary>
        public int SaveCrEtapaEventoR(CrEtapaEventoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            int id = 0;
            id = FactorySic.GetCrEtapaEventoRepository().SaveR(entity, conn, tran);
            return id;
        }
    }
}
