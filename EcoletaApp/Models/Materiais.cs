using Ecoleta.Models.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecoleta.Models
{
    public class Materiais
    {
        public int IdMaterial { get; set; }

        public string DescricaoMaterial { get; set; } = string.Empty;
        public MateriaisEnuns MateriaisEnuns { get; set; }
    }
}
