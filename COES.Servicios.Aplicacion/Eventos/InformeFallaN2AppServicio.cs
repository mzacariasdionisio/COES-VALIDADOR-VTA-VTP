using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Eventos.Helper;

namespace COES.Servicios.Aplicacion.InformefallaN2
{
    /// <summary>
    /// Clases con métodos del módulo Eventos
    /// </summary>
    public class InformefallaN2AppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(InformefallaN2AppServicio));

        #region Métodos Tabla EVE_INFORMEFALLA_N2

        /// <summary>
        /// Inserta un registro de la tabla EVE_INFORMEFALLA_N2
        /// </summary>
        public void SaveEveInformefallaN2(EveInformefallaN2DTO entity)
        {
            try
            {
                FactorySic.GetEveInformefallaN2Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_INFORMEFALLA_N2
        /// </summary>
        public void UpdateEveInformefallaN2(EveInformefallaN2DTO entity)
        {
            try
            {
                FactorySic.GetEveInformefallaN2Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_INFORMEFALLA_N2
        /// </summary>
        public void DeleteEveInformefallaN2(int eveninfn2codi)
        {
            try
            {
                FactorySic.GetEveInformefallaN2Repository().Delete(eveninfn2codi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_INFORMEFALLA_N2
        /// </summary>
        public EveInformefallaN2DTO GetByIdEveInformefallaN2(int eveninfn2codi)
        {
            return FactorySic.GetEveInformefallaN2Repository().GetById(eveninfn2codi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_INFORMEFALLA_N2
        /// </summary>
        public List<EveInformefallaN2DTO> ListEveInformefallaN2s()
        {
            return FactorySic.GetEveInformefallaN2Repository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveInformefallaN2
        /// </summary>
        public List<EveInformefallaN2DTO> GetByCriteriaEveInformefallaN2s()
        {
            return FactorySic.GetEveInformefallaN2Repository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla EVE_INFORMEFALLA_N2
        /// </summary>
        public int SaveEveInformefallaN2Id(EveInformefallaN2DTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Eveninfn2codi == 0)
                {
                    int idEvento = (new GeneralAppServicio()).ObtenerNextIdTabla(ConstantesEvento.TablaInformeFallaN2);
                    entity.Eveninfn2codi = idEvento;
                    id = FactorySic.GetEveInformefallaN2Repository().Save(entity);
                }
                else
                {
                    FactorySic.GetEveInformefallaN2Repository().Update(entity);
                    id = entity.Eveninfn2codi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla EVE_INFORMEFALLA_N2
        /// </summary>
        public List<EveInformefallaN2DTO> BuscarOperaciones(string infEmitido, int emprCodi, string equiAbrev,
            DateTime fechaIni, DateTime fechaFin, int nroPage, int pageSize)
        {
            return FactorySic.GetEveInformefallaN2Repository().BuscarOperaciones(infEmitido, emprCodi, equiAbrev, fechaIni, fechaFin, nroPage, pageSize);
        }


        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla EVE_INFORMEFALLA_N2
        /// </summary>
        public int ObtenerNroFilas(string infEmitido, int emprCodi, string equiAbrev,
            DateTime fechaIni, DateTime fechaFin)
        {
            return FactorySic.GetEveInformefallaN2Repository().ObtenerNroFilas(infEmitido, emprCodi, equiAbrev, fechaIni, fechaFin);
        }


        #endregion

    }
}
