using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.CortoPlazo
{
    public class CalculoCostosMarginales
    {
        /// <summary>
        /// Permite calcular los costos marginales de corto plazo
        /// </summary>
        /// <param name="fechaProceso">Fecha en que se está ejecutando el proceso</param>
        /// <param name="indicadorEstimador">P: PSS / ODMS, T: TNA</param>
        /// <param name="indicadorPSSE">1: Es archivo manual, 0: Es archivo por defecto</param>
        /// <param name="reproceso">true: Corresponde a un reproceso, false: es en tiempo real</param>
        /// <param name="indicadorNCP">true: se especifica una nueva ruta de archivos NCP, false, ruta por defecto</param>
        /// <param name="flagWeb">true: se muestra en el portal, false, no se muestra en el portal</param>
        /// <param name="rutaNCP">En caso que indicadorNCP sea true, puede indicarse una carpeta especifica de archivos NCP</param>
        /// <param name="flagMD">Si se utilizará NCP o YUPANA</param>
        /// <param name="idEscenario">Codigo de escenario en YUPANA</param>
        /// <param name="usuario">Usuario que está realizando el proceso</param>
        public void ProcesarCM(
            DateTime fechaProceso, 
            string indicadorEstimador,
            int indicadorPSSE, 
            bool reproceso, 
            bool indicadorNCP, 
            bool flagWeb, 
            string rutaNCP, 
            bool flagMD, 
            int idEscenario, 
            string usuario,
            int tipo,
            int version)
        {
            if (indicadorEstimador == ConstantesCortoPlazo.EstimadorTNA)
            {
                if (version == ConstantesCortoPlazo.VersionCMOriginal)
                {
                    CostoMarginalTnaAppServicio cm = new CostoMarginalTnaAppServicio();
                    cm.Procesar(fechaProceso, indicadorPSSE, reproceso, indicadorNCP, flagWeb, rutaNCP, flagMD, idEscenario, usuario, tipo);
                }
                else if(version == ConstantesCortoPlazo.VersionCMPR07)
                {
                    CostoMarginalTnaV2AppServicio cm = new CostoMarginalTnaV2AppServicio();
                    cm.Procesar(fechaProceso, indicadorPSSE, reproceso, indicadorNCP, flagWeb, rutaNCP, flagMD, idEscenario, usuario, tipo);
                }
            }
            else if (indicadorEstimador == ConstantesCortoPlazo.EstimadorPSS)
            {
                CostoMarginalAppServicio cm = new CostoMarginalAppServicio();
                cm.Procesar(fechaProceso, indicadorPSSE, reproceso, indicadorNCP, flagWeb, rutaNCP, flagMD, idEscenario, usuario, tipo);
            }
        }

        /// <summary>
        /// Permite procesar los costos marginales masivamente
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="horas"></param>
        /// <param name="flagMD"></param>
        /// <param name="usuario"></param>
        /// <param name="indicadorEstimador"></param>
        public void ProcesarMasivoCM(
            DateTime fechaInicio,
            DateTime fechaFin,
            List<string> horas,
            bool flagMD,
            string usuario,
            string indicadorEstimador,
            int versionModelo
            )
        {
            if (indicadorEstimador == ConstantesCortoPlazo.EstimadorTNA)
            {
                if (versionModelo == ConstantesCortoPlazo.VersionCMOriginal)
                {
                    CostoMarginalTnaAppServicio cm = new CostoMarginalTnaAppServicio();
                    cm.ProcesarMasivo(fechaInicio, fechaFin, horas, flagMD, usuario);
                }
                else if(versionModelo == ConstantesCortoPlazo.VersionCMPR07)
                {
                    CostoMarginalTnaV2AppServicio cm = new CostoMarginalTnaV2AppServicio();
                    cm.ProcesarMasivo(fechaInicio, fechaFin, horas, flagMD, usuario);
                }            
            
            }
            else if (indicadorEstimador == ConstantesCortoPlazo.EstimadorPSS)
            {
                CostoMarginalAppServicio cm = new CostoMarginalAppServicio();
                cm.ProcesarMasivo(fechaInicio, fechaFin, horas, flagMD, usuario);
            }
        }

        /// <summary>
        /// Permite ejecutar de forma masiva los costos marginales
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="usuario"></param>
        public void ProcesarMasivoCMModificado(string[][] datos, string usuario, int version)
        {
            if (version == ConstantesCortoPlazo.VersionCMOriginal)
            {
                CostoMarginalTnaAppServicio cm = new CostoMarginalTnaAppServicio();
                cm.ProcesarMasivoModificado(datos, usuario);
            }
            else if (version == ConstantesCortoPlazo.VersionCMPR07)
            {
                CostoMarginalTnaV2AppServicio cm = new CostoMarginalTnaV2AppServicio();
                cm.ProcesarMasivoModificado(datos, usuario);
            }
        }

        /// <summary>
        /// Permite ejecutar de forma masiva los costos marginales
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="usuario"></param>
        public void ProcesarMasivoTIE(string[][] datos, string usuario, DateTime fechaProceso, int idBarra, int version)
        {
            ParametrosAnguloOptimo result = (new ReprocesoAppServicio()).ReprocesoPorTransaccionInternacional(fechaProceso, datos, idBarra, version);

            if (result.Resultado == 1)
            {
                if (version == ConstantesCortoPlazo.VersionCMOriginal)
                {
                    CostoMarginalTnaAppServicio cm = new CostoMarginalTnaAppServicio();
                    cm.ProcesarMasivoTIE(result, usuario);
                }
                else if (version == ConstantesCortoPlazo.VersionCMPR07)
                {
                    CostoMarginalTnaV2AppServicio cm = new CostoMarginalTnaV2AppServicio();
                    cm.ProcesarMasivoTIE(result, usuario);
                }
            }          
        }

        /// <summary>
        /// Permite ejecutar el proceso de CM en modo automático
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="indicadorPSSE"></param>
        /// <param name="reproceso"></param>
        /// <param name="indicadorNCP"></param>
        /// <param name="flagWeb"></param>
        /// <param name="rutaNCP"></param>
        /// <param name="flagMD"></param>
        /// <param name="idEscenario"></param>
        /// <param name="usuario"></param>
        /// <param name="tipo"></param>
        public void EjecutarProcesoCM(DateTime fecha, int indicadorPSSE, bool reproceso, bool indicadorNCP, bool flagWeb,
            string rutaNCP, bool flagMD, int idEscenario, string usuario, int tipo)
        {
            string indicadorV1 = ConfigurationManager.AppSettings[ConstantesCortoPlazo.EjecucionVersionActualCM];
            string indicadorV2 = ConfigurationManager.AppSettings[ConstantesCortoPlazo.EjecucionVersionPR07CM];
            string orden = ConfigurationManager.AppSettings[ConstantesCortoPlazo.OrdenEjecucionModelo];

            if (orden == ConstantesAppServicio.SI)
            {
                if (indicadorV2 == ConstantesAppServicio.SI)
                {
                    CostoMarginalTnaV2AppServicio cmTna = new CostoMarginalTnaV2AppServicio();
                    cmTna.Procesar(fecha, indicadorPSSE, reproceso, indicadorNCP, flagWeb, string.Empty, true, 0, ConstantesCortoPlazo.UsuarioAutomatico, 0);
                }

                if (indicadorV1 == ConstantesAppServicio.SI)
                {
                    CostoMarginalTnaAppServicio cmTna = new CostoMarginalTnaAppServicio();
                    cmTna.Procesar(fecha, indicadorPSSE, reproceso, indicadorNCP, flagWeb, string.Empty, true, 0, ConstantesCortoPlazo.UsuarioAutomatico, 0);
                }
            }
            else 
            {
                if (indicadorV1 == ConstantesAppServicio.SI)
                {
                    CostoMarginalTnaAppServicio cmTna = new CostoMarginalTnaAppServicio();
                    cmTna.Procesar(fecha, indicadorPSSE, reproceso, indicadorNCP, flagWeb, string.Empty, true, 0, ConstantesCortoPlazo.UsuarioAutomatico, 0);
                }

                if (indicadorV2 == ConstantesAppServicio.SI)
                {
                    CostoMarginalTnaV2AppServicio cmTna = new CostoMarginalTnaV2AppServicio();
                    cmTna.Procesar(fecha, indicadorPSSE, reproceso, indicadorNCP, flagWeb, string.Empty, true, 0, ConstantesCortoPlazo.UsuarioAutomatico, 0);
                }

            }
        }


        /// <summary>
        /// Permite ejecutar de forma masiva los costos marginales
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="usuario"></param>
        public void ProcesarMasivoVA(string horas, string usuario, DateTime fechaProceso, int version)
        {            
            if (version == ConstantesCortoPlazo.VersionCMPR07)
            {
                CostoMarginalTnaV2AppServicio cm = new CostoMarginalTnaV2AppServicio();
                cm.ProcesarMasivoVA(horas, fechaProceso, usuario, version);
            }

        }
    }
}
