using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VTP_RECALCULO_POTENCIA
    /// </summary>
    public interface IVtpRecalculoPotenciaRepository
    {
        int Save(VtpRecalculoPotenciaDTO entity);
        void Update(VtpRecalculoPotenciaDTO entity);
        void Delete(int pericodi, int recpotcodi);
        VtpRecalculoPotenciaDTO GetById(int pericodi, int recpotcodi);
        List<VtpRecalculoPotenciaDTO> List();
        List<VtpRecalculoPotenciaDTO> GetByCriteria();
        List<VtpRecalculoPotenciaDTO> ListView();
        List<VtpRecalculoPotenciaDTO> ListVTP(int anio, int mes); // PrimasRER.2023
        VtpRecalculoPotenciaDTO GetByIdView(int pericodi, int recpotcodi);
        List<VtpRecalculoPotenciaDTO> ListByPericodi(int pericodi);
        int GetByMaxIdRecPotCodi(int pericodi);
        //ASSETEC 202108 - TIEE
        List<VtpRecalculoPotenciaDTO> ListMaxRecalculoByPeriodo();
        string MigrarSaldosVTP(int emprcodiorigen, int emprcodidestino, int pericodi, int recpotcodi);
        string MigrarCalculoVTP(int emprcodiorigen, int emprcodidestino, int pericodi, int recpotcodi);
        VtpRecalculoPotenciaDTO GetByIdCerrado(int pericodi, int recpotcodi);
        //ASSETEC CPA - CU05
        List<VtpRecalculoPotenciaDTO> ListRecalculoByPeriodo(int anio);
    }
}
