using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    public class CDespachoDTO : EntityBase
    {
        public List<EveSubcausaeventoDTO> ListaEveSubcausaevento = new List<EveSubcausaeventoDTO>();
        public List<SiEmpresaDTO> ListaSiEmpresa = new List<SiEmpresaDTO>();
        public List<EqEquipoDTO> ListaCentrales = new List<EqEquipoDTO>();
        public List<PrGrupoDTO> ListaPrgrupo = new List<PrGrupoDTO>();
        public List<PrGrupoDTO> ListaPrgrupoFunc = new List<PrGrupoDTO>();
        public List<MeMedicion48DTO> ListaFechas = new List<MeMedicion48DTO>();
        public List<MePtomedicionDTO> ListaPtoMedicion = new List<MePtomedicionDTO>();
        public List<PrGrupoDTO> ListaPrgrupoFuncD = new List<PrGrupoDTO>();
    }
}
