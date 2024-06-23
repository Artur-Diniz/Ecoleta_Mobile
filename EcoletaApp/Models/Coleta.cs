using Ecoleta.Models.Enuns;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoletaApp.Models
{
    public class Coleta
    {
        [Key]
        public int IdColeta { get; set; }

        [ForeignKey("IdEcoponto")]
        public int IdEcoponto { get; set; }
        [ForeignKey("IdUtilizador")]
        public int IdUtilizador { get; set; }

        public MateriaisEnuns Classe { get; set; }
        public DateTime DataColeta { get; set; }
        public Double Peso { get; set; }
        public string SituacaoColeta { get; set; } = string.Empty;

     
    }
}
