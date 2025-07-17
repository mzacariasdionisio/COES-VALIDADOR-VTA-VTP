using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnAsociacionRepository
    {
        List<PrnAsociacionDTO> List();
        PrnAsociacionDTO GetById(int codigo);
        void Delete(int codigo);
        void Save(PrnAsociacionDTO entity);
        void Update(PrnAsociacionDTO entity);
        List<PrnAsociacionDTO> ListUnidadAgrupadaByTipo(string tipo);
        List<PrnAsociacionDTO> ListUnidadByAgrupacion(int codigo);
        void DeleteAsociacionDetalleByTipo(string tipo);
        void DeleteAsociacionByTipo(string tipo);
        List<PrnAsociacionDTO> ListAsociacionDetalleByTipo(string tipo);
        int SaveReturnId(PrnAsociacionDTO entity);
        void SaveDetalle(PrnAsociacionDTO entity);
    }
}
