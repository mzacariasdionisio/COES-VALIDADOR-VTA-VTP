using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IExportExcelRepository
    {
        List<ExportExcelDTO> GetByPeriVer(int pericodi,int version);
        List<ExportExcelDTO> GetByEmprPeriVersion(int emprcodi,int pericodi, int version);
        List<EmpresaPagoDTO> GetMatrizByPeriVersion(int emprcodi, int pericodi, int version);
        List<EmpresaPagoDTO> GetMatrizEmprByPeriVersion(int pericodi, int version);
    }
}

