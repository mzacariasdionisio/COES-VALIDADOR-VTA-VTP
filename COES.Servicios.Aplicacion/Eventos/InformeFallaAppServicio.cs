using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Linq;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Eventos.Helper;

namespace COES.Servicios.Aplicacion.Informefalla
{
    /// <summary>
    /// Clases con métodos del módulo Eventos
    /// </summary>
    public class InformefallaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(InformefallaAppServicio));

        #region Métodos Tabla EVE_INFORMEFALLA

        /// <summary>
        /// Inserta un registro de la tabla EVE_INFORMEFALLA
        /// </summary>
        public void SaveEveInformefalla(EveInformefallaDTO entity)
        {
            try
            {
                FactorySic.GetEveInformefallaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_INFORMEFALLA
        /// </summary>
        public void UpdateEveInformefalla(EveInformefallaDTO entity)
        {
            try
            {
                FactorySic.GetEveInformefallaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_INFORMEFALLA
        /// </summary>
        public void DeleteEveInformefalla(int eveninfcodi)
        {
            try
            {
                FactorySic.GetEveInformefallaRepository().Delete(eveninfcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_INFORMEFALLA
        /// </summary>
        public EveInformefallaDTO GetByIdEveInformefalla(int eveninfcodi)
        {
            return FactorySic.GetEveInformefallaRepository().GetById(eveninfcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_INFORMEFALLA
        /// </summary>
        public List<EveInformefallaDTO> ListEveInformefallas()
        {
            return FactorySic.GetEveInformefallaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveInformefalla
        /// </summary>
        public List<EveInformefallaDTO> GetByCriteriaEveInformefallas()
        {
            return FactorySic.GetEveInformefallaRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla EVE_INFORMEFALLA
        /// </summary>
        public int SaveEveInformefallaId(EveInformefallaDTO entity)
        {

            try
            {
                int id = 0;

                if (entity.Eveninfcodi == 0)
                {
                    int idEvento = (new GeneralAppServicio()).ObtenerNextIdTabla(ConstantesEvento.TablaInformeFalla);
                    entity.Eveninfcodi = idEvento;
                    id = FactorySic.GetEveInformefallaRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetEveInformefallaRepository().Update(entity);
                    id = entity.Eveninfcodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla EVE_INFORMEFALLA
        /// </summary>
        public List<EveInformefallaDTO> BuscarOperaciones(string infMem, string infEmitido, int emprCodi, string equiAbrev,
            DateTime fechaIni, DateTime fechaFin, int nroPage, int pageSize)
        {
            return FactorySic.GetEveInformefallaRepository().BuscarOperaciones(infMem, infEmitido, emprCodi, equiAbrev, fechaIni, fechaFin, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla EVE_INFORMEFALLA
        /// </summary>
        public int ObtenerNroFilas(string infMem, string infEmitido, int emprCodi, string equiAbrev,
            DateTime fechaIni, DateTime fechaFin)
        {
            return FactorySic.GetEveInformefallaRepository().ObtenerNroFilas(infMem, infEmitido, emprCodi, equiAbrev, fechaIni, fechaFin);
        }

        #endregion

        #region Logica para alarmas de Envio de Informes de Fallas

        /// <summary>
        /// Permite obtener el listado de informes de fallas por vencer y vencidos
        /// </summary>
        /// <returns></returns>
        public List<EveInformefallaDTO> ObtenerAlertaInformeFallas()
        {
            List<EveInformefallaDTO> result = new List<EveInformefallaDTO>();
            List<EveInformefallaDTO> list = FactorySic.GetEveInformefallaRepository().ObtenerAlertaInformeFalla();

            List<EveInformefallaDTO> listVencidos = list.Where(x => x.Plazo <= 0 && x.Plazo >= -24).ToList();



            List<EveInformefallaDTO> listPorVencer = list.Where(x => x.Plazo > 0).ToList();

            //- SCO_IPI_N1 --60 minutos y 30 minutos
            //- SCO_IP_N1  --60 minutos y 30 minutos
            //- SCO_IF_N1  --60 horas, 30 horas, 10 horas, 5 horas, 2 horas
            //- SCO_IPI_N2 --30 minutos
            //- SCO_IF_N2  --30 minutos

            List<EveInformefallaDTO> listN1 = listPorVencer.Where(x => (
                                                                          x.Correlativo.StartsWith("SCO_IPI_N1") ||
                                                                          x.Correlativo.StartsWith("SCO_IP_N1")) && (
                                                                          Math.Round(x.Plazo, 1) == 0.5M ||
                                                                          Math.Round(x.Plazo) == 1M)).ToList();

            List<EveInformefallaDTO> listN1F = listPorVencer.Where(x =>
                                                                          x.Correlativo.StartsWith("SCO_IF_N1") && (
                                                                            (x.Plazo > 60 && x.Plazo - 60 < 0.1M) ||
                                                                            (x.Plazo > 30 && x.Plazo - 30 < 0.1M) ||
                                                                            (x.Plazo > 10 && x.Plazo - 10 < 0.1M) ||
                                                                            (x.Plazo > 5 && x.Plazo - 5 < 0.1M) ||
                                                                            (x.Plazo > 2 && x.Plazo - 2 < 0.1M)
                                                                          )).ToList();

            List<EveInformefallaDTO> listN2 = listPorVencer.Where(x => (
                                                                          x.Correlativo.StartsWith("SCO_IPI_N2") ||
                                                                          x.Correlativo.StartsWith("SCO_IF_N2")) &&
                                                                          Math.Round(x.Plazo, 1) == 0.5M).ToList();

            result.AddRange(listN1);
            result.AddRange(listN1F);
            result.AddRange(listN2);

            //- Listado de los informes vencidos
            if (DateTime.Now.Minute == 0 || DateTime.Now.Minute == 30)
            {
                result.AddRange(listVencidos);
            }

            return result;
        }

        #endregion

    }
}
