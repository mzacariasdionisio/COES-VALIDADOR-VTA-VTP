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
    /// Interface de acceso a datos de la tabla VTP_INGRESO_POTUNID_INTERVL
    /// </summary>
    public interface IVtpIngresoPotUnidIntervlRepository
    {
        int Save(VtpIngresoPotUnidIntervlDTO entity);
        void Update(VtpIngresoPotUnidIntervlDTO entity);
        void Delete(int inpuincodi);
        VtpIngresoPotUnidIntervlDTO GetById(int inpuincodi);
        List<VtpIngresoPotUnidIntervlDTO> List();
        List<VtpIngresoPotUnidIntervlDTO> GetByCriteria(int pericodi, int recpotcodi, int emprcodi, int equicodi, int ipefrcodi);
        List<VtpIngresoPotUnidIntervlDTO> ListSumIntervl(int pericodi, int recpotcodi);
        void DeleteByCriteria(int pericodi, int recpotcodi);
        List<VtpIngresoPotUnidIntervlDTO> ListSumIntervlEmpresa(int pericodi, int recpotcodi, int emprcodi, int ipefrcodi);
    }
}
