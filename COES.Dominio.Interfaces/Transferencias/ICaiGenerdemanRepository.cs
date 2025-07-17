using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_GENERDEMAN
    /// </summary>
    public interface ICaiGenerdemanRepository
    {
        int Save(CaiGenerdemanDTO entity);
        void Update(CaiGenerdemanDTO entity);
        void Delete(int caiajcodi, string cagdcmfuentedat);
        CaiGenerdemanDTO GetById(int cagdcmcodi);
        List<CaiGenerdemanDTO> List();
        List<CaiGenerdemanDTO> GetByCriteria();
        List<CaiGenerdemanDTO> GetByUsuarioLibresSGOCOES(string sFechaInicio, string sFechaFin, int iFormatcodi, int iTipoEmprcodi, string lectCodiPR16, string lectCodiAlpha);
        int GetCodigoGenerado();
        void BulkInsert(List<CaiGenerdemanDTO> entitys);
        List<CaiGenerdemanDTO> ListGenDemBarrMes(int caiajcodi, int emprcodi, int ptomedicodi, int caajcmmes, int pericodi, int recacodi);
        List<CaiGenerdemanDTO> ListGenDemProyMes(int caiajcodi, int emprcodi, int ptomedicodi, int caajcmmes);
        void SaveAsSelectUsuariosLibres(Int32 cagdcmcodi, int caiajcodi, string cagdcmfuentedat, string cagdcmcalidadinfo, string T, string user, int Formatcodi, string FechaInicio, string FechaFin, int TipoEmprcodi, string lectCodiPR16, string lectCodiAlpha);
        void SaveCaiGenerdemanAsSelectMeMedicion96(int cagdcmcodi, int caiajcodi, string cagdcmfuentedat, string cagdcmcalidadinfo, string tipodato, string user, int Formatcodi, int Lectcodi, string FechaInicio, string FechaFin);
        
    }
}
