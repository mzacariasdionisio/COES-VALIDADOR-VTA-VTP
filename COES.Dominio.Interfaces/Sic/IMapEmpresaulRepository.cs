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
    public interface IMapEmpresaulRepository
    {
        int Save(MapEmpresaulDTO entity);
        void Update(MapEmpresaulDTO entity);
        void Delete(int empulcodi);
        MapEmpresaulDTO GetById(int empulcodi);
        List<MapEmpresaulDTO> List();
        List<MapEmpresaulDTO> GetByCriteria(int vermcodi);
    }
}
