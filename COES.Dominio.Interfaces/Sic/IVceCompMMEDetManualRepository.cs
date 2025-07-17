using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCE_COMP_MME_DET_MANUAL
    /// </summary>
    public interface IVceCompMMEDetManualRepository
    {

        void DeleteCompensacionManual(int pecacodi, int grupocodi, DateTime cmmedmhora);

        void DeleteCompensacionManualByVersion(int pecacodi);

        void SaveEntity(VceCompMMEDetManualDetDTO entity);

        void UpdateCompensacionDet(int pecacodi);

        void SaveCompensacionDet(int pecacodi);

        List<VceCompMMEDetManualDetDTO> ListCompensacionesManuales(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fechaini, string fechafin, string tipocalculo);
    }
}
