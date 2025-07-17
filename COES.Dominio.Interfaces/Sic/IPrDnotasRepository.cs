using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_DNOTAS
    /// </summary>
    public interface IPrDnotasRepository
    {
        void Save(PrDnotasDTO entity);
        void Update(PrDnotasDTO entity);
        void Delete(DateTime fecha, int lectcodi, int notaitem);
        PrDnotasDTO GetById(DateTime fecha, int lectcodi, int notaitem);
        List<PrDnotasDTO> List();
        List<PrDnotasDTO> GetByCriteria(DateTime fecha);
    }
}
