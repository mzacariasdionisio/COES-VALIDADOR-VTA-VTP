using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_ELEMENTO
    /// </summary>
    public interface IAudElementoRepository
    {
        int Save(AudElementoDTO entity);
        void Update(AudElementoDTO entity);
        void Delete(AudElementoDTO elemento);
        AudElementoDTO GetById(int elemcodi);
        List<AudElementoDTO> List();
        List<AudElementoDTO> GetByCriteria(AudElementoDTO audElementoDTO);
        List<SiAreaDTO> GetByAreaElemento();
        List<AudProcesoDTO> GetByProcesoElemento();
        List<AudElementoDTO> GetByProcesoPorElemento(AudElementoDTO audElementoDTO);
        List<AudElementoDTO> GetByElementosPorProceso(int proccodi);
        List<AudElementoDTO> GetByElementosPorProcesoAP(int plancodi, string procesos);
        List<AudElementoDTO> GetByElementosPorTipo(int tipoelemento);
        List<AudElementoDTO> GetByElementosPorProcesoTipo(string procesos, int tipoelemento);
        int ObtenerNroRegistrosBusqueda(AudElementoDTO audElementoDTO);
        string GetByElementoValidacion(int elemcodi);
    }
}
