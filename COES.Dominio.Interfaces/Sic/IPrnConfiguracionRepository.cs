using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnConfiguracionRepository
    {
        void Save(PrnConfiguracionDTO entity);
        void Update(PrnConfiguracionDTO entity);
        void Delete(int ptomedicodi, DateTime prncfgfecha);
        List<PrnConfiguracionDTO> List();
        PrnConfiguracionDTO GetById(int ptomedicodi, DateTime prncfgfecha);
        void BulkInsert(List<PrnConfiguracionDTO> entitys);
        List<PrnConfiguracionDTO> ParametrosList(string fecdesde, string fechasta, string lstpuntos);
        PrnConfiguracionDTO GetConfiguracion(int ptomedicodi, string fecha, int defid, string deffecha);
    }
}
