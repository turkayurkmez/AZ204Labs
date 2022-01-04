using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

public static class Echo
{
   [FunctionName("Echo")] 
   public static IActionResult Run([HttpTrigger("POST")]HttpRequest request, ILogger logger)
   {
      logger.LogInformation("Post işlemi gerçekleşti");
      return new OkObjectResult(request.Body);
   }
}