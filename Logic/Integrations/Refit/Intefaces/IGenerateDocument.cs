using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OfficeTime.GenerationModels;
using Refit;

namespace OfficeTime.Logic.Integrations.Refit.Intefaces
{
    public class InputModel
    {
        public TypeEnum TypeEnum { get; set; }
        public JObject Payload { get; set; }
    }

    public interface IGenerateDocument
    {
        [Post("/Generate")]
        Task CreateDocument([FromBody] InputModel model);
    }
}
