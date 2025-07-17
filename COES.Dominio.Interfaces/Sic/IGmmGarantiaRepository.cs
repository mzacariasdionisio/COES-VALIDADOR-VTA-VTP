using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IGmmGarantiaRepository
    {
        int Save(GmmGarantiaDTO entity);
        int Update(GmmGarantiaDTO entity);
        bool Delete(GmmGarantiaDTO entity);
        GmmGarantiaDTO GetByEmpgcodi(int empgcodi);
        GmmGarantiaDTO mensajeProcesamiento(int anio, string mes);
        List<GmmGarantiaDTO> mensajeProcesamientoParticipante(int anio, string mes);
    }
}
