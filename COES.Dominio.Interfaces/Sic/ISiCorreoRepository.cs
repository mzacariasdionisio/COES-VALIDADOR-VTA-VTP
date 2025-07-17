using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_CORREO
    /// </summary>
    public interface ISiCorreoRepository
    {
        int Save(SiCorreoDTO entity);
        void Update(SiCorreoDTO entity);
        void Delete(int corrcodi);
        SiCorreoDTO GetById(int corrcodi);
        List<SiCorreoDTO> List();
        List<SiCorreoDTO> GetByCriteria(string strIdEmpresas, DateTime fechaIni, DateTime fechaFin, string idsPlantilla);
        List<SiCorreoDTO> ListarLogCorreo(DateTime fecha, string idsPlantilla);
    }
}
