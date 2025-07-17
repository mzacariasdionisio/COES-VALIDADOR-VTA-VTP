using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;


namespace COES.Dominio.Interfaces.Sic
{
    public interface IEpoPuntoConexionRepository
    {
        List<EpoPuntoConexionDTO> List();
        EpoPuntoConexionDTO GetById(int estepocodi);
        List<EpoPuntoConexionDTO> GetByCriteria(EpoPuntoConexionDTO estudioepo);

        int ObtenerNroRegistroBusqueda(EpoPuntoConexionDTO estudioepo);


        int Save(EpoPuntoConexionDTO entity);
        void Update(EpoPuntoConexionDTO entity);
        void Delete(int puntcodi);
        EpoPuntoConexionDTO GetByCodigo(string descripcion);

    }
}
