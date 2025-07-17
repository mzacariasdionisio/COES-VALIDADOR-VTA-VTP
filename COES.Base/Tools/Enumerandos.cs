using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COES.Framework.Base.Tools
{
    /// <summary>
    /// Enumerando estados de atencion
    /// </summary> 
    [DataContract]
    [Flags]
    public enum EstadoAtencion
    {        
        [EnumMember]
        Pendiente = 'P',

        [EnumMember]
        Derivado = 'D',

        [EnumMember]
        Atendido = 'A',

        [EnumMember]
        Indefinido = 'I',

        [EnumMember]
        Anulado = 'X',

        [EnumMember]
        Todos = 'T',

        [EnumMember]
        Pend_or_Derivado = 'Q'
    }

    /// <summary>
    /// Enumerando estados de registro
    /// </summary>
    [DataContract]
    [Flags]
    public enum EstadoRegistro
    {
        [EnumMember]
        Anulado = 'A',

        [EnumMember]
        Borrado = 'B',

        [EnumMember]
        Habilitado = 'H',

        [EnumMember]
        Derivado = 'D'
    }

    /// <summary>
    /// Enumerando estados de trámite
    /// </summary>
    [DataContract]
    [Flags]
    public enum EstadoTramite
    {
        [EnumMember]
        Atendido = 'A',

        [EnumMember]
        Pendiente = 'P',

        [EnumMember]
        Anulado = 'X',

        [EnumMember]
        Borrado = 'B',

        [EnumMember]
        Archivado = 'C',

        [EnumMember]
        Indeterminado = 'I',

        [EnumMember]
        Nuevo = 'N'
    }

    /// <summary>
    /// Enumerando estados de registro auditoria
    /// </summary>
    [DataContract]
    [Flags]
    public enum EstadoRegistroAuditoria
    {
        [EnumMember]
        Activo = 1,

        [EnumMember]
        Inactivo = 0,

        [EnumMember]
        Historico = 1,
    }

    [DataContract]
    [Flags]
    public enum TablaCodigoAuditoria
    {
        [EnumMember]
        ValoracionesInherente = 1,

        [EnumMember]
        ValoracionesResidual = 2,

        [EnumMember]
        MesesAnio = 3,

        [EnumMember]
        TipoElemento = 4,

        [EnumMember]
        EstadoAuditoria = 6,

        [EnumMember]
        TipoActividad = 5,

        [EnumMember]
        EstadosRequerimientoInformacion = 7,

        [EnumMember]
        TipoInvolucrados = 8,

        [EnumMember]
        TipoHallazgo = 9,

        [EnumMember]
        CodigoPlanAuditoria = 10,

        [EnumMember]
        CodigoAuditoria = 11,

        [EnumMember]
        EstadosActividad = 16,
        [EnumMember]
        EstadosHallazgo = 15,
    }

    [DataContract]
    [Flags]
    public enum TablaCodigoAuditoriaDetalle
    {
        [EnumMember]
        MensajeDefecto = 48,
        [EnumMember]
        MensajeDefectoPrograma = 54,
        [EnumMember]
        MensajeDefectoReunionApertura = 55,
        [EnumMember]
        MensajeDefectoReunionCierre = 56,
        [EnumMember]
        MensajeDefectoInformeFinal = 57,
    }

    [DataContract]
    [Flags]
    public enum EstadoAuditoria
    {
        [EnumMember]
        Pendiente = 30,

        [EnumMember]
        Iniciado = 31,

        [EnumMember]
        Finalizado = 37
    }

    [DataContract]
    [Flags]
    public enum TipoActividad
    {
        [EnumMember]
        ReunionApertura = 24,
        [EnumMember]
        RequerimientoInformacion = 25,
        [EnumMember]
        RevisionElementosAuditar = 26,
        [EnumMember]
        ElaboracionPlanAccion = 27,
        [EnumMember]
        ReunionCierre = 28,
        [EnumMember]
        PresentacionInformeFinal = 29,
    }

    [DataContract]
    [Flags]
    public enum TipoElemento
    {
        [EnumMember]
        Control = 21,
        [EnumMember]
        NumeralProcedimiento = 22,
        [EnumMember]
        Otros = 23
    }

    [DataContract]
    [Flags]
    public enum TipoInvolucrado
    {
        [EnumMember]
        Equipo = 36,
        [EnumMember]
        Responsables = 37
    }


    public enum TablaCodigo
    {
        [EnumMember]
        EstadoDeAuditoria = 6,
        [EnumMember]
        TipoElemento = 4,
    }

    [DataContract]
    [Flags]
    public enum EstadoRequerimientoInformacion
    {
        [EnumMember]
        Pendiente = 33,
        [EnumMember]
        Entregado = 34,
        [EnumMember]
        Completado = 35,
    }

    [DataContract]
    [Flags]
    public enum TipoDestinatario
    {
        [EnumMember]
        Para = 45,
        [EnumMember]
        Copia = 46
    }

    [DataContract]
    [Flags]
    public enum TipoNotificacion
    {
        [EnumMember]
        NotificacionApertura = 47,
        [EnumMember]
        NotificacionCierre = 49,
        [EnumMember]
        NotificacionRequerimiento = 50,
        [EnumMember]
        NotificacionPrograma = 53,
        [EnumMember]
        NotificacionInformeFinal = 58,
    }

    [DataContract]
    [Flags]
    public enum TipoHallazgo
    {
        [EnumMember]
        OM = 38,
        [EnumMember]
        NC = 39,
        [EnumMember]
        OBS = 40,
        [EnumMember]
        FOR = 41,
        [EnumMember]
        TODOCONFORME = 44,
    }


    [DataContract]
    [Flags]
    public enum EstadoHallazgo
    {
        [EnumMember]
        Pendiente = 51,
        [EnumMember]
        Completado = 52,
    }

    [DataContract]
    [Flags]
    public enum EstadoActividad
    {
        [EnumMember]
        EnProceso = 59,
        [EnumMember]
        Finalizado = 60,
    }

    [DataContract]
    [Flags]
    public enum TablaCodigoDAI
    {
        [EnumMember]
        TipoEmpresa = 1,
    }

    [DataContract]
    [Flags]
    public enum TablaCodigoDetalleDAI
    {
        [EnumMember]
        Tasa_Interes = 6,
        [EnumMember]
        IGV = 9,
        [EnumMember]
        MONTOMINIMOPARTICIPACION = 10,
    }

    [DataContract]
    [Flags]
    public enum DaiEstadoAportante
    {
        [EnumMember]
        Finalizado = 2,
        [EnumMember]
        Liquidado = 3,
        [EnumMember]
        SinProcesar = 4,
        [EnumMember]
        Procesado = 5,
    }

    [DataContract]
    [Flags]
    public enum DaiEstadoRegistro
    {
        [EnumMember]
        Activo = 1,
        [EnumMember]
        Inactivo = 0,
    }

    [DataContract]
    [Flags]
    public enum DaiEstadoRegistroAportanteImportado
    {
        [EnumMember]
        Error = 0,
        [EnumMember]
        Correcto = 1,
    }

    [DataContract]
    [Flags]
    public enum DaiEstadoCronogramaCuota
    {
        [EnumMember]
        Pendiente = 7,
        [EnumMember]
        Pagado = 8,
        [EnumMember]
        Liquidado = 11,
    }

    /// <summary>
    /// Enumerando estados de atencion
    /// </summary> 
    [DataContract]
    [Flags]
    public enum DaiEmpresa
    {
        [EnumMember]
        Emprcoes = 'S',

        [EnumMember]
        Empractivo = 'A'
    }
}
