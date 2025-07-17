using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoDatos96Repository
    {
        void BulkInsert(List<DpoDatos96DTO> entitys, string nombreTabla);
        void DeleteBetweenDates(string inicio, string fin, string subestaciones);
        List<DpoDatos96DTO> ListBetweenDates(string inicio, string fin, string subestaciones);
        List<DpoDatos96DTO> ListMedidorDemandaSirpit(string carga, string inicio, string fin, int tipo);
        List<DpoDatos96DTO> ListGroupByMonthYear(string anio, string mes, string cargas, string tipo);
        List<DpoDatos96DTO> ListDatosSIRPIT(int anio, string mes, string cargas, string tipo);
        List<DpoDatos96DTO> ListAllBetweenDates(string fechainicio, string fechafin, int subestacion, int transformador, int barra);
        List<DpoDatos96DTO> ListSirpitByDateRange(string codigo, string inicio, string fin, string tipo);
        List<DpoDatos96DTO> ObtenerDemandaSirpit(string dpotnfcodi, string dpodatfecha);
        List<DpoDatos96DTO> ListTrafoBarraInfo(string fechainicio, string fechafin);
    }
}
