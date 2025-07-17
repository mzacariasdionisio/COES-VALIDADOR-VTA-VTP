using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_CAUSA_INTERRUPCION
    /// </summary>
    public interface IReCausaInterrupcionRepository
    {
        int Save(ReCausaInterrupcionDTO entity);
        void Update(ReCausaInterrupcionDTO entity);
        void Delete(int recintcodi);
        ReCausaInterrupcionDTO GetById(int recintcodi);
        List<ReCausaInterrupcionDTO> List();
        List<ReCausaInterrupcionDTO> GetByCriteria(int periodo);
        List<ReCausaInterrupcionDTO> ObtenerConfiguracion(int idTipo);
        List<ReCausaInterrupcionDTO> ObtenerCausasInterrupcionUtilizadosPorPeriodo(int idPeriodo, int idSuministrador);
    }
}
