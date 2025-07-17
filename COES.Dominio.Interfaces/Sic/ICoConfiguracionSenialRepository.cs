using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_CONFIGURACION_SENIAL
    /// </summary>
    public interface ICoConfiguracionSenialRepository
    {
        int Save(CoConfiguracionSenialDTO entity);
        void Update(CoConfiguracionSenialDTO entity);
        void Delete(int consencodi);
        CoConfiguracionSenialDTO GetById(int consencodi);
        List<CoConfiguracionSenialDTO> List();
        List<CoConfiguracionSenialDTO> GetByCriteria(int idConfiguracionDet);
        List<CoConfiguracionSenialDTO> ListarSeniales(int copercodi, int covercodi);
        List<CoConfiguracionSenialDTO> ListarSenialesPeriodosAnteriores(int anio, int mes, string strcanalcodis);
        List<CoConfiguracionSenialDTO> ObtenerSenialesPorUrs(int idUrs);
    }
}
