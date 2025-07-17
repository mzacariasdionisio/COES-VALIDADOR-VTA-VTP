using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_PALEATORIA
    /// </summary>
    public interface IEvePaleatoriaRepository
    {
        void Save(EvePaleatoriaDTO entity);
        void Update(EvePaleatoriaDTO entity);
        void Delete(DateTime pafecha);
        EvePaleatoriaDTO GetById(DateTime pafecha);
        List<EvePaleatoriaDTO> List();
        List<EvePaleatoriaDTO> GetByCriteria();
        List<SiPersonaDTO> ListProgramador();
        int ObtenerNroFilas(DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize);
        List<EvePaleatoriaDTO> BuscarOperaciones(DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize);
        string ProgramadorPrueba(DateTime Fecha);
        List<string> ListaCoordinadores();

    }
}
