using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnPtomedpropRepository
    {
        void Save(PrnPtomedpropDTO entity);
        void Update(PrnPtomedpropDTO entity);
        void Delete(int ptomedicodi);
        List<PrnPtomedpropDTO> List();
        PrnPtomedpropDTO GetById(int ptomedicodi);

        List<MePtomedicionDTO> PR03Puntos();
        List<PrnAgrupacionDTO> PR03Agrupaciones(int origlectcodi, int grupocodibarra);
    }
}
