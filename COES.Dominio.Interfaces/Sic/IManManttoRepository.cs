using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MAN_MANTTO
    /// </summary>
    public interface IManManttoRepository
    {
        int Save(ManManttoDTO entity);
        void Update(ManManttoDTO entity);
        void Delete(int mancodi);
        ManManttoDTO GetById(int mancodi);
        List<ManManttoDTO> List();
        List<ManManttoDTO> GetByCriteria();
        List<ManManttoDTO> ListManttoPorEquipoFecha(int equicodi, DateTime fecha);
    }
}
