using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la vista VW_TRN_RATIO_CUMPLIMIENTO
    /// </summary>
    public interface IRatioCumplimientoRepository
    {
       List<RatioCumplimientoDTO> GetByCodigo(int? tipoemprcodi, int? pericodi,int version);
       List<RatioCumplimientoDTO> GetByCriteria(string nombre);
    }
}

