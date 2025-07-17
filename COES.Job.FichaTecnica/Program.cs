using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.General;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Job.FichaTecnica
{
    internal class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        private static readonly TareaProgramadaAppServicio logic = new TareaProgramadaAppServicio();
        static void Main(string[] args)
        {
            log.Info("EjecutarProcesoTarea [Tarea automática] - Ficha técnica");
            List<SiProcesoDTO> listTask = logic.ObtenerTareasProgramadas(DateTime.Now);
            foreach (SiProcesoDTO item in listTask)
            {
                switch (item.Prcsmetodo)
                {
                    #region Ficha Tecnica 2 - 2024 
                    case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoSubsanar_Conexion:
                    case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoSubsanar_Integracion:
                    case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoSubsanar_OpComercial:
                    case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoSubsanar_Modif:
                    case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoSubsanar_ModifBaja:

                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitud_Conexion:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitud_Integracion:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitud_OpComercial:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitud_Modif:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitud_ModifBaja:

                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacion_Conexion:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacion_Integracion:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacion_OpComercial:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacion_Modif:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacion_ModifBaja:

                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitudAreas_Conexion:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitudAreas_Integracion:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitudAreas_OpComercial:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitudAreas_Modif:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSolicitudAreas_ModifBaja:

                    case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSolicitud_Conexion:
                    case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSolicitud_Integracion:
                    case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSolicitud_OpComercial:
                    case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSolicitud_Modif:
                    case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSolicitud_ModifBaja:

                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacionAreas_Conexion:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacionAreas_Integracion:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacionAreas_OpComercial:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacionAreas_Modif:
                    case ConstantesFichaTecnica.MetodoFT2RecordatorioVencPlazoParaRevisarSubsanacionAreas_ModifBaja:

                    case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSubsanacion_Conexion:
                    case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSubsanacion_Integracion:
                    case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSubsanacion_OpComercial:
                    case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSubsanacion_Modif:
                    case ConstantesFichaTecnica.MetodoFT2NotificacionCulminacionPlazoRevisarAreasSubsanacion_ModifBaja:
                    {
                        log.Info("Ficha Tecnica (Inicio).");
                        try
                        {
                            var servicioFT = new FichaTecnicaAppServicio();
                            int plantillaCorreo = servicioFT.ObtenerPlantillaCorreoSegunProceso(item.Prcscodi, out int ftetcodi);
                            servicioFT.EjecutarRecordatoriosManualmente(plantillaCorreo, ftetcodi);
                            log.Info("Ficha Tecnica (Fin):");
                        }
                        catch (Exception ex)
                        {
                            log.Error("Ficha Tecnica (error).");
                            log.Error(ex);
                        }
                        break;
                    }
                    default:
                    {

                        log.Error("Ficha Tecnica (Fin): Proceso ha culminado sin realizar actividad.");
                        break;
                    }
                    #endregion
                }
            }
        }
    }
}
