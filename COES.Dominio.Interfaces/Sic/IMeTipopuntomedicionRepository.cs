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
        #region Modificaci�n Tipo punto de medici�n
        List<MeTipopuntomedicionDTO> ListarTiposPuntoMedicion(int famCodi, int tipoInfoCodi);
        #endregion
        #region Medidores de Generaci�n PR15
        /// <summary>
        /// M�todo que lista los tipos de punto de medici�n filtrados por tipo de informaci�n
        /// </summary>
        /// <param name="tipoinfocodi">C�digo de tipo de informaci�n</param>
        /// <returns>Listado de Tipos de punto de medici�n</returns>
        List<MeTipopuntomedicionDTO> ListarTiposPuntoMedicionPorTipoInformacion(int tipoinfocodi);
        #endregion

        List<MeTipopuntomedicionDTO> ListFromPtomedicion(string origlectcodi);
    }
}
