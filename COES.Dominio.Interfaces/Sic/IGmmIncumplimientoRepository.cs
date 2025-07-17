using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IGmmIncumplimientoRepository
    {
        int Save(GmmIncumplimientoDTO entity);
        void Update(GmmIncumplimientoDTO entity);
        void UpdateTrienio(GmmIncumplimientoDTO entity);
        void Delete(int empgcodi);
        GmmIncumplimientoDTO GetById(int incucodi);
        GmmIncumplimientoDTO GetByIdEdit(int incucodi);
        List<GmmIncumplimientoDTO> ListarFiltroIncumplimientoAfectada(int anio, string mes, string razonSocial, string numDocumento);
        List<GmmIncumplimientoDTO> ListarFiltroIncumplimientoDeudora(int anio, string mes, string razonSocial, string numDocumento);
    }
}
