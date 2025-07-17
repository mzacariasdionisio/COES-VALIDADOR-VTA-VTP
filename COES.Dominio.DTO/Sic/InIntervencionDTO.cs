using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_INTERVENCION
    /// </summary>
    public partial class InIntervencionDTO : EntityBase, ICloneable
    {
        public int Intercodi { get; set; } // Id de Intervención
        public string Intercodsegempr { get; set; }    // Código de seguimiento de empresa para registro masivo de la intervención (Para controlar la trazabilidad de las intervenciones en el horizonte establecido (Programación))

        public int Progrcodi { get; set; }  // Id de Programación
        public int Evenclasecodi { get; set; }  // Id Tipo de Programacion

        public int Emprcodi { get; set; }  // Id de Empresa
        public int Areacodi { get; set; } // Id de Area (Ubicación del Equipo)
        public int Equicodi { get; set; } // Id de Equipo
        public int Operadoremprcodi { get; set; } // Id de la empresa Operador

        public DateTime? Interfechapreini { get; set; } // Fecha de Pre Inicio
        public DateTime Interfechaini { get; set; }     // Fecha Hora de Inicio de la Intervención
        public DateTime? Interfechaprefin { get; set; } // Fecha de Pre Fin
        public DateTime Interfechafin { get; set; }    // Fecha Hora de Fin de la Intervención

        public int Tipoevencodi { get; set; } // Tipo de Intervención
        public int Subcausacodi { get; set; }  // Id sub causa de evento
        public int Claprocodi { get; set; }         // Clase de programación
        public string Interdescrip { get; set; }  // Descripción de la Intervención (Mantenimiento)

        public decimal Intermwindispo { get; set; } // MW Indisp.
        public string Interindispo { get; set; }   // Flag de Indisp.
        public string Interinterrup { get; set; }  // Flag de Interrupción
        public int Intermantrelev { get; set; }    // Flag de mantenimiento relevante
        public string Interconexionprov { get; set; }   // Flag de Conexion Provicional
        public string Intersistemaaislado { get; set; }  // Flag de Sistema Aislado      

        public DateTime? Interfecaprobrechaz { get; set; }  // Fecha de aprobación y rechazo
        public string Interjustifaprobrechaz { get; set; }  // Justificación u observación
        public int Interprocesado { get; set; }    // Flag de registro procesado.

        public string Interisfiles { get; set; }  // Flag indicador de archivos adjuntos
        public string Intermensaje { get; set; } // Flag indicador de mensaje
        public int Intermensajecoes { get; set; } // Flag indicador de mensaje
        public int Intermensajeagente { get; set; } // Flag indicador de mensaje
        public string Interrepetir { get; set; } // Flag indicador de repetición de registros

        public string Interregprevactivo { get; set; } // Flag indicador de registro activo
        public int Estadocodi { get; set; }  // Id de estado del registro de la intervención
        public int Intercreated { get; set; }  // Flag indicador de registro creado
        public int Interdeleted { get; set; }  // Flag indicador de registro borrado

        public int? Intercodipadre { get; set; } // Id recursivo de intervención
        public int? Intermanttocodi { get; set; }  // Id de la tabla Mantto
        public int? Interevenpadre { get; set; }  // Id recursivo de evento

        public string Interusucreacion { get; set; }  // Usuario que creo el registro
        public DateTime? Interfeccreacion { get; set; }  // Fecha de creación del registro
        public string Interusumodificacion { get; set; } // Usuario que modifico el registro
        public DateTime? Interfecmodificacion { get; set; } // Fecha de modificación del registro
        public string Interusuagrup { get; set; } // Fecha de modificación de agrupación desagrupación
        public DateTime? Interfecagrup { get; set; } // Fecha de modificación de agrupación desagrupación

        public int Interfuentestado { get; set; } //campo adicional para ver los estados de modificación

        //campos de PR25
        public string Intertipoindisp { get; set; }
        public decimal? Interpr { get; set; }
        public string Interasocproc { get; set; }

        public int Intercarpetafiles { get; set; }
        public string Interipagente { get; set; }

        public string Internota { get; set; }
        public int? Interflagsustento { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class InIntervencionDTO
    {
        public bool ExisteCambio { get; set; }

        public DateTime UltimaModificacionFecha { get; set; }
        public string FechaCambioAgenteDesc { get; set; }
        public string UsuarioCambioAgenteDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
        public string UltimaModificacionUsuarioAgrupDesc { get; set; }
        public string UltimaModificacionFecAgrupDesc { get; set; }

        public string InterfechainiDesc { get; set; } // Cadena de Fecha y hora de Inicio Formateada
        public string InterfechafinDesc { get; set; } // Cadena de Fecha y hora de Fin Formateada
        public string InterindispoDesc { get; set; }   // Flag de Indisp.
        public string InterinterrupDesc { get; set; }  // Flag de Interrupción

        public string InterconexionprovDesc { get; set; }  // Flag de Conexion Provicional
        public string IntersistemaaisladoDesc { get; set; }  // Flag de Sistema Aislado

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 11/10/2017: CAMPOS ADICIONALES CALCULADOS
        //--------------------------------------------------------------------------------
        //public int? Areacodi { get; set; } // Id de Area (Id de Ubicación)
        public int Famcodi { get; set; } // Id de Familia (Id Tipo de Equipo)
        public string TipoEvenDesc { get; set; } // Nombre de Tipo de evento (Nombre de tipo de intervención)
        public string EmprNomb { get; set; } // Nombre de Empresa
        public string AreaNomb { get; set; } // Nombre de Area (Nombre de Ubicación)
        public string FamNomb { get; set; } // Nombre de Familia (Nombre de Tipo de Equipo)
        public string EquiNomb { get; set; } // Nombre de Equipo
        public string Operadornomb { get; set; } //Nombre del operador del equipo

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 21/11/2017: CAMPOS ADICIONALES CALCULADOS
        //--------------------------------------------------------------------------------
        public string CodEqOsinerg { get; set; }  // Cod. Eq  COD.
        public string InterTeosinerg { get; set; }  // Flag de TE_Osinerg
        public string IntNombTipoProgramacion { get; set; }  // Clase (Tipo de Programacion)
        public string IntNombTipoIntervencion { get; set; }  // Tipo de Intervención (Tipo Evento o Mantenimiento)
        public string IntNombClaseProgramacion { get; set; }  // Clase de programación (Progr.)

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 06/12/2017: CAMPOS ADICIONALES CALCULADOS
        //--------------------------------------------------------------------------------
        public int NroItem { get; set; } // Indice para la importación de intervenciones

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 11/01/2018: CAMPOS ADICIONALES CALCULADOS
        //--------------------------------------------------------------------------------
        public int? Mes { get; set; } // Mes del año
        public int? Semana { get; set; } // Semana del Año-Mes
        public int? Dia { get; set; } // Dia al Año-Mes-Semana
        public string Clapronombre { get; set; } // Nombre de la clase de programación

        public int TipoComunicacion { get; set; }
        public bool ChkMensaje { get; set; }

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 22/01/2018: CAMPOS ADICIONALES CALCULADOS
        //--------------------------------------------------------------------------------      
        public string Famabrev { get; set; } // Nombre Abreviado de la familia o tipo de equipo
        public string Equiabrev { get; set; } // Nombre abreviado del equipo

        public int Tareacodi { get; set; } // Código de Tipo de Area

        public decimal Equipot { get; set; } // Potencia promedio del equipo        
        public string Equimaniobra { get; set; } //Flag indicador si el equipo esta en maniobras
        public string Actividad { get; set; } //Flag indicador Registrar/Actualizar         
        public bool ChkAprobacion { get; set; } //Flag que indica que tiene check de seleccion del listado

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 02/04/2018: CAMPOS PARAMETRICOS
        //--------------------------------------------------------------------------------   
        public int FDatCodi { get; set; } // Id de Fuente de Datos para poblar la tabla ME_ENVIO

        //---------------------------------------------------------------------------------------
        // ASSETEC.SGH - 09/04/2018: CAMPOS ADICIONALES CALCULADOS
        //---------------------------------------------------------------------------------------
        public string Clase { get; set; } // Clase (Flag Tipo programacion = S)
        public string Progr { get; set; }
        public string Tipoevenabrev { get; set; } // Nombre Abreviado de Tipo de Evento

        //---------------------------------------------------------------------------------------
        // ASSETEC.SGH - 18/04/2018: CAMPOS ADICIONALES CALCULADOS
        //---------------------------------------------------------------------------------------
        public int Duracion { get; set; }
        public string Observaciones { get; set; }
        public string Comentario { get; set; }

        public string ClaseProgramacion { get; set; }


        //Adicional 
        public string Areaabrev { get; set; }

        //---------------------------------------------------------------------------------------
        // ASSETEC.SGH - 29/08/2018: CAMPOS ADICIONALES CALCULADOS
        //---------------------------------------------------------------------------------------
        public int DiferenciaDias { get; set; }
        public int HoraFechaFin { get; set; }
        public int MinutoFechaFin { get; set; }

        //---------------------------------------------------------------------------------------
        // ASSETEC.SGH - 25/06/2019: CAMPO ADICIONAL PARA SER PRESENTADO EN MANTTO CONSUL. Y EXCEL CONSUL.
        //---------------------------------------------------------------------------------------
        public string Subcausadesc { get; set; }

        #region campos para Mantenimientos programados 7d
        public string TEOsinerg { get; set; }
        #endregion

        #region Indisponibilidades
        public int EventoGenerado { get; set; }
        public int Hopcodi { get; set; }
        public string Grupotipocogen { get; set; }
        #endregion

        //Para los tratar los checkboxes
        public bool ChkInterinterrup { get; set; }  //S o N
        public bool ChkIntermantrelev { get; set; }  // 1 o 0 
        public bool ChkInterconexionprov { get; set; }//S o N
        public bool ChkIntersistemaaislado { get; set; }//S o N

        #region Alertas Intervenciones

        public bool TieneAlertaHoraOperacion { get; set; }
        public bool TieneAlertaScada { get; set; }
        public bool TieneAlertaEms { get; set; }
        public bool TieneAlertaIDCC { get; set; }
        public bool TieneAlertaPR21 { get; set; }
        public bool TieneAlertaMedidores { get; set; }
        public bool TieneAlertaEstadoPendiente { get; set; }
        public int AlertaNoEjecutado { get; set; }

        #endregion

        public string IniFecha { get; set; }
        public string IniHora { get; set; }
        public string IniMinuto { get; set; }
        public string FinFecha { get; set; }
        public string FinHora { get; set; }
        public string FinMinuto { get; set; }

        public int Progrsololectura { get; set; }
        public int EstadoIntranet { get; set; }

        public bool EsContinuoFraccionado { get; set; } //Intervención única de varios días que se muestra fraccionado en la vista web (color azul)
        public bool EsConsecutivoRangoHora { get; set; } //Varias intervenciones que tienen el mismo equipo, tipo de intervención, descripción y consecutivo (color verde)

        public string DescripcionFormateado { get; set; }  // Descripción de la Intervención (Mantenimiento)
        public int Nrosublista { get; set; } //sublista que pertenece la intervencion consecutiva

        public string Areadesc { get; set; }
        public string Emprabrev { get; set; }
        public string Causaevenabrev { get; set; }
        public Nullable<decimal> Equitension { get; set; }
        public string Tipoemprdesc { get; set; }
        public string Osigrupocodi { get; set; }
        public int Tipoemprcodi { get; set; }
        public string Evenclasedesc { get; set; }
        public string Evenclaseabrev { get; set; }
        public int? Equipadre { get; set; }
        public decimal? Evenpr { get; set; }
        public string Evenasocproc { get; set; }
        public string EstadoRegistro { get; set; }

        //Copiar - mover Archivos
        public List<InArchivoDTO> ListaArchivo { get; set; }
        public bool EsCopiarArchivo { get; set; }

        public string CarpetaProgOrigenFS { get; set; }
        public string CarpetaProgDestinoFS { get; set; }
        public int CarpetafilesOrigenFS { get; set; }
        public int CarpetafilesDestinoFS { get; set; }

        public int CarpetaInTemporal { get; set; }

        public List<string> ListaCampo { get; set; } = new List<string>();
        public List<int> ListaCodigo { get; set; } = new List<int>();

        public int? Interleido { get; set; }
        public int? Estadopadre { get; set; }
        public string EstadoRegistroPadre { get; set; }

        //Sustento
        public InSustentoDTO Sustento { get; set; }
        public InSustentoDTO SustentoOld { get; set; }
        public string Nomprogramacion { get; set; }
        public bool EsEditable { get; set; }

        //Factor F1
        public int IntercodiPivote { get; set; }
    }
}
