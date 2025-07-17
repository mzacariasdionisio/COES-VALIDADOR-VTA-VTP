using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCE_COMP_BAJAEFIC
    /// </summary>
    public interface IVceCompBajaeficRepository
    {
        void Save(VceCompBajaeficDTO entity);
        void Update(VceCompBajaeficDTO entity);
        void Delete(DateTime crcbehorfin, DateTime crcbehorini, int subcausacodi, int grupocodi, int pecacodi);
        VceCompBajaeficDTO GetById(DateTime crcbehorfin, DateTime crcbehorini, int subcausacodi, int grupocodi, int pecacodi);
        List<VceCompBajaeficDTO> List();
        List<VceCompBajaeficDTO> GetByCriteria();

        //NETC

        List<VceCompBajaeficDTO> ListCompensacionesRegulares(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fechaini, string fechafin, string tipocalculo);
        void ProcesarCompensacionRegular(int pecacodi);
        void DeleteCompensacionManual(int pecacodi);
        void DeleteByVersion(int pecacodi);
        void DeleteByVersionTipoCalculoAutomatico(int pecacodi);
        void SaveByEntity(VceCompBajaeficDTO entity);

        // DSH 06-05-2017 : Se agrego por requerimiento
        void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen);

        // DSH 31-05-2017 : Se cambio por requerimiento
        List<VceCompBajaeficDTO> ListCompensacionOperacionInflexibilidad(int pecacodi);
        List<VceCompBajaeficDTO> ListCompensacionOperacionSeguridad(int pecacodi);
        List<VceCompBajaeficDTO> ListCompensacionOperacionRSF(int pecacodi);
        List<VceCompBajaeficDTO> ListCompensacionRegulacionTension(int pecacodi);

        IDataReader ListCompensacionOperacionMME(int pecacodi, List<EveSubcausaeventoDTO> subCausaEvento);

        IDataReader ListCompensacionDiarioMME(int pecacodi, List<PrGrupoDTO> listaGrupo);

    }
}
