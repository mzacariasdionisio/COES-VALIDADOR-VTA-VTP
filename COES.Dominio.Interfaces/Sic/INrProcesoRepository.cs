using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla NR_PROCESO
    /// </summary>
    public interface INrProcesoRepository
    {
        int Save(NrProcesoDTO entity);
        void Update(NrProcesoDTO entity);
        void Delete(int nrprccodi);
        void Delete(int nrpercodi, int nrcptcodi);
        NrProcesoDTO GetById(int nrprccodi);
        List<NrProcesoDTO> List();
        string ListObservaciones(int nrperCodi);
        List<NrProcesoDTO> GetByCriteria();
        int SaveNrProcesoId(NrProcesoDTO entity);
        List<NrProcesoDTO> BuscarOperaciones(string estado,int nrperCodi,int grupoCodi,int nrcptCodi,DateTime nrprcFechaInicio,DateTime nrprcFechaFin,int nroPage, int PageSize);
        int ObtenerNroFilas(string estado, int nrperCodi, int grupoCodi, int nrcptCodi, DateTime nrprcFechaInicio, DateTime nrprcFechaFin);
        List<ReservaDTO> ObtenerReservaDiariaEjecutada(DateTime dtFecha);
    }
}
