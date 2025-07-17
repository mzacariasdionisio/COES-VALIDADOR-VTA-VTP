using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// Interface de acceso a datos de la tabla Vce_Periodo_Calculo
    public interface IVcePeriodoCalculoRepository
    {
        int Save(VcePeriodoCalculoDTO entity);
        void Update(VcePeriodoCalculoDTO entity);
        void Delete(System.Int32 id);
        VcePeriodoCalculoDTO GetById(System.Int32? id);
        List<VcePeriodoCalculoDTO> List();
        List<VcePeriodoCalculoDTO> GetByCriteria(string nombre);
        List<VcePeriodoCalculoDTO> GetByAnioMes(int iPeriAnioMes);
        List<VcePeriodoCalculoDTO> GetByIdPeriodo(int iPeriCodi);

        Int32 ObtenerNroCalculosActivosPeriodo(int pericodi);
        Int32 GetIdAnteriorCalculo(int pecacodi);
        Int32 GetIdAnteriorConfig(int pecacodi);

        //cambio
        VcePeriodoCalculoDTO GetPeriodo(int pecacodi);
        Int32 GetPeriodoMaximo(int pericodi);
        int UpdateCompensacionInforme(int pericodi, string nombreinforme);
        //int GetNumRegistros(System.Int32 id);

        // List<VcePeriodoCalculoDTO> ListarByEstado(string sPeriEstado);
        // List<VcePeriodoCalculoDTO> ListarByEstadoPublicarCerrado();
        // VcePeriodoCalculoDTO BuscarPeriodoAnterior(int iPeriCodi);
        // List<VcePeriodoCalculoDTO> ListarPeriodosFuturos(int iPeriCodi);
        //  VcePeriodoCalculoDTO ObtenerPeriodoDTR(int anio, int mes);

    }
}
