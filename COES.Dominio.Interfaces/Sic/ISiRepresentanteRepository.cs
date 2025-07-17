using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;
namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_REPRESENTANTE
    /// </summary>
    public interface ISiRepresentanteRepository
    {
        int Save(SiRepresentanteDTO entity, IDbConnection conn, DbTransaction tran);
        int Save(SiRepresentanteDTO entity);
        void Update(SiRepresentanteDTO entity);
        void UpdateEstadoRegistro(SiRepresentanteDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int rptecodi);
        SiRepresentanteDTO GetById(int rptecodi);
        List<SiRepresentanteDTO> List();
        List<SiRepresentanteDTO> GetByEmprcodi(int emprcodi);
        List<SiRepresentanteDTO> GetByCriteria();
        int ActualizarRepresentanteGestionModificacion(int idRepresentante,
            string tipoRepresentante,
            string dni,
            string nombre,
            string apellido,
            string cargo,
            string telefono,
            string telefonoMovil,
            DateTime? fechaVigenciaPoder,
            string usuario, string email);
        int ActualizarRepresentanteGestionModificacion(int idRepresentante, string telefono, string telefonoMovil);
        int ActualizarRepresentanteGestionModificacionVigenciaPoder(int idRepresentante, string telefono, string telefonoMovil, DateTime fechaVigenciaPoder);

        List<SiRepresentanteDTO> ObtenerRepresentantesTitulares(int idEmpresa);

        void ActualizarNotificacion(int idRepresentante, string indicador);
        void ActualizarRepresentante(SiRepresentanteDTO representante);
        void DarBajaRepresentante(int idRepresentante, string usuario);
    }
}
