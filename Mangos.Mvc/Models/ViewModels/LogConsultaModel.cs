using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class LogConsultaModel
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data/hora")]
        public DateTime DataHora { get; set; }

        [Display(Name = "Level")]
        public string LogLevel { get; set; }

        [Display(Name = "Aplicação")]
        public string? Aplicacao { get; set; }

        [Display(Name = "Category")]
        public string? CategoryName { get; set; }

        public string Mensagem { get; set; }

        public string? Exception { get; set; }

        public LogConsultaModel(int id, DateTime dataHora, string logLevel, string? aplicacao, string? categoryName, string mensagem, string? exception)
        {
            Id = id;
            DataHora = dataHora;
            LogLevel = logLevel;
            Aplicacao = aplicacao;
            CategoryName = categoryName;
            Mensagem = mensagem;
            Exception = exception;
        }
    }
}