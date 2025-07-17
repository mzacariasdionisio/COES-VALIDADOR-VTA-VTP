using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.ReportesFrecuencia
{
    public interface IExtraerFrecuenciaRepository
    {
        ExtraerFrecuenciaDTO GetById(int IdCarga);
        List<ExtraerFrecuenciaDTO> GetListaExtraerFrecuencia(string FechaInicial, string FechaFinal);
        List<LecturaVirtualDTO> GetListaMilisegundos(int IdCarga);
        ExtraerFrecuenciaDTO SaveUpdate(ExtraerFrecuenciaDTO entidad);
    }
}
