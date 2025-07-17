using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_PRUEBAUNIDAD
    /// </summary>
    public interface IEvePruebaunidadRepository
    {
        int Save(EvePruebaunidadDTO entity);
        void Update(EvePruebaunidadDTO entity);
        void Delete(int prundcodi);
        EvePruebaunidadDTO GetById(int prundcodi);
        List<EvePruebaunidadDTO> List();
        List<EvePruebaunidadDTO> GetByCriteria(DateTime prundFechaIni, DateTime prundFechaFin);
        int SaveEvePruebaunidadId(EvePruebaunidadDTO entity);
        List<EvePruebaunidadDTO> BuscarOperaciones(string estado, DateTime prundFechaIni, DateTime prundFechaFin, int nroPage, int pageSize);
        int ObtenerNroFilas(string estado, DateTime prundFechaIni, DateTime prundFechaFin);
        EqEquipoDTO ObtenerUnidadSorteada(DateTime prundFecha);
        List<EqEquipoDTO> ObtenerUnidadSorteadaHabilitada(DateTime prundFecha);
    }
}
