using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APITarefa.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        [Required]
        public string? Titulo { get; set; }
        [Required]
        public string? Descricao { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public EnumStatusTarefa Status { get; set; }
    }
}