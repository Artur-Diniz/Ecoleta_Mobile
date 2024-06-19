using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecoleta.Models
{
    public class Resgate
    {
        [Key]
        public int IdResgate { get; set; }
        [ForeignKey("IdUtilizador")]
        public int IdUtilizador { get; set; }
        [ForeignKey("IdBrinde")]
        public int IdBrinde { get; set; }
        public DateTime DataResgate { get; set; }

    }
}
