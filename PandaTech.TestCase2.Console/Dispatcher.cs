using AutoMapper;
using FluentValidation;
using PandaTech.TestCase2.Configuration;
using PandaTech.TestCase2.Extensions;
using PandaTech.TestCase2.IO;
using PandaTech.TestCase2.Services;
using PandaTech.TestCase2.Validators;

namespace PandaTech.TestCase2.Console;

public sealed class Dispatcher
{
    private readonly IIoProvider<string?, string?> _ioProvider;
    private readonly ISeekerService<int, int> _seekerService;
    private readonly IMapper _mapper;
    private readonly MatrixValidator _matrixValidator;

    public Dispatcher(
        IIoProvider<string?, string?> ioProvider,
        ISeekerService<int, int> seekerService,
        IMapper mapper,
        MatrixValidator matrixValidator) =>
        (_ioProvider, _seekerService, _mapper, _matrixValidator) = (ioProvider, seekerService, mapper, matrixValidator);

    public async Task Run(params string[] args)
    {
        while (!_ioProvider.IsStopped)
        {
            try
            {
                var input = _ioProvider.GetInput();

                if (_ioProvider.IsStopped)
                {
                    System.Console.WriteLine("Abort requested. Application will be closed. Press any key to continue...");
                    System.Console.ReadKey();
                    return;
                }

                var matrix = _mapper.Map<int[][]>(input);
                var validationResult = await _matrixValidator.ValidateAsync(matrix);
                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.Errors);
                
                // var matrix = input.ToMatrix<int>(EnvSettings.RowDelimiter, EnvSettings.ColDelimiter);
                // var validationResult = await _matrixValidator.ValidateAsync(matrix);
                // if (!validationResult.IsValid)
                //     throw new ValidationException(validationResult.Errors);
                
                var output = _seekerService.Seek(matrix, EnvSettings.LookedValue);

                _ioProvider.SetOutput($"Result: {output}" );
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Console.WriteLine("Press any key to continue...");
                System.Console.ReadKey();
            }
        }
    }
}