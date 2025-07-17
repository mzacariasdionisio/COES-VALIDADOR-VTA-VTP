using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCE_DAT_CALCULO
    /// </summary>
    public interface IVceDatcalculoRepository
    {
        void Save(VceDatcalculoDTO entity);
        void Update(VceDatcalculoDTO entity);
        void Delete(DateTime crdcgfecmod, int grupocodi, int pecacodi);
        VceDatcalculoDTO GetById(DateTime crdcgfecmod, int grupocodi, int pecacodi);
        List<VceDatcalculoDTO> List();
        List<VceDatcalculoDTO> GetByCriteria();

        //NETC
        List<VceDatcalculoDTO> ListByFiltro(int pecacodi, string empresa, string central, string grupo, string modo);
        VceDatcalculoDTO GetByIdGrupo(int pecacodi, int grupocodi);
        void UpdateCalculo(VceDatcalculoDTO entity);

        //- compensaciones.HDT - Inicio 03/03/2017: Cambio para atender el requerimiento.
        /// <summary>
        /// Permite configurar los parámetros para el cálculo.
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <param name="periAnioMes"></param>
        /// <param name="pecaTipoCambio"></param>
        void ConfigurarParametroCalculo(int pecacodi, ref string periAnioMes, ref string pecaTipoCambio);

        //- compensaciones.HDT - Inicio 03/03/2017: Cambio para atender el requerimiento.
        /// <summary>
        /// Puebla los registros sin los cálculos, estos serán actualizados posteriormente.
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <param name="perianiomes"></param>
        void PoblarRegistroSinCalculos(int pecacodi, string perianiomes);

        //- compensaciones.HDT - Inicio 03/03/2017: Cambio para atender el requerimiento.
        /// <summary>
        /// Obtiene una lista de las distintas fechas de modificación.
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <returns></returns>
        List<DateTime> ObtenerDistintasFechasModificacion(int pecacodi);

        //- compensaciones.HDT - Inicio 16/03/2017: Cambio para atender el requerimiento.
        /// <summary>
        /// Obtiene una lista de las distintas fechas de modificación de los registros pendientes de actualizar por Potencia Efectiva.
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <returns></returns>
        List<DateTime> ObtenerDistintasFechasModificacionPotenciaEfectiva(int pecacodi);
        
        //- compensaciones.HDT - Inicio 03/03/2017: Cambio para atender el requerimiento.
        /// <summary>
        /// Obtiene los distintos identificadores de grupo de un periodo dado.
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <param name="fenergcodi"></param>
        /// <param name="cfgdccondsql"></param>
        /// <returns></returns>
        List<int> ObtenerDistintosIdGrupo(int pecacodi, int fenergcodi, string cfgdccondsql);

        //- compensaciones.HDT - Inicio 16/03/2017: Cambio para atender el requerimiento.
        /// <summary>
        /// Obtiene los distintos identificadores de grupo de un periodo dado.
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <returns></returns>
        List<int> ObtenerDistintosIdGrupo(int pecacodi);

        void SaveCalculo(int pecacodi, List<VceCfgDatCalculoDTO> lVceCfgDatCalculoDTO);
        //- HDT Fin

        //- compensaciones.HDT - Inicio 03/03/2017: Cambio para atender el requerimiento.
        /// <summary>
        /// Permite actualizar los datos pos cálculo.
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <param name="pecatipocambio"></param>
        void ActualizarDatosPosCalculo(int pecacodi, string pecatipocambio);

        //- compensaciones.HDT - Inicio 03/03/2017: Cambio para atender el requerimiento.
        /// <summary>
        /// Obtiene los registros sin cálculo alguno.
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <returns></returns>
        List<VceDatcalculoDTO> ObtenerRegistroSinCalculos(int pecacodi);

        //- compensaciones.HDT - Inicio 16/03/2017: Cambio para atender el requerimiento.
        /// <summary>
        /// Obtiene los registros sin cálculo alguno.
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <returns></returns>
        List<VceDatcalculoDTO> ObtenerRegistroSinCalculosPotenciaEfectica(int pecacodi);
        
        void DeleteCalculo(int pecacodi);
        IDataReader ListCompensacionArrPar(int pecacodi, string empresa, string central, string grupo, string modo);
        IDataReader ListCabCompensacionArrPar(int pecacodi);

        //- compensaciones.JDEL - Inicio 05/03/2017: Cambio para atender el requerimiento.
        void SaveArranquesParadas(int pecacodi);

        //- compensaciones.HDT - Inicio 16/03/2017: Cambio para atender el requerimiento.
        /// <summary>
        /// Realiza la actualización del tipo de combustible.
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <param name="fechaModificacion"></param>
        void ActualizarTipoCombustible(int pecacodi, string fechaModificacion);
        // DSH 06-05-2017 : Se agrego por requerimiento
        void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen);
    }
}
