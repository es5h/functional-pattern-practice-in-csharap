using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using static LanguageExt.Validation<LanguageExt.Common.Error,MyNamespace.MakeTransfer>;
using static MyNamespace.FpModuleExample;

namespace MyNamespace;

public class FpModuleExampleTest
{
  [Fact]
  public void WhenValid_SaveSuccess_ReturnsOk()
  {
    var handler = HandleSaveTransfer(
      validate: Success,
      save: _ => () => Prelude.Some(Unit.Default)
    );
    
    // Act
   // var result = 
    
    // Assert
    // Assert.IsType<OkResult>(result);
  }
}
