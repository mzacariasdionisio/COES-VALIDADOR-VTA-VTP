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
    /// Interface de acceso a datos de la tabla VTP_PEAJE_EGRESO
    /// </summary>
    public interface IVtpPeajeEgresoRepository
    {
        int Save(VtpPeajeEgresoDTO entity);
        void Update(VtpPeajeEgresoDTO entity);
        void Delete(int pegrcodi);
        VtpPeajeEgresoDTO GetById(int pegrcodi);
        List<VtpPeajeEgresoDTO> List(int pericodi, int recpotcodi);
        VtpPeajeEgresoDTO GetByCriteria(int emprcodi, int pericodi, int recpotcodi);
        void UpdateByCriteria(int emprcodi, int pericodi, int recpotcodi);
        List<VtpPeajeEgresoDTO> ListView(int emprcodi, int pericodi, int recpotcodi);
        List<VtpPeajeEgresoDTO> ObtenerReporteEnvioPorEmpresa(int pericodi, int recpotcodi);
        void DeleteByCriteria(int pericodi, int recpotcodi);
        List<VtpPeajeEgresoDTO> ListConsulta(int pericodi, int recpotcodi, int emprcodi, string plazo, string liquidacion);
        VtpPeajeEgresoDTO GetPreviusPeriod(int pericodi, int emprcodi);
    }
}
