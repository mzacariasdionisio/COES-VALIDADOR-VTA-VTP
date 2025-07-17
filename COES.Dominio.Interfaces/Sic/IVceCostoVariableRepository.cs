using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCE_COSTO_VARIABLE
    /// </summary>
    public interface IVceCostoVariableRepository
    {
        void Save(VceCostoVariableDTO entity);
        void Update(VceCostoVariableDTO entity);
        void Delete(DateTime crcvfecmod, int grupocodi, int pecacodi);
        VceCostoVariableDTO GetById(DateTime crcvfecmod, int grupocodi, int pecacodi);
        List<VceCostoVariableDTO> List();
        List<VceCostoVariableDTO> GetByCriteria();

        //NETC

        List<VceCostoVariableDTO> ListCostoVariable(int pecacodi);
        void LlenarCostoVariable(int pecacodi);
        
        //DSH 05-05-2017,06-05-2017 : Se agrego por requerimiento
        void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen);
        void DeleteByVersion(int pecacodi);
    }
}
