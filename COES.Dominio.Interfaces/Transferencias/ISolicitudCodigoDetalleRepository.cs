using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface ISolicitudCodigoDetalleRepository : IRepositoryBase
    {
        int Save(SolicitudCodigoDetalleDTO entity);
        int Update(SolicitudCodigoDetalleDTO entity);
        int Delete(System.Int32 id);

        List<SolicitudCodigoDetalleDTO> ListaRelacion(int barrcoditra);

        List<BarraDTO> ListarBarraSuministro();

        int SaveBR(BarraRelacionDTO entity);

        int DeleteBR(int id);

        List<SolicitudCodigoDetalleDTO> ListarDetalle(int id);

        int SaveSubDetalle(CodigoGeneradoDTO entity);
        int DeleteGenerado(int id, string usuario,string codestado);
        int ObtenerNroCodigosGenerados(int id);

        int SolicitarBajarGenerado(CodigoGeneradoDTO entity);

        CodigoGeneradoDTO GetByIdGenerado(int id);
    }
}
