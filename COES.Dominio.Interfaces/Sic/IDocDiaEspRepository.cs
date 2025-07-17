using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla DOC_DIA_ESP
    /// </summary>
    public interface IDocDiaEspRepository
    {
        List<DocDiaEspDTO> List();
        List<DocDiaEspDTO> GetByCriteria();
        DateTime ObtenerFechaDiasHabiles(DateTime fInicio, int Dias);
        int NumDiasHabiles(DateTime dtFechaInicio, DateTime dtFechaFin);
    }
}
