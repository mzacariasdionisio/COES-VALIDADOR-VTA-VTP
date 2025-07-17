using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IIO_CONTROL_IMPORTACION
    /// </summary>
    public interface IIioControlImportacionRepository
    {
        void Update(IioControlImportacionDTO iioControlImportacionDTO);
        int Save(IioControlImportacionDTO iioControlImportacionDTO);
        IioControlImportacionDTO GetByCriteria(IioControlImportacionDTO iioControlImportacionDTO);
        int GetCantidadRegistros(int periodo);
        int GetMaxId();
        void BulkInsert(List<IioControlImportacionDTO> entitys);
        List<IioControlImportacionDTO> ListByTabla(int periodo, string tabla);
        IioControlImportacionDTO GetByEmpresaTabla(int periodo, string tabla, string empresa);
        
    }
}