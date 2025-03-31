using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OfficeTime.GenerationModels;
using Refit;
using System.Text.Json.Serialization;

namespace OfficeTime.Logic.Integrations.Refit.Intefaces
{
    public class InputModel
    {
        public TypeEnum TypeEnum { get; set; }
        public string Payload { get; set; }
        public string TelegramId { get; set; }
    }

    public interface IGenerateDocument
    {
        [Post("/Generate")]
        Task CreateDocument([FromBody]InputModel model);
    }
}
