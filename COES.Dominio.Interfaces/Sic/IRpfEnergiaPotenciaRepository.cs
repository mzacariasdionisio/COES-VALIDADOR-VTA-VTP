using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IRpfEnergiaPotenciaRepository
    {
        int Save(RpfEnergiaPotenciaDTO entity);
        void Update(RpfEnergiaPotenciaDTO entity);
        List<RpfEnergiaPotenciaDTO> List(DateTime fechaini, DateTime fechafin);
        void Delete(DateTime fecha);
        RpfEnergiaPotenciaDTO GetById(DateTime fecha);
    }
}

