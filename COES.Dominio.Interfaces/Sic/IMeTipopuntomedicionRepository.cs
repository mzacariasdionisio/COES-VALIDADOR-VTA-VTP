using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_TIPOPUNTOMEDICION
    /// </summary>
    public interface IMeTipopuntomedicionRepository
    {
        int Save(MeTipopuntomedicionDTO entity);
        void Update(MeTipopuntomedicionDTO entity);
        void Delete(int tipoptomedicodi);
        MeTipopuntomedicionDTO GetById(int tipoptomedicodi);
        List<MeTipopuntomedicionDTO> List(string origlectcodi);
        List<MeTipopuntomedicionDTO> GetByCriteria();
        List<MeTipopuntomedicionDTO> ListarMeTipoPuntoMedicion(string StrTptoMedicodi, string idsestado);
        #region Modificación Tipo punto de medición
        List<MeTipopuntomedicionDTO> ListarTiposPuntoMedicion(int famCodi, int tipoInfoCodi);
        #endregion
        #region Medidores de Generación PR15
        /// <summary>
        /// Método que lista los tipos de punto de medición filtrados por tipo de información
        /// </summary>
        /// <param name="tipoinfocodi">Código de tipo de información</param>
        /// <returns>Listado de Tipos de punto de medición</returns>
        List<MeTipopuntomedicionDTO> ListarTiposPuntoMedicionPorTipoInformacion(int tipoinfocodi);
        #endregion

        List<MeTipopuntomedicionDTO> ListFromPtomedicion(string origlectcodi);
    }
}
