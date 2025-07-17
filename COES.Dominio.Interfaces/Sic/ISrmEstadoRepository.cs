using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SRM_ESTADO
    /// </summary>
    public interface ISrmEstadoRepository
    {
        int Save(SrmEstadoDTO entity);
        void Update(SrmEstadoDTO entity);
        void Delete(int srmstdcodi);
        SrmEstadoDTO GetById(int srmstdcodi);
        List<SrmEstadoDTO> List();
        List<SrmEstadoDTO> GetByCriteria();
        int SaveSrmEstadoId(SrmEstadoDTO entity);
        List<SrmEstadoDTO> BuscarOperaciones(DateTime srmstdFeccreacion,DateTime srmstdFecmodificacion,int nroPage, int pageSize);
        int ObtenerNroFilas(DateTime srmstdFeccreacion,DateTime srmstdFecmodificacion);
    }
}
