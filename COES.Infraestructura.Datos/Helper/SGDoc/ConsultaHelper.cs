using COES.Base.Core;
using COES.Dominio.DTO.SGDoc;
using COES.Framework.Base.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.SGDoc
{
    public class ConsultaHelper : HelperBase
    {
        public ConsultaHelper()
            : base(Consultas.ConsultaSGDocSql)
        {
        }

        public string SqlVerAtencion
        {
            get { return base.GetSqlXml("VerAtencion"); }
        }

        public string SqlSeguimientoArea
        {
            get { return base.GetSqlXml("SeguimientoArea"); }
        }

        public string SqlSeguimientoEspecialista
        {
            get { return base.GetSqlXml("SeguimientoEspecialista"); }
        }

        public string SqlVerSello
        {
            get { return base.GetSqlXml("VerSello"); }
        }

        public string SqlVerReferenciaDe
        {
            get { return base.GetSqlXml("VerReferenciaDe"); }
        }

        public string SqlVerReferenciaA
        {
            get { return base.GetSqlXml("VerReferenciaA"); }
        }

        public string SqlVerMensaje
        {
            get { return base.GetSqlXml("VerMensajes"); }
        }

        public string SqlVerDocumentos
        {
            get { return base.GetSqlXml("VerDocumentos"); }
        }

        public string SqlVerDocumentosV
        {
            get { return base.GetSqlXml("VerDocumentosV"); }
        }

        public string SqlObtenerArea
        {
            get { return base.GetSqlXml("ObtenerArea"); }
        }

        public string SqlListarAreas
        {
            get { return base.GetSqlXml("ListarAreas"); }
        }

        public string SqlRolesxUsuario
        {
            get { return base.GetSqlXml("RolesxUsuario"); }
        }

        public string SqlLeerEtiquetas
        {
            get { return base.GetSqlXml("LeerEtiquetas"); }
        }

        public string SqlListarEtiquetasPorArea
        {
            get { return base.GetSqlXml("ListarEtiquetasPorArea"); }
        }

        public string SqlLeerTipoAtencion
        {
            get { return base.GetSqlXml("LeerTipoAtencion"); }
        }

        public string SqlVerificarUserRol
        {
            get { return base.GetSqlXml("VerificarUserRol"); }
        }

        public string SqlObtenerDiasFeriados
        {
            get { return base.GetSqlXml("ObtenerDiasFeriados"); }
        }

        public string SqlLeerDirectorioRoot
        {
            get { return base.GetSqlXml("LeerDirectorioRoot"); }
        }

        public string SqlLeerPathArchivo
        {
            get { return base.GetSqlXml("SqlLeerPathArchivo"); }
        }

        public string SqlListarEmpresasNoRI
        {
            get { return base.GetSqlXml("ListarEmpresasNoRI"); }
        }

        public string SqlListarMaestroEmpresas
        {
            get { return base.GetSqlXml("ListarMaestroEmpresas"); }
        }

        public string SqlListarEmpresasRIPorTipo
        {
            get { return base.GetSqlXml("ListarEmpresasRIPorTipo"); }
        }

        public string SqlObtenerQueryMigracion
        {
            get { return base.GetSqlXml("ObtenerQueryMigracion"); }
        }

        public string SqlActualizarDatosMigracion
        {
            get { return base.GetSqlXml("ActualizarDatosMigracion"); }
        }

        public string ObtenerEstado(string estado)
        {
            switch (estado.ToUpper())
            {
                case "A":
                    return "Atendido";
                case "P":
                    return "Pendiente";
                case "X":
                    return "Anulado";
                case "B":
                    return "Borrado";
                case "C":
                    return "Archivado";
                case "I":
                    return "Indeterminado";
                case "N":
                    return "Nuevo";
                case "D":
                    return "Derivado";
                default:
                    return "Indeterminado";
            }

        }

        public string ObtenerAtencion(string atencion)
        {
            char[] arreglo = atencion.ToCharArray();
            string desAtencion = string.Empty;

            if (arreglo.Length == 8)
            {
                if (arreglo[0] == '1') { desAtencion += " Prpta"; }
                if (arreglo[1] == '1') { desAtencion += " Opinar"; }
                if (arreglo[2] == '1') { desAtencion += " Revisar"; }
                if (arreglo[3] == '1') { desAtencion += " Coordinar"; }
                if (arreglo[4] == '1') { desAtencion += " Atender"; }
                if (arreglo[5] == '1') { desAtencion += " Informar"; }
                if (arreglo[6] == '1') { desAtencion += " Conocimiento"; }
                if (arreglo[7] == '1') { desAtencion += " Archivar"; }
            }

            return desAtencion;
        }
    }

    public class BandejaHelper
    {
        public string Fljcodi = "Fljcodi";
        public string Corrnumproc = "Corrnumproc";
        public string Nombarearem = "Nombarearem";
        public string Areaorig = "Areaorig";
        public string Fljnumext = "Fljnumext";
        public string Asunto = "Asunto";
        public string Frecepcion = "Frecepcion";
        public string Xfileruta = "Xfileruta";
        public string Filecodi = "Filecodi";
        public string Fljremdetalle = "Fljremdetalle";
        public string Fljestado = "Fljestado";
        public string Fljdetcodi = "Fljdetcodi";
        public string Nombdestino = "Nombdestino";
        public string Fljdetdestino = "Fljdetdestino";
        public string Fljdetorigen = "Fljdetorigen";
        public string Fljdetnivel = "Fljdetnivel";
        public string Fljcadatencion = "Fljcadatencion";
        public string Fljpadrecomentario = "Fljpadrecomentario";
        public string Fljfechaproce = "Fljfechaproce";
        public string Areadestino = "Areadestino";
        public string Conplazo = "Conplazo";
        public string Fmax = "Fmax";
        public string Archcodificacion = "Archcodificacion";
        public string Archsubcodif = "Archsubcodif";
        public string Nombemprem = "Nombemprem";
        public string Fljconf = "Fljconf";
        public string Fljanio = "Fljanio";
        public string Prioridad = "Prioridad";
        public string Fljfileatrib = "Fljfileatrib";
        public string Rpta = "Rpta";
        public string Leido = "Leido";
        public string Nmsg = "Nmsg";
        public string Estadohijo = "Estadohijo";
        public string Fatencion = "Fatencion";
        public string Imp = "Imp";
        public string Usercode = "usercode";
        public string Docrolcodi = "docrolcodi";

            
        public BandejaDTO Create(IDataReader dr, int origen)
        {
            BandejaDTO entity = new BandejaDTO();

            int iFljcodi = dr.GetOrdinal(this.Fljcodi);
            if (!dr.IsDBNull(iFljcodi)) entity.CodRegistro = Convert.ToInt32(dr.GetValue(iFljcodi));

            int iCorrnumproc = dr.GetOrdinal(this.Corrnumproc);
            if (!dr.IsDBNull(iCorrnumproc)) entity.Correlativo = dr.GetDecimal(iCorrnumproc);

            int iNombarearem = dr.GetOrdinal(this.Nombarearem);
            if (!dr.IsDBNull(iNombarearem)) entity.NombAreaOrigen = dr.GetString(iNombarearem);

            int iAreaorig = dr.GetOrdinal(this.Areaorig);
            if (!dr.IsDBNull(iAreaorig)) entity.Areaorig = Convert.ToInt32(dr.GetValue(iAreaorig));

            int iFljnumext = dr.GetOrdinal(this.Fljnumext);
            if (!dr.IsDBNull(iFljnumext)) entity.NumDocumento = dr.GetString(iFljnumext);

            int iAsunto = dr.GetOrdinal(this.Asunto);
            if (!dr.IsDBNull(iAsunto)) entity.Asunto = dr.GetString(iAsunto);

            int iFrecepcion = dr.GetOrdinal(this.Frecepcion);
            if (!dr.IsDBNull(iFrecepcion)) entity.FechaLlegadaCoes = dr.GetDateTime(iFrecepcion);

            int iXfileruta = dr.GetOrdinal(this.Xfileruta);
            if (!dr.IsDBNull(iXfileruta)) entity.Xfileruta = dr.GetString(iXfileruta);

            int iFilecodi = dr.GetOrdinal(this.Filecodi);
            if (!dr.IsDBNull(iFilecodi)) entity.Filecodi = Convert.ToInt32(dr.GetValue(iFilecodi));

            int iFljremdetalle = dr.GetOrdinal(this.Fljremdetalle);
            if (!dr.IsDBNull(iFljremdetalle)) entity.Fljremdetalle = dr.GetString(iFljremdetalle);

            string estado = string.Empty;
            int iFljestado = dr.GetOrdinal(this.Fljestado);
            if (!dr.IsDBNull(iFljestado)) estado = dr.GetString(iFljestado);

            entity.Estado = this.ObtenerEstado(estado);

            int iFljdetcodi = dr.GetOrdinal(this.Fljdetcodi);
            if (!dr.IsDBNull(iFljdetcodi)) entity.CodDetregistro = Convert.ToInt32(dr.GetValue(iFljdetcodi));

            int iNombdestino = dr.GetOrdinal(this.Nombdestino);
            if (!dr.IsDBNull(iNombdestino)) entity.NombAreaDestino = dr.GetString(iNombdestino);

            int iFljdetdestino = dr.GetOrdinal(this.Fljdetdestino);
            if (!dr.IsDBNull(iFljdetdestino)) entity.Fljdetdestino = Convert.ToInt32(dr.GetValue(iFljdetdestino));

            int iFljdetorigen = dr.GetOrdinal(this.Fljdetorigen);
            if (!dr.IsDBNull(iFljdetorigen)) entity.Fljdetorigen = Convert.ToInt32(dr.GetValue(iFljdetorigen));

            int iFljdetnivel = dr.GetOrdinal(this.Fljdetnivel);
            if (!dr.IsDBNull(iFljdetnivel)) entity.Fljdetnivel = Convert.ToInt32(dr.GetValue(iFljdetnivel));


            string atencion = string.Empty;

            int iFljcadatencion = dr.GetOrdinal(this.Fljcadatencion);
            if (!dr.IsDBNull(iFljcadatencion)) atencion= dr.GetString(iFljcadatencion);

            entity.Fljcadatencion = this.ObtenerAtencion(atencion);
            entity.Codatencion = atencion;

            int iFljpadrecomentario = dr.GetOrdinal(this.Fljpadrecomentario);
            if (!dr.IsDBNull(iFljpadrecomentario)) entity.ComentarioPadre = dr.GetString(iFljpadrecomentario);

            int iFljfechaproce = dr.GetOrdinal(this.Fljfechaproce);
            if (!dr.IsDBNull(iFljfechaproce)) entity.Fljfechaproce = dr.GetDateTime(iFljfechaproce);

            int iAreadestino = dr.GetOrdinal(this.Areadestino);
            if (!dr.IsDBNull(iAreadestino)) entity.Area = Convert.ToInt32(dr.GetValue(iAreadestino));

            int iConplazo = dr.GetOrdinal(this.Conplazo);
            if (!dr.IsDBNull(iConplazo)) entity.Conplazo = Convert.ToInt32(dr.GetValue(iConplazo));

            int iFmax = dr.GetOrdinal(this.Fmax);
            entity.TiempoMaxAtencion = false;

            if (!dr.IsDBNull(iFmax))
            {
                entity.TiempoMaxAtencion = true;
                entity.FechaMaxAtencion = dr.GetDateTime(iFmax);
            }

            int iArchcodificacion = dr.GetOrdinal(this.Archcodificacion);
            if (!dr.IsDBNull(iArchcodificacion)) entity.ArchivoAdm = dr.GetString(iArchcodificacion);

            int iArchsubcodif = dr.GetOrdinal(this.Archsubcodif);
            if (!dr.IsDBNull(iArchsubcodif)) entity.ArchivoAdmSub = dr.GetString(iArchsubcodif);

            int iNombemprem = dr.GetOrdinal(this.Nombemprem);
            if (!dr.IsDBNull(iNombemprem)) entity.NombreEmpresaRem = dr.GetString(iNombemprem);

            int iFljconf = dr.GetOrdinal(this.Fljconf);
            if (!dr.IsDBNull(iFljconf)) entity.Confidencial = dr.GetString(iFljconf);

            int iFljanio = dr.GetOrdinal(this.Fljanio);
            if (!dr.IsDBNull(iFljanio)) entity.AnioLibro = Convert.ToInt32(dr.GetValue(iFljanio));

            int iPrioridad = dr.GetOrdinal(this.Prioridad);
            if (!dr.IsDBNull(iPrioridad)) entity.Prioridad = Convert.ToInt32(dr.GetValue(iPrioridad));

            int iFljfileatrib = dr.GetOrdinal(this.Fljfileatrib);
            if (!dr.IsDBNull(iFljfileatrib)) entity.Atributos = Convert.ToInt32(dr.GetValue(iFljfileatrib));

            int iRpta = dr.GetOrdinal(this.Rpta);
            if (!dr.IsDBNull(iRpta)) entity.Rpta = dr.GetString(iRpta);

            int iLeido = dr.GetOrdinal(this.Leido);
            if (!dr.IsDBNull(iLeido)) entity.Leido = dr.GetString(iLeido);

            int iNmsg = dr.GetOrdinal(this.Nmsg);
            if (!dr.IsDBNull(iNmsg)) entity.Nmsg = Convert.ToInt32(dr.GetValue(iNmsg));

            int iEstadohijo = dr.GetOrdinal(this.Estadohijo);
            if (!dr.IsDBNull(iEstadohijo)) entity.Estadohijo = dr.GetString(iEstadohijo);

            int iFatencion = dr.GetOrdinal(this.Fatencion);
            if (!dr.IsDBNull(iFatencion)) entity.FechaAtencion = dr.GetDateTime(iFatencion);

            int iImp = dr.GetOrdinal(this.Imp);
            if (!dr.IsDBNull(iImp)) entity.Importancia = dr.GetString(iImp);

            entity.EnlaceArchivo = "http://sicoes.coes.org.pe/sgdocenc/show.aspx?at=" + Encriptacion.Encripta(entity.Filecodi.ToString());
            entity.EnlaceArchivo = entity.EnlaceArchivo.Replace("+", "%2b");

            if (origen == 0)
            {
                entity.FechaIngreso = entity.FechaLlegadaCoes;
            }
            else
            {
                entity.FechaIngreso = entity.Fljfechaproce;
            }

            if (entity.Confidencial == "S") { entity.Cursiva = "S"; }
            else { entity.Cursiva = "N"; }

            entity.ColorLetra = null;

            if (entity.TiempoMaxAtencion && (estado != "A") && (estado != "X"))
            {                
                ColorAlarma(ref entity);                
            }
            else
            {
                entity.ColorFondo = ColorEstado(estado);                
            }
            

            //falta filtro de alarma
            if (entity.Fljcadatencion != "00000000" &&
                    (estado == "P" || estado == "D") &&
                    !entity.TiempoMaxAtencion &&
                    (entity.FechaLlegadaCoes.Date.AddDays(15) < DateTime.Now.Date) &&
                    (entity.FechaLlegadaCoes.Date.AddDays(365) > DateTime.Now.Date))
            {
                char[] lc_cadena = atencion.ToCharArray();
                if (lc_cadena.Length > 0)
                {
                    if ((lc_cadena[6] != '1' && lc_cadena[7] != '1') || lc_cadena[0] == '1' || lc_cadena[1] == '1' || lc_cadena[2] == '1' || lc_cadena[3] == '1' || lc_cadena[4] == '1' || lc_cadena[5] == '1') // Diferente de conocimiento y Archivar                    
                    {
                        entity.ColorLetra = "#FF0000";
                        entity.ColorFondo = "#FFFFE6";
                    }
                }               
            }

            /**             
             * 
             * 
             * 
            fecha_asignacion = Convert.ToDateTime(ln_grilla.Rows[e.RowIndex].Cells["fechaLlegadaCoes"].Value);
            ls_accion = ln_grilla.Rows[e.RowIndex].Cells["_is_fljcadatencion"].Value.ToString();

            if ((ls_accion != "00000000") && (ls_estado == "P" || ls_estado == "D") && (!lb_alarma) && (fecha_asignacion.Date.AddDays(15) < DateTime.Now.Date) && (fecha_asignacion.Date.AddDays(365) > DateTime.Now.Date))//Pendiente
            {

                lc_cadena = ls_accion.ToCharArray();
                if (lc_cadena.Length > 0)
                {
                    if ((lc_cadena[6] != '1' && lc_cadena[7] != '1') || lc_cadena[0] == '1' || lc_cadena[1] == '1' || lc_cadena[2] == '1' || lc_cadena[3] == '1' || lc_cadena[4] == '1' || lc_cadena[5] == '1') // Diferente de conocimiento y Archivar                    
                    {
                        ln_grilla.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        ln_grilla.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 230); // Color.LightYellow;
                    }
                }
            }

             */


            return entity;
        }

        public  string ObtenerEstado(string estado)
        {
            switch (estado.ToUpper())
            {
                case "A":
                    return "Atendido";
                case "P":
                    return "Pendiente";
                case "X":
                    return "Anulado";
                case "B":
                    return "Borrado";
                case "C":
                    return "Archivado";
                case "I":
                    return "Indeterminado";
                case "N":
                    return "Nuevo";
                case "D":
                    return "Derivado";
                default:
                    return "Indeterminado";
            }

        }

        private string ObtenerAtencion(string atencion)
        {
            char[] arreglo = atencion.ToCharArray();
            string desAtencion = string.Empty;

            if (arreglo.Length == 8)
            {
                if (arreglo[0] == '1') { desAtencion += " Prpta"; }
                if (arreglo[1] == '1') { desAtencion += " Opinar"; }
                if (arreglo[2] == '1') { desAtencion += " Revisar"; }
                if (arreglo[3] == '1') { desAtencion += " Coordinar"; }
                if (arreglo[4] == '1') { desAtencion += " Atender"; }
                if (arreglo[5] == '1') { desAtencion += " Informar"; }
                if (arreglo[6] == '1') { desAtencion += " Conocimiento"; }
                if (arreglo[7] == '1') { desAtencion += " Archivar"; }
            }

            return desAtencion;
        }

        private void ColorAlarma(ref BandejaDTO entity)
        {
            int tiempo;

            if (entity.FechaMaxAtencion != null)
            {

                tiempo = ((DateTime)entity.FechaMaxAtencion).Date.CompareTo(DateTime.Now.Date);

                if (tiempo < 0) //Menos que el plazo --Naranja
                {
                    entity.ColorFondo = "#FCC963"; //Color.FromArgb(252, 201, 99);
                    entity.Bold = false;
                }
                else if (tiempo == 0) //Igual al plazo --Rojo
                {
                    entity.ColorFondo = "#DE4043"; //Color.FromArgb(222, 64, 67);//(214, 40, 40);                
                    entity.Bold = true;
                }
                else//Mayor al plazo --Verde
                {
                    entity.ColorFondo = "#B5CB95"; //(0, 173, 96); // 51, 158, 53  0,172,122
                    entity.Bold = false;
                }
            }
        }

        private string ColorEstado(string estado)
        {
            if (estado == "D")//Derivado
            {
                return "#BFDFFA"; //ln_color = Color.FromArgb(191, 223, 250);
            }
            else if (estado == "A")//Atendido
            {
                return "#E9E9E9"; //ln_color = Color.FromArgb(233, 233, 233);
            }
            else if (estado == "X")//Anulado
            {
                //ln_color = Color.Linen;
                return "#faf0e6";
            }
            else if (estado == "B")//Borrado
            {
                //ln_color = Color.FromArgb(205, 85, 63);
                return "#CD553F";
            }
            else if (estado == "C") //Archivado
            {
                //ln_color = Color.FromArgb(245, 92, 37);
                return "#F55C25";
            }
            else if (estado == "N") //Para la mesa de partes
            {
                //ln_color = Color.FromArgb(230, 245, 80);
                return "#E6F550";
            }
            else
            {
                //ln_color = Color.White;
                return "#FFFFFF";
            }
        }
    }

    public class AtencionHelper
    {
        public string Fljdetcodi = "fljdetcodi";
        public string FechaMsg = "fljfechamsg";
        public string NombAreaOrig = "nombareaorig";
        public string NombAreaDest = "nombareadest";
        public string Msg = "fljmsg";
        public string Estado = "fljmsgestado";
        public string Fileruta = "filerutax";
        public string Fljfileruta = "fljfileruta";
    }

    public class SeguimientoHelper
    {
        public string NumMsg = "nmsg";
        public string Prioridad = "prioridad";
        public string FechaMax = "fljfechamax";
        public string DescripAten = "tatdesc";
        public string FljCodigo = "fljcodi";
        public string NombreAreaPadre = "origen";
        public string FljDetCodigo = "fljdetcodi";
        public string NombreArea = "areaabrev";
        public string FechaAsignacion = "fljfechainicio";
        public string Estado = "fljestado";
        public string ComentarioPadre = "descripdeleg";
        public string FechaAtencion = "fljfechaatencion";
        public string CodigoArea = "areacodeorig";
        public string CodigoAreaPadre = "areapadre";
        public string FljNivel = "fljdetnivel";
        public string FljDetOrigen = "fljdetorigen";
        public string FljDetDestino = "fljdetdestino";
        public string Areacode = "areacode";
    }

    public class SelloHelper
    {
        public string FljCadAtencion = "fljcadatencion";
        public string AreaPadre = "areapadre";
        public string AreaCode = "areacode";
        public string DescripDeleg = "descripdeleg";
        public string FljFechaMax = "fljfechamax";
        public string Fljcodi = "fljcodi";
    }

    public class ReferenciaHelper
    {
        public string Correlativo = "corrnumproc";
        public string FechaDoc = "fljfechaorig";
        public string NDoc = "fljnumext";
        public string Asunto = "fljnombre";
        public string RutaArchivo = "xfileruta";
        public string Fljcodi = "fljcodi";
    }

    public class MensajeHelper
    {
        public string FechaMsg = "fljfechamsg";
        public string NombAreaOrig = "nombareaorig";
        public string NombAreaDest = "nombareadest";
        public string Msg = "fljmsg";
        public string Estado = "fljmsgestado";
        public string Fileruta = "filerutax";
        public string Fljdetcodi = "fljdetcodi";
        public string Fljfileruta = "fljfileruta";
    }

    public class DocumentoHelper
    {
        public string Filecodi = "filecodi";
        public string Fileruta = "xfileruta";
        public string Lastdate = "lastdate";
        public string Lastuser = "lastuser";
        public string Filecomentario = "filecomentario";
        public string Fileanio = "fileanio";
        public string Fljcodi = "fljcodi";
        public string Fljdetcodi = "fljdetcodi";
    }

    public class AreaHelper
    {
        public string AreaCodi = "areacode";
        public string AreaName = "areaname";
    }

    public class RolUsuarioHelper
    {
        public string RolCodi = "docrolcodi";
        public string UserCode = "usercode";
    }

    public class EtiquetaHelper
    {
        public string EtiqCode = "etiqcode";
        public string EtiqNomb = "etiqnomb";
        public string AreaCode = "areacode";
        public string BandejaLvl = "bandejalvl";
    }

    public class ReporteHelper
    {
        public string Fljcodi = "fljcodi";
        public string Fljdetcodi = "fljdetcodi";
        public string Corrnumproc = "corrnumproc";
        public string Remitente = "remitente";
        public string Destino = "destino";
        public string Areaorig = "areaorig";
        public string Areadestino = "areadestino";
        public string Fljnumext = "fljnumext";
        public string Asunto = "asunto";
        public string Frecepcion = "frecepcion";
        public string Xfileruta = "xfileruta";
        public string Fljestado = "fljestado";
        public string Fljcadatencion = "fljcadatencion";
        public string Fljpadrecomentario = "fljpadrecomentario";
        public string Fljfechaproce = "fljfechaproce";
        public string Fmax = "fmax";
        public string Fasignacion = "fasignacion";
        public string Fljdiasmaxaten = "fljdiasmaxaten";
        public string Fmaxgen = "fmaxgen";

        public ReporteDTO Create(IDataReader dr)
        {
            ReporteDTO entity = new ReporteDTO();

            int iFljcodi = dr.GetOrdinal(this.Fljcodi);
            if (!dr.IsDBNull(iFljcodi)) entity.Fljcodi = Convert.ToInt32(dr.GetValue(iFljcodi));

            int iFljdetcodi = dr.GetOrdinal(this.Fljdetcodi);
            if (!dr.IsDBNull(iFljdetcodi)) entity.Fljdetcodi = Convert.ToInt32(dr.GetValue(iFljdetcodi));

            int iCorrnumproc = dr.GetOrdinal(this.Corrnumproc);
            if (!dr.IsDBNull(iCorrnumproc)) entity.Correlativo = Convert.ToInt32(dr.GetValue(iCorrnumproc));

            int iRemitente = dr.GetOrdinal(this.Remitente);
            if (!dr.IsDBNull(iRemitente)) entity.Remitente = dr.GetString(iRemitente);

            int iDestino = dr.GetOrdinal(this.Destino);
            if (!dr.IsDBNull(iDestino)) entity.Destino = dr.GetString(iDestino);

            int iAreaorig = dr.GetOrdinal(this.Areaorig);
            if (!dr.IsDBNull(iAreaorig)) entity.Areaorig = Convert.ToInt32(dr.GetValue(iAreaorig));

            int iAreadestino = dr.GetOrdinal(this.Areadestino);
            if (!dr.IsDBNull(iAreadestino)) entity.Areadestino = Convert.ToInt32(dr.GetValue(iAreadestino));

            int iFljnumext = dr.GetOrdinal(this.Fljnumext);
            if (!dr.IsDBNull(iFljnumext)) entity.NumDocumento = dr.GetString(iFljnumext);

            int iAsunto = dr.GetOrdinal(this.Asunto);
            if (!dr.IsDBNull(iAsunto)) entity.Asunto = dr.GetString(iAsunto);

            int iFrecepcion = dr.GetOrdinal(this.Frecepcion);
            if (!dr.IsDBNull(iFrecepcion)) entity.FechaAsignacion = dr.GetDateTime(iFrecepcion);

            int iXfileruta = dr.GetOrdinal(this.Xfileruta);
            if (!dr.IsDBNull(iXfileruta)) entity.Xfileruta = dr.GetString(iXfileruta);

            string estado = string.Empty;
            int iFljestado = dr.GetOrdinal(this.Fljestado);
            if (!dr.IsDBNull(iFljestado)) estado = dr.GetString(iFljestado);
            entity.Estado = (new ConsultaHelper()).ObtenerEstado(estado);

            string atencion = string.Empty;
            int iFljcadatencion = dr.GetOrdinal(this.Fljcadatencion);
            if (!dr.IsDBNull(iFljcadatencion)) atencion = dr.GetString(iFljcadatencion);
            entity.Fljcadatencion = (new ConsultaHelper()).ObtenerAtencion(atencion);


            int iFljpadrecomentario = dr.GetOrdinal(this.Fljpadrecomentario);
            if (!dr.IsDBNull(iFljpadrecomentario)) entity.ComentarioPadre = dr.GetString(iFljpadrecomentario);

            int iFljfechaproce = dr.GetOrdinal(this.Fljfechaproce);
            if (!dr.IsDBNull(iFljfechaproce)) entity.Fljfechaproce = dr.GetDateTime(iFljfechaproce);
                      
            int iFmax = dr.GetOrdinal(this.Fmax);
            entity.TiempoMaxAtencion = false;

            if (!dr.IsDBNull(iFmax))
            {
                entity.TiempoMaxAtencion = true;
                entity.Fmax = dr.GetDateTime(iFmax);
            }

            int iFasignacion = dr.GetOrdinal(this.Fasignacion);
            if (!dr.IsDBNull(iFasignacion)) entity.Fasignacion = dr.GetDateTime(iFasignacion);

            int iFljdiasmaxaten = dr.GetOrdinal(this.Fljdiasmaxaten);
            if (!dr.IsDBNull(iFljdiasmaxaten)) entity.Fljdiasmaxaten = Convert.ToInt32(dr.GetValue(iFljdiasmaxaten));

            int iFmaxgen = dr.GetOrdinal(this.Fmaxgen);
            if (!dr.IsDBNull(iFmaxgen)) entity.FechaMaxAtencion = dr.GetDateTime(iFmaxgen);

            entity.ColorLetra = null;
            entity.ColorFondo = null;

            if (entity.Fljcadatencion != "00000000" &&
                   (estado == "P" || estado == "D") &&
                   !entity.TiempoMaxAtencion &&
                   (entity.FechaAsignacion.Date.AddDays(15) < DateTime.Now.Date) &&
                   (entity.FechaAsignacion.Date.AddDays(365) > DateTime.Now.Date))
            {
                char[] lc_cadena = atencion.ToCharArray();
                if (lc_cadena.Length > 0)
                {
                    if ((lc_cadena[6] != '1' && lc_cadena[7] != '1') || lc_cadena[0] == '1' || lc_cadena[1] == '1' || lc_cadena[2] == '1' || lc_cadena[3] == '1' || lc_cadena[4] == '1' || lc_cadena[5] == '1') // Diferente de conocimiento y Archivar                    
                    {
                        entity.ColorLetra = "#FF0000";
                        entity.ColorFondo = "#FFFFE6";
                    }
                }
            }


            return entity;
        }
    }

    public class AlertaTramiteHelper
    {
        public string Fljcodi = "Fljcodi";
        public string Corrnumproc = "Corrnumproc";
        public string Nombarearem = "Nombarearem";
        public string Areaorig = "Areaorig";
        public string Fljnumext = "Fljnumext";
        public string Asunto = "Asunto";
        public string Frecepcion = "Frecepcion";
        public string Xfileruta = "Xfileruta";
        public string Filecodi = "Filecodi";
        public string Fljremdetalle = "Fljremdetalle";
        public string Fljestado = "Fljestado";
        public string Fljdetcodi = "Fljdetcodi";
        public string Nombdestino = "Nombdestino";
        public string Fljdetdestino = "Fljdetdestino";
        public string Fljdetorigen = "Fljdetorigen";
        public string Fljdetnivel = "Fljdetnivel";
        public string Fljcadatencion = "Fljcadatencion";
        public string Fljpadrecomentario = "Fljpadrecomentario";
        public string Fljfechaproce = "Fljfechaproce";
        public string Areadestino = "Areadestino";
        public string Conplazo = "Conplazo";
        public string Fmax = "Fmax";
        public string Archcodificacion = "Archcodificacion";
        public string Archsubcodif = "Archsubcodif";
        public string Nombemprem = "Nombemprem";
        public string Fljconf = "Fljconf";
        public string Fljanio = "Fljanio";
        public string Prioridad = "Prioridad";
        public string Fljfileatrib = "Fljfileatrib";

        public AlertaTramiteDTO Create(IDataReader dr)
        {
            AlertaTramiteDTO entity = new AlertaTramiteDTO();

            int iFljcodi = dr.GetOrdinal(this.Fljcodi);
            if (!dr.IsDBNull(iFljcodi)) entity.CodRegistro = Convert.ToInt32(dr.GetValue(iFljcodi));

            int iCorrnumproc = dr.GetOrdinal(this.Corrnumproc);
            if (!dr.IsDBNull(iCorrnumproc)) entity.Correlativo = dr.GetDecimal(iCorrnumproc);

            int iNombarearem = dr.GetOrdinal(this.Nombarearem);
            if (!dr.IsDBNull(iNombarearem)) entity.NombAreaOrigen = dr.GetString(iNombarearem);

            int iAreaorig = dr.GetOrdinal(this.Areaorig);
            if (!dr.IsDBNull(iAreaorig)) entity.Areaorig = Convert.ToInt32(dr.GetValue(iAreaorig));

            int iFljnumext = dr.GetOrdinal(this.Fljnumext);
            if (!dr.IsDBNull(iFljnumext)) entity.NumDocumento = dr.GetString(iFljnumext);

            int iAsunto = dr.GetOrdinal(this.Asunto);
            if (!dr.IsDBNull(iAsunto)) entity.Asunto = dr.GetString(iAsunto);

            int iFrecepcion = dr.GetOrdinal(this.Frecepcion);
            if (!dr.IsDBNull(iFrecepcion)) entity.FechaLlegadaCoes = dr.GetDateTime(iFrecepcion);

            int iXfileruta = dr.GetOrdinal(this.Xfileruta);
            if (!dr.IsDBNull(iXfileruta)) entity.Xfileruta = dr.GetString(iXfileruta);

            int iFilecodi = dr.GetOrdinal(this.Filecodi);
            if (!dr.IsDBNull(iFilecodi)) entity.Filecodi = Convert.ToInt32(dr.GetValue(iFilecodi));

            int iFljremdetalle = dr.GetOrdinal(this.Fljremdetalle);
            if (!dr.IsDBNull(iFljremdetalle)) entity.Fljremdetalle = dr.GetString(iFljremdetalle);

            int iFljestado = dr.GetOrdinal(this.Fljestado);
            if (!dr.IsDBNull(iFljestado)) entity.Estado = dr.GetString(iFljestado);

            int iFljdetcodi = dr.GetOrdinal(this.Fljdetcodi);
            if (!dr.IsDBNull(iFljdetcodi)) entity.CodDetregistro = Convert.ToInt32(dr.GetValue(iFljdetcodi));

            int iNombdestino = dr.GetOrdinal(this.Nombdestino);
            if (!dr.IsDBNull(iNombdestino)) entity.NombAreaDestino = dr.GetString(iNombdestino);

            int iFljdetdestino = dr.GetOrdinal(this.Fljdetdestino);
            if (!dr.IsDBNull(iFljdetdestino)) entity.Fljdetdestino = Convert.ToInt32(dr.GetValue(iFljdetdestino));

            int iFljdetorigen = dr.GetOrdinal(this.Fljdetorigen);
            if (!dr.IsDBNull(iFljdetorigen)) entity.Fljdetorigen = Convert.ToInt32(dr.GetValue(iFljdetorigen));

            int iFljdetnivel = dr.GetOrdinal(this.Fljdetnivel);
            if (!dr.IsDBNull(iFljdetnivel)) entity.Fljdetnivel = Convert.ToInt32(dr.GetValue(iFljdetnivel));

            int iFljcadatencion = dr.GetOrdinal(this.Fljcadatencion);
            if (!dr.IsDBNull(iFljcadatencion)) entity.Fljcadatencion = dr.GetString(iFljcadatencion);

            int iFljpadrecomentario = dr.GetOrdinal(this.Fljpadrecomentario);
            if (!dr.IsDBNull(iFljpadrecomentario)) entity.ComentarioPadre = dr.GetString(iFljpadrecomentario);

            int iFljfechaproce = dr.GetOrdinal(this.Fljfechaproce);
            if (!dr.IsDBNull(iFljfechaproce)) entity.Fljfechaproce = dr.GetDateTime(iFljfechaproce);

            int iAreadestino = dr.GetOrdinal(this.Areadestino);
            if (!dr.IsDBNull(iAreadestino)) entity.Area = Convert.ToInt32(dr.GetValue(iAreadestino));

            int iConplazo = dr.GetOrdinal(this.Conplazo);
            if (!dr.IsDBNull(iConplazo)) entity.Conplazo = Convert.ToInt32(dr.GetValue(iConplazo));

            int iFmax = dr.GetOrdinal(this.Fmax);
            if (!dr.IsDBNull(iFmax)) entity.FechaMaxAtencion = dr.GetDateTime(iFmax);

            int iArchcodificacion = dr.GetOrdinal(this.Archcodificacion);
            if (!dr.IsDBNull(iArchcodificacion)) entity.ArchivoAdm = dr.GetString(iArchcodificacion);

            int iArchsubcodif = dr.GetOrdinal(this.Archsubcodif);
            if (!dr.IsDBNull(iArchsubcodif)) entity.ArchivoAdmSub = dr.GetString(iArchsubcodif);

            int iNombemprem = dr.GetOrdinal(this.Nombemprem);
            if (!dr.IsDBNull(iNombemprem)) entity.NombreEmpresaRem = dr.GetString(iNombemprem);

            int iFljconf = dr.GetOrdinal(this.Fljconf);
            if (!dr.IsDBNull(iFljconf)) entity.Confidencial = dr.GetString(iFljconf);

            int iFljanio = dr.GetOrdinal(this.Fljanio);
            if (!dr.IsDBNull(iFljanio)) entity.AnioLibro = Convert.ToInt32(dr.GetValue(iFljanio));

            int iPrioridad = dr.GetOrdinal(this.Prioridad);
            if (!dr.IsDBNull(iPrioridad)) entity.Prioridad = Convert.ToInt32(dr.GetValue(iPrioridad));

            int iFljfileatrib = dr.GetOrdinal(this.Fljfileatrib);
            if (!dr.IsDBNull(iFljfileatrib)) entity.Atributos = Convert.ToInt32(dr.GetValue(iFljfileatrib));


            return entity;
        }
           

    }

    public class TipoAtencionHelper
    {
        public string TipoAtencionCodi = "tatcodi2";
        public string TipoAtencionNomb = "tatabrev";
    }

    public class DiaEspecialHelper
    {
        public string DiaCodi = "DIACODI";
        public string DiaFecha = "DIAFECHA";
        public string DiaTipo = "DIATIPO";
        public string DiaFrec = "DIAFREC";
    }

    public class ContenidoCdHelper
    {
        public string Fljcodi = "fljcodi";
        public string Fileruta = "fileruta";
        public string Filecodi = "filecodi";
    }
}
