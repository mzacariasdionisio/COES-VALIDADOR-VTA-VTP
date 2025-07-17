using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla NR_POTENCIACONSIGNA
    /// </summary>
    public interface INrPotenciaconsignaRepository
    {
        int Save(NrPotenciaconsignaDTO entity);
        void Update(NrPotenciaconsignaDTO entity);
        void Delete(int nrpccodi);
        NrPotenciaconsignaDTO GetById(int nrpccodi);
        List<NrPotenciaconsignaDTO> List();
        List<NrPotenciaconsignaDTO> GetByCriteria();
        int SaveNrPotenciaconsignaId(NrPotenciaconsignaDTO entity);
        List<NrPotenciaconsignaDTO> BuscarOperaciones(int nrsmodCodi, int grupoCodi, DateTime nrpcFechaIni, DateTime nrpcFechaFin,string estado, int nroPage, int PageSize);
        int ObtenerNroFilas(int nrsmodCodi, int grupoCodi, DateTime nrpcFechaIni, DateTime nrpcFechaFin, string estado);
    }
}
