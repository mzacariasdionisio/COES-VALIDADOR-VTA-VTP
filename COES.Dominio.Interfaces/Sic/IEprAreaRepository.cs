using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPR_PROYECTO_ACTEQP
    /// </summary>
    public interface IEprAreaRepository
    {
        int Save(EprAreaDTO entity);
        void Update(EprAreaDTO entity);
        void Delete_UpdateAuditoria(EprAreaDTO entity);
        EprAreaDTO GetById(int Epareacodi);
        List<EprAreaDTO> ListSubEstacion();
        EprAreaDTO GetCantidadUbicacionSGOCOESEliminar(int epareacodi);
        List<EprAreaDTO> ListAreaxCelda(string celda1, string celda2);

    }
}
