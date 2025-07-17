using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnPerfilBarraRepository
    {
        void Save(PrnPerfilBarraDTO entity);
        void Update(PrnPerfilBarraDTO entity);
        void Delete(int prfbarcodi);
        List<PrnPerfilBarraDTO> List();
        PrnPerfilBarraDTO GetByIds(int reltnacodi, string prfbartipodia);
    }
}
