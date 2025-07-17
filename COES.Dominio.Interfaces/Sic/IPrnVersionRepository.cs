using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnVersionRepository
    {
        void Save(PrnVersionDTO entity);
        void Delete(int version);
        void Update(PrnVersionDTO entity);
        List<PrnVersionDTO> List();
        PrnVersionDTO GetById(int codigo);
        void UpdateAllVersionInactivo(string estado);
        List<PrnVersionDTO> GetModeloActivo(string idBarra);
    }
}
