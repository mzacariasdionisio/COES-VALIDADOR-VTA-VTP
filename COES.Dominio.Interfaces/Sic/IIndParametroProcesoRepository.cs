using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IIndParametroProcesoRepository
    {
        int Save(IndParametroProcesoDTO entity);
        void Update(IndParametroProcesoDTO entity);
        void Delete(int CbcTrtCodi);
        List<IndParametroProcesoDTO> List();
        IndParametroProcesoDTO GetById(int ParmetCodi);
        List<IndParametroProcesoDTO> GetByCriteria();

        List<IndParametroProcesoDTO> ParametrosProcesosFiltroConRangoDeFechas(string fechaInicio, string fechaFin);
        int UpdateParamProcConEstadoActivo(IndParametroProcesoDTO entity);
        int UpdateParmProcAnuevoParamActivo(IndParametroProcesoDTO entity);
        int AsignarIDParmProceso();
        int ValidarPeriodoParaRegistroContratoCombustible(IndParametroProcesoDTO entity);
    }
}
