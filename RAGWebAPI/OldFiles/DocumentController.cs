//using Microsoft.AspNetCore.Mvc;
//using RAGWebAPI.Services;
//using RAGWebAPI.Database;
//using UglyToad.PdfPig;
//using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

//namespace RAGWebAPI.Controllers;

//[ApiController]
//[Route("[controller]")]
//public class DocumentController(IEmbeddingService embeddingService) : ControllerBase
//{
//    private readonly IEmbeddingService _embeddingService = embeddingService;

//    [HttpPost()]
//    public async Task<IActionResult> AddDocument([FromQuery] string document)
//    {
//        var response = await _embeddingService.AddDocument(document);

//        return Ok(response);
//    }

//    [HttpPost("many")]
//    public async Task<IActionResult> AddDocuments([FromBody] string[] documents)
//    {
//        var response = await _embeddingService.AddDocuments(documents);

//        return Ok(response);
//    }

//    //[HttpPost("pdf")]
//    //public async Task<IActionResult> UploadPdf(IFormFile file)
//    //{
//    //    if (file == null || file.Length == 0)
//    //    {
//    //        return BadRequest("No file uploaded");
//    //    }

//    //    if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
//    //    {
//    //        return BadRequest("Only PDF files are allowed.");
//    //    }

//    //    string? pdfText = "";
//    //    using (var pdf = PdfDocument.Open(file.OpenReadStream()))
//    //    {
//    //        foreach (var page in pdf.GetPages())
//    //        {
//    //            var text = ContentOrderTextExtractor.GetText(page);

//    //            Console.WriteLine(text);

//    //            pdfText += text;
//    //        }
//    //    }

//    //    return Ok(pdfText);
//    //}

//    [HttpGet()]
//    public async Task<IActionResult> SearchDocument([FromQuery] string searchPrompt)
//    {
//        var response = await _embeddingService.SearchDocument(searchPrompt);

//        return Ok(response);
//    }
//}

