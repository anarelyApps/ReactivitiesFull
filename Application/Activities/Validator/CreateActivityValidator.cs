using System;
using Application.Activities.DTOs;
using Application.Commands;
using FluentValidation;

namespace Application.Activities.Validator;

public class CreateActivityValidator:BaseActivityValidator<CreateActivity.Command,CreateActivityDto>
{   
    public CreateActivityValidator(): base(x=>x.ActivityDto)
    {
       
    }
}
