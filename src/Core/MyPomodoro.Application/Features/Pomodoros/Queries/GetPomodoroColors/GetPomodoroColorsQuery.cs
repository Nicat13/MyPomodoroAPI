using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyPomodoro.Application.Exceptions;
using MyPomodoro.Application.Interfaces.UnitOfWork;
using MyPomodoro.Domain.Enums;

namespace MyPomodoro.Application.Features.Pomodoros.Queries.GetPomodoroColors
{
    public class GetPomodoroColorsQuery : IRequest<List<PomodoroColors>>
    {
    }
    public class GetPomodoroColorsQueryHandler : IRequestHandler<GetPomodoroColorsQuery, List<PomodoroColors>>
    {
        IUowContext uowContext;

        public GetPomodoroColorsQueryHandler(IUowContext uowContext)
        {
            this.uowContext = uowContext;
        }

        public async Task<List<PomodoroColors>> Handle(GetPomodoroColorsQuery request, CancellationToken cancellationToken)
        {
            using (var uow = uowContext.GetUow())
            {
                try
                {
                    return await Task.FromResult(uow.PomodoroRepository.GetPomodoroColors());
                }
                catch (Exception ex)
                {
                    throw new HttpStatusException(new List<string> { ex.Message });
                }
            }
        }
    }
}