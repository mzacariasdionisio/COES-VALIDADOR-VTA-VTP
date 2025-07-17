using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_PTOMEDICION
    /// </summary>
    //[Serializable]
    public class MePtomedicionDTO : EntityBase, ICloneable
    {
        public int Ptomedicodi { get; set; }
        public DateTime? Lastdate { get; set; }
        public string Lastuser { get; set; }
        public int? Emprcodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Tipoinfocodi { get; set; }
        public string Osicodi { get; set; }
        public int? Equicodi { get; set; }
        public int? Codref { get; set; }
        public string Ptomedidesc { get; set; }
        public int? Orden { get; set; }
        public string Ptomedielenomb { get; set; }
        public string Ptomedibarranomb { get; set; }
        public int? Origlectcodi { get; set; }
        public int Tipoptomedicodi { get; set; }
        public string Ptomediestado { get; set; }
        public int? Emprcodref { get; set; }
        public short Famcodi { get; set; }
        public string Centralnomb { get; set; }
        public string Gruponomb { get; set; }
        public string Grupoabrev { get; set; }
        public string Grupoestado { get; set; }
        public string Emprnomb { get; set; }
        public string Emprestado { get; set; }
        public string Equinomb { get; set; }
        public string Famnomb { get; set; }
        public string Tipoptomedinomb { get; set; }
        public string Origlectnombre { get; set; }
        public int ColFormato { get; set; }
        public double LimiteUp { get; set; }
        public string AreaOperativa { get; set; }
        public decimal NivelTension { get; set; }
        public string DesUbicacion { get; set; }

        public int? TipoSerie { get; set; }

        //Region Transferencia  de Equipos
        public int Lastcodi { get; set; }
        //Fin Transferencia Equipos
        public string CoordenadaX { get; set; }
        public string CoordenadaY { get; set; }
        public string Altitud { get; set; }
        public string Capacidad { get; set; }
        public string EquiPadrenomb { get; set; }

        #region PR5
        public int Equipadre { get; set; }
        public string Padrenomb { get; set; }
        public string Subestacion { get; set; }
        public decimal ValorEje { get; set; }
        public decimal ValorProg { get; set; }
        public decimal ValorReprog { get; set; }
        public decimal ValorDifer { get; set; }
        public string Central { get; set; }
        public List<MeMedicion48DTO> ListaReservaFria { get; set; }
        public string Tipoinfoabrev { get; set; }
        public string Emprabrev { get; set; }
        public string Tptomedinomb { get; set; }
        public string Ptomediestadodescrip { get; set; }
        public int? Grupopadre { get; set; }
        public string Grupocentral { get; set; }
        public string Equiabrev { get; set; }
        public string PtomediCalculado { get; set; }
        public string PtomediCalculadoDescrip { get; set; }
        public int? TipoinfocodiOrigen { get; set; }
        public decimal FactorOrigen { get; set; }

        public decimal FactorPotencia {  get; set; }
        public int Tgenercodi { get; set; }
        public string Tgenernomb { get; set; }
        public int Fenergcodi { get; set; }
        public string Fenergnomb { get; set; }
        public int PtomedicodiOrigen { get; set; }
        public int TipoRelacioncodi { get; set; }
        public string PtomedibarranombOrigen { get; set; }
        public string PtomedielenombOrigen { get; set; }
        public string PtomedicodidescOrigen { get; set; }

        public int FamcodiOrigen { get; set; }
        public string FamnombOrigen { get; set; }
        public string FamabrevOrigen { get; set; }
        public string Famabrev { get; set; }
        public string Catenomb { get; set; }

        public int EquipadreOrigen { get; set; }
        public string CentralOrigen { get; set; }

        public int EquicodiOrigen { get; set; }
        public string EquiabrevOrigen { get; set; }
        public string EquinombOrigen { get; set; }

        public decimal Relptocodi { get; set; }
        public int EmprcodiOrigen { get; set; }
        public string EmprabrevOrigen { get; set; }
        public string EmprnombOrigen { get; set; }
        public string PtomedicodiCalculadoDescrip { get; set; }
        public int PtomedicodiCalculado { get; set; }
        public string Lectnomb { get; set; }
        public int? Hojaptoactivo { get; set; }
        public int? Emprorden { get; set; }
        public int? Grupoorden { get; set; }

        public int Repptotabmed { get; set; }
        public string RepptotabmedDesc { get; set; }

        public int Ctgdetcodi { get; set; }
        public string Repptonomb { get; set; }
        public string Colorcelda { get; set; }

        #endregion

        #region Pronostico de la Demanda

        public int Lectcodi { get; set; }
        //Pendiente
        public bool PrnValidacion { get; set; }
        public DateTime Medifecha { get; set; }
        public int Prnitv { get; set; }
        public bool Prnval { get; set; }

        //Agregado 20181126
        public int Tipoemprcodi { get; set; }
        public string Tipoemprdesc { get; set; }
        public string Tareaabrev { get; set; }
        //----------

        //Agregado 20190109
        public int Ptogrpcodi { get; set; }
        public int Ptogrppronostico { get; set; }
        public DateTime Ptogrpfechaini { get; set; }
        public DateTime Ptogrpfechafin { get; set; }        
        //----------

        //Agregado 20190123
        public List<object> ListPtomedidesc { get; set; }
		//----------

        //Agregado 20190211
        public decimal Meditotal { get; set; }
        //----------

        //Agregado 20190312
        public bool Ptodepurado { get; set; }
        //----------

        //Agregado 20200128
        public int Grupocodibarra { get; set; }
        //----------

        //Agregado 20200303
        public int Ptogrphijocodi { get; set; }
        public string Ptogrphijodesc { get; set; }
        //----------

        //Agregado 20200616
        public string Prnpmpvarexoproceso { get; set; }
        //----------
        #endregion

        #region MigracionSGOCOES-GrupoB
        public string Areanomb { get; set; }
        public int Areacodi { get; set; }
        public string StrAreacodi { get; set; }
        public double Equitension { get; set; }
        public string Equiestado { get; set; }
        public string Grupoactivo { get; set; }
        #endregion

        #region SIOSEIN2

        public int Sconcodi { get; set; }
        public int Hopcodi { get; set; }
        public int Catecodi { get; set; }

        #endregion

        #region Numerales Datos Base
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Valor { get; set; }

        public string Emprruc { get; set; }
        public string Emprrazsocial { get; set; }
        public string Osicodi1 { get; set; }
        #endregion

        public int Idtv { get; set; }
        public bool TieneTv { get; set; }
        public string ColorEstado { get; set; }
        public string Tipogenerrer { get; set; }
        public string Grupotipocogen { get; set; }
        public string Grupointegrante { get; set; }
        public string Tipoinfodesc { get; set; }
        public decimal? H1 { get; set; }

        #region FIT - Aplicativo VTD
        public int? Clientecodi { get; set; }
        public int? Barrcodi { get; set; }
        public string ClientNomb { get; set; }
        public string BarrNomb { get; set; }
        public string PuntoConexion { get; set; }
        #endregion

        public int Recurcodi { get; set; }
        public string Recurnombre { get; set; }

        #region PMPOe3
        public string CodinombSDDP { get; set; }
        public string DescripcionSDDP { get; set; }
        public decimal Hojaptoliminf { get; set; }
        public decimal Hojaptolimsup { get; set; }
        #endregion

        public string Canales { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
