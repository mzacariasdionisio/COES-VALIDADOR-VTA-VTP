using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla DOC_FLUJO
    /// </summary>
    public interface IDocFlujoRepository
    {
        List<DocFlujoDTO> ListEstad(DateTime fechaInicio,DateTime fechaFin,String listaTipoAtencion);
        String GetDocRespuesta(int fljcodiref);
        List<DocFlujoDTO> GetAreasResponsables(int fljcodi, String ListaAreasPadres);

        #region MigracionSGOCOES-GrupoB
        List<DocFlujoDTO> ListDocCV(DateTime fechaIni, DateTime fechaFin);
        #endregion
    }
}
