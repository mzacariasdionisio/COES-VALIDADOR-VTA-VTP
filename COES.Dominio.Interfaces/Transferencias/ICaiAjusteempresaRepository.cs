using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_AJUSTEEMPRESA
    /// </summary>
    public interface ICaiAjusteempresaRepository
    {
        int Save(CaiAjusteempresaDTO entity);
        void Update(CaiAjusteempresaDTO entity);
        void Delete(int caiajecodi);
        CaiAjusteempresaDTO GetById(int caiajecodi);
        List<CaiAjusteempresaDTO> List();
        List<CaiAjusteempresaDTO> GetByCriteria();
        List<CaiAjusteempresaDTO> ListAjuste(int caiajcodi, string caiajetipoinfo);
        List<CaiAjusteempresaDTO> ListAjusteEmpresa(int caiajcodi, int emprcodi);
        List<CaiAjusteempresaDTO> ListEmpresasByAjuste(int caiajcodi);
        List<CaiAjusteempresaDTO> ListCaiAjusteempresasTipoEmpresa(int caiajcodi);
        List<CaiAjusteempresaDTO> ListEmpresasXPtoGeneracion(string sFechaInicio, string sFechaFin, string tiposGeneracion, string empresas, int IdFamiliaSSAA, int IdTipogrupoNoIntegrante, int lectcodi, int IdTipoInfoPotenciaActiva, int TptoMedicodiTodos);
        List<CaiAjusteempresaDTO> ListEmpresasXPtoUL(string sFechaInicio, string sFechaFin, int iFormatcodi, int iTipoEmprcodi, string lectCodiPR16, string lectCodiAlpha);
        List<CaiAjusteempresaDTO> ListEmpresaByAjusteTipoEmpresa(int caiajcodi, int tipoemprcodi);
        List<CaiAjusteempresaDTO> ObtenerListaPeriodoEjecutado(string caiajetipoinfo, int caiajcodi, int emprcodi);
        List<CaiAjusteempresaDTO> ObtenerListaPeriodoProyectado(string caiajetipoinfo, int caiajcodi, int emprcodi);
        MePtomedicionDTO GetMePtomedicionByNombre(int Emprcodi, string Ptomedidesc);
        List<CaiAjusteempresaDTO> ListEmpresasXPtoDist();
        List<CaiAjusteempresaDTO> ListEmpresasXPtoTrans();
        List<MeHojaptomedDTO> GetByCriteriaMeHojaptomeds(int emprcodi, int formatcodi);
        List<MePtomedicionDTO> ListPtomed(int origlectcodi);
    }
}
