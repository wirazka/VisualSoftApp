using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace VisualSoftApp.Controllers
{
	[ApiController]
	[Route("api")]
	public class DocumentController : ControllerBase
	{
		private const string _validUsername = "vs";
		private const string _validPassword = "rekrutacja";
		private DocumentParser _documentParser = new();

		[HttpPost("test/{x}")]		
		public async Task<IActionResult> ProcessDocument(int x)
		{
			if (!IsAuthorized(Request.Headers["Authorization"]))
			{
				return Unauthorized();
			}

			try
			{
				using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
				{
					var requestBody = await reader.ReadToEndAsync();
					var lines = requestBody.Split('\n');

					int lineCount = lines.Length;
					int charCount = lines.Sum(line => line.Length);
					int sum = 0;
					int xCount = 0;
					decimal maxNetValue = decimal.MinValue;
					string maxNetValueProductName = "";

					var documents = _documentParser.ParseLines(lines);

					sum = documents.Sum(s => s.Positions.Count);
					xCount = documents.Count(doc => doc.Positions.Count > x);

					var productGroups = documents
						.SelectMany(doc => doc.Positions)
						.GroupBy(pos => pos.ProductName)
						.Select(group => new
						{
							ProductName = group.Key,
							TotalNetValue = group.Sum(pos => pos.NetValue)
						})
						.OrderByDescending(group => group.TotalNetValue)
						.ToList();

					maxNetValue = productGroups.First().TotalNetValue;

					var productsWithMaxNetValue = productGroups
						.Where(group => group.TotalNetValue == maxNetValue)
						.Select(group => group.ProductName)
						.ToList();

					maxNetValueProductName = string.Join(", ", productsWithMaxNetValue);

					var response = new
					{
						documents = documents,
						lineCount = lineCount,
						charCount = charCount,
						sum = sum,
						xCount = xCount,
						productswithMaxNetValue = maxNetValueProductName
					};

					return Ok(response);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Błąd przetwarzania pliku: {ex.Message}");
			}
		}

		private bool IsAuthorized(string authHeader)
		{
			if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Basic "))
			{
				var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
				var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
				var parts = credentials.Split(':', 2);

				if (parts.Length == 2 && parts[0] == _validUsername && parts[1] == _validPassword)
				{
					return true;
				}
			}
			return false;
		}
	}
}
