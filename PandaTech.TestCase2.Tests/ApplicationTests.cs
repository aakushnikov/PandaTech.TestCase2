using AutoMapper;
using FluentValidation.Results;
using PandaTech.TestCase2.Extensions;
using PandaTech.TestCase2.Services;
using PandaTech.TestCase2.Validators;

namespace PandaTech.TestCase2.Tests;

public class ApplicationTests
{
    private readonly IMapper _mapper;
    private readonly MatrixValidator _matrixValidator;
    private readonly ISeekerService<int, int> _seekerService;

    public ApplicationTests()
    {
        _mapper = new MapperConfiguration(Mapping.CreateMap).CreateMapper();
        _matrixValidator = new MatrixValidator();
        _seekerService = new SeekerService<int>();
    }
    
    [TestCase("1,0;1,2", TestName = "Good formatted input data 1")]
    [TestCase("1,0;1,2;1,3", TestName = "Good formatted input data 2")]
    [TestCase("1,0,4; 1,2, 5;1, 3, 7", TestName = "Good formatted input data 3")]
    public void WHEN_Map_To_Matrix_WITH_Correct_Formatted_Data_THEN_Not_Throws_Exception(string? source)
    {
        Assert.DoesNotThrow(() => _mapper.Map<int[][]>(source));
    }
    
    [TestCase("0:1 2", TestName = "Bad symbol in input data")]
    [TestCase("1,2,1,3;", TestName = "Empty dimension in input data")]
    [TestCase("1,0,4; 1,2, 5;1, 3 7", TestName = "Wrong dimension format in input data")]
    [TestCase("1,0,0; 0,1, 1;1, a, 1", TestName = "Wrong value type in input data")]
    public void WHEN_Map_To_Matrix_WITH_Incorrect_Formatted_Data_THEN_Throws_FormatException(string source)
    {
        Assert.Throws<FormatException>(() => _mapper.Map<int[][]>(source));
    }
    
    [TestCase("1,0;1,1", TestName = "Good formatted matrix 1")]
    [TestCase("1,0;1,1;1,1", TestName = "Good formatted matrix 2")]
    [TestCase("1,0,0; 0,1, 1;1, 1, 1", TestName = "Good formatted matrix 3")]
    public void WHEN_Validate_Matrix_WITH_Correct_Data_THEN_Not_Throws_Exception(string? source)
    {
        var matrix = Array.Empty<int[]>();
        Assert.DoesNotThrow(() => matrix = _mapper.Map<int[][]>(source));
        Assert.DoesNotThrowAsync(async () => await _matrixValidator.ValidateAsync(matrix));
    }
    
    [TestCase("1,0;1;1,1", TestName = "Various X-dimensions sizes in matrix")]
    [TestCase("1,0,0; 0,1, 1;1, 1, 1; 1,1,1", TestName = "Exceeded dimensions sizes in matrix")]
    [TestCase("1,0,0; 0,5, 6;1, 1, 1; 1,1,1", TestName = "Restricted values in matrix")]
    public void WHEN_Validate_Matrix_WITH_Incorrect_Data_THEN_Throws_ValidationException(string? source)
    {
        var matrix = Array.Empty<int[]>();
        Assert.DoesNotThrow(() => matrix = _mapper.Map<int[][]>(source));
        var validationResult = new ValidationResult();
        Assert.DoesNotThrowAsync(async () => validationResult = await _matrixValidator.ValidateAsync(matrix));
        Assert.False(validationResult.IsValid);
        Assert.True(validationResult.Errors.Count > 0);
    }
    
    [TestCase("1,0;1,1", 1, 3, TestName = "Seek results match 1")]
    [TestCase("1,0;1,1;1,1", 1, 5, TestName = "Seek results match 2")]
    [TestCase("1,0,0; 0,1, 1;1, 1, 1", 1, 6, TestName = "Seek results match 3")]
    public void WHEN_Seek_In_Matrix_WITH_Correct_Data_THEN_Get_Expected_Result(string? source, int seekValue, int expectedResult)
    {
        var matrix = Array.Empty<int[]>();
        Assert.DoesNotThrow(() => matrix = _mapper.Map<int[][]>(source));
        Assert.DoesNotThrowAsync(async () => await _matrixValidator.ValidateAsync(matrix));
        Assert.AreEqual(_seekerService.Seek(matrix, seekValue), expectedResult);
    }
}