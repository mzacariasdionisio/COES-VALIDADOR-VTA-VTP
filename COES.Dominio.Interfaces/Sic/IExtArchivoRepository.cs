using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EXT_ARCHIVO
    /// </summary>
    public interface IExtArchivoRepository
    {
        int Save(ExtArchivoDTO entity);
        void Update(ExtArchivoDTO entity);
        void Delete(int earcodi);
        ExtArchivoDTO GetById(int earcodi);
        List<ExtArchivoDTO> List();
        List<ExtArchivoDTO> GetByCriteria(int empresa, int estado, DateTime fechaInicial, DateTime fechaFinal);
        void UpdateMaxId(int idEnvio);
        List<ExtArchivoDTO> ListaEnvioPagInterconexion(int empresa,int estado ,DateTime fechaInicial, DateTime fechaFinal, int nroPagina, int nroFilas);
        int TotalEnvioInterconexion(DateTime fechaini, DateTime fechafin, int empresa);
        int SaveUpload(ExtArchivoDTO entity, string nombreFile, string extension);
    }
}
