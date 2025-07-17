using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_PAGORSF
    /// </summary>
    public interface IVcrPagorsfRepository
    {
        int Save(VcrPagorsfDTO entity);
        void Update(VcrPagorsfDTO entity);
        void Delete(int vcrecacodi);
        VcrPagorsfDTO GetById(int vcprsfcodi);
        List<VcrPagorsfDTO> List();
        List<VcrPagorsfDTO> GetByCriteria();
        VcrPagorsfDTO GetByIdUnidad2020(int vcrecacodi, int equicodi);
        VcrPagorsfDTO GetByIdUnidad(int vcrecacodi, int equicodi);
        VcrPagorsfDTO GetByIdUnidadPorEmpresa(int vcrecacodi, int equicodi, int empresa);
        VcrPagorsfDTO GetByMigracionEquiposPorEmpresaOrigenxDestino(int vcrecacodi, int emprcodiorigen, int emprcodidestino);
    }
}
