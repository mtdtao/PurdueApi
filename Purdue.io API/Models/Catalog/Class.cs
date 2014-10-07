﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PurdueIo.Models.Catalog
{
	/// <summary>
	/// Class model, representing the second order of class representation.
	/// Part of Contains classes, which contain sections (CRNs).
	/// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	/// !! DO NOT MODIFY THIS CLASS UNLESS YOU ARE FAMILIAR WITH MIGRATIONS !!
	/// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	/// </summary>
	public class Class
	{
		/// <summary>
		/// Unique ID with which to reference a class internally.
		/// </summary>
		public Guid ClassId { get; set; }

		/// <summary>
		/// The course to which this class belongs.
		/// </summary>
		public virtual Course Course { get; set; }

		/// <summary>
		/// The term (e.g. semester) that this class belongs to.
		/// </summary>
		public virtual Term Term { get; set; }

		/// <summary>
		/// The campus on which this class is taught.
		/// </summary>
		public virtual Campus Campus { get; set; }

		/// <summary>
		/// The sections that belong to this class.
		/// </summary>
		[InverseProperty("Class")]
		public virtual ICollection<Section> Sections { get; set; }
	}
}