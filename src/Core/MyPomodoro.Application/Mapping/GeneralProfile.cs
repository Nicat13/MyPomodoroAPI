using System;
using AutoMapper;
using MyPomodoro.Application.DTOs.ViewModels;
using MyPomodoro.Application.Features.Pomodoros.Commands.CreatePomodoro;
using MyPomodoro.Application.Features.Pomodoros.Commands.UpdatePomodoro;
using MyPomodoro.Application.Features.Users.Commands.UpdateUserConfiguration;
using MyPomodoro.Domain.Entities;

namespace MyPomodoro.Application.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<CreatePomodoroCommand, Pomodoro>();
            CreateMap<UpdatePomodoroCommand, Pomodoro>();
            CreateMap<Pomodoro, PomodoroDetailsViewModel>();
            CreateMap<UpdateUserConfigurationCommand, UserConfiguration>();
            CreateMap<UserConfiguration, UserConfigurationViewModel>();
            CreateMap<PomodoroDetailsViewModel, PomodoroSessionDetailsViewModel>();

        }
    }
}