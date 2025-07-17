using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCE_ENERGIA
    /// </summary>
    public interface IVceEnergiaRepository
    {
        void Save(VceEnergiaDTO entity);
        void Update(VceEnergiaDTO entity);
        void Delete(DateTime crmefecha, int ptomedicodi, int pecacodi);
        VceEnergiaDTO GetById(DateTime crmefecha, int ptomedicodi, int pecacodi);
        List<VceEnergiaDTO> List();
        List<VceEnergiaDTO> GetByCriteria();
        void SaveFromMeMedicion96(int pecacodi, string fechaini, string fechafin);
        void DeletexFecha(string fechaini, string fechafin);
        List<VceEnergiaDTO> ListByCriteria(int registros, int pecacodi);

        // DSH 06-05-2017 : Se agrego por requerimiento
        void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen);

        //DSH 05-05-2017 : Se agrego por requerimiento
        void DeleteByVersion(int pecacodi);
    }
}
