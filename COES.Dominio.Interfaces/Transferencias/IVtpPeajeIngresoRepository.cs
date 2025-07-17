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
    /// Interface de acceso a datos de la tabla VTP_PEAJE_INGRESO
    /// </summary>
    public interface IVtpPeajeIngresoRepository
    {
        int Save(VtpPeajeIngresoDTO entity);
        void Update(VtpPeajeIngresoDTO entity);
        void Delete(int pericodi, int recpotcodi, int pingcodi);
        VtpPeajeIngresoDTO GetById(int pericodi, int recpotcodi, int pingcodi);
        List<VtpPeajeIngresoDTO> List();
        List<VtpPeajeIngresoDTO> GetByCriteria();
        List<VtpPeajeIngresoDTO> ListView(int pericodi, int recpotcodi);
        VtpPeajeIngresoDTO GetByIdView(int pericodi, int recpotcodi, int pingcodi);
        void UpdateDesarrollo(VtpPeajeIngresoDTO entity);
        VtpPeajeIngresoDTO GetByNombreIngresoTarifario(int pericodi, int recpotcodi, string pingnombre);
        List<VtpPeajeIngresoDTO> ListPagoSi(int pericodi, int recpotcodi);
        List<VtpPeajeIngresoDTO> ListCargo(int pericodi, int recpotcodi);
        List<VtpPeajeIngresoDTO> ListTransmisionSi(int pericodi, int recpotcodi);
        List<VtpPeajeIngresoDTO> ListIngresoTarifarioMensual(int pericodi, int recpotcodi);
        List<VtpPeajeIngresoDTO> ListCargoPrimaRER(int emprcodi); //PrimasRER.2023
        void DeleteByCriteria(int pericodi, int recpotcodi);
        List<VtpPeajeIngresoDTO> GetByEmpresaGeneradora(int pericodi, int recpotcodi, int emprcodi);
    }
}
