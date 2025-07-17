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
    /// Interface de acceso a datos de la tabla VTP_INGRESO_POTUNID_PROMD
    /// </summary>
    public interface IVtpIngresoPotUnidPromdRepository
    {
        int Save(VtpIngresoPotUnidPromdDTO entity);
        void Update(VtpIngresoPotUnidPromdDTO entity);
        void Delete(int inpuprcodi);
        VtpIngresoPotUnidPromdDTO GetById(int inpuprcodi);
        List<VtpIngresoPotUnidPromdDTO> List();
        List<VtpIngresoPotUnidPromdDTO> GetByCriteria();
        void DeleteByCriteria(int pericodi, int recpotcodi);
        List<VtpIngresoPotUnidPromdDTO> ListEmpresa(int pericodi, int recpotcodi);
        List<VtpIngresoPotUnidPromdDTO> ListEmpresaCentral(int pericodi, int recpotcodi);
        List<VtpIngresoPotUnidPromdDTO> GetIngresoPotUnidPromdByComparative(int pericodiini, int pericodifin, int emprcodi, int equicodi);
        List<VtpIngresoPotUnidPromdDTO> GetIngresoPotUnidPromdByComparativeUnique(int pericodiini, int pericodifin, int emprcodi, int equicodi);

        List<VtpIngresoPotUnidPromdDTO> GetIngresoPotUnidPromdByCompHist(int pericodiini, int recpotcodiini, int pericodifin, int recpotcodifin, int emprcodi);
        List<VtpIngresoPotUnidPromdDTO> GetIngresoPotUnidPromdByCompHistUnique(int pericodiini, int recpotcodiini, int pericodifin, int recpotcodifin, int emprcodi);
        List<VtpIngresoPotUnidPromdDTO> ListEmpresaCentral2(int pericodi, int recpotcodi);
        //CU21
        VtpIngresoPotUnidPromdDTO GetByCentral(int pericodi, int recpotcodi, int equicodi);
        VtpIngresoPotUnidPromdDTO GetByCentralSumUnidades(int pericodi, int recpotcodi, int equicodi);
    }
}
