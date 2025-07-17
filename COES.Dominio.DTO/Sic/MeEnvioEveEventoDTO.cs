using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public partial class MeEnvioEveEventoDTO : EntityBase
    {
        public int Env_Evencodi { get; set; }
        public int Enviocodi { get; set; }
        public int Evencodi { get; set; }
        public int Envetapainforme { get; set; }
    }
}
