using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_PARAMETRO_VALOR
    /// </summary>
    public interface ISiParametroValorRepository
    {
        int Save(SiParametroValorDTO entity);
        void Update(SiParametroValorDTO entity);
        void Delete(int siparvcodi);
        SiParametroValorDTO GetById(int siparvcodi);
        List<SiParametroValorDTO> List();
        List<SiParametroValorDTO> GetByCriteria();
        int SaveSiParametroValorId(SiParametroValorDTO entity);
        List<SiParametroValorDTO> BuscarOperaciones(int siparCodi, DateTime siparvFechaInicial, DateTime siparvFechaFinal, int nroPage, int PageSize, string estado);
        int ObtenerNroFilas(int siparCodi, DateTime siparvFechaInicial, DateTime siparvFechaFinal, string estado);
        //inicio agregado
        List<SiParametroValorDTO> ListByIdParametro(int siparCodi);
        List<SiParametroValorDTO> ListByIdParametroAndFechaInicial(int siparCodi, DateTime fechaInicial);
        decimal ObtenerValorParametro(int parametro, DateTime fecha);
        //fin agregado
    }
}
