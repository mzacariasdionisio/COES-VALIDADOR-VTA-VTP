using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_DATO_SENIAL
    /// </summary>
    public interface ICoDatoSenialRepository
    {
        int Save(CoDatoSenialDTO entity);
        void Update(CoDatoSenialDTO entity);
        void Delete(int codasecodi);
        CoDatoSenialDTO GetById(int codasecodi);
        List<CoDatoSenialDTO> List();
        List<CoDatoSenialDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin, int urs, int canalcodi);
        List<CoDatoSenialDTO> ObtenerListaPorFechas(string fecIni, string fecFin);
    }
}
