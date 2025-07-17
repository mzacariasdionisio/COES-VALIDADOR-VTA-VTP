using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Busqueda
{
    public class BCDBusquedasDTO
    {
        public int Id_search { get; set; }
        public DateTime Search_date { get; set; }
        public string Search_user { get; set; }
        public string Search_text { get; set; }
        public string Key_words { get; set; }
        public string Tipo_documento { get; set; }
        public string Key_concepts { get; set; }
        public int Result_number { get; set; }
        public DateTime Search_start_date { get; set; }
        public DateTime Search_end_date { get; set; }
        public Nullable<bool> Search_type { get; set; }
        public Nullable<bool> Search_relation { get; set; }
        public string Exclude_words { get; set; }
    }
}
