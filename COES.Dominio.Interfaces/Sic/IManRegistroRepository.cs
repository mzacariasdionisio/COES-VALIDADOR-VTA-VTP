using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MAN_REGISTRO
    /// </summary>
    public interface IManRegistroRepository
    {
        int Save(ManRegistroDTO entity);
        void Update(ManRegistroDTO entity);
        void Delete(int regcodi);
        ManRegistroDTO GetById(int regcodi);
        List<ManRegistroDTO> List();
        List<ManRegistroDTO> GetByCriteria();
        int ObtenerNroFilasManRegistroxTipo(int? idEvento);
        List<ManRegistroDTO> BuscarManRegistro(int idEvento, int nroPagina, int nroFilas);
    }
}

