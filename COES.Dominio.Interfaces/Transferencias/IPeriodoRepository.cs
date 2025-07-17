using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_PERIODO
    /// </summary>
    public interface IPeriodoRepository
    {
        int Save(PeriodoDTO entity);
        void Update(PeriodoDTO entity);
        void Delete(System.Int32 id);
        PeriodoDTO GetById(System.Int32? id);
        List<PeriodoDTO> List();
        List<PeriodoDTO> ListPeriodoPotencia();
        List<PeriodoDTO> GetByCriteria(string nombre);
        PeriodoDTO GetByAnioMes(int iPeriAnioMes);
        int GetNumRegistros(System.Int32 id);
        int GetFirstPeriodoFormatNew();
        List<PeriodoDTO> ListarByEstado(string sPeriEstado);
        List<PeriodoDTO> ListarByEstadoPublicarCerrado();
        PeriodoDTO BuscarPeriodoAnterior(int iPeriCodi);
        List<PeriodoDTO> ListarPeriodosFuturos(int iPeriCodi);
        PeriodoDTO ObtenerPeriodoDTR(int anio, int mes);

        // Inicio de Agregados - Sistema de Compensaciones
        List<PeriodoDTO> ListarPeriodosTC();
        PeriodoDTO GetPeriodoByIdProcesa(int id);
        List<PeriodoDTO> ListarPeriodosCompensacion();
        // Fin de Agregados - Sistema de Compensaciones

        //2018.Setiembre - Agregados por ASSETEC
        int GetPKTrnContador(string trncnttabla, string trncntcolumna);
        void UpdatePKTrnContador(string trncnttabla, string trncntcolumna, Int32 trncntcontador);
    }
}
