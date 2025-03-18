using AutoMapper;
using Goldev.Core.Attributes;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OfficeTime.Logic.Integrations.Models;
using RestSharp;

namespace OfficeTime.Logic.Integrations
{
    [TrackedType]
    public class LoadAllTasksByFilterQueryHandler(ILogger<LoadAllTasksByFilterQueryHandler> logger,
        IMediator mediator,
        IMapper mapper,
        IOptions<YandexTrackerConfiguration> ytConfig
    ) : AbstractQueryHandler<LoadAllTasksByFilterCommand, List<YandexTask>>(logger, mediator)
    {
        private YandexTrackerConfiguration _configuration;
        private RestClient _restClient;
        private IMapper _mapper = mapper;

        [Constant(BlockName = "YandexTrackerConfiguration")]
        private static string _filtredTaskUrl;

        public override async Task<IHandleResult<List<YandexTask>>> HandleAsync(LoadAllTasksByFilterCommand request, CancellationToken cancellationToken)
        {
            _configuration = ytConfig.Value;

            _restClient = _configuration.CreateClient();

            try
            {
                var listTasks = new HashSet<ApiYandexTask>();
                var url = _filtredTaskUrl;

                while (url != null)
                {
                    var restRequest = new RestRequest(url, RestSharp.Method.Post);

                    restRequest.AddBody(await CreateFilter(request));

                    var responce = await _restClient.ExecuteAsync<List<ApiYandexTask>>(restRequest);

                    if (!responce.IsSuccessful)
                    {
                        return await BadRequest(responce.ErrorMessage);
                    }
                    if (responce.Data == null)
                    {
                        return await BadRequest("Empty responce");
                    }

                    var obj = JsonConvert.DeserializeObject<List<ApiYandexTask>>(responce.Content);

                    GetFilteredTask(obj, request).ForEach(o => listTasks.Add(o));

                    var headers = responce.Headers.Where(h => h.Value.Contains("next")).Select(h => h.Value).FirstOrDefault();

                    if (headers == null)
                    {
                        break;
                    }

                    url = headers.Split('<', '>')[1];
                }

                var list = listTasks.Select(x => _mapper.Map<YandexTask>(x)).ToList();


                return await Ok(list);
            }
            catch (Exception ex)
            {
                return await BadRequest(ex.Message);
            }
        }

        public Task<string> CreateFilter(LoadAllTasksByFilterCommand filter)
        {
            List<string> conditions = new()
            {
                AddParameter("status", filter.Statuses),
                AddParameter("assignee", filter.UserId),
                AddParameter("\\\"Start Date\\\"", filter.StartIntervalEnding, filter.EndIntervalEnding)
            };

            return System.Threading.Tasks.Task.FromResult($"{{\"query\":\"{CreateResultQuery(conditions)}\"}}");
        }

        public string CreateResultQuery(List<string> conditions, string separator = " ") => string.Join(separator, conditions.Where(x => !String.IsNullOrEmpty(x)));

        public string AddParameter<T>(string paramName, List<T>? list) => list == null ? "" : AddParameter(paramName, string.Join(", ", list));

        public string AddParameter(string paramName, string? value) => String.IsNullOrEmpty(value) ? "" : $"{paramName}: {value}";

        public string AddParameter(string paramName, DateTime? from = null, DateTime? to = null) => ((from != null ? 1 : 0) | (to != null ? 1 : 0) << 1) switch
        {
            1 => AddParameter(paramName, $"> {from.Value.ToString("yyyy-MM-dd")}"),
            2 => AddParameter(paramName, $"< {to.Value.ToString("yyyy-MM-dd")}"),
            3 => AddParameter(paramName, $"{from.Value.ToString("yyyy-MM-dd")}..{to.Value.ToString("yyyy-MM-dd")}"),
            _ => ""
        };

        public List<ApiYandexTask> GetFilteredTask(List<ApiYandexTask> answers, LoadAllTasksByFilterCommand filter)
        {
            List<ApiYandexTask> filteredList = new();
            foreach (var answer in answers)
            {
                if (filter.Statuses != null && filter.Statuses.Contains(answer.Status)
                    || (filter.UserId != null && filter.UserId == answer.Assignee.Id))
                {
                    filteredList.Add(answer);
                }
            }

            return filteredList;
        }
    }
}
