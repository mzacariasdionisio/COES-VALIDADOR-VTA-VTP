using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_HOJAPTOMED
    /// </summary>
    public interface IMeHojaptomedRepository
    {
        void Save(MeHojaptomedDTO entity, int empresa);
        void Update(MeHojaptomedDTO entity);
        void DeleteById(int hojaptomedcodi);
        void Delete(int formatcodi, int tipoinfocodi, int hojaptoorden, int ptomedicodi, int tptomedi);
        void DeleteHoja(int formatcodi, int tipoinfocodi, int hojaptoorden, int ptomedicodi, int tptomedi, int hoja);
        MeHojaptomedDTO GetById(int formatcodi, int tipoinfocodi, int ptomedicodi, int hojaptosigno, int tptomedicodi);
        MeHojaptomedDTO GetByIdHoja(int formatcodi, int tipoinfocodi, int ptomedicodi, int hojaptosigno, int tptomedicodi, int hojacodi);
        List<MeHojaptomedDTO> List();
        List<MeHojaptomedDTO> ListByFormatcodi(string formatcodi);
        List<MeHojaptomedDTO> GetByCriteria(int emprcodi, int formatcodi, DateTime fechaIni, DateTime fechaFin);
        List<MeHojaptomedDTO> GetByCriteria2(int emprcodi, int formatcodi, string query, DateTime fechaIni, DateTime fechaFin);
        List<SiEmpresaDTO> ObtenerEmpresasPorFormato(int idFormato);        
        List<MeHojaptomedDTO> GetPtoMedicionPR16(int emprcodi, int formatcodi, string periodo, string query);
        List<MeHojaptomedDTO> ObtenerPtosXFormato(int formatcodi, int emprcodi);
        List<MeHojaptomedDTO> GetPuntosFormato(int emprcodi, int formatcodi);
        List<MeHojaptomedDTO> GetByCriteria2(int emprcodi, int formatcodi, int hojacodi, string query, DateTime fechaPeriodo);
        List<MeHojaptomedDTO> ListPtosWithTipoGeneracion(int formatcodi, int tgenercodi);
        List<MeHojaptomedDTO> ListarHojaPtoByFormatoAndEmpresa(int emprcodi, string formatcodi);
        List<MeHojaptomedDTO> ListarHojaPtoByFormatoAndEmpresaHoja(int emprcodi, int formatcodi, int hojacodi);

        List<MeHojaptomedDTO> GetByCriteria3(int emprcodi, int formatcodi, int cuenca, string query);
    }
}
