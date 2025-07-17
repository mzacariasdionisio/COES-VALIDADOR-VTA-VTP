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
    /// Interface de acceso a datos de la tabla VTP_INGRESO_POTENCIA
    /// </summary>
    public interface IVtpIngresoPotenciaRepository
    {
        int Save(VtpIngresoPotenciaDTO entity);
        void Update(VtpIngresoPotenciaDTO entity);
        void Delete(int potipcodi);
        VtpIngresoPotenciaDTO GetById(int potipcodi);
        List<VtpIngresoPotenciaDTO> List();
        List<VtpIngresoPotenciaDTO> GetByCriteria();
        void DeleteByCriteria(int pericodi, int recpotcodi);
        List<VtpIngresoPotenciaDTO> ListEmpresa(int pericodi, int recpotcodi);
    }
}
