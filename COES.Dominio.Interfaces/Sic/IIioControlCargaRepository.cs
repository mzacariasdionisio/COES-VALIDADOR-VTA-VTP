using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IIO_CONTROL_CARGA
    /// </summary>
    public interface IIioControlCargaRepository
    {
        void Update(IioControlCargaDTO scoControlCargaDto);
        int Save(IioControlCargaDTO scoControlCargaDto);
        IioControlCargaDTO GetByCriteria(IioControlCargaDTO scoControlCargaDto);
        List<IioControlCargaDTO> GetByPeriodo(int PseinCodi);

        IioControlCargaDTO GetById(int rccaCodi);
        List<IioControlCargaDTO> GetByCriteriaXTabla(int pseinCodi, string rtabcodi);
    }
}