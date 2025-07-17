using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_CONFIGURACION_GEN
    /// </summary>
    public interface ICoConfiguracionGenRepository
    {
        int Save(CoConfiguracionGenDTO entity);
        void Update(CoConfiguracionGenDTO entity);
        void Delete(int courgecodi);
        CoConfiguracionGenDTO GetById(int courgecodi);
        List<CoConfiguracionGenDTO> List();
        List<CoConfiguracionGenDTO> GetByCriteria(int idConfiguracionDet);
        List<CoConfiguracionGenDTO> GetUnidadesSeleccionadas(string strCourdecodis);
    }
}
