using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_PERIODO
    /// </summary>
    public interface IRePeriodoRepository
    {
        int Save(RePeriodoDTO entity);
        void Update(RePeriodoDTO entity);
        void Delete(int repercodi);
        RePeriodoDTO GetById(int repercodi);
        List<RePeriodoDTO> List();
        List<RePeriodoDTO> GetByCriteria(int anioDesde, int anioHasta, string estado);
        List<RePeriodoDTO> ObtenerPeriodosPadre(int anio);
        int ValidarNombre(string nombre, int id);
        List<RePeriodoDTO> ObtenerPeriodosCercanos(int anio, int id);
    }
}
