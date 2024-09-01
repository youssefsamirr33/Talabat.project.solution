using AutoMapper.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.ApplicationContext;

namespace Talabat.APIs.Controllers
{
	public class BuggyController : BaseApiController
	{
		private readonly StoreDbContext _context;

		public BuggyController(StoreDbContext context)
        {
			_context = context;
		}

		[HttpGet("notfound")]
		public ActionResult GetNotFound()
		{
			var product = _context.Products.Find(100);
			if (product is null)
				return NotFound(new ApiResponse(404));
			return Ok(product);
		}

		[HttpGet("servererror")]
		public ActionResult GetServerError()
		{
			var product = _context.Products.Find(100);
			var productToreturn = product.ToString();
			return Ok(productToreturn);
		}

		[HttpGet("badrequest")]
		public ActionResult GetBadRequest()
		{
			return BadRequest(new ApiResponse(400));
		}

		[HttpGet("validationerror/{id}")]
		public ActionResult GetValidationError(int id)
		{
			var product = _context.Products.Where(p => p.Id == id).FirstOrDefault();
			return Ok(product);
		}
    }
}
