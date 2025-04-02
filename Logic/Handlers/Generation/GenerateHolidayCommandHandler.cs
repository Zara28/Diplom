using AutoMapper;
using Goldev.Core.Attributes;
using Goldev.Core.MediatR.Handlers;
using Goldev.Core.MediatR.Models;
using MediatR;
using Newtonsoft.Json;
using OfficeTime.DBModels;
using OfficeTime.GenerationModels;
using OfficeTime.Logic.Commands;
using OfficeTime.Logic.Handlers.Employees;
using OfficeTime.Logic.Integrations.Refit.Commands;
using OfficeTime.ViewModels;
using System.Data.Entity;

namespace OfficeTime.Logic.Handlers.Generation
{
    [TrackedType]
    public class GenerateHolidayCommandHandler : AbstractCommandHandler<GenerateHolidayCommand>
    {
        [Constant(BlockName = "Constants")]
        private static string _fIODirector;
        [Constant(BlockName = "Constants")]
        private static string _companyName; 
        [Constant(BlockName = "Constants")]
        private static string _telegramMain;

        private readonly diplom_adminkaContext _context;

        private readonly IMediator _mediator;

        public GenerateHolidayCommandHandler(diplom_adminkaContext context, 
                                            IMediator mediator, 
                                            ILogger<GenerateHolidayCommandHandler> logger) : base(logger, mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public override async Task<IHandleResult> HandleAsync(GenerateHolidayCommand command, CancellationToken cancellationToken = default)
        {
            DateTime date = DateTime.Now;
            DateTime dateStart;
            DateTime dateEnd;

            if(command.Type == TypeEnum.PutHolidays)
            {
                dateStart = new DateTime(date.Year, 1, 1);
                dateEnd = dateStart.AddYears(1).AddDays(-1);
            }
            else
            {
                dateStart = new DateTime(date.Year, date.Month, 1);
                dateEnd = dateStart.AddMonths(1).AddDays(-1);
            }
            

            var holidays = _context.Holidays
                .Where(h => h.Datestart > dateStart && h.Datestart < dateEnd);

            var emp = _context.Employees.Include(e => e.Post);

            string data;

            if (command.Type == TypeEnum.PutHolidays)
            {
                var model = new PutHolidays()
                {
                    NameCompany = _companyName,
                    FIODirector = _fIODirector,
                    Rows = holidays
                        .Select(h => new PutHolidaysRow
                        {
                            FIO = emp.FirstOrDefault(e => e.Id == h.Empid).Fio,
                            Number = emp.FirstOrDefault(e => e.Id == h.Empid).Id,
                            Post = emp.FirstOrDefault(e => e.Id == h.Empid).Post.Name,
                            Type = (bool)h.Pay ? "оплачиваемый" : "за свой счет",
                            Count = Convert.ToInt32((h.Dateend.Value - h.Datestart.Value).TotalDays),
                            DateStartWork = emp.FirstOrDefault(e => e.Id == h.Empid).Datestart ?? DateTime.Now,
                            DateStart = h.Datestart.Value,
                            DateEnd = h.Dateend.Value
                        }).ToArray()
                };

                data = JsonConvert.SerializeObject(model);
            }
            else
            {
                var model = new HolidaysT7
                {
                    NameCompany = _companyName,
                    FIODirector = _fIODirector,
                    Holidays = holidays
                        .Select(h => new HolidayRow
                        {
                            FIO = emp.FirstOrDefault(e => e.Id == h.Empid).Fio,
                            Number = emp.FirstOrDefault(e => e.Id == h.Empid).Id.ToString(),
                            Post = emp.FirstOrDefault(e => e.Id == h.Empid).Post.Name,
                            CountDays = Convert.ToInt32((h.Dateend.Value - h.Datestart.Value).TotalDays),
                            DateStart = h.Datestart.Value,
                        }).ToArray()
                };

                data = JsonConvert.SerializeObject(model);
            }

            await _mediator.Send(new DocumentSendCommand
            {
                InputModel = new Integrations.Refit.Intefaces.InputModel
                {
                    Payload = data,
                    TelegramId = _telegramMain,
                    TypeEnum = command.Type
                }
            });

            return await Ok();
        }
    }
}
