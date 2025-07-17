using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IBarraRelacionRepository : IRepositoryBase
    {
        int Save(BarraRelacionDTO entity);
        int Delete(BarraRelacionDTO entity);
        List<BarraRelacionDTO> ListaRelacion(int id);
        bool ExisteRelacionBarra(BarraRelacionDTO entity);
    }
}
