using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPR_PROYECTO_ACTEQP
    /// </summary>
    public interface IEprProyectoActEqpRepository
    {
        int Save(EprProyectoActEqpDTO entity);
        void Update(EprProyectoActEqpDTO entity);
        void Delete_UpdateAuditoria(EprProyectoActEqpDTO entity);
        EprProyectoActEqpDTO GetById(int epprproycodi);
        List<EprProyectoActEqpDTO> List(int epproysgcodi, string epproynomb, string epproyfecregistroIni, string epproyfecregistroFin);
        List<EprProyectoActEqpDTO> ListProyectoProyectoActualizacion(int equicodi);

        List<EprProyectoActEqpDTO> ListMaestroProyecto();

        EprProyectoActEqpDTO ValidarProyectoActualizacionPorRele(int epproycodi);
    }
}
