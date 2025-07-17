using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_PERIODOCUADRO
    /// </summary>
    public interface IIndPeriodocuadroRepository
    {
        int Save(IndPeriodocuadroDTO entity);
        void Update(IndPeriodocuadroDTO entity);
        void Delete(int percuacodi);
        IndPeriodocuadroDTO GetById(int percuacodi);
        List<IndPeriodocuadroDTO> List();
        List<IndPeriodocuadroDTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin, int descucodi);

        //Cuadro1
        List<IndPeriodocuadroDTO> GetIndisponibilidadesTolalesTermoXFecha(DateTime fechaIni, DateTime fechaFin);
   
        //Cuadro2
        List<IndPeriodocuadroDTO> ListPerCuad2PorFiltro(string TipOrigen, string FechaI, string FechaF,int cuadro);
        //Cuadro3
        List<IndPeriodocuadroDTO> GetFactorKTermoelectrico( string FechaI, string FechaF, int cuadro);
        //Cuadro4
        //cuadro5
        List<IndPeriodocuadroDTO> ListPerCuad5PorFiltro(string TipOrigen, string FechaI, string FechaF, int cuadro);
        
        List<IndPeriodocuadroDTO> GetCargarPotenciaAsegurada(DateTime mes);
   
    }
}
