using FluentValidation;
using PandaTech.TestCase2.Configuration;

namespace PandaTech.TestCase2.Validators;

public sealed class MatrixValidator : AbstractValidator<int[][]>
{
    public MatrixValidator()
    {
        When(
                array => array is null,
                () =>
                {
                    RuleFor(array => array)
                        .NotNull()
                        .WithMessage("Matrix data cannot be null");
                }
            )
            .Otherwise(
                () =>
                    RuleFor(array => array)
                        .Must(array => array.Length <= EnvSettings.MatrixMaxY)
                        .WithMessage($"Matrix Y-dimension cannot be greater then {EnvSettings.MatrixMaxY}")
                        .Must(array => array.GroupBy(list => list.Length).Count() == 1)
                        .WithMessage($"Matrix cannot has a various dimensions by X")
                        .Must(array => array.All(list => list.Length <= EnvSettings.MatrixMaxX))
                        .WithMessage($"Matrix X-dimension cannot be greater then {EnvSettings.MatrixMaxX}")
                        .Must(array => !array.Any(list => list.Distinct().Except(EnvSettings.MatrixAllowedValues).Any()))
                        .WithMessage($"Matrix can contains only values from \"{EnvSettings.MatrixAllowedValues}\"")
            );
    }
}