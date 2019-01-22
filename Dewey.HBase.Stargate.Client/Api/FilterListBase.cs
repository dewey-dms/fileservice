#region FreeBSD

// Copyright (c) 2013, The Tribe
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
//  * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// 
//  * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
// TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace Dewey.HBase.Stargate.Client.Api
{
	/// <summary>
	///    Provides a base type for scanner filters that are also lists of values.
	/// </summary>
	public abstract class FilterListBase<TValue> : ScannerFilterBase, IList<TValue>
	{
		private readonly List<TValue> _innerList;

		/// <summary>
		///    Initializes a new instance of the <see cref="FilterListBase{TValue}" /> class.
		/// </summary>
		protected FilterListBase()
		{
			_innerList = new List<TValue>();
		}

		/// <summary>
		///    Initializes a new instance of the <see cref="FilterListBase{TValue}" /> class.
		/// </summary>
		/// <param name="filters">The filters.</param>
		protected FilterListBase(IEnumerable<TValue> filters)
		{
			_innerList = new List<TValue>(filters);
		}

		/// <summary>
		///    Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <returns>
		///    The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </returns>
		public int Count
		{
			get { return _innerList.Count; }
		}

		/// <summary>
		///    Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
		/// </summary>
		/// <returns>
		///    true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.
		/// </returns>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		///    Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		///    A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<TValue> GetEnumerator()
		{
			return _innerList.GetEnumerator();
		}

		/// <summary>
		///    Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		///    An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		///    Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <param name="item">
		///    The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </param>
		public void Add(TValue item)
		{
			_innerList.Add(item);
		}

		/// <summary>
		///    Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		public void Clear()
		{
			_innerList.Clear();
		}

		/// <summary>
		///    Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
		/// </summary>
		/// <param name="item">
		///    The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </param>
		/// <returns>
		///    true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
		/// </returns>
		public bool Contains(TValue item)
		{
			return _innerList.Contains(item);
		}

		/// <summary>
		///    Copies to.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Index of the array.</param>
		public void CopyTo(TValue[] array, int arrayIndex)
		{
			_innerList.CopyTo(array, arrayIndex);
		}

		/// <summary>
		///    Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <param name="item">
		///    The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </param>
		/// <returns>
		///    true if <paramref name="item" /> was successfully removed from the
		///    <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if
		///    <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </returns>
		public bool Remove(TValue item)
		{
			return _innerList.Remove(item);
		}

		/// <summary>
		///    Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
		/// </summary>
		/// <param name="item">
		///    The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.
		/// </param>
		/// <returns>
		///    The index of <paramref name="item" /> if found in the list; otherwise, -1.
		/// </returns>
		public int IndexOf(TValue item)
		{
			return _innerList.IndexOf(item);
		}

		/// <summary>
		///    Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
		/// </summary>
		/// <param name="index">
		///    The zero-based index at which <paramref name="item" /> should be inserted.
		/// </param>
		/// <param name="item">
		///    The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.
		/// </param>
		public void Insert(int index, TValue item)
		{
			_innerList.Insert(index, item);
		}

		/// <summary>
		///    Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		public void RemoveAt(int index)
		{
			_innerList.RemoveAt(index);
		}

		/// <summary>
		///    Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public TValue this[int index]
		{
			get { return _innerList[index]; }
			set { _innerList[index] = value; }
		}

		/// <summary>
		///    Adds the elements of the specified collection to the end of the <see cref="T:System.Collections.Generic.List`1" />.
		/// </summary>
		/// <param name="items">
		///    The collection whose elements should be added to the end of the
		///    <see cref="T:System.Collections.Generic.List`1" />. The collection itself cannot be null, but it can contain
		///    elements that are null, if type <typeparamref name="TValue" /> is a reference type.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		///    <paramref name="items" /> is null.
		/// </exception>
		public void AddRange(IEnumerable<TValue> items)
		{
			_innerList.AddRange(items);
		}

		/// <summary>
		///    Sorts the elements in the entire <see cref="T:System.Collections.Generic.List`1" /> using the default comparer.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		///    The default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default" /> cannot find an implementation of the
		///    <see cref="T:System.IComparable`1" /> generic interface or the <see cref="T:System.IComparable" /> interface for type
		///    <typeparamref name="TValue" />.
		/// </exception>
		public void Sort()
		{
			_innerList.Sort();
		}

		/// <summary>
		///    Sorts the elements in the entire <see cref="T:System.Collections.Generic.List`1" /> using the specified
		///    <see cref="T:System.Comparison`1" />.
		/// </summary>
		/// <param name="comparison">
		///    The <see cref="T:System.Comparison`1" /> to use when comparing elements.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		///    <paramref name="comparison" /> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		///    The implementation of <paramref name="comparison" /> caused an error during the sort. For example,
		///    <paramref name="comparison" /> might not return 0 when comparing an item with itself.
		/// </exception>
		public void Sort(Comparison<TValue> comparison)
		{
			_innerList.Sort(comparison);
		}

		/// <summary>
		///    Sorts the elements in the entire <see cref="T:System.Collections.Generic.List`1" /> using the specified comparer.
		/// </summary>
		/// <param name="comparer">
		///    The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing elements, or null to use the default comparer
		///    <see cref="P:System.Collections.Generic.Comparer`1.Default" />.
		/// </param>
		/// <exception cref="T:System.InvalidOperationException">
		///    <paramref name="comparer" /> is null, and the default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default" />
		///    cannot find implementation of the <see cref="T:System.IComparable`1" />
		///    generic interface or the <see cref="T:System.IComparable" /> interface for type <typeparamref name="TValue" />.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		///    The implementation of <paramref name="comparer" /> caused an error during the sort. For example,
		///    <paramref name="comparer" /> might not return 0 when comparing an item with itself.
		/// </exception>
		public void Sort(IComparer<TValue> comparer)
		{
			_innerList.Sort(comparer);
		}

		/// <summary>
		///    Sorts the elements in a range of elements in <see cref="T:System.Collections.Generic.List`1" /> using the specified comparer.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range to sort.</param>
		/// <param name="count">The length of the range to sort.</param>
		/// <param name="comparer">
		///    The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing elements, or null to use the default comparer
		///    <see cref="P:System.Collections.Generic.Comparer`1.Default" />.
		/// </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///    <paramref name="index" /> is less than 0.-or-<paramref name="count" /> is less than 0.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		///    <paramref name="index" /> and <paramref name="count" /> do not specify a valid range in the
		///    <see cref="T:System.Collections.Generic.List`1" />
		///    .-or-The implementation of <paramref name="comparer" /> caused an error during the sort. For example,
		///    <paramref name="comparer" /> might not return 0 when comparing an item with itself.
		/// </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///    <paramref name="comparer" /> is null, and the default comparer
		///    <see cref="P:System.Collections.Generic.Comparer`1.Default" />
		///    cannot find implementation of the <see cref="T:System.IComparable`1" />
		///    generic interface or the <see cref="T:System.IComparable" /> interface for type
		///    <typeparamref name="TValue" />.
		/// </exception>
		public void Sort(int index, int count, IComparer<TValue> comparer)
		{
			_innerList.Sort(index, count, comparer);
		}

		/// <summary>
		///    Converts the values in the list to a JSON array.
		/// </summary>
		/// <param name="convert">The convert method to use on each item.</param>
		protected JArray ConvertToJsonArray(Func<TValue, JToken> convert)
		{
			var filterArray = new JArray();
			foreach (TValue value in this)
			{
				filterArray.Add(convert(value));
			}
			return filterArray;
		}
	}
}