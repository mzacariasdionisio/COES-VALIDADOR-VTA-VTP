using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPR_EQUIPO
    /// </summary>
    [DataContract]
    [Serializable]
    public class EprEquipoDTO : EntityBase
    {
        [DataMember]
        public int Epequicodi { get; set; }
        [DataMember]
        public int? Equicodi { get; set; }
        [DataMember]
        public string Epequinomb { get; set; }
        [DataMember]
        public string Epequiestregistro { get; set; }
        [DataMember]
        public string Epequiusucreacion { get; set; }
        [DataMember]
        public string Epequiusumodificacion { get; set; }
        [DataMember]
        public DateTime Epequifeccreacion { get; set; }

        [DataMember]
        public DateTime? Epequifecmodificacion { get; set; }

        [DataMember]
        public string Epequiflagsev { get; set; }


        #region ArbolEquipoProteccion
        [DataMember]
        public int Equicodipadre { get; set; }

        [DataMember]
        public String Equinomb { get; set; }
        [DataMember]
        public int Nivel { get; set; }
        #endregion

        #region SpGuardarRele
        public int IdCelda { get; set; }
        public int IdProyecto { get; set; }
        public string Codigo { get; set; }
        public string Fecha { get; set; }
        public int IdTitular { get; set; }
        public string Tension { get; set; }
        public string IdSistermaRele { get; set; }
        public string IdMarca { get; set; }
        public string Modelo { get; set; }
        public string IdTipoUso { get; set; }
        public string RtcPrimario { get; set; }
        public string RtcSecundario { get; set; }
        public string RttPrimario { get; set; }
        public string RttSecundario { get; set; }
        public string ProtCondinables { get; set; }
        public string SincroCheckActivo { get; set; }
        public string IdInterruptor { get; set; }
        public string DeltaTension { get; set; }
        public string DeltaAngulo { get; set; }
        public string DeltaFrecuencia { get; set; }
        public string SobreCCheckActivo { get; set; }
        public string SobreCI { get; set; }
        public string SobreTCheckActivo { get; set; }
        public string SobreTU { get; set; }
        public string SobreTT { get; set; }
        public string SobreTUU { get; set; }
        public string SobreTTT { get; set; }
        public string PmuCheckActivo { get; set; }
        public string PmuAccion { get; set; }
        public string IdInterruptorMS { get; set; }
        public string IdMandoSincronizado { get; set; }
        public string MedidaMitigacion { get; set; }
        public string ReleTorsImpl { get; set; }
        public string RelePmuAccion { get; set; }
        public string RelePmuImpl { get; set; }
        public string UsuarioAuditoria { get; set; }
        public int EquiCodiRele { get; set; }
        public string FlagResultado { get; set; }
        public string MensajeError { get; set; }


        public int IdLinea { get; set; }
        public int IdArea { get; set; }
        public string CapacidadA { get; set; }
        public string CapacidadMva { get; set; }
        public int IdCelda2 { get; set; }
        public int IdBancoCondensador { get; set; }
        public string CapacTransCond1Porcen { get; set; }
        public string CapacTransCond1Min { get; set; }
        public string CapacTransCond1A { get; set; }
        public string CapacTransCond2Porcen { get; set; }
        public string CapacTransCond2Min { get; set; }
        public string CapacTransCond2A { get; set; }
        public string CapacidadTransmisionA { get; set; }
        public string CapacidadTransmisionMVA { get; set; }
        public string LimiteSegCoes { get; set; }
        public string FactorLimitanteCalc { get; set; }
        public string FactorLimitanteFinal { get; set; }
        public string Observaciones { get; set; }

        public string CapacidadMvar { get; set; }
        public int IdUbicacion { get; set; }
        public string Ubicacion { get; set; }
        public string BancoCondensadorSerieCapacidadA { get; set; }
        public string BancoCondensadorSerieCapacidadMVAR { get; set; }

        public int CantidadFilas { get; set; }


        #endregion

        #region ListEquipoProtGrilla
        public string Celda { get; set; }
        public string Sistema { get; set; }
        public string Marca { get; set; }
        public string TipoUso { get; set; }
        public string Estado{ get; set; }
        public int Areacodi { get; set; }

        #endregion

        #region ListLineaTiempo
        public string ProyectoNomb { get; set; }
        public string ProyectoDesc { get; set; }
        #endregion

        #region EquipamientoModificado
        [DataMember]
        public string SubestacionNomb { get; set; }
        [DataMember]
        public string Rele { get; set; }
        [DataMember]
        public string SistemaRele { get; set; }
        [DataMember]
        public string MemoriaCalculo { get; set; }
        [DataMember]
        public string MemoriaCalculoTexo { get; set; }
        [DataMember]
        public string Accion { get; set; }
        [DataMember]
        public string ProyectoCreador { get; set; }
        [DataMember]
        public string Fechacreacionstr { get; set; }
        [DataMember]
        public string Fechamodificacionstr { get; set; }
        public string ProyectoActualizador { get; set; }
        #endregion

        #region ZipHistorialCambio
        [DataMember]
        public string Zona { get; set; }
        [DataMember]
        public string NombreArchivo { get; set; }
        [DataMember]
        public string Tipo { get; set; }
        #endregion

        #region Validacion de Eliminacion
        [DataMember]
        public int? NroEquipos { get; set; }
        #endregion

        #region Relé
        public string Subestacion { get; set; }
        public string CodigoInterruptor { get; set; }
        public string UsuarioModificacion { get; set; }
        public string FechaModificacion { get; set; }

        #endregion



        #region LineaEvaluacíón
        public string AreaNomb { get; set; }
        public string EmprNomb { get; set; }
        public string PosicionNucleoTc { get; set; }
        public string PickUp { get; set; }
        public string Longitud { get; set; }
        public string CeldaPosicionNucleoTc { get; set; }
        public string CeldaPickUp { get; set; }

        public string Celda2PosicionNucleoTc { get; set; }
        public string Celda2PickUp { get; set; }
        public string EquiAbrev { get; set; }
        public string Subestacion1 { get; set; }
        public string Subestacion2 { get; set; }
        #endregion

        #region Celda Acoplamiento
        public string AreaDescripcion { get; set; }
        public string Empresa { get; set; }
        public string EquipoTension { get; set; }
        public string EquipoEstado { get; set; }
        public string EquipoEstadoDescripcion { get; set; }
        public string InterruptorEmpresa { get; set; }
        public string InterruptorTension { get; set; }
        public string InterruptorCapacidadA { get; set; }
        public string InterruptorCapacidadAComent { get; set; }
        public string InterruptorCapacidadMva { get; set; }
        public string InterruptorCapacidadMvaComent { get; set; }
        public string CapacidadTransmisionMva { get; set; }
        public string CapacidadTransmisionMvaComent { get; set; }
        public string CapacidadAComent { get; set; }
        public string CapacidadMvarComent { get; set; }

        #endregion

        #region Transversal
        public string Area { get; set; }
        public string Propiedad { get; set; }
        public string Valor { get; set; }
        public string Comentario { get; set; }
        public string MotivoActualizacion { get; set; }
        public string Motivo { get; set; }
        public string Usuario { get; set; }
        public string FechaActualizacion { get; set; }
        public string Propnomb { get; set; }
        public string Propequicomentario { get; set; }
        public string Propcodi { get; set; }
        public string Fechapropequi { get; set; }
        public string Orden { get; set; }
        public string Epproycodi { get; set; }
        public string FechaProyecto { get;  set; }
        public string FechaVigencia { get; set; }
        #endregion

        #region Reactor
        public int IdReactor { get; set; }
        public string IdCelda1 { get; set; }
        public string IdCelda22 { get; set; }
        public string CapacidadTransmisionAComent { get; set; }
        public string CapacidadTransmisionMvar { get; set; }
        public string CapacidadTransmisionMvarComent { get; set; }
        public string FactorLimitanteCalcComent { get; set; }
        public string FactorLimitanteFinalComent { get; set; }
        public string ActualizadoPor { get; set; }
        public string ActualizadoEl { get; set; }

        public string Proyecto { get; set; }
        public string IdEmpresa { get; set; }
        public string Celda1PosicionNucleoTc { get; set; }
        public string Celda1PickUp { get; set; }
        public string NivelTension { get; set; }

        #endregion

        #region Transformador
        public int IdTransformador { get; set; }
        public string D1IdCelda { get; set; }
        public string D1CapacidadOnanMva { get; set; }
        public string D1CapacidadOnanMvaComent { get; set; }
        public string D1CapacidadOnafMva { get; set; }
        public string D1CapacidadOnafMvaComent { get; set; }
        public string D1CapacidadMva { get; set; }
        public string D1CapacidadMvaComent { get; set; }
        public string D1CapacidadA { get; set; }
        public string D1CapacidadAComent { get; set; }
        public string D1PosicionTcA { get; set; }
        public string D1PosicionPickUpA { get; set; }
        public string D1CapacidadTransmisionA { get; set; }
        public string D1CapacidadTransmisionAComent { get; set; }
        public string D1CapacidadTransmisionMva { get; set; }
        public string D1CapacidadTransmisionMvaComent { get; set; }
        public string D1FactorLimitanteCalc { get; set; }
        public string D1FactorLimitanteCalcComent { get; set; }
        public string D1FactorLimitanteFinal { get; set; }
        public string D1FactorLimitanteFinalComent { get; set; }
        public string D2IdCelda { get; set; }
        public string D2CapacidadOnanMva { get; set; }
        public string D2CapacidadOnanMvaComent { get; set; }
        public string D2CapacidadOnafMva { get; set; }
        public string D2CapacidadOnafMvaComent { get; set; }
        public string D2CapacidadMva { get; set; }
        public string D2CapacidadMvaComent { get; set; }
        public string D2CapacidadA { get; set; }
        public string D2CapacidadAComent { get; set; }
        public string D2PosicionTcA { get; set; }
        public string D2PosicionPickUpA { get; set; }
        public string D2CapacidadTransmisionA { get; set; }
        public string D2CapacidadTransmisionAComent { get; set; }
        public string D2CapacidadTransmisionMva { get; set; }
        public string D2CapacidadTransmisionMvaComent { get; set; }
        public string D2FactorLimitanteCalc { get; set; }
        public string D2FactorLimitanteCalcComent { get; set; }
        public string D2FactorLimitanteFinal { get; set; }
        public string D2FactorLimitanteFinalComent { get; set; }
        public string D3IdCelda { get; set; }
        public string D3CapacidadOnanMva { get; set; }
        public string D3CapacidadOnanMvaComent { get; set; }
        public string D3CapacidadOnafMva { get; set; }
        public string D3CapacidadOnafMvaComent { get; set; }
        public string D3CapacidadMva { get; set; }
        public string D3CapacidadMvaComent { get; set; }
        public string D3CapacidadA { get; set; }
        public string D3CapacidadAComent { get; set; }
        public string D3PosicionTcA { get; set; }
        public string D3PosicionPickUpA { get; set; }
        public string D3CapacidadTransmisionA { get; set; }
        public string D3CapacidadTransmisionAComent { get; set; }
        public string D3CapacidadTransmisionMva { get; set; }
        public string D3CapacidadTransmisionMvaComent { get; set; }
        public string D3FactorLimitanteCalc { get; set; }
        public string D3FactorLimitanteCalcComent { get; set; }
        public string D3FactorLimitanteFinal { get; set; }
        public string D3FactorLimitanteFinalComent { get; set; }
        public string D4IdCelda { get; set; }
        public string D1Tension { get; set; }
        public string D2Tension { get; set; }
        public string D3Tension { get; set; }
        public string D4Tension { get; set; }
        public string D4CapacidadOnanMva { get; set; }
        public string D4CapacidadOnanMvaComent { get; set; }
        public string D4CapacidadOnafMva { get; set; }
        public string D4CapacidadOnafMvaComent { get; set; }
        public string D4CapacidadMva { get; set; }
        public string D4CapacidadMvaComent { get; set; }
        public string D4CapacidadA { get; set; }
        public string D4CapacidadAComent { get; set; }
        public string D4CapacidadTransmisionMva { get; set; }
        public string D4CapacidadTransmisionMvaComent { get; set; }
        public string D4CapacidadTransmisionA { get; set; }
        public string D4CapacidadTransmisionAComent { get; set; }
        public string D4FactorLimitanteCalc { get; set; }
        public string D4FactorLimitanteCalcComent { get; set; }
        public string D4FactorLimitanteFinal { get; set; }
        public string D4FactorLimitanteFinalComent { get; set; }
        public string D2Observaciones { get; set; }
        public string D3Observaciones { get; set; }
        public string D4Observaciones { get; set; }
        public string D4PosicionTcA { get; set; }
        public string D4PosicionPickUpA { get; set; }

        public string D1TensionComent { get; set; }
        public string D2TensionComent { get; set; }
        public string D3TensionComent { get; set; }
        public string D4TensionComent { get; set; }
        #endregion

        #region Relé
        public string MandoSincronizado { get; set; }
        public string ReleTorsionalImplementadoDsc { get; set; }
        #endregion

        #region Equipo
        public string EmprCodi { get; set; }
        public string GrupoCodi { get; set; }
        public string EleCodi { get; set; }
        public string Areacodigo { get; set; }
        public string FamCodigo { get; set; }
        public string EquiNomb { get; set; }
        public string EquiAbrev2 { get; set; }
        public string EquiTension { get; set; }
        public string EquiPadre { get; set; }
        public string EquiPot { get; set; }
        public string LastUser { get; set; }
        public string LastDate { get; set; }
        public string ECodigo { get; set; }
        public string EquiEstado { get; set; }
        public string OsinergCodi { get; set; }
        public string OsinergCodigen { get; set; }
        public string OperadorEmprcodi { get; set; }
        public string LastCodi { get; set; }
        public string EquiFechiniopcom { get; set; }
        public string EquiFechfinopcom { get; set; }
        public string FamNomb { get; set; }
        public string FamAbrev { get; set; }
        public string TareaAbrev { get; set; }
        public string UsuarioUpdate { get; set; }
        public string FechaUpdate { get; set; }
        public string EquiManiobr { get; set; }
        #endregion

        #region Reporte Limite Capacidad
        public string Revision { get; set; }
        public string Descripcion { get; set; }
        public string EmitidoEl { get; set; }
        public string ElaboradoPor { get; set; }
        public string RevisadoPor { get; set; }
        public string AprobadoPor { get; set; }
        public string EprtlcCodi { get; set; }
        public string EprtlcRevision { get; set; }
        public string EprtlcDescripcion { get; set; }
        public string EprtlcFecemision { get; set; }
        public string EprtlcUsuelabora { get; set; }
        public string EprtlcUsurevisa { get; set; }
        public string EprtlcUsuaprueba { get; set; }
        public string EprtlcEstregistro { get; set; }
        public string EprtlcUsucreacion { get; set; }
        public string EprtlcFeccreacion { get; set; }
        public string EprtlcUsumodificacion { get; set; }
        public string EprtlcFecmodificacion { get; set; }
        public string EprtlcNoarchivo { get; set; }

        #endregion


        #region Lista Linea
        public string CapacidadABancoCondensador { get; set; }
        public string CapacidadABancoCondensadorComent { get; set; }
        public string CapacidadMvarBancoCondensador { get; set; }
        public string CapacidadMvarBancoCondensadorComent { get; set; }
        public string CapacidadMvaComent { get; set; }
        public string CapacTransCond1PorcenComent { get; set; }
        public string CapacTransCond1MinComent { get; set; }
        public string CapacTransCond1AComent { get; set; }
        public string CapacTransCond2PorcenComent { get; set; }
        public string CapacTransCond2MinComent { get; set; }
        public string CapacTransCond2AComent { get; set; }
        public string LimiteSegCoesComent { get; set; }
        public string BancoCapacidadAComent { get; set; }
        public string BancoCapacidadMVArComent { get; set; }
        public string CapacidadTransmisionMVAComent { get; set; }
        public string BancoCapacidadA { get; set; }
        public string BancoCapacidadMVAr { get; set; }
        #endregion

        public string D1PickUp { get; set; }

        public string D2PickUp { get; set; }

        public string D3PickUp { get; set; }

        public string D4PickUp { get; set; }

        public int Famcodi {  get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
