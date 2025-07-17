using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_RECURSO
    /// </summary>
    public partial class CpRecursoDTO : EntityBase
    {
        public int? Recurconsideragams { get; set; }
        public int? Recurfamsic { get; set; }
        public int? Recurlogico { get; set; }
        public string Recurformula { get; set; }
        public int? Recurtoescenario { get; set; }
        public int? Recurorigen3 { get; set; }
        public int? Recurorigen2 { get; set; }
        public int Topcodi { get; set; }
        public int Catcodi { get; set; }
        public DateTime? Lastdate { get; set; }
        public string Lastuser { get; set; }
        public int? Recurestado { get; set; }
        public string Tablasicoes { get; set; }
        public int? Recurcodisicoes { get; set; }
        public int? Recurorigen { get; set; }
        public int? Recurpadre { get; set; }
        public string Recurnombre { get; set; }
        public int Recurcodi { get; set; }
        public int Equipadre { get; set; }

        //Yupana
        public decimal Ursmin { get; set; }
        public decimal Ursmax { get; set; }
        public int ProvisionBase { get; set; }
        public bool Seleccion { get; set; }
        public int Gequicodi { get; set; }
        public string Gequinomb { get; set; }
        public string Emprnomb { get; set; }
        public string Centralnomb { get; set; }
        public int Cnfbarcodi { get; set; }
        public string Recurnombsicoes { get; set; }

        #region Yupana Continuo
        public int ProvisionBaseUp { get; set; }
        public int ProvisionBaseDn { get; set; }
        public string ConsideraEquipo { get; set; }
        public string RecNodoTopOrigen { get; set; }
        public string RecNodoTopDestino { get; set; }
        public int? RecNodoTopOrigenID { get; set; }
        public int? RecNodoTopDestinoID { get; set; }
        public int? RecNodoID { get; set; } // id al nodo topologico conectado.
                                            //Reservado para topologia Hidrologica (Vierte y Turbina)
        public int CatcodiVierte { get; set; }
        public int IDVierte { get; set; }
        public int CatcodiTurbina { get; set; }
        public int IDTurbina { get; set; }
        public int RecurcodiOffline { get; set; } // Aca se almacenar el RecursoID en el archivo CSV
        public int RecurcodiConec { get; set; }
        public int CatcodiConec { get; set; }
        //Yupana Iteracion 3
        public int Fuentecodi { get; set; }
        public int Combcodi { get; set; }
        // Fin Yupana Iteracion 3
        //Yupana 2022
        public decimal? TiempoUp { get; set; }
        public decimal? TiempoDown { get; set; }
        public decimal? TiempoES { get; set; }
        public decimal? TiempoFS { get; set; }
        //Fin Yupana 2022
        #endregion
    }

    public partial class CpRecursoDTO
    {
        public int Propcodi { get; set; }
        public string Valor { get; set; }
        public string Equinomb { get; set; }
        public int Recurcodicentral { get; set; }
        public int Recurcodisicoescentral { get; set; }
        public int Recurcodibarra { get; set; }
        public int Recurcodisicoesbarra { get; set; }
    }
}
