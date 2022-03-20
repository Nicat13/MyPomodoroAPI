using System;
using AutoMapper;
using MyPomodoro.Application.Features.Pomodoros.Commands.CreatePomodoro;
using MyPomodoro.Domain.Entities;

namespace MyPomodoro.Application.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<CreatePomodoroCommand, Pomodoro>();

        }
    }
}