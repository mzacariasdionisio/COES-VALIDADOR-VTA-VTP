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
    /// Interface de acceso a datos de la tabla VTP_INGRESO_POTEFR_DETALLE
    /// </summary>
    public interface IVtpIngresoPotefrDetalleRepository
    {
        int Save(VtpIngresoPotefrDetalleDTO entity);
        void Update(VtpIngresoPotefrDetalleDTO entity);
        void Delete(int ipefrdcodi);
        VtpIngresoPotefrDetalleDTO GetById(int ipefrdcodi);
        List<VtpIngresoPotefrDetalleDTO> List();
        List<VtpIngresoPotefrDetalleDTO> GetByCriteria(int ipefrcodi,int pericodi,int recpotcodi);
        void DeleteByCriteria(int ipefrcodi, int pericodi,int recpotcodi);
        List<VtpIngresoPotefrDetalleDTO> GetByCriteriaSumCentral(int ipefrcodi, int pericodi, int recpotcodi);
        void DeleteByCriteriaVersion(int pericodi, int recpotcodi);
        List<VtpIngresoPotefrDetalleDTO> ObtenerPotenciaEFRSumPorEmpresa(string ipefrcodis, int periCodi, int recpotcodi);
        //SIOSEIN-PRIE-2021
        List<VtpIngresoPotefrDetalleDTO> GetByCriteriaSinPRIE(DateTime dFecha, int ipefrcodi, int pericodi, int recpotcodi);
        List<VtpIngresoPotefrDetalleDTO> GetCentralUnidadByEmpresa(int emprcodi);       // PrimasRER.2023

    }
}
