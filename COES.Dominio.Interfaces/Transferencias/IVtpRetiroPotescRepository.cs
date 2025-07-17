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
    /// Interface de acceso a datos de la tabla VTP_RETIRO_POTESC
    /// </summary>
    public interface IVtpRetiroPotescRepository
    {
        int Save(VtpRetiroPotescDTO entity);
        void Update(VtpRetiroPotescDTO entity);
        void Delete(int pericodi, int recpotcodi);
        VtpRetiroPotescDTO GetById(int rpsccodi);
        List<VtpRetiroPotescDTO> List();
        List<VtpRetiroPotescDTO> GetByCriteria();
        List<VtpRetiroPotescDTO> ListView(int pericodi, int recpotcodi);
        List<VtpRetiroPotescDTO> ListByEmpresa(int pericodi, int recpotcodi);
    }
}
