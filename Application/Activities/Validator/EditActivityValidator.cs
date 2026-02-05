using System;
using System.Security.Cryptography.X509Certificates;
using Application.Activities.Commands;
using Application.Activities.DTOs;
using FluentValidation;

namespace Application.Activities.Validator;

public class EditActivityValidator:BaseActivityValidator<EditActivity.Command,EditActivityDto>
{
    public EditActivityValidator():base(x=>x.ActivityDto)
    {
        RuleFor(x=>x.ActivityDto)
        .NotEmpty().WithMessage("Id is required");
    }
}
