using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PRN_MEDICIONEQ
    /// </summary>
    /// 
    public interface IPrnMedicioneqRepository
    {
        void Save(PrnMedicioneqDTO entity);
        void Update(PrnMedicioneqDTO entity);
        void Delete(int Prnmeqtipo, DateTime Medifecha);
        PrnMedicioneqDTO GetById(int Equicodi, int Prnmeqtipo, DateTime Medifecha);
        List<PrnMedicioneqDTO> List(int Areacodi, DateTime fecha);
        List<PrnMedicioneqDTO> GetByCriteria();
        List<int> ObtenerPuntosFaltantes(int equicodi, int emprcodi);
    }
}
