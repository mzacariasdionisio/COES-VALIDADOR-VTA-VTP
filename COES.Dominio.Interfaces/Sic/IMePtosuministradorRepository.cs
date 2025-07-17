using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_PTOSUMINISTRADOR
    /// </summary>
    public interface IMePtosuministradorRepository
    {
        int Save(MePtosuministradorDTO entity);
        void Update(MePtosuministradorDTO entity);
        void Delete(int ptosucodi);
        MePtosuministradorDTO GetById(int ptosucodi);
        List<MePtosuministradorDTO> List();
        List<MePtosuministradorDTO> GetByCriteria();

        //- pr16.JDEL - Inicio 21/10/2016: Cambio para atender el requerimiento.
        List<MePtosuministradorDTO> ListaEditorPtoSuministro(string periodo, int empresa, int formato);
        MePtosuministradorDTO GetByPtoPeriodo(int ptomedicodi, string fecha);
        //- JDEL Fin
        MePtosuministradorDTO ObtenerSuministradorVigente(int ptomedicodi);
        #region Rechazo Carga
        List<RcaSuministradorDTO> ListaSuministradoresRechazoCarga();
        #endregion
    }
}
