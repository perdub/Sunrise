using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sunrise.Types;

namespace Sunrise.Controllers
{
	[Route("tags")]
	public class TagsController : Controller
	{
		public TagsController(SunriseContext sunriseContext)
		{
			this.sc = sunriseContext;
		}

		[Route("complete")]
		public IActionResult CompleteTag(string tag)
		{
			string pattern = tag + "%";
			IQueryable<Tag> tags = (from a in this.sc.Tags
			orderby a.PostCount descending
			where EF.Functions.Like(a.SearchText, pattern)
			select a).Take(10);
			return this.Ok(tags);
		}

		
		private SunriseContext sc;
	}
}
