using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCE_PTOMED_MODOPE
    /// </summary>
    public interface IVcePtomedModopeRepository
    {
        void Save(VcePtomedModopeDTO entity);
        void Update(VcePtomedModopeDTO entity);
        void Delete(int grupocodi, int ptomedicodi);
        VcePtomedModopeDTO GetById(int grupocodi, int ptomedicodi);
        List<VcePtomedModopeDTO> List();
        List<VcePtomedModopeDTO> GetByCriteria();

        //NETC

        List<VcePtomedModopeDTO> ListById(int pecacodi, int ptomedicodi);
        int SaveByEntity(VcePtomedModopeDTO entity);
        void DeleteByEntity(int pecacodi, int ptomedicodi, int grupocodi);
        int Validar(int pecacodi, int ptomedicodi, int grupocodi);
        // DSH 05-05-2017 : Se agrego por requerimiento
        void DeleteByVersion(int pecacodi);
        
        // DSH 06-05-2017 : Se agrego por requerimiento
        void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen, string usuCreacion);
    }
}
