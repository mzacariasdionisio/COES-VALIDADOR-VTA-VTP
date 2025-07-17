using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.ReportesFrecuencia
{
    public interface IEtapaERARepository
    {
        EtapaERADTO GetById(int EtapaCodi);
        List<EtapaERADTO> GetListaEtapas();
        int GetUltimoCodigoGenerado();
        int ValidarNombreEtapa(EtapaERADTO entity);
        EtapaERADTO SaveUpdate(EtapaERADTO entity);
        EtapaERADTO Eliminar(EtapaERADTO entity);
    }
}
