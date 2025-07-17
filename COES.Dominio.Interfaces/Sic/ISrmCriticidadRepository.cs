using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SRM_CRITICIDAD
    /// </summary>
    public interface ISrmCriticidadRepository
    {
        int Save(SrmCriticidadDTO entity);
        void Update(SrmCriticidadDTO entity);
        void Delete(int srmcrtcodi);
        SrmCriticidadDTO GetById(int srmcrtcodi);
        List<SrmCriticidadDTO> List();
        List<SrmCriticidadDTO> GetByCriteria();
        int SaveSrmCriticidadId(SrmCriticidadDTO entity);
        List<SrmCriticidadDTO> BuscarOperaciones(DateTime srmcrtFeccreacion,DateTime srmcrtFecmodificacion,int nroPage, int pageSize);
        int ObtenerNroFilas(DateTime srmcrtFeccreacion,DateTime srmcrtFecmodificacion);
    }
}
