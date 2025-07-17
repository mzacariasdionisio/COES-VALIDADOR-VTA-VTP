using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Dominio.Interfaces.Sic
{
    public interface IMapDemandaRepository
    {
        int Save(MapDemandaDTO entity);
        void Update(MapDemandaDTO entity);
        void Delete(int empulcodi);
        MapDemandaDTO GetById(int empulcodi);
        List<MapDemandaDTO> List();
        List<MapDemandaDTO> GetByCriteria(int vermcodi);
    }
}
