﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.OData;
using System.Web.OData.Routing;
using PurdueIo.Models;
using PurdueIo.Models.Catalog;
using PurdueIo.Utils;

namespace PurdueIo.Controllers.Odata
{
	[ODataRoutePrefix("Classes")]
    public class ClassesController : ODataController
    {
        private ApplicationDbContext _Db = new ApplicationDbContext();

		//Disabled, we dont have users to have access to this
        //GET: odata/Classes
		[EnableQuery]
		public IHttpActionResult GetClasses()
		{
			//return db.Classes;
			return NotFound();
		}

        // GET: odata/Classes({GUID})
		[HttpGet]
		[ODataRoute("({classKey})")]
        [EnableQuery]
		public IHttpActionResult GetClass([FromODataUri] Guid classKey)
        {
			//For those confused we use @class because class is a keyword
            return Ok(SingleResult.Create(_Db.Classes.Where(@class => @class.ClassId == classKey)));
        }

		// GET: odata/Classes/Default.ByNumber(Number={[subject][number]})
		[HttpGet]
		[ODataRoute("Default.ByNumber(Number={subjectAndNumber})")]
		[EnableQuery(MaxAnyAllExpressionDepth = 2)]
		public IHttpActionResult GetClassesByNumber([FromODataUri] String subjectAndNumber)
		{
			System.Diagnostics.Debug.WriteLine("Inside GetCoursesByNumber");
			Tuple<String, String> course = Utilities.ParseCourse(subjectAndNumber);

			if (course == null)
			{
				//invalid input
				return BadRequest("Invalid format: Course Number is not in the format [Subject][Number] (ex. CS30700)");
			}

			//It's course
			IEnumerable<Class> selectedClasses = _Db.Classes
			.Where(
				x =>
					x.Course.Subject.Abbreviation == course.Item1 &&
					x.Course.Number == course.Item2
				);

			return Ok(selectedClasses);
		}

		// GET: odata/Classes/Default.ByTerm({term})
		[HttpGet]
		[ODataRoute("Default.ByTerm(Term={term})")]
		[EnableQuery(MaxAnyAllExpressionDepth = 2)]
		public IHttpActionResult GetClassesByTerm([FromODataUri] String term)
		{
			String match = Utilities.ParseTerm(term);

			//check if match exists
			if (match == null)
			{
				return BadRequest("Invalid format: Term does not match term format (ex. 201510)");
			}

			IQueryable<Class> selectedClasses = _Db.Classes
				.Where(
					x =>
						x.Term.TermCode == match
					);
			return Ok(selectedClasses);

		}

		// GET: odata/Courses/Default.ByTermAndNumber(Term={term},Number={subjectAndNumber}
		[HttpGet]
		[ODataRoute("Default.ByTermAndNumber(Term={term},Number={subjectAndNumber})")]
		[EnableQuery(MaxAnyAllExpressionDepth = 2)]
		public IHttpActionResult GetClassesByTermAndNumber([FromODataUri] String term, [FromODataUri] String subjectAndNumber)
		{
			Tuple<String, String> course = Utilities.ParseCourse(subjectAndNumber);
			String match = Utilities.ParseTerm(term);

			if (course == null)
			{
				return BadRequest("Invalid format: Course Number is not in the format [Subject][Number] (ex. CS30700)");
			}

			if (match == null)
			{
				return BadRequest("Invalid format: Term does not match term format (ex. 201510)");
			}

			IQueryable<Class> selectedCourses = _Db.Classes
					.Where(
						x =>
							x.Term.TermCode == match &&
							x.Course.Subject.Abbreviation == course.Item1 &&
							x.Course.Number == course.Item2
						);

			return Ok(selectedCourses);
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _Db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClassExists(Guid key)
        {
            return _Db.Classes.Count(e => e.ClassId == key) > 0;
        }
    }
}