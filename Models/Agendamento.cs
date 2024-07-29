using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Barbearia.API.Models
{
    public class Agendamento
    {
        [Key]
        public int AgendamentoID { get; set; }
        [Required]
        public DateTime DataHora { get; set; }
        [DisplayName("Observações")]
        public string? Observacoes { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public int ClienteID { get; set; }
        [ForeignKey("ClienteID")]
        public Cliente cliente { get; set; }

        public virtual ICollection<Servico> Servicos { get; set; }
    }
}
