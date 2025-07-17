using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla NR_POTENCIACONSIGNA_DETALLE
    /// </summary>
    public interface INrPotenciaconsignaDetalleRepository
    {
        int Save(NrPotenciaconsignaDetalleDTO entity);
        void Update(NrPotenciaconsignaDetalleDTO entity);
        void Delete(int nrpcdcodi);
        void DeleteTotal(int nrpccodi);
        NrPotenciaconsignaDetalleDTO GetById(int nrpcdcodi);
        List<NrPotenciaconsignaDetalleDTO> List();
        List<NrPotenciaconsignaDetalleDTO> GetByCriteria();
        int SaveNrPotenciaconsignaDetalleId(NrPotenciaconsignaDetalleDTO entity);
        List<NrPotenciaconsignaDetalleDTO> BuscarOperaciones(int nrpcCodi,DateTime nrpcdFecha,DateTime nrpcdFecCreacion,int nroPage, int PageSize);
        int ObtenerNroFilas(int nrpcCodi,DateTime nrpcdFecha,DateTime nrpcdFecCreacion);
    }
}
