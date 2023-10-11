using Microsoft.AspNetCore.Mvc.Filters;
using sodoff.Util;
using System.Text;

namespace sodoff.Attributes;
public class EncryptResponse : Attribute, IAsyncResultFilter {
    const string key = "56BB211B-CF06-48E1-9C1D-E40B5173D759";
    const string land1Key = "A6EE1C3903204be1B6D2B82533AAF1D2";

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next) {
        var originalBodyStream = context.HttpContext.Response.Body;

        using (var memoryStream = new MemoryStream()) {
            context.HttpContext.Response.Body = memoryStream;

            await next();

            // Read body and encrypt
            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
            var result = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<string>{TripleDES.EncryptUnicode(responseBody, key)}</string>";
            if (context.HttpContext.Request.Form["apiKey"] == "B4E0F71A-1CDA-462a-97B3-0B355E87E0C8") result = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<string>{TripleDES.EncryptUnicode(responseBody, land1Key)}</string>";
            context.HttpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(result);
            var newBody = new MemoryStream(Encoding.UTF8.GetBytes(result));
            // Override body with encrypted data
            await newBody.CopyToAsync(originalBodyStream);
        }
    }
}
