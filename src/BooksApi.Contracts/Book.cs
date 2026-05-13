using Amazon.DynamoDBv2.DataModel;

namespace BooksApi.Contracts;

[DynamoDBTable("Books")]
public class Book
{
    [DynamoDBHashKey("id")]
    public int Id { get; set; }

    [DynamoDBProperty("title")]
    public string Title { get; set; }
    
    [DynamoDBProperty("author")]
    public string Author { get; set; }
    
    [DynamoDBProperty("year")]
    public int Year { get; set; }

}