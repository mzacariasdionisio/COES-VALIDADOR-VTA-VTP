using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_PTORELACION
    /// </summary>
    public interface IMePtorelacionRepository
    {
        int Save(MePtorelacionDTO entity);
        void Update(MePtorelacionDTO entity);
        void Delete(int equicodi);
        MePtorelacionDTO GetById(int ptorelcodi);
        List<MePtorelacionDTO> List(int idCentral);
        List<MePtorelacionDTO> GetByCriteria(int? idEmpresa, int? idCentral);
        List<SiEmpresaDTO> ObtenerEmpresas();
        List<EqEquipoDTO> ListarCentrales(int idEmpresa, DateTime fechaPeriodo);
        List<MePtorelacionDTO> ObtenerPuntosRelacion(int? idEmpresa, int? idCentral, DateTime fechaPeriodo);
        MeMedicion48DTO ObtenerDatosDespacho(DateTime fecha, string ptomedicodi);
        List<MePtorelacionDTO> ObtenerPuntosRPF(DateTime fechaPeriodo);
    }
}
