using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCE_HORA_OPERACION
    /// </summary>
    public interface IVceHoraOperacionRepository
    {
        void Save(VceHoraOperacionDTO entity);
        void Update(VceHoraOperacionDTO entity);
        // DSH 08-10-2017 : Se agrego por requerimiento
        void UpdateRangoHora(VceHoraOperacionDTO entity);
        void Delete(int hopcodi, int pecacodi);
        VceHoraOperacionDTO GetById(int hopcodi, int pecacodi);
        List<VceHoraOperacionDTO> List();
        List<VceHoraOperacionDTO> GetByCriteria();

        //NETC
        void SaveByRango(int pecacodi, string fechaini, string fechafin);
        void DeleteById(int pecacodi);
        List<VceHoraOperacionDTO> ListFiltro(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fecIni, string fecFin, string arranque, string parada);
        List<VceHoraOperacionDTO> ListById(int pecacodi);

        // DSH 06-05-2017 : Se agrego por requerimiento
        void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen);
        // DSH 08-10-2017 : Se agrego por requerimiento
        List<VceHoraOperacionDTO> ListVerificarHoras(int pecacodi);
        VceHoraOperacionDTO GetDataById(int hopcodi, int pecacodi);
    }
}
