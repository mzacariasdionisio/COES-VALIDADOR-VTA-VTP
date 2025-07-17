using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla NR_SOBRECOSTO
    /// </summary>
    public interface INrSobrecostoRepository
    {
        int Save(NrSobrecostoDTO entity);
        void Update(NrSobrecostoDTO entity);
        void Delete(int nrsccodi);
        NrSobrecostoDTO GetById(int nrsccodi);
        List<NrSobrecostoDTO> List();
        List<NrSobrecostoDTO> GetByCriteria();
        int SaveNrSobrecostoId(NrSobrecostoDTO entity);
        //List<NrSobrecostoDTO> BuscarOperaciones(int grupoCodi, DateTime nrscFechaIni, DateTime nrscFechaFin, string estado, int nroPage, int PageSize);
        List<NrSobrecostoDTO> BuscarOperaciones(DateTime nrscFechaIni, DateTime nrscFechaFin, string estado, int nroPage, int PageSize);
        //int ObtenerNroFilas(int grupoCodi, DateTime nrscFechaIni, DateTime nrscFechaFin, string estado);
        int ObtenerNroFilas(DateTime nrscFechaIni, DateTime nrscFechaFin, string estado);
    }
}
