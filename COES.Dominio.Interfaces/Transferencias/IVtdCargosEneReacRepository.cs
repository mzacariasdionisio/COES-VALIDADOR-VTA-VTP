using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VTD_CARGOSENEREAC
    /// </summary>
    public interface IVtdCargosEneReacRepository
    {      
        
        //filtro por rango de fecha
        List<VtdCargoEneReacDTO> ListByParticipant(int emprcodi);
        List<VtdCargoEneReacDTO> List(DateTime date); // cambios

        //guardar
        int Save(VtdCargoEneReacDTO entity);
        //actualizar
        void Update(VtdCargoEneReacDTO entity);
        //borrar
        void Delete(int caercodi, DateTime date);
        void DeleteByEmpresa(int emprcodi);

        //obtener solo el monto
        VtdCargoEneReacDTO GetMontoByEmpresa(int emprcodi, DateTime caerfecha);

        //DeletedFisicByDate borrar fecha fisica
        //UpdateByResultDate actualizar el resultado de fecha
        void UpdateByResultDate(int caerdeleted, DateTime caerfecha);

        //SaveNowDate guardar la fecha actual

    }
}
