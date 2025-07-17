using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_RECALCULO
    /// </summary>
    public interface IRecalculoRepository
    {

        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int Save(RecalculoDTO entity);
        void Update(RecalculoDTO entity);
        void Delete(System.Int32 iPeriCodi, System.Int32 iRecaCodi);
        int GetUltimaVersion(int pericodi); 
        RecalculoDTO GetById(System.Int32 iPerCodi, System.Int32 iRecaCodi);
        List<RecalculoDTO> List(int id);
        List<RecalculoDTO> ListByAnioMes(int anio, int mes);    // PrimasRER.2023
        List<RecalculoDTO> ListVTEAByAnioMes(int anio, int mes);    // PrimasRER.2023
        List<RecalculoDTO> GetByCriteria(string nombre);
        List<RecalculoDTO> ListEstadoPublicarCerrado(int iPerCodi);
        List<RecalculoDTO> ListRecalculosTrnCodigoEnviado(int pericodi, int emprcodi);
        int ObtenerVersionDTR(int pericodi);
        //ASSETEC 202108 - TIEE
        List<RecalculoDTO> ListMaxRecalculoByPeriodo();
        string MigrarSaldosVTEA(int emprcodiorigen, int emprcodidestino, int pericodi, int recacodi);
        string MigrarCalculoVTEA(int emprcodiorigen, int emprcodidestino, int pericodi, int recacodi);
    }
}
