using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
using fwapp;
using AjaxControlToolkit;

namespace WScoes
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IManttoService" in both code and config file together.
    [ServiceContract]
    public interface IManttoService
    {
        [OperationContract]
        int GetVersion();
        [OperationContract]
        int Register(string as_login, int ai_PassNumber);
        [OperationContract]
        DataTable GetDTData(string as_info, int ai_key);

        [OperationContract]
        DataTable GetMantto(int ai_mancodi);
        [OperationContract]
        DataTable GetArchivosMantto(int ai_mancodi);

        [OperationContract]
        DataTable GetMantto(string as_empresas, int ai_registro);

        [OperationContract]
        DataTable GetMantto(string as_empresas, int ai_registro, DateTime adt_EvenIni, DateTime adt_EvenFin);

        [OperationContract]
        DataTable GetManttoPrint(string as_empresas, int ai_registro);
        [OperationContract]
        int CopyMantto(string as_empresas, int ai_registro, int ai_evenclasecodi, DateTime adt_FechaInicial, DateTime adt_FechaFinal, string as_lastuser);
        [OperationContract]
        Dictionary<int, string> H_GetManttoRegistros(DateTime adt_inicio, DateTime adt_final);

        [OperationContract]
        CManttoRegistro GetManttoRegistro(int ai_regcodi);

        [OperationContract]
        int DeleteMantto(int ai_mancodi, string as_lastuser);

        [OperationContract]
        int UndoDeleteMantto(int ai_mancodi, string as_lastuser);
        
        //[OperationContract]
        //int UpdateMantto(int ai_mancodi, string as_SetFields, string as_lastuser);
        [OperationContract]
        int UpdateMantto(int ai_mancodi, DateTime adt_ini, DateTime adt_fin, string as_SetFields, string as_lastuser);
        [OperationContract]
        int UpdateMantto(int ai_mancodi, DateTime adt_ini, DateTime adt_fin, string as_evendescrip, string as_evenobsrv, string as_evenindispo,
            string as_tipoevencodi, string as_eventipoprog, string as_eveninterrup, string as_evenmwindisp, string as_lastuser);
        //[OperationContract]
        //int InsertMantto(int ai_registro, string as_fields, string as_values, string as_lastuser);
        [OperationContract]
        int InsertMantto(int ai_registro, DateTime adt_ini, DateTime adt_fin, string as_fields, string as_values, string as_lastuser);
        [OperationContract]
        int InsertReferenciaArchivoMantto(int ai_mancodi, string as_fields, string as_values, string as_lastuser);
        [OperationContract]
        int DeleteReferenciaArchivoMantto(int ai_mancodi, int ai_numarchivo, string as_lastuser);
        [OperationContract]
        int CreateFechaLim(int ai_regcodi, DateTime adt_fechalim, string as_lastuser);
        [OperationContract]
        int CreateRegMan(DateTime adt_fechini, string as_regnomb, int ai_evenclasecodi, string as_lastuser);
        [OperationContract]
        int Create_EnvioArchivo(int ai_etacodi, int ai_emprcodi, string as_arch_nomb, double ad_arch_tam, string as_arch_vers, string as_arch_ruta, int ai_usercode, string as_user_ip, string as_last_user, DateTime ad_fecha, int ai_estad_env, string as_copiado);
        [OperationContract]
        bool ValidateRegMan(DateTime adt_fechini, int ai_evenclasecodi);
        [OperationContract]
        List<int> L_GetCodiEquipos(string as_empresas);
        [OperationContract]
        CascadingDropDownNameValue[] GetEmpresas(string knownCategoryValues, string category,string contextKey);
        [OperationContract]
        CascadingDropDownNameValue[] GetAreasPorEmpresa(string knownCategoryValues, string category);
        [OperationContract]
        CascadingDropDownNameValue[] GetEquiposPorArea(string knownCategoryValues, string category);

        [OperationContract]
        DataTable H_GetManttosRegistros(DateTime adt_inicio, DateTime adt_final);

        //[OperationContract]
        //DataTable H_GetManttoRegistrosXTipo(DateTime adt_inicio, DateTime adt_final, int ai_tipoPrograma);

        [OperationContract]
        Dictionary<int, string> H_GetManttoRegistrosXTipo(DateTime adt_inicio, DateTime adt_final, int ai_tipoPrograma);

        [OperationContract]
        bool GetEquipoValido(string ps_empresas, int pi_equicodi, out string ps_mensaje);

        [OperationContract]
        DateTime GetFechaLimitePrograma(DateTime pdt_fechaInicial, int pi_codigoPrograma);

    }

    [DataContract]
    public class CManttoRegistro
    {
        private int ii_regcodi = -1;
        [DataMember]
        public int RegCodi
        {
            get { return ii_regcodi; }
            set { ii_regcodi = value; }
        }

        private int ii_evenclasecodi = -1;
        [DataMember]
        public int EvenClaseCodi
        {
            get { return ii_evenclasecodi; }
            set { ii_evenclasecodi = value; }
        }

        private string is_evenclaseabrev = "";
        [DataMember]
        public string EvenClaseAbrev
        {
            get { return is_evenclaseabrev; }
            set { is_evenclaseabrev = value.Trim(); }
        }

        private string is_evenclasedesc = "";
        [DataMember]
        public string EvenClaseDesc
        {
            get { return is_evenclasedesc; }
            set { is_evenclasedesc = value; }
        }

        private DateTime idt_FechaInicial = new DateTime(2001, 1, 1, 0, 0, 0);
        [DataMember]
        public DateTime FechaInicial
        {
            get { return idt_FechaInicial; }
            set { idt_FechaInicial = value; }
        }

        private DateTime idt_FechaFinal = new DateTime(2001, 1, 1, 0, 0, 0);
        [DataMember]
        public DateTime FechaFinal
        {
            get { return idt_FechaFinal; }
            set { idt_FechaFinal = value; }
        }

        private string is_regnomb = "";
        [DataMember]
        public string RegistroNombre
        {
            get { return is_regnomb; }
            set { is_regnomb = value; }
        }

        private DateTime idt_FechaLimiteInicial = new DateTime(2001, 1, 1, 0, 0, 0);
        [DataMember]
        public DateTime FechaLimiteInicial
        {
            get { return idt_FechaLimiteInicial; }
            set { idt_FechaLimiteInicial = value; }
        }

        private DateTime idt_FechaLimiteFinal = new DateTime(2001, 1, 1, 0, 0, 0);
        [DataMember]
        public DateTime FechaLimiteFinal
        {
            get { return idt_FechaLimiteFinal; }
            set { idt_FechaLimiteFinal = value; }
        }
    }

   

    [DataContract] 
    public class CEquipo
    {
        public int ii_equicodi = -1;
        public int ii_equipadre = -1;
        public string is_equiabrev = "";
        public string is_equinomb = "";
        public CGrupo in_grupo = null;
        public double id_PotenciaMaxima = 0;
        public double id_PotenciaMinima = 0;
        public int ii_tminopera = 0;
        public int ii_tminparada = 0;
                
        public CEquipo(int ai_equicodi, int ai_equipadre, string as_equiabrev, string as_equinomb)
        {
            ii_equicodi = ai_equicodi;
            ii_equipadre = ai_equipadre;
            is_equiabrev = as_equiabrev;
            is_equinomb = as_equinomb;
        }
    }

    public class CEquipoDemanda
    {
        public int ii_equicodi = -1;
        public string is_equiabrev = "";
        public string is_equinomb = "";
        public int ii_famcodi = -1;
        public double id_equitension = 0;
        public int ii_areacodi = 0;
        public int ii_emprcodi = 0;

        public CEquipoDemanda()
        { }

        public CEquipoDemanda(int ai_equicodi, string as_equiabrev, string as_equinomb, int ai_famcodi, double ad_equitension, int ai_areacodi, int ai_emprcodi)
        {
            ii_equicodi = ai_equicodi;
            is_equiabrev = as_equiabrev;
            is_equinomb = as_equinomb;
            ii_famcodi = ai_famcodi;
            id_equitension = ad_equitension;
            ii_areacodi = ai_areacodi;
            ii_emprcodi = ai_emprcodi;
        }
    }



    public class CEmpresa
    {
        public int ii_emprcodi;
        public string is_emprnomb;
        public string is_emprabrev;
        public bool ib_IsCOES = false;
        public List<CGrupo> L_Grupo = new List<CGrupo>();
        public List<CCaldero> L_Caldero = new List<CCaldero>();

        public CEmpresa(int ai_emprcodi, string as_emprnomb, string as_emprabrev, bool ab_IsCOES)
        {
            ii_emprcodi = ai_emprcodi;
            is_emprnomb = as_emprnomb;
            is_emprabrev = as_emprabrev;
            ib_IsCOES = ab_IsCOES;
        }
        public override string ToString()
        {
            return is_emprnomb;
        }
    }


    public enum TipoGeneracion { Hidraulico, Termico };
    public enum TipoEquipo { _NoDefinido = -1, Central, Grupo, GrupoFunc, Caldero };

    public class CElemento
    {
        public TipoEquipo i_TipoEquipo;
        public int ii_grupocodi = -1;
        public string is_gruponomb;
        public string is_grupoabrev;
        public int ii_grupopadrecodi = -1;
        public string is_grupoactivo = "S";
        public override string ToString()
        {
            return is_gruponomb;
        }

        //[CategoryAttribute("ID Settings"), DescriptionAttribute("Nombre de Equipo")]
        //public string Nombre
        //{
        //   get
        //   {
        //      return is_gruponomb;
        //   }
        //   set
        //   {
        //      is_gruponomb = value;
        //   }
        //}      
    }

    public class CGrupoFunc : CElemento
    {
        //public string tcomb;
        //public TipoGeneracion i_tipogeneracion = TipoGeneracion.Termico;
        //public int CCindex;//ciclo combinado
        //public double cv, a, b, c, pmax, pmin, ccomb; //estas propiedades pueden variar por cada dia    
        public CGrupo in_grupo;
        public string ls_grupocomb;
        public CGrupoFunc(int ai_grupofuncodi, string as_gruponomb, string as_grupoabrev, int ai_grupocodi, int ai_barracodi, int ai_grupoorden, string as_grupoactivo, string as_grupocomb)
        {
            //id_idfunc = ai_idfunc
            ii_grupocodi = ai_grupofuncodi;
            is_gruponomb = as_gruponomb.Trim();
            is_grupoabrev = as_grupoabrev.Trim();
            ii_grupopadrecodi = ai_grupocodi;
            //id = ai_id;         
            //a = b = c = pmin = ccomb = 0;
            //tcomb = 0; //1-diesel/residual ; 2-gas ; 3-carbon
            //CCindex = 0;
            ////ip_grupopadre = null;        
        }
    }


    public class CCentral : CElemento
    {
        public TipoGeneracion i_tipogeneracion;
        public List<CGrupo> L_Grupo = new List<CGrupo>();
        public CCentral(int ai_grupocodi, string as_gruponomb, string as_grupoabrev, TipoGeneracion a_tipogen)
        {
            ii_grupocodi = ai_grupocodi;
            is_gruponomb = as_gruponomb.Trim();
            is_grupoabrev = as_grupoabrev.Trim();
            i_tipogeneracion = a_tipogen;
        }
    }

    public class CCaldero : CElemento
    {
        public CEmpresa in_empresa;
        public CCentral in_central;
        public string is_grupocomb;

        public CCaldero(int ai_grupocodi, string as_gruponomb, string as_grupoabrev)//, CEmpresa an_empresa, CCentral an_central, int ai_barracodi, int ai_grupoorden, string as_grupoactivo, string as_grupocomb)
        {
            i_TipoEquipo = TipoEquipo.Caldero;
            ii_grupocodi = ai_grupocodi;
            is_gruponomb = as_gruponomb.Trim();
            is_grupoabrev = as_grupoabrev.Trim();
        }
    }

    public class CGrupo : CElemento
    {
        public CEmpresa in_empresa;
        public CCentral in_central;
        public string is_gruponombNCP = "";
        //public double id_vmax;
        //public double id_vmin;
        public int ii_barracodi = -1;    //barra 1 
        public double id_barramw1 = 0;
        public double id_barramw2 = 0;   //para distribucion de la carga de la central a las barras barracodi y barracodi2
        public int ii_barracodi2 = -1;   //barra 2 - para centrales hidraulicas por ahora 

        public string is_barraabrev;
        public string is_barraabrev2;
        public int ii_grupoorden;
        public int ii_ptomedicodi = -1; //codigo de medicion

        public string is_grupocomb;
        public int ii_grupotipo = -1; //1=Integrantes, 2=Renovables 3=CoGeneracion, 10=NoIntegrantes


        public string is_grupoMWsumSCADA; //listado de canalcodi que suman su generacion 
        public string is_grupoMVARsumSCADA; //listado de canalcodi que suman su generacion 


        public double id_PotenciaMaxima = 0;

        public int ii_TMinOpera = 0;
        public int ii_TMinParada = 0;

        public CGrupoFunc i_grupofunc_default = null;
        public List<CGrupoFunc> L_GrupoFunc = new List<CGrupoFunc>();
        public List<CEquipo> L_Equipo = new List<CEquipo>();
        ////public Cgrupo ip_grupoCC = null; //grupo relacionado de CC

        public CGrupo(int ai_grupocodi, string as_gruponomb, string as_grupoabrev)//, CEmpresa an_empresa, CCentral an_central, int ai_barracodi, int ai_grupoorden, string as_grupoactivo, string as_grupocomb)
        {
            i_TipoEquipo = TipoEquipo.Grupo;
            ii_grupocodi = ai_grupocodi;
            is_gruponomb = as_gruponomb.Trim();
            is_grupoabrev = as_grupoabrev.Trim();
       
        }       
    }

    
}
