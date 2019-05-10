using Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsTest
{
    [TestFixture]
    public class MyLinkedListTests
    {
        #region fields

        //min=3
        private const int _listCount = 10;
        private MyLinkedList<string> _list;
        private MyLinkedList<string> _emptyList;

        #endregion

        #region helpers

        [SetUp]
        public void Init()
        {
            _list = new MyLinkedList<string>();
            for (int i = 0; i < _listCount; i++)
            {
                _list.Add(new MyLinkedListNode<string>($"item{i + 1}"));
            }
            _emptyList = new MyLinkedList<string>();
        }

        private bool FilledListOkay
        {
            get
            {
                return TestList(_list);
            }
        }
        private bool EmptyListOkay
        {
            get
            {
                return TestList(_emptyList);
            }
        }

        private bool TestList(MyLinkedList<string> list)
        {
            
            try
            {
                if (list.Count == 0)
                {
                    if (list.First != null || list.Last != null)
                        return false;
                }
                else if (list.Count == 1)
                {
                    if (list.First != list[0] || list.Last != list[0])
                        return false;
                    if (list.First.Previous != null || list.First.Next != null)
                        return false;
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];

                        if (i > 0 && i < list.Count - 1)
                        {
                            if (item.Previous != list[i - 1])
                                return false;
                            if (item.Next != list[i + 1])
                                return false;
                        }
                        else if (i == 0)
                        {
                            if (item.Previous != null)
                                return false;

                            if (item.Next != list[i + 1])
                                return false;
                        }
                        else
                        {
                            if (item.Next != null)
                                return false;
                            if (item.Previous != list[i - 1])
                                return false;
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Index

        [Test]
        public void Index_Get_Delivers()
        {
            for (int i = 0; i < _listCount; i++)
            {
                var item = _list[i];
                Assert.AreEqual($"item{i+1}", item.Item);
            }
            Assert.True(FilledListOkay);
        }

        [TestCase(-1)]
        [TestCase(_listCount)]
        public void Index_Get_Fails(long index)
        {
            void act()
            {
                var t = _list[index];
            }
            Assert.Throws<IndexOutOfRangeException>(new TestDelegate(act));
            Assert.True(FilledListOkay);
        }

        [Test]
        public void Index_Get_Fails_OnEmpty()
        {
            void act()
            {
                var t = _emptyList[0];
            }
            Assert.Throws<IndexOutOfRangeException>(new TestDelegate(act));
            Assert.True(EmptyListOkay);
        }

        [Test]
        public void Index_Set_Sets()
        {
            for (int i = 0; i < _listCount; i++)
            {
                var item = new MyLinkedListNode<string>($"set{i + 1}");
                _list[i] = item;
            }
            for (int i = 0; i < _listCount; i++)
            {
                var itemValueExpected = $"set{i + 1}";
                Assert.AreEqual(itemValueExpected, _list[i].Item);

                if (i > 0)
                {
                    var prevValueExpected = $"set{i}";
                    Assert.AreEqual(prevValueExpected, _list[i -1].Item);
                }
                if (i < _listCount - 1)
                {
                    var nextValueExpected = $"set{i+2}";
                    Assert.AreEqual(nextValueExpected, _list[i + 1].Item);
                }
            }
            Assert.True(FilledListOkay);
        }

        [TestCase(-1)]
        [TestCase(_listCount)]
        public void Index_Set_Fails(long index)
        {
            void act()
            {
                _list[index] = new MyLinkedListNode<string>("hehe");
            }
            Assert.Throws<IndexOutOfRangeException>(new TestDelegate(act));
            Assert.True(FilledListOkay);
        }

        [Test]
        public void Index_Set_Fails_OnEmpty()
        {
            void act()
            {
                _emptyList[0] = new MyLinkedListNode<string>("hehe");
            }
            Assert.Throws<IndexOutOfRangeException>(new TestDelegate(act));
            Assert.True(EmptyListOkay);
        }

        #endregion

        #region Add/Insert

        [Test]
        public void Add_Adds()
        {
            Assert.AreEqual(_listCount, _list.Count);

            var itemToAdd = new MyLinkedListNode<string>("added");
            _list.Add(itemToAdd);

            Assert.AreEqual(itemToAdd, _list[_listCount]);
            Assert.AreEqual(_listCount + 1, _list.Count);


            Assert.True(FilledListOkay);
        }

        [Test]
        public void Add_Adds_OnEmpty()
        {
            var itemToAdd = new MyLinkedListNode<string>("added");
            _emptyList.Add(itemToAdd);

            Assert.AreEqual(itemToAdd, _emptyList[0]);
            Assert.AreEqual(1, _emptyList.Count);

            Assert.True(EmptyListOkay);
        }

        [Test]
        public void Add_AddsValue()
        {
            Assert.AreEqual(_listCount, _list.Count);

            var valueToAdd = "added";
            _list.Add(valueToAdd);

            Assert.AreEqual(valueToAdd, _list[_listCount].Item);
            Assert.AreEqual(_listCount + 1, _list.Count);


            Assert.True(FilledListOkay);
        }

        [Test]
        public void Add_AddsValue_OnEmpty()
        {
            var valueToAdd = "added";
            _emptyList.Add(valueToAdd);

            Assert.AreEqual(valueToAdd, _emptyList[0].Item);
            Assert.AreEqual(1, _emptyList.Count);

            Assert.True(EmptyListOkay);
        }

        [Test]
        public void Add_ReturnsAdded()
        {
            Assert.AreEqual(_listCount, _list.Count);

            var value = "returns";
            var returnItem = _list.Add(value);

            Assert.AreEqual(returnItem, _list[_listCount]);
            Assert.AreEqual(_listCount + 1, _list.Count);


            Assert.True(FilledListOkay);
        }


        [TestCase(0)]
        [TestCase(1)]
        [TestCase(_listCount)]
        public void Insert_Inserts(long index)
        {
            var itemToInsert = new MyLinkedListNode<string>("inserted");
            _list.InsertAt(index, itemToInsert);

            Assert.AreEqual(itemToInsert, _list[index]);
            Assert.AreEqual(_listCount + 1, _list.Count);

            Assert.True(FilledListOkay);
        }

        [Test]
        public void Insert_Inserts_First_OnEmpty()
        {
            var itemToInsert = new MyLinkedListNode<string>("inserted");
            _emptyList.InsertAt(0, itemToInsert);

            Assert.AreEqual(itemToInsert, _emptyList[0]);
            Assert.AreEqual(1, _emptyList.Count);

            Assert.True(EmptyListOkay);
        }

        [TestCase(-1)]
        [TestCase(_listCount + 1)]
        public void Insert_Fails(long index)
        {
            void act()
            {
                _list[index] = new MyLinkedListNode<string>("hehe");
            }
            Assert.Throws<IndexOutOfRangeException>(new TestDelegate(act));
            Assert.True(FilledListOkay);
        }

        #endregion

        #region AddBefore/After

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(_listCount - 1)]
        public void AddBefore_AddsBefore(long index)
        {
            var itemToAdd = new MyLinkedListNode<string>("added");
            _list.AddBefore(itemToAdd, _list[index]);

            Assert.AreEqual(itemToAdd, _list[index]);
            Assert.AreEqual(_listCount + 1, _list.Count);

            Assert.True(FilledListOkay);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(_listCount - 1)]
        public void AddBefore_WithValue_AddsBefore(long index)
        {
            var valueToAdd = "added";
            _list.AddBefore(valueToAdd, _list[index]);

            Assert.AreEqual(valueToAdd, _list[index].Item);
            Assert.AreEqual(_listCount + 1, _list.Count);

            Assert.True(FilledListOkay);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(_listCount - 1)]
        public void AddBefore_WithValue_ReturnsItem(long index)
        {
            var valueToAdd = "added";
            var item = _list.AddBefore(valueToAdd, _list[index]);

            Assert.AreEqual(valueToAdd, item.Item);
            Assert.AreEqual(_listCount + 1, _list.Count);

            Assert.True(FilledListOkay);
        }

        [Test]
        public void AddBefore_FailsOnNull()
        {
            void act()
            {
                MyLinkedListNode<string> relItem = null;
                _list.AddBefore(new MyLinkedListNode<string>("toAdd"), relItem);
            }
            Assert.Throws<ArgumentNullException>(new TestDelegate(act));
            Assert.True(FilledListOkay);
        }

        [Test]
        public void AddBefore_FailsOnUnknown()
        {
            void act()
            {
                MyLinkedListNode<string> relItem = new MyLinkedListNode<string>("relItem");
                _list.AddBefore(new MyLinkedListNode<string>("toAdd"), relItem);
            }
            Assert.Throws<ArgumentOutOfRangeException>(new TestDelegate(act));
            Assert.True(FilledListOkay);
        }

        [Test]
        public void AddBefore_FailsOnNull_OnEmpty()
        {
            void act()
            {
                MyLinkedListNode<string> relItem = null;
                _emptyList.AddBefore(new MyLinkedListNode<string>("toAdd"), relItem);
            }
            Assert.Throws<ArgumentNullException>(new TestDelegate(act));
            Assert.True(EmptyListOkay);
        }

        [Test]
        public void AddBefore_FailsOnUnknown_OnEmpty()
        {
            void act()
            {
                MyLinkedListNode<string> relItem = new MyLinkedListNode<string>("relItem");
                _emptyList.AddBefore(new MyLinkedListNode<string>("toAdd"), relItem);
            }
            Assert.Throws<ArgumentOutOfRangeException>(new TestDelegate(act));
            Assert.True(EmptyListOkay);
        }


        [TestCase(0)]
        [TestCase(1)]
        [TestCase(_listCount - 1)]
        public void AddAfter_AddsAfter(long index)
        {
            var itemToAdd = new MyLinkedListNode<string>("added");
            _list.AddAfter(itemToAdd, _list[index]);

            Assert.AreEqual(itemToAdd, _list[index +1]);
            Assert.AreEqual(_listCount + 1, _list.Count);

            Assert.True(FilledListOkay);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(_listCount - 1)]
        public void AddAfter_WithValue_AddsAfter(long index)
        {
            var valueToAdd = "added";
            _list.AddAfter(valueToAdd, _list[index]);

            Assert.AreEqual(valueToAdd, _list[index + 1].Item);
            Assert.AreEqual(_listCount + 1, _list.Count);

            Assert.True(FilledListOkay);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(_listCount - 1)]
        public void AddAfter_WithValue_ReturnsItem(long index)
        {
            var valueToAdd = "added";
            var item = _list.AddAfter(valueToAdd, _list[index]);

            Assert.AreEqual(valueToAdd, item.Item);
            Assert.AreEqual(_listCount + 1, _list.Count);

            Assert.True(FilledListOkay);
        }


        [Test]
        public void AddAfter_FailsOnNull()
        {
            void act()
            {
                _list.AddAfter(new MyLinkedListNode<string>("toAdd"), null);
            }
            Assert.Throws<ArgumentNullException>(new TestDelegate(act));
            Assert.True(FilledListOkay);
        }

        [Test]
        public void AddAfter_FailsOnUnknown()
        {
            void act()
            {
                MyLinkedListNode<string> relItem = new MyLinkedListNode<string>("relItem");
                _list.AddAfter(new MyLinkedListNode<string>("toAdd"), relItem);
            }
            Assert.Throws<ArgumentOutOfRangeException>(new TestDelegate(act));
            Assert.True(FilledListOkay);
        }

        [Test]
        public void AddAfter_FailsOnNull_OnEmpty()
        {
            void act()
            {
                MyLinkedListNode<string> relItem = null;
                _emptyList.AddAfter(new MyLinkedListNode<string>("toAdd"), relItem);
            }
            Assert.Throws<ArgumentNullException>(new TestDelegate(act));
            Assert.True(EmptyListOkay);
        }

        [Test]
        public void AddAfter_FailsOnUnknown_OnEmpty()
        {
            void act()
            {
                MyLinkedListNode<string> relItem = new MyLinkedListNode<string>("relItem");
                _emptyList.AddAfter(new MyLinkedListNode<string>("toAdd"), relItem);
            }
            Assert.Throws<ArgumentOutOfRangeException>(new TestDelegate(act));
            Assert.True(EmptyListOkay);
        }

        #endregion

        #region Remove/RemoveAt

        [TestCase(0, null, "item2")]
        [TestCase(1, "item1", "item3")]
        [TestCase(9, "item9", null)]
        public void Remove_Removes(long index, string beforeValue, string afterValue)
        {
            var toRemove = _list[index];
            _list.Remove(toRemove);

            if (index > 0)
            {
                Assert.AreEqual(beforeValue, _list[index - 1].Item);
            }
            if (index < _listCount - 1)
            {
                Assert.AreEqual(afterValue, _list[index].Item);
            }
            Assert.AreEqual(_listCount - 1, _list.Count);
            
            Assert.IsNull(toRemove.Next);
            Assert.IsNull(toRemove.Previous);

            Assert.True(FilledListOkay);
        }

        [Test]
        public void Remove_NoException()
        {
            _list.Remove(new MyLinkedListNode<string>("notThere"));
            Assert.True(FilledListOkay);
        }

        [Test]
        public void Remove_NoException_OnNull()
        {
            _list.Remove(null);
            Assert.True(FilledListOkay);
        }

        [Test]
        public void Remove_NoException_OnEmpty()
        {
            _emptyList.Remove(new MyLinkedListNode<string>("notThere"));
            Assert.True(EmptyListOkay);
        }



        [TestCase(0, null, "item2")]
        [TestCase(1, "item1", "item3")]
        [TestCase(9, "item9", null)]
        public void RemoveAt_Removes(long index, string beforeValue, string afterValue)
        {
            var toRemove = _list[index];
            _list.RemoveAt(index);

            if (index > 0)
            {
                Assert.AreEqual(beforeValue, _list[index - 1].Item);
            }
            if (index < _listCount - 1)
            {
                Assert.AreEqual(afterValue, _list[index].Item);
            }
            Assert.AreEqual(_listCount - 1, _list.Count);

            Assert.IsNull(toRemove.Previous);
            Assert.IsNull(toRemove.Next);

            Assert.True(FilledListOkay);
        }
        
        [TestCase(-1)]
        [TestCase(_listCount)]
        public void RemoveAt_Fails(long index)
        {
            void act()
            {
                _list.RemoveAt(index);
            }
            Assert.Throws<IndexOutOfRangeException>(new TestDelegate(act));

            Assert.True(FilledListOkay);
        }

        [Test]
        public void RemoveAt_Fails_OnEmpty()
        {
            void act()
            {
                _emptyList.RemoveAt(0);
            }
            Assert.Throws<IndexOutOfRangeException>(new TestDelegate(act));

            Assert.True(EmptyListOkay);
        }

        [Test]
        public void Clear_Clears()
        {
            _list.Clear();

            Assert.AreEqual(0, _list.Count);

            Assert.IsTrue(FilledListOkay);
        }

        [Test]
        public void Clear_Clears_OnEmpty()
        {
            _emptyList.Clear();

            Assert.AreEqual(0, _emptyList.Count);

            Assert.IsTrue(EmptyListOkay);
        }

        #endregion

        #region Finding

        [TestCase("item1", true)]
        [TestCase("item5", true)]
        [TestCase("item10", true)]
        [TestCase("notThere", false)]
        public void Contains_Finds(string value, bool isIn)
        {
            var result = _list.Contains(value);

            Assert.AreEqual(isIn, result);

            Assert.IsTrue(FilledListOkay);
        }

        [Test]
        public void Contains_NotFinds_OnEmpty()
        {
            var result = _emptyList.Contains("item1");

            Assert.AreEqual(false, result);

            Assert.IsTrue(FilledListOkay);
        }

        [TestCase(-1, false)]
        [TestCase(0, true)]
        [TestCase(5, true)]
        [TestCase(9, true)]
        [TestCase(25, false)]
        public void Contains_FindsItem(long index, bool isIn)
        {
            MyLinkedListNode<string> item = null;
            if (index >= 0 && index < _listCount)
            {
                item = _list[index];
            }
            else if (index >= _listCount)
            {
                item = new MyLinkedListNode<string>("notThere");
            }
            var result = _list.Contains(item);

            Assert.AreEqual(isIn, result);

            Assert.IsTrue(FilledListOkay);
        }




        #endregion
    }
}
