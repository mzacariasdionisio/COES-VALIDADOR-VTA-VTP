using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_OFERTA
    /// </summary>
    public interface IVcrOfertaRepository
    {
        int Save(VcrOfertaDTO entity);
        void Update(VcrOfertaDTO entity);
        void Delete(int vcrecacodi);
        VcrOfertaDTO GetById(int vcrofecodi);
        List<VcrOfertaDTO> List();
        List<VcrOfertaDTO> GetByCriteria();
        VcrOfertaDTO GetByFwUserByNombre(string Username);
        VcrOfertaDTO GetByIdMaxDia(int vcrecacodi, DateTime dFecha);
        VcrOfertaDTO GetByIdMaxDiaGrupoCodi(int vcrecacodi, int grupocodi, DateTime dFecha);
        VcrOfertaDTO GetByIdMaxMes(int vcrecacodi, DateTime dFecha); //, int grupocodi
        VcrOfertaDTO GetByIdMaxDiaUrs(int vcrecacodi, DateTime dFecha);
        //ASSETEC 20190115
        List<VcrOfertaDTO> ListSinDuplicados(int vcrecacodi);
        VcrOfertaDTO GetByCriteriaOferta(int vcrecacodi, int grupocodi, DateTime vcrofefecha, string vcrofecodigoenv, DateTime vcrofehorinicio, int vcrofetipocarga);

        decimal GetOfertaMaxDiaGrupoCodiHiHf(int vcrecacodi, int grupocodi, DateTime dFecha, DateTime dHoraInicio, DateTime dHoraFinal, int vcrofetipocarga);

        decimal GetOfertaMaxDiaGrupoCodiHiHf2020(int vcrecacodi, int grupocodi, DateTime dFecha, DateTime dHoraInicio, DateTime dHoraFinal, int vcrofetipocarga);
    }
}
