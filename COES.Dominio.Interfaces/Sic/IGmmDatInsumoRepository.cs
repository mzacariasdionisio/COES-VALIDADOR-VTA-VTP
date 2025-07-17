using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IGmmDatInsumoRepository
    {
        List<GmmDatInsumoDTO> ListarDatosInsumoTipoSC(int anio, string mes);
        void UpsertDatosInsumoTipoSC(GmmDatInsumoDTO entity);
        List<GmmDatInsumoDTO> ListarDatosEntregas(int anio, string mes);
        void UpsertDatosEntregas(GmmDatInsumoDTO entity);
        List<GmmDatInsumoDTO> ListarDatosInflexibilidad(int anio, string mes);
        void UpsertDatosInflexibilidad(GmmDatInsumoDTO entity);
        List<GmmDatInsumoDTO> ListarDatosRecaudacion(int anio, string mes);
        void UpsertDatosRecaudacion(GmmDatInsumoDTO entity);
        bool UpsertDatosCalculo(GmmDatInsumoDTO entity, string tipo);
        List<GmmDatInsumoDTO> ListadoInsumos(int anio, string mes);
        bool ActualizarUpsertDatosCalculo(GmmDatInsumoDTO entity, string tipo);
        bool EliminarUpsertDatosCalculo(GmmDatInsumoDTO entity, string tipo);
        List<GmmDatInsumoDTO> ListaDatosCalculo(GmmDatInsumoDTO entity, string tipo);
        bool ConsultaParticipanteExistente(int empgcodi);
        string ConsultaEstadoPeriodo(int anio, string mes);

    }
}
