using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla DATOS_SP7
    /// </summary>
    public interface IDatosSp7Repository
    {
        //int Save(DatosSP7DTO entity);
        /*void Update(DatosSP7DTO entity);
        void Delete(int canalcodi);
        DatosSP7DTO GetById(int canalcodi);
        List<DatosSP7DTO> List();
        List<DatosSP7DTO> GetByCriteria();
        int SaveDatosSp7Id(DatosSp7DTO entity);*/
        //List<DatosSp7DTO> BuscarOperaciones(DateTime fecha,DateTime fechasistema,int nroPage, int PageSize);
        //int ObtenerNroFilas(DateTime fecha,DateTime fechasistema);
        List<DatosSP7DTO> ObtenerListadoSp7(string tabla, DateTime fechaInicial, DateTime fechaFinal, string path);

    }
}
