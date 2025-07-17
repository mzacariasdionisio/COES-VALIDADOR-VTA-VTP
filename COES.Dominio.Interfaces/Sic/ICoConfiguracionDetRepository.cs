using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_CONFIGURACION_DET
    /// </summary>
    public interface ICoConfiguracionDetRepository
    {
        int Save(CoConfiguracionDetDTO entity);
        void Update(CoConfiguracionDetDTO entity);
        void Delete(int courdecodi);
        CoConfiguracionDetDTO GetById(int courdecodi);
        List<CoConfiguracionDetDTO> List();
        List<CoConfiguracionDetDTO> GetByCriteria(int idConfiguracion);
        List<CoConfiguracionDetDTO> ObtenerConfiguracion(int idPeriodo, int idVersion);
        List<CoConfiguracionDetDTO> ObtenerConfiguracionDetalle(string strFecha);
        
    }
}
