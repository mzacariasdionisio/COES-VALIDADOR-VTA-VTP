using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_PERIODO
    /// </summary>
    public interface IPmoPeriodoRepository
    {
        int Save(PmoPeriodoDTO entity);
        PmoPeriodoDTO GetById(int id);
        void UpdateFechasMantenimiento(PmoPeriodoDTO entity);
        void Update(PmoPeriodoDTO entity);
        void Delete(int pmpericodi);
        List<PmoPeriodoDTO> List();
        List<PmoPeriodoDTO> GetByCriteria(int anio);
    }
}
