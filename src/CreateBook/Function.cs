using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using BooksApi.Contracts;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CreateBook;

public class Function
{
    
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task<APIGatewayHttpApiV2ProxyResponse> CreateBookAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var bookRequest = System.Text.Json.JsonSerializer.Deserialize<Book>(request.Body, options)!; 
        
        AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        DynamoDBContext dbContext = new DynamoDBContext(client);
        await dbContext.SaveAsync(bookRequest);

        var message = $"book with Id {bookRequest?.Id} created";
        
        LambdaLogger.Log(message);
        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Body = message
        };
    }
}
