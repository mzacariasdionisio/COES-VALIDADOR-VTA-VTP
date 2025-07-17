using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamPeriodoRepository
    {

        List<PeriodoDTO> GetPeriodos();

        List<PeriodoDTO> GetPeriodosByAnioAndEstado(int anio, string estado);
        List<PeriodoDTO> GetPeriodosByAnio(int anio);

        bool SavePeriodo(PeriodoDTO periodoDTO);

        bool DeletePeriodoById(int id, string usuario);

        int GetPeriodoId();

        PeriodoDTO GetPeriodoById(int id);

        bool UpdatePeriodo(PeriodoDTO periodoDTO);

        int GetPeriodoByDate(DateTime perifechaini, DateTime perifechafin, int id);
    }
}
