using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla INT_DIRECTORIO
    /// </summary>
    public class IntDirectorioHelper : HelperBase
    {
        public IntDirectorioHelper(): base(Consultas.IntDirectorioSql)
        {
        }

        public IntDirectorioDTO Create(IDataReader dr)
        {
            IntDirectorioDTO entity = new IntDirectorioDTO();

            int iDircodi = dr.GetOrdinal(this.Dircodi);
            if (!dr.IsDBNull(iDircodi)) entity.Dircodi = Convert.ToInt32(dr.GetValue(iDircodi));

            int iDirnombre = dr.GetOrdinal(this.Dirnombre);
            if (!dr.IsDBNull(iDirnombre)) entity.Dirnombre = dr.GetString(iDirnombre);

            int iDirapellido = dr.GetOrdinal(this.Dirapellido);
            if (!dr.IsDBNull(iDirapellido)) entity.Dirapellido = dr.GetString(iDirapellido);

            int iDircorreo = dr.GetOrdinal(this.Dircorreo);
            if (!dr.IsDBNull(iDircorreo)) entity.Dircorreo = dr.GetString(iDircorreo);

            int iDiranexo = dr.GetOrdinal(this.Diranexo);
            if (!dr.IsDBNull(iDiranexo)) entity.Diranexo = dr.GetString(iDiranexo);

            int iDirtelefono = dr.GetOrdinal(this.Dirtelefono);
            if (!dr.IsDBNull(iDirtelefono)) entity.Dirtelefono = dr.GetString(iDirtelefono);

            //int iDirfuncion = dr.GetOrdinal(this.Dirfuncion);
            //if (!dr.IsDBNull(iDirfuncion)) entity.Dirfuncion = string;

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iDircumpleanios = dr.GetOrdinal(this.Dircumpleanios);
            if (!dr.IsDBNull(iDircumpleanios)) entity.Dircumpleanios = dr.GetDateTime(iDircumpleanios);

            int iDirfoto = dr.GetOrdinal(this.Dirfoto);
            if (!dr.IsDBNull(iDirfoto)) entity.Dirfoto = dr.GetString(iDirfoto);

            int iDirestado = dr.GetOrdinal(this.Direstado);
            if (!dr.IsDBNull(iDirestado)) entity.Direstado = dr.GetString(iDirestado);

            int iDirpadre = dr.GetOrdinal(this.Dirpadre);
            if (!dr.IsDBNull(iDirpadre)) entity.Dirpadre = Convert.ToInt32(dr.GetValue(iDirpadre));

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iDirusucreacion = dr.GetOrdinal(this.Dirusucreacion);
            if (!dr.IsDBNull(iDirusucreacion)) entity.Dirusucreacion = dr.GetString(iDirusucreacion);

            int iDirfeccreacion = dr.GetOrdinal(this.Dirfeccreacion);
            if (!dr.IsDBNull(iDirfeccreacion)) entity.Dirfeccreacion = dr.GetDateTime(iDirfeccreacion);

            int iDirusumodificacion = dr.GetOrdinal(this.Dirusumodificacion);
            if (!dr.IsDBNull(iDirusumodificacion)) entity.Dirusumodificacion = dr.GetString(iDirusumodificacion);

            int iDirfecmodificacion = dr.GetOrdinal(this.Dirfecmodificacion);
            if (!dr.IsDBNull(iDirfecmodificacion)) entity.Dirfecmodificacion = dr.GetDateTime(iDirfecmodificacion);

            int iDircargo = dr.GetOrdinal(this.Dircargo);
            if (!dr.IsDBNull(iDircargo)) entity.Dircargo = dr.GetString(iDircargo);

            int iDirapoyo = dr.GetOrdinal(this.Dirapoyo);
            if (!dr.IsDBNull(iDirapoyo)) entity.Dirapoyo = dr.GetString(iDirapoyo);

            int iDirorganigrama = dr.GetOrdinal(this.Dirorganigrama);
            if (!dr.IsDBNull(iDirorganigrama)) entity.Dirorganigrama = dr.GetString(iDirorganigrama);

            int iDirsecretaria = dr.GetOrdinal(this.Dirsecretaria);
            if (!dr.IsDBNull(iDirsecretaria)) entity.Dirsecretaria = dr.GetString(iDirsecretaria);

            int iDirsuperiores = dr.GetOrdinal(this.Dirsuperiores);
            if (!dr.IsDBNull(iDirsuperiores)) entity.Dirsuperiores = dr.GetString(iDirsuperiores);

            int iDirindsuperior = dr.GetOrdinal(this.Dirindsuperior);
            if (!dr.IsDBNull(iDirindsuperior)) entity.Dirindsuperior = dr.GetString(iDirindsuperior);

            int iDirnivel = dr.GetOrdinal(this.Dirnivel);
            if (!dr.IsDBNull(iDirnivel)) entity.Dirnivel = Convert.ToInt32(dr.GetValue(iDirnivel));

            return entity;
        }


        #region Mapeo de Campos

        public string Dircodi = "DIRCODI";
        public string Dirnombre = "DIRNOMBRE";
        public string Dirapellido = "DIRAPELLIDO";
        public string Dircorreo = "DIRCORREO";
        public string Diranexo = "DIRANEXO";
        public string Dirtelefono = "DIRTELEFONO";
        public string Dirfuncion = "DIRFUNCION";
        public string Areacodi = "AREACODI";
        public string Dircumpleanios = "DIRCUMPLEANIOS";
        public string Dirfoto = "DIRFOTO";
        public string Direstado = "DIRESTADO";
        public string Dirpadre = "DIRPADRE";
        public string Usercode = "USERCODE";
        public string Dirusucreacion = "DIRUSUCREACION";
        public string Dirfeccreacion = "DIRFECCREACION";
        public string Dirusumodificacion = "DIRUSUMODIFICACION";
        public string Dirfecmodificacion = "DIRFECMODIFICACION";
        public string Dircargo = "DIRCARGO";
        public string Dirapoyo = "DIRAPOYO";
        public string Dirorganigrama = "DIRORGANIGRAMA";
        public string Dirsecretaria = "DIRSECRETARIA";
        public string Dirsuperiores = "DIRSUPERIORES";
        public string Dirindsuperior = "DIRINDSUPERIOR";
        public string Dirnivel = "DIRNIVEL";

        #endregion
    }
}
