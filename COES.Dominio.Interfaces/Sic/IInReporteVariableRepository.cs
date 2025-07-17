using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_REPORTE_VARIABLE
    /// </summary>
    public interface IInReporteVariableRepository
    {
        int Save(InReporteVariableDTO entity);
        void Update(InReporteVariableDTO entity);
        void Delete(int inrevacodi);
        InReporteVariableDTO GetById(int inrevacodi);
        List<InReporteVariableDTO> List();
        List<InReporteVariableDTO> GetByCriteria(int progcodi, int tipo);
    }
}
