using NUnit.Framework;
using JSONUtil;
using System.Collections.Generic;
using System.Text;

namespace JSONUtil_specification
{

    [TestFixture]
    public class JSONParser_Object_Positive_Tests
    {
        [Test]
        public void OBJ01_empty_object_creation_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{}");
            Assert.AreEqual("{}", jObj.ToJsonText());
        }
        
        [Test]
        public void OBJ02_object_of_single_simple_stringvalue_works()
        {
            JSONParser jsParser = new JSONParser();            
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"Name\": \"Einstein\"}");            
            Assert.AreEqual("{\"Name\": \"Einstein\"}", jObj.ToJsonText());
        }
        [Test]
        public void OBJ03_object_of_multiple_simple_stringvalues_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"LName\": \"Einstein\", \"Fname\": \"Albert\"}");
            Assert.AreEqual("{\"LName\": \"Einstein\", \"Fname\": \"Albert\"}", jObj.ToJsonText());
        }

        [Test]
        public void OBJ04_object_of_single_simple_intvalue_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"Age\": 20}");
            Assert.AreEqual("{\"Age\": 20}", jObj.ToJsonText());
        }

        [Test]
        public void OBJ05_object_of_multiple_simple_intvalues_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"Age\": 20, \"Id\": 100}");
            Assert.AreEqual("{\"Age\": 20, \"Id\": 100}", jObj.ToJsonText());
        }

        [Test]
        public void OBJ05_object_of_floatvalues_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"Age\": 20.25, \"Wt\": 70.77}");
            Assert.AreEqual("{\"Age\": 20.25, \"Wt\": 70.77}", jObj.ToJsonText());
        }

        [Test]
        public void OBJ06_object_with_mix_of_simple_int_and_string_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"Name\": \"Easwari\", \"Id\": 100}");
            Assert.AreEqual("{\"Name\": \"Easwari\", \"Id\": 100}", jObj.ToJsonText());
        }

        [Test]
        public void OBJ07_object_of_bool_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"Good\":True, \"Bad\": False}");
            Assert.AreEqual("{\"Good\": True, \"Bad\": False}", jObj.ToJsonText());
        }

        [Test]
        public void OBJ08_object_of_heterogeneous_primitives_order1_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"Name\":\"Easwari\", \"Bad\": False, \"Age\": 17}");
            Assert.AreEqual("{\"Name\": \"Easwari\", \"Bad\": False, \"Age\": 17}", jObj.ToJsonText());
        }

        [Test]
        public void OBJ09_object_of_heterogeneous_primitives_order2_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"Bad\": False, \"Name\":\"Easwari\", \"Age\": 17}");
            Assert.AreEqual("{\"Bad\": False, \"Name\": \"Easwari\", \"Age\": 17}", jObj.ToJsonText());
        }

        [Test]
        public void OBJ10_object_of_heterogeneous_primitives_order3_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"Age\": 17, \"Name\":\"Easwari\", \"Bad\": False}");
            Assert.AreEqual("{\"Age\": 17, \"Name\": \"Easwari\", \"Bad\": False}", jObj.ToJsonText());
        }

        [Test]
        public void OBJ11_nested_object_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"Age\": 17, \"Name\": \"Easwari\", \"Bad\": False, \"dob\": {\"d\": 10, \"m\": 4}}");
            Assert.AreEqual("{\"Age\": 17, \"Name\": \"Easwari\", \"Bad\": False, \"dob\": {\"d\": 10, \"m\": 4}}", jObj.ToJsonText());
        }

        [Test]
        public void OBJ12_two_level_nested_object_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"Age\": 17, \"Name\":\"Easwari\", \"Bad\": False, \"dob\":{\"d\": 10, \"y\": {\"a\": \"x\"}, \"m\": 4}}");
            Assert.AreEqual("{\"Age\": 17, \"Name\": \"Easwari\", \"Bad\": False, \"dob\": {\"d\": 10, \"y\": {\"a\": \"x\"}, \"m\": 4}}", jObj.ToJsonText());
        }

        [Test]
        public void OBJ13_object_of_nullvalue_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"Name\": \"Easwari\", \"Id\": null}");
            Assert.AreEqual("{\"Name\": \"Easwari\", \"Id\": null}", jObj.ToJsonText());
        }

        [Test]
        public void OBJ14_object_of_nullvalue_order2_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"Name\": \"Easwari\", \"Id\": null, \"IId\": null }");
            Assert.AreEqual("{\"Name\": \"Easwari\", \"Id\": null, \"IId\": null}", jObj.ToJsonText());
        }
    }

    public class JSONParser_Object_Negative_Tests
    {
        [Test, ExpectedException]
        public void leftcurly_followedby_another_leftcurly_throws_exception()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{{");
        }

        [Test, ExpectedException]
        public void text_starting_with_rightcurly_throws_exception()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("}");
        }
        
        [Test, ExpectedException]
        public void object_with_more_than_one_propname_throws_exception()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"x\":\"y\":\"z\"}");
        }

        [Test, ExpectedException]        
        public void object_with_incomplete_property_throws_exception()
        {
            JSONParser jsParser = new JSONParser();
            JSONObject jObj = (JSONObject)jsParser.FromJsonText("{\"x\":\"y\", \"z\"");
        }
    }

    public class JSONParser_Array_Positive_Tests
    {
        [Test]
        public void ARR01_empty_array_creation_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[]");
        }

        [Test]
        public void ARR02_array_of_ints_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[1, 2, 3]");
            Assert.AreEqual("[1, 2, 3]", jArr.ToJsonText());
        }

        [Test]
        public void ARR03_array_of_floats_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[1.2, 2.3, 3.4]");
            Assert.AreEqual("[1.2, 2.3, 3.4]", jArr.ToJsonText());
        }

        [Test]
        public void ARR04_array_of_bools_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[true, false, true]");
            Assert.AreEqual("[True, False, True]", jArr.ToJsonText());
        }

        [Test]
        public void ARR05_array_of_null_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[null, null]");
            Assert.AreEqual("[null, null]", jArr.ToJsonText());
        }

        [Test]
        public void ARR06_array_of_strings_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[\"name\", \"age\"]");
            Assert.AreEqual("[\"name\", \"age\"]", jArr.ToJsonText());
        }

        [Test]
        public void ARR07_array_of_heterogeneous_primitives_order1_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[null, \"name\", 10, True]");
            Assert.AreEqual("[null, \"name\", 10, True]", jArr.ToJsonText());
        }

        [Test]
        public void ARR08_array_of_heterogeneous_primitives_order2_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[\"name\", null, 10, True]");
            Assert.AreEqual("[\"name\", null, 10, True]", jArr.ToJsonText());
        }

        [Test]
        public void ARR09_array_of_heterogeneous_primitives_order3_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[5, null, \"name\", 10, True]");
            Assert.AreEqual("[5, null, \"name\", 10, True]", jArr.ToJsonText());
        }

        [Test]
        public void ARR10_array_of_heterogeneous_primitives_order4_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[False, null, \"name\", 10, True]");
            Assert.AreEqual("[False, null, \"name\", 10, True]", jArr.ToJsonText());
        }

        [Test]
        public void ARR11_array_of_heterogeneous_primitives_order5_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[False, null, \"name\", 10, True]");
            Assert.AreEqual("[False, null, \"name\", 10, True]", jArr.ToJsonText());
        }

        [Test]
        public void ARR12_heterogenepous_array_with_objects_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[False, null, {\"x\": 1}, \"name\", 10, True]");
            Assert.AreEqual("[False, null, {\"x\": 1}, \"name\", 10, True]", jArr.ToJsonText());
        }

        [Test]
        public void ARR13_array_of_all_objects_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[{\"x\": 1}, {\"y\": True}, {\"z\": null}, {\"a\": \"haha\"}]");
            Assert.AreEqual("[{\"x\": 1}, {\"y\": True}, {\"z\": null}, {\"a\": \"haha\"}]", jArr.ToJsonText());
        }

        [Test]
        public void ARR14_array_of_arrays_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[10, [{\"x\": 1}, {\"y\": True}, {\"z\": null}, {\"a\": \"haha\"}]]");
            Assert.AreEqual("[10, [{\"x\": 1}, {\"y\": True}, {\"z\": null}, {\"a\": \"haha\"}]]", jArr.ToJsonText());
        }

        [Test]
        public void ARR15_array_as_property_value_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[10, {\"xy\": [{\"x\": 1}, {\"y\": True}, {\"z\": null}, {\"a\": \"abcd\"}]}]");            
            Assert.AreEqual("[10, {\"xy\": [{\"x\": 1}, {\"y\": True}, {\"z\": null}, {\"a\": \"abcd\"}]}]", jArr.ToJsonText());
        }

        [Test]
        public void ARR16_nested_array_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[10, [20, 30, [40, [50, 60]]]]");
            Assert.AreEqual("[10, [20, 30, [40, [50, 60]]]]", jArr.ToJsonText());
        }
    }

    public class JSONParser_Array_Negative_Tests
    {        
        [Test, ExpectedException]
        public void values_not_seperated_by_comma_throws_exception()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[1 2 3]");
            Assert.AreEqual("[1 2 3]", jArr.ToJsonText());
        }
        [Test, ExpectedException]
        public void incomplete_array_throws_exception()
        {
            JSONParser jsParser = new JSONParser();
            JSONArray jArr = (JSONArray)jsParser.FromJsonText("[[1, 2, 3]");
            Assert.AreEqual("[[1, 2, 3]", jArr.ToJsonText());
        }
    }

    public class JSONParser_Empty_Container_Positive_Tests
    {
        [Test]
        public void empty_container_of_int_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONEmptyContainer jEmpty = (JSONEmptyContainer)jsParser.FromJsonText("10");
            Assert.AreEqual("10", jEmpty.ToJsonText());

        }
        [Test]
        public void empty_container_of_float_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONEmptyContainer jEmpty = (JSONEmptyContainer)jsParser.FromJsonText("10.55");
            Assert.AreEqual("10.55", jEmpty.ToJsonText());

        }
        [Test]
        public void empty_container_of_bool_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONEmptyContainer jEmpty = (JSONEmptyContainer)jsParser.FromJsonText("True");
            Assert.AreEqual("True", jEmpty.ToJsonText());

        }
        [Test]
        public void empty_container_of_null_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONEmptyContainer jEmpty = (JSONEmptyContainer)jsParser.FromJsonText("null");
            Assert.AreEqual("null", jEmpty.ToJsonText());
        }

        [Test]
        public void empty_container_of_string_works()
        {
            JSONParser jsParser = new JSONParser();
            JSONEmptyContainer jEmpty = (JSONEmptyContainer)jsParser.FromJsonText("\"null\"");
            Assert.AreEqual("\"null\"", jEmpty.ToJsonText());
        }
    }

    public class JSONParser_Empty_Container_Negative_Tests
    {
        [Test, ExpectedException]
        public void Empty_Container_multiple_values_throws_exception()
        {
            JSONParser jsParser = new JSONParser();
            JSONEmptyContainer jEmpty = (JSONEmptyContainer)jsParser.FromJsonText("10,20");
            Assert.AreEqual("10,20", jEmpty.ToJsonText());

        }
    }
}