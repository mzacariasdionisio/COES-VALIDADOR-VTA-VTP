using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCE_COMP_REGULAR_DET
    /// </summary>
    public interface IVceCompRegularDetRepository
    {
        void Save(VceCompRegularDetDTO entity);
        void Update(VceCompRegularDetDTO entity);
        void Delete(DateTime crdethora, int grupocodi, int pecacodi);
        VceCompRegularDetDTO GetById(DateTime crdethora, int grupocodi, int pecacodi);
        List<VceCompRegularDetDTO> List();
        List<VceCompRegularDetDTO> GetByCriteria();

        //NETC

        List<VceCompRegularDetDTO> ListCompensacionesEspeciales(int pecacodi, string empresa, string central, string grupo, string modo);
        List<VceCompRegularDetDTO> ListCompensacionesMME(int pecacodi, string empresa, string central, string grupo, string modo);
        void ProcesarCompensacionEspecial(int pecacodi);
        void ProcesarCompensacionMME(int pecacodi);
        void DeleteCompensacionManual(int pecacodi, int grupocodi, DateTime crdethora);
        void SaveEntity(VceCompRegularDetDTO entity);
        void LlenarMedeneriaGrupo(int pecacodi);
        
        // DSH 25-04-2017, 06-052017 : se agrego por requerimiento
        void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen);
        void DeleteByGroup(int pecacodi, int grupocodi, int subcausacodi, DateTime crcbehorini, DateTime crcbehorfin);
        void DeleteByVersion(int pecacodi);
        void DeleteByVersionTipoCalculoAutomatico(int pecacodi);
    }
}
